using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Fiddler;

namespace EXOFiddlerInspector
{
    public partial class ResponseUserControl : UserControl
    {
        public string SessionData;

        //private DebugConsole DevConsole;

        public ResponseUserControl()
        {
            InitializeComponent();
            // Telemtry call goes here. Called once per session.

        }

        private void ResponseUserControl_Load(object sender, EventArgs e)
        {
            /////////////////////////////
            //
            // Before we go ahead and run the add tab code work out if 
            // the user is a developer or not.

            // Developer list is actually set in Preferences.cs.
            Preferences calledPreferences = new Preferences();
            List<string> calledDeveloperList = calledPreferences.GetDeveloperList();

            // Based on the above set the Boolean Developer for use through the rest of the code.
            if (calledDeveloperList.Any(Environment.UserName.Contains))
            {
                Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: Developer mode {Environment.UserName} on {Environment.MachineName}.");
                DeveloperSessionGroupBox.Visible = true;
            }
            else
            {
                // Don't do anything right now. Leave the above as invisible to other users.
            }


        }

        // Code to write to ResponseProcessTextBox.Text value.
        internal void SetResponseProcessTextBox(string txt)
        {
            ResponseProcessTextBox.Text = txt;
        }

        // Code to write to HTTPResponseCodeTextBox.Text value.
        internal void SetHTTPResponseCodeTextBoxText(string txt)
        {
            HTTPResponseCodeTextBox.Text = txt;
        }

        // Code to write to HTTPStatusDescriptionTextBox.Text value.
        internal void SetHTTPStatusDescriptionTextBox(string txt)
        {
            HTTPStatusDescriptionTextBox.Text = txt;
        }

        // Code to write to ClientRequestBeginTimeTextBox.Text value.
        internal void SetClientRequestBeginTimeTextBox(string txt)
        {
            ClientRequestBeginTimeTextBox.Text = txt;
        }

        // Code to write to ClientRequestBeginDateTextBox.Text value.
        internal void SetClientRequestBeginDateTextBox(string txt)
        {
            ClientRequestBeginDateTextBox.Text = txt;
        }

        // Code to write to ClientRequestEndTimeTextBox.Text value.
        internal void SetClientRequestEndTimeTextBox(string txt)
        {
            ClientRequestEndTimeTextBox.Text = txt;
        }

        // Code to write to ClientRequestEndDateTextBox.Text value.
        internal void SetClientRequestEndDateTextBox(string txt)
        {
            ClientRequestEndDateTextBox.Text = txt;
        }

        // Code to write to ServerGotRequestDateTextbox.Text value.
        internal void SetServerGotRequestDateTextbox(string txt)
        {
            ServerGotRequestDateTextbox.Text = txt;
        }

        // Code to write to ServerGotRequestTimeTextbox.Text value.
        internal void SetServerGotRequestTimeTextbox(string txt)
        {
            ServerGotRequestTimeTextbox.Text = txt;
        }

        // Code to write to ServerBeginResponseDateTextbox.Text value.
        internal void SetServerBeginResponseDateTextbox(string txt)
        {
            ServerBeginResponseDateTextbox.Text = txt;
        }

        // Code to write to ServerBeginResponseTimeTextbox.Text value.
        internal void SetServerBeginResponseTimeTextbox(string txt)
        {
            ServerBeginResponseTimeTextbox.Text = txt;
        }


        // Code to write to ServerDoneResponseDateTextbox.Text value.
        internal void SetServerDoneResponseDateTextbox(string txt)
        {
            ServerDoneResponseDateTextbox.Text = txt;
        }

        // Code to write to ServerDoneResponseTimeTextbox.Text value.
        internal void SetServerDoneResponseTimeTextbox(string txt)
        {
            ServerDoneResponseTimeTextbox.Text = txt;
        }

        // Code to write to ClientBeginRequestDoneResponseDurationTextbox.Text value.
        internal void SetOverallElapsedTextbox(string txt)
        {
            OverallElapsedTextbox.Text = txt;
        }

        // Code to write to ServerBeginRequestDoneResponseDurationTextbox.Text value.
        internal void SetServerThinkTimeTextbox(string txt)
        {
            ServerThinkTimeTextbox.Text = txt;
        }

        // Code to write to ResponseAlertTextBox.Text value.
        internal void SetResponseAlertTextBox(string txt)
        {
            ResponseAlertTextBox.Text = txt;
        }

        // Code to write to ResponseCommentsTextBox.Text value.
        internal void SetResponseCommentsRichTextboxText(string txt)
        {
            ResponseCommentsRichTextBox.Text = txt;
        }

        // Code to write to ResponseAlertTextBox.Text value.
        internal void SetDataAgeTextBox(string txt)
        {
            DataAgeTextBox.Text = txt;
        }

