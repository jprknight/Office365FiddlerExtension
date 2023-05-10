namespace Office365FiddlerExtension.UI
{
    partial class Office365ResponseControl
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
            this.webBrowserControl = new System.Windows.Forms.WebBrowser();
            this.Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowserControl
            // 
            this.webBrowserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserControl.Location = new System.Drawing.Point(0, 0);
            this.webBrowserControl.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserControl.Name = "webBrowserControl";
            this.webBrowserControl.Size = new System.Drawing.Size(631, 598);
            this.webBrowserControl.TabIndex = 2;
            this.webBrowserControl.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserControl_DocumentCompleted);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(3, 3);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 3;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Office365ResponseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.Save);
            this.Controls.Add(this.webBrowserControl);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Office365ResponseControl";
            this.Size = new System.Drawing.Size(631, 598);
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.TextBox ResultsDisplay;
        private System.Windows.Forms.WebBrowser webBrowserControl;
        private System.Windows.Forms.Button Save;
    }
}
