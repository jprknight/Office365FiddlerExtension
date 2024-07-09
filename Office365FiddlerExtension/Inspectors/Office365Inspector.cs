using System.Windows.Forms;
using Fiddler;
using System;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using System.Text;
using Office365FiddlerExtension.UI;

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
        internal Session session { get; set; }

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
                if (CachedSessionId != this.session.id || CachedSessionId == 0 && this.session != null)
                {
                    this.CachedSessionId = this.session.id;
                    this.ParseSession(this.session);
                }
            }
        }

        /// <summary>
        /// Update the view with parsed and diagnosed data
        /// </summary>
        private async void ParseSession(Session Session)
        {
            this.session = Session;
            await ParseHTTPResponse(this.session);
        }
        
        /// <summary>
        /// This is called every time this inspector is shown
        /// </summary>
        /// <param name="oS">Session object passed by Fiddler</param>
        public override void AssignSession(Session Session)
        {
            this.session = Session;
            base.AssignSession(this.session);
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
            this.session = Session;

            return 100;
        }

        /// <summary>
        /// Called by Fiddler to add the Office 365 inspector tab
        /// </summary>
        /// <param name="o">The tab control for the inspector</param>
        public override void AddToTab(TabPage o)
        {
            Office365ResponseControl webBrowser = new Office365ResponseControl();
            o.Text = LangHelper.GetString("Office 365");
            o.ToolTipText = $"{LangHelper.GetString("Office 365")} {LangHelper.GetString("Inspector")}";
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
        // AWAIT. Can this be done or would it break things.
        // Tested with await task.run and it broke the inspector.
        public async Task ParseHTTPResponse(Session Session)
        {
            this.session = Session;

            // Extension disabled.
            if (!SettingsJsonService.Instance.ExtensionSessionProcessingEnabled)
            {
                // Clear ResultsString.
                Clear();
                ResultsString.AppendLine($"<br /><h2>{LangHelper.GetString("Office 365 Fiddler Extension")} {LangHelper.GetString("Disabled")}</h2>");
                Office365ResponseControl.ResultsOutput.DocumentText = ResultsString.ToString();
                return;
            }

            if (this.session["Microsoft365FiddlerExtensionJson"] == null)
            {
                // Clear ResultsString.
                Clear();
                ResultsString.AppendLine($"<br /><h2>{LangHelper.GetString("Office 365 Fiddler Extension")}</h2>");
                ResultsString.AppendLine($"<p>{LangHelper.GetString("Inspector_Session_Analysis")}</p>");
                Office365ResponseControl.ResultsOutput.DocumentText = ResultsString.ToString();
                return;
            }

            try
            {              
                // Clear ResultsString.
                Clear();

                this.session = Session;

                var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

                // Check if the SectionTitle is blank, if it is session analysis hasn't been performed on this session, write this alternative output.
                if (ExtensionSessionFlags.SectionTitle == "")
                {
                    // Clear ResultsString.
                    Clear();
                    ResultsString.AppendLine($"<br /><h2>{LangHelper.GetString("Office 365 Fiddler Extension")}</h2>");
                    ResultsString.AppendLine($"<p>{LangHelper.GetString("Inspector_Session_Analysis")}</p>");
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

                ResultsString.AppendLine($"<h2>{LangHelper.GetString("General Session Data")}</h2>");

                ResultsString.AppendLine("<table border='0'>");
                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td width='150px'>");
                ResultsString.AppendLine($"{LangHelper.GetString("Session ID")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.id.ToString());
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Response Code")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"<a href='{URLsJsonService.Instance.GetDeserializedExtensionURLs().ResponseCodes}' target='_blank'>{ExtensionSessionFlags.ResponseCodeDescription}</a>");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Session_Severity")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                
                switch (ExtensionSessionFlags.SessionSeverity)
                {
                    case 10:
                        ResultsString.AppendLine($"{ExtensionSessionFlags.SessionSeverity}");
                        break;
                    case 20:
                        ResultsString.AppendLine($"{ExtensionSessionFlags.SessionSeverity}");
                        break;
                    case 30:
                        ResultsString.AppendLine($"<span style='color:green'>{ExtensionSessionFlags.SessionSeverity}</span>");
                        break;
                    case 40:
                        ResultsString.AppendLine($"<span style='color:orange'>{ExtensionSessionFlags.SessionSeverity}</span>");
                        break;
                    case 50:
                        ResultsString.AppendLine($"{ExtensionSessionFlags.SessionSeverity}");
                        break;
                    case 60:
                        ResultsString.AppendLine($"<span style='color:red'><b>{ExtensionSessionFlags.SessionSeverity}</b></span>");
                        break;
                    default:
                        ResultsString.AppendLine($"<span style='color:red'><b>Unexpected value: {ExtensionSessionFlags.SessionSeverity}</b></span>");
                        break;
                }
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Session Captured")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.DateDataCollected);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Capture was")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.DataAge);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Process")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.ProcessName);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                if (ExtensionSessionFlags.InspectorElapsedTime != "Insufficient data")
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine($"{LangHelper.GetString("Elapsed Time")}");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.InspectorElapsedTime);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                if (ExtensionSessionFlags.ResponseServer != null)
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine($"{LangHelper.GetString("Response Server")}");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.ResponseServer);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                if ((this.session["X-HostIP"] != null) && (this.session["X-HostIP"] != ""))
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine($"{LangHelper.GetString("Host IP")}");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(this.session["X-HostIP"]);

                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                ResultsString.AppendLine("</table>");

                if ((this.session["X-HostIP"] != null) && (this.session["X-HostIP"] != ""))
                {
                    // Tuple -- tupleIsPrivateIPAddress (bool), matching subnet (string).
                    Tuple<bool, string> tupleIsPrivateIPAddress = NetworkingService.Instance.IsPrivateIPAddress(this.session);

                    if (tupleIsPrivateIPAddress.Item1)
                    {
                        ResultsString.AppendLine($"<p>{this.session["X-HostIP"]} {LangHelper.GetString("IsPrivateSubnet")} " +
                            $"{tupleIsPrivateIPAddress.Item2} {LangHelper.GetString("network")}.</p>");
                    }
                    else
                    {
                        if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
                        {
                            ResultsString.AppendLine($"<p>{this.session["X-HostIP"]}</p>");
                        }
                        else
                        {
                            // Tuple -- IsMicrosoft365IP (bool), matching subnet (string).
                            Tuple<bool, string> tupleIsMicrosoft365IPAddress = NetworkingService.Instance.IsMicrosoft365IPAddress(this.session);

                            if (tupleIsMicrosoft365IPAddress.Item1)
                            {
                                ResultsString.AppendLine($"<p>{this.session["X-HostIP"]} {LangHelper.GetString("IsMicrosoft365subnet")}: " +
                                    $"{tupleIsMicrosoft365IPAddress.Item2}.</p>");
                            }
                            else
                            {
                                ResultsString.AppendLine($"<p>{this.session["X-HostIP"]} {LangHelper.GetString("IsPublicIPAddress")}</p>");
                            }
                        }
                    }
                }

                #endregion

                // Session Analysis.
                ResultsString.AppendLine($"<h2>{LangHelper.GetString("Session Analysis")}</h2>");
                
                ResultsString.AppendLine($"<p>{ExtensionSessionFlags.ResponseComments}</p>");

                // Session Age.
                ResultsString.AppendLine($"<h2>{LangHelper.GetString("Session Age")}</h2>");

                ResultsString.AppendLine($"<p>{ExtensionSessionFlags.CalculatedSessionAge}</p>");

                // Authentication
                #region Authentication
                if (ExtensionSessionFlags.Authentication != "No Auth Headers")
                {
                    ResultsString.AppendLine($"<h2>{LangHelper.GetString("Authentication")}</h2>");

                    ResultsString.AppendLine($"<h3>{ExtensionSessionFlags.Authentication}</h3>");

                    ResultsString.AppendLine($"<p>{ExtensionSessionFlags.AuthenticationDescription}</p>");
                }

                if (ExtensionSessionFlags.AuthenticationType == "SAMLResponseParser")
                {

                    ResultsString.AppendLine($"<h2>{LangHelper.GetString("SAML Response Parser")}</h2>");

                    ResultsString.AppendLine("<table border='0'>");
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine($"{LangHelper.GetString("Issuer")}");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SamlTokenIssuer);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine($"{LangHelper.GetString("Attribute Name Immutable ID")}");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SamlTokenAttributeNameImmutibleID);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine($"{LangHelper.GetString("Attribute Name UPN")}");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SamlTokenAttributeNameUPN);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");

                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine($"{LangHelper.GetString("Name Identifier Format")}");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SamlTokenNameIdentifierFormat);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                    ResultsString.AppendLine("</table>");

                    ResultsString.AppendLine($"<p>{LangHelper.GetString("Copy Save Signing Certificate")}</p>");
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
                ResultsString.AppendLine($"<h2>{LangHelper.GetString("Overall Session Timers")}</h2>");

                ResultsString.AppendLine("<table border='0'>");
                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td width='150px'>");
                ResultsString.AppendLine($"{LangHelper.GetString("Client Connected")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ClientConnected.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Client Begin Request")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ClientBeginRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Got Request Headers")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.FiddlerGotRequestHeaders.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Client Done Response")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ClientDoneResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Elapsed Time")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.InspectorElapsedTime);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("</table>");
                #endregion

                // Server Timers.
                #region ServerTimers
                ResultsString.AppendLine($"<h2>{LangHelper.GetString("Server Timers")}</h2>");

                ResultsString.AppendLine("<table border='0'>");
                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td width='150px'>");
                ResultsString.AppendLine($"{LangHelper.GetString("Fiddler Begin Request")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.FiddlerBeginRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Server Got Request")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ServerGotRequest.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Server Begin Response")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ServerBeginResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Got Response Headers")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.FiddlerGotResponseHeaders.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Server Done Response")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(this.session.Timers.ServerDoneResponse.ToString("yyyy/MM/dd H:mm:ss.fff tt"));
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Server Think Time")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.ServerThinkTime);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                ResultsString.AppendLine("<tr>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine($"{LangHelper.GetString("Transit Time")}");
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("<td>");
                ResultsString.AppendLine(ExtensionSessionFlags.TransitTime);
                ResultsString.AppendLine("</td>");
                ResultsString.AppendLine("</tr>");

                if (ExtensionSessionFlags.SessionTimersDescription != null && ExtensionSessionFlags.SessionTimersDescription != "")
                {
                    ResultsString.AppendLine("<tr>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine($"{LangHelper.GetString("Description")}");
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("<td>");
                    ResultsString.AppendLine(ExtensionSessionFlags.SessionTimersDescription);
                    ResultsString.AppendLine("</td>");
                    ResultsString.AppendLine("</tr>");
                }

                ResultsString.AppendLine("</table>");
                #endregion

                ResultsString.AppendLine($"<p>{LangHelper.GetString("For an explantion of session timers refer to")} <a href='https://github.com/jprknight/Office365FiddlerExtension/wiki/Timers-Definitions' target='_blank'>https://github.com/jprknight/Office365FiddlerExtension/wiki/Timers-Definitions</a>.</p>");

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