        // Code to write to ResponseServerTextBox.Text value.
        internal void SetResponseServerTextBoxText(string txt)
        {
            ResponseServerTextBox.Text = txt;
        }

        // Code to write to RequestHeadersTextBox.Text value.
        internal void SetRequestHeadersTextBoxText(string txt)
        {
            RequestHeadersTextBox.Text = txt;
        }

        // Code to write to RequestBodyTextBox.Text value.
        internal void SetRequestBodyTextBoxText(string txt)
        {
            RequestBodyTextbox.Text = txt;
        }

        // Code to write to ResponseHeadersTextBox.Text value.
        internal void SetResponseHeadersTextBoxText(string txt)
        {
            ResponseHeadersTextbox.Text = txt;
        }

        // Code to write to RequestBodyTextBox.Text value.
        internal void SetResponseBodyTextBoxText(string txt)
        {
            ResponseBodyTextbox.Text = txt;
        }

        // Code to write to ExchangeTypeTextBox.Text value.
        internal void SetExchangeTypeTextBoxText(string txt)
        {
            ExchangeTypeTextbox.Text = txt;
        }

        // Code to write to SessionIDTextBox.Text value.
        internal void SetSessionIDTextBoxText(string txt)
        {
            SessionIDTextbox.Text = txt;
        }

        // Code to write to XHostIP textbox.Text value.
        internal void SetXHostIPTextBoxText(string txt)
        {
            XHostIPTextbox.Text = txt;
        }

        private void HTTPStatusCodeLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void VisitLink()
        {
            // Change the color of the link text by setting LinkVisited   
            // to true.  
            HTTPStatusCodeLinkLabel.LinkVisited = true;
            //Call the Process.Start method to open the default browser   
            //with a URL:  
            System.Diagnostics.Process.Start(Properties.Settings.Default.HTTPStatusCodesURL);
        }

        private void HTTPResponseCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            // Reset colours.
            HTTPResponseCodeTextBox.BackColor = System.Drawing.Color.White;
            HTTPStatusDescriptionTextBox.BackColor = System.Drawing.Color.White;

