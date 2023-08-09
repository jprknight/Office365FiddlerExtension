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
    class FiddlerUpdateSessions
    {
        internal Session session { get; set; }

        private static FiddlerUpdateSessions _instance;

        public static FiddlerUpdateSessions Instance => _instance ?? (_instance = new FiddlerUpdateSessions());

        public void Run(Session session)
        {
            this.session = session;

            if (this.session.hostname == "www.fiddler2.com" && this.session.uriContains("UpdateCheck.aspx"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Fiddler Updates.");

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
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("BroadLogicChecks|FiddlerUpdateSessions");
                    
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
                    sessionType = "Fiddler Update Check";
                    sessionResponseCodeDescription = "Fiddler Update Check";
                    sessionResonseServer = "Fiddler Update Check";
                    sessionResponseAlert = "Fiddler Update Check";
                    sessionResponseComments = "This is Fiddler itself checking for updates.";
                    sessionAuthentication = "Fiddler Update Check";

                    sessionAuthenticationConfidenceLevel = 10;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 10;
                    sessionSeverity = 10;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = sessionSectionTitle,

                    SessionType = sessionType,
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
