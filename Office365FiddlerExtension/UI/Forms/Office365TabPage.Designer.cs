﻿namespace Office365FiddlerExtension.UI.Forms
{
    partial class Office365TabPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AnalyseAllSessionsButton = new System.Windows.Forms.Button();
            this.SessionAnalysisGroupBox = new System.Windows.Forms.GroupBox();
            this.CreateConsolidatedAnalysisButton = new System.Windows.Forms.Button();
            this.ClearSelectedSessionAnalysisButton = new System.Windows.Forms.Button();
            this.AnalyseSelectedSessionsButton = new System.Windows.Forms.Button();
            this.ClearAllSessionAnalysisButton = new System.Windows.Forms.Button();
            this.CheckIPAddressGroupBox = new System.Windows.Forms.GroupBox();
            this.CheckIPAddressClearButton = new System.Windows.Forms.Button();
            this.CheckIPAddressResultTextBox = new System.Windows.Forms.TextBox();
            this.CheckIPAddressButton = new System.Windows.Forms.Button();
            this.EnterIPAddressTextBox = new System.Windows.Forms.TextBox();
            this.ExtensionOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.ColumnsUIGroupbox = new System.Windows.Forms.GroupBox();
            this.ElapsedTimeCheckbox = new System.Windows.Forms.CheckBox();
            this.HostIPCheckbox = new System.Windows.Forms.CheckBox();
            this.ResponseServerCheckbox = new System.Windows.Forms.CheckBox();
            this.AuthenticationCheckbox = new System.Windows.Forms.CheckBox();
            this.SessionTypeCheckbox = new System.Windows.Forms.CheckBox();
            this.SeverityCheckbox = new System.Windows.Forms.CheckBox();
            this.WarnBeforeProcessingGroupBox = new System.Windows.Forms.GroupBox();
            this.WarnBeforeProcessingSessionsLabel = new System.Windows.Forms.Label();
            this.WarnBeforeAnalysingTextBox = new System.Windows.Forms.TextBox();
            this.WhenToAnalyseSessionsGroupBox = new System.Windows.Forms.GroupBox();
            this.SessionAnalysisOnImportCheckBox = new System.Windows.Forms.CheckBox();
            this.SessionAnalysisOnLiveTraceCheckBox = new System.Windows.Forms.CheckBox();
            this.SessionAnalysisOnLoadSazCheckBox = new System.Windows.Forms.CheckBox();
            this.ExtensionEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.DebugModeCheckBox = new System.Windows.Forms.CheckBox();
            this.NeverWebCallCheckBox = new System.Windows.Forms.CheckBox();
            this.DebugGroupBox = new System.Windows.Forms.GroupBox();
            this.ExtensionSettingsTextbox = new System.Windows.Forms.TextBox();
            this.DebugModeUpdateButton = new System.Windows.Forms.Button();
            this.ExtensionVersionInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.UpdateLinkLabel = new System.Windows.Forms.LinkLabel();
            this.RulesetVersionLabel = new System.Windows.Forms.Label();
            this.ExtensionVersionLabel = new System.Windows.Forms.Label();
            this.SessionAnalysisGroupBox.SuspendLayout();
            this.CheckIPAddressGroupBox.SuspendLayout();
            this.ExtensionOptionsGroupBox.SuspendLayout();
            this.ColumnsUIGroupbox.SuspendLayout();
            this.WarnBeforeProcessingGroupBox.SuspendLayout();
            this.WhenToAnalyseSessionsGroupBox.SuspendLayout();
            this.DebugGroupBox.SuspendLayout();
            this.ExtensionVersionInformationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AnalyseAllSessionsButton
            // 
            this.AnalyseAllSessionsButton.Location = new System.Drawing.Point(6, 19);
            this.AnalyseAllSessionsButton.Name = "AnalyseAllSessionsButton";
            this.AnalyseAllSessionsButton.Size = new System.Drawing.Size(193, 23);
            this.AnalyseAllSessionsButton.TabIndex = 0;
            this.AnalyseAllSessionsButton.Text = "Analyse All Sessions HC";
            this.AnalyseAllSessionsButton.UseVisualStyleBackColor = true;
            this.AnalyseAllSessionsButton.Click += new System.EventHandler(this.AnalyseAllSessionsButton_Click);
            // 
            // SessionAnalysisGroupBox
            // 
            this.SessionAnalysisGroupBox.Controls.Add(this.CreateConsolidatedAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.ClearSelectedSessionAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.AnalyseSelectedSessionsButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.ClearAllSessionAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.AnalyseAllSessionsButton);
            this.SessionAnalysisGroupBox.Location = new System.Drawing.Point(3, 244);
            this.SessionAnalysisGroupBox.Name = "SessionAnalysisGroupBox";
            this.SessionAnalysisGroupBox.Size = new System.Drawing.Size(409, 112);
            this.SessionAnalysisGroupBox.TabIndex = 1;
            this.SessionAnalysisGroupBox.TabStop = false;
            this.SessionAnalysisGroupBox.Text = "Session Analysis HC";
            // 
            // CreateConsolidatedAnalysisButton
            // 
            this.CreateConsolidatedAnalysisButton.Location = new System.Drawing.Point(7, 77);
            this.CreateConsolidatedAnalysisButton.Name = "CreateConsolidatedAnalysisButton";
            this.CreateConsolidatedAnalysisButton.Size = new System.Drawing.Size(392, 23);
            this.CreateConsolidatedAnalysisButton.TabIndex = 3;
            this.CreateConsolidatedAnalysisButton.Text = "Create Consolidated Analysis Report HC";
            this.CreateConsolidatedAnalysisButton.UseVisualStyleBackColor = true;
            this.CreateConsolidatedAnalysisButton.Click += new System.EventHandler(this.CreateConsolidatedAnalysisButton_Click);
            // 
            // ClearSelectedSessionAnalysisButton
            // 
            this.ClearSelectedSessionAnalysisButton.Location = new System.Drawing.Point(205, 48);
            this.ClearSelectedSessionAnalysisButton.Name = "ClearSelectedSessionAnalysisButton";
            this.ClearSelectedSessionAnalysisButton.Size = new System.Drawing.Size(193, 23);
            this.ClearSelectedSessionAnalysisButton.TabIndex = 3;
            this.ClearSelectedSessionAnalysisButton.Text = "Clear Selected Session Analysis HC";
            this.ClearSelectedSessionAnalysisButton.UseVisualStyleBackColor = true;
            this.ClearSelectedSessionAnalysisButton.Click += new System.EventHandler(this.ClearSelectedSessionAnalysisButton_Click);
            // 
            // AnalyseSelectedSessionsButton
            // 
            this.AnalyseSelectedSessionsButton.Location = new System.Drawing.Point(205, 19);
            this.AnalyseSelectedSessionsButton.Name = "AnalyseSelectedSessionsButton";
            this.AnalyseSelectedSessionsButton.Size = new System.Drawing.Size(193, 23);
            this.AnalyseSelectedSessionsButton.TabIndex = 2;
            this.AnalyseSelectedSessionsButton.Text = "Analyse Selected Sessions HC";
            this.AnalyseSelectedSessionsButton.UseVisualStyleBackColor = true;
            this.AnalyseSelectedSessionsButton.Click += new System.EventHandler(this.AnalyseSelectedSessionsButton_Click);
            // 
            // ClearAllSessionAnalysisButton
            // 
            this.ClearAllSessionAnalysisButton.Location = new System.Drawing.Point(6, 48);
            this.ClearAllSessionAnalysisButton.Name = "ClearAllSessionAnalysisButton";
            this.ClearAllSessionAnalysisButton.Size = new System.Drawing.Size(193, 23);
            this.ClearAllSessionAnalysisButton.TabIndex = 1;
            this.ClearAllSessionAnalysisButton.Text = "Clear All Session Analysis HC";
            this.ClearAllSessionAnalysisButton.UseVisualStyleBackColor = true;
            this.ClearAllSessionAnalysisButton.Click += new System.EventHandler(this.ClearAllSessionAnalysisButton_Click);
            // 
            // CheckIPAddressGroupBox
            // 
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressClearButton);
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressResultTextBox);
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressButton);
            this.CheckIPAddressGroupBox.Controls.Add(this.EnterIPAddressTextBox);
            this.CheckIPAddressGroupBox.Location = new System.Drawing.Point(3, 362);
            this.CheckIPAddressGroupBox.Name = "CheckIPAddressGroupBox";
            this.CheckIPAddressGroupBox.Size = new System.Drawing.Size(409, 115);
            this.CheckIPAddressGroupBox.TabIndex = 3;
            this.CheckIPAddressGroupBox.TabStop = false;
            this.CheckIPAddressGroupBox.Text = "Check IP Address HC";
            // 
            // CheckIPAddressClearButton
            // 
            this.CheckIPAddressClearButton.Location = new System.Drawing.Point(323, 48);
            this.CheckIPAddressClearButton.Name = "CheckIPAddressClearButton";
            this.CheckIPAddressClearButton.Size = new System.Drawing.Size(75, 23);
            this.CheckIPAddressClearButton.TabIndex = 3;
            this.CheckIPAddressClearButton.Text = "Clear HC";
            this.CheckIPAddressClearButton.UseVisualStyleBackColor = true;
            this.CheckIPAddressClearButton.Click += new System.EventHandler(this.CheckIPAddressClearButton_Click);
            // 
            // CheckIPAddressResultTextBox
            // 
            this.CheckIPAddressResultTextBox.BackColor = System.Drawing.Color.White;
            this.CheckIPAddressResultTextBox.Location = new System.Drawing.Point(6, 50);
            this.CheckIPAddressResultTextBox.Multiline = true;
            this.CheckIPAddressResultTextBox.Name = "CheckIPAddressResultTextBox";
            this.CheckIPAddressResultTextBox.ReadOnly = true;
            this.CheckIPAddressResultTextBox.Size = new System.Drawing.Size(311, 50);
            this.CheckIPAddressResultTextBox.TabIndex = 2;
            // 
            // CheckIPAddressButton
            // 
            this.CheckIPAddressButton.Location = new System.Drawing.Point(323, 19);
            this.CheckIPAddressButton.Name = "CheckIPAddressButton";
            this.CheckIPAddressButton.Size = new System.Drawing.Size(75, 23);
            this.CheckIPAddressButton.TabIndex = 1;
            this.CheckIPAddressButton.Text = "Check HC";
            this.CheckIPAddressButton.UseVisualStyleBackColor = true;
            this.CheckIPAddressButton.Click += new System.EventHandler(this.CheckIPAddressButton_Click);
            // 
            // EnterIPAddressTextBox
            // 
            this.EnterIPAddressTextBox.Location = new System.Drawing.Point(6, 21);
            this.EnterIPAddressTextBox.Name = "EnterIPAddressTextBox";
            this.EnterIPAddressTextBox.Size = new System.Drawing.Size(311, 20);
            this.EnterIPAddressTextBox.TabIndex = 0;
            // 
            // ExtensionOptionsGroupBox
            // 
            this.ExtensionOptionsGroupBox.Controls.Add(this.ColumnsUIGroupbox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.WarnBeforeProcessingGroupBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.WhenToAnalyseSessionsGroupBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.ExtensionEnabledCheckBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.DebugModeCheckBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.NeverWebCallCheckBox);
            this.ExtensionOptionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ExtensionOptionsGroupBox.Name = "ExtensionOptionsGroupBox";
            this.ExtensionOptionsGroupBox.Size = new System.Drawing.Size(409, 235);
            this.ExtensionOptionsGroupBox.TabIndex = 4;
            this.ExtensionOptionsGroupBox.TabStop = false;
            this.ExtensionOptionsGroupBox.Text = "Extension Options HC";
            // 
            // ColumnsUIGroupbox
            // 
            this.ColumnsUIGroupbox.Controls.Add(this.ElapsedTimeCheckbox);
            this.ColumnsUIGroupbox.Controls.Add(this.HostIPCheckbox);
            this.ColumnsUIGroupbox.Controls.Add(this.ResponseServerCheckbox);
            this.ColumnsUIGroupbox.Controls.Add(this.AuthenticationCheckbox);
            this.ColumnsUIGroupbox.Controls.Add(this.SessionTypeCheckbox);
            this.ColumnsUIGroupbox.Controls.Add(this.SeverityCheckbox);
            this.ColumnsUIGroupbox.Location = new System.Drawing.Point(7, 42);
            this.ColumnsUIGroupbox.Name = "ColumnsUIGroupbox";
            this.ColumnsUIGroupbox.Size = new System.Drawing.Size(391, 70);
            this.ColumnsUIGroupbox.TabIndex = 11;
            this.ColumnsUIGroupbox.TabStop = false;
            this.ColumnsUIGroupbox.Text = "Columns Enabled HC";
            // 
            // ElapsedTimeCheckbox
            // 
            this.ElapsedTimeCheckbox.AutoSize = true;
            this.ElapsedTimeCheckbox.Location = new System.Drawing.Point(11, 19);
            this.ElapsedTimeCheckbox.Name = "ElapsedTimeCheckbox";
            this.ElapsedTimeCheckbox.Size = new System.Drawing.Size(108, 17);
            this.ElapsedTimeCheckbox.TabIndex = 5;
            this.ElapsedTimeCheckbox.Text = "Elapsed Time HC";
            this.ElapsedTimeCheckbox.UseVisualStyleBackColor = true;
            this.ElapsedTimeCheckbox.CheckedChanged += new System.EventHandler(this.ElapsedTimeCheckbox_CheckedChanged);
            // 
            // HostIPCheckbox
            // 
            this.HostIPCheckbox.AutoSize = true;
            this.HostIPCheckbox.Location = new System.Drawing.Point(277, 44);
            this.HostIPCheckbox.Name = "HostIPCheckbox";
            this.HostIPCheckbox.Size = new System.Drawing.Size(79, 17);
            this.HostIPCheckbox.TabIndex = 4;
            this.HostIPCheckbox.Text = "Host IP HC";
            this.HostIPCheckbox.UseVisualStyleBackColor = true;
            this.HostIPCheckbox.CheckedChanged += new System.EventHandler(this.HostIPCheckbox_CheckedChanged);
            // 
            // ResponseServerCheckbox
            // 
            this.ResponseServerCheckbox.AutoSize = true;
            this.ResponseServerCheckbox.Location = new System.Drawing.Point(138, 44);
            this.ResponseServerCheckbox.Name = "ResponseServerCheckbox";
            this.ResponseServerCheckbox.Size = new System.Drawing.Size(126, 17);
            this.ResponseServerCheckbox.TabIndex = 3;
            this.ResponseServerCheckbox.Text = "Response Server HC";
            this.ResponseServerCheckbox.UseVisualStyleBackColor = true;
            this.ResponseServerCheckbox.CheckedChanged += new System.EventHandler(this.ResponseServerCheckbox_CheckedChanged);
            // 
            // AuthenticationCheckbox
            // 
            this.AuthenticationCheckbox.AutoSize = true;
            this.AuthenticationCheckbox.Location = new System.Drawing.Point(277, 19);
            this.AuthenticationCheckbox.Name = "AuthenticationCheckbox";
            this.AuthenticationCheckbox.Size = new System.Drawing.Size(112, 17);
            this.AuthenticationCheckbox.TabIndex = 2;
            this.AuthenticationCheckbox.Text = "Authentication HC";
            this.AuthenticationCheckbox.UseVisualStyleBackColor = true;
            this.AuthenticationCheckbox.CheckedChanged += new System.EventHandler(this.AuthenticationCheckbox_CheckedChanged);
            // 
            // SessionTypeCheckbox
            // 
            this.SessionTypeCheckbox.AutoSize = true;
            this.SessionTypeCheckbox.Location = new System.Drawing.Point(138, 19);
            this.SessionTypeCheckbox.Name = "SessionTypeCheckbox";
            this.SessionTypeCheckbox.Size = new System.Drawing.Size(108, 17);
            this.SessionTypeCheckbox.TabIndex = 1;
            this.SessionTypeCheckbox.Text = "Session Type HC";
            this.SessionTypeCheckbox.UseVisualStyleBackColor = true;
            this.SessionTypeCheckbox.CheckedChanged += new System.EventHandler(this.SessionTypeCheckbox_CheckedChanged);
            // 
            // SeverityCheckbox
            // 
            this.SeverityCheckbox.AutoSize = true;
            this.SeverityCheckbox.Location = new System.Drawing.Point(11, 44);
            this.SeverityCheckbox.Name = "SeverityCheckbox";
            this.SeverityCheckbox.Size = new System.Drawing.Size(82, 17);
            this.SeverityCheckbox.TabIndex = 0;
            this.SeverityCheckbox.Text = "Severity HC";
            this.SeverityCheckbox.UseVisualStyleBackColor = true;
            this.SeverityCheckbox.CheckedChanged += new System.EventHandler(this.SeverityCheckbox_CheckedChanged);
            // 
            // WarnBeforeProcessingGroupBox
            // 
            this.WarnBeforeProcessingGroupBox.Controls.Add(this.WarnBeforeProcessingSessionsLabel);
            this.WarnBeforeProcessingGroupBox.Controls.Add(this.WarnBeforeAnalysingTextBox);
            this.WarnBeforeProcessingGroupBox.Location = new System.Drawing.Point(7, 171);
            this.WarnBeforeProcessingGroupBox.Name = "WarnBeforeProcessingGroupBox";
            this.WarnBeforeProcessingGroupBox.Size = new System.Drawing.Size(391, 52);
            this.WarnBeforeProcessingGroupBox.TabIndex = 10;
            this.WarnBeforeProcessingGroupBox.TabStop = false;
            this.WarnBeforeProcessingGroupBox.Text = "Warn Before Analysing HC";
            // 
            // WarnBeforeProcessingSessionsLabel
            // 
            this.WarnBeforeProcessingSessionsLabel.AutoSize = true;
            this.WarnBeforeProcessingSessionsLabel.Location = new System.Drawing.Point(89, 22);
            this.WarnBeforeProcessingSessionsLabel.Name = "WarnBeforeProcessingSessionsLabel";
            this.WarnBeforeProcessingSessionsLabel.Size = new System.Drawing.Size(67, 13);
            this.WarnBeforeProcessingSessionsLabel.TabIndex = 8;
            this.WarnBeforeProcessingSessionsLabel.Text = "Sessions HC";
            // 
            // WarnBeforeAnalysingTextBox
            // 
            this.WarnBeforeAnalysingTextBox.Location = new System.Drawing.Point(11, 19);
            this.WarnBeforeAnalysingTextBox.Name = "WarnBeforeAnalysingTextBox";
            this.WarnBeforeAnalysingTextBox.Size = new System.Drawing.Size(72, 20);
            this.WarnBeforeAnalysingTextBox.TabIndex = 7;
            this.WarnBeforeAnalysingTextBox.TextChanged += new System.EventHandler(this.WarnBeforeAnalysingTextBox_TextChanged);
            // 
            // WhenToAnalyseSessionsGroupBox
            // 
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnImportCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnLiveTraceCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnLoadSazCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Location = new System.Drawing.Point(7, 118);
            this.WhenToAnalyseSessionsGroupBox.Name = "WhenToAnalyseSessionsGroupBox";
            this.WhenToAnalyseSessionsGroupBox.Size = new System.Drawing.Size(391, 47);
            this.WhenToAnalyseSessionsGroupBox.TabIndex = 7;
            this.WhenToAnalyseSessionsGroupBox.TabStop = false;
            this.WhenToAnalyseSessionsGroupBox.Text = "Choose When To Analyse Sessions HC";
            // 
            // SessionAnalysisOnImportCheckBox
            // 
            this.SessionAnalysisOnImportCheckBox.AutoSize = true;
            this.SessionAnalysisOnImportCheckBox.Location = new System.Drawing.Point(277, 20);
            this.SessionAnalysisOnImportCheckBox.Name = "SessionAnalysisOnImportCheckBox";
            this.SessionAnalysisOnImportCheckBox.Size = new System.Drawing.Size(90, 17);
            this.SessionAnalysisOnImportCheckBox.TabIndex = 5;
            this.SessionAnalysisOnImportCheckBox.Text = "On Import HC";
            this.SessionAnalysisOnImportCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnImportCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnImportCheckBox_CheckedChanged);
            // 
            // SessionAnalysisOnLiveTraceCheckBox
            // 
            this.SessionAnalysisOnLiveTraceCheckBox.AutoSize = true;
            this.SessionAnalysisOnLiveTraceCheckBox.Location = new System.Drawing.Point(11, 20);
            this.SessionAnalysisOnLiveTraceCheckBox.Name = "SessionAnalysisOnLiveTraceCheckBox";
            this.SessionAnalysisOnLiveTraceCheckBox.Size = new System.Drawing.Size(112, 17);
            this.SessionAnalysisOnLiveTraceCheckBox.TabIndex = 4;
            this.SessionAnalysisOnLiveTraceCheckBox.Text = "On Live Trace HC";
            this.SessionAnalysisOnLiveTraceCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnLiveTraceCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnLiveTraceCheckBox_CheckedChanged);
            // 
            // SessionAnalysisOnLoadSazCheckBox
            // 
            this.SessionAnalysisOnLoadSazCheckBox.AutoSize = true;
            this.SessionAnalysisOnLoadSazCheckBox.Location = new System.Drawing.Point(138, 20);
            this.SessionAnalysisOnLoadSazCheckBox.Name = "SessionAnalysisOnLoadSazCheckBox";
            this.SessionAnalysisOnLoadSazCheckBox.Size = new System.Drawing.Size(106, 17);
            this.SessionAnalysisOnLoadSazCheckBox.TabIndex = 3;
            this.SessionAnalysisOnLoadSazCheckBox.Text = "On Load Saz HC";
            this.SessionAnalysisOnLoadSazCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnLoadSazCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnLoadSazCheckBox_CheckedChanged);
            // 
            // ExtensionEnabledCheckBox
            // 
            this.ExtensionEnabledCheckBox.AutoSize = true;
            this.ExtensionEnabledCheckBox.Location = new System.Drawing.Point(18, 19);
            this.ExtensionEnabledCheckBox.Name = "ExtensionEnabledCheckBox";
            this.ExtensionEnabledCheckBox.Size = new System.Drawing.Size(132, 17);
            this.ExtensionEnabledCheckBox.TabIndex = 0;
            this.ExtensionEnabledCheckBox.Text = "Extension Enabled HC";
            this.ExtensionEnabledCheckBox.UseVisualStyleBackColor = true;
            this.ExtensionEnabledCheckBox.CheckedChanged += new System.EventHandler(this.ExtensionEnabledCheckBox_CheckedChanged);
            // 
            // DebugModeCheckBox
            // 
            this.DebugModeCheckBox.AutoSize = true;
            this.DebugModeCheckBox.Location = new System.Drawing.Point(145, 19);
            this.DebugModeCheckBox.Name = "DebugModeCheckBox";
            this.DebugModeCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DebugModeCheckBox.Size = new System.Drawing.Size(88, 17);
            this.DebugModeCheckBox.TabIndex = 8;
            this.DebugModeCheckBox.Text = "Debug Mode";
            this.DebugModeCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DebugModeCheckBox.UseVisualStyleBackColor = true;
            this.DebugModeCheckBox.CheckedChanged += new System.EventHandler(this.DebugModeCheckBox_CheckedChanged);
            // 
            // NeverWebCallCheckBox
            // 
            this.NeverWebCallCheckBox.AutoSize = true;
            this.NeverWebCallCheckBox.Location = new System.Drawing.Point(284, 19);
            this.NeverWebCallCheckBox.Name = "NeverWebCallCheckBox";
            this.NeverWebCallCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NeverWebCallCheckBox.Size = new System.Drawing.Size(101, 17);
            this.NeverWebCallCheckBox.TabIndex = 7;
            this.NeverWebCallCheckBox.Text = "Never Web Call";
            this.NeverWebCallCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NeverWebCallCheckBox.UseVisualStyleBackColor = true;
            this.NeverWebCallCheckBox.CheckedChanged += new System.EventHandler(this.NeverWebCallCheckBox_CheckedChanged);
            // 
            // DebugGroupBox
            // 
            this.DebugGroupBox.Controls.Add(this.ExtensionSettingsTextbox);
            this.DebugGroupBox.Controls.Add(this.DebugModeUpdateButton);
            this.DebugGroupBox.Location = new System.Drawing.Point(3, 567);
            this.DebugGroupBox.Name = "DebugGroupBox";
            this.DebugGroupBox.Size = new System.Drawing.Size(409, 185);
            this.DebugGroupBox.TabIndex = 6;
            this.DebugGroupBox.TabStop = false;
            this.DebugGroupBox.Text = "Debug";
            // 
            // ExtensionSettingsTextbox
            // 
            this.ExtensionSettingsTextbox.Location = new System.Drawing.Point(7, 19);
            this.ExtensionSettingsTextbox.Multiline = true;
            this.ExtensionSettingsTextbox.Name = "ExtensionSettingsTextbox";
            this.ExtensionSettingsTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ExtensionSettingsTextbox.Size = new System.Drawing.Size(389, 123);
            this.ExtensionSettingsTextbox.TabIndex = 19;
            // 
            // DebugModeUpdateButton
            // 
            this.DebugModeUpdateButton.Location = new System.Drawing.Point(261, 148);
            this.DebugModeUpdateButton.Name = "DebugModeUpdateButton";
            this.DebugModeUpdateButton.Size = new System.Drawing.Size(135, 23);
            this.DebugModeUpdateButton.TabIndex = 17;
            this.DebugModeUpdateButton.Text = "Update Debug Page";
            this.DebugModeUpdateButton.UseVisualStyleBackColor = true;
            this.DebugModeUpdateButton.Click += new System.EventHandler(this.DebugModeUpdateButton_Click);
            // 
            // ExtensionVersionInformationGroupBox
            // 
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.UpdateLinkLabel);
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.RulesetVersionLabel);
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.ExtensionVersionLabel);
            this.ExtensionVersionInformationGroupBox.Location = new System.Drawing.Point(3, 483);
            this.ExtensionVersionInformationGroupBox.Name = "ExtensionVersionInformationGroupBox";
            this.ExtensionVersionInformationGroupBox.Size = new System.Drawing.Size(409, 78);
            this.ExtensionVersionInformationGroupBox.TabIndex = 5;
            this.ExtensionVersionInformationGroupBox.TabStop = false;
            this.ExtensionVersionInformationGroupBox.Text = "Extension Version Information HC";
            // 
            // UpdateLinkLabel
            // 
            this.UpdateLinkLabel.AutoSize = true;
            this.UpdateLinkLabel.Location = new System.Drawing.Point(6, 52);
            this.UpdateLinkLabel.Name = "UpdateLinkLabel";
            this.UpdateLinkLabel.Size = new System.Drawing.Size(88, 13);
            this.UpdateLinkLabel.TabIndex = 4;
            this.UpdateLinkLabel.TabStop = true;
            this.UpdateLinkLabel.Text = "UpdateLinkLabel";
            this.UpdateLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UpdateLinkLabel_LinkClicked);
            // 
            // RulesetVersionLabel
            // 
            this.RulesetVersionLabel.AutoSize = true;
            this.RulesetVersionLabel.Location = new System.Drawing.Point(6, 35);
            this.RulesetVersionLabel.Name = "RulesetVersionLabel";
            this.RulesetVersionLabel.Size = new System.Drawing.Size(99, 13);
            this.RulesetVersionLabel.TabIndex = 2;
            this.RulesetVersionLabel.Text = "Ruleset Version HC";
            // 
            // ExtensionVersionLabel
            // 
            this.ExtensionVersionLabel.AutoSize = true;
            this.ExtensionVersionLabel.Location = new System.Drawing.Point(6, 18);
            this.ExtensionVersionLabel.Name = "ExtensionVersionLabel";
            this.ExtensionVersionLabel.Size = new System.Drawing.Size(109, 13);
            this.ExtensionVersionLabel.TabIndex = 0;
            this.ExtensionVersionLabel.Text = "Extension Version HC";
            // 
            // Office365TabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DebugGroupBox);
            this.Controls.Add(this.ExtensionVersionInformationGroupBox);
            this.Controls.Add(this.ExtensionOptionsGroupBox);
            this.Controls.Add(this.CheckIPAddressGroupBox);
            this.Controls.Add(this.SessionAnalysisGroupBox);
            this.Name = "Office365TabPage";
            this.Size = new System.Drawing.Size(425, 768);
            this.Load += new System.EventHandler(this.Office365TabPage_Load);
            this.SessionAnalysisGroupBox.ResumeLayout(false);
            this.CheckIPAddressGroupBox.ResumeLayout(false);
            this.CheckIPAddressGroupBox.PerformLayout();
            this.ExtensionOptionsGroupBox.ResumeLayout(false);
            this.ExtensionOptionsGroupBox.PerformLayout();
            this.ColumnsUIGroupbox.ResumeLayout(false);
            this.ColumnsUIGroupbox.PerformLayout();
            this.WarnBeforeProcessingGroupBox.ResumeLayout(false);
            this.WarnBeforeProcessingGroupBox.PerformLayout();
            this.WhenToAnalyseSessionsGroupBox.ResumeLayout(false);
            this.WhenToAnalyseSessionsGroupBox.PerformLayout();
            this.DebugGroupBox.ResumeLayout(false);
            this.DebugGroupBox.PerformLayout();
            this.ExtensionVersionInformationGroupBox.ResumeLayout(false);
            this.ExtensionVersionInformationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AnalyseAllSessionsButton;
        private System.Windows.Forms.GroupBox SessionAnalysisGroupBox;
        private System.Windows.Forms.Button AnalyseSelectedSessionsButton;
        private System.Windows.Forms.Button ClearAllSessionAnalysisButton;
        private System.Windows.Forms.Button ClearSelectedSessionAnalysisButton;
        private System.Windows.Forms.Button CreateConsolidatedAnalysisButton;
        private System.Windows.Forms.GroupBox CheckIPAddressGroupBox;
        private System.Windows.Forms.TextBox CheckIPAddressResultTextBox;
        private System.Windows.Forms.Button CheckIPAddressButton;
        private System.Windows.Forms.TextBox EnterIPAddressTextBox;
        private System.Windows.Forms.GroupBox ExtensionOptionsGroupBox;
        private System.Windows.Forms.CheckBox ExtensionEnabledCheckBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLiveTraceCheckBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLoadSazCheckBox;
        private System.Windows.Forms.Button CheckIPAddressClearButton;
        private System.Windows.Forms.GroupBox ExtensionVersionInformationGroupBox;
        private System.Windows.Forms.Label ExtensionVersionLabel;
        private System.Windows.Forms.Label RulesetVersionLabel;
        private System.Windows.Forms.LinkLabel UpdateLinkLabel;
        private System.Windows.Forms.CheckBox NeverWebCallCheckBox;
        private System.Windows.Forms.CheckBox DebugModeCheckBox;
        private System.Windows.Forms.GroupBox DebugGroupBox;
        private System.Windows.Forms.GroupBox WhenToAnalyseSessionsGroupBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnImportCheckBox;
        private System.Windows.Forms.GroupBox WarnBeforeProcessingGroupBox;
        private System.Windows.Forms.Label WarnBeforeProcessingSessionsLabel;
        private System.Windows.Forms.TextBox WarnBeforeAnalysingTextBox;
        private System.Windows.Forms.Button DebugModeUpdateButton;
        private System.Windows.Forms.GroupBox ColumnsUIGroupbox;
        private System.Windows.Forms.CheckBox SessionTypeCheckbox;
        private System.Windows.Forms.CheckBox SeverityCheckbox;
        private System.Windows.Forms.CheckBox HostIPCheckbox;
        private System.Windows.Forms.CheckBox ResponseServerCheckbox;
        private System.Windows.Forms.CheckBox AuthenticationCheckbox;
        private System.Windows.Forms.CheckBox ElapsedTimeCheckbox;
        private System.Windows.Forms.TextBox ExtensionSettingsTextbox;
    }
}