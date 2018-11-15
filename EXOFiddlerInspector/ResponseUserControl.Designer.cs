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
            this.ClientRquestBeginTimeLabel = new System.Windows.Forms.Label();
            this.ClientRequestBeginTimeTextBox = new System.Windows.Forms.TextBox();
            this.ClientRequestEndTimelabel = new System.Windows.Forms.Label();
            this.ClientRequestEndTimeTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ResponseAlertTextBox = new System.Windows.Forms.TextBox();
            this.ResponseProcessTextBox = new System.Windows.Forms.TextBox();
            this.ResponseProcessLabel = new System.Windows.Forms.Label();
            this.ResponseCommentsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.DataAgeLabel = new System.Windows.Forms.Label();
            this.DataAgeTextBox = new System.Windows.Forms.TextBox();
            this.ClientRequestBeginDateTextBox = new System.Windows.Forms.TextBox();
            this.ClientRequestEndDateTextBox = new System.Windows.Forms.TextBox();
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
            this.ClientDurationLabel = new System.Windows.Forms.Label();
            this.OverallElapsedTextbox = new System.Windows.Forms.TextBox();
            this.ServerGotRequestLabel = new System.Windows.Forms.Label();
            this.ServerGotRequestDateTextbox = new System.Windows.Forms.TextBox();
            this.ServerDoneResponseLabel = new System.Windows.Forms.Label();
            this.ServerDoneResponseDateTextbox = new System.Windows.Forms.TextBox();
            this.ServerDoneResponseTimeTextbox = new System.Windows.Forms.TextBox();
            this.ServerThinkTimeLabel = new System.Windows.Forms.Label();
            this.ServerThinkTimeTextbox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ServerBeginResponseLabel = new System.Windows.Forms.Label();
            this.ServerBeginResponseTimeTextbox = new System.Windows.Forms.TextBox();
            this.ServerGotRequestTimeTextbox = new System.Windows.Forms.TextBox();
            this.ServerBeginResponseDateTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.XHostIPTextbox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TransmitLabel = new System.Windows.Forms.Label();
            this.TransmitTimeTextbox = new System.Windows.Forms.TextBox();
            this.TransmitGroupBox = new System.Windows.Forms.GroupBox();
            this.DeveloperSessionGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.TransmitGroupBox.SuspendLayout();
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
            this.HTTPResponseCodeTextBox.Location = new System.Drawing.Point(131, 3);
            this.HTTPResponseCodeTextBox.Name = "HTTPResponseCodeTextBox";
            this.HTTPResponseCodeTextBox.ReadOnly = true;
            this.HTTPResponseCodeTextBox.Size = new System.Drawing.Size(24, 20);
            this.HTTPResponseCodeTextBox.TabIndex = 10;
            this.HTTPResponseCodeTextBox.TextChanged += new System.EventHandler(this.HTTPResponseCodeTextBox_TextChanged);
            // 
            // HTTPStatusDescriptionTextBox
            // 
            this.HTTPStatusDescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.HTTPStatusDescriptionTextBox.Location = new System.Drawing.Point(161, 3);
            this.HTTPStatusDescriptionTextBox.Name = "HTTPStatusDescriptionTextBox";
            this.HTTPStatusDescriptionTextBox.ReadOnly = true;
            this.HTTPStatusDescriptionTextBox.Size = new System.Drawing.Size(173, 20);
            this.HTTPStatusDescriptionTextBox.TabIndex = 11;
            this.HTTPStatusDescriptionTextBox.TextChanged += new System.EventHandler(this.HTTPStatusDescriptionTextBox_TextChanged);
            // 
            // ClientRquestBeginTimeLabel
            // 
            this.ClientRquestBeginTimeLabel.AutoSize = true;
            this.ClientRquestBeginTimeLabel.Location = new System.Drawing.Point(6, 22);
            this.ClientRquestBeginTimeLabel.Name = "ClientRquestBeginTimeLabel";
            this.ClientRquestBeginTimeLabel.Size = new System.Drawing.Size(106, 13);
            this.ClientRquestBeginTimeLabel.TabIndex = 14;
            this.ClientRquestBeginTimeLabel.Text = "Client Begin Request";
            // 
            // ClientRequestBeginTimeTextBox
            // 
            this.ClientRequestBeginTimeTextBox.BackColor = System.Drawing.Color.White;
            this.ClientRequestBeginTimeTextBox.Location = new System.Drawing.Point(205, 19);
            this.ClientRequestBeginTimeTextBox.Name = "ClientRequestBeginTimeTextBox";
            this.ClientRequestBeginTimeTextBox.ReadOnly = true;
            this.ClientRequestBeginTimeTextBox.Size = new System.Drawing.Size(73, 20);
            this.ClientRequestBeginTimeTextBox.TabIndex = 15;
            this.ClientRequestBeginTimeTextBox.TextChanged += new System.EventHandler(this.RequestBeginTimeTextBox_TextChanged);
            // 
            // ClientRequestEndTimelabel
            // 
            this.ClientRequestEndTimelabel.AutoSize = true;
            this.ClientRequestEndTimelabel.Location = new System.Drawing.Point(6, 48);
            this.ClientRequestEndTimelabel.Name = "ClientRequestEndTimelabel";
            this.ClientRequestEndTimelabel.Size = new System.Drawing.Size(113, 13);
            this.ClientRequestEndTimelabel.TabIndex = 16;
            this.ClientRequestEndTimelabel.Text = "Client Done Response";
            // 
            // ClientRequestEndTimeTextBox
            // 
            this.ClientRequestEndTimeTextBox.BackColor = System.Drawing.Color.White;
            this.ClientRequestEndTimeTextBox.Location = new System.Drawing.Point(205, 45);
            this.ClientRequestEndTimeTextBox.Name = "ClientRequestEndTimeTextBox";
            this.ClientRequestEndTimeTextBox.ReadOnly = true;
            this.ClientRequestEndTimeTextBox.Size = new System.Drawing.Size(73, 20);
            this.ClientRequestEndTimeTextBox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Response Alert";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ResponseAlertTextBox
            // 
            this.ResponseAlertTextBox.BackColor = System.Drawing.Color.White;
            this.ResponseAlertTextBox.Location = new System.Drawing.Point(131, 148);
            this.ResponseAlertTextBox.Name = "ResponseAlertTextBox";
            this.ResponseAlertTextBox.ReadOnly = true;
            this.ResponseAlertTextBox.Size = new System.Drawing.Size(269, 20);
            this.ResponseAlertTextBox.TabIndex = 21;
            // 
            // ResponseProcessTextBox
            // 
            this.ResponseProcessTextBox.Location = new System.Drawing.Point(131, 45);
            this.ResponseProcessTextBox.Name = "ResponseProcessTextBox";
            this.ResponseProcessTextBox.Size = new System.Drawing.Size(269, 20);
            this.ResponseProcessTextBox.TabIndex = 22;
            // 
            // ResponseProcessLabel
            // 
            this.ResponseProcessLabel.AutoSize = true;
            this.ResponseProcessLabel.Location = new System.Drawing.Point(6, 48);
            this.ResponseProcessLabel.Name = "ResponseProcessLabel";
            this.ResponseProcessLabel.Size = new System.Drawing.Size(74, 13);
            this.ResponseProcessLabel.TabIndex = 23;
            this.ResponseProcessLabel.Text = "Local Process";
            // 
            // ResponseCommentsRichTextBox
            // 
            this.ResponseCommentsRichTextBox.BackColor = System.Drawing.Color.White;
            this.ResponseCommentsRichTextBox.Location = new System.Drawing.Point(6, 174);
            this.ResponseCommentsRichTextBox.Name = "ResponseCommentsRichTextBox";
            this.ResponseCommentsRichTextBox.ReadOnly = true;
            this.ResponseCommentsRichTextBox.Size = new System.Drawing.Size(394, 125);
            this.ResponseCommentsRichTextBox.TabIndex = 33;
            this.ResponseCommentsRichTextBox.Text = "";
            this.ResponseCommentsRichTextBox.TextChanged += new System.EventHandler(this.ResponseCommentsRichTextBox_TextChanged);
            // 
            // DataAgeLabel
            // 
            this.DataAgeLabel.AutoSize = true;
            this.DataAgeLabel.Location = new System.Drawing.Point(6, 22);
            this.DataAgeLabel.Name = "DataAgeLabel";
            this.DataAgeLabel.Size = new System.Drawing.Size(52, 13);
            this.DataAgeLabel.TabIndex = 26;
            this.DataAgeLabel.Text = "Data Age";
            // 
            // DataAgeTextBox
            // 
            this.DataAgeTextBox.BackColor = System.Drawing.Color.White;
            this.DataAgeTextBox.Location = new System.Drawing.Point(131, 19);
            this.DataAgeTextBox.Name = "DataAgeTextBox";
            this.DataAgeTextBox.ReadOnly = true;
            this.DataAgeTextBox.Size = new System.Drawing.Size(269, 20);
            this.DataAgeTextBox.TabIndex = 27;
            // 
            // ClientRequestBeginDateTextBox
            // 
            this.ClientRequestBeginDateTextBox.BackColor = System.Drawing.Color.White;
            this.ClientRequestBeginDateTextBox.Location = new System.Drawing.Point(131, 19);
            this.ClientRequestBeginDateTextBox.Name = "ClientRequestBeginDateTextBox";
            this.ClientRequestBeginDateTextBox.ReadOnly = true;
            this.ClientRequestBeginDateTextBox.Size = new System.Drawing.Size(68, 20);
            this.ClientRequestBeginDateTextBox.TabIndex = 31;
            // 
            // ClientRequestEndDateTextBox
            // 
            this.ClientRequestEndDateTextBox.BackColor = System.Drawing.Color.White;
            this.ClientRequestEndDateTextBox.Location = new System.Drawing.Point(131, 45);
            this.ClientRequestEndDateTextBox.Name = "ClientRequestEndDateTextBox";
            this.ClientRequestEndDateTextBox.ReadOnly = true;
            this.ClientRequestEndDateTextBox.Size = new System.Drawing.Size(68, 20);
            this.ClientRequestEndDateTextBox.TabIndex = 32;
            // 
            // ResponseServerLabel
            // 
            this.ResponseServerLabel.AutoSize = true;
            this.ResponseServerLabel.Location = new System.Drawing.Point(6, 99);
            this.ResponseServerLabel.Name = "ResponseServerLabel";
            this.ResponseServerLabel.Size = new System.Drawing.Size(89, 13);
            this.ResponseServerLabel.TabIndex = 33;
            this.ResponseServerLabel.Text = "Response Server";
            // 
            // ResponseServerTextBox
            // 
            this.ResponseServerTextBox.BackColor = System.Drawing.Color.White;
            this.ResponseServerTextBox.Location = new System.Drawing.Point(131, 96);
            this.ResponseServerTextBox.Name = "ResponseServerTextBox";
            this.ResponseServerTextBox.ReadOnly = true;
            this.ResponseServerTextBox.Size = new System.Drawing.Size(269, 20);
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
            this.SaveSessionDataButton.Location = new System.Drawing.Point(272, 305);
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
            this.ExchangeTypeLabel.Location = new System.Drawing.Point(6, 74);
            this.ExchangeTypeLabel.Name = "ExchangeTypeLabel";
            this.ExchangeTypeLabel.Size = new System.Drawing.Size(82, 13);
            this.ExchangeTypeLabel.TabIndex = 45;
            this.ExchangeTypeLabel.Text = "Exchange Type";
            // 
            // ExchangeTypeTextbox
            // 
            this.ExchangeTypeTextbox.BackColor = System.Drawing.Color.White;
            this.ExchangeTypeTextbox.Location = new System.Drawing.Point(131, 71);
            this.ExchangeTypeTextbox.Name = "ExchangeTypeTextbox";
            this.ExchangeTypeTextbox.ReadOnly = true;
            this.ExchangeTypeTextbox.Size = new System.Drawing.Size(269, 20);
            this.ExchangeTypeTextbox.TabIndex = 46;
            // 
            // SessionIDTextbox
            // 
            this.SessionIDTextbox.BackColor = System.Drawing.Color.White;
            this.SessionIDTextbox.Location = new System.Drawing.Point(364, 3);
            this.SessionIDTextbox.Name = "SessionIDTextbox";
            this.SessionIDTextbox.ReadOnly = true;
            this.SessionIDTextbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SessionIDTextbox.Size = new System.Drawing.Size(45, 20);
            this.SessionIDTextbox.TabIndex = 47;
            // 
            // SessionIDLabel
            // 
            this.SessionIDLabel.AutoSize = true;
            this.SessionIDLabel.Location = new System.Drawing.Point(340, 6);
            this.SessionIDLabel.Name = "SessionIDLabel";
            this.SessionIDLabel.Size = new System.Drawing.Size(18, 13);
            this.SessionIDLabel.TabIndex = 48;
            this.SessionIDLabel.Text = "ID";
            // 
            // OpenSessionData
            // 
            this.OpenSessionData.Location = new System.Drawing.Point(144, 305);
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
            this.DeveloperSessionGroupBox.Location = new System.Drawing.Point(421, 3);
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
            // ClientDurationLabel
            // 
            this.ClientDurationLabel.AutoSize = true;
            this.ClientDurationLabel.Location = new System.Drawing.Point(6, 16);
            this.ClientDurationLabel.Name = "ClientDurationLabel";
            this.ClientDurationLabel.Size = new System.Drawing.Size(45, 26);
            this.ClientDurationLabel.TabIndex = 57;
            this.ClientDurationLabel.Text = "Overall\r\nElapsed";
            // 
            // OverallElapsedTextbox
            // 
            this.OverallElapsedTextbox.BackColor = System.Drawing.Color.White;
            this.OverallElapsedTextbox.Location = new System.Drawing.Point(57, 18);
            this.OverallElapsedTextbox.Name = "OverallElapsedTextbox";
            this.OverallElapsedTextbox.ReadOnly = true;
            this.OverallElapsedTextbox.Size = new System.Drawing.Size(53, 20);
            this.OverallElapsedTextbox.TabIndex = 58;
            // 
            // ServerGotRequestLabel
            // 
            this.ServerGotRequestLabel.AutoSize = true;
            this.ServerGotRequestLabel.Location = new System.Drawing.Point(6, 22);
            this.ServerGotRequestLabel.Name = "ServerGotRequestLabel";
            this.ServerGotRequestLabel.Size = new System.Drawing.Size(101, 13);
            this.ServerGotRequestLabel.TabIndex = 59;
            this.ServerGotRequestLabel.Text = "Server Got Request";
            this.ServerGotRequestLabel.Click += new System.EventHandler(this.ServerGotRequestLabel_Click);
            // 
            // ServerGotRequestDateTextbox
            // 
            this.ServerGotRequestDateTextbox.BackColor = System.Drawing.Color.White;
            this.ServerGotRequestDateTextbox.Location = new System.Drawing.Point(131, 19);
            this.ServerGotRequestDateTextbox.Name = "ServerGotRequestDateTextbox";
            this.ServerGotRequestDateTextbox.ReadOnly = true;
            this.ServerGotRequestDateTextbox.Size = new System.Drawing.Size(68, 20);
            this.ServerGotRequestDateTextbox.TabIndex = 60;
            // 
            // ServerDoneResponseLabel
            // 
            this.ServerDoneResponseLabel.AutoSize = true;
            this.ServerDoneResponseLabel.Location = new System.Drawing.Point(6, 22);
            this.ServerDoneResponseLabel.Name = "ServerDoneResponseLabel";
            this.ServerDoneResponseLabel.Size = new System.Drawing.Size(118, 13);
            this.ServerDoneResponseLabel.TabIndex = 64;
            this.ServerDoneResponseLabel.Text = "Server Done Response";
            // 
            // ServerDoneResponseDateTextbox
            // 
            this.ServerDoneResponseDateTextbox.BackColor = System.Drawing.Color.White;
            this.ServerDoneResponseDateTextbox.Location = new System.Drawing.Point(131, 19);
            this.ServerDoneResponseDateTextbox.Name = "ServerDoneResponseDateTextbox";
            this.ServerDoneResponseDateTextbox.ReadOnly = true;
            this.ServerDoneResponseDateTextbox.Size = new System.Drawing.Size(68, 20);
            this.ServerDoneResponseDateTextbox.TabIndex = 65;
            // 
            // ServerDoneResponseTimeTextbox
            // 
            this.ServerDoneResponseTimeTextbox.BackColor = System.Drawing.Color.White;
            this.ServerDoneResponseTimeTextbox.Location = new System.Drawing.Point(205, 19);
            this.ServerDoneResponseTimeTextbox.Name = "ServerDoneResponseTimeTextbox";
            this.ServerDoneResponseTimeTextbox.ReadOnly = true;
            this.ServerDoneResponseTimeTextbox.Size = new System.Drawing.Size(73, 20);
            this.ServerDoneResponseTimeTextbox.TabIndex = 66;
            // 
            // ServerThinkTimeLabel
            // 
            this.ServerThinkTimeLabel.AutoSize = true;
            this.ServerThinkTimeLabel.Location = new System.Drawing.Point(6, 10);
            this.ServerThinkTimeLabel.Name = "ServerThinkTimeLabel";
            this.ServerThinkTimeLabel.Size = new System.Drawing.Size(38, 39);
            this.ServerThinkTimeLabel.TabIndex = 67;
            this.ServerThinkTimeLabel.Text = "Server\r\nThink\r\nTime";
            this.ServerThinkTimeLabel.Click += new System.EventHandler(this.ServerResponseDurationLabel_Click);
            // 
            // ServerThinkTimeTextbox
            // 
            this.ServerThinkTimeTextbox.BackColor = System.Drawing.Color.White;
            this.ServerThinkTimeTextbox.Location = new System.Drawing.Point(56, 19);
            this.ServerThinkTimeTextbox.Name = "ServerThinkTimeTextbox";
            this.ServerThinkTimeTextbox.ReadOnly = true;
            this.ServerThinkTimeTextbox.Size = new System.Drawing.Size(54, 20);
            this.ServerThinkTimeTextbox.TabIndex = 68;
            this.ServerThinkTimeTextbox.TextChanged += new System.EventHandler(this.ServerResponseDurationTextbox_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.ClientRquestBeginTimeLabel);
            this.groupBox1.Controls.Add(this.ClientRequestBeginTimeTextBox);
            this.groupBox1.Controls.Add(this.ClientRequestEndTimelabel);
            this.groupBox1.Controls.Add(this.ClientRequestEndTimeTextBox);
            this.groupBox1.Controls.Add(this.ClientRequestBeginDateTextBox);
            this.groupBox1.Controls.Add(this.ClientRequestEndDateTextBox);
            this.groupBox1.Location = new System.Drawing.Point(9, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 77);
            this.groupBox1.TabIndex = 69;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Overall Elapsed Time";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ClientDurationLabel);
            this.groupBox4.Controls.Add(this.OverallElapsedTextbox);
            this.groupBox4.Location = new System.Drawing.Point(284, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(116, 52);
            this.groupBox4.TabIndex = 74;
            this.groupBox4.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.ServerBeginResponseLabel);
            this.groupBox2.Controls.Add(this.ServerBeginResponseTimeTextbox);
            this.groupBox2.Controls.Add(this.ServerGotRequestTimeTextbox);
            this.groupBox2.Controls.Add(this.ServerBeginResponseDateTextbox);
            this.groupBox2.Controls.Add(this.ServerGotRequestDateTextbox);
            this.groupBox2.Controls.Add(this.ServerGotRequestLabel);
            this.groupBox2.Location = new System.Drawing.Point(9, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 75);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server Processing Time";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ServerThinkTimeTextbox);
            this.groupBox5.Controls.Add(this.ServerThinkTimeLabel);
            this.groupBox5.Location = new System.Drawing.Point(284, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(116, 52);
            this.groupBox5.TabIndex = 74;
            this.groupBox5.TabStop = false;
            // 
            // ServerBeginResponseLabel
            // 
            this.ServerBeginResponseLabel.AutoSize = true;
            this.ServerBeginResponseLabel.Location = new System.Drawing.Point(6, 48);
            this.ServerBeginResponseLabel.Name = "ServerBeginResponseLabel";
            this.ServerBeginResponseLabel.Size = new System.Drawing.Size(119, 13);
            this.ServerBeginResponseLabel.TabIndex = 75;
            this.ServerBeginResponseLabel.Text = "Server Begin Response";
            // 
            // ServerBeginResponseTimeTextbox
            // 
            this.ServerBeginResponseTimeTextbox.BackColor = System.Drawing.Color.White;
            this.ServerBeginResponseTimeTextbox.Location = new System.Drawing.Point(205, 45);
            this.ServerBeginResponseTimeTextbox.Name = "ServerBeginResponseTimeTextbox";
            this.ServerBeginResponseTimeTextbox.ReadOnly = true;
            this.ServerBeginResponseTimeTextbox.Size = new System.Drawing.Size(73, 20);
            this.ServerBeginResponseTimeTextbox.TabIndex = 77;
            // 
            // ServerGotRequestTimeTextbox
            // 
            this.ServerGotRequestTimeTextbox.BackColor = System.Drawing.Color.White;
            this.ServerGotRequestTimeTextbox.Location = new System.Drawing.Point(205, 19);
            this.ServerGotRequestTimeTextbox.Name = "ServerGotRequestTimeTextbox";
            this.ServerGotRequestTimeTextbox.ReadOnly = true;
            this.ServerGotRequestTimeTextbox.Size = new System.Drawing.Size(73, 20);
            this.ServerGotRequestTimeTextbox.TabIndex = 73;
            this.ServerGotRequestTimeTextbox.TextChanged += new System.EventHandler(this.ServerGotRequestTimeTextbox_TextChanged);
            // 
            // ServerBeginResponseDateTextbox
            // 
            this.ServerBeginResponseDateTextbox.BackColor = System.Drawing.Color.White;
            this.ServerBeginResponseDateTextbox.Location = new System.Drawing.Point(131, 45);
            this.ServerBeginResponseDateTextbox.Name = "ServerBeginResponseDateTextbox";
            this.ServerBeginResponseDateTextbox.ReadOnly = true;
            this.ServerBeginResponseDateTextbox.Size = new System.Drawing.Size(68, 20);
            this.ServerBeginResponseDateTextbox.TabIndex = 76;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 71;
            this.label2.Text = "X-HostIP";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // XHostIPTextbox
            // 
            this.XHostIPTextbox.Location = new System.Drawing.Point(131, 122);
            this.XHostIPTextbox.Name = "XHostIPTextbox";
            this.XHostIPTextbox.Size = new System.Drawing.Size(269, 20);
            this.XHostIPTextbox.TabIndex = 72;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ResponseCommentsRichTextBox);
            this.groupBox3.Controls.Add(this.DataAgeTextBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.ResponseAlertTextBox);
            this.groupBox3.Controls.Add(this.OpenSessionData);
            this.groupBox3.Controls.Add(this.ResponseProcessTextBox);
            this.groupBox3.Controls.Add(this.XHostIPTextbox);
            this.groupBox3.Controls.Add(this.ResponseProcessLabel);
            this.groupBox3.Controls.Add(this.SaveSessionDataButton);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.DataAgeLabel);
            this.groupBox3.Controls.Add(this.ResponseServerLabel);
            this.groupBox3.Controls.Add(this.ResponseServerTextBox);
            this.groupBox3.Controls.Add(this.ExchangeTypeLabel);
            this.groupBox3.Controls.Add(this.ExchangeTypeTextbox);
            this.groupBox3.Location = new System.Drawing.Point(9, 247);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(406, 337);
            this.groupBox3.TabIndex = 73;
            this.groupBox3.TabStop = false;
            // 
            // TransmitLabel
            // 
            this.TransmitLabel.AutoSize = true;
            this.TransmitLabel.Location = new System.Drawing.Point(290, 15);
            this.TransmitLabel.Name = "TransmitLabel";
            this.TransmitLabel.Size = new System.Drawing.Size(47, 26);
            this.TransmitLabel.TabIndex = 74;
            this.TransmitLabel.Text = "Transmit\r\nTime";
            this.TransmitLabel.Click += new System.EventHandler(this.TransmitLabel_Click);
            // 
            // TransmitTimeTextbox
            // 
            this.TransmitTimeTextbox.Location = new System.Drawing.Point(340, 19);
            this.TransmitTimeTextbox.Name = "TransmitTimeTextbox";
            this.TransmitTimeTextbox.Size = new System.Drawing.Size(54, 20);
            this.TransmitTimeTextbox.TabIndex = 74;
            // 
            // TransmitGroupBox
            // 
            this.TransmitGroupBox.Controls.Add(this.TransmitTimeTextbox);
            this.TransmitGroupBox.Controls.Add(this.ServerDoneResponseDateTextbox);
            this.TransmitGroupBox.Controls.Add(this.ServerDoneResponseTimeTextbox);
            this.TransmitGroupBox.Controls.Add(this.TransmitLabel);
            this.TransmitGroupBox.Controls.Add(this.ServerDoneResponseLabel);
            this.TransmitGroupBox.Location = new System.Drawing.Point(9, 193);
            this.TransmitGroupBox.Name = "TransmitGroupBox";
            this.TransmitGroupBox.Size = new System.Drawing.Size(406, 48);
            this.TransmitGroupBox.TabIndex = 74;
            this.TransmitGroupBox.TabStop = false;
            this.TransmitGroupBox.Text = "Server Transmit Back";
            // 
            // ResponseUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.TransmitGroupBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.DeveloperSessionGroupBox);
            this.Controls.Add(this.SessionIDLabel);
            this.Controls.Add(this.SessionIDTextbox);
            this.Controls.Add(this.HTTPStatusDescriptionTextBox);
            this.Controls.Add(this.HTTPResponseCodeTextBox);
            this.Controls.Add(this.HTTPStatusCodeLinkLabel);
            this.Name = "ResponseUserControl";
            this.Size = new System.Drawing.Size(828, 597);
            this.Load += new System.EventHandler(this.ResponseUserControl_Load);
            this.DeveloperSessionGroupBox.ResumeLayout(false);
            this.DeveloperSessionGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.TransmitGroupBox.ResumeLayout(false);
            this.TransmitGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel HTTPStatusCodeLinkLabel;
        private System.Windows.Forms.TextBox HTTPResponseCodeTextBox;
        private System.Windows.Forms.TextBox HTTPStatusDescriptionTextBox;
        private System.Windows.Forms.Label ClientRquestBeginTimeLabel;
        private System.Windows.Forms.TextBox ClientRequestBeginTimeTextBox;
        private System.Windows.Forms.Label ClientRequestEndTimelabel;
        private System.Windows.Forms.TextBox ClientRequestEndTimeTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ResponseAlertTextBox;
        private System.Windows.Forms.TextBox ResponseProcessTextBox;
        private System.Windows.Forms.Label ResponseProcessLabel;
        private System.Windows.Forms.Label DataAgeLabel;
        private System.Windows.Forms.TextBox DataAgeTextBox;
        private System.Windows.Forms.TextBox ClientRequestBeginDateTextBox;
        private System.Windows.Forms.TextBox ClientRequestEndDateTextBox;
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
        private System.Windows.Forms.Button RemoveAllAppPrefsButton;
        private System.Windows.Forms.Label ClientDurationLabel;
        private System.Windows.Forms.TextBox OverallElapsedTextbox;
        private System.Windows.Forms.Label ServerGotRequestLabel;
        private System.Windows.Forms.TextBox ServerGotRequestDateTextbox;
        private System.Windows.Forms.Label ServerDoneResponseLabel;
        private System.Windows.Forms.TextBox ServerDoneResponseDateTextbox;
        private System.Windows.Forms.TextBox ServerDoneResponseTimeTextbox;
        private System.Windows.Forms.Label ServerThinkTimeLabel;
        private System.Windows.Forms.TextBox ServerThinkTimeTextbox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox XHostIPTextbox;
        private System.Windows.Forms.TextBox ServerGotRequestTimeTextbox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label ServerBeginResponseLabel;
        private System.Windows.Forms.TextBox ServerBeginResponseTimeTextbox;
        private System.Windows.Forms.TextBox ServerBeginResponseDateTextbox;
        private System.Windows.Forms.TextBox TransmitTimeTextbox;
        private System.Windows.Forms.Label TransmitLabel;
        private System.Windows.Forms.GroupBox TransmitGroupBox;
    }
}
