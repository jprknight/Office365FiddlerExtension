using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Ruleset.AlwaysRun.BroadLogicChecks
{
    internal class ConnectTunnelSessions
    {
        internal Session session { get; set; }

        public static void CTS(Session session)
        {
            // Connect Tunnel.
            //
            // Check for connect tunnel with no usable data in the response body.
            //
            // This check does not work for sessions which have not been loaded from a SAZ file.
            // My best guess is this is a timing issue, where the data is not immediately available when this check runs.
            // SetSessionType makes exactly the same call later on down the code path and it works.
            if (session.isTunnel)
            {
                string TLS;

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {session.id} Broad Logic Checks (connect tunnel).");

                // TLS 1.0 in request/response pair.

                // Request:
                //   Version: 3.1 (TLS/1.0)

                //Response:
                //   Secure Protocol: Tls
                //   Cipher: Aes256 256bits
                //   Hash Algorithm: Sha1 160bits

                if (session.utilFindInResponse("Secure Protocol: Tls10", false) > 1 || session.utilFindInResponse("(TLS/1.0)", false) > 1)
                {
                    TLS = "TLS 1.0";
                }
                // TLS 1.1 in request/response pair.
                else if (session.utilFindInResponse("Secure Protocol: Tls11", false) > 1 || session.utilFindInRequest("(TLS/1.1)", false) > 1)
                {
                    TLS = "TLS 1.1";
                }
                // TLS 1.2 in request/response pair.
                else if (session.utilFindInRequest("Secure Protocol: Tls12", false) > 1 || session.utilFindInRequest("(TLS/1.2)", false) > 1)
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

                switch (session.responseCode)
                {
                    case 403:
                        // If this is a HTTP 403 we need analysis on this session.
                        // I have seen HTTP 403 connect tunnels actually show interesting data in authentication scenarios.
                        var sessionFlags403 = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            SectionTitle = "Broad Logic Checks",
                            UIBackColour = "Orange",
                            UITextColour = "Black",

                            SessionType = "Connect Tunnel: " + TLS,
                            ResponseServer = "Connect Tunnel",
                            ResponseAlert = "Connect Tunnel",
                            ResponseComments = "This is an encrypted tunnel. If all or most of the sessions are connect tunnels "
                            + "the sessions collected did not have decryption enabled. Setup Fiddler to 'Decrypt HTTPS traffic', click Tools -> Options -> HTTPS tab."
                            + "<p>If in any doubt see instructions at https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/DecryptHTTPS. </p>",

                            SessionAuthenticationConfidenceLevel = 5,
                            SessionTypeConfidenceLevel = 5,
                            SessionResponseServerConfidenceLevel = 5

                        };

                        var sessionFlagsJson403 = JsonConvert.SerializeObject(sessionFlags403);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(session, sessionFlagsJson403);
                        break;
                    case 200:
                        var sessionFlags200 = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            SectionTitle = "Broad Logic Checks",
                            UIBackColour = "Orange",
                            UITextColour = "Black",

                            SessionType = "Connect Tunnel: " + TLS,
                            ResponseCodeDescription = "200 OK",
                            ResponseServer = "Connect Tunnel",
                            ResponseAlert = "Connect Tunnel",
                            ResponseComments = "This is an encrypted tunnel. If all or most of the sessions are connect tunnels "
                            + "the sessions collected did not have decryption enabled. Setup Fiddler to 'Decrypt HTTPS traffic', click Tools -> Options -> HTTPS tab."
                            + "<p>If in any doubt see instructions at https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/DecryptHTTPS. </p>",

                            Authentication = "Connect Tunnel: " + TLS,

                            SessionAuthenticationConfidenceLevel = 10,
                            SessionTypeConfidenceLevel = 10,
                            SessionResponseServerConfidenceLevel = 10
                        };

                        var sessionFlagsJson200 = JsonConvert.SerializeObject(sessionFlags200);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(session, sessionFlagsJson200);
                        break;
                    default:
                        var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                        {
                            SectionTitle = "Broad Logic Checks",
                            UIBackColour = "Orange",
                            UITextColour = "Black",

                            SessionType = "Connect Tunnel: " + TLS,
                            ResponseServer = "Connect Tunnel",
                            ResponseAlert = "Connect Tunnel",
                            ResponseComments = "This is an encrypted tunnel. If all or most of the sessions are connect tunnels "
                            + "the sessions collected did not have decryption enabled. Setup Fiddler to 'Decrypt HTTPS traffic', click Tools -> Options -> HTTPS tab."
                            + "<p>If in any doubt see instructions at https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/DecryptHTTPS. </p>",

                            Authentication = "Connect Tunnel: " + TLS,

                            SessionAuthenticationConfidenceLevel = 5,
                            SessionTypeConfidenceLevel = 5,
                            SessionResponseServerConfidenceLevel = 5
                        };

                        var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                        SessionFlagHandler.Instance.UpdateSessionFlagJson(session, sessionFlagsJson);
                        break;
                }
            }
        }
    }
}
