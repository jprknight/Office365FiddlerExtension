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
    class LoopBackTunnel
    {
        internal Session session { get; set; }

        private static LoopBackTunnel _instance;

        public static LoopBackTunnel Instance => _instance ?? (_instance = new LoopBackTunnel());

        public void Run(Session session)
        {
            this.session = session;

            if (!this.session.uriContains("127.0.0.1"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Loopback Tunnel.");

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
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("BroadLogicChecks|LoopBackTunnel");

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
                sessionType = "Loopback Tunnel";
                sessionResponseCodeDescription = "Loopback Tunnel";
                sessionResonseServer = "Loopback Tunnel";
                sessionResponseAlert = "Loopback Tunnel";
                sessionResponseComments = "Seeing many or few of these? Either way these aren't typical Microsoft365 traffic sessions. "
                + "They may be an indication of a proxy client forcing traffic down a certain network path?"
                + "If there's no Microsoft365 client traffic in this Fiddler trace and it's suspected this could be a factor, "
                + "change your network, try a different machine without any proxy client / proxy configuration in place.";
                sessionAuthentication = "Loopback Tunnel";

                sessionAuthenticationConfidenceLevel = 10;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 10;
                sessionSeverity = 40;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = sessionSectionTitle,

                SessionType = sessionType,
                ResponseServer = sessionResonseServer,
                ResponseAlert = sessionResponseAlert,
                ResponseCodeDescription = sessionResponseCodeDescription,
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
