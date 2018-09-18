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
        //private byte[] _body;
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

    // DISABLING REQUEST TAB, DOES NOT ADD VALUE TO INSPECTOR.
    /*
    // Request class, inherits the generic class above, only defines things specific or different from the base class
    public class RequestInspector : EXOBaseFiddlerInspector, IRequestInspector2
    {
        private bool _readOnly;
        HTTPRequestHeaders _headers;
        private byte[] _body;

        RequestUserControl _displayControl;
        
        // Double click on a session to highlight inpsector or not.
        public override int ScoreForSession(Session oS)
        {
            this.session = oS;

            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            _displayControl.SetRequestAlertTextBox("");

            if (this.session.url.Contains("autodiscover"))
            {
                //_displayControl.SetRequestAlertTextBox("SFS:100");
                return 100;
            }
            else if (this.session.hostname.Contains("autodiscover"))
            {
                //_displayControl.SetRequestAlertTextBox("SFS:100");
                return 100;
            }
            else if (this.session.url.Contains("outlook"))
            {
                //_displayControl.SetRequestAlertTextBox("SFS:100");
                return 100;
            }
            else if (this.session.url.Contains("GetUserAvailability") || 
                this.session.url.Contains("WSSecurity") ||
                this.session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                //_displayControl.SetRequestAlertTextBox("SFS:100");
                return 100;
            }
            else if (this.session.LocalProcess.Contains("outlook"))
            {
                //_displayControl.SetRequestAlertTextBox("SFS:100");
                return 100;
            }
            else
            {
                //_displayControl.SetRequestAlertTextBox("SFS:0");
                return 0;
            }
        }

        // Add EXO Request tab into inspectors tab.
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
            { }
        }
        
        public void SetSessionType(Session oS)
        {
            if (this.session.fullUrl.Contains("outlook.office365.com/mapi")) { _displayControl.SetRequestTypeTextBox("EXO MAPI"); }
            else if (this.session.fullUrl.Contains("autodiscover-s.outlook.com")) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
            else if (this.session.fullUrl.Contains("onmicrosoft.com/autodiscover")) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
            else if (this.session.utilFindInResponse("<Action>redirectAddr</Action>", false) > 1) { _displayControl.SetRequestTypeTextBox("On-Prem Autodiscover Redirect"); }
            else if (this.session.utilFindInRequest("autodiscover", false) > 1 && this.session.utilFindInRequest("onmicrosoft.com", false) > 1) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
            else if (this.session.fullUrl.Contains("autodiscover") && (this.session.fullUrl.Contains(".onmicrosoft.com"))) { _displayControl.SetRequestTypeTextBox("EXO Autodiscover"); }
            else if (this.session.fullUrl.Contains("autodiscover")) { _displayControl.SetRequestTypeTextBox("Autodiscover"); }
            else if (this.session.url.Contains("autodiscover")) { _displayControl.SetRequestTypeTextBox("Autodiscover"); }
            else if (this.session.hostname.Contains("autodiscover")) { _displayControl.SetRequestTypeTextBox("Autodiscover"); }
            else if (this.session.fullUrl.Contains("WSSecurity")) { _displayControl.SetRequestTypeTextBox("Free/Busy"); }
            else if (this.session.fullUrl.Contains("GetUserAvailability")) { _displayControl.SetRequestTypeTextBox("Free/Busy"); }
            else if (this.session.utilFindInResponse("GetUserAvailability", false) > 1) { _displayControl.SetRequestTypeTextBox("Free/Busy"); }
            else if (this.session.fullUrl.Contains("outlook.office365.com/EWS")) { _displayControl.SetRequestTypeTextBox("EXO EWS"); }
            else if (this.session.fullUrl.Contains(".onmicrosoft.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
            else if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com")) { _displayControl.SetRequestTypeTextBox("Office 365 Authentication"); }
            else if (this.session.fullUrl.Contains("outlook.office365.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
            else if (this.session.fullUrl.Contains("outlook.office.com")) { _displayControl.SetRequestTypeTextBox("Office 365"); }
            else if (this.session.fullUrl.Contains("adfs/services/trust/mex")) { _displayControl.SetRequestTypeTextBox("ADFS Authentication"); }
            else if (this.session.LocalProcess.Contains("outlook")) { _displayControl.SetRequestTypeTextBox("Something Outlook"); }
            else if (this.session.LocalProcess.Contains("iexplore")) { _displayControl.SetRequestTypeTextBox("Something Internet Explorer"); }
            else if (this.session.LocalProcess.Contains("chrome")) { _displayControl.SetRequestTypeTextBox("Something Chrome"); }
            else if (this.session.LocalProcess.Contains("firefox")) { _displayControl.SetRequestTypeTextBox("Something Firefox"); }
            else { _displayControl.SetRequestTypeTextBox("Not Exchange"); }
        }
        
        public void SetRequestValues(Session oS)
        {

            // Store response body in variable for opening in notepad.
            EXOResponseBody = this.session.oResponse.ToString();

            // Write HTTP Status Code Text box, convert int to string.
            _displayControl.SetRequestHostTextBox(this.session.hostname);

            // Write Request URL Text box.
            _displayControl.SetRequestURLTextBox(this.session.url);

            // Set Request Process Textbox.
            _displayControl.SetRequestProcessTextBox(this.session.LocalProcess);

            // Classify type of traffic. Set in order of presence to correctly identify as much traffic as possible.
            // First off make sure we only classify traffic from Outlook or browsers.
            if (this.session.LocalProcess.Contains("outlook") ||
                    this.session.LocalProcess.Contains("iexplore") ||
                    this.session.LocalProcess.Contains("chrome") ||
                    this.session.LocalProcess.Contains("firefox") ||
                    this.session.LocalProcess.Contains("edge") ||
                    this.session.LocalProcess.Contains("w3wp"))
            {
                SetSessionType(this.session);
            }
            else
            // If the traffic did not originate from Outlook, web browser or EXO web service (w3wp), call it out.
            {
                _displayControl.SetRequestTypeTextBox("Not from Outlook, EXO Browser or web service.");
            }
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

        public string EXOResponseBody { get; set; }
    }
    */

    // Response class, same as request class except for responses
    public class ResponseInspector : EXOBaseFiddlerInspector, IResponseInspector2
    {
        ResponseUserControl _displayControl;
        private HTTPResponseHeaders responseHeaders;
        private string searchTerm;
        private string RealmURL;

        // Double click on a session to highlight inpsector.
        public override int ScoreForSession(Session oS)
        {
            this.session = oS;

            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            if (this.session.LocalProcess.Contains("outlook") ||
            this.session.LocalProcess.Contains("searchprotocolhost") ||
            this.session.LocalProcess.Contains("iexplore") ||
            this.session.LocalProcess.Contains("chrome") ||
            this.session.LocalProcess.Contains("firefox") ||
            this.session.LocalProcess.Contains("edge") ||
            this.session.LocalProcess.Contains("w3wp"))
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
                SetResponseValues(this.session);
            }
        }

        public void SetResponseValues(Session oS)
        {

            this.session = oS;

            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            // Clear any previous data.
            _displayControl.SetResponseAlertTextBox("");
            _displayControl.SetResponseCommentsRichTextboxText("");
            // _displayControl.SetElapsedTimeCommentTextBoxText("");

            // Write HTTP Status Code Text box, convert int to string.
            _displayControl.SetHTTPResponseCodeTextBoxText(this.session.responseCode.ToString());

            // Write Client Begin Request into textboxes
            _displayControl.SetRequestBeginDateTextBox(this.session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd"));
            _displayControl.SetRequestBeginTimeTextBox(this.session.Timers.ClientBeginRequest.ToString(" H:mm:ss.ffff"));

            // Write Client End Request into textboxes
            _displayControl.SetRequestEndDateTextBox(this.session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd"));
            _displayControl.SetRequestEndTimeTextBox(this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.ffff"));

            // Write Elapsed Time into textbox.
            _displayControl.SetResponseElapsedTimeTextBox(this.session.oResponse.iTTLB + "ms");

            //Write response server from headers into textbox.
            _displayControl.SetResponseServerTextBoxText(this.session.oResponse["Server"]);

            // Write Elapsed Time comment into textbox.
            if (this.session.oResponse.iTTLB > 5000)
            {
                //     _displayControl.SetElapsedTimeCommentTextBoxText("> 5 second response time.");
            }

            // Write Data age data into textbox.
            String DataAgeOutput = "";
            DateTime SessionDateTime = this.session.Timers.ClientBeginRequest;
            DateTime DateTimeNow = DateTime.Now;
            TimeSpan CalcDataAge = DateTimeNow - SessionDateTime;
            int TimeSpanDays = CalcDataAge.Days;
            int TimeSpanHours = CalcDataAge.Hours;
            int TimeSpanMinutes = CalcDataAge.Minutes;

            if (TimeSpanDays == 0)
            {
                DataAgeOutput = "Session is " + TimeSpanHours + " Hour(s), " + TimeSpanMinutes + " minute(s) old.";
            }
            else
            {
                DataAgeOutput = "Session is " + TimeSpanDays + " Day(s), " + TimeSpanHours + " Hour(s), " + TimeSpanMinutes + " minute(s) old.";
            }

            _displayControl.SetDataAgeTextBox(DataAgeOutput);

            // Write Process into textbox.
            _displayControl.SetResponseProcessTextBox(this.session.LocalProcess);

            //var ruleSet = new WebTrafficRuleSet(session);
            //ruleSet.RunWebTrafficRuleSet();

            int wordCount = 0;

            // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
            //
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
            //

            string text = this.session.ToString();

            //Convert the string into an array of words  
            string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                             select word;

            // Query samples:
            //string searchTerm = "error";
            //string[] searchTerms = { "Error", "FederatedStsUnreachable" };

            switch (this.session.responseCode)
                {
                    case 0:
                       #region HTTP0
                        /////////////////////////////
                        //
                        //  HTTP 0: No Response.
                        //

                        // Thinking a check on this.session["X-ResponseCode"] is needed to eliminate false positives here.
                        _displayControl.SetResponseAlertTextBox("HTTP 0 No Response!");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTPQuantity);
                        //
                        /////////////////////////////
                        break;
                        #endregion
                    case 200:
                        #region HTTP200s
                        /////////////////////////////
                        //
                        // HTTP 200
                        //
                        
                        /////////////////////////////
                        // 1. Exchange On-Premise Autodiscover redirect.
                        if (this.session.utilFindInResponse("<Action>redirectAddr</Action>", false) > 1)
                            {
                                /*
                                <?xml version="1.0" encoding="utf-8"?>
                                <Autodiscover xmlns="http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006">
                                <Response xmlns="http://schemas.microsoft.com/exchange/autodiscover/outlook/responseschema/2006a">
                                <Account>
                                <Action>redirectAddr</Action>
                                <RedirectAddr>user@contoso.mail.onmicrosoft.com</RedirectAddr>       
                                </Account>
                                </Response>
                                </Autodiscover>
                                */

                                // Logic to detected the redirect address in this session.
                                // 
                                string RedirectResponseBody = this.session.GetResponseBodyAsString();
                                int start = this.session.GetResponseBodyAsString().IndexOf("<RedirectAddr>");
                                int end = this.session.GetResponseBodyAsString().IndexOf("</RedirectAddr>");
                                int charcount = end - start;
                                string RedirectAddress = RedirectResponseBody.Substring(start, charcount).Replace("<RedirectAddr>", "");
                        
                            if (RedirectAddress.Contains(".onmicrosoft.com"))
                                {
                                    _displayControl.SetResponseAlertTextBox("Exchange On-Premise Autodiscover redirect.");
                                    _displayControl.SetResponseCommentsRichTextboxText("Exchange On-Premise Autodiscover redirect address to Exchange Online found." + Environment.NewLine + RedirectAddress);
                                }
                                // Highlight if we got this far and do not have a redirect address which points to
                                // Exchange Online such as: contoso.mail.onmicrosoft.com.
                                else
                                {
                                    _displayControl.SetResponseAlertTextBox("Exchange On-Premise Autodiscover redirect.");
                                    _displayControl.SetResponseCommentsRichTextboxText("Exchange On-Premise Autodiscover redirect address found, which does not contain .onmicrosoft.com." + Environment.NewLine + RedirectAddress);
                                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 200 On-Prem Autodiscover redirect - Address doesn't contain .onmicrosoft.com.");
                                }
                            }

                        /////////////////////////////
                        //
                        // 2. Exchange On-Premise Autodiscover redirect - address can't be found
                        //
                        if ((this.session.utilFindInResponse("<Message>The email address can't be found.</Message>", false) > 1) && 
                            (this.session.utilFindInResponse("<ErrorCode>500</ErrorCode>", false) > 1))
                            {
                                /*
                                <?xml version="1.0" encoding="utf-8"?>
                                <Autodiscover xmlns="http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006">
                                  <Response>
                                    <Error Time="12:03:32.8803744" Id="2422600485">
                                      <ErrorCode>500</ErrorCode>
                                      <Message>The email address can't be found.</Message>
                                      <DebugData />
                                    </Error>
                                  </Response>
                                </Autodiscover>
                                */
                                _displayControl.SetResponseAlertTextBox("Exchange On-Premise Autodiscover redirect: Error Code 500.");
                                _displayControl.SetResponseCommentsRichTextboxText("Exchange On-Premise Autodiscover redirect address can't be found.");
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 200 On-Prem Autodiscover redirect - Address can't be found.");
                            }

                        /////////////////////////////
                        //
                        // 99. No other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.
                        else
                        {
                            
                            searchTerm = "Error";

                            // Count the matches, which executes the query.  
                            wordCount = matchQuery.Count();

                            if (wordCount > 0)
                            {
                                string result = "After splitting all words in the response body the word 'error' was found " + wordCount + " time(s).";
                                _displayControl.SetResponseAlertTextBox("Word Search 'Error' found in respone body.");
                                _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP200ErrorsFound + result);
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 200 keyword 'error' found in response body!");
                            }
                            else
                            {
                                string result = "Keyword 'error' not found in response body.";
                                _displayControl.SetResponseAlertTextBox("Word Search 'Error' found in respone body.");
                                _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP200ErrorsFound + result);
                            }
                        }
                        //
                        /////////////////////////////
                        break;
                    case 204:
                        /////////////////////////////
                        //
                        //  HTTP 204: No Content.
                        //
                        _displayControl.SetResponseAlertTextBox("HTTP 204 No Content.");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTPQuantity);
                        //
                        /////////////////////////////
                        break;
                        #endregion
                    case 301:
                        #region HTTP300s
                        /////////////////////////////
                        //
                        //  HTTP 301: Moved Permanently.
                        //
                        _displayControl.SetResponseAlertTextBox("HTTP 301 Moved Permanently");
                        _displayControl.SetResponseCommentsRichTextboxText("Nothing of concern here at this time.");
                        //
                        /////////////////////////////
                        break;
                    case 302:
                        /////////////////////////////
                        //
                        //  HTTP 302: Found / Redirect.
                        //
                        if (session.utilFindInResponse("https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml", false) > 1)
                        {
                            _displayControl.SetResponseAlertTextBox("Exchange On-Premise Autodiscover redirect to Exchange Online.");
                            _displayControl.SetResponseCommentsRichTextboxText("Exchange On-Premise Autodiscover redirect to Exchange Online.");
                        }
                        //
                        /////////////////////////////
                        break;
                    case 304:
                        /////////////////////////////
                        //
                        //  HTTP 304: Not modified.
                        //
                        _displayControl.SetResponseAlertTextBox("HTTP 304 Not Modified");
                        _displayControl.SetResponseCommentsRichTextboxText("Nothing of concern here at this time.");
                        //
                        /////////////////////////////
                        break;
                    case 307:
                        /////////////////////////////
                        //
                        //  HTTP 307: Temporary Redirect.
                        //

                        // Specific scenario where a HTTP 307 Temporary Redirect incorrectly send an EXO Autodiscover request to an On-Premise resource, breaking Outlook connectivity.
                        if (this.session.hostname.Contains("autodiscover") && 
                            (this.session.hostname.Contains("mail.onmicrosoft.com") && 
                            (this.session.fullUrl.Contains("autodiscover") && 
                            (this.session.ResponseHeaders["Location"] != "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml"))))
                        {
                            _displayControl.SetResponseAlertTextBox("HTTP 307 Temporary Redirect");
                            _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP307IncorrectTemporaryRedirect);
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 307 On-Prem Temp Redirect - Unexpected location!");
                        } else
                        {
                            _displayControl.SetResponseAlertTextBox("HTTP 307 Temporary Redirect");
                            _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP307TemporaryRedirect);
                        }
                        //
                        /////////////////////////////
                        break;
                        #endregion
                    case 401:
                        #region HTTP400s
                        /////////////////////////////
                        //
                        //  HTTP 401: UNAUTHORIZED.
                        //
                        _displayControl.SetResponseAlertTextBox("HTTP 401 Unauthorized");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP401Unauthorized);
                        //
                        /////////////////////////////
                        break;
                    case 403:
                        /////////////////////////////
                        //
                        //  HTTP 403: FORBIDDEN.
                        //
                        // Simply looking for the term "Access Denied" works fine using utilFindInResponse.
                        // Specific scenario where a web proxy is blocking traffic.
                        if (this.session.utilFindInResponse("Access Denied", false) > 1)
                        {
                            _displayControl.SetResponseAlertTextBox("HTTP 403 Access Denied!");
                            _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP403WebProxyBlocking);
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");
                        }
                        else
                        {
                            // Pick up any 403 Forbidden and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("HTTP 403 Forbidden!");
                            _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP403Generic);
                        }
                        //
                        /////////////////////////////
                        break;
                    case 404:
                        /////////////////////////////
                        //
                        //  HTTP 404: Not Found.
                        //
                        // Pick up any 404 Not Found and write data into the comments box.
                        _displayControl.SetResponseAlertTextBox("HTTP 404 Not Found");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTPQuantity);
                        //
                        /////////////////////////////
                        break;
                    case 429:
                        /////////////////////////////
                        //
                        //  HTTP 429: Too Many Requests.
                        //
                        _displayControl.SetResponseAlertTextBox("HTTP 429 Too Many Requests");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP429TooManyRequests);
                        //
                        /////////////////////////////
                        break;
                    case 440:
                        /////////////////////////////
                        //
                        // HTTP 440: Need to know more about these.
                        // For the moment do nothing.
                        //
                        /////////////////////////////
                        break;
                    #endregion
                    case 500:
                        #region HTTP500s
                        /////////////////////////////
                        //
                        //  HTTP 500: Internal Server Error.
                        //
                        // Pick up any 500 Internal Server Error and write data into the comments box.
                        // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in green? >
                        // Pick up any 500 Internal Server Error and write data into the comments box.
                        _displayControl.SetResponseAlertTextBox("HTTP 500 Internal Server Error");
                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 500 Internal Server Error");
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 500 Internal Server Error.");
                        break;
                        //
                        /////////////////////////////
                    case 502:
                        /////////////////////////////
                        //
                        //  HTTP 502: BAD GATEWAY.
                        //

                        /////////////////////////////
                        //
                        // 1. telemetry false positive. <Need to validate in working scenarios>
                        //
                        if ((this.session.oRequest["Host"] == "sqm.telemetry.microsoft.com:443") &&
                            (this.session.utilFindInResponse("target machine actively refused it", false) > 1))
                            {
                                _displayControl.SetResponseAlertTextBox("These aren't the droids your looking for.");
                                _displayControl.SetResponseCommentsRichTextboxText("Unlikely the cause of Outlook / OWA connectivity.");
                            }

                        /////////////////////////////
                        //
                        // 2. Exchange Online Autodiscover False Positive.
                        //
                        // Specific scenario on Outlook & OFffice 365 Autodiscover false positive on connections to:
                        //      autodiscover.domain.onmicrosoft.com:443
                        else if ((this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                            (this.session.utilFindInResponse("autodiscover", false) > 1) &&
                            (this.session.utilFindInResponse(":443", false) > 1))
                                {
                                    _displayControl.SetResponseAlertTextBox("These aren't the droids your looking for.");
                                    _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP502AutodiscoverFalsePositive);
                                }

                        /////////////////////////////
                        //
                        // 3. Exchange Online DNS lookup on contoso.onmicrosoft.com, False Positive!?
                        //
                        // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
                        else if ((this.session.utilFindInResponse("The requested name is valid, but no data of the requested type was found", false) > 1) &&
                                // Found Outlook is going root domain Autodiscover lookups. Vanity domain, which we have no way to key off of in logic here.
                                // Excluding this if statement to broaden DNS lookups we say are OK.
                                //if (this.session.utilFindInResponse(".onmicrosoft.com", false) > 1)
                                //{
                                (this.session.utilFindInResponse("failed. System.Net.Sockets.SocketException", false) > 1) &&
                                (this.session.utilFindInResponse("DNS Lookup for ", false) > 1))
                            {
                                _displayControl.SetResponseAlertTextBox("These aren't the droids your looking for.");
                                _displayControl.SetResponseCommentsRichTextboxText("DNS record does not exist. Connection on port 443 will not work by design.");
                            }

                        /////////////////////////////
                        //
                        // 99. Everything else.
                        //
                        else
                        {
                            // Pick up any other 502 Bad Gateway and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("HTTP 502 Bad Gateway");
                            _displayControl.SetResponseCommentsRichTextboxText("Nothing detected directly related to Exchange Online.");
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 502 Bad Gateway.");
                        }
                        //
                        /////////////////////////////
                        break;
                    case 503:
                        /////////////////////////////
                        //
                        //  HTTP 503: SERVICE UNAVAILABLE.
                        //
                        // Specific scenario where Federation service is unavailable, preventing authentication, preventing access to Office 365 mailbox.
                        searchTerm = "FederatedStsUnreachable";
                        //"Service Unavailable"

                        // Count the matches, which executes the query.  
                        wordCount = matchQuery.Count();
                        if (wordCount > 0)
                        {
                            //XAnchorMailbox = this.session.oRequest["X-AnchorMailbox"];
                            RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";

                            _displayControl.SetResponseAlertTextBox("The federation service is unreachable or unavailable.");
                            _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP503FederatedSTSUnreachableStart + Environment.NewLine + RealmURL + Environment.NewLine + Properties.Settings.Default.HTTP503FederatedSTSUnreachableEnd);
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 503 Service Unavailable. Found keyword 'FederatedStsUnreachable' in response body!");
                        }
                        else
                        {
                            // Pick up any other 503 Service Unavailable and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("HTTP 503 Service Unavailable.");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 503 Service Unavailable.");
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 503 Service Unavailable.");
                        }
                        //
                        /////////////////////////////
                        break;
                    case 504:
                        /////////////////////////////
                        //
                        //  HTTP 504: GATEWAY TIMEOUT.
                        //
                        // Pick up any 504 Gateway Timeout and write data into the comments box.
                        _displayControl.SetResponseAlertTextBox("HTTP 504 Gateway Timeout");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTPQuantity);
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 504 Gateway Timeout.");
                        //
                        /////////////////////////////
                        break;
                        #endregion
                    default:
                        break;
                }
        }
        
        /////////////////////////////
        // Add the EXO Response tab into the inspector tab.
        public override void AddToTab(TabPage o)
        {
            _displayControl = new ResponseUserControl();
            o.Text = "Exchange Online";
            o.ToolTipText = "Exchange Online Inspector";
            o.Controls.Add(_displayControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }
        //
        /////////////////////////////

        // Mandatory, but not sure what this does.
        public override int GetOrder()
        {
            return 0;
        }

        // Not sure what to do with this.
        void IBaseInspector2.Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}
