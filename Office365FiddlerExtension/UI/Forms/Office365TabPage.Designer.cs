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
            this.CreateConsolidatedAnalysisButton = new System.Windows.Forms.Button();
            this.ClearSelectedSessionAnalysisButton = new System.Windows.Forms.Button();
            this.AnalyseSelectedSessionsButton = new System.Windows.Forms.Button();
            this.ClearAllSessionAnalysisButton = new System.Windows.Forms.Button();
            this.CheckIPAddressGroupBox = new System.Windows.Forms.GroupBox();
            this.WhoisCheckBox = new System.Windows.Forms.CheckBox();
            this.CheckIPAddressClearButton = new System.Windows.Forms.Button();
            this.CheckIPAddressResultTextBox = new System.Windows.Forms.TextBox();
            this.CheckIPAddressButton = new System.Windows.Forms.Button();
            this.EnterIPAddressTextBox = new System.Windows.Forms.TextBox();
            this.ExtensionOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.WarnBeforeProcessingGroupBox = new System.Windows.Forms.GroupBox();
            this.WarnBeforeProcessingSessionsLabel = new System.Windows.Forms.Label();
            this.WarnBeforeAnalysingTextBox = new System.Windows.Forms.TextBox();
            this.WhenToAnalyseSessionsGroupBox = new System.Windows.Forms.GroupBox();
            this.NeverRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionAnalysisOnImportCheckBox = new System.Windows.Forms.CheckBox();
            this.AlwaysSessionAnalysisRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionAnalysisOnLiveTraceCheckBox = new System.Windows.Forms.CheckBox();
            this.SelectiveSessionAnalysisRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionAnalysisOnLoadSazCheckBox = new System.Windows.Forms.CheckBox();
            this.CaptureTrafficCheckBox = new System.Windows.Forms.CheckBox();
            this.ExtensionEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.DebugGroupBox = new System.Windows.Forms.GroupBox();
            this.DebugModeUpdateButton = new System.Windows.Forms.Button();
            this.DebugModeExtensionPathLabel = new System.Windows.Forms.Label();
            this.DebugModeExtensionPathTextbox = new System.Windows.Forms.TextBox();
            this.DebugModeExtensionEnabledLabel = new System.Windows.Forms.Label();
            this.DebugModeExtensionEnabledTextbox = new System.Windows.Forms.TextBox();
            this.DebugModeLanguageTextBox = new System.Windows.Forms.TextBox();
            this.DebugModeNextUpdateCheckLabel = new System.Windows.Forms.Label();
            this.DebugModeNextUpdateCheckTextBox = new System.Windows.Forms.TextBox();
            this.DebugModeExecutionCountLabel = new System.Windows.Forms.Label();
            this.DebugModeLanguageLabel = new System.Windows.Forms.Label();
            this.DebugModeNeverWebCallCheckBox = new System.Windows.Forms.CheckBox();
            this.DebugModeCheckBox = new System.Windows.Forms.CheckBox();
            this.DebugModeExecutionCountTextBox = new System.Windows.Forms.TextBox();
            this.ExtensionVersionInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.UpdateLinkLabel = new System.Windows.Forms.LinkLabel();
            this.RulesetVersionLabel = new System.Windows.Forms.Label();
            this.ExtensionVersionLabel = new System.Windows.Forms.Label();
            this.DebugModeUpgradeCheck = new System.Windows.Forms.Button();
            this.SessionAnalysisGroupBox.SuspendLayout();
            this.CheckIPAddressGroupBox.SuspendLayout();
            this.ExtensionOptionsGroupBox.SuspendLayout();
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
            this.SessionAnalysisGroupBox.Location = new System.Drawing.Point(3, 215);
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
            this.CheckIPAddressGroupBox.Controls.Add(this.WhoisCheckBox);
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressClearButton);
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressResultTextBox);
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressButton);
            this.CheckIPAddressGroupBox.Controls.Add(this.EnterIPAddressTextBox);
            this.CheckIPAddressGroupBox.Location = new System.Drawing.Point(3, 333);
            this.CheckIPAddressGroupBox.Name = "CheckIPAddressGroupBox";
            this.CheckIPAddressGroupBox.Size = new System.Drawing.Size(409, 115);
            this.CheckIPAddressGroupBox.TabIndex = 3;
            this.CheckIPAddressGroupBox.TabStop = false;
            this.CheckIPAddressGroupBox.Text = "Check IP Address HC";
            // 
            // WhoisCheckBox
            // 
            this.WhoisCheckBox.AutoSize = true;
            this.WhoisCheckBox.Location = new System.Drawing.Point(324, 78);
            this.WhoisCheckBox.Name = "WhoisCheckBox";
            this.WhoisCheckBox.Size = new System.Drawing.Size(56, 17);
            this.WhoisCheckBox.TabIndex = 4;
            this.WhoisCheckBox.Text = "Whois";
            this.WhoisCheckBox.UseVisualStyleBackColor = true;
            this.WhoisCheckBox.CheckedChanged += new System.EventHandler(this.WhoisCheckBox_CheckedChanged);
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
            this.ExtensionOptionsGroupBox.Controls.Add(this.WarnBeforeProcessingGroupBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.WhenToAnalyseSessionsGroupBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.CaptureTrafficCheckBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.ExtensionEnabledCheckBox);
            this.ExtensionOptionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ExtensionOptionsGroupBox.Name = "ExtensionOptionsGroupBox";
            this.ExtensionOptionsGroupBox.Size = new System.Drawing.Size(409, 206);
            this.ExtensionOptionsGroupBox.TabIndex = 4;
            this.ExtensionOptionsGroupBox.TabStop = false;
            this.ExtensionOptionsGroupBox.Text = "Extension Options HC";
            // 
            // WarnBeforeProcessingGroupBox
            // 
            this.WarnBeforeProcessingGroupBox.Controls.Add(this.WarnBeforeProcessingSessionsLabel);
            this.WarnBeforeProcessingGroupBox.Controls.Add(this.WarnBeforeAnalysingTextBox);
            this.WarnBeforeProcessingGroupBox.Location = new System.Drawing.Point(7, 141);
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
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.NeverRadioButton);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnImportCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.AlwaysSessionAnalysisRadioButton);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnLiveTraceCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SelectiveSessionAnalysisRadioButton);
            this.WhenToAnalyseSessionsGroupBox.Controls.Add(this.SessionAnalysisOnLoadSazCheckBox);
            this.WhenToAnalyseSessionsGroupBox.Location = new System.Drawing.Point(7, 42);
            this.WhenToAnalyseSessionsGroupBox.Name = "WhenToAnalyseSessionsGroupBox";
            this.WhenToAnalyseSessionsGroupBox.Size = new System.Drawing.Size(391, 92);
            this.WhenToAnalyseSessionsGroupBox.TabIndex = 7;
            this.WhenToAnalyseSessionsGroupBox.TabStop = false;
            this.WhenToAnalyseSessionsGroupBox.Text = "Choose When To Analyse Sessions HC";
            // 
            // NeverRadioButton
            // 
            this.NeverRadioButton.AutoSize = true;
            this.NeverRadioButton.Location = new System.Drawing.Point(11, 65);
            this.NeverRadioButton.Name = "NeverRadioButton";
            this.NeverRadioButton.Size = new System.Drawing.Size(72, 17);
            this.NeverRadioButton.TabIndex = 6;
            this.NeverRadioButton.TabStop = true;
            this.NeverRadioButton.Text = "Never HC";
            this.NeverRadioButton.UseVisualStyleBackColor = true;
            this.NeverRadioButton.CheckedChanged += new System.EventHandler(this.NeverRadioButton_CheckedChanged);
            // 
            // SessionAnalysisOnImportCheckBox
            // 
            this.SessionAnalysisOnImportCheckBox.AutoSize = true;
            this.SessionAnalysisOnImportCheckBox.Location = new System.Drawing.Point(138, 66);
            this.SessionAnalysisOnImportCheckBox.Name = "SessionAnalysisOnImportCheckBox";
            this.SessionAnalysisOnImportCheckBox.Size = new System.Drawing.Size(90, 17);
            this.SessionAnalysisOnImportCheckBox.TabIndex = 5;
            this.SessionAnalysisOnImportCheckBox.Text = "On Import HC";
            this.SessionAnalysisOnImportCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnImportCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnImportCheckBox_CheckedChanged);
            // 
            // AlwaysSessionAnalysisRadioButton
            // 
            this.AlwaysSessionAnalysisRadioButton.AutoSize = true;
            this.AlwaysSessionAnalysisRadioButton.Location = new System.Drawing.Point(11, 19);
            this.AlwaysSessionAnalysisRadioButton.Name = "AlwaysSessionAnalysisRadioButton";
            this.AlwaysSessionAnalysisRadioButton.Size = new System.Drawing.Size(76, 17);
            this.AlwaysSessionAnalysisRadioButton.TabIndex = 1;
            this.AlwaysSessionAnalysisRadioButton.TabStop = true;
            this.AlwaysSessionAnalysisRadioButton.Text = "Always HC";
            this.AlwaysSessionAnalysisRadioButton.UseVisualStyleBackColor = true;
            this.AlwaysSessionAnalysisRadioButton.CheckedChanged += new System.EventHandler(this.AllSessionAnalysisRadioButton_CheckedChanged);
            // 
            // SessionAnalysisOnLiveTraceCheckBox
            // 
            this.SessionAnalysisOnLiveTraceCheckBox.AutoSize = true;
            this.SessionAnalysisOnLiveTraceCheckBox.Location = new System.Drawing.Point(138, 43);
            this.SessionAnalysisOnLiveTraceCheckBox.Name = "SessionAnalysisOnLiveTraceCheckBox";
            this.SessionAnalysisOnLiveTraceCheckBox.Size = new System.Drawing.Size(112, 17);
            this.SessionAnalysisOnLiveTraceCheckBox.TabIndex = 4;
            this.SessionAnalysisOnLiveTraceCheckBox.Text = "On Live Trace HC";
            this.SessionAnalysisOnLiveTraceCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnLiveTraceCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnLiveTraceCheckBox_CheckedChanged);
            // 
            // SelectiveSessionAnalysisRadioButton
            // 
            this.SelectiveSessionAnalysisRadioButton.AutoSize = true;
            this.SelectiveSessionAnalysisRadioButton.Location = new System.Drawing.Point(11, 42);
            this.SelectiveSessionAnalysisRadioButton.Name = "SelectiveSessionAnalysisRadioButton";
            this.SelectiveSessionAnalysisRadioButton.Size = new System.Drawing.Size(87, 17);
            this.SelectiveSessionAnalysisRadioButton.TabIndex = 2;
            this.SelectiveSessionAnalysisRadioButton.TabStop = true;
            this.SelectiveSessionAnalysisRadioButton.Text = "Selective HC";
            this.SelectiveSessionAnalysisRadioButton.UseVisualStyleBackColor = true;
            this.SelectiveSessionAnalysisRadioButton.CheckedChanged += new System.EventHandler(this.SelectiveSessionAnalysisRadioButton_CheckedChanged);
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
            // CaptureTrafficCheckBox
            // 
            this.CaptureTrafficCheckBox.AutoSize = true;
            this.CaptureTrafficCheckBox.Location = new System.Drawing.Point(284, 19);
            this.CaptureTrafficCheckBox.Name = "CaptureTrafficCheckBox";
            this.CaptureTrafficCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CaptureTrafficCheckBox.Size = new System.Drawing.Size(114, 17);
            this.CaptureTrafficCheckBox.TabIndex = 9;
            this.CaptureTrafficCheckBox.Text = "Capture Traffic HC";
            this.CaptureTrafficCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CaptureTrafficCheckBox.UseVisualStyleBackColor = true;
            this.CaptureTrafficCheckBox.Visible = false;
            this.CaptureTrafficCheckBox.CheckedChanged += new System.EventHandler(this.CaptureTrafficCheckBox_CheckedChanged);
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
            // DebugGroupBox
            // 
            this.DebugGroupBox.Controls.Add(this.DebugModeUpgradeCheck);
            this.DebugGroupBox.Controls.Add(this.DebugModeUpdateButton);
            this.DebugGroupBox.Controls.Add(this.DebugModeExtensionPathLabel);
            this.DebugGroupBox.Controls.Add(this.DebugModeExtensionPathTextbox);
            this.DebugGroupBox.Controls.Add(this.DebugModeExtensionEnabledLabel);
            this.DebugGroupBox.Controls.Add(this.DebugModeExtensionEnabledTextbox);
            this.DebugGroupBox.Controls.Add(this.DebugModeLanguageTextBox);
            this.DebugGroupBox.Controls.Add(this.DebugModeNextUpdateCheckLabel);
            this.DebugGroupBox.Controls.Add(this.DebugModeNextUpdateCheckTextBox);
            this.DebugGroupBox.Controls.Add(this.DebugModeExecutionCountLabel);
            this.DebugGroupBox.Controls.Add(this.DebugModeLanguageLabel);
            this.DebugGroupBox.Controls.Add(this.DebugModeNeverWebCallCheckBox);
            this.DebugGroupBox.Controls.Add(this.DebugModeCheckBox);
            this.DebugGroupBox.Controls.Add(this.DebugModeExecutionCountTextBox);
            this.DebugGroupBox.Location = new System.Drawing.Point(3, 538);
            this.DebugGroupBox.Name = "DebugGroupBox";
            this.DebugGroupBox.Size = new System.Drawing.Size(409, 183);
            this.DebugGroupBox.TabIndex = 6;
            this.DebugGroupBox.TabStop = false;
            this.DebugGroupBox.Text = "Debug";
            // 
            // DebugModeUpdateButton
            // 
            this.DebugModeUpdateButton.Location = new System.Drawing.Point(263, 148);
            this.DebugModeUpdateButton.Name = "DebugModeUpdateButton";
            this.DebugModeUpdateButton.Size = new System.Drawing.Size(135, 23);
            this.DebugModeUpdateButton.TabIndex = 17;
            this.DebugModeUpdateButton.Text = "Update Debug Page";
            this.DebugModeUpdateButton.UseVisualStyleBackColor = true;
            this.DebugModeUpdateButton.Click += new System.EventHandler(this.DebugModeUpdateButton_Click);
            // 
            // DebugModeExtensionPathLabel
            // 
            this.DebugModeExtensionPathLabel.AutoSize = true;
            this.DebugModeExtensionPathLabel.Location = new System.Drawing.Point(6, 125);
            this.DebugModeExtensionPathLabel.Name = "DebugModeExtensionPathLabel";
            this.DebugModeExtensionPathLabel.Size = new System.Drawing.Size(78, 13);
            this.DebugModeExtensionPathLabel.TabIndex = 16;
            this.DebugModeExtensionPathLabel.Text = "Extension Path";
            // 
            // DebugModeExtensionPathTextbox
            // 
            this.DebugModeExtensionPathTextbox.Location = new System.Drawing.Point(145, 122);
            this.DebugModeExtensionPathTextbox.Name = "DebugModeExtensionPathTextbox";
            this.DebugModeExtensionPathTextbox.Size = new System.Drawing.Size(253, 20);
            this.DebugModeExtensionPathTextbox.TabIndex = 15;
            // 
            // DebugModeExtensionEnabledLabel
            // 
            this.DebugModeExtensionEnabledLabel.AutoSize = true;
            this.DebugModeExtensionEnabledLabel.Location = new System.Drawing.Point(6, 99);
            this.DebugModeExtensionEnabledLabel.Name = "DebugModeExtensionEnabledLabel";
            this.DebugModeExtensionEnabledLabel.Size = new System.Drawing.Size(95, 13);
            this.DebugModeExtensionEnabledLabel.TabIndex = 14;
            this.DebugModeExtensionEnabledLabel.Text = "Extension Enabled";
            // 
            // DebugModeExtensionEnabledTextbox
            // 
            this.DebugModeExtensionEnabledTextbox.Location = new System.Drawing.Point(298, 96);
            this.DebugModeExtensionEnabledTextbox.Name = "DebugModeExtensionEnabledTextbox";
            this.DebugModeExtensionEnabledTextbox.Size = new System.Drawing.Size(100, 20);
            this.DebugModeExtensionEnabledTextbox.TabIndex = 13;
            // 
            // DebugModeLanguageTextBox
            // 
            this.DebugModeLanguageTextBox.BackColor = System.Drawing.Color.White;
            this.DebugModeLanguageTextBox.Location = new System.Drawing.Point(298, 69);
            this.DebugModeLanguageTextBox.Name = "DebugModeLanguageTextBox";
            this.DebugModeLanguageTextBox.ReadOnly = true;
            this.DebugModeLanguageTextBox.Size = new System.Drawing.Size(100, 20);
            this.DebugModeLanguageTextBox.TabIndex = 5;
            // 
            // DebugModeNextUpdateCheckLabel
            // 
            this.DebugModeNextUpdateCheckLabel.AutoSize = true;
            this.DebugModeNextUpdateCheckLabel.Location = new System.Drawing.Point(125, 47);
            this.DebugModeNextUpdateCheckLabel.Name = "DebugModeNextUpdateCheckLabel";
            this.DebugModeNextUpdateCheckLabel.Size = new System.Drawing.Size(101, 13);
            this.DebugModeNextUpdateCheckLabel.TabIndex = 12;
            this.DebugModeNextUpdateCheckLabel.Text = "Next Update Check";
            // 
            // DebugModeNextUpdateCheckTextBox
            // 
            this.DebugModeNextUpdateCheckTextBox.Location = new System.Drawing.Point(232, 43);
            this.DebugModeNextUpdateCheckTextBox.Name = "DebugModeNextUpdateCheckTextBox";
            this.DebugModeNextUpdateCheckTextBox.Size = new System.Drawing.Size(166, 20);
            this.DebugModeNextUpdateCheckTextBox.TabIndex = 11;
            // 
            // DebugModeExecutionCountLabel
            // 
            this.DebugModeExecutionCountLabel.AutoSize = true;
            this.DebugModeExecutionCountLabel.Location = new System.Drawing.Point(142, 21);
            this.DebugModeExecutionCountLabel.Name = "DebugModeExecutionCountLabel";
            this.DebugModeExecutionCountLabel.Size = new System.Drawing.Size(85, 13);
            this.DebugModeExecutionCountLabel.TabIndex = 10;
            this.DebugModeExecutionCountLabel.Text = "Execution Count";
            // 
            // DebugModeLanguageLabel
            // 
            this.DebugModeLanguageLabel.AutoSize = true;
            this.DebugModeLanguageLabel.Location = new System.Drawing.Point(6, 72);
            this.DebugModeLanguageLabel.Name = "DebugModeLanguageLabel";
            this.DebugModeLanguageLabel.Size = new System.Drawing.Size(101, 13);
            this.DebugModeLanguageLabel.TabIndex = 6;
            this.DebugModeLanguageLabel.Text = "Preferred Language";
            this.DebugModeLanguageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DebugModeNeverWebCallCheckBox
            // 
            this.DebugModeNeverWebCallCheckBox.AutoSize = true;
            this.DebugModeNeverWebCallCheckBox.Location = new System.Drawing.Point(7, 46);
            this.DebugModeNeverWebCallCheckBox.Name = "DebugModeNeverWebCallCheckBox";
            this.DebugModeNeverWebCallCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DebugModeNeverWebCallCheckBox.Size = new System.Drawing.Size(101, 17);
            this.DebugModeNeverWebCallCheckBox.TabIndex = 7;
            this.DebugModeNeverWebCallCheckBox.Text = "Never Web Call";
            this.DebugModeNeverWebCallCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DebugModeNeverWebCallCheckBox.UseVisualStyleBackColor = true;
            this.DebugModeNeverWebCallCheckBox.CheckedChanged += new System.EventHandler(this.NeverWebCallCheckBox_CheckedChanged);
            // 
            // DebugModeCheckBox
            // 
            this.DebugModeCheckBox.AutoSize = true;
            this.DebugModeCheckBox.Location = new System.Drawing.Point(7, 20);
            this.DebugModeCheckBox.Name = "DebugModeCheckBox";
            this.DebugModeCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DebugModeCheckBox.Size = new System.Drawing.Size(88, 17);
            this.DebugModeCheckBox.TabIndex = 8;
            this.DebugModeCheckBox.Text = "Debug Mode";
            this.DebugModeCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DebugModeCheckBox.UseVisualStyleBackColor = true;
            this.DebugModeCheckBox.CheckedChanged += new System.EventHandler(this.DebugModeCheckBox_CheckedChanged);
            // 
            // DebugModeExecutionCountTextBox
            // 
            this.DebugModeExecutionCountTextBox.Location = new System.Drawing.Point(233, 17);
            this.DebugModeExecutionCountTextBox.Name = "DebugModeExecutionCountTextBox";
            this.DebugModeExecutionCountTextBox.Size = new System.Drawing.Size(166, 20);
            this.DebugModeExecutionCountTextBox.TabIndex = 9;
            // 
            // ExtensionVersionInformationGroupBox
            // 
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.UpdateLinkLabel);
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.RulesetVersionLabel);
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.ExtensionVersionLabel);
            this.ExtensionVersionInformationGroupBox.Location = new System.Drawing.Point(3, 454);
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
            // DebugModeUpgradeCheck
            // 
            this.DebugModeUpgradeCheck.Location = new System.Drawing.Point(99, 148);
            this.DebugModeUpgradeCheck.Name = "DebugModeUpgradeCheck";
            this.DebugModeUpgradeCheck.Size = new System.Drawing.Size(158, 23);
            this.DebugModeUpgradeCheck.TabIndex = 18;
            this.DebugModeUpgradeCheck.Text = "Extension Upgrade Check";
            this.DebugModeUpgradeCheck.UseVisualStyleBackColor = true;
            this.DebugModeUpgradeCheck.Click += new System.EventHandler(this.DebugModeUpgradeCheck_Click);
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
            this.Size = new System.Drawing.Size(655, 797);
            this.Load += new System.EventHandler(this.Office365TabPage_Load);
            this.SessionAnalysisGroupBox.ResumeLayout(false);
            this.CheckIPAddressGroupBox.ResumeLayout(false);
            this.CheckIPAddressGroupBox.PerformLayout();
            this.ExtensionOptionsGroupBox.ResumeLayout(false);
            this.ExtensionOptionsGroupBox.PerformLayout();
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
        private System.Windows.Forms.RadioButton SelectiveSessionAnalysisRadioButton;
        private System.Windows.Forms.RadioButton AlwaysSessionAnalysisRadioButton;
        private System.Windows.Forms.Label DebugModeLanguageLabel;
        private System.Windows.Forms.TextBox DebugModeLanguageTextBox;
        private System.Windows.Forms.Button CheckIPAddressClearButton;
        private System.Windows.Forms.GroupBox ExtensionVersionInformationGroupBox;
        private System.Windows.Forms.Label ExtensionVersionLabel;
        private System.Windows.Forms.Label RulesetVersionLabel;
        private System.Windows.Forms.LinkLabel UpdateLinkLabel;
        private System.Windows.Forms.CheckBox DebugModeNeverWebCallCheckBox;
        private System.Windows.Forms.CheckBox DebugModeCheckBox;
        private System.Windows.Forms.CheckBox CaptureTrafficCheckBox;
        private System.Windows.Forms.GroupBox DebugGroupBox;
        private System.Windows.Forms.TextBox DebugModeExecutionCountTextBox;
        private System.Windows.Forms.Label DebugModeExecutionCountLabel;
        private System.Windows.Forms.TextBox DebugModeNextUpdateCheckTextBox;
        private System.Windows.Forms.Label DebugModeNextUpdateCheckLabel;
        private System.Windows.Forms.GroupBox WhenToAnalyseSessionsGroupBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnImportCheckBox;
        private System.Windows.Forms.RadioButton NeverRadioButton;
        private System.Windows.Forms.GroupBox WarnBeforeProcessingGroupBox;
        private System.Windows.Forms.Label WarnBeforeProcessingSessionsLabel;
        private System.Windows.Forms.TextBox WarnBeforeAnalysingTextBox;
        private System.Windows.Forms.CheckBox WhoisCheckBox;
        private System.Windows.Forms.Label DebugModeExtensionEnabledLabel;
        private System.Windows.Forms.TextBox DebugModeExtensionEnabledTextbox;
        private System.Windows.Forms.Label DebugModeExtensionPathLabel;
        private System.Windows.Forms.TextBox DebugModeExtensionPathTextbox;
        private System.Windows.Forms.Button DebugModeUpdateButton;
        private System.Windows.Forms.Button DebugModeUpgradeCheck;
    }
}