            // Write HTTP Status code short descriptions in HTTP Status Description TextBox.
            // Standardised codes from https://en.wikipedia.org/wiki/List_of_HTTP_status_codes.
            switch (HTTPResponseCodeTextBox.Text) {
                // 1xx Informational.
                case "100": HTTPStatusDescriptionTextBox.Text = "Continue";
                    break;
                case "101": HTTPStatusDescriptionTextBox.Text = "Switching Protocols";
                    break;
                case "102": HTTPStatusDescriptionTextBox.Text = "Processing";
                    break;
                case "103": HTTPStatusDescriptionTextBox.Text = "Early Hints";
                    break;
                // 2xx Success.
                case "200": HTTPStatusDescriptionTextBox.Text = "OK";
                    break;
                case "201": HTTPStatusDescriptionTextBox.Text = "Created";
                    break;
                case "202": HTTPStatusDescriptionTextBox.Text = "Accepted";
                    break;
                case "203": HTTPStatusDescriptionTextBox.Text = "Non-Authoritative Information";
                    break;
                case "204": HTTPStatusDescriptionTextBox.Text = "No Content";
                    break;
                case "205": HTTPStatusDescriptionTextBox.Text = "Reset Content";
                    break;
                case "206": HTTPStatusDescriptionTextBox.Text = "Partial Content";
                    break;
                case "207": HTTPStatusDescriptionTextBox.Text = "Multi-Status";
                    break;
                case "208": HTTPStatusDescriptionTextBox.Text = "Already Reported";
                    break;
                case "226": HTTPStatusDescriptionTextBox.Text = "IM Used";
                    break;
                // 3xx Redirections.
                case "300": HTTPStatusDescriptionTextBox.Text = "Multiple Choices";
                    break;
                case "301": HTTPStatusDescriptionTextBox.Text = "Moved Permanently";
                    break;
                case "302": HTTPStatusDescriptionTextBox.Text = "Found";
                    break;
                case "303": HTTPStatusDescriptionTextBox.Text = "See Other";
                    break;
                case "304": HTTPStatusDescriptionTextBox.Text = "Not Modified";
                    break;
                case "305": HTTPStatusDescriptionTextBox.Text = "Use Proxy";
                    break;
                case "306": HTTPStatusDescriptionTextBox.Text = "Switch Proxy";
                    break;
                case "307": HTTPStatusDescriptionTextBox.Text = "Temporary Redirect";
                    break;
                case "308": HTTPStatusDescriptionTextBox.Text = "Permanent Redirect";
                    break;
                // 4xx Client errors.
                case "400": HTTPStatusDescriptionTextBox.Text = "Bad Request";
                    break;
                case "401": HTTPStatusDescriptionTextBox.Text = "Unauthorized";
                    break;
                case "402": HTTPStatusDescriptionTextBox.Text = "Payment Required";
                    break;
                case "403": HTTPStatusDescriptionTextBox.Text = "Forbidden";
                    break;
                case "404": HTTPStatusDescriptionTextBox.Text = "Not Found";
                    break;
                case "405": HTTPStatusDescriptionTextBox.Text = "Method Not Allowed";
                    break;
                case "406": HTTPStatusDescriptionTextBox.Text = "Not Acceptable";
                    break;
                case "407": HTTPStatusDescriptionTextBox.Text = "Proxy Authentication Required";
                    break;
                case "408": HTTPStatusDescriptionTextBox.Text = "Request Timeout";
                    break;
                case "409": HTTPStatusDescriptionTextBox.Text = "Conflict";
                    break;
                case "410": HTTPStatusDescriptionTextBox.Text = "Gone";
                    break;
                case "411": HTTPStatusDescriptionTextBox.Text = "Length Required";
                    break;
                case "412": HTTPStatusDescriptionTextBox.Text = "Precondition Failed";
                    break;
                case "413": HTTPStatusDescriptionTextBox.Text = "Payload Too Large";
                    break;
                case "414": HTTPStatusDescriptionTextBox.Text = "Request - URI Too Long";
                    break;
                case "415": HTTPStatusDescriptionTextBox.Text = "Unsupported Media Type";
                    break;
                case "416": HTTPStatusDescriptionTextBox.Text = "Requested Range Not Satisfiable";
                    break;
                case "417": HTTPStatusDescriptionTextBox.Text = "Expectation Failed";
                    break;
                case "418": HTTPStatusDescriptionTextBox.Text = "I'm a teapot";
                    break;
                case "421": HTTPStatusDescriptionTextBox.Text = "Misdirected Request";
                    break;
                case "422": HTTPStatusDescriptionTextBox.Text = "Unprocessable Entity";
                    break;
                case "423": HTTPStatusDescriptionTextBox.Text = "Locked";
                    break;
                case "424": HTTPStatusDescriptionTextBox.Text = "Failed Dependency";
                    break;
                case "426": HTTPStatusDescriptionTextBox.Text = "Upgrade Required";
                    break;
                case "428": HTTPStatusDescriptionTextBox.Text = "Precondition Required";
                    break;
                case "429": HTTPStatusDescriptionTextBox.Text = "Too Many Requests";
                    break;
                case "431": HTTPStatusDescriptionTextBox.Text = "Request Header Fields Too Large";
                    break;
                case "444": HTTPStatusDescriptionTextBox.Text = "Connection Closed Without Response";
                    break;
                case "451": HTTPStatusDescriptionTextBox.Text = "Unavailable For Legal Reasons";
                    break;
                case "499": HTTPStatusDescriptionTextBox.Text = "Client Closed Request";
                    break;
                //5xx Server Errors.
                case "500": HTTPStatusDescriptionTextBox.Text = "Internal Server Error";
                    break;
                case "501": HTTPStatusDescriptionTextBox.Text = "Not Implemented";
                    break;
                case "502": HTTPStatusDescriptionTextBox.Text = "Bad Gateway";
                    break;
                case "503": HTTPStatusDescriptionTextBox.Text = "Service Unavailable";
                    break;
                case "504": HTTPStatusDescriptionTextBox.Text = "Gateway Timeout";
                    break;
                case "505": HTTPStatusDescriptionTextBox.Text = "HTTP Version Not Supported";
                    break;
                case "506": HTTPStatusDescriptionTextBox.Text = "Variant Also Negotiates";
                    break;
                case "507": HTTPStatusDescriptionTextBox.Text = "Insufficient Storage";
                    break;
                case "508": HTTPStatusDescriptionTextBox.Text = "Loop Detected";
                    break;
                case "510": HTTPStatusDescriptionTextBox.Text = "Not Extended";
                    break;
                case "511": HTTPStatusDescriptionTextBox.Text = "Network Authentication Required";
                    break;
                case "599": HTTPStatusDescriptionTextBox.Text = "Network Connect Timeout Error";
                    break;
                case "DIS": HTTPStatusDescriptionTextBox.Text = "Inspector disabled";
                    break;
                default: HTTPStatusDescriptionTextBox.Text = "No known HTTP status.";
                    break;
            }
        }

