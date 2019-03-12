using System.Windows.Forms;
using System.Linq;
using System.IO;
using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EXOFiddlerInspector.Services;
using EXOFiddlerInspector.UI;
using System.Collections.ObjectModel;
using System.Text;

namespace EXOFiddlerInspector.Inspectors
{
    /// <summary>
    /// Base class, generic inspector
    /// </summary>
    public class EXOInspector : Inspector2, IResponseInspector2
    {
        public EXOInspector()
        {

        }

        public StringBuilder ResultsString { get; set; }

        /// <summary>
        /// Gets or sets the Session object to pull frame data from Fiddler.
        /// </summary>
        internal Session session { get; set; }

        internal int cachedSessionId { get; set; }

        /// <summary>
        /// Gets or sets the base HTTP headers assigned by the request or response
        /// </summary>
        public HTTPHeaders BaseHeaders { get; set; }

        public HTTPResponseHeaders headers
        {
            get
            {
                return this.BaseHeaders as HTTPResponseHeaders;
            }

            set
            {
                this.BaseHeaders = value;
            }
        }

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
                if (cachedSessionId != session.id || cachedSessionId == 0 && this.session != null)
                {
                    this.cachedSessionId = session.id;
                    this.ParseSession(this.session);
                }
            }
        }

        /// <summary>
        /// Update the view with parsed and diagnosed data
        /// </summary>
        private async void ParseSession(Session _session)
        {
            await ParseHTTPResponse(_session);
        }

        public void SaveSessionData(Session oS)
        {
            this.session = oS;
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

            ExchangeResponseControl testControl = new ExchangeResponseControl();
            o.Text = "Exchange Online";
            o.ToolTipText = "Exchange Online Inspector";
            testControl.Size = o.Size;
            o.Controls.Add(testControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }


        /// <summary>
        ///  Parse the HTTP payload to FSSHTTP and WOPI message.
        /// </summary>
        /// <param name="_session"></param>
        /// <returns></returns>
        public async Task ParseHTTPResponse(Session _session)
        {
            try
            {
                if (!Preferences.ExtensionEnabled)
                    return;

                Clear();

                this.session = _session;

                this.session.utilDecodeRequest(true);
                this.session.utilDecodeResponse(true);

                this.Clear();

                // Write data into Exchange Type and session ID.
                ResultsString.AppendLine($"Type: {this.session["X-ExchangeType"]}");
                ResultsString.AppendLine($"SessionId: {this.session.id.ToString()}");

                // Write HTTP Status Code Text box, convert int to string.
                ResultsString.AppendLine($"ResponseCode: {this.session.responseCode.ToString()}");

                /// <remarks>
                /// Client Begin and done response. -- Overall elapsed time.
                /// </remarks>

                if (this.session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff") != "0:00:00.000" || this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
                {
                    ResultsString.AppendLine($"StartDate: {this.session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd")}");
                    ResultsString.AppendLine($"StartTime: {this.session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff")}");

                    ResultsString.AppendLine($"EndDate: {this.session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd")}");
                    ResultsString.AppendLine($"EndTime: {this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff")}");

                    double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

                    ResultsString.AppendLine($"Elapsed: {ClientMilliseconds} ms");
                    ResultsString.AppendLine();
                    /// <remarks>
                    /// Notify on slow running session with threshold pulled from Preferences.cs.
                    /// </remarks>
                    /// 
                    int SlowRunningSessionThreshold = Preferences.GetSlowRunningSessionThreshold();

                    if (ClientMilliseconds > SlowRunningSessionThreshold)
                    {
                        ResultsString.AppendLine("Long running session!");
                        ResultsString.AppendLine("Found a long running session." +
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

                        ResultsString.AppendLine();

                        if (Preferences.AppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running session.");
                        }
                    }
                }

                /// <remarks>
                /// Server Got and Done Response. -- Server Think Time.
                /// </remarks>
                /// 
                if (this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff") != "0:00:00.000" ||
                    this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff") != "0:00:00.000" ||
                    this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
                {
                    // Write Server data into textboxes.
                    ResultsString.AppendLine($"Server StartDate: { this.session.Timers.ServerGotRequest.ToString("yyyy / MM / dd")}");
                    ResultsString.AppendLine($"Server StartTime: { this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff")}");
                    ResultsString.AppendLine($"Server BeginResponseDate: { this.session.Timers.ServerBeginResponse.ToString("yyyy/MM/dd")}");
                    ResultsString.AppendLine($"Server BeginResponseTime: { this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff")}");
                    ResultsString.AppendLine($"Server DoneResponseDate: {this.session.Timers.ServerDoneResponse.ToString("yyyy /MM/dd")}");
                    ResultsString.AppendLine($"Seerver DoneResponseTime: {this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff")}");

                    double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

                    ResultsString.AppendLine($"ServerThinkTime: {ServerMilliseconds} ms");

                    ResultsString.AppendLine($"TransitTime: { Math.Round((this.session.Timers.ServerDoneResponse - this.session.Timers.ServerBeginResponse).TotalMilliseconds)} ms");
                    ResultsString.AppendLine();

                    /// <remarks>
                    /// Notify on slow running session with threshold pulled from Preferences.cs.
                    /// </remarks>
                    /// 
                    int SlowRunningSessionThreshold = Preferences.GetSlowRunningSessionThreshold();

                    if (ServerMilliseconds > SlowRunningSessionThreshold)
                    {
                        ResultsString.AppendLine("Long running EXO session!");
                        ResultsString.AppendLine("Found a long running EXO session (> 5 seconds)." + Environment.NewLine +
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

                        ResultsString.AppendLine();

                        if (Preferences.AppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running EXO session.");
                        }
                    }
                }

                if (this.session["X-HostIP"]?.ToString().Length > 0)
                {
                    ResultsString.AppendLine($"XHostIP: {this.session["X-HostIP"]}");
                }


                // If the response server header is not null or blank then populate it into the response server value.
                if (this.session.isTunnel == true)
                {
                    ResultsString.AppendLine($"ResponseServer: {this.session.url}");
                }
                if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != ""))
                {
                    ResultsString.AppendLine($"ResponseServer: {this.session.oResponse["Server"]}");
                }
                // Else if the reponnse Host header is not null or blank then populate it into the response server value
                // Some traffic identifies a host rather than a response server.
                else if ((this.session.oResponse["Host"] != null && (this.session.oResponse["Host"] != "")))
                {
                    ResultsString.AppendLine($"Host: {this.session.oResponse["Host"]}");
                }
                // Else if the response PoweredBy header is not null or blank then populate it into the response server value.
                // Some Office 365 servers respond as X-Powered-By ASP.NET.
                else if ((this.session.oResponse["X-Powered-By"] != null) && (this.session.oResponse["X-Powered-By"] != ""))
                {
                    ResultsString.AppendLine($"X-Powered-By: {this.session.oResponse["X-Powered-By"]}");
                }
                // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
                else if ((this.session.oResponse["X-Served-By"] != null && (this.session.oResponse["X-Served-By"] != "")))
                {
                    ResultsString.AppendLine($"X-Served-By: {this.session.oResponse["X-Served-By"]}");
                }
                // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
                else if ((this.session.oResponse["X-Server-Name"] != null && (this.session.oResponse["X-Server-Name"] != "")))
                {
                    ResultsString.AppendLine($"X-Server-Name: {this.session.oResponse["X-Server-Name"]}");
                }

                ResultsString.AppendLine();

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

                ResultsString.AppendLine($"DataAge: {DataAgeOutput}");

                // Write Process into textbox.
                ResultsString.AppendLine($"ResponseProcess: { this.session.LocalProcess}");

                ResultsString.AppendLine();

                // Session rule set used to live here.
                // Now all the logic is ran in SessionRuleSet.cs.
                // Data for these two textboxes on the inspector tab is now written into session tags.
                ResultsString.AppendLine($"ResponseAlert: {this.session["X-ResponseAlertTextBox"]}");

                ResultsString.AppendLine($"ResponseComment: {this.session["X-ResponseCommentsRichTextboxText"]}");
                ResultsString.AppendLine();

                ExchangeResponseControl.ResultsOutput.AppendText(ResultsString.ToString());
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
            ExchangeResponseControl.ResultsOutput.Clear();

            ResultsString = new StringBuilder();
        }
    }
}
