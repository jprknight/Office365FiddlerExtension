using System.Windows.Forms;
using System.Linq;
using System.IO;
using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EXOFiddlerInspector
{
    /// <summary>
    /// Base class, generic inspector, common between request and response
     /// </summary>
    public class EXOBaseFiddlerInspector : Inspector2
    {
        // These application preferences are actually set in ColouriseWebSessions.cs, pulling them into variables for use here.
        public bool bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public bool bResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);
        public bool bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public bool bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public bool bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public bool bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);

        public Boolean Developer;

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

    #region RequestInspectorNotInProduction

    /// <summary>
    /// Request Inspector class, inherits the generic class above, only defines things specific or different from the base class
    /// Request inspector tab not used in production.
    /// Code originally added to work out what was possible with Fiddler, however the inspector part
    /// of the extension has grown out of server responses rather than client requests.
    /// -- ScoreForSession.
    /// -- SetSessionType -- for request tab.
    /// -- SetRequestValues, small rule set.
    /// -- AddToTab.
    /// </summary>
    public class RequestInspector : EXOBaseFiddlerInspector, IRequestInspector2
    {

        
        private bool _readOnly;
        HTTPRequestHeaders _headers;
        private byte[] _body;

        RequestUserControl _displayControl;

        /// <summary>
        /// Double click or press return for Score For Session.
        /// </summary>
        /// <param name="oS"></param>
        /// <returns></returns>
        public override int ScoreForSession(Session oS)
        {
            // Discussion with EE, not expecting ScoreForSession to be consistent.

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

        /// <summary>
        /// Add Exchange Online Request tab into inspectors tab.
        /// </summary>
        /// <param name="o"></param>
        public override void AddToTab(TabPage o)
        {
            /////////////////////////////
            //
            // Before we go ahead and run the add tab code work out if 
            // the user is a developer or not.
            // Developer list is actually set in Preferences.cs.
            Preferences calledPreferences = new Preferences();
            List<string> calledDeveloperList = calledPreferences.GetDeveloperList();

            Boolean DeveloperDemoMode = calledPreferences.GetDeveloperMode();

            // Based on the above set the Boolean Developer for use through the rest of the code.
            if (calledDeveloperList.Any(Environment.UserName.Contains))
            {
                Developer = true;
            }
            else
            {
                Developer = false;
            }
            
            if (Developer)
            {
                _displayControl = new RequestUserControl();
                o.Text = "Exchange Request";
                o.ToolTipText = "Exchange Online Inspector";
                o.Controls.Add(_displayControl);
                o.Controls[0].Dock = DockStyle.Fill;
            }
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
            if (Developer)
            {
                // Earlier version of Exchange Type.
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
        }
        
        public void SetRequestValues(Session session)
        {
            this.session = session;

            if (Developer)
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

    #endregion

    /// <summary>
    /// Response Inspector class.
    /// -- ScoreForSession.
    /// -- SetResponseValues containing SessionRuleSet.
    /// -- AddToTab.
    /// </summary>
    public class ResponseInspector : EXOBaseFiddlerInspector, IResponseInspector2
    {
        public ResponseUserControl _displayControl;
        private HTTPResponseHeaders responseHeaders;
        // Used with Linq word split, looking for particular search terms in response body.
        private string searchTerm;
        // Used in HTTP 503 responses when dealing with Federated domains.
        private string RealmURL;

        private string RedirectAddress;

        #region ScoreForSession
        // Double click or hit return with session selected.
        // From discussion with EE Fiddler code known to be problematic with the ScoreForSession feature.
        // Not expected to work 100% of the time per logic below.
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
        #endregion

        public HTTPResponseHeaders headers
        {
            get { return responseHeaders; }
            set { responseHeaders = value;
            }
        }

        // Function which starts everything.
        public byte[] body
        {
            get { return rawBody; }
            set
            {
                // If the extension is enabled, start analysing the sessions.
                if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false))
                {
                    SetResponseValues(this.session);
                }
                // If the extension is disabled, do as little as possible, mark all user control fields as such.
                else
                {
                    _displayControl.SetHTTPResponseCodeTextBoxText("DIS");

                    _displayControl.SetClientRequestBeginDateTextBox("Inspector disabled.");
                    _displayControl.SetClientRequestBeginTimeTextBox("Inspector disabled.");
                    _displayControl.SetClientRequestEndDateTextBox("Inspector disabled.");
                    _displayControl.SetClientRequestEndTimeTextBox("Inspector disabled.");
                    _displayControl.SetResponseAlertTextBox("Inspector disabled.");
                    _displayControl.SetOverallElapsedTextbox("Inspector disabled");
                    _displayControl.SetServerThinkTimeTextbox("Inspector disabled");
                    _displayControl.SetDataAgeTextBox("Inspector disabled");
                    _displayControl.SetResponseProcessTextBox("Inspector disabled");
                    _displayControl.SetResponseServerTextBoxText("Inspector disabled");
                    _displayControl.SetResponseCommentsRichTextboxText("Inspector disabled.");
                }
            }
        }

        public HTTPRequestHeaders RequestHeaders { get; private set; }
        public HTTPResponseHeaders ResponseHeaders { get; private set; }

        /////////////////////////////
        // Function which analyses request/response data to provide additional feedback.
        public void SetResponseValues(Session oS)
        {
            Preferences calledPreferences = new Preferences();

            // create this.session for use everywhere in code.
            this.session = oS;

            /// <remarks

            // decode sessions to make sure request/response body can be fully read by logic checks.
            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            // Clear any previous data.
            _displayControl.SetResponseAlertTextBox("");
            _displayControl.SetResponseCommentsRichTextboxText("");
            _displayControl.SetRequestHeadersTextBoxText("");
            _displayControl.SetRequestBodyTextBoxText("");
            _displayControl.SetResponseHeadersTextBoxText("");
            _displayControl.SetResponseBodyTextBoxText("");
            _displayControl.SetExchangeTypeTextBoxText("");

            _displayControl.SetClientRequestBeginDateTextBox("");
            _displayControl.SetClientRequestBeginTimeTextBox("");

            _displayControl.SetClientRequestEndDateTextBox("");
            _displayControl.SetClientRequestEndTimeTextBox("");

            _displayControl.SetOverallElapsedTextbox("");

            _displayControl.SetServerGotRequestDateTextbox("");
            _displayControl.SetServerGotRequestTimeTextbox("");

            _displayControl.SetServerDoneResponseDateTextbox("");
            _displayControl.SetServerDoneResponseTimeTextbox("");

            _displayControl.SetServerThinkTimeTextbox("");

            _displayControl.SetXHostIPTextBoxText("");

            // Write data into hidden fields.
            _displayControl.SetRequestHeadersTextBoxText(this.session.oRequest.headers.ToString());
            _displayControl.SetRequestBodyTextBoxText(this.session.GetRequestBodyAsString());
            _displayControl.SetResponseHeadersTextBoxText(this.session.oResponse.headers.ToString());
            _displayControl.SetResponseBodyTextBoxText(this.session.GetResponseBodyAsString());

            // Write data into Exchange Type and session ID.
            _displayControl.SetExchangeTypeTextBoxText(this.session["X-ExchangeType"]);
            _displayControl.SetSessionIDTextBoxText(this.session.id.ToString());

            // Write HTTP Status Code Text box, convert int to string.
            _displayControl.SetHTTPResponseCodeTextBoxText(this.session.responseCode.ToString());

            /// <remarks>
            /// Client Begin and done response. -- Overall elapsed time.
            /// </remarks>

            if (this.session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff") == "0:00:00.000" || this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") == "0:00:00.000")
            {
                _displayControl.SetClientRequestBeginDateTextBox("No Data");
                _displayControl.SetClientRequestBeginTimeTextBox("No Data");

                _displayControl.SetClientRequestEndDateTextBox("No Data");
                _displayControl.SetClientRequestEndTimeTextBox("No Data");

                _displayControl.SetOverallElapsedTextbox("No Data");

            }
            else
            {
                _displayControl.SetClientRequestBeginDateTextBox(this.session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd"));
                _displayControl.SetClientRequestBeginTimeTextBox(this.session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff"));

                _displayControl.SetClientRequestEndDateTextBox(this.session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd"));
                _displayControl.SetClientRequestEndTimeTextBox(this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff"));

                double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

                _displayControl.SetOverallElapsedTextbox(ClientMilliseconds + "ms");

                /// <remarks>
                /// Notify on slow running session with threshold pulled from Preferences.cs.
                /// </remarks>
                /// 
                int SlowRunningSessionThreshold = calledPreferences.GetSlowRunningSessionThreshold();

                if (ClientMilliseconds > SlowRunningSessionThreshold)
                {
                    _displayControl.SetResponseAlertTextBox("Long running session!");
                    _displayControl.SetResponseCommentsRichTextboxText("Found a long running session." +
                        Environment.NewLine +
                        Environment.NewLine +
                        "What is Server Think Time? The time the server spent processing the request. (ServerBeginResponse - ServerGotRequest)." +
                        Environment.NewLine +
                        "The rest of the time is the time spent sending the response back to the client application which made the request." +
                        Environment.NewLine +
                        Environment.NewLine +
                        "ClientBeginRequest == Fiddler is aware of when the traffic is initially passed to it as a proxy server." +
                        Environment.NewLine +
                        "ClientDoneRequest == Fiddler is aware of when it has finished sending the server response back to the application which made the request." +
                        Environment.NewLine +
                        "ServerGotRequest == Fiddler is aware of when the server received the request." +
                        Environment.NewLine +
                        "ServerBeginResponse == Fiddler is aware of when the server started to send the response." +
                        Environment.NewLine +
                        "ServerDoneResponse == Fiddler is aware of when it was was able to complete sending the server response back to the application which made the request.");

                    if (bAppLoggingEnabled && bExtensionEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running session.");
                    }
                }
                
            }

            /// <remarks>
            /// Server Got and Done Response. -- Server Think Time.
            /// </remarks>
            /// 
            if (this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff") == "0:00:00.000" ||
                this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff") == "0:00:00.000" ||
                this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") == "0:00:00.000")
            {
                // No data on the session to write or calculate on.
                _displayControl.SetServerGotRequestDateTextbox("No Data");
                _displayControl.SetServerGotRequestTimeTextbox("No Data");

                _displayControl.SetServerBeginResponseDateTextbox("No Data");
                _displayControl.SetServerBeginResponseTimeTextbox("No Data");

                _displayControl.SetServerDoneResponseDateTextbox("No Data");
                _displayControl.SetServerDoneResponseTimeTextbox("No Data");

                _displayControl.SetServerThinkTimeTextbox("No Data");

                _displayControl.SetTransmitTimeTextbox("No Data");
            }
            else
            {
                // Write Server data into textboxes.
                _displayControl.SetServerGotRequestDateTextbox(this.session.Timers.ServerGotRequest.ToString("yyyy/MM/dd"));
                _displayControl.SetServerGotRequestTimeTextbox(this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff"));

                _displayControl.SetServerBeginResponseDateTextbox(this.session.Timers.ServerBeginResponse.ToString("yyyy/MM/dd"));
                _displayControl.SetServerBeginResponseTimeTextbox(this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff"));

                _displayControl.SetServerDoneResponseDateTextbox(this.session.Timers.ServerDoneResponse.ToString("yyyy/MM/dd"));
                _displayControl.SetServerDoneResponseTimeTextbox(this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff"));

                double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

                _displayControl.SetServerThinkTimeTextbox(ServerMilliseconds + "ms");

                _displayControl.SetTransmitTimeTextbox(Math.Round((this.session.Timers.ServerDoneResponse - this.session.Timers.ServerBeginResponse).TotalMilliseconds) + "ms");

                /// <remarks>
                /// Notify on slow running session with threshold pulled from Preferences.cs.
                /// </remarks>
                /// 
                int SlowRunningSessionThreshold = calledPreferences.GetSlowRunningSessionThreshold();

                if (ServerMilliseconds > SlowRunningSessionThreshold)
                {
                    _displayControl.SetResponseAlertTextBox("Long running EXO session!");
                    _displayControl.SetResponseCommentsRichTextboxText("Found a long running EXO session (> 5 seconds)." + Environment.NewLine +
                        Environment.NewLine +
                        "What is Server Think Time? The time the server spent processing the request. (ServerBeginResponse - ServerGotRequest)." +
                        Environment.NewLine +
                        "The rest of the time is the time spent sending the response back to the client application which made the request." +
                        Environment.NewLine +
                        Environment.NewLine +
                        "ClientBeginRequest == Fiddler is aware of when the traffic is initially passed to it as a proxy server." +
                        Environment.NewLine +
                        "ClientDoneRequest == Fiddler is aware of when it has finished sending the server response back to the application which made the request." +
                        Environment.NewLine +
                        "ServerGotRequest == Fiddler is aware of when the server received the request." +
                        Environment.NewLine +
                        "ServerBeginResponse == Fiddler is aware of when the server started to send the response." +
                        Environment.NewLine +
                        "ServerDoneResponse == Fiddler is aware of when it was was able to complete sending the server response back to the application which made the request.");
                    if (bAppLoggingEnabled && bExtensionEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running EXO session.");
                    }
                }
            }

            _displayControl.SetXHostIPTextBoxText(this.session["X-HostIP"]);

            // If the response server header is not null or blank then populate it into the response server value.
            if (this.session.isTunnel == true)
            {
                _displayControl.SetResponseServerTextBoxText(this.session.url);
            }
            if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != ""))
            {
                _displayControl.SetResponseServerTextBoxText(this.session.oResponse["Server"]);
            }
            // Else if the reponnse Host header is not null or blank then populate it into the response server value
            // Some traffic identifies a host rather than a response server.
            else if ((this.session.oResponse["Host"] != null && (this.session.oResponse["Host"] != "")))
            {
                _displayControl.SetResponseServerTextBoxText("Host: " + this.session.oResponse["Host"]);
            }
            // Else if the response PoweredBy header is not null or blank then populate it into the response server value.
            // Some Office 365 servers respond as X-Powered-By ASP.NET.
            else if ((this.session.oResponse["X-Powered-By"] != null) && (this.session.oResponse["X-Powered-By"] != ""))
            {
                _displayControl.SetResponseServerTextBoxText("X-Powered-By: " + this.session.oResponse["X-Powered-By"]);
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Served-By"] != null && (this.session.oResponse["X-Served-By"] != "")))
            {
                _displayControl.SetResponseServerTextBoxText("X-Served-By: " + this.session.oResponse["X-Served-By"]);
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Server-Name"] != null && (this.session.oResponse["X-Server-Name"] != "")))
            {
                _displayControl.SetResponseServerTextBoxText("X-Server-Name: " + this.session.oResponse["X-Server-Name"]);
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
            int wordCountError = 0;
            int wordCountFailed = 0;
            int wordCountException = 0;

            #region SessionRuleSet

            /////////////////////////////
            //
            //  Broader code logic for sessions, where we do not want to use the response code as in the switch statement.
            //
            
            /////////////////////////////
            //
            // From a scenario where an Apache Web Server (Unix/Linux) found to be answering Autodiscover calls and throwing HTTP 301 & 405 responses.
            // When this happens unexpected XML data can be passed to Outlook causing credential prompts which cannot be satisfied with username/password.
            //
            if ((this.session.url.Contains("autodiscover") && (this.session.oResponse["server"] == "Apache")))
            {
                _displayControl.SetResponseAlertTextBox("Apache is answering Autodiscover requests!");
                _displayControl.SetResponseCommentsRichTextboxText("An Apache Web Server(Unix/Linux) is answering Autodiscover requests!" + Environment.NewLine +
                    "This should not be happening. Consider disabling Root Domain Autodiscover lookups." + Environment.NewLine +
                    "See ExcludeHttpsRootDomain on https://support.microsoft.com/en-us/help/2212902/unexpected-autodiscover-behavior-when-you-have-registry-settings-under" + Environment.NewLine +
                    "Beyond this, the customer needs their web administrator responsible for the server answering the calls to stop the Apache web server from answering to Autodiscover.");
                if (bAppLoggingEnabled && bExtensionEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 405 Method Not Allowed; Apache is answering Autodiscover requests!");
                }
            }
            //
            /////////////////////////////
            // 
            // If the above is not true, then drop into the switch statement based on individual response codes.
            //
            else
            {
                /////////////////////////////
                //
                // Response code logic.
                //
                switch (this.session.responseCode)
                {
                    #region HTTP0
                    case 0:

                        /////////////////////////////
                        //
                        //  HTTP 0: No Response.
                        //

                        // Thinking a check on this.session["X-ResponseCode"] is needed to eliminate false positives here.
                        _displayControl.SetResponseAlertTextBox("!HTTP 0 No Response!");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTPQuantity);
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region HTTP200s
                    case 200:

                        // Set HTTP200SkipLogic to zero for each session we work through in the trace.
                        int HTTP200SkipLogic = 0;

                        /////////////////////////////
                        //
                        // HTTP 200
                        //

                        /////////////////////////////
                        // 1. Connect Tunnel.
                        if (this.session.isTunnel == true)
                        {
                            _displayControl.SetResponseAlertTextBox("Connect Tunnel");
                            _displayControl.SetResponseCommentsRichTextboxText("Encrypted HTTPS traffic flows through this CONNECT tunnel. " +
                                "HTTPS Decryption is enabled in Fiddler, so decrypted sessions running in this tunnel will be shown in the Web Sessions list.");
                            // No reason currently known to check the response body on tunnel sessions. Compute saving.
                            HTTP200SkipLogic++;

                            // Write the value of HTTP200SkipLogic into debug output.
                            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: IsTunnel - HTT200SkipLogic {HTTP200SkipLogic.ToString()}");
                        }

                        /////////////////////////////
                        // 2. Exchange On-Premise Autodiscover redirect.
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
                            
                            // If demo mode is running then substitute in this static redirect address.
                            // Otherwise run as normal based on code above to set RedirectAddress string.
                            if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.DemoMode", false) == true)
                            {
                                // If as well as being in demo mode, demo mode break scenarios is enabled. Show fault through incorrect direct
                                // address for an Exchange Online mailbox.
                                if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.DemoModeBreakScenarios", false) == true)
                                {
                                    RedirectAddress = "user@contoso.com";
                                }
                                else
                                {
                                    RedirectAddress = "user@contoso.mail.onmicrosoft.com";
                                }
                            }
                            else
                            {
                                // If demo mode is not running, set RedirectAddress detected from the session.
                                RedirectAddress = RedirectResponseBody.Substring(start, charcount).Replace("<RedirectAddr>", "");
                            }

                            if (RedirectAddress.Contains(".onmicrosoft.com"))
                            {
                                _displayControl.SetResponseAlertTextBox("Exchange On-Premise Autodiscover redirect.");
                                _displayControl.SetResponseCommentsRichTextboxText("Exchange On-Premise Autodiscover redirect address to Exchange Online found." + 
                                    Environment.NewLine + 
                                    Environment.NewLine +
                                    "RedirectAddress: " + RedirectAddress + 
                                    Environment.NewLine +
                                    Environment.NewLine + 
                                    "This is what we want to see, the mail.onmicrosoft.com redirect address (you may know this as the target address or remote routing address) from On-Premise sends Outlook to Office 365.");
                                // Increment HTTP200SkipLogic so that 99 does not run below.
                                HTTP200SkipLogic++;
                            }
                            // Highlight if we got this far and do not have a redirect address which points to
                            // Exchange Online such as: contoso.mail.onmicrosoft.com.
                            else
                            {
                                _displayControl.SetResponseAlertTextBox("!Exchange On-Premise Autodiscover redirect!");
                                _displayControl.SetResponseCommentsRichTextboxText("Exchange On-Premise Autodiscover redirect address found, which does not contain .onmicrosoft.com." + 
                                    Environment.NewLine + 
                                    Environment.NewLine +
                                    "RedirectAddress: " + RedirectAddress + 
                                    Environment.NewLine + 
                                    Environment.NewLine + 
                                    "If this is an Office 365 mailbox the targetAddress from On-Premise is not sending Outlook to Office 365!");
                                // Increment HTTP200SkipLogic so that 99 does not run below.
                                HTTP200SkipLogic++;
                                if (bAppLoggingEnabled && bExtensionEnabled)
                                {
                                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 On-Prem Autodiscover redirect - Address doesn't contain .onmicrosoft.com.");
                                }
                            }

                            // Write the value of HTTP200SkipLogic into debug output.
                            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: Exchange On-Premise Autodiscover redirect. - HTT200SkipLogic {HTTP200SkipLogic.ToString()}");
                        }

                        /////////////////////////////
                        //
                        // 3. Exchange On-Premise Autodiscover redirect - address can't be found
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
                            _displayControl.SetResponseAlertTextBox("!Exchange On-Premise Autodiscover redirect: Error Code 500!");
                            _displayControl.SetResponseCommentsRichTextboxText("Exchange On-Premise Autodiscover redirect address can't be found. Look for other On-Premise Autodiscover responses, we may have a " +
                                "valid Autodiscover targetAddress from On-Premise in another session in this trace.");
                            // Increment HTTP200SkipLogic so that 99 does not run below.
                            HTTP200SkipLogic++;
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 On-Prem Autodiscover redirect - Address can't be found.");
                            }

                            // Write the value of HTTP200SkipLogic into debug output.
                            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: HTTP 200 Exchange On-Premise Autodiscover redirect. - HTT200SkipLogic {HTTP200SkipLogic.ToString()}");
                        }

                        /////////////////////////////
                        //
                        // 4. Exchange Online Autodiscover
                        //

                        // Make sure this session if an Exchange Online Autodiscover request.
                        if ((this.session.hostname == "autodiscover-s.outlook.com") && (this.session.uriContains("autodiscover.xml")))
                        {
                            if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) &&
                                (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) &&
                                (this.session.utilFindInResponse("<MailStore>", false) > 1) &&
                                (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                            {
                                _displayControl.SetResponseAlertTextBox("Exchange Online Autodiscover");
                                _displayControl.SetResponseCommentsRichTextboxText("Exchange Online Autodiscover.");
                                // Increment HTTP200SkipLogic so that 99 does not run below.
                                HTTP200SkipLogic++;
                            }
                            // If we got this far and those strings do not exist in the response body something is wrong.
                            else
                            {
                                _displayControl.SetResponseAlertTextBox("!Exchange Online Autodiscover - FAILURE!");
                                _displayControl.SetResponseCommentsRichTextboxText("Exchange Online Autodiscover. FAILURE!");
                                // Don't use skip logic here, we want to dig deeper and see if there are errors, failures, or exceptions.
                                //HTTP200SkipLogic++;
                            }

                            // Write the value of HTTP200SkipLogic into debug output.
                            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: HTTP 200 Exchange Online Autodiscover - HTT200SkipLogic {HTTP200SkipLogic.ToString()}");
                        }

                        /////////////////////////////
                        //
                        // 5. Outlook MAPI traffic.
                        //
                        if (this.session.HostnameIs("outlook.office365.com") && (this.session.uriContains("/mapi/emsdb/?MailboxId=")))
                        {
                            // Cannot think of any good reason to run the keyword search on this session.
                            // If we are interacting with MAPI on the mailbox at this point this is a working scenario as far as connectivity
                            // is concerned.
                            // Increment HTTP200SkipLogic so that 99 does not run below.
                            HTTP200SkipLogic++;

                            // Write the value of HTTP200SkipLogic into debug output.
                            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: HTTP 200 Outlook MAPI traffic. - HTT200SkipLogic {HTTP200SkipLogic.ToString()}");
                        }

                        /////////////////////////////
                        //
                        // 6. GetUnifiedGroupsSettings EWS call.
                        //

                        if (this.session.HostnameIs("outlook.office365.com") && 
                            (this.session.uriContains("ews/exchange.asmx") && 
                            (this.session.utilFindInRequest("GetUnifiedGroupsSettings", false) >1)))
                        {
                            // User can create Office 365 gropus.
                            if (this.session.utilFindInResponse("<GroupCreationEnabled>true</GroupCreationEnabled>", false) > 1)
                            {
                                _displayControl.SetResponseAlertTextBox("GetUnifiedGroupsSettings EWS call.");
                                _displayControl.SetResponseCommentsRichTextboxText("<GroupCreationEnabled>true</GroupCreationEnabled> found in response body. " +
                                    "Expect user to be able to create Office 365 groups in Outlook.");
                                HTTP200SkipLogic++;
                            }
                            // User cannot create Office 365 groups. Not an error condition in and of itself.
                            else if (this.session.utilFindInResponse("<GroupCreationEnabled>false</GroupCreationEnabled>", false) > 1)
                            {
                                _displayControl.SetResponseAlertTextBox("GetUnifiedGroupsSettings EWS call!");
                                _displayControl.SetResponseCommentsRichTextboxText("<GroupCreationEnabled>false</GroupCreationEnabled> found in response body. " +
                                    "Expect user to NOT be able to create Office 365 groups in Outlook.");
                                HTTP200SkipLogic++;
                            }
                            // Did not see the expected keyword in the response body. This is the error condition.
                            else
                            {
                                _displayControl.SetResponseAlertTextBox("!GetUnifiedGroupsSettings EWS call!");
                                _displayControl.SetResponseCommentsRichTextboxText("Though GetUnifiedGroupsSettings scenario was detected neither <GroupCreationEnabled>true</GroupCreationEnabled> or" +
                                    "<GroupCreationEnabled>false</GroupCreationEnabled> was found in the response body. Check the Raw tab for more details.");
                            }

                            // Write the value of HTTP200SkipLogic into debug output.
                            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: HTTP 200 GetUnifiedGroupsSettings EWS call. - HTT200SkipLogic {HTTP200SkipLogic.ToString()}");

                        }


                        /////////////////////////////
                        //
                        // 99. All other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.
                        else
                        {
                            // Write the value of HTTP200SkipLogic into debug output.
                            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: HTTP 200 99 Skip Logic. - HTT200SkipLogic {HTTP200SkipLogic.ToString()}");

                            // If the HTTP200SkipLogic value is zero, then none of the above logic has run on the session.
                            // Treat this session as a HTTP 200 we need to check for error / failures / exceptions on.
                            if (HTTP200SkipLogic == 0)
                            {

                                string wordCountErrorText;
                                string wordCountFailedText;
                                string wordCountExceptionText;

                                // Only want to start splitting word in responses only sessions we need to.
                                // Specifically HTTP 200's with the appropriate content type.
                                if ((this.session.ResponseHeaders.ExistsAndContains("Content-Type", "text") ||
                                    (this.session.ResponseHeaders.ExistsAndContains("Content-Type", "html") ||
                                    (this.session.ResponseHeaders.ExistsAndContains("Content-Type", "xml")))))
                                {

                                    // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
                                    //
                                    // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
                                    //

                                    string text200 = this.session.ToString();

                                    //Convert the string into an array of words  
                                    string[] source200 = text200.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

                                    // Create the query. Use ToLowerInvariant to match "data" and "Data"   
                                    var matchQuery200 = from word in source200
                                                        where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                                        select word;

                                    searchTerm = "error";

                                    // Count the matches, which executes the query.  
                                    wordCountError = matchQuery200.Count();

                                    searchTerm = "failed";

                                    // Count the matches, which executes the query.  
                                    wordCountFailed = matchQuery200.Count();

                                    searchTerm = "exception";

                                    // Count the matches, which executes the query.  
                                    wordCountException = matchQuery200.Count();


                                    // If either the keyword searches give us a result.
                                    if (wordCountError > 0 || wordCountFailed > 0 || wordCountException > 0)
                                    {

                                        if (wordCountError == 1)
                                        {
                                            wordCountErrorText = wordCountError + " time.";
                                        }
                                        else
                                        {
                                            wordCountErrorText = wordCountError + " times.";
                                        }

                                        if (wordCountFailed == 1)
                                        {
                                            wordCountFailedText = wordCountFailed + " time.";
                                        }
                                        else
                                        {
                                            wordCountFailedText = wordCountFailed + " times.";
                                        }

                                        if (wordCountException == 1)
                                        {
                                            wordCountExceptionText = wordCountException + " time.";
                                        }
                                        else
                                        {
                                            wordCountExceptionText = wordCountException + " times.";
                                        }

                                        _displayControl.SetResponseAlertTextBox("!'error', 'failed' or 'exception' found in respone body!");
                                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 200: Errors or failures found in response body. " + 
                                            "Check the Raw tab, click 'View in Notepad' button bottom right, and search for error in the response to review." +
                                            Environment.NewLine + Environment.NewLine +
                                            "After splitting all words in the response body the following were found:" + Environment.NewLine +
                                            Environment.NewLine + "Keyword 'Error' found " + wordCountErrorText +
                                            Environment.NewLine + "Keyword 'Failed' found " + wordCountFailedText +
                                            Environment.NewLine + "Keyword 'Exception' found " + wordCountExceptionText);
                                        if (bAppLoggingEnabled && bExtensionEnabled)
                                        {
                                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 keyword 'error', 'failed' or 'exception' found in respone body!");
                                        }
                                    }
                                    // both word count variables are zero.
                                    else
                                    {
                                        _displayControl.SetResponseAlertTextBox("No failures keywords detected in respone body.");
                                        _displayControl.SetResponseCommentsRichTextboxText("No failures keywords ('error', 'failed' or 'exception') detected in respone body.");
                                    }
                                }
                            }
                            // HTTP200SkipLogic is >= 1.
                            else
                            {
                                // Do nothing here right now.
                            }
                        }
                        //
                        /////////////////////////////
                        break;
                    case 201:
                        /////////////////////////////
                        //
                        //  HTTP 201: Created.
                        //
                        _displayControl.SetResponseAlertTextBox("HTTP 201 Created.");
                        _displayControl.SetResponseCommentsRichTextboxText("Not expecting this to be anything which needs attention for troubleshooting.");
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

                    #region HTTP300s
                    case 301:
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
                            _displayControl.SetResponseAlertTextBox("!HTTP 307 Temporary Redirect!");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 307: Temporary Redirects have been seen to redirect Exchange Online Autodiscover " +
                                "calls back to On-Premise resources, breaking Outlook connectivity." + Environment.NewLine +
                                "This session has enough data points to be an Autodiscover request for Exchange Online which has not been sent to " +
                                "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml as expected." + Environment.NewLine +
                                "Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place.");

                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 307 On-Prem Temp Redirect - Unexpected location!");
                            }

                        }
                        else
                        {
                            _displayControl.SetResponseAlertTextBox("HTTP 307 Temporary Redirect");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 307: Temporary Redirects have been seen to redirect Exchange Online Autodiscover calls " +
                                "back to On-Premise resources, breaking Outlook connectivity. " + Environment.NewLine + "Check the Headers or Raw tab and the Location to ensure the Autodiscover call is " +
                                "going to the correct place. " + Environment.NewLine + "If this session is not for an Outlook process then the information above may not be relevant to the issue under investigation.");
                        }
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region HTTP400s
                    case 401:
                        /////////////////////////////
                        //
                        //  HTTP 401: UNAUTHORIZED.
                        //
                        _displayControl.SetResponseAlertTextBox("HTTP 401 Unauthorized");
                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 401: Unauthorized / Authentication Challenge. These are expected and are not an issue as long as a subsequent " +
                            "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. " + 
                            Environment.NewLine + 
                            Environment.NewLine +
                            "If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.");
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
                            _displayControl.SetResponseAlertTextBox("!HTTP 403 Access Denied!");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 403: Forbidden. Is your firewall or web proxy blocking Outlook connectivity?" + Environment.NewLine +
                                "To fire this message a HTTP 403 response code was detected and 'Access Denied' was found in the response body." + Environment.NewLine +
                                "Check the Raw and WebView tabs, do you see anything which indicates traffic is blocked?");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");
                            }
                        }
                        else
                        {
                            // Pick up any 403 Forbidden and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("!HTTP 403 Forbidden!");
                            _displayControl.SetResponseCommentsRichTextboxText("While HTTP 403's can be symptomatic of a proxy server blocking traffic, " +
                                "however the phrase 'Access Denied' was NOT detected in the response body." + 
                                Environment.NewLine + 
                                Environment.NewLine +
                                "A small number of HTTP 403's can be seen in normal working scenarios. Check the Raw and WebView tabs to look for anything which looks suspect." + 
                                Environment.NewLine + 
                                Environment.NewLine + 
                                "If you are troubleshooting Free/Busy (Meeting availability info) or setting Out of Office messages then you may be more interested in these." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "See: https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd877045(v=exchg.140)");
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
                        _displayControl.SetResponseAlertTextBox("!HTTP 404 Not Found!");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTPQuantity);
                        //
                        /////////////////////////////
                        break;
                    case 405:
                        /////////////////////////////
                        //
                        //  HTTP 405: Method Not Allowed.
                        //
                        _displayControl.SetResponseAlertTextBox("!HTTP 405: Method Not Allowed!");
                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 405: Method Not Allowed");
                        //
                        /////////////////////////////
                        break;
                    case 429:
                        /////////////////////////////
                        //
                        //  HTTP 429: Too Many Requests.
                        //
                        _displayControl.SetResponseAlertTextBox("!HTTP 429 Too Many Requests!");
                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 429: These responses need to be taken into context with the rest of the sessions in the trace. " + 
                            "A small number is probably not an issue, larger numbers of these could be cause for concern.");
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
                    case 456:
                        /////////////////////////////
                        //
                        // HTTP 456: Multi-Factor Authentication. Complications seen when MFA is enabled on accounts, but Modern Authentication
                        // is not enabled in Exchange Online.
                        //
                        /////////////////////////////
                        if (this.session.utilFindInResponse("you must use multi-factor authentication", false) > 1)
                        {
                            _displayControl.SetResponseAlertTextBox("HTTP 456 Multi-Factor Authentication");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 429: See details on Raw tab. Look for the presence of 'you must use multi-factor authentication'." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "This has been seen where users have MFA enabled/enforced, but Modern Authentication is not enabled in Exchange Online" +
                                Environment.NewLine +
                                Environment.NewLine +
                                "See https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662"
                                );
                        }
                        else
                        {
                            _displayControl.SetResponseAlertTextBox("!HTTP 456 Multi-Factor Authentication!");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 429: See details on Raw tab.");
                        }
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region HTTP500s
                    case 500:
                        /////////////////////////////
                        //
                        //  HTTP 500: Internal Server Error.
                        //
                        // Pick up any 500 Internal Server Error and write data into the comments box.
                        // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in green? >
                        // Pick up any 500 Internal Server Error and write data into the comments box.
                        _displayControl.SetResponseAlertTextBox("!HTTP 500 Internal Server Error!");
                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 500 Internal Server Error");
                        if (bAppLoggingEnabled && bExtensionEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 500 Internal Server Error.");
                        }
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
                            _displayControl.SetResponseAlertTextBox("False Positive");
                            _displayControl.SetResponseCommentsRichTextboxText("Unlikely the cause of Outlook / OWA connectivity.");
                        }

                        /////////////////////////////
                        //
                        // 2. Exchange Online DNS lookup on contoso.onmicrosoft.com, False Positive!
                        //
                        // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
                        else if ((this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                            (this.session.utilFindInResponse("DNS Lookup for ", false) > 1) &&
                            (this.session.utilFindInResponse(" failed.", false) > 1))
                        {
                            _displayControl.SetResponseAlertTextBox("False Positive");
                            _displayControl.SetResponseCommentsRichTextboxText("From the data in the response body this failure is likely due to a Microsoft DNS MX record " + Environment.NewLine +
                                "which points to an Exchange Online Protection mail host that accepts connections only on port 25. Connection on port 443 will not work by design." + Environment.NewLine +
                                Environment.NewLine + Environment.NewLine + "To validate this above lookup the record, confirm it is a MX record and attempt to connect to the MX host on ports 25 and 443.");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. EXO DNS lookup false positive.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 3. Exchange Online connection to autodiscover.contoso.mail.onmicrosoft.com, False Positive!
                        //
                        // Specific scenario on Outlook and Office 365 invalid connection to contoso.mail.onmicrosoft.com
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
                        else if ((this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                            // Too specific, it looks as though we see ConnectionRefused or The socket connection to ... failed.
                            //(this.session.utilFindInResponse("ConnectionRefused ", false) > 1) &&
                            (this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                            (this.session.utilFindInResponse(":443", false) > 1))
                        {

                            string AutoDFalsePositiveResponseBody = this.session.GetResponseBodyAsString();
                            int start = this.session.GetResponseBodyAsString().IndexOf("'");
                            int end = this.session.GetResponseBodyAsString().LastIndexOf("'");
                            int charcount = end - start;
                            string AutoDFalsePositiveDomain = AutoDFalsePositiveResponseBody.Substring(start, charcount).Replace("'", "");
                            //MessageBox.Show("Test: " + AutoDFalsePositiveDomain);

                            _displayControl.SetResponseAlertTextBox("False Positive");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 502: False Positive. By design Office 365 Autodiscover does not respond to " +
                                AutoDFalsePositiveDomain + " on port 443. " + Environment.NewLine + Environment.NewLine +
                                "Validate this message by confirming this is an Office 365 Host/IP address and perform a telnet to it on port 80." +
                                Environment.NewLine + Environment.NewLine +
                                "If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design redirects requests to https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml.");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. O365 Autodiscover false positive.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 4. Vanity domain points to Office 365 autodiscover; false positive.
                        //

                        /*
                        HTTP/1.1 502 Fiddler - Connection Failed
                        Date: Mon, 12 Nov 2018 09:47:06 GMT
                        Content-Type: text/html; charset=UTF-8
                        Connection: close
                        Cache-Control: no-cache, must-revalidate
                        Timestamp: 04:47:06.295

                        [Fiddler] The connection to 'autodiscover.contoso.com' failed. <br />Error: ConnectionRefused (0x274d). <br />
                        System.Net.Sockets.SocketException No connection could be made because the target machine actively refused it 40.97.100.8:443                                                                                                                                                                                                                                                                                  
                        */

                        if ((this.session.utilFindInResponse("autodiscover.", false) > 1) &&
                                (this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                                (this.session.utilFindInResponse("40.97.", false) > 1))
                        {
                            _displayControl.SetResponseAlertTextBox("Office 365 Autodiscover False Positive");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 502: False Positive. By design Office 365 certain IP addresses used for " +
                                "Autodiscover do not respond on port 443. " +
                                Environment.NewLine +
                                Environment.NewLine +
                                "Validate this message by confirming this is an Office 365 Host/IP address and perform a telnet to it on port 80." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design " +
                                "redirects requests to https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml.");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. Vanity domain; O365 Autodiscover false positive.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 5. Anything else Exchange Autodiscover.
                        //
                        else if ((this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                            (this.session.utilFindInResponse("autodiscover", false) > 1) &&
                            (this.session.utilFindInResponse(":443", false) > 1))
                        {
                            _displayControl.SetResponseAlertTextBox("!AUTODISCOVER!");
                            _displayControl.SetResponseCommentsRichTextboxText("Autodiscover request detected, which failed.");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. Autodiscover failure.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 99. Everything else.
                        //
                        else
                        {
                            // Pick up any other 502 Bad Gateway and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("!HTTP 502 Bad Gateway!");
                            _displayControl.SetResponseCommentsRichTextboxText("Potential to cause the issue you are investigating. " +
                                "Do you see expected responses beyond this session in the trace? Is this an Exchange On - Premise, Exchange Online or other device ?");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway.");
                            }
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

                        // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
                        //
                        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
                        //

                        string text503 = this.session.ToString();

                        //Convert the string into an array of words  
                        string[] source503 = text503.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // Create the query. Use ToLowerInvariant to match "data" and "Data"   
                        var matchQuery503 = from word in source503
                                         where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                         select word;

                        // Count the matches, which executes the query.  
                        wordCount = matchQuery503.Count();
                        if (wordCount > 0)
                        {
                            //XAnchorMailbox = this.session.oRequest["X-AnchorMailbox"];
                            RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";
                            if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.DemoMode", false) == true)
                            {
                                RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=user@contoso.com&xml=1";
                            }

                            _displayControl.SetResponseAlertTextBox("!FederatedSTSUnreachable!");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 503: FederatedSTSUnreachable." + Environment.NewLine +
                                "The fedeation service is unreachable or unavailable. Check the Raw tab for additional details." + Environment.NewLine +
                                "Check the realm page for the authenticating domain." + Environment.NewLine + RealmURL + Environment.NewLine + Environment.NewLine +
                                "Expected responses:" + Environment.NewLine +
                                "AuthURL: Normally expected to show federation service logon page." + Environment.NewLine +
                                "STSAuthURL: Normally expected to show HTTP 400." + Environment.NewLine +
                                "MEXURL: Normally expected to show long stream of XML data." + Environment.NewLine + Environment.NewLine +
                                "If any of these show the HTTP 503 Service Unavailable this confirms a consistent failure on the federation service." + Environment.NewLine +
                                "If however you get the expected responses, this does not neccessarily mean the federation service / everything authentication is healthy. Further investigation is advised.");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 503 Service Unavailable. Found keyword 'FederatedStsUnreachable' in response body!");
                            }
                        }
                        /////////////////////////////
                        //
                        // 99. Everything else.
                        //
                        else
                        {
                            // Pick up any other 503 Service Unavailable and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("!HTTP 503 Service Unavailable!");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 503 Service Unavailable.");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 503 Service Unavailable.");
                            }
                        }
                        //
                        /////////////////////////////
                        break;
                    case 504:
                        /////////////////////////////
                        //
                        //  HTTP 504: GATEWAY TIMEOUT.
                        //

                        /////////////////////////////
                        // 1. HTTP 504 Bad Gateway 'internet has been blocked'
                        if ((this.session.utilFindInResponse("access", false) > 1) &&
                            (this.session.utilFindInResponse("internet", false) > 1) &&
                            (this.session.utilFindInResponse("blocked", false) > 1) &&
                            bExtensionEnabled)
                        {
                            _displayControl.SetResponseAlertTextBox("!HTTP 504 Gateway Timeout -- Internet Access Blocked!");
                            _displayControl.SetResponseCommentsRichTextboxText("Detected the keywords 'internet' and 'access' and 'blocked'. Potentially the computer this trace was collected " +
                                "from has been quaratined for internet access on the customer's network." + Environment.NewLine + Environment.NewLine +
                                "Validate this by checking the webview and raw tabs for more information.");
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 504 Gateway Timeout -- Internet Access Blocked.");
                            }
                        }

                        /////////////////////////////
                        // 99. Pick up any other 504 Gateway Timeout and write data into the comments box.
                        else if (bAppLoggingEnabled && bExtensionEnabled)
                        {
                            _displayControl.SetResponseAlertTextBox("!HTTP 504 Gateway Timeout!");
                            _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTPQuantity);
                            if (bAppLoggingEnabled && bExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 504 Gateway Timeout.");
                            }
                                
                        }
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region Default
                    default:
                        _displayControl.SetResponseAlertTextBox("Undefined.");
                        _displayControl.SetResponseCommentsRichTextboxText("No specific information on this session in the EXO Fiddler Extension.");
                        break;
                    #endregion
                }
                //
                /////////////////////////////
            }
            //
            /////////////////////////////
            #endregion
        }
        //
        /////////////////////////////

        public void SaveSessionData(Session oS)
        {
            this.session = oS;

            RequestHeaders = this.session.RequestHeaders;
            ResponseHeaders = this.session.ResponseHeaders;

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
        //
        /////////////////////////////

        /////////////////////////////
        // Not sure what to do with this.
        void IBaseInspector2.Clear()
        {
            throw new System.NotImplementedException();
        }
        //
        /////////////////////////////
    }
}
