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
            this.ClearSelectedSessionAnalysisButton = new System.Windows.Forms.Button();
            this.AnalyseSelectedSessionsButton = new System.Windows.Forms.Button();
            this.ClearAllSessionAnalysisButton = new System.Windows.Forms.Button();
            this.ConsolidatedAnalysisGroupBox = new System.Windows.Forms.GroupBox();
            this.OpenLatestConsolidatedAnalysisReportButton = new System.Windows.Forms.Button();
            this.CreateConsolidatedAnalysisButton = new System.Windows.Forms.Button();
            this.CheckIPAddressGroupBox = new System.Windows.Forms.GroupBox();
            this.CheckIPAddressClearButton = new System.Windows.Forms.Button();
            this.CheckIPAddressResultTextBox = new System.Windows.Forms.TextBox();
            this.CheckIPAddressButton = new System.Windows.Forms.Button();
            this.EnterIPAddressTextBox = new System.Windows.Forms.TextBox();
            this.ExtensionOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.LanguageLabel = new System.Windows.Forms.Label();
            this.LanguageTextBox = new System.Windows.Forms.TextBox();
            this.SessionAnalysisOnLiveTraceCheckBox = new System.Windows.Forms.CheckBox();
            this.SessionAnalysisOnLoadSazCheckBox = new System.Windows.Forms.CheckBox();
            this.SomeSessionAnalysisRadioButton = new System.Windows.Forms.RadioButton();
            this.AllSessionAnalysisRadioButton = new System.Windows.Forms.RadioButton();
            this.ExtensionEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.ExtensionVersionInformationGroupBox = new System.Windows.Forms.GroupBox();
            this.UpdateLinkLabel = new System.Windows.Forms.LinkLabel();
            this.RulesetVersionLabel = new System.Windows.Forms.Label();
            this.ExtensionVersionLabel = new System.Windows.Forms.Label();
            this.SessionAnalysisGroupBox.SuspendLayout();
            this.ConsolidatedAnalysisGroupBox.SuspendLayout();
            this.CheckIPAddressGroupBox.SuspendLayout();
            this.ExtensionOptionsGroupBox.SuspendLayout();
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
            this.SessionAnalysisGroupBox.Controls.Add(this.ClearSelectedSessionAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.AnalyseSelectedSessionsButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.ClearAllSessionAnalysisButton);
            this.SessionAnalysisGroupBox.Controls.Add(this.AnalyseAllSessionsButton);
            this.SessionAnalysisGroupBox.Location = new System.Drawing.Point(3, 151);
            this.SessionAnalysisGroupBox.Name = "SessionAnalysisGroupBox";
            this.SessionAnalysisGroupBox.Size = new System.Drawing.Size(409, 84);
            this.SessionAnalysisGroupBox.TabIndex = 1;
            this.SessionAnalysisGroupBox.TabStop = false;
            this.SessionAnalysisGroupBox.Text = "Session Analysis HC";
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
            // ConsolidatedAnalysisGroupBox
            // 
            this.ConsolidatedAnalysisGroupBox.Controls.Add(this.OpenLatestConsolidatedAnalysisReportButton);
            this.ConsolidatedAnalysisGroupBox.Controls.Add(this.CreateConsolidatedAnalysisButton);
            this.ConsolidatedAnalysisGroupBox.Location = new System.Drawing.Point(3, 241);
            this.ConsolidatedAnalysisGroupBox.Name = "ConsolidatedAnalysisGroupBox";
            this.ConsolidatedAnalysisGroupBox.Size = new System.Drawing.Size(409, 84);
            this.ConsolidatedAnalysisGroupBox.TabIndex = 2;
            this.ConsolidatedAnalysisGroupBox.TabStop = false;
            this.ConsolidatedAnalysisGroupBox.Text = "Consolidated Analysis Report HC";
            // 
            // OpenLatestConsolidatedAnalysisReportButton
            // 
            this.OpenLatestConsolidatedAnalysisReportButton.Location = new System.Drawing.Point(6, 48);
            this.OpenLatestConsolidatedAnalysisReportButton.Name = "OpenLatestConsolidatedAnalysisReportButton";
            this.OpenLatestConsolidatedAnalysisReportButton.Size = new System.Drawing.Size(392, 23);
            this.OpenLatestConsolidatedAnalysisReportButton.TabIndex = 4;
            this.OpenLatestConsolidatedAnalysisReportButton.Text = "Open Latest Consolidated Analysis Report HC";
            this.OpenLatestConsolidatedAnalysisReportButton.UseVisualStyleBackColor = true;
            this.OpenLatestConsolidatedAnalysisReportButton.Click += new System.EventHandler(this.OpenLatestConsolidatedAnalysisReportButton_Click);
            // 
            // CreateConsolidatedAnalysisButton
            // 
            this.CreateConsolidatedAnalysisButton.Location = new System.Drawing.Point(6, 19);
            this.CreateConsolidatedAnalysisButton.Name = "CreateConsolidatedAnalysisButton";
            this.CreateConsolidatedAnalysisButton.Size = new System.Drawing.Size(392, 23);
            this.CreateConsolidatedAnalysisButton.TabIndex = 3;
            this.CreateConsolidatedAnalysisButton.Text = "Create Consolidated Analysis Report HC";
            this.CreateConsolidatedAnalysisButton.UseVisualStyleBackColor = true;
            this.CreateConsolidatedAnalysisButton.Click += new System.EventHandler(this.CreateConsolidatedAnalysisButton_Click);
            // 
            // CheckIPAddressGroupBox
            // 
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressClearButton);
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressResultTextBox);
            this.CheckIPAddressGroupBox.Controls.Add(this.CheckIPAddressButton);
            this.CheckIPAddressGroupBox.Controls.Add(this.EnterIPAddressTextBox);
            this.CheckIPAddressGroupBox.Location = new System.Drawing.Point(3, 331);
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
            this.ExtensionOptionsGroupBox.Controls.Add(this.LanguageLabel);
            this.ExtensionOptionsGroupBox.Controls.Add(this.LanguageTextBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.SessionAnalysisOnLiveTraceCheckBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.SessionAnalysisOnLoadSazCheckBox);
            this.ExtensionOptionsGroupBox.Controls.Add(this.SomeSessionAnalysisRadioButton);
            this.ExtensionOptionsGroupBox.Controls.Add(this.AllSessionAnalysisRadioButton);
            this.ExtensionOptionsGroupBox.Controls.Add(this.ExtensionEnabledCheckBox);
            this.ExtensionOptionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ExtensionOptionsGroupBox.Name = "ExtensionOptionsGroupBox";
            this.ExtensionOptionsGroupBox.Size = new System.Drawing.Size(409, 142);
            this.ExtensionOptionsGroupBox.TabIndex = 4;
            this.ExtensionOptionsGroupBox.TabStop = false;
            this.ExtensionOptionsGroupBox.Text = "Extension Options HC";
            // 
            // LanguageLabel
            // 
            this.LanguageLabel.AutoSize = true;
            this.LanguageLabel.Location = new System.Drawing.Point(284, 115);
            this.LanguageLabel.Name = "LanguageLabel";
            this.LanguageLabel.Size = new System.Drawing.Size(73, 13);
            this.LanguageLabel.TabIndex = 6;
            this.LanguageLabel.Text = "Language HC";
            this.LanguageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LanguageTextBox
            // 
            this.LanguageTextBox.BackColor = System.Drawing.Color.White;
            this.LanguageTextBox.Location = new System.Drawing.Point(363, 112);
            this.LanguageTextBox.Name = "LanguageTextBox";
            this.LanguageTextBox.ReadOnly = true;
            this.LanguageTextBox.Size = new System.Drawing.Size(35, 20);
            this.LanguageTextBox.TabIndex = 5;
            // 
            // SessionAnalysisOnLiveTraceCheckBox
            // 
            this.SessionAnalysisOnLiveTraceCheckBox.AutoSize = true;
            this.SessionAnalysisOnLiveTraceCheckBox.Location = new System.Drawing.Point(31, 114);
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
            this.SessionAnalysisOnLoadSazCheckBox.Location = new System.Drawing.Point(31, 91);
            this.SessionAnalysisOnLoadSazCheckBox.Name = "SessionAnalysisOnLoadSazCheckBox";
            this.SessionAnalysisOnLoadSazCheckBox.Size = new System.Drawing.Size(106, 17);
            this.SessionAnalysisOnLoadSazCheckBox.TabIndex = 3;
            this.SessionAnalysisOnLoadSazCheckBox.Text = "On Load Saz HC";
            this.SessionAnalysisOnLoadSazCheckBox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnLoadSazCheckBox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnLoadSazCheckBox_CheckedChanged);
            // 
            // SomeSessionAnalysisRadioButton
            // 
            this.SomeSessionAnalysisRadioButton.AutoSize = true;
            this.SomeSessionAnalysisRadioButton.Location = new System.Drawing.Point(7, 67);
            this.SomeSessionAnalysisRadioButton.Name = "SomeSessionAnalysisRadioButton";
            this.SomeSessionAnalysisRadioButton.Size = new System.Drawing.Size(151, 17);
            this.SomeSessionAnalysisRadioButton.TabIndex = 2;
            this.SomeSessionAnalysisRadioButton.TabStop = true;
            this.SomeSessionAnalysisRadioButton.Text = "Some Session Analysis HC";
            this.SomeSessionAnalysisRadioButton.UseVisualStyleBackColor = true;
            this.SomeSessionAnalysisRadioButton.CheckedChanged += new System.EventHandler(this.SomeSessionAnalysisRadioButton_CheckedChanged);
            // 
            // AllSessionAnalysisRadioButton
            // 
            this.AllSessionAnalysisRadioButton.AutoSize = true;
            this.AllSessionAnalysisRadioButton.Location = new System.Drawing.Point(7, 44);
            this.AllSessionAnalysisRadioButton.Name = "AllSessionAnalysisRadioButton";
            this.AllSessionAnalysisRadioButton.Size = new System.Drawing.Size(135, 17);
            this.AllSessionAnalysisRadioButton.TabIndex = 1;
            this.AllSessionAnalysisRadioButton.TabStop = true;
            this.AllSessionAnalysisRadioButton.Text = "All Session Analysis HC";
            this.AllSessionAnalysisRadioButton.UseVisualStyleBackColor = true;
            this.AllSessionAnalysisRadioButton.CheckedChanged += new System.EventHandler(this.AllSessionAnalysisRadioButton_CheckedChanged);
            // 
            // ExtensionEnabledCheckBox
            // 
            this.ExtensionEnabledCheckBox.AutoSize = true;
            this.ExtensionEnabledCheckBox.Location = new System.Drawing.Point(7, 20);
            this.ExtensionEnabledCheckBox.Name = "ExtensionEnabledCheckBox";
            this.ExtensionEnabledCheckBox.Size = new System.Drawing.Size(132, 17);
            this.ExtensionEnabledCheckBox.TabIndex = 0;
            this.ExtensionEnabledCheckBox.Text = "Extension Enabled HC";
            this.ExtensionEnabledCheckBox.UseVisualStyleBackColor = true;
            this.ExtensionEnabledCheckBox.CheckedChanged += new System.EventHandler(this.ExtensionEnabledCheckBox_CheckedChanged);
            // 
            // ExtensionVersionInformationGroupBox
            // 
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.UpdateLinkLabel);
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.RulesetVersionLabel);
            this.ExtensionVersionInformationGroupBox.Controls.Add(this.ExtensionVersionLabel);
            this.ExtensionVersionInformationGroupBox.Location = new System.Drawing.Point(3, 452);
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
            this.Controls.Add(this.ExtensionVersionInformationGroupBox);
            this.Controls.Add(this.ExtensionOptionsGroupBox);
            this.Controls.Add(this.CheckIPAddressGroupBox);
            this.Controls.Add(this.ConsolidatedAnalysisGroupBox);
            this.Controls.Add(this.SessionAnalysisGroupBox);
            this.Name = "Office365TabPage";
            this.Size = new System.Drawing.Size(778, 790);
            this.Load += new System.EventHandler(this.Office365TabPage_Load);
            this.SessionAnalysisGroupBox.ResumeLayout(false);
            this.ConsolidatedAnalysisGroupBox.ResumeLayout(false);
            this.CheckIPAddressGroupBox.ResumeLayout(false);
            this.CheckIPAddressGroupBox.PerformLayout();
            this.ExtensionOptionsGroupBox.ResumeLayout(false);
            this.ExtensionOptionsGroupBox.PerformLayout();
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
        private System.Windows.Forms.GroupBox ConsolidatedAnalysisGroupBox;
        private System.Windows.Forms.Button CreateConsolidatedAnalysisButton;
        private System.Windows.Forms.Button OpenLatestConsolidatedAnalysisReportButton;
        private System.Windows.Forms.GroupBox CheckIPAddressGroupBox;
        private System.Windows.Forms.TextBox CheckIPAddressResultTextBox;
        private System.Windows.Forms.Button CheckIPAddressButton;
        private System.Windows.Forms.TextBox EnterIPAddressTextBox;
        private System.Windows.Forms.GroupBox ExtensionOptionsGroupBox;
        private System.Windows.Forms.CheckBox ExtensionEnabledCheckBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLiveTraceCheckBox;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLoadSazCheckBox;
        private System.Windows.Forms.RadioButton SomeSessionAnalysisRadioButton;
        private System.Windows.Forms.RadioButton AllSessionAnalysisRadioButton;
        private System.Windows.Forms.Label LanguageLabel;
        private System.Windows.Forms.TextBox LanguageTextBox;
        private System.Windows.Forms.Button CheckIPAddressClearButton;
        private System.Windows.Forms.GroupBox ExtensionVersionInformationGroupBox;
        private System.Windows.Forms.Label ExtensionVersionLabel;
        private System.Windows.Forms.Label RulesetVersionLabel;
        private System.Windows.Forms.LinkLabel UpdateLinkLabel;
    }
}
