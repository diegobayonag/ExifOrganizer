﻿//
// MetaParser.cs: Static class to parse meta data from different filetypes.
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExifOrganizer.Meta.Parsers;

namespace ExifOrganizer.Meta
{
    public static class MetaParser
    {
        public static IEnumerable<MetaData> Parse(string path, bool recursive, IEnumerable<string> ignorePaths = null)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (Directory.Exists(path))
                return ParseDirectory(path, recursive, ignorePaths);
            else if (File.Exists(path))
                return new MetaData[] { ParseFile(path) };
            else
                throw new FileNotFoundException(String.Format("Could not find file or directory: {0}", path));
        }

        public static MetaData ParseFile(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (!File.Exists(path))
                throw new MetaParseException("File not found: {0}", path);

            string extension = Path.GetExtension(path).ToLower();
            switch (extension)
            {
                // Images (Exif)
                case ".jpg":
                case ".jpeg":
                case ".tif":
                case ".tiff":
                    return ExifParser.Parse(path);

                // Images (generic)
                case ".png":
                case ".gif":
                case ".bmp":
                    return GenericFileParser.Parse(path, MetaType.Image);

                // Music (generic)
                case ".mp3": // TODO: id3
                case ".wav": // TODO: exif
                case ".flac":
                case ".aac":
                    return GenericFileParser.Parse(path, MetaType.Music);

                // Movies (generic)
                case ".mpg":
                case ".mpeg":
                case ".mov":
                case ".mp4":
                    return GenericFileParser.Parse(path, MetaType.Video);

                default:
                    return GenericFileParser.Parse(path, MetaType.File);
            }
        }

        public static IEnumerable<MetaData> ParseDirectory(string path, bool recursive, IEnumerable<string> ignorePaths = null)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (!Directory.Exists(path))
                throw new MetaParseException("Directory not found: {0}", path);

            List<MetaData> list = new List<MetaData>();
            if (ignorePaths != null)
            {
                foreach (string ignorePath in ignorePaths)
                {
                    if (ignorePath.DirectoryIsSubPath(path, true))
                        return list;
                }
            }

            list.Add(DirectoryParser.Parse(path));

            foreach (string file in Directory.GetFiles(path))
            {
                MetaData meta;
                try
                {
                    meta = ParseFile(file);
                }
                catch (Exception)
                {
                    continue;
                }

                list.Add(meta);
            }

            if (recursive)
            {
                foreach (string directory in Directory.GetDirectories(path))
                    list.AddRange(ParseDirectory(directory, recursive, ignorePaths));
            }

            return list;
        }
    }
}
