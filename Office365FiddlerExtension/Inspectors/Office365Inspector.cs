using System.Windows.Forms;
using Fiddler;
using System;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using System.Text;
using Office365FiddlerExtension.UI;
using Newtonsoft.Json;
using Microsoft.Diagnostics.Instrumentation.Extensions.Intercept;

namespace Office365FiddlerExtension.Inspectors
{
    /// <summary>
    /// Fiddler inspector for extension.
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
        internal Session Session { get; set; }

        internal int CachedSessionId { get; set; }

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
                if (CachedSessionId != this.Session.id || CachedSessionId == 0 && this.Session != null)
                {
                    this.CachedSessionId = this.Session.id;
                    this.ParseSession(this.Session);
                }
            }
        }

        /// <summary>
        /// Update the view with parsed and diagnosed data
        /// </summary>
        private async void ParseSession(Session Session)
        {
            this.Session = Session;
            await ParseHTTPResponse(this.Session);
        }
        
        /// <summary>
        /// This is called every time this inspector is shown
        /// </summary>
        /// <param name="oS">Session object passed by Fiddler</param>
        public override void AssignSession(Session Session)
        {
            this.Session = Session;
            base.AssignSession(this.Session);
        }

        /// <summary>
        /// Gets or sets the raw bytes from the frame
        /// </summary>
        private byte[] RawBody { get; set; }

        /// <summary>
        /// Method that returns a sorting hint, make this the first inspector from the left.
        /// </summary>
        /// <returns>An integer indicating where we should order ourselves</returns>
        public override int GetOrder()
        {
            return -8443;
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
        public override int ScoreForSession(Session Session)
        {
            this.Session = Session;

            return 100;
        }

        /// <summary>
        /// Called by Fiddler to add the Office 365 inspector tab
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
        // public async Task ParseHTTPResponse(Session Session)
        // REVIEW THIS - AWAIT. Can this be done or would it break things.
        // Tested with await task.run and it broke the inspector.
        public async Task ParseHTTPResponse(Session Session)
        {
            this.Session = Session;

            // Extension disabled.
            if (!SettingsJsonService.Instance.ExtensionSessionProcessingEnabled)
            {
                // Clear ResultsString.
                Clear();
                ResultsString.AppendLine("<br /><h2>Office 365 Fiddler Extension Disabled</h2>");
                Office365ResponseControl.ResultsOutput.DocumentText = ResultsString.ToString();
                return;
            }

            if (this.Session["Microsoft365FiddlerExtensionJson"] == null)
            {
                // Clear ResultsString.
                Clear();
                ResultsString.AppendLine("<br /><h2>Office 365 Fiddler Extension</h2>");
                ResultsString.AppendLine("<p>No session analysis. Use the \"Process All Sessions\" option from the Office365 extension menu to get session analysis.</p>");
                Office365ResponseControl.ResultsOutput.DocumentText = ResultsString.ToString();
                return;
            }

            try
            {              
                // Clear ResultsString.
                Clear();

                this.Session = Session;

                //SessionFlagHandler sessionFlagProcessor = new SessionFlagHandler();

                /*var JsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };*/

                var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.Session);

                // Check if the SectionTitle is blank, if it is session analysis hasn't been performed on this session, write this alternative output.
                if (ExtensionSessionFlags.SectionTitle == "")
                {
                    // Clear ResultsString.
                    Clear();
                    ResultsString.AppendLine("<br /><h2>Office 365 Fiddler Extension</h2>");
                    ResultsString.AppendLine("<p>No session analysis. Enable all session analysis options in the About screen from the extension menu, "
                        + "or use the \"Process All Sessions\" option from the Office365 extension menu to get session analysis.</p>");
                    Office365ResponseControl.ResultsOutput.DocumentText = ResultsString.ToString();
                    return;
                }

                // HTML Header.
                ResultsString.AppendLine("<html>");
                ResultsString.AppendLine("<title></title>");
                ResultsString.AppendLine("<head></head>");
                ResultsString.AppendLine("<body>");
                ResultsString.AppendLine("<font face='open-sans'>");

                ResultsString.Append(FiddlerApplication.Prefs.GetStringPref("extensions.Office365.UpdateMessage", ""));

                // General Session Data.
                #region GeneralSessionData

                ResultsString.AppendLine("<br />");

                ResultsString.AppendLine("<h2>General Session Data</h2>");

                ResultsString.AppendLine("<table border='0'>");
                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td width='150px'>");
                ResultsString.AppendLine("Session Id");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.id.ToString());
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Response Code");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"<a href='https://en.wikipedia.org/wiki/List_of_HTTP_status_codes' target='_blank'>{ExtensionSessionFlags.ResponseCodeDescription}</a>");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Session Captured");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.DateDataCollected);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Session Analysis");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Capture was");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.DataAge);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Process");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.ProcessName);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                if ((this.Session["X-HostIP"] != null) && (this.Session["X-HostIP"] != "")) {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Host IP");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.Session["X-HostIP"]);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                if (ExtensionSessionFlags.ResponseServer != null)
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Response Server");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.ResponseServer);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                if (ExtensionSessionFlags.InspectorElapsedTime != "Insufficient data")
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Elapsed Time");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.InspectorElapsedTime);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                ResultsString.AppendLine("</table>");
                #endregion

                // Session Analysis.
                ResultsString.AppendLine("<h2>Session Analysis</h2>");

                ResultsString.AppendLine($"<p>{ExtensionSessionFlags.ResponseComments}</p>");

                // Session Age.
                ResultsString.AppendLine($"<h2>Session Age</h2>");

                ResultsString.AppendLine($"<p>{ExtensionSessionFlags.CalculatedSessionAge}</p>");

                // Authentication
                #region Authentication
                if (ExtensionSessionFlags.Authentication != "No Auth Headers")
                {
                    ResultsString.AppendLine("<h2>Authentication</h2>");

                    ResultsString.AppendLine($"<h3>{ExtensionSessionFlags.Authentication}</h3>");

                    ResultsString.AppendLine($"<p>{ExtensionSessionFlags.AuthenticationDescription}</p>");
                }

                if (ExtensionSessionFlags.AuthenticationType == "SAMLResponseParser")
                {

                    ResultsString.AppendLine("<h2>SAML Response Parser</h2>");

                    ResultsString.AppendLine("<table border='0'>");
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Issuer");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SamlTokenIssuer);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Attribute Name Immutable Id");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SamlTokenAttributeNameImmutibleID);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Attribute Name UPN");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SamlTokenAttributeNameUPN);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Name Identifier Format");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SamlTokenNameIdentifierFormat);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                    ResultsString.AppendLine("</table>");

                    ResultsString.AppendLine("<p>Copy and save the below text into a .cer file to view the signing certificate.</p>");
                    ResultsString.AppendLine("-----BEGIN CERTIFICATE-----<br />");

                    string str = ExtensionSessionFlags.SamlTokenSigningCertificate;
                    int chunkSize = 50;
                    int stringLength = str.Length;
                    for (int i = 0; i < stringLength; i += chunkSize)
                    {
                        if (i + chunkSize > stringLength) chunkSize = stringLength - i;
                        ResultsString.AppendLine(str.Substring(i, chunkSize));

                    }

                    ResultsString.AppendLine("<br />-----END CERTIFICATE-----");
                }
                #endregion

                // Session Timers.
                #region SessionTimers
                ResultsString.AppendLine("<h2>Overall Session Timers</h2>");

                ResultsString.AppendLine("<table border='0'>");
                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td width='150px'>");
                ResultsString.AppendLine("Client Connected");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.Timers.ClientConnected.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Client Begin Request");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Got Request Headers");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.Timers.FiddlerGotRequestHeaders.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Client Done Response");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Elapsed Time");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.InspectorElapsedTime);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

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
                ResultsString.AppendLine(this.Session.Timers.FiddlerBeginRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Server Got Request");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.Timers.ServerGotRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Server Begin Response");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.Timers.ServerBeginResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Got Response Headers");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.Timers.FiddlerGotResponseHeaders.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Server Done Response");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.Session.Timers.ServerDoneResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Server Think Time");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.ServerThinkTime);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine("Transit Time");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.TransitTime);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                if (ExtensionSessionFlags.SessionTimersDescription != null && ExtensionSessionFlags.SessionTimersDescription != "")
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine("Description");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SessionTimersDescription);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                ResultsString.AppendLine("</table>");
                #endregion

                ResultsString.AppendLine("<p>For an explantion of session timers refer to: <a href='https://aka.ms/Timers-Definitions' target='_blank'>https://aka.ms/Timers-Definitions</a>.</p>");

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