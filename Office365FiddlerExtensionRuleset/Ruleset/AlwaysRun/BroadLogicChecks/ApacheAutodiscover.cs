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
    class ApacheAutodiscover
    {
        internal Session session { get; set; }

        private static ApacheAutodiscover _instance;

        public static ApacheAutodiscover Instance => _instance ?? (_instance = new ApacheAutodiscover());

        public void Run(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // From a scenario where Apache Web Server found to be answering Autodiscover calls and throwing HTTP 301 & 405 responses.
            // This is typically seen on the root domain Autodiscover call made from Outlook if GetO365Explicit is not used.
            //
            if ((this.session.url.Contains("autodiscover") && (this.session.oResponse["server"].Contains("Apache"))))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Apache is answering Autodiscover requests! Investigate this first!.");

                string sessionSectionTitle;
                string sessionType;
                string sessionResponseCodeDescription;
                string sessionResonseServer;
                string sessionResponseAlert;
                string sessionResponseComments;

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("BroadLogicChecks|ApacheAutodiscover");

                    sessionSectionTitle = sessionClassificationJson.SectionTitle;
                    sessionType = sessionClassificationJson.SessionType;
                    sessionResponseCodeDescription = sessionClassificationJson.SessionResponseCodeDescription;
                    sessionResonseServer = sessionClassificationJson.SessionResponseServer;
                    sessionResponseAlert = sessionClassificationJson.SessionResponseAlert;
                    sessionResponseComments = sessionClassificationJson.SessionResponseComments;

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
                    sessionType = "***APACHE AUTODISCOVER ***";
                    sessionResponseCodeDescription = "200 OK";
                    sessionResonseServer = "!APACHE!";
                    sessionResponseAlert = "Apache is answering Autodiscover requests!";
                    sessionResponseComments = "<b><span style='color:red'>An Apache Web Server (Unix/Linux) is answering Autodiscover requests!</span></b>"
                    + "<p>This should not be happening. Consider disabling Root Domain Autodiscover lookups.</p>"
                    + "<p>See ExcludeHttpsRootDomain on </p>"
                    + "<p><a href='https://support.microsoft.com/en-us/help/2212902/unexpected-autodiscover-behavior-when-you-have-registry-settings-under' target='_blank'>"
                    + "https://support.microsoft.com/en-us/help/2212902/unexpected-autodiscover-behavior-when-you-have-registry-settings-under </a></p>"
                    + "<p>Beyond this the web administrator responsible for the server needs to stop the Apache web server from answering these requests.</p>";

                    sessionAuthenticationConfidenceLevel = 5;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 5;
                    sessionSeverity = 60;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = sessionSectionTitle,

                    SessionType = sessionType,
                    ResponseCodeDescription = sessionResponseCodeDescription,
                    ResponseServer = sessionResonseServer,
                    ResponseAlert = sessionResponseAlert,
                    ResponseComments = sessionResponseComments,                   

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
