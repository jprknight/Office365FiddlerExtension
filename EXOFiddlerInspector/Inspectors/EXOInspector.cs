using System.Windows.Forms;
using System.Linq;
using System.IO;
using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EXOFiddlerInspector.Services;

namespace EXOFiddlerInspector.Inspectors
{
    /// <summary>
    /// Base class, generic inspector, common between request and response
     /// </summary>
    public abstract class EXOInspector : Inspector2
    {
        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.enabled", false);
        public Boolean bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled", false);
        public Boolean bHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.HostIPColumnEnabled", false);
        public Boolean bAuthColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AuthColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", 0);

        public Boolean Developer;

        /// <summary>
        /// Gets or sets the control collection where displayed the MAPI parsed message and corresponding hex data.
        /// </summary>
        public EXOResponseControl EXOResponseControl { get; set; }
        
        /// <summary>
        /// Gets or sets the Session object to pull frame data from Fiddler.
        /// </summary>
        internal Session session { get; set; }

        /// <summary>
        /// Gets or sets the base HTTP headers assigned by the request or response
        /// </summary>
        public HTTPHeaders BaseHeaders { get; set; }

        /// <summary>
        /// Gets or sets the body byte[], called by Fiddler with session byte[]
        /// </summary>
        public byte[] body
        {
            get
            {
                return this.rawBody;
            }

            set
            {
                this.rawBody = value;
                this.ParseSession(this.session);
            }
        }
        
        /// <summary>
        /// Gets or sets the raw bytes from the frame
        /// </summary>
        private byte[] rawBody { get; set; }

