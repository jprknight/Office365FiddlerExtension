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
            this.ResponseCommentLabel = new System.Windows.Forms.Label();
            this.ResponseCommentsTextBox = new System.Windows.Forms.TextBox();
            this.HTTPStatusCodeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.HTTPResponseCodeTextBox = new System.Windows.Forms.TextBox();
            this.HTTPStatusDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.RquestBeginTimeLabel = new System.Windows.Forms.Label();
            this.RequestBeginTimeTextBox = new System.Windows.Forms.TextBox();
            this.RequestEndTimelabel = new System.Windows.Forms.Label();
            this.RequestEndTimeTextBox = new System.Windows.Forms.TextBox();
            this.ElapsedTimeLabel = new System.Windows.Forms.Label();
            this.ElapsedTimeTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ResponseAlertTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ResponseCommentLabel
            // 
            this.ResponseCommentLabel.AutoSize = true;
            this.ResponseCommentLabel.Location = new System.Drawing.Point(3, 130);
            this.ResponseCommentLabel.Name = "ResponseCommentLabel";
            this.ResponseCommentLabel.Size = new System.Drawing.Size(107, 13);
            this.ResponseCommentLabel.TabIndex = 5;
            this.ResponseCommentLabel.Text = "Response Comments";
            this.ResponseCommentLabel.Click += new System.EventHandler(this.ResponseCommentLabel_Click);
            // 
            // ResponseCommentsTextBox
            // 
            this.ResponseCommentsTextBox.BackColor = System.Drawing.Color.White;
            this.ResponseCommentsTextBox.Location = new System.Drawing.Point(6, 146);
            this.ResponseCommentsTextBox.Multiline = true;
            this.ResponseCommentsTextBox.Name = "ResponseCommentsTextBox";
            this.ResponseCommentsTextBox.ReadOnly = true;
            this.ResponseCommentsTextBox.Size = new System.Drawing.Size(400, 125);
            this.ResponseCommentsTextBox.TabIndex = 4;
            // 
            // HTTPStatusCodeLinkLabel
            // 
            this.HTTPStatusCodeLinkLabel.AutoSize = true;
            this.HTTPStatusCodeLinkLabel.Location = new System.Drawing.Point(3, 6);
            this.HTTPStatusCodeLinkLabel.Name = "HTTPStatusCodeLinkLabel";
            this.HTTPStatusCodeLinkLabel.Size = new System.Drawing.Size(97, 13);
            this.HTTPStatusCodeLinkLabel.TabIndex = 9;
            this.HTTPStatusCodeLinkLabel.TabStop = true;
            this.HTTPStatusCodeLinkLabel.Text = "HTTP Status Code";
            this.HTTPStatusCodeLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HTTPStatusCodeLinkLabel_LinkClicked);
            // 
            // HTTPResponseCodeTextBox
            // 
            this.HTTPResponseCodeTextBox.BackColor = System.Drawing.Color.White;
            this.HTTPResponseCodeTextBox.Location = new System.Drawing.Point(106, 3);
            this.HTTPResponseCodeTextBox.Name = "HTTPResponseCodeTextBox";
            this.HTTPResponseCodeTextBox.ReadOnly = true;
            this.HTTPResponseCodeTextBox.Size = new System.Drawing.Size(24, 20);
            this.HTTPResponseCodeTextBox.TabIndex = 10;
            this.HTTPResponseCodeTextBox.TextChanged += new System.EventHandler(this.HTTPResponseCodeTextBox_TextChanged);
            // 
            // HTTPStatusDescriptionTextBox
            // 
            this.HTTPStatusDescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.HTTPStatusDescriptionTextBox.Location = new System.Drawing.Point(136, 3);
            this.HTTPStatusDescriptionTextBox.Name = "HTTPStatusDescriptionTextBox";
            this.HTTPStatusDescriptionTextBox.ReadOnly = true;
            this.HTTPStatusDescriptionTextBox.Size = new System.Drawing.Size(270, 20);
            this.HTTPStatusDescriptionTextBox.TabIndex = 11;
            this.HTTPStatusDescriptionTextBox.TextChanged += new System.EventHandler(this.HTTPStatusDescriptionTextBox_TextChanged);
            // 
            // RquestBeginTimeLabel
            // 
            this.RquestBeginTimeLabel.AutoSize = true;
            this.RquestBeginTimeLabel.Location = new System.Drawing.Point(3, 32);
            this.RquestBeginTimeLabel.Name = "RquestBeginTimeLabel";
            this.RquestBeginTimeLabel.Size = new System.Drawing.Size(77, 13);
            this.RquestBeginTimeLabel.TabIndex = 14;
            this.RquestBeginTimeLabel.Text = "Request Begin";
            // 
            // RequestBeginTimeTextBox
            // 
            this.RequestBeginTimeTextBox.BackColor = System.Drawing.Color.White;
            this.RequestBeginTimeTextBox.Location = new System.Drawing.Point(106, 29);
            this.RequestBeginTimeTextBox.Name = "RequestBeginTimeTextBox";
            this.RequestBeginTimeTextBox.ReadOnly = true;
            this.RequestBeginTimeTextBox.Size = new System.Drawing.Size(300, 20);
            this.RequestBeginTimeTextBox.TabIndex = 15;
            this.RequestBeginTimeTextBox.TextChanged += new System.EventHandler(this.RequestBeginTimeTextBox_TextChanged);
            // 
            // RequestEndTimelabel
            // 
            this.RequestEndTimelabel.AutoSize = true;
            this.RequestEndTimelabel.Location = new System.Drawing.Point(3, 58);
            this.RequestEndTimelabel.Name = "RequestEndTimelabel";
            this.RequestEndTimelabel.Size = new System.Drawing.Size(69, 13);
            this.RequestEndTimelabel.TabIndex = 16;
            this.RequestEndTimelabel.Text = "Request End";
            // 
            // RequestEndTimeTextBox
            // 
            this.RequestEndTimeTextBox.BackColor = System.Drawing.Color.White;
            this.RequestEndTimeTextBox.Location = new System.Drawing.Point(106, 55);
            this.RequestEndTimeTextBox.Name = "RequestEndTimeTextBox";
            this.RequestEndTimeTextBox.ReadOnly = true;
            this.RequestEndTimeTextBox.Size = new System.Drawing.Size(300, 20);
            this.RequestEndTimeTextBox.TabIndex = 17;
            // 
            // ElapsedTimeLabel
            // 
            this.ElapsedTimeLabel.AutoSize = true;
            this.ElapsedTimeLabel.Location = new System.Drawing.Point(3, 84);
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            this.ElapsedTimeLabel.Size = new System.Drawing.Size(71, 13);
            this.ElapsedTimeLabel.TabIndex = 18;
            this.ElapsedTimeLabel.Text = "Elapsed Time";
            // 
            // ElapsedTimeTextBox
            // 
            this.ElapsedTimeTextBox.BackColor = System.Drawing.Color.White;
            this.ElapsedTimeTextBox.Location = new System.Drawing.Point(106, 81);
            this.ElapsedTimeTextBox.Name = "ElapsedTimeTextBox";
            this.ElapsedTimeTextBox.ReadOnly = true;
            this.ElapsedTimeTextBox.Size = new System.Drawing.Size(100, 20);
            this.ElapsedTimeTextBox.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Response Alert";
            // 
            // ResponseAlertTextBox
            // 
            this.ResponseAlertTextBox.Location = new System.Drawing.Point(106, 107);
            this.ResponseAlertTextBox.Name = "ResponseAlertTextBox";
            this.ResponseAlertTextBox.Size = new System.Drawing.Size(300, 20);
            this.ResponseAlertTextBox.TabIndex = 21;
            // 
            // ResponseUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ResponseAlertTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ElapsedTimeTextBox);
            this.Controls.Add(this.ElapsedTimeLabel);
            this.Controls.Add(this.RequestEndTimeTextBox);
            this.Controls.Add(this.RequestEndTimelabel);
            this.Controls.Add(this.RequestBeginTimeTextBox);
            this.Controls.Add(this.RquestBeginTimeLabel);
            this.Controls.Add(this.HTTPStatusDescriptionTextBox);
            this.Controls.Add(this.HTTPResponseCodeTextBox);
            this.Controls.Add(this.HTTPStatusCodeLinkLabel);
            this.Controls.Add(this.ResponseCommentLabel);
            this.Controls.Add(this.ResponseCommentsTextBox);
            this.Name = "ResponseUserControl";
            this.Size = new System.Drawing.Size(418, 285);
            this.Load += new System.EventHandler(this.ResponseUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ResponseCommentLabel;
        private System.Windows.Forms.TextBox ResponseCommentsTextBox;
        private System.Windows.Forms.LinkLabel HTTPStatusCodeLinkLabel;
        private System.Windows.Forms.TextBox HTTPResponseCodeTextBox;
        private System.Windows.Forms.TextBox HTTPStatusDescriptionTextBox;
        private System.Windows.Forms.Label RquestBeginTimeLabel;
        private System.Windows.Forms.TextBox RequestBeginTimeTextBox;
        private System.Windows.Forms.Label RequestEndTimelabel;
        private System.Windows.Forms.TextBox RequestEndTimeTextBox;
        private System.Windows.Forms.Label ElapsedTimeLabel;
        private System.Windows.Forms.TextBox ElapsedTimeTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ResponseAlertTextBox;
    }
}
