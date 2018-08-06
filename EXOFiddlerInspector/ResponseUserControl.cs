using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EXOFiddlerInspector
{
    public partial class ResponseUserControl : UserControl
    {
        public ResponseUserControl()
        {
            InitializeComponent();
        }

        private void ResponseUserControl_Load(object sender, EventArgs e)
        {

        }

        // Code to write to ResponseCommentsTextBox.Text value.
        internal void SetResponseCommentsTextBoxText(string txt)
        {
            ResponseCommentsTextBox.Text = txt;
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

        // Code to write to RequestBeginTimeTextBox.Text value.
        internal void SetRequestBeginTimeTextBox(string txt)
        {
            RequestBeginTimeTextBox.Text = txt;
        }

        // Code to write to RequestEndTimeTextBox.Text value.
        internal void SetRequestEndTimeTextBox(string txt)
        {
            RequestEndTimeTextBox.Text = txt;
        }

        // Code to write to TimeElapsedTextBox.Text value.
        internal void SetResponseElapsedTimeTextBox(string txt)
        {
            ElapsedTimeTextBox.Text = txt;
        }

        // Code to write to ResponseAlertTextBox.Text value.
        internal void SetResponseAlertTextBox(string txt)
        {
            ResponseAlertTextBox.Text = txt;
        }
        

        private void HTTPStatusCodeLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
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
                default: HTTPStatusDescriptionTextBox.Text = "No known HTTP status.";
                    break;
            } 
        }

        private void ResponseCommentLabel_Click(object sender, EventArgs e)
        {

        }

        private void ProcessTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void HTTPStatusDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void RequestBeginTimeTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