        /// <summary>
        /// Method that returns a sorting hint
        /// </summary>
        /// <returns>An integer indicating where we should order ourselves</returns>
        public override int GetOrder()
        {
            return 0;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame has been changed.
        /// </summary>
        public bool bDirty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame is read-only.
        /// </summary>
        public bool bReadOnly { get; set; }

        /// <summary>
        /// Called by Fiddler to determine how confident this inspector is that it can
        /// decode the data.  This is only called when the user hits enter or double-
        /// clicks a session.  
        /// If we score the highest out of the other inspectors, Fiddler will open this
        /// inspector's tab and then call AssignSession.
        /// </summary>
        /// <param name="oS">the session object passed by Fiddler</param>
        /// <returns>Int between 0-100 with 100 being the most confident</returns>
        public override int ScoreForSession(Session oS)
        {
            if (null == this.session)
            {
                this.session = oS;
            }

            if (null == this.BaseHeaders)
            {
                if (this is IRequestInspector2)
                {
                    this.BaseHeaders = this.session.oRequest.headers;
                }
                else
                {
                    this.BaseHeaders = this.session.oResponse.headers;
                }
            }

            if (this.IsEXOhttp)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Gets a value indicating whether the message is MAPI protocol message.
        /// </summary>
        public bool IsEXOhttp
        {
            get
            {
                if (this.session != null && (this is IResponseInspector2))
                {
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Called by Fiddler to add the MAPI inspector tab
        /// </summary>
        /// <param name="o">The tab control for the inspector</param>
        public override void AddToTab(TabPage o)
        {
            this.EXOResponseControl = new EXOResponseControl();
            o.Text = "Exchange Online";
            o.ToolTipText = "Exchange Online Inspector";
            this.EXOResponseControl.Size = o.Size;
            o.Controls.Add(EXOResponseControl);
            o.Controls[0].Dock = DockStyle.Fill;

        }

        public void SaveSessionData(Session oS)
        {
            this.session = oS;

            //RequestHeaders = this.session.RequestHeaders;
            //ResponseHeaders = this.session.ResponseHeaders;

        }


        /// <summary>
        /// This is called every time this inspector is shown
        /// </summary>
        /// <param name="oS">Session object passed by Fiddler</param>
        public override void AssignSession(Session oS)
        {
            this.session = oS;
            base.AssignSession(oS);
        }

        /// <summary>
        /// Update the view with parsed and diagnosed data
        /// </summary>
        private async void ParseSession(Session _session)
        {
            await ParseHTTPResponse(_session);
        }


        /// <summary>
        /// Parse the HTTP payload to FSSHTTP and WOPI message.
        /// </summary>
        /// <param name="responseHeaders">The HTTP response header.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        /// <param name="direction">The direction of the traffic.</param>
        /// <returns>The object parsed result</returns>
        public async Task ParseHTTPResponse(Session _session)
        {
            try
            {
                if (!Preferences.ExtensionEnabled)
                    return;

                this.session = _session;

                this.session.utilDecodeRequest(true);
                this.session.utilDecodeResponse(true);

                this.Clear();

                // Write data into hidden fields.
                EXOResponseControl.RequestHeadersTextbox.Text = this.session.oRequest.headers.ToString();
                EXOResponseControl.RequestBodyTextBox.Text = this.session.GetRequestBodyAsString();
                EXOResponseControl.ResponseHeadersTextBox.Text = this.session.oResponse.headers.ToString();
                EXOResponseControl.ResponseBodyTextBox.Text = this.session.GetResponseBodyAsString();

                // Write data into Exchange Type and session ID.
                EXOResponseControl.ExchangeTypeTextBox.Text = this.session["X-ExchangeType"];
                EXOResponseControl.SessionIDTextBox.Text = this.session.id.ToString();

                // Write HTTP Status Code Text box, convert int to string.
                EXOResponseControl.HTTPResponseCodeTextbox.Text = this.session.responseCode.ToString();

                /// <remarks>
                /// Client Begin and done response. -- Overall elapsed time.
                /// </remarks>

                if (this.session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff") == "0:00:00.000" || this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") == "0:00:00.000")
                {
                    EXOResponseControl.ClientRequestBeginDateTextbox.Text = "No Data";
                    EXOResponseControl.ClientRequestBeginTimeTextbox.Text = "No Data";
                    EXOResponseControl.ClientRequestEndDateTextbox.Text = "No Data";
                    EXOResponseControl.ClientRequestEndTimeTextbox.Text = "No Data";
                    EXOResponseControl.OverallElapsedTextBox.Text = "No Data";

                }
                else
                {
                    EXOResponseControl.ClientRequestBeginDateTextbox.Text = this.session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd");
                    EXOResponseControl.ClientRequestBeginTimeTextbox.Text = this.session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff");

                    EXOResponseControl.ClientRequestEndDateTextbox.Text = this.session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd");
                    EXOResponseControl.ClientRequestEndTimeTextbox.Text = this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff");

                    double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

                    EXOResponseControl.OverallElapsedTextBox.Text = ClientMilliseconds + "ms";

                    /// <remarks>
                    /// Notify on slow running session with threshold pulled from Preferences.cs.
                    /// </remarks>
                    /// 
                    int SlowRunningSessionThreshold = Preferences.GetSlowRunningSessionThreshold();

                    if (ClientMilliseconds > SlowRunningSessionThreshold)
                    {
                        EXOResponseControl.ResponseAlertTextbox.Text = "Long running session!";
                        EXOResponseControl.ResponseCommentsRichTextbox.Text = "Found a long running session." +
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
                            "ServerDoneResponse == Fiddler is aware of when it was was able to complete sending the server response back to the application which made the request.";

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
                    EXOResponseControl.ServerGotRequestDateTextBox.Text = "No Data";
                    EXOResponseControl.ServerGotRequestTimeTextBox.Text = "No Data";
                    EXOResponseControl.ServerBeginResponseDateTextBox.Text = "No Data";
                    EXOResponseControl.ServerBeginResponseTimeTextBox.Text = "No Data";
                    EXOResponseControl.ServerDoneResponseDateTextBox.Text = "No Data";
                    EXOResponseControl.ServerDoneResponseTimeTextBox.Text = "No Data";
                    EXOResponseControl.ServerThinkTimeTextBox.Text = "No Data";
                    EXOResponseControl.TransmitTimeTextBox.Text = "No Data";
                }
                else
                {
                    // Write Server data into textboxes.
                    EXOResponseControl.ServerGotRequestDateTextBox.Text = this.session.Timers.ServerGotRequest.ToString("yyyy/MM/dd");
                    EXOResponseControl.ServerGotRequestTimeTextBox.Text = this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff");
                    EXOResponseControl.ServerBeginResponseDateTextBox.Text = this.session.Timers.ServerBeginResponse.ToString("yyyy/MM/dd");
                    EXOResponseControl.ServerBeginResponseTimeTextBox.Text = this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff");
                    EXOResponseControl.ServerDoneResponseDateTextBox.Text = this.session.Timers.ServerDoneResponse.ToString("yyyy/MM/dd");
                    EXOResponseControl.ServerDoneResponseTimeTextBox.Text = this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff");

                    double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

                    EXOResponseControl.ServerThinkTimeTextBox.Text = ServerMilliseconds + "ms";

                    EXOResponseControl.TransmitTimeTextBox.Text = Math.Round((this.session.Timers.ServerDoneResponse - this.session.Timers.ServerBeginResponse).TotalMilliseconds) + "ms";

                    /// <remarks>
                    /// Notify on slow running session with threshold pulled from Preferences.cs.
                    /// </remarks>
                    /// 
                    int SlowRunningSessionThreshold = Preferences.GetSlowRunningSessionThreshold();

                    if (ServerMilliseconds > SlowRunningSessionThreshold)
                    {
                        EXOResponseControl.ResponseAlertTextbox.Text = "Long running EXO session!";
                        EXOResponseControl.ResponseCommentsRichTextbox.Text = "Found a long running EXO session (> 5 seconds)." + Environment.NewLine +
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
                            "ServerDoneResponse == Fiddler is aware of when it was was able to complete sending the server response back to the application which made the request.";
                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running EXO session.");
                        }
                    }
                }

                EXOResponseControl.XHostIPTextBox.Text = this.session["X-HostIP"];

                // If the response server header is not null or blank then populate it into the response server value.
                if (this.session.isTunnel == true)
                {
                    EXOResponseControl.ResponseServerTexbox.Text = this.session.url;
                }
                if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != ""))
                {
                    EXOResponseControl.ResponseServerTexbox.Text = this.session.oResponse["Server"];
                }
                // Else if the reponnse Host header is not null or blank then populate it into the response server value
                // Some traffic identifies a host rather than a response server.
                else if ((this.session.oResponse["Host"] != null && (this.session.oResponse["Host"] != "")))
                {
                    EXOResponseControl.ResponseServerTexbox.Text = "Host: " + this.session.oResponse["Host"];
                }
                // Else if the response PoweredBy header is not null or blank then populate it into the response server value.
                // Some Office 365 servers respond as X-Powered-By ASP.NET.
                else if ((this.session.oResponse["X-Powered-By"] != null) && (this.session.oResponse["X-Powered-By"] != ""))
                {
                    EXOResponseControl.ResponseServerTexbox.Text = "X-Powered-By: " + this.session.oResponse["X-Powered-By"];
                }
                // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
                else if ((this.session.oResponse["X-Served-By"] != null && (this.session.oResponse["X-Served-By"] != "")))
                {
                    EXOResponseControl.ResponseServerTexbox.Text = "X-Served-By: " + this.session.oResponse["X-Served-By"];
                }
                // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
                else if ((this.session.oResponse["X-Server-Name"] != null && (this.session.oResponse["X-Server-Name"] != "")))
                {
                    EXOResponseControl.ResponseServerTexbox.Text = "X-Server-Name: " + this.session.oResponse["X-Server-Name"];
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

                EXOResponseControl.DataAgeTextbox.Text = DataAgeOutput;

                // Write Process into textbox.
                EXOResponseControl.ResponseProcessTextbox.Text = this.session.LocalProcess;

                // Session rule set used to live here.
                // Now all the logic is ran in SessionRuleSet.cs.
                // Data for these two textboxes on the inspector tab is now written into session tags.
                EXOResponseControl.ResponseProcessTextbox.Text = this.session["X-ResponseAlertTextBox"];
                EXOResponseControl.ResponseCommentsRichTextbox.Text = this.session["X-ResponseCommentsRichTextboxText"];
            }        
            catch (Exception ex)
            {
               // TODO handle exception
            }
        }

     
        /// <summary>
        /// Method Fiddler calls to clear the display
        /// </summary>
        public void Clear()
        {
            EXOResponseControl.ResponseAlertTextbox.Text = string.Empty;
            EXOResponseControl.ResponseCommentsRichTextbox.Text = string.Empty;
            EXOResponseControl.RequestHeadersTextbox.Text = string.Empty;
            EXOResponseControl.RequestBodyTextBox.Text = string.Empty;
            EXOResponseControl.ResponseHeadersTextBox.Text = string.Empty;
            EXOResponseControl.ResponseBodyTextBox.Text = string.Empty;
            EXOResponseControl.ExchangeTypeTextBox.Text = string.Empty;
            EXOResponseControl.ClientRequestBeginDateTextbox.Text = string.Empty;
            EXOResponseControl.ClientRequestBeginTimeTextbox.Text = string.Empty;
            EXOResponseControl.ClientRequestEndDateTextbox.Text = string.Empty;
            EXOResponseControl.ClientRequestEndTimeTextbox.Text = string.Empty;
            EXOResponseControl.OverallElapsedTextBox.Text = string.Empty;
            EXOResponseControl.ServerGotRequestDateTextBox.Text = string.Empty;
            EXOResponseControl.ServerGotRequestTimeTextBox.Text = string.Empty;
            EXOResponseControl.ServerDoneResponseDateTextBox.Text = string.Empty;
            EXOResponseControl.ServerDoneResponseTimeTextBox.Text = string.Empty;
            EXOResponseControl.ServerThinkTimeTextBox.Text = string.Empty;
            EXOResponseControl.XHostIPTextBox.Text = string.Empty;
        }
    }
}
