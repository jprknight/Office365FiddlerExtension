namespace EXOFiddlerInspector
{
    partial class ResponseUserControl
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
            this.ResponseBodyTextBox = new System.Windows.Forms.TextBox();
            this.ResponseBodyLabel = new System.Windows.Forms.Label();
            this.ResponseCommentLabel = new System.Windows.Forms.Label();
            this.ResponseCommentsTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ResponseBodyTextBox
            // 
            this.ResponseBodyTextBox.Location = new System.Drawing.Point(7, 163);
            this.ResponseBodyTextBox.Multiline = true;
            this.ResponseBodyTextBox.Name = "ResponseBodyTextBox";
            this.ResponseBodyTextBox.Size = new System.Drawing.Size(587, 125);
            this.ResponseBodyTextBox.TabIndex = 7;
            // 
            // ResponseBodyLabel
            // 
            this.ResponseBodyLabel.AutoSize = true;
            this.ResponseBodyLabel.Location = new System.Drawing.Point(7, 147);
            this.ResponseBodyLabel.Name = "ResponseBodyLabel";
            this.ResponseBodyLabel.Size = new System.Drawing.Size(82, 13);
            this.ResponseBodyLabel.TabIndex = 6;
            this.ResponseBodyLabel.Text = "Response Body";
            // 
            // ResponseCommentLabel
            // 
            this.ResponseCommentLabel.AutoSize = true;
            this.ResponseCommentLabel.Location = new System.Drawing.Point(7, 3);
            this.ResponseCommentLabel.Name = "ResponseCommentLabel";
            this.ResponseCommentLabel.Size = new System.Drawing.Size(107, 13);
            this.ResponseCommentLabel.TabIndex = 5;
            this.ResponseCommentLabel.Text = "Response Comments";
            // 
            // ResponseCommentsTextBox
            // 
            this.ResponseCommentsTextBox.Location = new System.Drawing.Point(10, 19);
            this.ResponseCommentsTextBox.Multiline = true;
            this.ResponseCommentsTextBox.Name = "ResponseCommentsTextBox";
            this.ResponseCommentsTextBox.Size = new System.Drawing.Size(584, 125);
            this.ResponseCommentsTextBox.TabIndex = 4;
            // 
            // ResponseUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ResponseBodyTextBox);
            this.Controls.Add(this.ResponseBodyLabel);
            this.Controls.Add(this.ResponseCommentLabel);
            this.Controls.Add(this.ResponseCommentsTextBox);
            this.Name = "ResponseUserControl";
            this.Size = new System.Drawing.Size(600, 300);
            this.Load += new System.EventHandler(this.ResponseUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ResponseBodyTextBox;
        private System.Windows.Forms.Label ResponseBodyLabel;
        private System.Windows.Forms.Label ResponseCommentLabel;
        private System.Windows.Forms.TextBox ResponseCommentsTextBox;
    }
}
