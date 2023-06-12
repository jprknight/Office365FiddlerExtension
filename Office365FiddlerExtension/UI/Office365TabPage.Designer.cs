
namespace Office365FiddlerExtension.UI
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
            this.TabPageWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // TabPageWebBrowser
            // 
            this.TabPageWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabPageWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.TabPageWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.TabPageWebBrowser.Name = "TabPageWebBrowser";
            this.TabPageWebBrowser.Size = new System.Drawing.Size(928, 540);
            this.TabPageWebBrowser.TabIndex = 0;
            this.TabPageWebBrowser.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // Office365TabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabPageWebBrowser);
            this.Name = "Office365TabPage";
            this.Size = new System.Drawing.Size(928, 540);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser TabPageWebBrowser;
    }
}
