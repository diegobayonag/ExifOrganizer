﻿using ExifOrganizer.UI.Controls;

namespace ExifOrganizer.UI
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.organize = new System.Windows.Forms.Button();
            this.source = new System.Windows.Forms.GroupBox();
            this.infoSourceDirectory = new System.Windows.Forms.Label();
            this.infoRecursive = new System.Windows.Forms.Label();
            this.recursive = new System.Windows.Forms.CheckBox();
            this.sourcePath = new ExifOrganizer.UI.Controls.FileBrowseControl();
            this.infoFileComparator = new System.Windows.Forms.Label();
            this.destination = new System.Windows.Forms.GroupBox();
            this.infoFileVerification = new System.Windows.Forms.Label();
            this.fileComparator = new ExifOrganizer.UI.Controls.EnumFlagsDropDown();
            this.infoCopyPrecondition = new System.Windows.Forms.Label();
            this.copyPrecondition = new ExifOrganizer.UI.Controls.EnumDropDown();
            this.infoDestinationDirectory = new System.Windows.Forms.Label();
            this.infoCopyMode = new System.Windows.Forms.Label();
            this.infoLocalization = new System.Windows.Forms.Label();
            this.infoPatternImage = new System.Windows.Forms.Label();
            this.infoPatternVideo = new System.Windows.Forms.Label();
            this.infoPatternAudio = new System.Windows.Forms.Label();
            this.patternAudio = new System.Windows.Forms.TextBox();
            this.patternVideo = new System.Windows.Forms.TextBox();
            this.patternImage = new System.Windows.Forms.TextBox();
            this.localization = new System.Windows.Forms.ComboBox();
            this.copyMode = new ExifOrganizer.UI.Controls.EnumDropDown();
            this.destinationPath = new ExifOrganizer.UI.Controls.FileBrowseControl();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.infoVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.progress = new ExifOrganizer.UI.Controls.ProgressBarText();
            this.fileVerification = new ExifOrganizer.UI.Controls.EnumFlagsDropDown();
            this.source.SuspendLayout();
            this.destination.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // organize
            // 
            this.organize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.organize.Location = new System.Drawing.Point(302, 363);
            this.organize.Name = "organize";
            this.organize.Size = new System.Drawing.Size(75, 23);
            this.organize.TabIndex = 13;
            this.organize.Text = "Organize";
            this.organize.UseVisualStyleBackColor = true;
            this.organize.Click += new System.EventHandler(this.organize_Click);
            // 
            // source
            // 
            this.source.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.source.Controls.Add(this.infoSourceDirectory);
            this.source.Controls.Add(this.infoRecursive);
            this.source.Controls.Add(this.recursive);
            this.source.Controls.Add(this.sourcePath);
            this.source.Location = new System.Drawing.Point(12, 12);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(365, 74);
            this.source.TabIndex = 0;
            this.source.TabStop = false;
            this.source.Text = "Source";
            // 
            // infoSourceDirectory
            // 
            this.infoSourceDirectory.AutoSize = true;
            this.infoSourceDirectory.Location = new System.Drawing.Point(6, 25);
            this.infoSourceDirectory.Name = "infoSourceDirectory";
            this.infoSourceDirectory.Size = new System.Drawing.Size(49, 13);
            this.infoSourceDirectory.TabIndex = 9;
            this.infoSourceDirectory.Text = "Directory";
            // 
            // infoRecursive
            // 
            this.infoRecursive.AutoSize = true;
            this.infoRecursive.Location = new System.Drawing.Point(6, 49);
            this.infoRecursive.Name = "infoRecursive";
            this.infoRecursive.Size = new System.Drawing.Size(55, 13);
            this.infoRecursive.TabIndex = 10;
            this.infoRecursive.Text = "Recursive";
            // 
            // recursive
            // 
            this.recursive.AutoSize = true;
            this.recursive.Location = new System.Drawing.Point(103, 48);
            this.recursive.Name = "recursive";
            this.recursive.Size = new System.Drawing.Size(15, 14);
            this.recursive.TabIndex = 2;
            this.recursive.UseVisualStyleBackColor = true;
            // 
            // sourcePath
            // 
            this.sourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourcePath.BrowseType = ExifOrganizer.UI.Controls.FileBrowseType.Directory;
            this.sourcePath.DialogTitle = "Source directory";
            this.sourcePath.Location = new System.Drawing.Point(103, 19);
            this.sourcePath.Name = "sourcePath";
            this.sourcePath.ReadOnly = false;
            this.sourcePath.SelectedPath = "";
            this.sourcePath.Size = new System.Drawing.Size(256, 23);
            this.sourcePath.TabIndex = 1;
            // 
            // infoFileComparator
            // 
            this.infoFileComparator.AutoSize = true;
            this.infoFileComparator.Location = new System.Drawing.Point(6, 105);
            this.infoFileComparator.Name = "infoFileComparator";
            this.infoFileComparator.Size = new System.Drawing.Size(79, 13);
            this.infoFileComparator.TabIndex = 12;
            this.infoFileComparator.Text = "File comparator";
            // 
            // destination
            // 
            this.destination.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destination.Controls.Add(this.fileVerification);
            this.destination.Controls.Add(this.infoFileVerification);
            this.destination.Controls.Add(this.infoFileComparator);
            this.destination.Controls.Add(this.fileComparator);
            this.destination.Controls.Add(this.infoCopyPrecondition);
            this.destination.Controls.Add(this.copyPrecondition);
            this.destination.Controls.Add(this.infoDestinationDirectory);
            this.destination.Controls.Add(this.infoCopyMode);
            this.destination.Controls.Add(this.infoLocalization);
            this.destination.Controls.Add(this.infoPatternImage);
            this.destination.Controls.Add(this.infoPatternVideo);
            this.destination.Controls.Add(this.infoPatternAudio);
            this.destination.Controls.Add(this.patternAudio);
            this.destination.Controls.Add(this.patternVideo);
            this.destination.Controls.Add(this.patternImage);
            this.destination.Controls.Add(this.localization);
            this.destination.Controls.Add(this.copyMode);
            this.destination.Controls.Add(this.destinationPath);
            this.destination.Location = new System.Drawing.Point(12, 92);
            this.destination.Name = "destination";
            this.destination.Size = new System.Drawing.Size(365, 265);
            this.destination.TabIndex = 3;
            this.destination.TabStop = false;
            this.destination.Text = "Destination";
            // 
            // infoFileVerification
            // 
            this.infoFileVerification.AutoSize = true;
            this.infoFileVerification.Location = new System.Drawing.Point(6, 132);
            this.infoFileVerification.Name = "infoFileVerification";
            this.infoFileVerification.Size = new System.Drawing.Size(77, 13);
            this.infoFileVerification.TabIndex = 12;
            this.infoFileVerification.Text = "File verification";
            // 
            // fileComparator
            // 
            this.fileComparator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileComparator.CheckBoxAll = true;
            this.fileComparator.CheckBoxNone = true;
            this.fileComparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileComparator.EnumText = null;
            this.fileComparator.EnumType = null;
            this.fileComparator.EnumValue = null;
            this.fileComparator.FormattingEnabled = true;
            this.fileComparator.Items.AddRange(new object[] {
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "All"});
            this.fileComparator.Location = new System.Drawing.Point(103, 102);
            this.fileComparator.Name = "fileComparator";
            this.fileComparator.Size = new System.Drawing.Size(181, 21);
            this.fileComparator.TabIndex = 7;
            // 
            // infoCopyPrecondition
            // 
            this.infoCopyPrecondition.AutoSize = true;
            this.infoCopyPrecondition.Location = new System.Drawing.Point(6, 51);
            this.infoCopyPrecondition.Name = "infoCopyPrecondition";
            this.infoCopyPrecondition.Size = new System.Drawing.Size(92, 13);
            this.infoCopyPrecondition.TabIndex = 11;
            this.infoCopyPrecondition.Text = "Copy precondition";
            // 
            // copyPrecondition
            // 
            this.copyPrecondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.copyPrecondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.copyPrecondition.EnumText = null;
            this.copyPrecondition.EnumType = null;
            this.copyPrecondition.EnumValue = null;
            this.copyPrecondition.FormattingEnabled = true;
            this.copyPrecondition.Location = new System.Drawing.Point(103, 48);
            this.copyPrecondition.Name = "copyPrecondition";
            this.copyPrecondition.Size = new System.Drawing.Size(181, 21);
            this.copyPrecondition.TabIndex = 5;
            // 
            // infoDestinationDirectory
            // 
            this.infoDestinationDirectory.AutoSize = true;
            this.infoDestinationDirectory.Location = new System.Drawing.Point(6, 25);
            this.infoDestinationDirectory.Name = "infoDestinationDirectory";
            this.infoDestinationDirectory.Size = new System.Drawing.Size(49, 13);
            this.infoDestinationDirectory.TabIndex = 4;
            this.infoDestinationDirectory.Text = "Directory";
            // 
            // infoCopyMode
            // 
            this.infoCopyMode.AutoSize = true;
            this.infoCopyMode.Location = new System.Drawing.Point(6, 78);
            this.infoCopyMode.Name = "infoCopyMode";
            this.infoCopyMode.Size = new System.Drawing.Size(60, 13);
            this.infoCopyMode.TabIndex = 5;
            this.infoCopyMode.Text = "Copy mode";
            // 
            // infoLocalization
            // 
            this.infoLocalization.AutoSize = true;
            this.infoLocalization.Location = new System.Drawing.Point(6, 159);
            this.infoLocalization.Name = "infoLocalization";
            this.infoLocalization.Size = new System.Drawing.Size(63, 13);
            this.infoLocalization.TabIndex = 6;
            this.infoLocalization.Text = "Localization";
            // 
            // infoPatternImage
            // 
            this.infoPatternImage.AutoSize = true;
            this.infoPatternImage.Location = new System.Drawing.Point(6, 186);
            this.infoPatternImage.Name = "infoPatternImage";
            this.infoPatternImage.Size = new System.Drawing.Size(72, 13);
            this.infoPatternImage.TabIndex = 7;
            this.infoPatternImage.Text = "Image pattern";
            // 
            // infoPatternVideo
            // 
            this.infoPatternVideo.AutoSize = true;
            this.infoPatternVideo.Location = new System.Drawing.Point(6, 212);
            this.infoPatternVideo.Name = "infoPatternVideo";
            this.infoPatternVideo.Size = new System.Drawing.Size(70, 13);
            this.infoPatternVideo.TabIndex = 8;
            this.infoPatternVideo.Text = "Video pattern";
            // 
            // infoPatternAudio
            // 
            this.infoPatternAudio.AutoSize = true;
            this.infoPatternAudio.Location = new System.Drawing.Point(6, 238);
            this.infoPatternAudio.Name = "infoPatternAudio";
            this.infoPatternAudio.Size = new System.Drawing.Size(70, 13);
            this.infoPatternAudio.TabIndex = 9;
            this.infoPatternAudio.Text = "Audio pattern";
            // 
            // patternAudio
            // 
            this.patternAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patternAudio.Location = new System.Drawing.Point(103, 235);
            this.patternAudio.Name = "patternAudio";
            this.patternAudio.Size = new System.Drawing.Size(256, 20);
            this.patternAudio.TabIndex = 11;
            // 
            // patternVideo
            // 
            this.patternVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patternVideo.Location = new System.Drawing.Point(103, 209);
            this.patternVideo.Name = "patternVideo";
            this.patternVideo.Size = new System.Drawing.Size(256, 20);
            this.patternVideo.TabIndex = 10;
            // 
            // patternImage
            // 
            this.patternImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patternImage.Location = new System.Drawing.Point(103, 183);
            this.patternImage.Name = "patternImage";
            this.patternImage.Size = new System.Drawing.Size(256, 20);
            this.patternImage.TabIndex = 9;
            // 
            // localization
            // 
            this.localization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.localization.FormattingEnabled = true;
            this.localization.Location = new System.Drawing.Point(103, 156);
            this.localization.Name = "localization";
            this.localization.Size = new System.Drawing.Size(181, 21);
            this.localization.TabIndex = 8;
            // 
            // copyMode
            // 
            this.copyMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.copyMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.copyMode.EnumText = null;
            this.copyMode.EnumType = null;
            this.copyMode.EnumValue = null;
            this.copyMode.FormattingEnabled = true;
            this.copyMode.Location = new System.Drawing.Point(103, 75);
            this.copyMode.Name = "copyMode";
            this.copyMode.Size = new System.Drawing.Size(181, 21);
            this.copyMode.TabIndex = 6;
            // 
            // destinationPath
            // 
            this.destinationPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationPath.BrowseType = ExifOrganizer.UI.Controls.FileBrowseType.Directory;
            this.destinationPath.DialogTitle = "Destination directory";
            this.destinationPath.Location = new System.Drawing.Point(103, 19);
            this.destinationPath.Name = "destinationPath";
            this.destinationPath.ReadOnly = false;
            this.destinationPath.SelectedPath = "";
            this.destinationPath.Size = new System.Drawing.Size(256, 23);
            this.destinationPath.TabIndex = 4;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoVersion});
            this.statusBar.Location = new System.Drawing.Point(0, 393);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(389, 22);
            this.statusBar.TabIndex = 14;
            this.statusBar.Text = "statusStrip1";
            // 
            // infoVersion
            // 
            this.infoVersion.Name = "infoVersion";
            this.infoVersion.Size = new System.Drawing.Size(73, 17);
            this.infoVersion.Text = "Version: x.y.z";
            // 
            // progress
            // 
            this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progress.Brush = null;
            this.progress.CenterText = false;
            this.progress.Font = new System.Drawing.Font("Arial", 8.25F);
            this.progress.Location = new System.Drawing.Point(12, 363);
            this.progress.Name = "progress";
            this.progress.ProgressText = null;
            this.progress.Size = new System.Drawing.Size(284, 23);
            this.progress.TabIndex = 12;
            this.progress.Visible = false;
            // 
            // fileVerification
            // 
            this.fileVerification.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileVerification.CheckBoxAll = true;
            this.fileVerification.CheckBoxNone = true;
            this.fileVerification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileVerification.EnumText = null;
            this.fileVerification.EnumType = null;
            this.fileVerification.EnumValue = null;
            this.fileVerification.FormattingEnabled = true;
            this.fileVerification.Items.AddRange(new object[] {
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "<None>",
            "All"});
            this.fileVerification.Location = new System.Drawing.Point(103, 129);
            this.fileVerification.Name = "fileVerification";
            this.fileVerification.Size = new System.Drawing.Size(181, 21);
            this.fileVerification.TabIndex = 13;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 415);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.destination);
            this.Controls.Add(this.source);
            this.Controls.Add(this.organize);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "ExifOrganizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.source.ResumeLayout(false);
            this.source.PerformLayout();
            this.destination.ResumeLayout(false);
            this.destination.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button organize;
        private Controls.FileBrowseControl sourcePath;
        private Controls.FileBrowseControl destinationPath;
        private System.Windows.Forms.GroupBox source;
        private System.Windows.Forms.GroupBox destination;
        private System.Windows.Forms.CheckBox recursive;
        private Controls.EnumDropDown copyMode;
        private System.Windows.Forms.ComboBox localization;
        private System.Windows.Forms.TextBox patternAudio;
        private System.Windows.Forms.TextBox patternVideo;
        private System.Windows.Forms.TextBox patternImage;
        private System.Windows.Forms.Label infoSourceDirectory;
        private System.Windows.Forms.Label infoRecursive;
        private System.Windows.Forms.Label infoDestinationDirectory;
        private System.Windows.Forms.Label infoCopyMode;
        private System.Windows.Forms.Label infoLocalization;
        private System.Windows.Forms.Label infoPatternImage;
        private System.Windows.Forms.Label infoPatternVideo;
        private System.Windows.Forms.Label infoPatternAudio;
        private System.Windows.Forms.Label infoFileComparator;
        private Controls.ProgressBarText progress;
        private System.Windows.Forms.Label infoCopyPrecondition;
        private Controls.EnumDropDown copyPrecondition;
        private Controls.EnumFlagsDropDown fileComparator;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel infoVersion;
        private System.Windows.Forms.Label infoFileVerification;
        private EnumFlagsDropDown fileVerification;
    }
}

