﻿//
// MediaOrganizer.cs: Helper class to organize media files.
//
// Copyright (C) 2014 Rikard Johansson
//
// This program is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// this program. If not, see http://www.gnu.org/licenses/.
//

using ExifOrganizer.Meta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ExifOrganizer.Organizer
{
    public enum CopyPrecondition
    {
        None,
        RequireEmpty,
        WipeBefore
    }

    public enum CopyMode
    {
        KeepUnique,
        KeepExisting,
        OverwriteExisting,
    }

    [Flags]
    public enum FileComparator
    {
        None = 0x00,
        FileSize = 0x01,
        Created = 0x02,
        Modified = 0x04,
        ChecksumMD5 = 0x08,
        ChecksumSHA1 = 0x10,
        ChecksumSHA256 = 0x20,
        All = 0xFF
    }

    public enum ExceptionHandling
    {
        Throw,
        Ignore,
    }

    public class ParseSummary
    {
        public string[] parsed;
        public string[] ignored;
        public string[] totalFiles;
        public string[] totalDirectories;

        public override string ToString()
        {
            return String.Format("Summary {{ Parsed: {0} (Ignored: {1}), Total files: {2}, Total directories: {3} }}",
                parsed != null ? parsed.Length : 0,
                ignored != null ? ignored.Length : 0,
                totalFiles != null ? totalFiles.Length : 0,
                totalDirectories != null ? totalDirectories.Length : 0
            );
        }
    }

    public class OrganizeSummary
    {
        public HashSet<string> sources = new HashSet<string>();
        public HashSet<string> destinations = new HashSet<string>();
        public HashSet<string> modified = new HashSet<string>();
        public Dictionary<string, string> overwritten = new Dictionary<string, string>();
        public Dictionary<string, string> skipped = new Dictionary<string, string>();
        public Dictionary<string, string> duplicates = new Dictionary<string, string>();
        public Dictionary<string, string> renamed = new Dictionary<string, string>();

        public override string ToString()
        {
            return String.Format("Summary {{ Total files: {0}, Modified: {1}, Overwritten: {2}, Skipped: {3}, Duplicates: {4}, Renamed: {5} }}",
                sources.Count,
                modified.Count,
                overwritten.Count,
                skipped.Count,
                duplicates.Count,
                renamed.Count
            );
        }
    }

    public class MediaOrganizer
    {
        private const double PARSE_PROGRESS_FACTOR = 0.1;

        public event Action<MediaOrganizer, double, string> OnProgress = delegate { };

        public string sourcePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public string destinationPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public bool Recursive = true;
        public CultureInfo Locale = Thread.CurrentThread.CurrentCulture;
        public string DestinationPatternImage = @"%y/%m %M/%t/%n.%e";
        public string DestinationPatternVideo = @"%y/%m %M/Video/%t/%n.%e";
        public string DestinationPatternAudio = @"%y/%m %M/Audio/%t/%n.%e";
        public CopyPrecondition CopyPrecondition = CopyPrecondition.None;
        public FileComparator FileComparator = FileComparator.FileSize | FileComparator.ChecksumMD5;
        public CopyMode CopyMode = CopyMode.KeepUnique;
        public ExceptionHandling ExceptionHandling = ExceptionHandling.Throw;
        public bool VerifyFiles = true;
        public string[] IgnorePaths = null; // TODO: implement

        private CopyItems copyItems;
        private bool workerRunning;
        private bool workerAborted;

        public MediaOrganizer()
        {
        }

        private string IniConfigFileName
        {
            get
            {
                string executablePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string iniFilePath = Path.Combine(executablePath, "MediaOrganizer.ini");
                return iniFilePath;
            }
        }

        public void LoadConfig()
        {
            string iniFilePath = IniConfigFileName;

            if (!File.Exists(iniFilePath))
                SaveConfig(); // Generate default config file

            IniFile iniFile = new IniFile();
            iniFile.TryLoad(iniFilePath);

            if (iniFile.ContainsKey("sourcePath"))
                sourcePath = iniFile["sourcePath"];
            if (iniFile.ContainsKey("destinationPath"))
                destinationPath = iniFile["destinationPath"];
            if (iniFile.ContainsKey("recursive"))
                Recursive = iniFile["recursive"].ToBool();
            if (iniFile.ContainsKey("locale"))
                Locale = new CultureInfo(iniFile["locale"]);
            if (iniFile.ContainsKey("patternImage"))
                DestinationPatternImage = iniFile["patternImage"];
            if (iniFile.ContainsKey("patternAudio"))
                DestinationPatternAudio = iniFile["patternAudio"];
            if (iniFile.ContainsKey("patternVideo"))
                DestinationPatternVideo = iniFile["patternVideo"];
            if (iniFile.ContainsKey("precondition"))
                CopyPrecondition = iniFile["precondition"].ToEnum<CopyPrecondition>();
            if (iniFile.ContainsKey("comparator"))
                FileComparator = iniFile["comparator"].ToEnum<FileComparator>();
            if (iniFile.ContainsKey("copyMode"))
                CopyMode = iniFile["copyMode"].ToEnum<CopyMode>();
            if (iniFile.ContainsKey("exceptionHandling"))
                ExceptionHandling = iniFile["exceptionHandling"].ToEnum<ExceptionHandling>();
            if (iniFile.ContainsKey("verifyFiles"))
                VerifyFiles = iniFile["verifyFiles"].ToBool();
        }

        public void SaveConfig()
        {
            IniFile iniFile = new IniFile();
            iniFile["sourcePath"] = sourcePath;
            iniFile["destinationPath"] = destinationPath;
            iniFile["recursive"] = Recursive ? "1" : "0";
            iniFile["locale"] = Locale.Name;
            iniFile["patternImage"] = DestinationPatternImage;
            iniFile["patternAudio"] = DestinationPatternAudio;
            iniFile["patternVideo"] = DestinationPatternVideo;
            iniFile["precondition"] = CopyPrecondition.ToString();
            iniFile["comparator"] = FileComparator.ToString();
            iniFile["copyMode"] = CopyMode.ToString();
            iniFile["exceptionHandling"] = ExceptionHandling.ToString();
            iniFile["verifyFiles"] = VerifyFiles ? "1" : "0";

            string iniFilePath = IniConfigFileName;
            iniFile.TrySave(iniFilePath);
        }

        public bool IsRunning
        {
            get { return workerRunning; }
        }

        public bool IsAborted
        {
            get { return workerAborted; }
        }

        public void Abort()
        {
            if (!workerRunning)
                return;

            workerAborted = true;

            OnProgress(this, 1.0, "Aborted");
        }

        public ParseSummary Parse()
        {
            Task<ParseSummary> task = ParseAsync();
            task.ConfigureAwait(false); // Prevent deadlock of caller
            return task.Result;
        }

        public async Task<ParseSummary> ParseAsync()
        {
            if (sourcePath.DirectoryAreSame(destinationPath))
            {
                // TODO: implement
                throw new NotSupportedException("TODO");
            }

            if (workerRunning)
                throw new InvalidOperationException("Cannot start parsing: worker currently running");
            workerRunning = true;
            workerAborted = false;

            OnProgress(this, 0.0, "Parsing source");

            try
            {
                ParseSummary summary = new ParseSummary();

                copyItems = new CopyItems();
                copyItems.sourcePath = sourcePath;
                copyItems.destinationPath = destinationPath;
                copyItems.items = await ParseItemsAsync(sourcePath, destinationPath, summary);

                return summary;
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#else
                return null;
#endif
            }
            finally
            {
                workerRunning = false;

                OnProgress(this, PARSE_PROGRESS_FACTOR, "Parsing complete");
            }
        }

        public OrganizeSummary Organize()
        {
            Task<OrganizeSummary> task = OrganizeAsync();
            task.ConfigureAwait(false); // Prevent deadlock of caller
            return task.Result;
        }

        public async Task<OrganizeSummary> OrganizeAsync()
        {
            // TODO: solve in nicer manner
            if (copyItems == null)
                throw new InvalidOperationException("Parse() must be executed prior to Organize()");

            if (workerRunning)
                throw new InvalidOperationException("Cannot start parsing: worker currently running");
            workerRunning = true;
            workerAborted = false;

            OnProgress(this, PARSE_PROGRESS_FACTOR + 0.1, "Prepare destination");

            try
            {
                OrganizeSummary summary = new OrganizeSummary();

                await Task.Run(() => OrganizationThread(summary));

                return summary;
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#else
                return null;
#endif
            }
            finally
            {
                workerRunning = false;
                OnProgress(this, 1.0, "Organization complete");
            }
        }

        private void OrganizationThread(OrganizeSummary summary)
        {
            PrepareDestinationPath();

            int itemCount = copyItems.items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                if (workerAborted)
                    break;

                float progress = (float)(i) / (float)itemCount;
                if ((int)(progress * 10) % 2 == 0)
                    OnProgress(this, PARSE_PROGRESS_FACTOR + 0.1 + (progress * (1.0 - PARSE_PROGRESS_FACTOR - 0.1)), $"Organizing {i + 1} of {itemCount}");

                CopySourceToDestination(copyItems.items[i], summary);
            }
        }

        private void CopySourceToDestination(CopyItem item, OrganizeSummary summary)
        {
            string destinationDirectory = Path.GetDirectoryName(item.destinationPath);
            if (!Directory.Exists(destinationDirectory))
            {
                try
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                catch (Exception ex)
                {
                    if (ExceptionHandling == ExceptionHandling.Throw)
                        throw new MediaOrganizerException($"Failed to create directory: {destinationDirectory}", ex);

                    Trace.WriteLine($"[{nameof(MediaOrganizer)}] Ignored exception: {ex.Message}");
                    return;
                }
            }

            summary.sources.Add(item.sourcePath);
            summary.destinations.Add(item.destinationPath);

            bool overwrite = false;
            string targetDestinationPath = item.destinationPath; // Note: path may be altered later on to resolve conflicts
            bool fileExists = File.Exists(targetDestinationPath);

            switch (CopyMode)
            {
                case CopyMode.OverwriteExisting:
                    overwrite = true;
                    if (fileExists)
                    {
                        summary.overwritten[item.sourcePath] = targetDestinationPath;
                        Trace.WriteLine($"[{nameof(MediaOrganizer)}] Force overwrite file: {targetDestinationPath}");
                    }
                    break;

                case CopyMode.KeepExisting:
                    if (fileExists)
                    {
                        summary.skipped[item.sourcePath] = targetDestinationPath;
                        Trace.WriteLine($"[{nameof(MediaOrganizer)}] Keep existing file: {targetDestinationPath}");
                        return;
                    }
                    break;

                case CopyMode.KeepUnique:
                    FileInfo sourceInfo = item.sourceInfo;
                    FileInfo destinationInfo = new FileInfo(targetDestinationPath);
                    if (fileExists)
                    {
                        // Potentially slow, therefore previous optimizations
                        if (sourceInfo.AreFilesIdentical(destinationInfo, FileComparator))
                        {
                            summary.duplicates[item.sourcePath] = targetDestinationPath;
                            Trace.WriteLine($"[{nameof(MediaOrganizer)}] Duplicate file ignored: {item.sourcePath} (duplicate of {targetDestinationPath})");
                            return;
                        }
                    }

                    if (sourceInfo.FileExistsInDirectory(destinationInfo.Directory, FileComparator))
                    {
                        summary.duplicates[item.sourcePath] = destinationInfo.Directory.FullName;
                        Trace.WriteLine($"[{nameof(MediaOrganizer)}] Duplicate file ignored: {item.sourcePath} (exists in {destinationInfo.Directory})");
                        return; // Source file already exists in target directory
                    }

                    // Find next unused filename
                    string originalDestinationPath = targetDestinationPath;
                    int index = 1;
                    while (fileExists)
                    {
                        targetDestinationPath = destinationInfo.SuffixFileName(index++);
                        fileExists = File.Exists(targetDestinationPath);
                    }
                    if (targetDestinationPath != originalDestinationPath)
                    {
                        summary.renamed[originalDestinationPath] = targetDestinationPath;
                        Trace.WriteLine($"[{nameof(MediaOrganizer)}] Renamed to prevent conflict: {targetDestinationPath} (original: {originalDestinationPath})");
                    }
                    break;

                default:
                    throw new NotImplementedException($"CopyMode: {CopyMode}");
            }

            try
            {
                File.Copy(item.sourcePath, targetDestinationPath, overwrite);
                summary.modified.Add(targetDestinationPath);
                if (VerifyFiles)
                {
                    if (!item.sourceInfo.AreFilesIdentical(new FileInfo(targetDestinationPath), Organizer.FileComparator.ChecksumMD5))
                        throw new MediaOrganizerException("File verification failed. Source: {0}. Destination: {1}", item.sourcePath, targetDestinationPath);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHandling == ExceptionHandling.Throw)
                    throw new MediaOrganizerException($"Failed to copy file. Mode: {CopyMode}. Overwrite: {overwrite}. Source: {item.sourcePath}. Destination: {targetDestinationPath}", ex);

                Trace.WriteLine($"[{nameof(MediaOrganizer)}] Ignored exception: {ex.Message}");
            }
        }

        private void PrepareDestinationPath()
        {
            // Setup destination path
            if (!Directory.Exists(copyItems.destinationPath))
            {
                Directory.CreateDirectory(copyItems.destinationPath);
                return;
            }

            switch (CopyPrecondition)
            {
                case CopyPrecondition.None:
                    break;

                case CopyPrecondition.RequireEmpty:
                    if (Directory.Exists(copyItems.destinationPath))
                    {
                        if (Directory.GetFiles(copyItems.destinationPath).Length > 0)
                            throw new MediaOrganizerException("Path contains files but is required to be empty: {0}", copyItems.destinationPath);
                        if (Directory.GetDirectories(copyItems.destinationPath).Length > 0)
                            throw new MediaOrganizerException("Path contains directories but is required to be empty: {0}", copyItems.destinationPath);
                    }
                    break;

                case CopyPrecondition.WipeBefore:
                    if (copyItems.sourcePath.DirectoryAreSame(copyItems.destinationPath))
                    {
                        // TODO: handle exceptions
                        // Move source/destination path to temporary place before copying
                        string tempSourcePath = Path.GetTempFileName();
                        try
                        {
                            File.Delete(tempSourcePath);
                            Directory.Move(copyItems.sourcePath, tempSourcePath);
                        }
                        catch (Exception ex)
                        {
                            throw new MediaOrganizerException($"Failed to generate temporary directory: {tempSourcePath}", ex);
                        }
                        copyItems.sourcePath = tempSourcePath; // TODO: delete this path after organization
                    }
                    else
                    {
                        try
                        {
                            Directory.Delete(copyItems.destinationPath, true);
                        }
                        catch (Exception ex)
                        {
                            throw new MediaOrganizerException($"Failed to wipe destination path: {copyItems.destinationPath}", ex);
                        }
                    }

                    if (!Directory.Exists(copyItems.destinationPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(copyItems.destinationPath);
                        }
                        catch (Exception ex)
                        {
                            throw new MediaOrganizerException($"Failed to create destination path: {copyItems.destinationPath}", ex);
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException($"CopyPrecondition: {CopyPrecondition}");
            }
        }

        private async Task<List<CopyItem>> ParseItemsAsync(string sourcePath, string destinationPath, ParseSummary summary)
        {
            List<string> ignore = new List<string>();
            if (IgnorePaths != null)
                ignore.AddRange(IgnorePaths);
            if (!sourcePath.DirectoryAreSame(destinationPath))
                ignore.Add(destinationPath);

            IEnumerable<MetaData> data;
            try
            {
                MetaParserConfig config = new MetaParserConfig() { Recursive = Recursive, IgnorePaths = ignore };
                data = await MetaParser.ParseAsync(sourcePath, config);
            }
            catch (MetaParseException ex)
            {
                throw new MediaOrganizerException("Failed to parse meta data", ex);
            }

            PatternPathParser parser = new PatternPathParser(Locale);
            parser.Preload(data);

            HashSet<string> files = new HashSet<string>();
            HashSet<string> directories = new HashSet<string>();
            HashSet<string> valid = new HashSet<string>();
            HashSet<string> ignored = new HashSet<string>();
            List<CopyItem> items = new List<CopyItem>();
            foreach (MetaData meta in data)
            {
                if (workerAborted)
                    break;

                string path = meta.Path;

                switch (meta.Type)
                {
                    case MetaType.Directory:
                        directories.Add(path);
                        continue;
                    case MetaType.File:
                        files.Add(path);
                        ignored.Add(path);
                        continue;
                    default:
                        files.Add(path);
                        break;
                }

                CopyItem item = ParseItem(parser, destinationPath, meta);
                items.Add(item);
                valid.Add(path);
            }

            summary.totalDirectories = directories.ToArray();
            summary.totalFiles = files.ToArray();
            summary.parsed = valid.ToArray();
            summary.ignored = ignored.ToArray();

            return items;
        }

        private CopyItem ParseItem(PatternPathParser parser, string destinationPath, MetaData meta)
        {
            string sourcePath = meta.Path;

            CopyItem item = new CopyItem();
            item.sourceInfo = new FileInfo(sourcePath);
            item.sourcePath = sourcePath;
            item.destinationPath = CalculateDestinationPath(parser, destinationPath, meta);
            item.meta = meta.Data;
            return item;
        }

        private string CalculateDestinationPath(PatternPathParser parser, string destinationPath, MetaData meta)
        {
            string destinationPattern = null;
            switch (meta.Type)
            {
                case MetaType.Image:
                    destinationPattern = DestinationPatternImage;
                    break;

                case MetaType.Video:
                    destinationPattern = DestinationPatternVideo;
                    break;

                case MetaType.Music:
                    destinationPattern = DestinationPatternAudio;
                    break;

                default:
                    throw new NotSupportedException($"Meta media type not supported: {meta.Type}");
            }

            return parser.Parse(destinationPattern, destinationPath, meta);
        }
    }
}