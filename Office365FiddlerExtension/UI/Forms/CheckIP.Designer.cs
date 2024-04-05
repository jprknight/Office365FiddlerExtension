namespace Office365FiddlerExtension.UI.Forms
{
    partial class CheckIP
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
            this.IPAddressTextbox = new System.Windows.Forms.TextBox();
            this.CheckIPButton = new System.Windows.Forms.Button();
            this.ResultTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // IPAddressTextbox
            // 
            this.IPAddressTextbox.Location = new System.Drawing.Point(12, 12);
            this.IPAddressTextbox.Name = "IPAddressTextbox";
            this.IPAddressTextbox.Size = new System.Drawing.Size(177, 20);
            this.IPAddressTextbox.TabIndex = 0;
            // 
            // CheckIPButton
            // 
            this.CheckIPButton.Location = new System.Drawing.Point(195, 10);
            this.CheckIPButton.Name = "CheckIPButton";
            this.CheckIPButton.Size = new System.Drawing.Size(75, 23);
            this.CheckIPButton.TabIndex = 1;
            this.CheckIPButton.Text = "Check IP";
            this.CheckIPButton.UseVisualStyleBackColor = true;
            this.CheckIPButton.Click += new System.EventHandler(this.CheckIPButton_Click);
            // 
            // ResultTextbox
            // 
            this.ResultTextbox.Location = new System.Drawing.Point(12, 38);
            this.ResultTextbox.Multiline = true;
            this.ResultTextbox.Name = "ResultTextbox";
            this.ResultTextbox.ReadOnly = true;
            this.ResultTextbox.Size = new System.Drawing.Size(405, 154);
            this.ResultTextbox.TabIndex = 2;
            // 
            // CheckIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 205);
            this.Controls.Add(this.ResultTextbox);
            this.Controls.Add(this.CheckIPButton);
            this.Controls.Add(this.IPAddressTextbox);
            this.Name = "CheckIP";
            this.Text = "Check IP Address";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IPAddressTextbox;
        private System.Windows.Forms.Button CheckIPButton;
        private System.Windows.Forms.TextBox ResultTextbox;
    }
}