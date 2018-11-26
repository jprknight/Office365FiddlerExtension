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
        public bool bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled", false);
        public bool bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public bool bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public bool bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
        public bool bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public bool bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);

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
            //throw new System.NotImplementedException();
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
    /*
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
    */
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

        private string RedirectAddress;

        #region ScoreForSession
        // Double click or hit return with session selected.
        // From discussion with EE Fiddler code known to be problematic with the ScoreForSession feature.
        // Not expected to work 100% of the time per logic below.
        public override int ScoreForSession(Session oS)
        {
            if (bExtensionEnabled)
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
            // Not all path return a value. Well throw a zero.
            return 0;
        }
        #endregion

        public HTTPResponseHeaders headers
        {
            get { return responseHeaders; }
            set { responseHeaders = value; }
        }

        // Function which starts everything.
        public byte[] body
        {
            get { return rawBody; }
            set
            {
                if (bExtensionEnabled)
                {
                    SetResponseValues(this.session);
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

                    if (bAppLoggingEnabled)
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
                    if (bAppLoggingEnabled)
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

            // Session rule set used to live here.
            // Now all the logic is ran in SessionRuleSet.cs.
            // Data for these two textboxes on the inspector tab is now written into session tags.
            _displayControl.SetResponseAlertTextBox(this.session["X-ResponseAlertTextBox"]);
            _displayControl.SetResponseCommentsRichTextboxText(this.session["X-ResponseCommentsRichTextboxText"]);
            
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
