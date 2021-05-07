namespace O365FiddlerInspector.UI
{
    partial class O365ResponseControl
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
            this.ResultsDisplay = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ResultsDisplay
            // 
            this.ResultsDisplay.AcceptsReturn = true;
            this.ResultsDisplay.AcceptsTab = true;
            this.ResultsDisplay.BackColor = System.Drawing.Color.AliceBlue;
            this.ResultsDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ResultsDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultsDisplay.Font = new System.Drawing.Font("Lucida Console", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResultsDisplay.Location = new System.Drawing.Point(0, 0);
            this.ResultsDisplay.Margin = new System.Windows.Forms.Padding(6);
            this.ResultsDisplay.Multiline = true;
            this.ResultsDisplay.Name = "ResultsDisplay";
            this.ResultsDisplay.ReadOnly = true;
            this.ResultsDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResultsDisplay.Size = new System.Drawing.Size(558, 605);
            this.ResultsDisplay.TabIndex = 1;
            // 
            // ExchangeResponseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.ResultsDisplay);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ExchangeResponseControl";
            this.Size = new System.Drawing.Size(558, 605);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ResultsDisplay;
    }
}
