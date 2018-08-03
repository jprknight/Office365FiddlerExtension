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
            this.RequestHostlabel = new System.Windows.Forms.Label();
            this.RequestHostTextBox = new System.Windows.Forms.TextBox();
            this.RequestURLlabel = new System.Windows.Forms.Label();
            this.RequestURLTextBox = new System.Windows.Forms.TextBox();
            this.RequestTypeLabel = new System.Windows.Forms.Label();
            this.RequestTypeTextBox = new System.Windows.Forms.TextBox();
            this.RequestProcessLabel = new System.Windows.Forms.Label();
            this.RequestProcessTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RequestCommentsTextBox
            // 
            this.RequestCommentsTextBox.BackColor = System.Drawing.Color.White;
            this.RequestCommentsTextBox.Location = new System.Drawing.Point(6, 127);
            this.RequestCommentsTextBox.Multiline = true;
            this.RequestCommentsTextBox.Name = "RequestCommentsTextBox";
            this.RequestCommentsTextBox.ReadOnly = true;
            this.RequestCommentsTextBox.Size = new System.Drawing.Size(400, 125);
            this.RequestCommentsTextBox.TabIndex = 0;
            // 
            // RequestCommentLabel
            // 
            this.RequestCommentLabel.AutoSize = true;
            this.RequestCommentLabel.Location = new System.Drawing.Point(3, 111);
            this.RequestCommentLabel.Name = "RequestCommentLabel";
            this.RequestCommentLabel.Size = new System.Drawing.Size(99, 13);
            this.RequestCommentLabel.TabIndex = 1;
            this.RequestCommentLabel.Text = "Request Comments";
            // 
            // RequestHostlabel
            // 
            this.RequestHostlabel.AutoSize = true;
            this.RequestHostlabel.Location = new System.Drawing.Point(3, 6);
            this.RequestHostlabel.Name = "RequestHostlabel";
            this.RequestHostlabel.Size = new System.Drawing.Size(72, 13);
            this.RequestHostlabel.TabIndex = 2;
            this.RequestHostlabel.Text = "Request Host";
            // 
            // RequestHostTextBox
            // 
            this.RequestHostTextBox.BackColor = System.Drawing.Color.White;
            this.RequestHostTextBox.Location = new System.Drawing.Point(106, 3);
            this.RequestHostTextBox.Name = "RequestHostTextBox";
            this.RequestHostTextBox.ReadOnly = true;
            this.RequestHostTextBox.Size = new System.Drawing.Size(300, 20);
            this.RequestHostTextBox.TabIndex = 3;
            this.RequestHostTextBox.TextChanged += new System.EventHandler(this.RequestHostTextBox_TextChanged);
            // 
            // RequestURLlabel
            // 
            this.RequestURLlabel.AutoSize = true;
            this.RequestURLlabel.Location = new System.Drawing.Point(3, 32);
            this.RequestURLlabel.Name = "RequestURLlabel";
            this.RequestURLlabel.Size = new System.Drawing.Size(72, 13);
            this.RequestURLlabel.TabIndex = 4;
            this.RequestURLlabel.Text = "Request URL";
            // 
            // RequestURLTextBox
            // 
            this.RequestURLTextBox.BackColor = System.Drawing.Color.White;
            this.RequestURLTextBox.Location = new System.Drawing.Point(106, 29);
            this.RequestURLTextBox.Name = "RequestURLTextBox";
            this.RequestURLTextBox.ReadOnly = true;
            this.RequestURLTextBox.Size = new System.Drawing.Size(300, 20);
            this.RequestURLTextBox.TabIndex = 5;
            // 
            // RequestTypeLabel
            // 
            this.RequestTypeLabel.AutoSize = true;
            this.RequestTypeLabel.Location = new System.Drawing.Point(3, 58);
            this.RequestTypeLabel.Name = "RequestTypeLabel";
            this.RequestTypeLabel.Size = new System.Drawing.Size(74, 13);
            this.RequestTypeLabel.TabIndex = 6;
            this.RequestTypeLabel.Text = "Request Type";
            // 
            // RequestTypeTextBox
            // 
            this.RequestTypeTextBox.BackColor = System.Drawing.Color.White;
            this.RequestTypeTextBox.Location = new System.Drawing.Point(106, 55);
            this.RequestTypeTextBox.Name = "RequestTypeTextBox";
            this.RequestTypeTextBox.ReadOnly = true;
            this.RequestTypeTextBox.Size = new System.Drawing.Size(300, 20);
            this.RequestTypeTextBox.TabIndex = 7;
            // 
            // RequestProcessLabel
            // 
            this.RequestProcessLabel.AutoSize = true;
            this.RequestProcessLabel.Location = new System.Drawing.Point(3, 84);
            this.RequestProcessLabel.Name = "RequestProcessLabel";
            this.RequestProcessLabel.Size = new System.Drawing.Size(45, 13);
            this.RequestProcessLabel.TabIndex = 8;
            this.RequestProcessLabel.Text = "Process";
            // 
            // RequestProcessTextBox
            // 
            this.RequestProcessTextBox.BackColor = System.Drawing.Color.White;
            this.RequestProcessTextBox.Location = new System.Drawing.Point(106, 81);
            this.RequestProcessTextBox.Name = "RequestProcessTextBox";
            this.RequestProcessTextBox.ReadOnly = true;
            this.RequestProcessTextBox.Size = new System.Drawing.Size(300, 20);
            this.RequestProcessTextBox.TabIndex = 9;
            // 
            // RequestUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RequestProcessTextBox);
            this.Controls.Add(this.RequestProcessLabel);
            this.Controls.Add(this.RequestTypeTextBox);
            this.Controls.Add(this.RequestTypeLabel);
            this.Controls.Add(this.RequestURLTextBox);
            this.Controls.Add(this.RequestURLlabel);
            this.Controls.Add(this.RequestHostTextBox);
            this.Controls.Add(this.RequestHostlabel);
            this.Controls.Add(this.RequestCommentLabel);
            this.Controls.Add(this.RequestCommentsTextBox);
            this.Name = "RequestUserControl";
            this.Size = new System.Drawing.Size(415, 261);
            this.Load += new System.EventHandler(this.RequestUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox RequestCommentsTextBox;
        private System.Windows.Forms.Label RequestCommentLabel;
        private System.Windows.Forms.Label RequestHostlabel;
        private System.Windows.Forms.TextBox RequestHostTextBox;
        private System.Windows.Forms.Label RequestURLlabel;
        private System.Windows.Forms.TextBox RequestURLTextBox;
        private System.Windows.Forms.Label RequestTypeLabel;
        private System.Windows.Forms.TextBox RequestTypeTextBox;
        private System.Windows.Forms.Label RequestProcessLabel;
        private System.Windows.Forms.TextBox RequestProcessTextBox;
    }
}
