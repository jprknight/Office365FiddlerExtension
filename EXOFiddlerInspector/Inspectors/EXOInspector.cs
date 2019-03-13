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
                {
                    Clear();
                    ResultsString.AppendLine("-------------------------------");
                    ResultsString.AppendLine("EXO Fiddler Extension Disabled.");
                    ResultsString.AppendLine("-------------------------------");
                    ExchangeResponseControl.ResultsOutput.AppendText(ResultsString.ToString());
                    return;
                }

                Clear();

                this.session = _session;

                this.session.utilDecodeRequest(true);
                this.session.utilDecodeResponse(true);

                //this.Clear();

                ResultsString.AppendLine("General Session Data");
                ResultsString.AppendLine("--------------------");
                ResultsString.AppendLine();

                // Write data into Exchange Type and session ID.
                ResultsString.AppendLine($"Session Id: {this.session.id.ToString()}");

                ResultsString.AppendLine($"HTTP Response Code: {this.session.responseCode.ToString()}");

                // Write Data age data into textbox.
                String TimeSpanDaysText = "";
                String TimeSpanHoursText = "";
                String TimeSpanMinutesText = "";

                DateTime SessionDateTime = this.session.Timers.ClientBeginRequest;
                DateTime DateTimeNow = DateTime.Now;
                TimeSpan CalcDataAge = DateTimeNow - SessionDateTime;
                int TimeSpanDays = CalcDataAge.Days;
                int TimeSpanHours = CalcDataAge.Hours;
                int TimeSpanMinutes = CalcDataAge.Minutes;

                if (TimeSpanDays == 0)
                {
                    // Do nothing.
                }
                else if (TimeSpanDays == 1)
                {
                    TimeSpanDaysText = TimeSpanDays + " day, ";
                }
                else
                {
                    TimeSpanDaysText = TimeSpanDays + " days, ";
                }

                if (TimeSpanHours == 1)
                {
                    TimeSpanHoursText = TimeSpanHours + " hour, ";
                }
                else
                {
                    TimeSpanHoursText = TimeSpanHours + " hours, ";
                }

                if (TimeSpanMinutes == 1)
                {
                    TimeSpanMinutesText = TimeSpanMinutes + " minute ago.";
                }
                else
                {
                    TimeSpanMinutesText = TimeSpanMinutes + " minutes ago.";
                }

                String DataAge = TimeSpanDaysText + TimeSpanHoursText + TimeSpanMinutesText;

                String DataCollected = SessionDateTime.ToString("dddd, MMMM dd, yyyy h:mm tt");

                ResultsString.AppendLine($"Session Captured: {DataCollected}");
                ResultsString.AppendLine($"Capture was {DataAge}");

                ResultsString.AppendLine($"Session Type: {this.session["X-ExchangeType"]}");
                ResultsString.AppendLine($"Process: { this.session.LocalProcess}");


                if (this.session["X-HostIP"]?.ToString().Length > 0)
                {
                    ResultsString.AppendLine($"Host IP: {this.session["X-HostIP"]}");
                }

                // Response Server.
                if (this.session.isTunnel == true)
                {
                    ResultsString.AppendLine("Connect Tunnel");
                }
                else if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != ""))
                {
                    ResultsString.AppendLine($"Response Server: {this.session.oResponse["Server"]}");
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

                /// <remarks>
                /// Client Begin and done response. -- Overall elapsed time.
                /// </remarks>
                ResultsString.AppendLine("Overall Session Timers");
                ResultsString.AppendLine("----------------------");
                ResultsString.AppendLine();
                ResultsString.AppendLine("For an explantion of session timers refer to: https://aka.ms/Timers-Definitions");
                ResultsString.AppendLine();
                ResultsString.AppendLine($"Client Begin Request: {this.session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt")}");
                ResultsString.AppendLine($"Client Done Response: {this.session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt")}");

                // ClientDoneResponse can be blank. If so do not try to calculate and output Elapsed Time, we end up with a hideously large number.
                if (this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
                {
                    double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

                    ResultsString.Append($"Elapsed: {ClientMilliseconds}ms");

                    int SlowRunningSessionThreshold = Preferences.GetSlowRunningSessionThreshold();

                    if (ClientMilliseconds > SlowRunningSessionThreshold)
                    {
                        ResultsString.Append(" - Long running session!");
                        if (Preferences.AppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running session.");
                        }
                    }
                    ResultsString.AppendLine();
                }
                else
                {
                    ResultsString.AppendLine("Session does not contain data to calculate 'Elapsed Time'.");
                }

                /// <remarks>
                /// Server Got and Done Response. -- Server Think Time.
                /// </remarks>
                /// 
                ResultsString.AppendLine();
                ResultsString.AppendLine("Server Timers");
                ResultsString.AppendLine("-------------");
                ResultsString.AppendLine();
                // Write Server data into textboxes.
                ResultsString.AppendLine($"Server Got Request: { this.session.Timers.ServerGotRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt")}");
                ResultsString.AppendLine($"Server Begin Response: { this.session.Timers.ServerBeginResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt")}");
                ResultsString.AppendLine($"Server Done Response: {this.session.Timers.ServerDoneResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt")}");

                // ServerGotRequest, ServerBeginResponse or ServerDoneResponse can be blank. If so do not try to calculate and output 'Server Think Time' or 'Transmit Time', we end up with a hideously large number.
                if (this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                    this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                    this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
                {

                    double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

                    ResultsString.Append($"Server Think Time: {ServerMilliseconds}ms");

                    int SlowRunningSessionThreshold = Preferences.GetSlowRunningSessionThreshold();

                    if (ServerMilliseconds > SlowRunningSessionThreshold)
                    {
                        ResultsString.Append(" - Long running EXO session!");
                        if (Preferences.AppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running EXO session.");
                        }
                    }
                    ResultsString.AppendLine();
                    ResultsString.AppendLine($"Transit Time: { Math.Round((this.session.Timers.ServerDoneResponse - this.session.Timers.ServerBeginResponse).TotalMilliseconds)} ms");
                }
                else
                {
                    ResultsString.AppendLine("Session does not contain data to calculate 'Server Think Time' and 'Transit Time'.");
                }

                // Authentication
                ResultsString.AppendLine();
                ResultsString.AppendLine("Authentication");
                ResultsString.AppendLine("--------------");
                ResultsString.AppendLine();
                ResultsString.AppendLine($"Authentication Type: {this.session["X-AUTHENTICATION"]}");
                ResultsString.AppendLine($"Authentication Decription: {this.session["X-AUTHENTICATIONDESC"]}");

                if (this.session["X-Office365AuthType"] == "SAMLResponseParser")
                {
                    ResultsString.AppendLine($"Issuer: {this.session["X-ISSUER"]}");
                    ResultsString.AppendLine($"Attribute Name Immutable Id: {this.session["X-ATTRIBUTENAMEIMMUTABLEID"]}");
                    ResultsString.AppendLine($"Attribute Name UPN: {this.session["X-ATTRIBUTENAMEUPN"]}");
                    ResultsString.AppendLine($"Name Identifier Format: {this.session["X-NAMEIDENTIFIERFORMAT"]}");
                }

                ResultsString.AppendLine();
                ResultsString.AppendLine("Session Analysis");
                ResultsString.AppendLine("----------------");
                ResultsString.AppendLine();
                ResultsString.AppendLine($"Session Alert: {this.session["X-ResponseAlert"]}");
                ResultsString.AppendLine();
                ResultsString.AppendLine($"Session Comment: {this.session["X-ResponseComments"]}");
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