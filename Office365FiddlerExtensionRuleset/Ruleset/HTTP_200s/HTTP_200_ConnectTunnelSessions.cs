using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_ConnectTunnelSessions
    {
        internal Session session { get; set; }

        private static HTTP_200_ConnectTunnelSessions _instance;

        public static HTTP_200_ConnectTunnelSessions Instance => _instance ?? (_instance = new HTTP_200_ConnectTunnelSessions());

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
            if (!this.session.isTunnel)
            {
                return;
            }
            
            // 11/1/2022 -- There was some old code accompanying this comment, leaving this as it might be useful information for the future.

            // Trying to check session response body for a string value using !this.Session.bHasResponse does not impact performance, but is not reliable.
            // Using this.Session.GetResponseBodyAsString().Length == 0 kills performance. Fiddler wouldn't even load with this code in place.
            // Ideally looking to do: if (this.Session.utilFindInResponse("CONNECT tunnel, through which encrypted HTTPS traffic flows", false) > 1)
            // Only works reliably when loading a SAZ file and request/response data is immediately available to do logic checks against.

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_ConnectTunnelSessions");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 10;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 10;
                sessionSeverity = 40;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s",

                SessionType = $"{LangHelper.GetString("HTTP_200_ConnectTunnel")}: TLS {ExtensionSessionFlags.TLSVersion}",
                ResponseCodeDescription = LangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseServer = LangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseAlert = LangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseComments = LangHelper.GetString("HTTP_200_ConnectTunnel_RepsonseComments"),
                Authentication = $"{LangHelper.GetString("HTTP_200_ConnectTunnel")}: TLS {ExtensionSessionFlags.TLSVersion}",

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
