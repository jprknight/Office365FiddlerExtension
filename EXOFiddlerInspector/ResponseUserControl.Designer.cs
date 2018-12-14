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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResponseUserControl));
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
            this.OverallElapsedTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.ServerThinkTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.ServerBeginResponseLabel = new System.Windows.Forms.Label();
            this.ServerBeginResponseTimeTextbox = new System.Windows.Forms.TextBox();
            this.ServerGotRequestTimeTextbox = new System.Windows.Forms.TextBox();
            this.ServerBeginResponseDateTextbox = new System.Windows.Forms.TextBox();
            this.HostIPLabel = new System.Windows.Forms.Label();
            this.XHostIPTextbox = new System.Windows.Forms.TextBox();
            this.OtherSessionDataGroupBox = new System.Windows.Forms.GroupBox();
            this.TransmitLabel = new System.Windows.Forms.Label();
            this.TransmitTimeTextbox = new System.Windows.Forms.TextBox();
            this.TransmitTimeGroupBox = new System.Windows.Forms.GroupBox();
            this.HTTPResponseCodeIDGroupBox = new System.Windows.Forms.GroupBox();
            this.LiveTraceHelperGroupBox = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LiveTraceLabel1 = new System.Windows.Forms.Label();
            this.SessionDataGroupBox = new System.Windows.Forms.GroupBox();
            this.DeveloperSessionGroupBox.SuspendLayout();
            this.OverallElapsedTimeGroupBox.SuspendLayout();
            this.ServerThinkTimeGroupBox.SuspendLayout();
            this.OtherSessionDataGroupBox.SuspendLayout();
            this.TransmitTimeGroupBox.SuspendLayout();
            this.HTTPResponseCodeIDGroupBox.SuspendLayout();
            this.LiveTraceHelperGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SessionDataGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // HTTPStatusCodeLinkLabel
            // 
            this.HTTPStatusCodeLinkLabel.AutoSize = true;
            this.HTTPStatusCodeLinkLabel.Location = new System.Drawing.Point(6, 16);
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
            this.HTTPResponseCodeTextBox.Location = new System.Drawing.Point(131, 13);
            this.HTTPResponseCodeTextBox.Name = "HTTPResponseCodeTextBox";
            this.HTTPResponseCodeTextBox.ReadOnly = true;
            this.HTTPResponseCodeTextBox.Size = new System.Drawing.Size(24, 20);
            this.HTTPResponseCodeTextBox.TabIndex = 10;
            this.HTTPResponseCodeTextBox.TextChanged += new System.EventHandler(this.HTTPResponseCodeTextBox_TextChanged);
            // 
            // HTTPStatusDescriptionTextBox
            // 
            this.HTTPStatusDescriptionTextBox.BackColor = System.Drawing.Color.White;
            this.HTTPStatusDescriptionTextBox.Location = new System.Drawing.Point(161, 13);
            this.HTTPStatusDescriptionTextBox.Name = "HTTPStatusDescriptionTextBox";
            this.HTTPStatusDescriptionTextBox.ReadOnly = true;
            this.HTTPStatusDescriptionTextBox.Size = new System.Drawing.Size(161, 20);
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
            this.ClientRequestBeginTimeTextBox.Location = new System.Drawing.Point(212, 19);
            this.ClientRequestBeginTimeTextBox.Name = "ClientRequestBeginTimeTextBox";
            this.ClientRequestBeginTimeTextBox.ReadOnly = true;
            this.ClientRequestBeginTimeTextBox.Size = new System.Drawing.Size(69, 20);
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
            this.ClientRequestEndTimeTextBox.Location = new System.Drawing.Point(212, 45);
            this.ClientRequestEndTimeTextBox.Name = "ClientRequestEndTimeTextBox";
            this.ClientRequestEndTimeTextBox.ReadOnly = true;
            this.ClientRequestEndTimeTextBox.Size = new System.Drawing.Size(69, 20);
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
            this.ResponseAlertTextBox.Size = new System.Drawing.Size(275, 20);
            this.ResponseAlertTextBox.TabIndex = 21;
            // 
            // ResponseProcessTextBox
            // 
            this.ResponseProcessTextBox.Location = new System.Drawing.Point(131, 45);
            this.ResponseProcessTextBox.Name = "ResponseProcessTextBox";
            this.ResponseProcessTextBox.Size = new System.Drawing.Size(275, 20);
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
            this.ResponseCommentsRichTextBox.Size = new System.Drawing.Size(400, 102);
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
            this.DataAgeTextBox.Size = new System.Drawing.Size(275, 20);
            this.DataAgeTextBox.TabIndex = 27;
            // 
            // ClientRequestBeginDateTextBox
            // 
            this.ClientRequestBeginDateTextBox.BackColor = System.Drawing.Color.White;
            this.ClientRequestBeginDateTextBox.Location = new System.Drawing.Point(131, 19);
            this.ClientRequestBeginDateTextBox.Name = "ClientRequestBeginDateTextBox";
            this.ClientRequestBeginDateTextBox.ReadOnly = true;
            this.ClientRequestBeginDateTextBox.Size = new System.Drawing.Size(75, 20);
            this.ClientRequestBeginDateTextBox.TabIndex = 31;
            // 
            // ClientRequestEndDateTextBox
            // 
            this.ClientRequestEndDateTextBox.BackColor = System.Drawing.Color.White;
            this.ClientRequestEndDateTextBox.Location = new System.Drawing.Point(131, 45);
            this.ClientRequestEndDateTextBox.Name = "ClientRequestEndDateTextBox";
            this.ClientRequestEndDateTextBox.ReadOnly = true;
            this.ClientRequestEndDateTextBox.Size = new System.Drawing.Size(75, 20);
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
            this.ResponseServerTextBox.Size = new System.Drawing.Size(275, 20);
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
            this.SaveSessionDataButton.Location = new System.Drawing.Point(286, 282);
            this.SaveSessionDataButton.Name = "SaveSessionDataButton";
            this.SaveSessionDataButton.Size = new System.Drawing.Size(120, 23);
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
            this.ExchangeTypeTextbox.Size = new System.Drawing.Size(275, 20);
            this.ExchangeTypeTextbox.TabIndex = 46;
            // 
            // SessionIDTextbox
            // 
            this.SessionIDTextbox.BackColor = System.Drawing.Color.White;
            this.SessionIDTextbox.Location = new System.Drawing.Point(352, 13);
            this.SessionIDTextbox.Name = "SessionIDTextbox";
            this.SessionIDTextbox.ReadOnly = true;
            this.SessionIDTextbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SessionIDTextbox.Size = new System.Drawing.Size(54, 20);
            this.SessionIDTextbox.TabIndex = 47;
            // 
            // SessionIDLabel
            // 
            this.SessionIDLabel.AutoSize = true;
            this.SessionIDLabel.Location = new System.Drawing.Point(328, 16);
            this.SessionIDLabel.Name = "SessionIDLabel";
            this.SessionIDLabel.Size = new System.Drawing.Size(18, 13);
            this.SessionIDLabel.TabIndex = 48;
            this.SessionIDLabel.Text = "ID";
            // 
            // OpenSessionData
            // 
            this.OpenSessionData.Location = new System.Drawing.Point(161, 282);
            this.OpenSessionData.Name = "OpenSessionData";
            this.OpenSessionData.Size = new System.Drawing.Size(120, 23);
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
            this.DeveloperSessionGroupBox.Location = new System.Drawing.Point(438, 8);
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
            this.ClientDurationLabel.Location = new System.Drawing.Point(287, 31);
            this.ClientDurationLabel.Name = "ClientDurationLabel";
            this.ClientDurationLabel.Size = new System.Drawing.Size(45, 26);
            this.ClientDurationLabel.TabIndex = 57;
            this.ClientDurationLabel.Text = "Overall\r\nElapsed";
            // 
            // OverallElapsedTextbox
            // 
            this.OverallElapsedTextbox.BackColor = System.Drawing.Color.White;
            this.OverallElapsedTextbox.Location = new System.Drawing.Point(338, 31);
            this.OverallElapsedTextbox.Name = "OverallElapsedTextbox";
            this.OverallElapsedTextbox.ReadOnly = true;
            this.OverallElapsedTextbox.Size = new System.Drawing.Size(68, 20);
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
            this.ServerThinkTimeLabel.Location = new System.Drawing.Point(290, 22);
            this.ServerThinkTimeLabel.Name = "ServerThinkTimeLabel";
            this.ServerThinkTimeLabel.Size = new System.Drawing.Size(38, 39);
            this.ServerThinkTimeLabel.TabIndex = 67;
            this.ServerThinkTimeLabel.Text = "Server\r\nThink\r\nTime";
            // 
            // ServerThinkTimeTextbox
            // 
            this.ServerThinkTimeTextbox.BackColor = System.Drawing.Color.White;
            this.ServerThinkTimeTextbox.Location = new System.Drawing.Point(340, 31);
            this.ServerThinkTimeTextbox.Name = "ServerThinkTimeTextbox";
            this.ServerThinkTimeTextbox.ReadOnly = true;
            this.ServerThinkTimeTextbox.Size = new System.Drawing.Size(66, 20);
            this.ServerThinkTimeTextbox.TabIndex = 68;
            // 
            // OverallElapsedTimeGroupBox
            // 
            this.OverallElapsedTimeGroupBox.Controls.Add(this.ClientDurationLabel);
            this.OverallElapsedTimeGroupBox.Controls.Add(this.OverallElapsedTextbox);
            this.OverallElapsedTimeGroupBox.Controls.Add(this.ClientRequestBeginTimeTextBox);
            this.OverallElapsedTimeGroupBox.Controls.Add(this.ClientRquestBeginTimeLabel);
            this.OverallElapsedTimeGroupBox.Controls.Add(this.ClientRequestEndTimelabel);
            this.OverallElapsedTimeGroupBox.Controls.Add(this.ClientRequestEndTimeTextBox);
            this.OverallElapsedTimeGroupBox.Controls.Add(this.ClientRequestBeginDateTextBox);
            this.OverallElapsedTimeGroupBox.Controls.Add(this.ClientRequestEndDateTextBox);
            this.OverallElapsedTimeGroupBox.Location = new System.Drawing.Point(6, 63);
            this.OverallElapsedTimeGroupBox.Name = "OverallElapsedTimeGroupBox";
            this.OverallElapsedTimeGroupBox.Size = new System.Drawing.Size(412, 77);
            this.OverallElapsedTimeGroupBox.TabIndex = 69;
            this.OverallElapsedTimeGroupBox.TabStop = false;
            this.OverallElapsedTimeGroupBox.Text = "Overall Elapsed Time";
            // 
            // ServerThinkTimeGroupBox
            // 
            this.ServerThinkTimeGroupBox.Controls.Add(this.ServerThinkTimeTextbox);
            this.ServerThinkTimeGroupBox.Controls.Add(this.ServerBeginResponseLabel);
            this.ServerThinkTimeGroupBox.Controls.Add(this.ServerBeginResponseTimeTextbox);
            this.ServerThinkTimeGroupBox.Controls.Add(this.ServerThinkTimeLabel);
            this.ServerThinkTimeGroupBox.Controls.Add(this.ServerGotRequestTimeTextbox);
            this.ServerThinkTimeGroupBox.Controls.Add(this.ServerBeginResponseDateTextbox);
            this.ServerThinkTimeGroupBox.Controls.Add(this.ServerGotRequestDateTextbox);
            this.ServerThinkTimeGroupBox.Controls.Add(this.ServerGotRequestLabel);
            this.ServerThinkTimeGroupBox.Location = new System.Drawing.Point(6, 146);
            this.ServerThinkTimeGroupBox.Name = "ServerThinkTimeGroupBox";
            this.ServerThinkTimeGroupBox.Size = new System.Drawing.Size(412, 75);
            this.ServerThinkTimeGroupBox.TabIndex = 70;
            this.ServerThinkTimeGroupBox.TabStop = false;
            this.ServerThinkTimeGroupBox.Text = "Server Think Time";
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
            // HostIPLabel
            // 
            this.HostIPLabel.AutoSize = true;
            this.HostIPLabel.Location = new System.Drawing.Point(6, 125);
            this.HostIPLabel.Name = "HostIPLabel";
            this.HostIPLabel.Size = new System.Drawing.Size(42, 13);
            this.HostIPLabel.TabIndex = 71;
            this.HostIPLabel.Text = "Host IP";
            // 
            // XHostIPTextbox
            // 
            this.XHostIPTextbox.Location = new System.Drawing.Point(131, 122);
            this.XHostIPTextbox.Name = "XHostIPTextbox";
            this.XHostIPTextbox.Size = new System.Drawing.Size(275, 20);
            this.XHostIPTextbox.TabIndex = 72;
            // 
            // OtherSessionDataGroupBox
            // 
            this.OtherSessionDataGroupBox.Controls.Add(this.ResponseCommentsRichTextBox);
            this.OtherSessionDataGroupBox.Controls.Add(this.DataAgeTextBox);
            this.OtherSessionDataGroupBox.Controls.Add(this.label1);
            this.OtherSessionDataGroupBox.Controls.Add(this.ResponseAlertTextBox);
            this.OtherSessionDataGroupBox.Controls.Add(this.OpenSessionData);
            this.OtherSessionDataGroupBox.Controls.Add(this.ResponseProcessTextBox);
            this.OtherSessionDataGroupBox.Controls.Add(this.XHostIPTextbox);
            this.OtherSessionDataGroupBox.Controls.Add(this.ResponseProcessLabel);
            this.OtherSessionDataGroupBox.Controls.Add(this.SaveSessionDataButton);
            this.OtherSessionDataGroupBox.Controls.Add(this.HostIPLabel);
            this.OtherSessionDataGroupBox.Controls.Add(this.DataAgeLabel);
            this.OtherSessionDataGroupBox.Controls.Add(this.ResponseServerLabel);
            this.OtherSessionDataGroupBox.Controls.Add(this.ResponseServerTextBox);
            this.OtherSessionDataGroupBox.Controls.Add(this.ExchangeTypeLabel);
            this.OtherSessionDataGroupBox.Controls.Add(this.ExchangeTypeTextbox);
            this.OtherSessionDataGroupBox.Location = new System.Drawing.Point(6, 281);
            this.OtherSessionDataGroupBox.Name = "OtherSessionDataGroupBox";
            this.OtherSessionDataGroupBox.Size = new System.Drawing.Size(412, 314);
            this.OtherSessionDataGroupBox.TabIndex = 73;
            this.OtherSessionDataGroupBox.TabStop = false;
            // 
            // TransmitLabel
            // 
            this.TransmitLabel.AutoSize = true;
            this.TransmitLabel.Location = new System.Drawing.Point(290, 15);
            this.TransmitLabel.Name = "TransmitLabel";
            this.TransmitLabel.Size = new System.Drawing.Size(47, 26);
            this.TransmitLabel.TabIndex = 74;
            this.TransmitLabel.Text = "Transmit\r\nTime";
            // 
            // TransmitTimeTextbox
            // 
            this.TransmitTimeTextbox.Location = new System.Drawing.Point(340, 19);
            this.TransmitTimeTextbox.Name = "TransmitTimeTextbox";
            this.TransmitTimeTextbox.Size = new System.Drawing.Size(66, 20);
            this.TransmitTimeTextbox.TabIndex = 74;
            // 
            // TransmitTimeGroupBox
            // 
            this.TransmitTimeGroupBox.Controls.Add(this.TransmitTimeTextbox);
            this.TransmitTimeGroupBox.Controls.Add(this.ServerDoneResponseDateTextbox);
            this.TransmitTimeGroupBox.Controls.Add(this.ServerDoneResponseTimeTextbox);
            this.TransmitTimeGroupBox.Controls.Add(this.TransmitLabel);
            this.TransmitTimeGroupBox.Controls.Add(this.ServerDoneResponseLabel);
            this.TransmitTimeGroupBox.Location = new System.Drawing.Point(6, 227);
            this.TransmitTimeGroupBox.Name = "TransmitTimeGroupBox";
            this.TransmitTimeGroupBox.Size = new System.Drawing.Size(412, 48);
            this.TransmitTimeGroupBox.TabIndex = 74;
            this.TransmitTimeGroupBox.TabStop = false;
            this.TransmitTimeGroupBox.Text = "Transmit Time Back to Client App";
            // 
            // HTTPResponseCodeIDGroupBox
            // 
            this.HTTPResponseCodeIDGroupBox.Controls.Add(this.HTTPStatusCodeLinkLabel);
            this.HTTPResponseCodeIDGroupBox.Controls.Add(this.HTTPResponseCodeTextBox);
            this.HTTPResponseCodeIDGroupBox.Controls.Add(this.HTTPStatusDescriptionTextBox);
            this.HTTPResponseCodeIDGroupBox.Controls.Add(this.SessionIDTextbox);
            this.HTTPResponseCodeIDGroupBox.Controls.Add(this.SessionIDLabel);
            this.HTTPResponseCodeIDGroupBox.Location = new System.Drawing.Point(6, 16);
            this.HTTPResponseCodeIDGroupBox.Name = "HTTPResponseCodeIDGroupBox";
            this.HTTPResponseCodeIDGroupBox.Size = new System.Drawing.Size(412, 41);
            this.HTTPResponseCodeIDGroupBox.TabIndex = 75;
            this.HTTPResponseCodeIDGroupBox.TabStop = false;
            // 
            // LiveTraceHelperGroupBox
            // 
            this.LiveTraceHelperGroupBox.Controls.Add(this.pictureBox2);
            this.LiveTraceHelperGroupBox.Controls.Add(this.pictureBox1);
            this.LiveTraceHelperGroupBox.Controls.Add(this.LiveTraceLabel1);
            this.LiveTraceHelperGroupBox.Location = new System.Drawing.Point(8, 8);
            this.LiveTraceHelperGroupBox.Name = "LiveTraceHelperGroupBox";
            this.LiveTraceHelperGroupBox.Size = new System.Drawing.Size(424, 601);
            this.LiveTraceHelperGroupBox.TabIndex = 76;
            this.LiveTraceHelperGroupBox.TabStop = false;
            this.LiveTraceHelperGroupBox.Text = "Live Trace";
            this.LiveTraceHelperGroupBox.VisibleChanged += new System.EventHandler(this.LiveTraceHelperGroupBox_VisibleChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(9, 388);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(207, 207);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(9, 140);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(382, 242);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // LiveTraceLabel1
            // 
            this.LiveTraceLabel1.AutoSize = true;
            this.LiveTraceLabel1.Location = new System.Drawing.Point(6, 16);
            this.LiveTraceLabel1.Name = "LiveTraceLabel1";
            this.LiveTraceLabel1.Size = new System.Drawing.Size(288, 117);
            this.LiveTraceLabel1.TabIndex = 0;
            this.LiveTraceLabel1.Text = resources.GetString("LiveTraceLabel1.Text");
            // 
            // SessionDataGroupBox
            // 
            this.SessionDataGroupBox.Controls.Add(this.HTTPResponseCodeIDGroupBox);
            this.SessionDataGroupBox.Controls.Add(this.OverallElapsedTimeGroupBox);
            this.SessionDataGroupBox.Controls.Add(this.OtherSessionDataGroupBox);
            this.SessionDataGroupBox.Controls.Add(this.TransmitTimeGroupBox);
            this.SessionDataGroupBox.Controls.Add(this.ServerThinkTimeGroupBox);
            this.SessionDataGroupBox.Location = new System.Drawing.Point(841, 8);
            this.SessionDataGroupBox.Name = "SessionDataGroupBox";
            this.SessionDataGroupBox.Size = new System.Drawing.Size(424, 601);
            this.SessionDataGroupBox.TabIndex = 77;
            this.SessionDataGroupBox.TabStop = false;
            this.SessionDataGroupBox.Text = "Load Saz Archive";
            this.SessionDataGroupBox.Visible = false;
            this.SessionDataGroupBox.VisibleChanged += new System.EventHandler(this.SessionDataGroupBox_VisibleChanged);
            // 
            // ResponseUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.SessionDataGroupBox);
            this.Controls.Add(this.LiveTraceHelperGroupBox);
            this.Controls.Add(this.DeveloperSessionGroupBox);
            this.Name = "ResponseUserControl";
            this.Size = new System.Drawing.Size(1273, 616);
            this.Load += new System.EventHandler(this.ResponseUserControl_Load);
            this.DeveloperSessionGroupBox.ResumeLayout(false);
            this.DeveloperSessionGroupBox.PerformLayout();
            this.OverallElapsedTimeGroupBox.ResumeLayout(false);
            this.OverallElapsedTimeGroupBox.PerformLayout();
            this.ServerThinkTimeGroupBox.ResumeLayout(false);
            this.ServerThinkTimeGroupBox.PerformLayout();
            this.OtherSessionDataGroupBox.ResumeLayout(false);
            this.OtherSessionDataGroupBox.PerformLayout();
            this.TransmitTimeGroupBox.ResumeLayout(false);
            this.TransmitTimeGroupBox.PerformLayout();
            this.HTTPResponseCodeIDGroupBox.ResumeLayout(false);
            this.HTTPResponseCodeIDGroupBox.PerformLayout();
            this.LiveTraceHelperGroupBox.ResumeLayout(false);
            this.LiveTraceHelperGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.SessionDataGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox OverallElapsedTimeGroupBox;
        private System.Windows.Forms.GroupBox ServerThinkTimeGroupBox;
        private System.Windows.Forms.Label HostIPLabel;
        private System.Windows.Forms.TextBox XHostIPTextbox;
        private System.Windows.Forms.TextBox ServerGotRequestTimeTextbox;
        private System.Windows.Forms.GroupBox OtherSessionDataGroupBox;
        private System.Windows.Forms.Label ServerBeginResponseLabel;
        private System.Windows.Forms.TextBox ServerBeginResponseTimeTextbox;
        private System.Windows.Forms.TextBox ServerBeginResponseDateTextbox;
        private System.Windows.Forms.TextBox TransmitTimeTextbox;
        private System.Windows.Forms.Label TransmitLabel;
        private System.Windows.Forms.GroupBox TransmitTimeGroupBox;
        private System.Windows.Forms.GroupBox HTTPResponseCodeIDGroupBox;
        private System.Windows.Forms.GroupBox LiveTraceHelperGroupBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label LiveTraceLabel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox SessionDataGroupBox;
    }
}
