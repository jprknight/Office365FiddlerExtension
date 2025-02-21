using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
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

            var ExtensionSessionFlags = RulesetSessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 10;
            int sessionTypeConfidenceLevelFallback = 10;
            int sessionResponseServerConfidenceLevelFallback = 10;
            int sessionSeverityFallback = 40;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_ConnectTunnelSessions");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s",

                SessionType = $"{RulesetLangHelper.GetString("HTTP_200_ConnectTunnel")}: TLS {ExtensionSessionFlags.TLSVersion}",
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseServer = RulesetLangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_ConnectTunnel"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_ConnectTunnel_RepsonseComments"),
                Authentication = $"{RulesetLangHelper.GetString("HTTP_200_ConnectTunnel")}: TLS {ExtensionSessionFlags.TLSVersion}",

                SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                    sessionAuthenticationConfidenceLevelFallback),

                SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                    sessionTypeConfidenceLevelFallback),

                SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                    sessionResponseServerConfidenceLevelFallback),

                SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                    sessionSeverityFallback)
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
