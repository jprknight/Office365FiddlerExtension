namespace Office365FiddlerExtension.UI
{
    partial class UpdateAvailable
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
            this.ExtensionUpdateLink = new System.Windows.Forms.Panel();
            this.InstructionsLabel = new System.Windows.Forms.Label();
            this.UpdateLinkLabel = new System.Windows.Forms.LinkLabel();
            this.RulesetGroupbox = new System.Windows.Forms.GroupBox();
            this.RulesetVersionTextbox = new System.Windows.Forms.TextBox();
            this.RulesetUpdateMessageLabel = new System.Windows.Forms.Label();
            this.ExtensionGroupbox = new System.Windows.Forms.GroupBox();
            this.ExtensionVersionTextbox = new System.Windows.Forms.TextBox();
            this.ExtensionUpdateMessageLabel = new System.Windows.Forms.Label();
            this.ExtensionUpdateLink.SuspendLayout();
            this.RulesetGroupbox.SuspendLayout();
            this.ExtensionGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExtensionUpdateLink
            // 
            this.ExtensionUpdateLink.Controls.Add(this.InstructionsLabel);
            this.ExtensionUpdateLink.Controls.Add(this.UpdateLinkLabel);
            this.ExtensionUpdateLink.Controls.Add(this.RulesetGroupbox);
            this.ExtensionUpdateLink.Controls.Add(this.ExtensionGroupbox);
            this.ExtensionUpdateLink.Location = new System.Drawing.Point(12, 12);
            this.ExtensionUpdateLink.Name = "ExtensionUpdateLink";
            this.ExtensionUpdateLink.Size = new System.Drawing.Size(493, 156);
            this.ExtensionUpdateLink.TabIndex = 0;
            // 
            // InstructionsLabel
            // 
            this.InstructionsLabel.AutoSize = true;
            this.InstructionsLabel.Location = new System.Drawing.Point(7, 116);
            this.InstructionsLabel.Name = "InstructionsLabel";
            this.InstructionsLabel.Size = new System.Drawing.Size(208, 13);
            this.InstructionsLabel.TabIndex = 5;
            this.InstructionsLabel.Text = "Click the link below for update instructions.";
            // 
            // UpdateLinkLabel
            // 
            this.UpdateLinkLabel.AutoSize = true;
            this.UpdateLinkLabel.Location = new System.Drawing.Point(7, 134);
            this.UpdateLinkLabel.Name = "UpdateLinkLabel";
            this.UpdateLinkLabel.Size = new System.Drawing.Size(88, 13);
            this.UpdateLinkLabel.TabIndex = 4;
            this.UpdateLinkLabel.TabStop = true;
            this.UpdateLinkLabel.Text = "UpdateLinkLabel";
            // 
            // RulesetGroupbox
            // 
            this.RulesetGroupbox.Controls.Add(this.RulesetVersionTextbox);
            this.RulesetGroupbox.Controls.Add(this.RulesetUpdateMessageLabel);
            this.RulesetGroupbox.Location = new System.Drawing.Point(3, 60);
            this.RulesetGroupbox.Name = "RulesetGroupbox";
            this.RulesetGroupbox.Size = new System.Drawing.Size(478, 51);
            this.RulesetGroupbox.TabIndex = 3;
            this.RulesetGroupbox.TabStop = false;
            this.RulesetGroupbox.Text = "Ruleset";
            // 
            // RulesetVersionTextbox
            // 
            this.RulesetVersionTextbox.BackColor = System.Drawing.Color.White;
            this.RulesetVersionTextbox.Location = new System.Drawing.Point(7, 20);
            this.RulesetVersionTextbox.Name = "RulesetVersionTextbox";
            this.RulesetVersionTextbox.ReadOnly = true;
            this.RulesetVersionTextbox.Size = new System.Drawing.Size(100, 20);
            this.RulesetVersionTextbox.TabIndex = 0;
            // 
            // RulesetUpdateMessageLabel
            // 
            this.RulesetUpdateMessageLabel.AutoSize = true;
            this.RulesetUpdateMessageLabel.Location = new System.Drawing.Point(113, 23);
            this.RulesetUpdateMessageLabel.Name = "RulesetUpdateMessageLabel";
            this.RulesetUpdateMessageLabel.Size = new System.Drawing.Size(147, 13);
            this.RulesetUpdateMessageLabel.TabIndex = 1;
            this.RulesetUpdateMessageLabel.Text = "RulesetUpdateMessageLabel";
            // 
            // ExtensionGroupbox
            // 
            this.ExtensionGroupbox.Controls.Add(this.ExtensionVersionTextbox);
            this.ExtensionGroupbox.Controls.Add(this.ExtensionUpdateMessageLabel);
            this.ExtensionGroupbox.Location = new System.Drawing.Point(3, 3);
            this.ExtensionGroupbox.Name = "ExtensionGroupbox";
            this.ExtensionGroupbox.Size = new System.Drawing.Size(478, 51);
            this.ExtensionGroupbox.TabIndex = 2;
            this.ExtensionGroupbox.TabStop = false;
            this.ExtensionGroupbox.Text = "Extension";
            // 
            // ExtensionVersionTextbox
            // 
            this.ExtensionVersionTextbox.BackColor = System.Drawing.Color.White;
            this.ExtensionVersionTextbox.Location = new System.Drawing.Point(7, 20);
            this.ExtensionVersionTextbox.Name = "ExtensionVersionTextbox";
            this.ExtensionVersionTextbox.ReadOnly = true;
            this.ExtensionVersionTextbox.Size = new System.Drawing.Size(100, 20);
            this.ExtensionVersionTextbox.TabIndex = 0;
            // 
            // ExtensionUpdateMessageLabel
            // 
            this.ExtensionUpdateMessageLabel.AutoSize = true;
            this.ExtensionUpdateMessageLabel.Location = new System.Drawing.Point(113, 23);
            this.ExtensionUpdateMessageLabel.Name = "ExtensionUpdateMessageLabel";
            this.ExtensionUpdateMessageLabel.Size = new System.Drawing.Size(157, 13);
            this.ExtensionUpdateMessageLabel.TabIndex = 1;
            this.ExtensionUpdateMessageLabel.Text = "ExtensionUpdateMessageLabel";
            // 
            // UpdateAvailable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 177);
            this.Controls.Add(this.ExtensionUpdateLink);
            this.Name = "UpdateAvailable";
            this.Text = "Update Available";
            this.Load += new System.EventHandler(this.UpdateAvailable_Load);
            this.ExtensionUpdateLink.ResumeLayout(false);
            this.ExtensionUpdateLink.PerformLayout();
            this.RulesetGroupbox.ResumeLayout(false);
            this.RulesetGroupbox.PerformLayout();
            this.ExtensionGroupbox.ResumeLayout(false);
            this.ExtensionGroupbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ExtensionUpdateLink;
        private System.Windows.Forms.Label ExtensionUpdateMessageLabel;
        private System.Windows.Forms.GroupBox ExtensionGroupbox;
        private System.Windows.Forms.TextBox ExtensionVersionTextbox;
        private System.Windows.Forms.LinkLabel UpdateLinkLabel;
        private System.Windows.Forms.GroupBox RulesetGroupbox;
        private System.Windows.Forms.TextBox RulesetVersionTextbox;
        private System.Windows.Forms.Label RulesetUpdateMessageLabel;
        private System.Windows.Forms.Label InstructionsLabel;
    }
}