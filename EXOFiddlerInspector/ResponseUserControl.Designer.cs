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
            this.ResponseProcessTextBox = new System.Windows.Forms.TextBox();
            this.ResponseProcessLabel = new System.Windows.Forms.Label();
            this.ResponseGroupBox = new System.Windows.Forms.GroupBox();
            this.DataFreshnessLabel = new System.Windows.Forms.Label();
            this.DataFreshnessTextBox = new System.Windows.Forms.TextBox();
            this.ElapsedTimeComemntTextBox = new System.Windows.Forms.TextBox();
            this.RequestBeginDateTextBox = new System.Windows.Forms.TextBox();
            this.RequestEndDateTextBox = new System.Windows.Forms.TextBox();
            this.ResponseCommentsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ResponseGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // HTTPStatusCodeLinkLabel
            // 
            this.HTTPStatusCodeLinkLabel.AutoSize = true;
            this.HTTPStatusCodeLinkLabel.Location = new System.Drawing.Point(6, 6);
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
            this.HTTPResponseCodeTextBox.Location = new System.Drawing.Point(118, 3);
            this.HTTPResponseCodeTextBox.Name = "HTTPResponseCodeTextBox";
            this.HTTPResponseCodeTextBox.ReadOnly = true;
            this.HTTPResponseCodeTextBox.Size = new System.Drawing.Size(24, 20);
            this.HTTPResponseCodeTextBox.TabIndex = 10;
            this.HTTPResponseCodeTextBox.TextChanged += new System.EventHandler(this.HTTPResponseCodeTextBox_TextChanged);
            // 
            // HTTPStatusDescriptionTextBox
            // 
            this.HTTPStatusDescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.HTTPStatusDescriptionTextBox.Location = new System.Drawing.Point(148, 3);
            this.HTTPStatusDescriptionTextBox.Name = "HTTPStatusDescriptionTextBox";
            this.HTTPStatusDescriptionTextBox.ReadOnly = true;
            this.HTTPStatusDescriptionTextBox.Size = new System.Drawing.Size(258, 20);
            this.HTTPStatusDescriptionTextBox.TabIndex = 11;
            this.HTTPStatusDescriptionTextBox.TextChanged += new System.EventHandler(this.HTTPStatusDescriptionTextBox_TextChanged);
            // 
            // RquestBeginTimeLabel
            // 
            this.RquestBeginTimeLabel.AutoSize = true;
            this.RquestBeginTimeLabel.Location = new System.Drawing.Point(6, 32);
            this.RquestBeginTimeLabel.Name = "RquestBeginTimeLabel";
            this.RquestBeginTimeLabel.Size = new System.Drawing.Size(77, 13);
            this.RquestBeginTimeLabel.TabIndex = 14;
            this.RquestBeginTimeLabel.Text = "Request Begin";
            // 
            // RequestBeginTimeTextBox
            // 
            this.RequestBeginTimeTextBox.BackColor = System.Drawing.Color.White;
            this.RequestBeginTimeTextBox.Location = new System.Drawing.Point(212, 29);
            this.RequestBeginTimeTextBox.Name = "RequestBeginTimeTextBox";
            this.RequestBeginTimeTextBox.ReadOnly = true;
            this.RequestBeginTimeTextBox.Size = new System.Drawing.Size(194, 20);
            this.RequestBeginTimeTextBox.TabIndex = 15;
            this.RequestBeginTimeTextBox.TextChanged += new System.EventHandler(this.RequestBeginTimeTextBox_TextChanged);
            // 
            // RequestEndTimelabel
            // 
            this.RequestEndTimelabel.AutoSize = true;
            this.RequestEndTimelabel.Location = new System.Drawing.Point(6, 58);
            this.RequestEndTimelabel.Name = "RequestEndTimelabel";
            this.RequestEndTimelabel.Size = new System.Drawing.Size(69, 13);
            this.RequestEndTimelabel.TabIndex = 16;
            this.RequestEndTimelabel.Text = "Request End";
            // 
            // RequestEndTimeTextBox
            // 
            this.RequestEndTimeTextBox.BackColor = System.Drawing.Color.White;
            this.RequestEndTimeTextBox.Location = new System.Drawing.Point(212, 55);
            this.RequestEndTimeTextBox.Name = "RequestEndTimeTextBox";
            this.RequestEndTimeTextBox.ReadOnly = true;
            this.RequestEndTimeTextBox.Size = new System.Drawing.Size(194, 20);
            this.RequestEndTimeTextBox.TabIndex = 17;
            // 
            // ElapsedTimeLabel
            // 
            this.ElapsedTimeLabel.AutoSize = true;
            this.ElapsedTimeLabel.Location = new System.Drawing.Point(6, 84);
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            this.ElapsedTimeLabel.Size = new System.Drawing.Size(71, 13);
            this.ElapsedTimeLabel.TabIndex = 18;
            this.ElapsedTimeLabel.Text = "Elapsed Time";
            // 
            // ElapsedTimeTextBox
            // 
            this.ElapsedTimeTextBox.BackColor = System.Drawing.Color.White;
            this.ElapsedTimeTextBox.Location = new System.Drawing.Point(118, 81);
            this.ElapsedTimeTextBox.Name = "ElapsedTimeTextBox";
            this.ElapsedTimeTextBox.ReadOnly = true;
            this.ElapsedTimeTextBox.Size = new System.Drawing.Size(88, 20);
            this.ElapsedTimeTextBox.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Response Alert";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ResponseAlertTextBox
            // 
            this.ResponseAlertTextBox.BackColor = System.Drawing.Color.White;
            this.ResponseAlertTextBox.Location = new System.Drawing.Point(118, 159);
            this.ResponseAlertTextBox.Name = "ResponseAlertTextBox";
            this.ResponseAlertTextBox.ReadOnly = true;
            this.ResponseAlertTextBox.Size = new System.Drawing.Size(288, 20);
            this.ResponseAlertTextBox.TabIndex = 21;
            // 
            // ResponseProcessTextBox
            // 
            this.ResponseProcessTextBox.Location = new System.Drawing.Point(118, 133);
            this.ResponseProcessTextBox.Name = "ResponseProcessTextBox";
            this.ResponseProcessTextBox.Size = new System.Drawing.Size(288, 20);
            this.ResponseProcessTextBox.TabIndex = 22;
            // 
            // ResponseProcessLabel
            // 
            this.ResponseProcessLabel.AutoSize = true;
            this.ResponseProcessLabel.Location = new System.Drawing.Point(6, 136);
            this.ResponseProcessLabel.Name = "ResponseProcessLabel";
            this.ResponseProcessLabel.Size = new System.Drawing.Size(45, 13);
            this.ResponseProcessLabel.TabIndex = 23;
            this.ResponseProcessLabel.Text = "Process";
            // 
            // ResponseGroupBox
            // 
            this.ResponseGroupBox.Controls.Add(this.ResponseCommentsRichTextBox);
            this.ResponseGroupBox.Location = new System.Drawing.Point(9, 185);
            this.ResponseGroupBox.Name = "ResponseGroupBox";
            this.ResponseGroupBox.Size = new System.Drawing.Size(397, 154);
            this.ResponseGroupBox.TabIndex = 25;
            this.ResponseGroupBox.TabStop = false;
            this.ResponseGroupBox.Text = "Response Comments";
            this.ResponseGroupBox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // DataFreshnessLabel
            // 
            this.DataFreshnessLabel.AutoSize = true;
            this.DataFreshnessLabel.Location = new System.Drawing.Point(6, 110);
            this.DataFreshnessLabel.Name = "DataFreshnessLabel";
            this.DataFreshnessLabel.Size = new System.Drawing.Size(81, 13);
            this.DataFreshnessLabel.TabIndex = 26;
            this.DataFreshnessLabel.Text = "Data Freshness";
            // 
            // DataFreshnessTextBox
            // 
            this.DataFreshnessTextBox.BackColor = System.Drawing.Color.White;
            this.DataFreshnessTextBox.Location = new System.Drawing.Point(118, 107);
            this.DataFreshnessTextBox.Name = "DataFreshnessTextBox";
            this.DataFreshnessTextBox.ReadOnly = true;
            this.DataFreshnessTextBox.Size = new System.Drawing.Size(288, 20);
            this.DataFreshnessTextBox.TabIndex = 27;
            // 
            // ElapsedTimeComemntTextBox
            // 
            this.ElapsedTimeComemntTextBox.BackColor = System.Drawing.Color.White;
            this.ElapsedTimeComemntTextBox.Location = new System.Drawing.Point(212, 81);
            this.ElapsedTimeComemntTextBox.Name = "ElapsedTimeComemntTextBox";
            this.ElapsedTimeComemntTextBox.ReadOnly = true;
            this.ElapsedTimeComemntTextBox.Size = new System.Drawing.Size(194, 20);
            this.ElapsedTimeComemntTextBox.TabIndex = 30;
            // 
            // RequestBeginDateTextBox
            // 
            this.RequestBeginDateTextBox.BackColor = System.Drawing.Color.White;
            this.RequestBeginDateTextBox.Location = new System.Drawing.Point(118, 29);
            this.RequestBeginDateTextBox.Name = "RequestBeginDateTextBox";
            this.RequestBeginDateTextBox.ReadOnly = true;
            this.RequestBeginDateTextBox.Size = new System.Drawing.Size(88, 20);
            this.RequestBeginDateTextBox.TabIndex = 31;
            // 
            // RequestEndDateTextBox
            // 
            this.RequestEndDateTextBox.BackColor = System.Drawing.Color.White;
            this.RequestEndDateTextBox.Location = new System.Drawing.Point(118, 56);
            this.RequestEndDateTextBox.Name = "RequestEndDateTextBox";
            this.RequestEndDateTextBox.ReadOnly = true;
            this.RequestEndDateTextBox.Size = new System.Drawing.Size(88, 20);
            this.RequestEndDateTextBox.TabIndex = 32;
            // 
            // ResponseCommentsRichTextBox
            // 
            this.ResponseCommentsRichTextBox.Location = new System.Drawing.Point(6, 19);
            this.ResponseCommentsRichTextBox.Name = "ResponseCommentsRichTextBox";
            this.ResponseCommentsRichTextBox.ReadOnly = true;
            this.ResponseCommentsRichTextBox.Size = new System.Drawing.Size(385, 125);
            this.ResponseCommentsRichTextBox.TabIndex = 33;
            this.ResponseCommentsRichTextBox.Text = "";
            // 
            // ResponseUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RequestEndDateTextBox);
            this.Controls.Add(this.RequestBeginDateTextBox);
            this.Controls.Add(this.ElapsedTimeComemntTextBox);
            this.Controls.Add(this.DataFreshnessTextBox);
            this.Controls.Add(this.DataFreshnessLabel);
            this.Controls.Add(this.ResponseGroupBox);
            this.Controls.Add(this.ResponseProcessLabel);
            this.Controls.Add(this.ResponseProcessTextBox);
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
            this.Name = "ResponseUserControl";
            this.Size = new System.Drawing.Size(412, 350);
            this.Load += new System.EventHandler(this.ResponseUserControl_Load);
            this.ResponseGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.TextBox ResponseProcessTextBox;
        private System.Windows.Forms.Label ResponseProcessLabel;
        private System.Windows.Forms.GroupBox ResponseGroupBox;
        private System.Windows.Forms.Label DataFreshnessLabel;
        private System.Windows.Forms.TextBox DataFreshnessTextBox;
        private System.Windows.Forms.TextBox ElapsedTimeComemntTextBox;
        private System.Windows.Forms.TextBox RequestBeginDateTextBox;
        private System.Windows.Forms.TextBox RequestEndDateTextBox;
        private System.Windows.Forms.RichTextBox ResponseCommentsRichTextBox;
    }
}
