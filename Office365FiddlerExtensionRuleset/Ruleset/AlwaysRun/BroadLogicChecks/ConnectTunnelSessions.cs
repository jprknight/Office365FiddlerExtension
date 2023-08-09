using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Ruleset.AlwaysRun.BroadLogicChecks
{
    class ConnectTunnelSessions
    {
        internal Session session { get; set; }

        private static ConnectTunnelSessions _instance;

        public static ConnectTunnelSessions Instance => _instance ?? (_instance = new ConnectTunnelSessions());

        public void Run(Session session)
        {
            this.session = session;

            // Connect Tunnel.
            //
            // Check for connect tunnel with no usable data in the response body.
            //
            // This check does not work for sessions which have not been loaded from a SAZ file.
            // My best guess is this is a timing issue, where the data is not immediately available when this check runs.
            // SetSessionType makes exactly the same call later on down the code path and it works.
            if (this.session.isTunnel)
            {
                string TLS;

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Broad Logic Checks (connect tunnel).");

                // TLS 1.0 in request/response pair.

                // Request:
                //   Version: 3.1 (TLS/1.0)

                //Response:
                //   Secure Protocol: Tls
                //   Cipher: Aes256 256bits
                //   Hash Algorithm: Sha1 160bits

                if (this.session.utilFindInResponse("Secure Protocol: Tls10", false) > 1 || this.session.utilFindInResponse("(TLS/1.0)", false) > 1)
                {
                    TLS = "TLS 1.0";
                }
                // TLS 1.1 in request/response pair.
                else if (this.session.utilFindInResponse("Secure Protocol: Tls11", false) > 1 || this.session.utilFindInRequest("(TLS/1.1)", false) > 1)
                {
                    TLS = "TLS 1.1";
                }
                // TLS 1.2 in request/response pair.
                else if (this.session.utilFindInRequest("Secure Protocol: Tls12", false) > 1 || this.session.utilFindInRequest("(TLS/1.2)", false) > 1)
                {
                    TLS = "TLS 1.2";
                }
                else
                {
                    // If we cannot determine the TLS version do nothing.
                    // This can happen when live tracing traffic. The request/responses cannot be read fast enough to get accurate results.
                    TLS = "TLS Unknown";
                }

                // 11/1/2022 -- There was some old code accompanying this comment, leaving this as it might be useful information for the future.

                // Trying to check session response body for a string value using !this.Session.bHasResponse does not impact performance, but is not reliable.
                // Using this.Session.GetResponseBodyAsString().Length == 0 kills performance. Fiddler wouldn't even load with this code in place.
                // Ideally looking to do: if (this.Session.utilFindInResponse("CONNECT tunnel, through which encrypted HTTPS traffic flows", false) > 1)
                // Only works reliably when loading a SAZ file and request/response data is immediately available to do logic checks against.

                string sessionSectionTitle;
                string sessionType;
                string sessionResponseCodeDescription;
                string sessionResonseServer;
                string sessionResponseAlert;
                string sessionResponseComments;
                string sessionAuthentication;

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("BroadLogicChecks|ConnectTunnelSessions");

                    sessionSectionTitle = sessionClassificationJson.SectionTitle;
                    sessionType = sessionClassificationJson.SessionType;
                    sessionResponseCodeDescription = sessionClassificationJson.SessionResponseCodeDescription;
                    sessionResonseServer = sessionClassificationJson.SessionResponseServer;
                    sessionResponseAlert = sessionClassificationJson.SessionResponseAlert;
                    sessionResponseComments = sessionClassificationJson.SessionResponseComments;
                    sessionAuthentication = sessionClassificationJson.SessionAuthentication;

                    sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                    sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                    sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                    sessionSeverity = sessionClassificationJson.SessionSeverity;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                    sessionSectionTitle = "Broad Logic Checks";
                    sessionType = "Connect Tunnel: " + TLS;
                    sessionResponseCodeDescription = "Connect Tunnel";
                    sessionResonseServer = "Connect Tunnel";
                    sessionResponseAlert = "Connect Tunnel";
                    sessionResponseComments = "This is an encrypted tunnel. If all or most of the sessions are connect tunnels "
                    + "the sessions collected did not have decryption enabled. Setup Fiddler to 'Decrypt HTTPS traffic', click Tools -> Options -> HTTPS tab."
                    + "<p>If in any doubt see instructions at https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/DecryptHTTPS. </p>";
                    sessionAuthentication = "Connect Tunnel: " + TLS;

                    sessionAuthenticationConfidenceLevel = 10;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 10;
                    sessionSeverity = 40;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = sessionSectionTitle,

                    SessionType = sessionType + TLS,
                    ResponseCodeDescription = sessionResponseCodeDescription,
                    ResponseServer = sessionResonseServer,
                    ResponseAlert = sessionResponseAlert,
                    ResponseComments = sessionResponseComments,

                    Authentication = sessionAuthentication,

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}
