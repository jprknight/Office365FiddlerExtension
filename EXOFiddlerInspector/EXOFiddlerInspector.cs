using System.Windows.Forms;
using System.Linq;
using System.IO;
using Fiddler;
using System;

namespace EXOFiddlerInspector
{
    // Base class, generic inspector, common between request and response
    public class EXOBaseFiddlerInspector : Inspector2
    {
        private byte[] _body;
        private bool _readOnly;

        internal byte[] rawBody;

        internal Session session { get; set; }

        public bool bDirty
        {
            get { return false; }
        }

        public bool bReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
            }
        }

        public override void AddToTab(TabPage o)
        {
            throw new System.NotImplementedException();
        }

        public override int GetOrder()
        {
            throw new System.NotImplementedException();
        }

        public override void AssignSession(Session oS)
        {
            this.session = oS;

            base.AssignSession(oS);
        }
    }

    // Request class, inherits the generic class above, only defines things specific or different from the base class
    public class RequestInspector : EXOBaseFiddlerInspector, IRequestInspector2
    {
        private bool _readOnly;
        HTTPRequestHeaders _headers;
        private byte[] _body;
        RequestUserControl _displayControl;

        // Double click on a session to highlight inpsector.
        public override int ScoreForSession(Session oS)
        {
            this.session = oS;

            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            if (this.session.url.Contains("autodiscover"))
            {
                return 100;
            }
            else if (this.session.hostname.Contains("autodiscover"))
            {
                return 100;
            }
            else if (this.session.url.Contains("outlook"))
            {
                return 100;
            }
            else if (this.session.url.Contains("GetUserAvailability") || 
                this.session.url.Contains("WSSecurity") ||
                this.session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                return 100;
            }
            else if (this.session.LocalProcess.Contains("outlook"))
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        public override void AddToTab(TabPage o)
        {
            _displayControl = new RequestUserControl();
            o.Text = "Exchange Request";
            o.ToolTipText = "Exchange Online Inspector";
            o.Controls.Add(_displayControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }

        public HTTPRequestHeaders headers
        {
            get
            {
                return _headers;

            }
            set
            {
                /*_headers = value;
                System.Collections.Generic.Dictionary<string, string> httpHeaders =
                    new System.Collections.Generic.Dictionary<string, string>();
                foreach (var item in headers)
                {
                    httpHeaders.Add(item.Name, item.Value);
                }*/
                //_displayControl.Headers = httpHeaders;

            }
        }

 /*       public void Sessions(Session oS)
        {
            if (oS.fullUrl.Contains("autodiscover-s.outlook.com")) {
                _displayControl.Text = "365 Autodiscover";
            }
        }
*/
        public void SetRequestValues(Session oS)
        {
            // Write HTTP Status Code Text box, convert int to string.
            _displayControl.SetRequestHostTextBox(this.session.hostname);

            // Write Request URL Text box.
            _displayControl.SetRequestURLTextBox(this.session.url);

            // Classify type on traffic. Set in order of presence to correctly identify as much traffic as possible.
            // First off make sure we only classify traffic from Outlook or browsers.
            if (this.session.LocalProcess.Contains("outlook") ||
                    this.session.LocalProcess.Contains("iexplore") ||
                    this.session.LocalProcess.Contains("chrome") ||
                    this.session.LocalProcess.Contains("firefox") ||
                    this.session.LocalProcess.Contains("edge") ||
                    this.session.LocalProcess.Contains("w3wp"))
            {

                if (this.session.fullUrl.Contains("outlook.office365.com/mapi")) { _displayControl.SetRequestTypeTextBox("EXO MAPI"); }
                else if (this.session.fullUrl.Contains("autodiscover-s.outlook.com")) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
                else if (this.session.fullUrl.Contains("onmicrosoft.com/autodiscover")) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
                else if (this.session.utilFindInRequest("autodiscover", false) > 1 && this.session.utilFindInRequest("onmicrosoft.com", false) > 1) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
                else if (this.session.fullUrl.Contains("autodiscover") && (this.session.fullUrl.Contains(".onmicrosoft.com"))) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
                else if (this.session.fullUrl.Contains("autodiscover")) { _displayControl.SetRequestTypeTextBox("Autodiscover"); }
                else if (this.session.fullUrl.Contains("WSSecurity")) { _displayControl.SetRequestTypeTextBox("Free/Busy"); }
                else if (this.session.fullUrl.Contains("GetUserAvailability")) { _displayControl.SetRequestTypeTextBox("Free/Busy"); }
                else if (this.session.utilFindInResponse("GetUserAvailability", false) > 1) { _displayControl.SetRequestTypeTextBox("Free/Busy"); }
                else if (this.session.fullUrl.Contains("outlook.office365.com/EWS")) { _displayControl.SetRequestTypeTextBox("EXO EWS"); }
                else if (this.session.fullUrl.Contains(".onmicrosoft.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
                else if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com")) { _displayControl.SetRequestTypeTextBox("Office 365 Authentication"); }
                else if (this.session.fullUrl.Contains("outlook.office365.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
                else if (this.session.fullUrl.Contains("outlook.office.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
                else if (this.session.LocalProcess.Contains("outlook")) { _displayControl.SetRequestTypeTextBox("Something Outlook"); }
                else if (this.session.LocalProcess.Contains("iexplore")) { _displayControl.SetRequestTypeTextBox("Something Internet Explorer"); }
                else if (this.session.LocalProcess.Contains("chrome")) { _displayControl.SetRequestTypeTextBox("Something Chrome"); }
                else if (this.session.LocalProcess.Contains("firefox")) { _displayControl.SetRequestTypeTextBox("Something Firefox"); }
                else { _displayControl.SetRequestTypeTextBox("Not Exchange"); }
            }
            else
                // If the traffic did not originate from Outlook or a web browser, call it out.
                {
                    _displayControl.SetRequestTypeTextBox("Not Outlook or EXO Browser");
                }
            // Set Request Process Textbox.
            _displayControl.SetRequestProcessTextBox(this.session.LocalProcess);
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public override int GetOrder()
        {
            return 0;
        }

        public bool bDirty
        {
            get { return false; }
        }

        public bool bReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
            }
        }

        public byte[] body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                SetRequestValues(this.session);
                //_displayControl.Body = body;
            }
        }
    }

    // Response class, same as request class except for responses
    public class ResponseInspector : EXOBaseFiddlerInspector, IResponseInspector2
    {
        ResponseUserControl _displayControl;
        private HTTPResponseHeaders responseHeaders;
        //private int oResponseCode;

        // Double click on a session to highlight inpsector.
        public override int ScoreForSession(Session oS)
        {
            this.session = oS;

            if (this.session.url.Contains("autodiscover"))
            {
                return 100;
            }
            else if (this.session.hostname.Contains("autodiscover"))
            {
                return 100;
            }
            else if (this.session.url.Contains("outlook"))
            {
                return 100;
            }
            else if (this.session.url.Contains("GetUserAvailability"))
            {
                return 100;
            }
            else if (this.session.LocalProcess.Contains("outlook"))
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        public HTTPResponseHeaders headers
        {
            get { return responseHeaders; }
            set { responseHeaders = value;
            }
        }

        public byte[] body
        {
            get { return rawBody; }
            set
            {
                SetResponseComments(this.session);
            }
        }

        public void SetResponseComments (Session oS)
        {

            // Write HTTP Status Code Text box, convert int to string.
            _displayControl.SetHTTPResponseCodeTextBoxText(this.session.responseCode.ToString());

            // Write Client Begin Request into textbox
            _displayControl.SetRequestBeginTimeTextBox(this.session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd H:mm:ss.ffff"));

            // Write Client End Request into textbox
            _displayControl.SetRequestEndTimeTextBox(this.session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd H:mm:ss.ffff"));

            // Write Elapsed Time into textbox.
            _displayControl.SetResponseElapsedTimeTextBox(this.session.oResponse.iTTLB + "ms");

            // Write Data Freshness data into textbox.
            String DataFreshnessOutput = "";
            DateTime SessionDateTime = this.session.Timers.ClientBeginRequest;
            DateTime DateTimeNow = DateTime.Now;
            TimeSpan CalcDataFreshness = DateTimeNow - SessionDateTime;
            int TimeSpanDays = CalcDataFreshness.Days;
            int TimeSpanHours = CalcDataFreshness.Hours;
            int TimeSpanMinutes = CalcDataFreshness.Minutes;

            if (TimeSpanDays == 0)
            {
                DataFreshnessOutput = "Trace is " + TimeSpanHours + " Hour(s), " + TimeSpanMinutes + " minute(s) old.";
            } else
            {
                DataFreshnessOutput = "Trace is " + TimeSpanDays + " Day(s), " + TimeSpanHours + " Hour(s), " + TimeSpanMinutes + " minute(s) old.";
            }

            _displayControl.SetDataFreshnessTextBox(DataFreshnessOutput);
            
            // Write Process into textbox.
            _displayControl.SetResponseProcessTextBox(this.session.LocalProcess);

            // Clear any previous data.
            _displayControl.SetResponseAlertTextBox("");
            _displayControl.SetResponseCommentsWebBrowserDocumentText("");



            int wordCount = 0;

            // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
            //
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
            //

            string text = this.session.ToString();
                
            //Convert the string into an array of words  
            string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '"' }, StringSplitOptions.RemoveEmptyEntries);

            //string searchTerm = "error";
            string[] searchTerms = { "error", "FederatedStsUnreachable" };

            foreach (string searchTerm in searchTerms)
            {
                // Create the query.  Use ToLowerInvariant to match "data" and "Data"   
                var matchQuery = from word in source
                                    where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                    select word;

                // Count the matches, which executes the query.  
                wordCount = matchQuery.Count();

                //
                //  HTTP 200.
                //
                // Looking for errors lurking in HTTP 200 OK results.
                if (this.session.responseCode == 200)
                {
                    if (searchTerm == "Error")
                    {
                        string result = "After splitting all words in the response body the word 'error' was found " + wordCount + " time(s).";

                        if (wordCount > 0)
                        {
                            _displayControl.SetResponseAlertTextBox("Word Search 'Error' found in respone body.");
                            _displayControl.SetResponseCommentsWebBrowserDocumentText(Properties.Settings.Default.HTTP200ErrorsFound + "<br /><br />" + result);
                        }
                        else
                        {
                            _displayControl.SetResponseAlertTextBox("Word Search 'Error' Not found in response body.");
                            _displayControl.SetResponseCommentsWebBrowserDocumentText(result);
                        }
                    }
                }
                //
                //  HTTP 403.
                //
                // Simply looking for the term "Access Denied" works fine using utilFindInResponse.
                if (this.session.responseCode == 403)
                {
                    if (this.session.utilFindInResponse("Access Denied", false) > 1)
                    {
                        _displayControl.SetResponseAlertTextBox("Panic Stations!!!");
                        _displayControl.SetResponseCommentsWebBrowserDocumentText(Properties.Settings.Default.HTTP403WebProxyBlocking);
                    }
                }
                //
                //  HTTP 502.
                //
                else if (this.session.responseCode == 502)
                {
                    if (this.session.utilFindInResponse("autodiscover", false) > 1)
                    {
                        if (this.session.utilFindInResponse("target machine actively refused it", false) > 1)
                        {
                            if (this.session.utilFindInResponse(":443", false) > 1)
                            {
                                _displayControl.SetResponseAlertTextBox("These aren't the droids your looking for.");
                                _displayControl.SetResponseCommentsWebBrowserDocumentText(Properties.Settings.Default.HTTP502AutodiscoverFalsePositive);
                            }
                        }
                    }
                }
                //
                //  HTTP 503.
                //
                // Using utilFindInResponse to find FederatedStsUnreachable did not work for some reason.
                // So instead we split all words in the response body and check them with Linq.
                else if (this.session.responseCode == 503)
                {
                    if (searchTerm == "FederatedStsUnreachable")
                    {
                        if (wordCount > 0)
                        {
                            _displayControl.SetResponseAlertTextBox("The federation service is unreachable or unavailable.");
                            _displayControl.SetResponseCommentsWebBrowserDocumentText(Properties.Settings.Default.HTTP503FederatedSTSUnreachable);
                        }
                        else
                        {
                            _displayControl.SetResponseAlertTextBox("Federation failure error missed.");
                        }
                    }
                }              
            }
        }

        public override void AddToTab(TabPage o)
        {
            _displayControl = new ResponseUserControl();
            o.Text = "Exchange Response";
            o.ToolTipText = "Exchange Online Inspector";
            o.Controls.Add(_displayControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }



        /*public HTTPResponseHeaders headers
        {
            get
            {
                return _headers;
            }
            set
            {
                
                _headers = value;
                System.Collections.Generic.Dictionary<string, string> httpHeaders =
                    new System.Collections.Generic.Dictionary<string, string>();
                foreach (var item in headers)
                {
                    httpHeaders.Add(item.Name, item.Value);
                }
                //_displayControl.Headers = httpHeaders;
            }
        }*/

        //HTTPResponseHeaders IResponseInspector2.headers { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        //byte[] IBaseInspector2.body { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

            //bool IBaseInspector2.bReadOnly { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override int GetOrder()
        {
            return 0;
        }

        void IBaseInspector2.Clear()
        {
            throw new System.NotImplementedException();
        }
    }

}
