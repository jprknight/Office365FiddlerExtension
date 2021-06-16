using System.Windows.Forms;
using Fiddler;
using System;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using System.Text;
using O365FiddlerInspector.UI;
using Office365FiddlerInspector.UI;

namespace Office365FiddlerInspector.Inspectors
{
    /// <summary>
    /// Base class, generic inspector
    /// </summary>
    public class Office365Inspector : Inspector2, IResponseInspector2
    {
        public Office365Inspector()
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
                return this.RawBody;
            }

            set
            {
                this.RawBody = value;
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
        private byte[] RawBody { get; set; }

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

            if (this.IsHTTP)
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
        public bool IsHTTP
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
            Office365ResponseControl webBrowser = new Office365ResponseControl();
            o.Text = "Office 365";
            o.ToolTipText = "Office 365 Inspector";
            webBrowser.Size = o.Size;
            o.Controls.Add(webBrowser);
            o.Controls[0].Dock = DockStyle.Fill;
        }

        /// <summary>
        ///  Parse the HTTP payload to FSSHTTP and WOPI message.
        /// </summary>
        /// <param name="_session"></param>
        /// <returns></returns>
        public async Task ParseHTTPResponse(Session session)
        {
            try
            {
                // Extension disabled.
                if (!Preferences.ExtensionEnabled)
                {
                    // Clear ResultsString.
                    Clear();
                    ResultsString.AppendLine("<h2>Office 365 Fiddler Extension Disabled</h2>");
                    Office365ResponseControl.ResultsOutput.DocumentText = ResultsString.ToString();
                    return;
                }

                // Clear ResultsString.
                Clear();

                this.session = session;

                // HTML Header.
                ResultsString.AppendLine("<html>");
                ResultsString.AppendLine("<title></title>");
                ResultsString.AppendLine("<head></head>");
                ResultsString.AppendLine("<body>");
                ResultsString.AppendLine("<font face='open-sans'>");

                ResultsString.Append(FiddlerApplication.Prefs.GetStringPref("extensions.Office365.UpdateMessage", ""));

                // General Session Data.
                #region GeneralSessionData

                if (!this.session.isFlagSet(SessionFlags.LoadedFromSAZ))
                {
                    ResultsString.AppendLine("<h2><span style=color:'red'>Sessions Not Loaded from SAZ</span></h2>");
                    ResultsString.AppendLine("<p>For the best results analysing data save the sessions "
                        + "as a SAZ file and load them back in. Click <i>File, Save, All Sessions</i>.</p>"
                        + "<p>When analysing live traffic, there are multiple scenarios where the session response is not immeidately available. This Alters the "
                        + "responses the extension shows on session analysis.</p>");
                }

                ResultsString.AppendLine("<h2>General Session Data</h2>");
                
                ResultsString.AppendLine("<table border='0'>");
                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td width='150px'>");
                ResultsString.AppendLine("Session Id");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.id.ToString());
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("HTTP Response Code");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"<a href='https://en.wikipedia.org/wiki/List_of_HTTP_status_codes' target='_blank'>{this.session["X-ResponseCodeDescription"]}</a>");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                
                ResultsString.AppendLine("Session Captured");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session["X-DataCollected"]);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Capture was");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session["X-DataAge"]);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Session Alert");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session["X-ResponseAlert"]);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Process");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session["X-ProcessName"]);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                if (this.session["X-ResponseServer"] != null)
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Response Server");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-ResponseServer"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                if (this.session["X-InspectorElapsedTime"] != "Insufficient data")
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Elapsed Time");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-InspectorElapsedTime"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                ResultsString.AppendLine("</table>");
                #endregion

                // Session Analysis.
                ResultsString.AppendLine("<h2>Session Analysis</h2>");

                ResultsString.AppendLine($"<p>{this.session["X-ResponseComments"]}</p>");

                // Session Age.
                ResultsString.AppendLine($"<h2>Session Age</h2>");

                ResultsString.AppendLine($"<p>{this.session["X-CalculatedSessionAge"]}</p>");

                // Authentication
                #region Authentication
                if (this.session["X-AUTHENTICATION"] != "No Auth Headers")
                {
                    ResultsString.AppendLine("<h2>Authentication</h2>");

                    ResultsString.AppendLine("<table border='0'>");
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td width='150px'>");
                    ResultsString.AppendLine("Type");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-AUTHENTICATION"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Description");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-AUTHENTICATIONDESC"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("</table>");
                }

                if (this.session["X-Office365AuthType"] == "SAMLResponseParser")
                {

                    ResultsString.AppendLine("<h2>SAML Response Parser</h2>");

                    ResultsString.AppendLine("<table border='0'>");
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Issuer");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-ISSUER"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Attribute Name Immutable Id");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-ATTRIBUTENAMEIMMUTABLEID"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Attribute Name UPN");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-ATTRIBUTENAMEUPN"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Name Identifier Format");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-NAMEIDENTIFIERFORMAT"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("</table>");

                    ResultsString.AppendLine("<p>Copy and save the below text into a .cer file to view the signing certificate.</p>");
                    ResultsString.AppendLine("-----BEGIN CERTIFICATE-----<br />");
                    ResultsString.AppendLine($"{this.session["X-SigningCertificate"]}<br />");
                    ResultsString.AppendLine("-----END CERTIFICATE-----");
                }
                #endregion

                // Session Timers.
                #region SessionTimers
                ResultsString.AppendLine("<h2>Overall Session Timers</h2>");

                ResultsString.AppendLine("<p>For an explantion of session timers refer to: <a href='https://aka.ms/Timers-Definitions' target='_blank'>https://aka.ms/Timers-Definitions</a>.</p>");

                ResultsString.AppendLine("<table border='0'>");
                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td width='150px'>");
                ResultsString.AppendLine("Client Connected");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ClientConnected.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Client Begin Request");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Got Request Headers");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.FiddlerGotRequestHeaders.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Client Done Response");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                if (this.session["X-InspectorElapsedTime"] != "Insufficient data")
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Elapsed Time");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-InspectorElapsedTime"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                ResultsString.AppendLine("</table>");
                #endregion

                // Server Timers.
                #region ServerTimers
                ResultsString.AppendLine("<h2>Server Timers</h2>");

                ResultsString.AppendLine("<table border='0'>");
                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td width='150px'>");
                ResultsString.AppendLine("Fiddler Begin Request");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.FiddlerBeginRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Server Got Request");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ServerGotRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Server Begin Response");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ServerBeginResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Got Response Headers");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.FiddlerGotResponseHeaders.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Server Done Response");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ServerDoneResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                if (this.session["X-ServerThinkTime"] != "Insufficient data" && this.session["X-ServerThinkTime"] != null)
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Server Think Time");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-ServerThinkTime"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                if (this.session["X-TransitTime"] != "Insufficient data" && this.session["X-TransitTime"] != null)
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Transit Time");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-TransitTime"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                ResultsString.AppendLine("</table>");
                #endregion

                // HTML Footer.
                ResultsString.AppendLine("</font>");
                ResultsString.AppendLine("</body>");
                ResultsString.AppendLine("</html>");

                Office365ResponseControl.ResultsOutput.DocumentText = ResultsString.ToString();
            }
            catch (Exception ex)
            {
                ResultsString.AppendLine();
                ResultsString.AppendLine(ex.Message);
                ResultsString.AppendLine();

                Office365ResponseControl.ResultsOutput.DocumentText = ResultsString.ToString();
            }
        }


        /// <summary>
        /// Method Fiddler calls to clear the display
        /// </summary>
        public void Clear()
        {
            Office365ResponseControl.ResultsOutput.DocumentText = "";

            ResultsString = new StringBuilder();
        }
    }
}