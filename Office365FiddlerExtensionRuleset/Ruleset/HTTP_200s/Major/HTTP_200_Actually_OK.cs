using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_Actually_OK
    {
        internal Session session { get; set; }

        private static HTTP_200_Actually_OK _instance;

        public static HTTP_200_Actually_OK Instance => _instance ?? (_instance = new HTTP_200_Actually_OK());

        public void Run(Session session)
        {
            this.session = session;

            if (SessionWordSearch.Instance.Search(this.session, "Error") == 0 &&
                SessionWordSearch.Instance.Search(this.session, "failed") == 0 &&
                SessionWordSearch.Instance.Search(this.session, "exception") == 0)
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Actually_OK");

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 OK");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_No_Lurking_Errors",

                    SessionType = "200 OK",
                    ResponseCodeDescription = "200 OK",
                    ResponseAlert = "HTTP 200 OK, with no errors, failed, or exceptions found.",
                    ResponseComments = "HTTP 200 OK, with no errors, failed, or exceptions found.",

                    SessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionClassificationJson.SessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}