        private void ResponseCommentLabel_Click(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void ProcessTextBox_TextChanged(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void HTTPStatusDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void RequestBeginTimeTextBox_TextChanged(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Do nothing.
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void ElapsedTimeComemntTextBox_TextChanged(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void ResponseCommentsRichTextBox_TextChanged(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void WriteSessionData()
        {
            // Put all the data together to be sent to text file.
            SessionData = "HIGH LEVEL SESSION DATA" + Environment.NewLine + Environment.NewLine +
                "Session ID: " + SessionIDTextbox.Text + Environment.NewLine +
                "HTTP Response Code: " + HTTPResponseCodeTextBox.Text + Environment.NewLine +
                "Client Begin Request: " + ClientRequestBeginDateTextBox.Text + " " + ClientRequestBeginTimeTextBox.Text + Environment.NewLine +
                "Client Done Response: " + ClientRequestEndDateTextBox.Text + " " + ClientRequestEndTimeTextBox.Text + Environment.NewLine +
                //"Elapsed Time: " + ElapsedTimeTextBox.Text + " " + ElapsedTimeCommentTextBox.Text + Environment.NewLine +
                "Local Process: " + ResponseProcessTextBox.Text + Environment.NewLine +
                "Exchange Type: " + ExchangeTypeTextbox.Text + Environment.NewLine +
                "Response Server: " + ResponseServerTextBox.Text + Environment.NewLine +
                "Response Alert: " + ResponseAlertTextBox.Text + Environment.NewLine + Environment.NewLine +
                "Response Comments: " + Environment.NewLine + "------------------------------------------" + Environment.NewLine +
                ResponseCommentsRichTextBox.Text + Environment.NewLine + "------------------------------------------" + Environment.NewLine + Environment.NewLine +
                "REQUEST HEADERS" + Environment.NewLine + "------------------------------------------" + Environment.NewLine +
                RequestHeadersTextBox.Text + Environment.NewLine + "------------------------------------------" + Environment.NewLine + Environment.NewLine +
                "REQUEST BODY" + Environment.NewLine + "------------------------------------------" + Environment.NewLine +
                RequestBodyTextbox.Text + Environment.NewLine + "------------------------------------------" + Environment.NewLine + Environment.NewLine +
                "RESPONSE HEADERS" + Environment.NewLine + "------------------------------------------" + Environment.NewLine +
                ResponseHeadersTextbox.Text + Environment.NewLine + "------------------------------------------" + Environment.NewLine + Environment.NewLine +
                "RESPONSE BODY" + Environment.NewLine + "------------------------------------------" + Environment.NewLine +
                ResponseBodyTextbox.Text + Environment.NewLine + "------------------------------------------";
        }

        private void SaveSessionDataButton_Click(object sender, EventArgs e)
        {
            if (RequestBodyTextbox.Text == "")
            {
                RequestBodyTextbox.Text = "-- Request Body was found to be blank in session. --";
            }
            if (ResponseBodyTextbox.Text == "")
            {
                ResponseBodyTextbox.Text = "-- Response Body was found to be blank in the session. --";
            }
            if (ResponseHeadersTextbox.Text == "")
            {
                ResponseHeadersTextbox.Text = "-- Response Headers were found to be blank in the session. --";
            }

            // Initialise new SaveFileDialog.
            SaveFileDialog save = new SaveFileDialog();

            // Use the user setting PreviousPath to determine if we open %USERPROFILE%\Documents or some other previous path.
            if (string.IsNullOrEmpty(Properties.Settings.Default.PreviousPath))
            {
                save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                save.InitialDirectory = Properties.Settings.Default.PreviousPath;
            }

            // Setup dialog.
            save.FileName = "FiddlerTrace-SessionID-" + SessionIDTextbox.Text + "-HTTP-" + HTTPResponseCodeTextBox.Text + ".txt";
            save.RestoreDirectory = true;
            save.Filter = "Text File|*.txt";

            WriteSessionData();

            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.Write(SessionData);
                writer.Dispose();
                writer.Close();
            }
        }

        private void OpenSessionData_Click(object sender, EventArgs e)
        {
            // As the user has elected to open the file instead of save somewhere specific, write data out to a text file in %TEMP% environment variable and open it up in Notepad.
            WriteSessionData();
            System.IO.File.WriteAllText(Environment.GetEnvironmentVariable("temp") + "\\FiddlerTrace - SessionID - " + SessionIDTextbox.Text + " - HTTP - " + HTTPResponseCodeTextBox.Text + ".txt", SessionData);
            System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("temp") + "\\FiddlerTrace - SessionID - " + SessionIDTextbox.Text + " - HTTP - " + HTTPResponseCodeTextBox.Text + ".txt");
        }

        private void DeveloperControlsLabel_Click(object sender, EventArgs e)
        {
            // Do nothing.
        }

        private void RemoveAllAppPrefsButton_Click(object sender, EventArgs e)
        {
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.enabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ColumnsEnableAll");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.DemoMode");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.DemoModeBreakScenarios");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.AppLoggingEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerInspector.ExecutionCount");
            MessageBox.Show("Removed extensions.EXOFiddlerInspector Prefs.");
        }

        private void ServerResponseDurationTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ServerGotRequestLabel_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ServerGotRequestTimeTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ServerResponseDurationLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
