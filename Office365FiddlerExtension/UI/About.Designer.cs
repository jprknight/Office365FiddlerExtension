﻿namespace Office365FiddlerExtension.UI
{
    partial class About
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
            this.InfoGroupbox = new System.Windows.Forms.GroupBox();
            this.LocalRulesetVersionLabel = new System.Windows.Forms.Label();
            this.LocalRulesetVersionTextbox = new System.Windows.Forms.TextBox();
            this.LocalDLLVersionLabel = new System.Windows.Forms.Label();
            this.LocalDLLVersionTextbox = new System.Windows.Forms.TextBox();
            this.ExtensionDLLLabel = new System.Windows.Forms.Label();
            this.ExtensionDLLTextbox = new System.Windows.Forms.TextBox();
            this.ExtensionPathLabel = new System.Windows.Forms.Label();
            this.ExtensionPathTextbox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SessionTimeThresholdLink = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SlowRunningSessionThresholdUpdateButton = new System.Windows.Forms.Button();
            this.WarningSessionTimeThresholdUpdateButton = new System.Windows.Forms.Button();
            this.SlowRunningSessionThresholdTextbox = new System.Windows.Forms.TextBox();
            this.SlowRunningSessionThresholdLabel = new System.Windows.Forms.Label();
            this.WarningSessionTimeThresholdLabel = new System.Windows.Forms.Label();
            this.WarningSessionTimeThresholdTextbox = new System.Windows.Forms.TextBox();
            this.ExtensionEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.SessionAnalysisOnLiveTraceCheckbox = new System.Windows.Forms.CheckBox();
            this.AllSessionAnalysisRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionAnalysisOnLoadSazCheckbox = new System.Windows.Forms.CheckBox();
            this.SomeSessionAnalysisRadioButton = new System.Windows.Forms.RadioButton();
            this.CloseButton = new System.Windows.Forms.Button();
            this.GithubInfoGroupbox = new System.Windows.Forms.GroupBox();
            this.NextUpdateCheckTimestampLabel = new System.Windows.Forms.Label();
            this.NextUpdateCheckTimestampTextbox = new System.Windows.Forms.TextBox();
            this.GithubRulesetVersionLabel = new System.Windows.Forms.Label();
            this.GithubRulesetVersionTextbox = new System.Windows.Forms.TextBox();
            this.GithubDLLVersionLabel = new System.Windows.Forms.Label();
            this.GithubDLLVersionTextbox = new System.Windows.Forms.TextBox();
            this.InfoGroupbox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.GithubInfoGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // InfoGroupbox
            // 
            this.InfoGroupbox.Controls.Add(this.LocalRulesetVersionLabel);
            this.InfoGroupbox.Controls.Add(this.LocalRulesetVersionTextbox);
            this.InfoGroupbox.Controls.Add(this.LocalDLLVersionLabel);
            this.InfoGroupbox.Controls.Add(this.LocalDLLVersionTextbox);
            this.InfoGroupbox.Controls.Add(this.ExtensionDLLLabel);
            this.InfoGroupbox.Controls.Add(this.ExtensionDLLTextbox);
            this.InfoGroupbox.Controls.Add(this.ExtensionPathLabel);
            this.InfoGroupbox.Controls.Add(this.ExtensionPathTextbox);
            this.InfoGroupbox.Location = new System.Drawing.Point(12, 12);
            this.InfoGroupbox.Name = "InfoGroupbox";
            this.InfoGroupbox.Size = new System.Drawing.Size(362, 132);
            this.InfoGroupbox.TabIndex = 0;
            this.InfoGroupbox.TabStop = false;
            this.InfoGroupbox.Text = "Extension Information";
            // 
            // LocalRulesetVersionLabel
            // 
            this.LocalRulesetVersionLabel.AutoSize = true;
            this.LocalRulesetVersionLabel.Location = new System.Drawing.Point(6, 101);
            this.LocalRulesetVersionLabel.Name = "LocalRulesetVersionLabel";
            this.LocalRulesetVersionLabel.Size = new System.Drawing.Size(110, 13);
            this.LocalRulesetVersionLabel.TabIndex = 7;
            this.LocalRulesetVersionLabel.Text = "Local Ruleset Verison";
            // 
            // LocalRulesetVersionTextbox
            // 
            this.LocalRulesetVersionTextbox.BackColor = System.Drawing.Color.White;
            this.LocalRulesetVersionTextbox.Location = new System.Drawing.Point(140, 98);
            this.LocalRulesetVersionTextbox.Name = "LocalRulesetVersionTextbox";
            this.LocalRulesetVersionTextbox.ReadOnly = true;
            this.LocalRulesetVersionTextbox.Size = new System.Drawing.Size(216, 20);
            this.LocalRulesetVersionTextbox.TabIndex = 6;
            // 
            // LocalDLLVersionLabel
            // 
            this.LocalDLLVersionLabel.AutoSize = true;
            this.LocalDLLVersionLabel.Location = new System.Drawing.Point(6, 75);
            this.LocalDLLVersionLabel.Name = "LocalDLLVersionLabel";
            this.LocalDLLVersionLabel.Size = new System.Drawing.Size(94, 13);
            this.LocalDLLVersionLabel.TabIndex = 5;
            this.LocalDLLVersionLabel.Text = "Local DLL Verison";
            // 
            // LocalDLLVersionTextbox
            // 
            this.LocalDLLVersionTextbox.BackColor = System.Drawing.Color.White;
            this.LocalDLLVersionTextbox.Location = new System.Drawing.Point(140, 72);
            this.LocalDLLVersionTextbox.Name = "LocalDLLVersionTextbox";
            this.LocalDLLVersionTextbox.ReadOnly = true;
            this.LocalDLLVersionTextbox.Size = new System.Drawing.Size(216, 20);
            this.LocalDLLVersionTextbox.TabIndex = 4;
            // 
            // ExtensionDLLLabel
            // 
            this.ExtensionDLLLabel.AutoSize = true;
            this.ExtensionDLLLabel.Location = new System.Drawing.Point(6, 49);
            this.ExtensionDLLLabel.Name = "ExtensionDLLLabel";
            this.ExtensionDLLLabel.Size = new System.Drawing.Size(76, 13);
            this.ExtensionDLLLabel.TabIndex = 3;
            this.ExtensionDLLLabel.Text = "Extension DLL";
            // 
            // ExtensionDLLTextbox
            // 
            this.ExtensionDLLTextbox.BackColor = System.Drawing.Color.White;
            this.ExtensionDLLTextbox.Location = new System.Drawing.Point(140, 46);
            this.ExtensionDLLTextbox.Name = "ExtensionDLLTextbox";
            this.ExtensionDLLTextbox.ReadOnly = true;
            this.ExtensionDLLTextbox.Size = new System.Drawing.Size(216, 20);
            this.ExtensionDLLTextbox.TabIndex = 2;
            // 
            // ExtensionPathLabel
            // 
            this.ExtensionPathLabel.AutoSize = true;
            this.ExtensionPathLabel.Location = new System.Drawing.Point(6, 22);
            this.ExtensionPathLabel.Name = "ExtensionPathLabel";
            this.ExtensionPathLabel.Size = new System.Drawing.Size(78, 13);
            this.ExtensionPathLabel.TabIndex = 1;
            this.ExtensionPathLabel.Text = "Extension Path";
            // 
            // ExtensionPathTextbox
            // 
            this.ExtensionPathTextbox.BackColor = System.Drawing.Color.White;
            this.ExtensionPathTextbox.Location = new System.Drawing.Point(140, 19);
            this.ExtensionPathTextbox.Name = "ExtensionPathTextbox";
            this.ExtensionPathTextbox.ReadOnly = true;
            this.ExtensionPathTextbox.Size = new System.Drawing.Size(216, 20);
            this.ExtensionPathTextbox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(380, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 236);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extension Options";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SessionTimeThresholdLink);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SlowRunningSessionThresholdUpdateButton);
            this.panel1.Controls.Add(this.WarningSessionTimeThresholdUpdateButton);
            this.panel1.Controls.Add(this.SlowRunningSessionThresholdTextbox);
            this.panel1.Controls.Add(this.SlowRunningSessionThresholdLabel);
            this.panel1.Controls.Add(this.WarningSessionTimeThresholdLabel);
            this.panel1.Controls.Add(this.WarningSessionTimeThresholdTextbox);
            this.panel1.Controls.Add(this.ExtensionEnabledCheckbox);
            this.panel1.Controls.Add(this.SessionAnalysisOnLiveTraceCheckbox);
            this.panel1.Controls.Add(this.AllSessionAnalysisRadioButton);
            this.panel1.Controls.Add(this.SessionAnalysisOnLoadSazCheckbox);
            this.panel1.Controls.Add(this.SomeSessionAnalysisRadioButton);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 211);
            this.panel1.TabIndex = 5;
            // 
            // SessionTimeThresholdLink
            // 
            this.SessionTimeThresholdLink.AutoSize = true;
            this.SessionTimeThresholdLink.Location = new System.Drawing.Point(4, 191);
            this.SessionTimeThresholdLink.Name = "SessionTimeThresholdLink";
            this.SessionTimeThresholdLink.Size = new System.Drawing.Size(414, 13);
            this.SessionTimeThresholdLink.TabIndex = 15;
            this.SessionTimeThresholdLink.TabStop = true;
            this.SessionTimeThresholdLink.Text = "https://github.com/jprknight/Office365FiddlerExtension/wiki/Session-Time-Threshol" +
    "ds";
            this.SessionTimeThresholdLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SessionTimeThresholdLink_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "For information on these two threshold values see:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 13;
            // 
            // SlowRunningSessionThresholdUpdateButton
            // 
            this.SlowRunningSessionThresholdUpdateButton.Location = new System.Drawing.Point(234, 145);
            this.SlowRunningSessionThresholdUpdateButton.Name = "SlowRunningSessionThresholdUpdateButton";
            this.SlowRunningSessionThresholdUpdateButton.Size = new System.Drawing.Size(56, 23);
            this.SlowRunningSessionThresholdUpdateButton.TabIndex = 12;
            this.SlowRunningSessionThresholdUpdateButton.Text = "Update";
            this.SlowRunningSessionThresholdUpdateButton.UseVisualStyleBackColor = true;
            this.SlowRunningSessionThresholdUpdateButton.Click += new System.EventHandler(this.SlowRunningSessionThresholdUpdateButton_Click);
            // 
            // WarningSessionTimeThresholdUpdateButton
            // 
            this.WarningSessionTimeThresholdUpdateButton.Location = new System.Drawing.Point(234, 119);
            this.WarningSessionTimeThresholdUpdateButton.Name = "WarningSessionTimeThresholdUpdateButton";
            this.WarningSessionTimeThresholdUpdateButton.Size = new System.Drawing.Size(56, 23);
            this.WarningSessionTimeThresholdUpdateButton.TabIndex = 11;
            this.WarningSessionTimeThresholdUpdateButton.Text = "Update";
            this.WarningSessionTimeThresholdUpdateButton.UseVisualStyleBackColor = true;
            this.WarningSessionTimeThresholdUpdateButton.Click += new System.EventHandler(this.WarningSessionTimeThresholdUpdateButton_Click);
            // 
            // SlowRunningSessionThresholdTextbox
            // 
            this.SlowRunningSessionThresholdTextbox.Location = new System.Drawing.Point(173, 147);
            this.SlowRunningSessionThresholdTextbox.Name = "SlowRunningSessionThresholdTextbox";
            this.SlowRunningSessionThresholdTextbox.Size = new System.Drawing.Size(55, 20);
            this.SlowRunningSessionThresholdTextbox.TabIndex = 9;
            // 
            // SlowRunningSessionThresholdLabel
            // 
            this.SlowRunningSessionThresholdLabel.AutoSize = true;
            this.SlowRunningSessionThresholdLabel.Location = new System.Drawing.Point(4, 150);
            this.SlowRunningSessionThresholdLabel.Name = "SlowRunningSessionThresholdLabel";
            this.SlowRunningSessionThresholdLabel.Size = new System.Drawing.Size(163, 13);
            this.SlowRunningSessionThresholdLabel.TabIndex = 8;
            this.SlowRunningSessionThresholdLabel.Text = "Slow Running Session Threshold";
            // 
            // WarningSessionTimeThresholdLabel
            // 
            this.WarningSessionTimeThresholdLabel.AutoSize = true;
            this.WarningSessionTimeThresholdLabel.Location = new System.Drawing.Point(4, 124);
            this.WarningSessionTimeThresholdLabel.Name = "WarningSessionTimeThresholdLabel";
            this.WarningSessionTimeThresholdLabel.Size = new System.Drawing.Size(163, 13);
            this.WarningSessionTimeThresholdLabel.TabIndex = 7;
            this.WarningSessionTimeThresholdLabel.Text = "Warning Session Time Threshold";
            // 
            // WarningSessionTimeThresholdTextbox
            // 
            this.WarningSessionTimeThresholdTextbox.Location = new System.Drawing.Point(173, 121);
            this.WarningSessionTimeThresholdTextbox.Name = "WarningSessionTimeThresholdTextbox";
            this.WarningSessionTimeThresholdTextbox.Size = new System.Drawing.Size(55, 20);
            this.WarningSessionTimeThresholdTextbox.TabIndex = 6;
            // 
            // ExtensionEnabledCheckbox
            // 
            this.ExtensionEnabledCheckbox.AutoSize = true;
            this.ExtensionEnabledCheckbox.Location = new System.Drawing.Point(4, 4);
            this.ExtensionEnabledCheckbox.Name = "ExtensionEnabledCheckbox";
            this.ExtensionEnabledCheckbox.Size = new System.Drawing.Size(114, 17);
            this.ExtensionEnabledCheckbox.TabIndex = 5;
            this.ExtensionEnabledCheckbox.Text = "Extension Enabled";
            this.ExtensionEnabledCheckbox.UseVisualStyleBackColor = true;
            this.ExtensionEnabledCheckbox.CheckedChanged += new System.EventHandler(this.ExtensionEnabledCheckbox_CheckedChanged);
            // 
            // SessionAnalysisOnLiveTraceCheckbox
            // 
            this.SessionAnalysisOnLiveTraceCheckbox.AutoSize = true;
            this.SessionAnalysisOnLiveTraceCheckbox.Location = new System.Drawing.Point(21, 98);
            this.SessionAnalysisOnLiveTraceCheckbox.Name = "SessionAnalysisOnLiveTraceCheckbox";
            this.SessionAnalysisOnLiveTraceCheckbox.Size = new System.Drawing.Size(94, 17);
            this.SessionAnalysisOnLiveTraceCheckbox.TabIndex = 4;
            this.SessionAnalysisOnLiveTraceCheckbox.Text = "On Live Trace";
            this.SessionAnalysisOnLiveTraceCheckbox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnLiveTraceCheckbox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnLiveTraceCheckbox_CheckedChanged);
            // 
            // AllSessionAnalysisRadioButton
            // 
            this.AllSessionAnalysisRadioButton.AutoSize = true;
            this.AllSessionAnalysisRadioButton.Location = new System.Drawing.Point(4, 27);
            this.AllSessionAnalysisRadioButton.Name = "AllSessionAnalysisRadioButton";
            this.AllSessionAnalysisRadioButton.Size = new System.Drawing.Size(117, 17);
            this.AllSessionAnalysisRadioButton.TabIndex = 0;
            this.AllSessionAnalysisRadioButton.TabStop = true;
            this.AllSessionAnalysisRadioButton.Text = "All Session Analysis";
            this.AllSessionAnalysisRadioButton.UseVisualStyleBackColor = true;
            this.AllSessionAnalysisRadioButton.CheckedChanged += new System.EventHandler(this.AllSessionAnalysisRadioButton_CheckedChanged);
            // 
            // SessionAnalysisOnLoadSazCheckbox
            // 
            this.SessionAnalysisOnLoadSazCheckbox.AutoSize = true;
            this.SessionAnalysisOnLoadSazCheckbox.Location = new System.Drawing.Point(21, 74);
            this.SessionAnalysisOnLoadSazCheckbox.Name = "SessionAnalysisOnLoadSazCheckbox";
            this.SessionAnalysisOnLoadSazCheckbox.Size = new System.Drawing.Size(88, 17);
            this.SessionAnalysisOnLoadSazCheckbox.TabIndex = 3;
            this.SessionAnalysisOnLoadSazCheckbox.Text = "On Load Saz";
            this.SessionAnalysisOnLoadSazCheckbox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnLoadSazCheckbox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnLoadSazCheckbox_CheckedChanged);
            // 
            // SomeSessionAnalysisRadioButton
            // 
            this.SomeSessionAnalysisRadioButton.AutoSize = true;
            this.SomeSessionAnalysisRadioButton.Location = new System.Drawing.Point(4, 51);
            this.SomeSessionAnalysisRadioButton.Name = "SomeSessionAnalysisRadioButton";
            this.SomeSessionAnalysisRadioButton.Size = new System.Drawing.Size(133, 17);
            this.SomeSessionAnalysisRadioButton.TabIndex = 1;
            this.SomeSessionAnalysisRadioButton.TabStop = true;
            this.SomeSessionAnalysisRadioButton.Text = "Some Session Analysis";
            this.SomeSessionAnalysisRadioButton.UseVisualStyleBackColor = true;
            this.SomeSessionAnalysisRadioButton.CheckedChanged += new System.EventHandler(this.SomeSessionAnalysisRadioButton_CheckedChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(743, 254);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 10;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // GithubInfoGroupbox
            // 
            this.GithubInfoGroupbox.Controls.Add(this.NextUpdateCheckTimestampLabel);
            this.GithubInfoGroupbox.Controls.Add(this.NextUpdateCheckTimestampTextbox);
            this.GithubInfoGroupbox.Controls.Add(this.GithubRulesetVersionLabel);
            this.GithubInfoGroupbox.Controls.Add(this.GithubRulesetVersionTextbox);
            this.GithubInfoGroupbox.Controls.Add(this.GithubDLLVersionLabel);
            this.GithubInfoGroupbox.Controls.Add(this.GithubDLLVersionTextbox);
            this.GithubInfoGroupbox.Location = new System.Drawing.Point(12, 150);
            this.GithubInfoGroupbox.Name = "GithubInfoGroupbox";
            this.GithubInfoGroupbox.Size = new System.Drawing.Size(362, 98);
            this.GithubInfoGroupbox.TabIndex = 11;
            this.GithubInfoGroupbox.TabStop = false;
            this.GithubInfoGroupbox.Text = "Github Information";
            // 
            // NextUpdateCheckTimestampLabel
            // 
            this.NextUpdateCheckTimestampLabel.AutoSize = true;
            this.NextUpdateCheckTimestampLabel.Location = new System.Drawing.Point(6, 74);
            this.NextUpdateCheckTimestampLabel.Name = "NextUpdateCheckTimestampLabel";
            this.NextUpdateCheckTimestampLabel.Size = new System.Drawing.Size(101, 13);
            this.NextUpdateCheckTimestampLabel.TabIndex = 12;
            this.NextUpdateCheckTimestampLabel.Text = "Next Update Check";
            // 
            // NextUpdateCheckTimestampTextbox
            // 
            this.NextUpdateCheckTimestampTextbox.BackColor = System.Drawing.Color.White;
            this.NextUpdateCheckTimestampTextbox.Location = new System.Drawing.Point(140, 71);
            this.NextUpdateCheckTimestampTextbox.Name = "NextUpdateCheckTimestampTextbox";
            this.NextUpdateCheckTimestampTextbox.ReadOnly = true;
            this.NextUpdateCheckTimestampTextbox.Size = new System.Drawing.Size(216, 20);
            this.NextUpdateCheckTimestampTextbox.TabIndex = 11;
            // 
            // GithubRulesetVersionLabel
            // 
            this.GithubRulesetVersionLabel.AutoSize = true;
            this.GithubRulesetVersionLabel.Location = new System.Drawing.Point(6, 48);
            this.GithubRulesetVersionLabel.Name = "GithubRulesetVersionLabel";
            this.GithubRulesetVersionLabel.Size = new System.Drawing.Size(115, 13);
            this.GithubRulesetVersionLabel.TabIndex = 10;
            this.GithubRulesetVersionLabel.Text = "Github Ruleset Version";
            // 
            // GithubRulesetVersionTextbox
            // 
            this.GithubRulesetVersionTextbox.BackColor = System.Drawing.Color.White;
            this.GithubRulesetVersionTextbox.Location = new System.Drawing.Point(140, 45);
            this.GithubRulesetVersionTextbox.Name = "GithubRulesetVersionTextbox";
            this.GithubRulesetVersionTextbox.ReadOnly = true;
            this.GithubRulesetVersionTextbox.Size = new System.Drawing.Size(216, 20);
            this.GithubRulesetVersionTextbox.TabIndex = 9;
            // 
            // GithubDLLVersionLabel
            // 
            this.GithubDLLVersionLabel.AutoSize = true;
            this.GithubDLLVersionLabel.Location = new System.Drawing.Point(6, 22);
            this.GithubDLLVersionLabel.Name = "GithubDLLVersionLabel";
            this.GithubDLLVersionLabel.Size = new System.Drawing.Size(99, 13);
            this.GithubDLLVersionLabel.TabIndex = 8;
            this.GithubDLLVersionLabel.Text = "Github DLL Version";
            // 
            // GithubDLLVersionTextbox
            // 
            this.GithubDLLVersionTextbox.BackColor = System.Drawing.Color.White;
            this.GithubDLLVersionTextbox.Location = new System.Drawing.Point(140, 19);
            this.GithubDLLVersionTextbox.Name = "GithubDLLVersionTextbox";
            this.GithubDLLVersionTextbox.ReadOnly = true;
            this.GithubDLLVersionTextbox.Size = new System.Drawing.Size(216, 20);
            this.GithubDLLVersionTextbox.TabIndex = 8;
            // 
            // AboutNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 286);
            this.Controls.Add(this.GithubInfoGroupbox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.InfoGroupbox);
            this.Controls.Add(this.CloseButton);
            this.Name = "AboutNew";
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.InfoGroupbox.ResumeLayout(false);
            this.InfoGroupbox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.GithubInfoGroupbox.ResumeLayout(false);
            this.GithubInfoGroupbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox InfoGroupbox;
        private System.Windows.Forms.TextBox ExtensionDLLTextbox;
        private System.Windows.Forms.Label ExtensionPathLabel;
        private System.Windows.Forms.TextBox ExtensionPathTextbox;
        private System.Windows.Forms.Label ExtensionDLLLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLiveTraceCheckbox;
        private System.Windows.Forms.RadioButton AllSessionAnalysisRadioButton;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLoadSazCheckbox;
        private System.Windows.Forms.RadioButton SomeSessionAnalysisRadioButton;
        private System.Windows.Forms.CheckBox ExtensionEnabledCheckbox;
        private System.Windows.Forms.Label WarningSessionTimeThresholdLabel;
        private System.Windows.Forms.Label SlowRunningSessionThresholdLabel;
        private System.Windows.Forms.TextBox SlowRunningSessionThresholdTextbox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button SlowRunningSessionThresholdUpdateButton;
        private System.Windows.Forms.Button WarningSessionTimeThresholdUpdateButton;
        public System.Windows.Forms.TextBox WarningSessionTimeThresholdTextbox;
        private System.Windows.Forms.LinkLabel SessionTimeThresholdLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LocalDLLVersionLabel;
        private System.Windows.Forms.TextBox LocalDLLVersionTextbox;
        private System.Windows.Forms.Label LocalRulesetVersionLabel;
        private System.Windows.Forms.TextBox LocalRulesetVersionTextbox;
        private System.Windows.Forms.GroupBox GithubInfoGroupbox;
        private System.Windows.Forms.Label NextUpdateCheckTimestampLabel;
        private System.Windows.Forms.TextBox NextUpdateCheckTimestampTextbox;
        private System.Windows.Forms.Label GithubRulesetVersionLabel;
        private System.Windows.Forms.TextBox GithubRulesetVersionTextbox;
        private System.Windows.Forms.Label GithubDLLVersionLabel;
        private System.Windows.Forms.TextBox GithubDLLVersionTextbox;
    }
}