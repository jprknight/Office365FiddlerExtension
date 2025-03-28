namespace Office365FiddlerExtension.UI.Forms
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
            this.StopSessionAnalysisButton = new System.Windows.Forms.Button();
            this.CreateConsolidatedAnalysisButton = new System.Windows.Forms.Button();
            this.ClearSelectedSessionAnalysisButton = new System.Windows.Forms.Button();
            this.AnalyseSelectedSessionsButton = new System.Windows.Forms.Button();
            this.ClearAllSessionAnalysisButton = new System.Windows.Forms.Button();
            this.ExtensionOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.ColumnsUIGroupbox = new System.Windows.Forms.GroupBox();
            this.ElapsedTimeCheckbox = new System.Windows.Forms.CheckBox();
            this.HostIPCheckbox = new System.Windows.Forms.CheckBox();
            this.ResponseServerCheckbox = new System.Windows.Forms.CheckBox();
            this.AuthenticationCheckbox = new System.Windows.Forms.CheckBox();
            this.SessionTypeCheckbox = new System.Windows.Forms.CheckBox();
            this.SeverityCheckbox = new System.Windows.Forms.CheckBox();
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
            this.ExtensionOptionsGroupBox.SuspendLayout();
            this.ColumnsUIGroupbox.SuspendLayout();
            this.WhenToAnalyseSessionsGroupBox.SuspendLayout();
            this.DebugGroupBox.SuspendLayout();
            this.ExtensionVersionInformationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AnalyseAllSessionsButton
            // 
            this.AnalyseAllSessionsButton.Location = new System.Drawing.Point(9, 29);
            this.AnalyseAllSessionsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AnalyseAllSessionsButton.Name = "AnalyseAllSessionsButton";
            this.AnalyseAllSessionsButton.Size = new System.Drawing.Size(290, 35);
            this.AnalyseAllSessionsButton.TabIndex = 0;
            this.AnalyseAllSessionsButton.Text = "Analyse All Sessions HC";
            this.AnalyseAllSessionsButton.UseVisualStyleBackColor = true;
            this.AnalyseAllSessionsButton.Click += new System.EventHandler(this.AnalyseAllSessionsButton_Click);
            // 
            // SessionAnalysisGroupBox
            // 
            this.SessionAnalysisGroupBox.Controls.Add(this.StopSessionAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.CreateConsolidatedAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.ClearSelectedSessionAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.AnalyseSelectedSessionsButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.ClearAllSessionAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.AnalyseAllSessionsButton);
            this.SessionAnalysisGroupBox.Location = new System.Drawing.Point(4, 289);
            this.SessionAnalysisGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SessionAnalysisGroupBox.Name = "SessionAnalysisGroupBox";
            this.SessionAnalysisGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SessionAnalysisGroupBox.Size = new System.Drawing.Size(614, 214);
            this.SessionAnalysisGroupBox.TabIndex = 1;
            this.SessionAnalysisGroupBox.TabStop = false;
            this.SessionAnalysisGroupBox.Text = "Session Analysis HC";
            // 
            // StopSessionAnalysisButton
            // 
            this.StopSessionAnalysisButton.Location = new System.Drawing.Point(12, 117);
            this.StopSessionAnalysisButton.Name = "StopSessionAnalysisButton";
            this.StopSessionAnalysisButton.Size = new System.Drawing.Size(586, 35);
            this.StopSessionAnalysisButton.TabIndex = 4;
            this.StopSessionAnalysisButton.Text = "Stop Session Analysis";
            this.StopSessionAnalysisButton.UseVisualStyleBackColor = true;
            this.StopSessionAnalysisButton.Click += new System.EventHandler(this.StopSessionAnalysisButton_Click);
            // 
            // CreateConsolidatedAnalysisButton
            // 
            this.CreateConsolidatedAnalysisButton.Location = new System.Drawing.Point(12, 160);
            this.CreateConsolidatedAnalysisButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CreateConsolidatedAnalysisButton.Name = "CreateConsolidatedAnalysisButton";
            this.CreateConsolidatedAnalysisButton.Size = new System.Drawing.Size(588, 35);
            this.CreateConsolidatedAnalysisButton.TabIndex = 3;
            this.CreateConsolidatedAnalysisButton.Text = "Create Consolidated Analysis Report HC";
            this.CreateConsolidatedAnalysisButton.UseVisualStyleBackColor = true;
            this.CreateConsolidatedAnalysisButton.Click += new System.EventHandler(this.CreateConsolidatedAnalysisButton_Click);
            // 
            // ClearSelectedSessionAnalysisButton
            // 
            this.ClearSelectedSessionAnalysisButton.Location = new System.Drawing.Point(308, 74);
            this.ClearSelectedSessionAnalysisButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ClearSelectedSessionAnalysisButton.Name = "ClearSelectedSessionAnalysisButton";
            this.ClearSelectedSessionAnalysisButton.Size = new System.Drawing.Size(290, 35);
            this.ClearSelectedSessionAnalysisButton.TabIndex = 3;
            this.ClearSelectedSessionAnalysisButton.Text = "Clear Selected Session Analysis HC";
            this.ClearSelectedSessionAnalysisButton.UseVisualStyleBackColor = true;
            this.ClearSelectedSessionAnalysisButton.Click += new System.EventHandler(this.ClearSelectedSessionAnalysisButton_Click);
            // 
            // AnalyseSelectedSessionsButton
            // 
            this.AnalyseSelectedSessionsButton.Location = new System.Drawing.Point(308, 29);
            this.AnalyseSelectedSessionsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AnalyseSelectedSessionsButton.Name = "AnalyseSelectedSessionsButton";
            this.AnalyseSelectedSessionsButton.Size = new System.Drawing.Size(290, 35);
            this.AnalyseSelectedSessionsButton.TabIndex = 2;
            this.AnalyseSelectedSessionsButton.Text = "Analyse Selected Sessions HC";
            this.AnalyseSelectedSessionsButton.UseVisualStyleBackColor = true;
            this.AnalyseSelectedSessionsButton.Click += new System.EventHandler(this.AnalyseSelectedSessionsButton_Click);
            // 
            // ClearAllSessionAnalysisButton
            // 
            this.ClearAllSessionAnalysisButton.Location = new System.Drawing.Point(9, 74);
            this.ClearAllSessionAnalysisButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ClearAllSessionAnalysisButton.Name = "ClearAllSessionAnalysisButton";
            this.ClearAllSessionAnalysisButton.Size = new System.Drawing.Size(290, 35);
            this.ClearAllSessionAnalysisButton.TabIndex = 1;
            this.ClearAllSessionAnalysisButton.Text = "Clear All Session Analysis HC";
            this.ClearAllSessionAnalysisButton.UseVisualStyleBackColor = true;
            this.ClearAllSessionAnalysisButton.Click += new System.EventHandler(this.ClearAllSessionAnalysisButton_Click);
            // 
            // ExtensionOptionsGroupBox
            // 
            this.ExtensionOptionsGroupBox.Controls.Add(this.ColumnsUIGroupbox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.WhenToAnalyseSessionsGroupBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.ExtensionEnabledCheckBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.DebugModeCheckBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.NeverWebCallCheckBox);
            this.ExtensionOptionsGroupBox.Location = new System.Drawing.Point(4, 5);
            this.ExtensionOptionsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExtensionOptionsGroupBox.Name = "ExtensionOptionsGroupBox";
            this.ExtensionOptionsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExtensionOptionsGroupBox.Size = new System.Drawing.Size(614, 274);
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
            this.ColumnsUIGroupbox.Location = new System.Drawing.Point(10, 65);
            this.ColumnsUIGroupbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ColumnsUIGroupbox.Name = "ColumnsUIGroupbox";
            this.ColumnsUIGroupbox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ColumnsUIGroupbox.Size = new System.Drawing.Size(586, 108);
            this.ColumnsUIGroupbox.TabIndex = 11;
            this.ColumnsUIGroupbox.TabStop = false;
            this.ColumnsUIGroupbox.Text = "Columns Enabled HC";
            // 
            // ElapsedTimeCheckbox
            // 
            this.ElapsedTimeCheckbox.AutoSize = true;
            this.ElapsedTimeCheckbox.Location = new System.Drawing.Point(16, 29);
            this.ElapsedTimeCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ElapsedTimeCheckbox.Name = "ElapsedTimeCheckbox";
            this.ElapsedTimeCheckbox.Size = new System.Drawing.Size(158, 24);
            this.ElapsedTimeCheckbox.TabIndex = 5;
            this.ElapsedTimeCheckbox.Text = "Elapsed Time HC";
            this.ElapsedTimeCheckbox.UseVisualStyleBackColor = true;
            this.ElapsedTimeCheckbox.CheckedChanged += new System.EventHandler(this.ElapsedTimeCheckbox_CheckedChanged);
            // 
            // HostIPCheckbox
            // 
            this.HostIPCheckbox.AutoSize = true;
            this.HostIPCheckbox.Location = new System.Drawing.Point(416, 68);
            this.HostIPCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.HostIPCheckbox.Name = "HostIPCheckbox";
            this.HostIPCheckbox.Size = new System.Drawing.Size(115, 24);
            this.HostIPCheckbox.TabIndex = 4;
            this.HostIPCheckbox.Text = "Host IP HC";
            this.HostIPCheckbox.UseVisualStyleBackColor = true;
            this.HostIPCheckbox.CheckedChanged += new System.EventHandler(this.HostIPCheckbox_CheckedChanged);
            // 
            // ResponseServerCheckbox
            // 
            this.ResponseServerCheckbox.AutoSize = true;
            this.ResponseServerCheckbox.Location = new System.Drawing.Point(207, 68);
            this.ResponseServerCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ResponseServerCheckbox.Name = "ResponseServerCheckbox";
            this.ResponseServerCheckbox.Size = new System.Drawing.Size(185, 24);
            this.ResponseServerCheckbox.TabIndex = 3;
            this.ResponseServerCheckbox.Text = "Response Server HC";
            this.ResponseServerCheckbox.UseVisualStyleBackColor = true;
            this.ResponseServerCheckbox.CheckedChanged += new System.EventHandler(this.ResponseServerCheckbox_CheckedChanged);
            // 
            // AuthenticationCheckbox
            // 
            this.AuthenticationCheckbox.AutoSize = true;
            this.AuthenticationCheckbox.Location = new System.Drawing.Point(416, 29);
            this.AuthenticationCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AuthenticationCheckbox.Name = "AuthenticationCheckbox";
            this.AuthenticationCheckbox.Size = new System.Drawing.Size(165, 24);
            this.AuthenticationCheckbox.TabIndex = 2;
            this.AuthenticationCheckbox.Text = "Authentication HC";
            this.AuthenticationCheckbox.UseVisualStyleBackColor = true;
            this.AuthenticationCheckbox.CheckedChanged += new System.EventHandler(this.AuthenticationCheckbox_CheckedChanged);
            // 
            // SessionTypeCheckbox
            // 
            this.SessionTypeCheckbox.AutoSize = true;
            this.SessionTypeCheckbox.Location = new System.Drawing.Point(207, 29);
            this.SessionTypeCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SessionTypeCheckbox.Name = "SessionTypeCheckbox";
            this.SessionTypeCheckbox.Size = new System.Drawing.Size(157, 24);
            this.SessionTypeCheckbox.TabIndex = 1;
            this.SessionTypeCheckbox.Text = "Session Type HC";
            this.SessionTypeCheckbox.UseVisualStyleBackColor = true;
            this.SessionTypeCheckbox.CheckedChanged += new System.EventHandler(this.SessionTypeCheckbox_CheckedChanged);
            // 
            // SeverityCheckbox
            // 
            this.SeverityCheckbox.AutoSize = true;
            this.SeverityCheckbox.Location = new System.Drawing.Point(16, 68);
            this.SeverityCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SeverityCheckbox.Name = "SeverityCheckbox";
            this.SeverityCheckbox.Size = new System.Drawing.Size(118, 24);
            this.SeverityCheckbox.TabIndex = 0;
            this.SeverityCheckbox.Text = "Severity HC";
            this.SeverityCheckbox.UseVisualStyleBackColor = true;
            this.SeverityCheckbox.CheckedChanged += new System.EventHandler(this.SeverityCheckbox_CheckedChanged);
            // 
            // WhenToAnalyseSessionsGroupBox
            // 
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnImportCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnLiveTraceCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnLoadSazCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Location = new System.Drawing.Point(10, 182);
            this.WhenToAnalyseSessionsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WhenToAnalyseSessionsGroupBox.Name = "WhenToAnalyseSessionsGroupBox";
            this.WhenToAnalyseSessionsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WhenToAnalyseSessionsGroupBox.Size = new System.Drawing.Size(586, 72);
            this.WhenToAnalyseSessionsGroupBox.TabIndex = 7;
            this.WhenToAnalyseSessionsGroupBox.TabStop = false;
            this.WhenToAnalyseSessionsGroupBox.Text = "Choose When To Analyse Sessions HC";
            // 
            // SessionAnalysisOnImportCheckBox
            // 
            this.SessionAnalysisOnImportCheckBox.AutoSize = true;
            this.SessionAnalysisOnImportCheckBox.Location = new System.Drawing.Point(416, 31);
            this.SessionAnalysisOnImportCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SessionAnalysisOnImportCheckBox.Name = "SessionAnalysisOnImportCheckBox";
            this.SessionAnalysisOnImportCheckBox.Size = new System.Drawing.Size(133, 24);
            this.SessionAnalysisOnImportCheckBox.TabIndex = 5;
            this.SessionAnalysisOnImportCheckBox.Text = "On Import HC";
            this.SessionAnalysisOnImportCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnImportCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnImportCheckBox_CheckedChanged);
            // 
            // SessionAnalysisOnLiveTraceCheckBox
            // 
            this.SessionAnalysisOnLiveTraceCheckBox.AutoSize = true;
            this.SessionAnalysisOnLiveTraceCheckBox.Location = new System.Drawing.Point(16, 31);
            this.SessionAnalysisOnLiveTraceCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SessionAnalysisOnLiveTraceCheckBox.Name = "SessionAnalysisOnLiveTraceCheckBox";
            this.SessionAnalysisOnLiveTraceCheckBox.Size = new System.Drawing.Size(159, 24);
            this.SessionAnalysisOnLiveTraceCheckBox.TabIndex = 4;
            this.SessionAnalysisOnLiveTraceCheckBox.Text = "On Live Trace HC";
            this.SessionAnalysisOnLiveTraceCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnLiveTraceCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnLiveTraceCheckBox_CheckedChanged);
            // 
            // SessionAnalysisOnLoadSazCheckBox
            // 
            this.SessionAnalysisOnLoadSazCheckBox.AutoSize = true;
            this.SessionAnalysisOnLoadSazCheckBox.Location = new System.Drawing.Point(207, 31);
            this.SessionAnalysisOnLoadSazCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SessionAnalysisOnLoadSazCheckBox.Name = "SessionAnalysisOnLoadSazCheckBox";
            this.SessionAnalysisOnLoadSazCheckBox.Size = new System.Drawing.Size(155, 24);
            this.SessionAnalysisOnLoadSazCheckBox.TabIndex = 3;
            this.SessionAnalysisOnLoadSazCheckBox.Text = "On Load Saz HC";
            this.SessionAnalysisOnLoadSazCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnLoadSazCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnLoadSazCheckBox_CheckedChanged);
            // 
            // ExtensionEnabledCheckBox
            // 
            this.ExtensionEnabledCheckBox.AutoSize = true;
            this.ExtensionEnabledCheckBox.Location = new System.Drawing.Point(27, 29);
            this.ExtensionEnabledCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExtensionEnabledCheckBox.Name = "ExtensionEnabledCheckBox";
            this.ExtensionEnabledCheckBox.Size = new System.Drawing.Size(195, 24);
            this.ExtensionEnabledCheckBox.TabIndex = 0;
            this.ExtensionEnabledCheckBox.Text = "Extension Enabled HC";
            this.ExtensionEnabledCheckBox.UseVisualStyleBackColor = true;
            this.ExtensionEnabledCheckBox.CheckedChanged += new System.EventHandler(this.ExtensionEnabledCheckBox_CheckedChanged);
            // 
            // DebugModeCheckBox
            // 
            this.DebugModeCheckBox.AutoSize = true;
            this.DebugModeCheckBox.Location = new System.Drawing.Point(218, 29);
            this.DebugModeCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DebugModeCheckBox.Name = "DebugModeCheckBox";
            this.DebugModeCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DebugModeCheckBox.Size = new System.Drawing.Size(127, 24);
            this.DebugModeCheckBox.TabIndex = 8;
            this.DebugModeCheckBox.Text = "Debug Mode";
            this.DebugModeCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DebugModeCheckBox.UseVisualStyleBackColor = true;
            this.DebugModeCheckBox.CheckedChanged += new System.EventHandler(this.DebugModeCheckBox_CheckedChanged);
            // 
            // NeverWebCallCheckBox
            // 
            this.NeverWebCallCheckBox.AutoSize = true;
            this.NeverWebCallCheckBox.Location = new System.Drawing.Point(426, 29);
            this.NeverWebCallCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NeverWebCallCheckBox.Name = "NeverWebCallCheckBox";
            this.NeverWebCallCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NeverWebCallCheckBox.Size = new System.Drawing.Size(143, 24);
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
            this.DebugGroupBox.Location = new System.Drawing.Point(4, 643);
            this.DebugGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DebugGroupBox.Name = "DebugGroupBox";
            this.DebugGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DebugGroupBox.Size = new System.Drawing.Size(614, 666);
            this.DebugGroupBox.TabIndex = 6;
            this.DebugGroupBox.TabStop = false;
            this.DebugGroupBox.Text = "Debug";
            // 
            // ExtensionSettingsTextbox
            // 
            this.ExtensionSettingsTextbox.AcceptsReturn = true;
            this.ExtensionSettingsTextbox.Location = new System.Drawing.Point(10, 29);
            this.ExtensionSettingsTextbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExtensionSettingsTextbox.Multiline = true;
            this.ExtensionSettingsTextbox.Name = "ExtensionSettingsTextbox";
            this.ExtensionSettingsTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ExtensionSettingsTextbox.Size = new System.Drawing.Size(582, 569);
            this.ExtensionSettingsTextbox.TabIndex = 19;
            // 
            // DebugModeUpdateButton
            // 
            this.DebugModeUpdateButton.Location = new System.Drawing.Point(394, 609);
            this.DebugModeUpdateButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DebugModeUpdateButton.Name = "DebugModeUpdateButton";
            this.DebugModeUpdateButton.Size = new System.Drawing.Size(202, 35);
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
            this.ExtensionVersionInformationGroupBox.Location = new System.Drawing.Point(4, 513);
            this.ExtensionVersionInformationGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExtensionVersionInformationGroupBox.Name = "ExtensionVersionInformationGroupBox";
            this.ExtensionVersionInformationGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExtensionVersionInformationGroupBox.Size = new System.Drawing.Size(614, 120);
            this.ExtensionVersionInformationGroupBox.TabIndex = 5;
            this.ExtensionVersionInformationGroupBox.TabStop = false;
            this.ExtensionVersionInformationGroupBox.Text = "Extension Version Information HC";
            // 
            // UpdateLinkLabel
            // 
            this.UpdateLinkLabel.AutoSize = true;
            this.UpdateLinkLabel.Location = new System.Drawing.Point(9, 80);
            this.UpdateLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UpdateLinkLabel.Name = "UpdateLinkLabel";
            this.UpdateLinkLabel.Size = new System.Drawing.Size(130, 20);
            this.UpdateLinkLabel.TabIndex = 4;
            this.UpdateLinkLabel.TabStop = true;
            this.UpdateLinkLabel.Text = "UpdateLinkLabel";
            this.UpdateLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UpdateLinkLabel_LinkClicked);
            // 
            // RulesetVersionLabel
            // 
            this.RulesetVersionLabel.AutoSize = true;
            this.RulesetVersionLabel.Location = new System.Drawing.Point(9, 54);
            this.RulesetVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RulesetVersionLabel.Name = "RulesetVersionLabel";
            this.RulesetVersionLabel.Size = new System.Drawing.Size(149, 20);
            this.RulesetVersionLabel.TabIndex = 2;
            this.RulesetVersionLabel.Text = "Ruleset Version HC";
            // 
            // ExtensionVersionLabel
            // 
            this.ExtensionVersionLabel.AutoSize = true;
            this.ExtensionVersionLabel.Location = new System.Drawing.Point(9, 28);
            this.ExtensionVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ExtensionVersionLabel.Name = "ExtensionVersionLabel";
            this.ExtensionVersionLabel.Size = new System.Drawing.Size(164, 20);
            this.ExtensionVersionLabel.TabIndex = 0;
            this.ExtensionVersionLabel.Text = "Extension Version HC";
            // 
            // Office365TabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DebugGroupBox);
            this.Controls.Add(this.ExtensionVersionInformationGroupBox);
            this.Controls.Add(this.ExtensionOptionsGroupBox);
            this.Controls.Add(this.SessionAnalysisGroupBox);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Office365TabPage";
            this.Size = new System.Drawing.Size(668, 1446);
            this.Load += new System.EventHandler(this.Office365TabPage_Load);
            this.SessionAnalysisGroupBox.ResumeLayout(false);
            this.ExtensionOptionsGroupBox.ResumeLayout(false);
            this.ExtensionOptionsGroupBox.PerformLayout();
            this.ColumnsUIGroupbox.ResumeLayout(false);
            this.ColumnsUIGroupbox.PerformLayout();
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
        private System.Windows.Forms.GroupBox ExtensionOptionsGroupBox;
        private System.Windows.Forms.CheckBox ExtensionEnabledCheckBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLiveTraceCheckBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLoadSazCheckBox;
        private System.Windows.Forms.GroupBox ExtensionVersionInformationGroupBox;
        private System.Windows.Forms.Label ExtensionVersionLabel;
        private System.Windows.Forms.Label RulesetVersionLabel;
        private System.Windows.Forms.LinkLabel UpdateLinkLabel;
        private System.Windows.Forms.CheckBox NeverWebCallCheckBox;
        private System.Windows.Forms.CheckBox DebugModeCheckBox;
        private System.Windows.Forms.GroupBox DebugGroupBox;
        private System.Windows.Forms.GroupBox WhenToAnalyseSessionsGroupBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnImportCheckBox;
        private System.Windows.Forms.Button DebugModeUpdateButton;
        private System.Windows.Forms.GroupBox ColumnsUIGroupbox;
        private System.Windows.Forms.CheckBox SessionTypeCheckbox;
        private System.Windows.Forms.CheckBox SeverityCheckbox;
        private System.Windows.Forms.CheckBox HostIPCheckbox;
        private System.Windows.Forms.CheckBox ResponseServerCheckbox;
        private System.Windows.Forms.CheckBox AuthenticationCheckbox;
        private System.Windows.Forms.CheckBox ElapsedTimeCheckbox;
        private System.Windows.Forms.TextBox ExtensionSettingsTextbox;
        private System.Windows.Forms.Button StopSessionAnalysisButton;
    }
}