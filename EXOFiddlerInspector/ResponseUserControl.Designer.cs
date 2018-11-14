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
            this.ResponseCommentsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.DataAgeLabel = new System.Windows.Forms.Label();
            this.DataAgeTextBox = new System.Windows.Forms.TextBox();
            this.ElapsedTimeCommentTextBox = new System.Windows.Forms.TextBox();
            this.RequestBeginDateTextBox = new System.Windows.Forms.TextBox();
            this.RequestEndDateTextBox = new System.Windows.Forms.TextBox();
            this.ResponseServerLabel = new System.Windows.Forms.Label();
            this.ResponseServerTextBox = new System.Windows.Forms.TextBox();
            this.RequestHeadersTextBox = new System.Windows.Forms.TextBox();
            this.RequestHeadersLabel = new System.Windows.Forms.Label();
            this.RequestBodyLabel = new System.Windows.Forms.Label();
            this.RequestBodyTextbox = new System.Windows.Forms.TextBox();
            this.ResponseHeadersLabel = new System.Windows.Forms.Label();
            this.ResponseHeadersTextbox = new System.Windows.Forms.TextBox();
            this.ResponseBodyLabel = new System.Windows.Forms.Label();
            this.ResponseBodyTextbox = new System.Windows.Forms.TextBox();
            this.SaveSessionDataButton = new System.Windows.Forms.Button();
            this.ExchangeTypeLabel = new System.Windows.Forms.Label();
            this.ExchangeTypeTextbox = new System.Windows.Forms.TextBox();
            this.SessionIDTextbox = new System.Windows.Forms.TextBox();
            this.SessionIDLabel = new System.Windows.Forms.Label();
            this.OpenSessionData = new System.Windows.Forms.Button();
            this.DeveloperSessionGroupBox = new System.Windows.Forms.GroupBox();
            this.RemoveAllAppPrefsButton = new System.Windows.Forms.Button();
            this.DeveloperControlsLabel = new System.Windows.Forms.Label();
            this.ResponseGroupBox.SuspendLayout();
            this.DeveloperSessionGroupBox.SuspendLayout();
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
            this.HTTPStatusDescriptionTextBox.Size = new System.Drawing.Size(190, 20);
            this.HTTPStatusDescriptionTextBox.TabIndex = 11;
            this.HTTPStatusDescriptionTextBox.TextChanged += new System.EventHandler(this.HTTPStatusDescriptionTextBox_TextChanged);
            // 
            // RquestBeginTimeLabel
            // 
            this.RquestBeginTimeLabel.AutoSize = true;
            this.RquestBeginTimeLabel.Location = new System.Drawing.Point(6, 32);
            this.RquestBeginTimeLabel.Name = "RquestBeginTimeLabel";
            this.RquestBeginTimeLabel.Size = new System.Drawing.Size(106, 13);
            this.RquestBeginTimeLabel.TabIndex = 14;
            this.RquestBeginTimeLabel.Text = "Client Begin Request";
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
            this.RequestEndTimelabel.Size = new System.Drawing.Size(113, 13);
            this.RequestEndTimelabel.TabIndex = 16;
            this.RequestEndTimelabel.Text = "Client Done Response";
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
            this.label1.Location = new System.Drawing.Point(6, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Response Alert";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ResponseAlertTextBox
            // 
            this.ResponseAlertTextBox.BackColor = System.Drawing.Color.White;
            this.ResponseAlertTextBox.Location = new System.Drawing.Point(118, 210);
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
            this.ResponseProcessLabel.Size = new System.Drawing.Size(74, 13);
            this.ResponseProcessLabel.TabIndex = 23;
            this.ResponseProcessLabel.Text = "Local Process";
            // 
            // ResponseGroupBox
            // 
            this.ResponseGroupBox.Controls.Add(this.ResponseCommentsRichTextBox);
            this.ResponseGroupBox.Location = new System.Drawing.Point(9, 240);
            this.ResponseGroupBox.Name = "ResponseGroupBox";
            this.ResponseGroupBox.Size = new System.Drawing.Size(397, 154);
            this.ResponseGroupBox.TabIndex = 25;
            this.ResponseGroupBox.TabStop = false;
            this.ResponseGroupBox.Text = "Response Comments";
            this.ResponseGroupBox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // ResponseCommentsRichTextBox
            // 
            this.ResponseCommentsRichTextBox.BackColor = System.Drawing.Color.White;
            this.ResponseCommentsRichTextBox.Location = new System.Drawing.Point(6, 19);
            this.ResponseCommentsRichTextBox.Name = "ResponseCommentsRichTextBox";
            this.ResponseCommentsRichTextBox.ReadOnly = true;
            this.ResponseCommentsRichTextBox.Size = new System.Drawing.Size(385, 125);
            this.ResponseCommentsRichTextBox.TabIndex = 33;
            this.ResponseCommentsRichTextBox.Text = "";
            this.ResponseCommentsRichTextBox.TextChanged += new System.EventHandler(this.ResponseCommentsRichTextBox_TextChanged);
            // 
            // DataAgeLabel
            // 
            this.DataAgeLabel.AutoSize = true;
            this.DataAgeLabel.Location = new System.Drawing.Point(6, 110);
            this.DataAgeLabel.Name = "DataAgeLabel";
            this.DataAgeLabel.Size = new System.Drawing.Size(52, 13);
            this.DataAgeLabel.TabIndex = 26;
            this.DataAgeLabel.Text = "Data Age";
            // 
            // DataAgeTextBox
            // 
            this.DataAgeTextBox.BackColor = System.Drawing.Color.White;
            this.DataAgeTextBox.Location = new System.Drawing.Point(118, 107);
            this.DataAgeTextBox.Name = "DataAgeTextBox";
            this.DataAgeTextBox.ReadOnly = true;
            this.DataAgeTextBox.Size = new System.Drawing.Size(288, 20);
            this.DataAgeTextBox.TabIndex = 27;
            // 
            // ElapsedTimeCommentTextBox
            // 
            this.ElapsedTimeCommentTextBox.BackColor = System.Drawing.Color.White;
            this.ElapsedTimeCommentTextBox.Location = new System.Drawing.Point(212, 81);
            this.ElapsedTimeCommentTextBox.Name = "ElapsedTimeCommentTextBox";
            this.ElapsedTimeCommentTextBox.ReadOnly = true;
            this.ElapsedTimeCommentTextBox.Size = new System.Drawing.Size(194, 20);
            this.ElapsedTimeCommentTextBox.TabIndex = 30;
            this.ElapsedTimeCommentTextBox.TextChanged += new System.EventHandler(this.ElapsedTimeComemntTextBox_TextChanged);
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
            // ResponseServerLabel
            // 
            this.ResponseServerLabel.AutoSize = true;
            this.ResponseServerLabel.Location = new System.Drawing.Point(6, 187);
            this.ResponseServerLabel.Name = "ResponseServerLabel";
            this.ResponseServerLabel.Size = new System.Drawing.Size(89, 13);
            this.ResponseServerLabel.TabIndex = 33;
            this.ResponseServerLabel.Text = "Response Server";
            // 
            // ResponseServerTextBox
            // 
            this.ResponseServerTextBox.BackColor = System.Drawing.Color.White;
            this.ResponseServerTextBox.Location = new System.Drawing.Point(118, 184);
            this.ResponseServerTextBox.Name = "ResponseServerTextBox";
            this.ResponseServerTextBox.ReadOnly = true;
            this.ResponseServerTextBox.Size = new System.Drawing.Size(288, 20);
            this.ResponseServerTextBox.TabIndex = 34;
            this.ResponseServerTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // RequestHeadersTextBox
            // 
            this.RequestHeadersTextBox.Location = new System.Drawing.Point(6, 32);
            this.RequestHeadersTextBox.Multiline = true;
            this.RequestHeadersTextBox.Name = "RequestHeadersTextBox";
            this.RequestHeadersTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RequestHeadersTextBox.Size = new System.Drawing.Size(385, 40);
            this.RequestHeadersTextBox.TabIndex = 36;
            // 
            // RequestHeadersLabel
            // 
            this.RequestHeadersLabel.AutoSize = true;
            this.RequestHeadersLabel.Location = new System.Drawing.Point(3, 16);
            this.RequestHeadersLabel.Name = "RequestHeadersLabel";
            this.RequestHeadersLabel.Size = new System.Drawing.Size(90, 13);
            this.RequestHeadersLabel.TabIndex = 37;
            this.RequestHeadersLabel.Text = "Request Headers";
            // 
            // RequestBodyLabel
            // 
            this.RequestBodyLabel.AutoSize = true;
            this.RequestBodyLabel.Location = new System.Drawing.Point(3, 140);
            this.RequestBodyLabel.Name = "RequestBodyLabel";
            this.RequestBodyLabel.Size = new System.Drawing.Size(74, 13);
            this.RequestBodyLabel.TabIndex = 38;
            this.RequestBodyLabel.Text = "Request Body";
            // 
            // RequestBodyTextbox
            // 
            this.RequestBodyTextbox.Location = new System.Drawing.Point(6, 156);
            this.RequestBodyTextbox.Multiline = true;
            this.RequestBodyTextbox.Name = "RequestBodyTextbox";
            this.RequestBodyTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RequestBodyTextbox.Size = new System.Drawing.Size(385, 40);
            this.RequestBodyTextbox.TabIndex = 39;
            // 
            // ResponseHeadersLabel
            // 
            this.ResponseHeadersLabel.AutoSize = true;
            this.ResponseHeadersLabel.Location = new System.Drawing.Point(3, 81);
            this.ResponseHeadersLabel.Name = "ResponseHeadersLabel";
            this.ResponseHeadersLabel.Size = new System.Drawing.Size(98, 13);
            this.ResponseHeadersLabel.TabIndex = 40;
            this.ResponseHeadersLabel.Text = "Response Headers";
            // 
            // ResponseHeadersTextbox
            // 
            this.ResponseHeadersTextbox.Location = new System.Drawing.Point(6, 97);
            this.ResponseHeadersTextbox.Multiline = true;
            this.ResponseHeadersTextbox.Name = "ResponseHeadersTextbox";
            this.ResponseHeadersTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResponseHeadersTextbox.Size = new System.Drawing.Size(385, 40);
            this.ResponseHeadersTextbox.TabIndex = 41;
            // 
            // ResponseBodyLabel
            // 
            this.ResponseBodyLabel.AutoSize = true;
            this.ResponseBodyLabel.Location = new System.Drawing.Point(3, 199);
            this.ResponseBodyLabel.Name = "ResponseBodyLabel";
            this.ResponseBodyLabel.Size = new System.Drawing.Size(82, 13);
            this.ResponseBodyLabel.TabIndex = 42;
            this.ResponseBodyLabel.Text = "Response Body";
            // 
            // ResponseBodyTextbox
            // 
            this.ResponseBodyTextbox.Location = new System.Drawing.Point(6, 215);
            this.ResponseBodyTextbox.Multiline = true;
            this.ResponseBodyTextbox.Name = "ResponseBodyTextbox";
            this.ResponseBodyTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResponseBodyTextbox.Size = new System.Drawing.Size(385, 40);
            this.ResponseBodyTextbox.TabIndex = 43;
            // 
            // SaveSessionDataButton
            // 
            this.SaveSessionDataButton.Location = new System.Drawing.Point(284, 400);
            this.SaveSessionDataButton.Name = "SaveSessionDataButton";
            this.SaveSessionDataButton.Size = new System.Drawing.Size(122, 23);
            this.SaveSessionDataButton.TabIndex = 44;
            this.SaveSessionDataButton.Text = "Save Session Data";
            this.SaveSessionDataButton.UseVisualStyleBackColor = true;
            this.SaveSessionDataButton.Click += new System.EventHandler(this.SaveSessionDataButton_Click);
            // 
            // ExchangeTypeLabel
            // 
            this.ExchangeTypeLabel.AutoSize = true;
            this.ExchangeTypeLabel.Location = new System.Drawing.Point(6, 162);
            this.ExchangeTypeLabel.Name = "ExchangeTypeLabel";
            this.ExchangeTypeLabel.Size = new System.Drawing.Size(82, 13);
            this.ExchangeTypeLabel.TabIndex = 45;
            this.ExchangeTypeLabel.Text = "Exchange Type";
            // 
            // ExchangeTypeTextbox
            // 
            this.ExchangeTypeTextbox.BackColor = System.Drawing.Color.White;
            this.ExchangeTypeTextbox.Location = new System.Drawing.Point(118, 159);
            this.ExchangeTypeTextbox.Name = "ExchangeTypeTextbox";
            this.ExchangeTypeTextbox.ReadOnly = true;
            this.ExchangeTypeTextbox.Size = new System.Drawing.Size(288, 20);
            this.ExchangeTypeTextbox.TabIndex = 46;
            // 
            // SessionIDTextbox
            // 
            this.SessionIDTextbox.BackColor = System.Drawing.Color.White;
            this.SessionIDTextbox.Location = new System.Drawing.Point(364, 3);
            this.SessionIDTextbox.Name = "SessionIDTextbox";
            this.SessionIDTextbox.ReadOnly = true;
            this.SessionIDTextbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SessionIDTextbox.Size = new System.Drawing.Size(42, 20);
            this.SessionIDTextbox.TabIndex = 47;
            // 
            // SessionIDLabel
            // 
            this.SessionIDLabel.AutoSize = true;
            this.SessionIDLabel.Location = new System.Drawing.Point(344, 6);
            this.SessionIDLabel.Name = "SessionIDLabel";
            this.SessionIDLabel.Size = new System.Drawing.Size(18, 13);
            this.SessionIDLabel.TabIndex = 48;
            this.SessionIDLabel.Text = "ID";
            // 
            // OpenSessionData
            // 
            this.OpenSessionData.Location = new System.Drawing.Point(156, 400);
            this.OpenSessionData.Name = "OpenSessionData";
            this.OpenSessionData.Size = new System.Drawing.Size(122, 23);
            this.OpenSessionData.TabIndex = 49;
            this.OpenSessionData.Text = "Open Session Data";
            this.OpenSessionData.UseVisualStyleBackColor = true;
            this.OpenSessionData.Click += new System.EventHandler(this.OpenSessionData_Click);
            // 
            // DeveloperSessionGroupBox
            // 
            this.DeveloperSessionGroupBox.Controls.Add(this.RemoveAllAppPrefsButton);
            this.DeveloperSessionGroupBox.Controls.Add(this.RequestBodyTextbox);
            this.DeveloperSessionGroupBox.Controls.Add(this.RequestHeadersTextBox);
            this.DeveloperSessionGroupBox.Controls.Add(this.RequestHeadersLabel);
            this.DeveloperSessionGroupBox.Controls.Add(this.RequestBodyLabel);
            this.DeveloperSessionGroupBox.Controls.Add(this.ResponseHeadersLabel);
            this.DeveloperSessionGroupBox.Controls.Add(this.ResponseHeadersTextbox);
            this.DeveloperSessionGroupBox.Controls.Add(this.ResponseBodyLabel);
            this.DeveloperSessionGroupBox.Controls.Add(this.ResponseBodyTextbox);
            this.DeveloperSessionGroupBox.Location = new System.Drawing.Point(412, 3);
            this.DeveloperSessionGroupBox.Name = "DeveloperSessionGroupBox";
            this.DeveloperSessionGroupBox.Size = new System.Drawing.Size(397, 295);
            this.DeveloperSessionGroupBox.TabIndex = 54;
            this.DeveloperSessionGroupBox.TabStop = false;
            this.DeveloperSessionGroupBox.Text = "DeveloperSessionGroupBox";
            this.DeveloperSessionGroupBox.Visible = false;
            // 
            // RemoveAllAppPrefsButton
            // 
            this.RemoveAllAppPrefsButton.Location = new System.Drawing.Point(6, 261);
            this.RemoveAllAppPrefsButton.Name = "RemoveAllAppPrefsButton";
            this.RemoveAllAppPrefsButton.Size = new System.Drawing.Size(163, 23);
            this.RemoveAllAppPrefsButton.TabIndex = 45;
            this.RemoveAllAppPrefsButton.Text = "Remove All Fiddler App Prefs";
            this.RemoveAllAppPrefsButton.UseVisualStyleBackColor = true;
            this.RemoveAllAppPrefsButton.Click += new System.EventHandler(this.RemoveAllAppPrefsButton_Click);
            // 
            // DeveloperControlsLabel
            // 
            this.DeveloperControlsLabel.AutoSize = true;
            this.DeveloperControlsLabel.Location = new System.Drawing.Point(247, 426);
            this.DeveloperControlsLabel.Name = "DeveloperControlsLabel";
            this.DeveloperControlsLabel.Size = new System.Drawing.Size(326, 13);
            this.DeveloperControlsLabel.TabIndex = 56;
            this.DeveloperControlsLabel.Text = "Developer mode enabled: Additional controls on the right hand side.";
            this.DeveloperControlsLabel.Visible = false;
            this.DeveloperControlsLabel.Click += new System.EventHandler(this.DeveloperControlsLabel_Click);
            // 
            // ResponseUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DeveloperControlsLabel);
            this.Controls.Add(this.DeveloperSessionGroupBox);
            this.Controls.Add(this.OpenSessionData);
            this.Controls.Add(this.SessionIDLabel);
            this.Controls.Add(this.SessionIDTextbox);
            this.Controls.Add(this.ExchangeTypeTextbox);
            this.Controls.Add(this.ExchangeTypeLabel);
            this.Controls.Add(this.SaveSessionDataButton);
            this.Controls.Add(this.ResponseServerTextBox);
            this.Controls.Add(this.ResponseServerLabel);
            this.Controls.Add(this.RequestEndDateTextBox);
            this.Controls.Add(this.RequestBeginDateTextBox);
            this.Controls.Add(this.ElapsedTimeCommentTextBox);
            this.Controls.Add(this.DataAgeTextBox);
            this.Controls.Add(this.DataAgeLabel);
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
            this.Size = new System.Drawing.Size(818, 466);
            this.Load += new System.EventHandler(this.ResponseUserControl_Load);
            this.ResponseGroupBox.ResumeLayout(false);
            this.DeveloperSessionGroupBox.ResumeLayout(false);
            this.DeveloperSessionGroupBox.PerformLayout();
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
        private System.Windows.Forms.Label DataAgeLabel;
        private System.Windows.Forms.TextBox DataAgeTextBox;
        private System.Windows.Forms.TextBox ElapsedTimeCommentTextBox;
        private System.Windows.Forms.TextBox RequestBeginDateTextBox;
        private System.Windows.Forms.TextBox RequestEndDateTextBox;
        private System.Windows.Forms.RichTextBox ResponseCommentsRichTextBox;
        private System.Windows.Forms.Label ResponseServerLabel;
        private System.Windows.Forms.TextBox ResponseServerTextBox;
        private System.Windows.Forms.TextBox RequestHeadersTextBox;
        private System.Windows.Forms.Label RequestHeadersLabel;
        private System.Windows.Forms.Label RequestBodyLabel;
        private System.Windows.Forms.TextBox RequestBodyTextbox;
        private System.Windows.Forms.Label ResponseHeadersLabel;
        private System.Windows.Forms.TextBox ResponseHeadersTextbox;
        private System.Windows.Forms.Label ResponseBodyLabel;
        private System.Windows.Forms.TextBox ResponseBodyTextbox;
        private System.Windows.Forms.Button SaveSessionDataButton;
        private System.Windows.Forms.Label ExchangeTypeLabel;
        private System.Windows.Forms.TextBox ExchangeTypeTextbox;
        private System.Windows.Forms.TextBox SessionIDTextbox;
        private System.Windows.Forms.Label SessionIDLabel;
        private System.Windows.Forms.Button OpenSessionData;
        private System.Windows.Forms.GroupBox DeveloperSessionGroupBox;
        private System.Windows.Forms.Label DeveloperControlsLabel;
        private System.Windows.Forms.Button RemoveAllAppPrefsButton;
    }
}
