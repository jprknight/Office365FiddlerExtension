namespace EXOFiddlerInspector
{
    partial class RequestUserControl
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
            this.RequestCommentsTextBox = new System.Windows.Forms.TextBox();
            this.RequestCommentLabel = new System.Windows.Forms.Label();
            this.RequestBodyLabel = new System.Windows.Forms.Label();
            this.RequestBodyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RequestCommentsTextBox
            // 
            this.RequestCommentsTextBox.Location = new System.Drawing.Point(6, 16);
            this.RequestCommentsTextBox.Multiline = true;
            this.RequestCommentsTextBox.Name = "RequestCommentsTextBox";
            this.RequestCommentsTextBox.Size = new System.Drawing.Size(584, 125);
            this.RequestCommentsTextBox.TabIndex = 0;
            // 
            // RequestCommentLabel
            // 
            this.RequestCommentLabel.AutoSize = true;
            this.RequestCommentLabel.Location = new System.Drawing.Point(3, 0);
            this.RequestCommentLabel.Name = "RequestCommentLabel";
            this.RequestCommentLabel.Size = new System.Drawing.Size(99, 13);
            this.RequestCommentLabel.TabIndex = 1;
            this.RequestCommentLabel.Text = "Request Comments";
            // 
            // RequestBodyLabel
            // 
            this.RequestBodyLabel.AutoSize = true;
            this.RequestBodyLabel.Location = new System.Drawing.Point(3, 144);
            this.RequestBodyLabel.Name = "RequestBodyLabel";
            this.RequestBodyLabel.Size = new System.Drawing.Size(74, 13);
            this.RequestBodyLabel.TabIndex = 2;
            this.RequestBodyLabel.Text = "Request Body";
            // 
            // RequestBodyTextBox
            // 
            this.RequestBodyTextBox.Location = new System.Drawing.Point(3, 160);
            this.RequestBodyTextBox.Multiline = true;
            this.RequestBodyTextBox.Name = "RequestBodyTextBox";
            this.RequestBodyTextBox.Size = new System.Drawing.Size(587, 125);
            this.RequestBodyTextBox.TabIndex = 3;
            // 
            // RequestUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RequestBodyTextBox);
            this.Controls.Add(this.RequestBodyLabel);
            this.Controls.Add(this.RequestCommentLabel);
            this.Controls.Add(this.RequestCommentsTextBox);
            this.Name = "RequestUserControl";
            this.Size = new System.Drawing.Size(600, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox RequestCommentsTextBox;
        private System.Windows.Forms.Label RequestCommentLabel;
        private System.Windows.Forms.Label RequestBodyLabel;
        private System.Windows.Forms.TextBox RequestBodyTextBox;
    }
}
