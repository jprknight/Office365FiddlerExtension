namespace Office365FiddlerExtension.UI
{
    partial class AboutNew
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
            this.ExtensionDLLLabel = new System.Windows.Forms.Label();
            this.ExtensionDLLTextbox = new System.Windows.Forms.TextBox();
            this.FiddlerPathLabel = new System.Windows.Forms.Label();
            this.FiddlerPathTextbox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ExtensionEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.SessionAnalysisOnLiveTraceCheckbox = new System.Windows.Forms.CheckBox();
            this.AllSessionAnalysisRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionAnalysisOnLoadSazCheckbox = new System.Windows.Forms.CheckBox();
            this.SomeSessionAnalysisRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionAnalysisOnFiddlerLoadCheckbox = new System.Windows.Forms.CheckBox();
            this.InfoGroupbox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // InfoGroupbox
            // 
            this.InfoGroupbox.Controls.Add(this.ExtensionDLLLabel);
            this.InfoGroupbox.Controls.Add(this.ExtensionDLLTextbox);
            this.InfoGroupbox.Controls.Add(this.FiddlerPathLabel);
            this.InfoGroupbox.Controls.Add(this.FiddlerPathTextbox);
            this.InfoGroupbox.Location = new System.Drawing.Point(16, 15);
            this.InfoGroupbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.InfoGroupbox.Name = "InfoGroupbox";
            this.InfoGroupbox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.InfoGroupbox.Size = new System.Drawing.Size(483, 107);
            this.InfoGroupbox.TabIndex = 0;
            this.InfoGroupbox.TabStop = false;
            this.InfoGroupbox.Text = "Extension Information";
            // 
            // ExtensionDLLLabel
            // 
            this.ExtensionDLLLabel.AutoSize = true;
            this.ExtensionDLLLabel.Location = new System.Drawing.Point(8, 60);
            this.ExtensionDLLLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ExtensionDLLLabel.Name = "ExtensionDLLLabel";
            this.ExtensionDLLLabel.Size = new System.Drawing.Size(92, 16);
            this.ExtensionDLLLabel.TabIndex = 3;
            this.ExtensionDLLLabel.Text = "Extension DLL";
            // 
            // ExtensionDLLTextbox
            // 
            this.ExtensionDLLTextbox.Location = new System.Drawing.Point(159, 57);
            this.ExtensionDLLTextbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExtensionDLLTextbox.Name = "ExtensionDLLTextbox";
            this.ExtensionDLLTextbox.Size = new System.Drawing.Size(315, 22);
            this.ExtensionDLLTextbox.TabIndex = 2;
            // 
            // FiddlerPathLabel
            // 
            this.FiddlerPathLabel.AutoSize = true;
            this.FiddlerPathLabel.Location = new System.Drawing.Point(8, 27);
            this.FiddlerPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FiddlerPathLabel.Name = "FiddlerPathLabel";
            this.FiddlerPathLabel.Size = new System.Drawing.Size(79, 16);
            this.FiddlerPathLabel.TabIndex = 1;
            this.FiddlerPathLabel.Text = "Fiddler Path";
            // 
            // FiddlerPathTextbox
            // 
            this.FiddlerPathTextbox.Location = new System.Drawing.Point(159, 23);
            this.FiddlerPathTextbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FiddlerPathTextbox.Name = "FiddlerPathTextbox";
            this.FiddlerPathTextbox.Size = new System.Drawing.Size(315, 22);
            this.FiddlerPathTextbox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(507, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(225, 236);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extension Options";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ExtensionEnabledCheckbox);
            this.panel1.Controls.Add(this.SessionAnalysisOnLiveTraceCheckbox);
            this.panel1.Controls.Add(this.AllSessionAnalysisRadioButton);
            this.panel1.Controls.Add(this.SessionAnalysisOnLoadSazCheckbox);
            this.panel1.Controls.Add(this.SomeSessionAnalysisRadioButton);
            this.panel1.Controls.Add(this.SessionAnalysisOnFiddlerLoadCheckbox);
            this.panel1.Location = new System.Drawing.Point(8, 23);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(193, 188);
            this.panel1.TabIndex = 5;
            // 
            // ExtensionEnabledCheckbox
            // 
            this.ExtensionEnabledCheckbox.AutoSize = true;
            this.ExtensionEnabledCheckbox.Location = new System.Drawing.Point(5, 5);
            this.ExtensionEnabledCheckbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExtensionEnabledCheckbox.Name = "ExtensionEnabledCheckbox";
            this.ExtensionEnabledCheckbox.Size = new System.Drawing.Size(141, 20);
            this.ExtensionEnabledCheckbox.TabIndex = 5;
            this.ExtensionEnabledCheckbox.Text = "Extension Enabled";
            this.ExtensionEnabledCheckbox.UseVisualStyleBackColor = true;
            this.ExtensionEnabledCheckbox.CheckedChanged += new System.EventHandler(this.ExtensionEnabledCheckbox_CheckedChanged);
            // 
            // SessionAnalysisOnLiveTraceCheckbox
            // 
            this.SessionAnalysisOnLiveTraceCheckbox.AutoSize = true;
            this.SessionAnalysisOnLiveTraceCheckbox.Location = new System.Drawing.Point(27, 151);
            this.SessionAnalysisOnLiveTraceCheckbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SessionAnalysisOnLiveTraceCheckbox.Name = "SessionAnalysisOnLiveTraceCheckbox";
            this.SessionAnalysisOnLiveTraceCheckbox.Size = new System.Drawing.Size(113, 20);
            this.SessionAnalysisOnLiveTraceCheckbox.TabIndex = 4;
            this.SessionAnalysisOnLiveTraceCheckbox.Text = "On Live Trace";
            this.SessionAnalysisOnLiveTraceCheckbox.UseVisualStyleBackColor = true;
            // 
            // AllSessionAnalysisRadioButton
            // 
            this.AllSessionAnalysisRadioButton.AutoSize = true;
            this.AllSessionAnalysisRadioButton.Location = new System.Drawing.Point(5, 33);
            this.AllSessionAnalysisRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AllSessionAnalysisRadioButton.Name = "AllSessionAnalysisRadioButton";
            this.AllSessionAnalysisRadioButton.Size = new System.Drawing.Size(149, 20);
            this.AllSessionAnalysisRadioButton.TabIndex = 0;
            this.AllSessionAnalysisRadioButton.TabStop = true;
            this.AllSessionAnalysisRadioButton.Text = "All Session Analysis";
            this.AllSessionAnalysisRadioButton.UseVisualStyleBackColor = true;
            this.AllSessionAnalysisRadioButton.CheckedChanged += new System.EventHandler(this.AllSessionAnalysisRadioButton_CheckedChanged);
            // 
            // SessionAnalysisOnLoadSazCheckbox
            // 
            this.SessionAnalysisOnLoadSazCheckbox.AutoSize = true;
            this.SessionAnalysisOnLoadSazCheckbox.Location = new System.Drawing.Point(27, 122);
            this.SessionAnalysisOnLoadSazCheckbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SessionAnalysisOnLoadSazCheckbox.Name = "SessionAnalysisOnLoadSazCheckbox";
            this.SessionAnalysisOnLoadSazCheckbox.Size = new System.Drawing.Size(106, 20);
            this.SessionAnalysisOnLoadSazCheckbox.TabIndex = 3;
            this.SessionAnalysisOnLoadSazCheckbox.Text = "On Load Saz";
            this.SessionAnalysisOnLoadSazCheckbox.UseVisualStyleBackColor = true;
            // 
            // SomeSessionAnalysisRadioButton
            // 
            this.SomeSessionAnalysisRadioButton.AutoSize = true;
            this.SomeSessionAnalysisRadioButton.Location = new System.Drawing.Point(5, 63);
            this.SomeSessionAnalysisRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SomeSessionAnalysisRadioButton.Name = "SomeSessionAnalysisRadioButton";
            this.SomeSessionAnalysisRadioButton.Size = new System.Drawing.Size(170, 20);
            this.SomeSessionAnalysisRadioButton.TabIndex = 1;
            this.SomeSessionAnalysisRadioButton.TabStop = true;
            this.SomeSessionAnalysisRadioButton.Text = "Some Session Analysis";
            this.SomeSessionAnalysisRadioButton.UseVisualStyleBackColor = true;
            this.SomeSessionAnalysisRadioButton.CheckedChanged += new System.EventHandler(this.SomeSessionAnalysisRadioButton_CheckedChanged);
            // 
            // SessionAnalysisOnFiddlerLoadCheckbox
            // 
            this.SessionAnalysisOnFiddlerLoadCheckbox.AutoSize = true;
            this.SessionAnalysisOnFiddlerLoadCheckbox.Location = new System.Drawing.Point(27, 92);
            this.SessionAnalysisOnFiddlerLoadCheckbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SessionAnalysisOnFiddlerLoadCheckbox.Name = "SessionAnalysisOnFiddlerLoadCheckbox";
            this.SessionAnalysisOnFiddlerLoadCheckbox.Size = new System.Drawing.Size(125, 20);
            this.SessionAnalysisOnFiddlerLoadCheckbox.TabIndex = 2;
            this.SessionAnalysisOnFiddlerLoadCheckbox.Text = "On Fiddler Load";
            this.SessionAnalysisOnFiddlerLoadCheckbox.UseVisualStyleBackColor = true;
            this.SessionAnalysisOnFiddlerLoadCheckbox.CheckedChanged += new System.EventHandler(this.SessionAnalysisOnFiddlerLoadCheckbox_CheckedChanged);
            // 
            // AboutNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 777);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.InfoGroupbox);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AboutNew";
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.InfoGroupbox.ResumeLayout(false);
            this.InfoGroupbox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox InfoGroupbox;
        private System.Windows.Forms.TextBox ExtensionDLLTextbox;
        private System.Windows.Forms.Label FiddlerPathLabel;
        private System.Windows.Forms.TextBox FiddlerPathTextbox;
        private System.Windows.Forms.Label ExtensionDLLLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLiveTraceCheckbox;
        private System.Windows.Forms.RadioButton AllSessionAnalysisRadioButton;
        private System.Windows.Forms.CheckBox SessionAnalysisOnLoadSazCheckbox;
        private System.Windows.Forms.RadioButton SomeSessionAnalysisRadioButton;
        private System.Windows.Forms.CheckBox SessionAnalysisOnFiddlerLoadCheckbox;
        private System.Windows.Forms.CheckBox ExtensionEnabledCheckbox;
    }
}