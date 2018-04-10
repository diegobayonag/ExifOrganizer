﻿//
// Main.cs: Graphical user interface (GUI) main window.
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

using ExifOrganizer.Organizer;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ExifOrganizer.UI
{
    public partial class Main : Form
    {
        private MediaOrganizer organizer = new MediaOrganizer();

        public Main()
        {
            InitializeComponent();

            organizer.LoadConfig();
            organizer.OnProgress += ReportProgress;
        }

        #region Control events

        private void Main_Load(object sender, EventArgs e)
        {
            infoVersion.Text = $"Version: {new Version(Application.ProductVersion).Get()}";

            sourcePath.Text = organizer.sourcePath;
            destinationPath.Text = organizer.destinationPath;
            recursive.Checked = organizer.Recursive;

            // FileComparator enum
            fileComparator.EnumText = value =>
            {
                switch ((FileComparator)value)
                {
                    case FileComparator.FileSize: return "File size";
                    case FileComparator.ChecksumMD5: return "MD5";
                    case FileComparator.ChecksumSHA1: return "SHA1";
                    case FileComparator.ChecksumSHA256: return "SHA256";
                    default: return value.ToString();
                }
            };
            fileComparator.EnumType = typeof(FileComparator);
            fileComparator.EnumValue = organizer.FileComparator;

            // CopyPrecondition enum
            copyPrecondition.EnumText = value =>
            {
                switch ((CopyPrecondition)value)
                {
                    case CopyPrecondition.RequireEmpty: return "Require empty";
                    case CopyPrecondition.WipeBefore: return "Wipe before";
                    default: return value.ToString();
                }
            };
            copyPrecondition.EnumType = typeof(CopyPrecondition);
            copyPrecondition.EnumValue = organizer.CopyPrecondition;

            // CopyMode enum
            copyMode.EnumText = value =>
            {
                switch ((CopyMode)value)
                {
                    case CopyMode.KeepExisting: return "Keep existing";
                    case CopyMode.KeepUnique: return "Keep unique";
                    case CopyMode.OverwriteExisting: return "Overwrite existing";
                    default: return value.ToString();
                }
            };
            copyMode.EnumType = typeof(CopyMode);
            copyMode.EnumValue = organizer.CopyMode;

            verifyFiles.Checked = organizer.VerifyFiles;

            // CultureInfo localization
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
                localization.Items.Add(culture);
            localization.SelectedItem = organizer.Locale;

            // Patterns
            patternImage.Text = organizer.DestinationPatternImage;
            patternAudio.Text = organizer.DestinationPatternAudio;
            patternVideo.Text = organizer.DestinationPatternVideo;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!organizer.IsRunning || organizer.IsAborted)
                return;

            if (MessageBox.Show(this, "Organization currently in progress, it must be aborted before the application can be closed.", "Abort current progress?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            organizer.Abort();
        }

        private async void organize_Click(object sender, EventArgs e)
        {
            if (organizer.IsRunning)
            {
                AbortProgress();
                return;
            }

            organizer.sourcePath = sourcePath.SelectedPath;
            organizer.destinationPath = destinationPath.SelectedPath;
            organizer.Recursive = recursive.Checked;
            organizer.FileComparator = (FileComparator)fileComparator.EnumValue;
            organizer.CopyPrecondition = (CopyPrecondition)copyPrecondition.SelectedItem;
            organizer.CopyMode = (CopyMode)copyMode.SelectedItem;
            organizer.VerifyFiles = verifyFiles.Checked;
            organizer.DestinationPatternImage = patternImage.Text;
            organizer.DestinationPatternVideo = patternVideo.Text;
            organizer.DestinationPatternAudio = patternAudio.Text;
            organizer.Locale = (CultureInfo)localization.SelectedItem;
            organizer.SaveConfig();

            ProgressStarted();

            try
            {
                OrganizeSummary summary = await organizer.ParseAsync();
                ParseComplete(organizer, summary);
            }
            catch (Exception ex)
            {
                ParseException(ex);
            }
        }

        #endregion Control events

        private void AbortProgress()
        {
            organizer.Abort();
        }

        private void ReportProgress(MediaOrganizer organizer, double value, string message)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => ReportProgress(organizer, value, message));
                return;
            }

            double percent = value * 100.0;
            if (String.IsNullOrEmpty(message))
                message = $"{Math.Round(percent, 1)}%";
            else
                message = $"{Math.Round(percent, 1)}% - {message}";

            progress.SetProgress(value, message);
        }

        private void ProgressStarted()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => ProgressStarted());
                return;
            }

            organize.Text = "Abort";
            progress.Value = progress.Minimum;
            progress.Visible = true;
        }

        private void ProgressEnded()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => ProgressEnded());
                return;
            }

            organize.Text = "Organize";
            progress.Visible = false;
        }

        #region Parse

        private async void ParseComplete(MediaOrganizer organizer, OrganizeSummary summary)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => ParseComplete(organizer, summary));
                return;
            }

            if (MessageBox.Show(summary.ToString(), "Continue organization?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                ProgressEnded();
                return;
            }

            try
            {
                await organizer.OrganizeAsync();
                OrganizeComplete(organizer);
            }
            catch (Exception ex)
            {
                OrganizeException(ex);
            }
        }

        private void ParseException(Exception ex)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => ParseException(ex));
                return;
            }

            Exception innerException = ex.GetInnerMost();
            MessageBox.Show(innerException.Message, "Media parse failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ProgressEnded();
        }

        #endregion Parse

        #region Organize

        private void OrganizeComplete(MediaOrganizer organizer)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => OrganizeComplete(organizer));
                return;
            }

            MessageBox.Show("Media organization completed successfully", "Media organization done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ProgressEnded();
        }

        private void OrganizeException(Exception ex)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => OrganizeException(ex));
                return;
            }

            Exception innerException = ex.GetInnerMost();
            MessageBox.Show(innerException.Message, "Media organization failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ProgressEnded();
        }

        #endregion Organize
    }
}