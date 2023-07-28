using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_404
    {
        internal Session session { get; set; }

        private static HTTP_404 _instance;

        public static HTTP_404 Instance => _instance ?? (_instance = new HTTP_404());

        public void HTTP_404_Not_Found(Session session)
        {
            this.session = session;

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_404s");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 404 Not found.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_404s",

                SessionType = "HTTP 404 Not Found",
                ResponseCodeDescription = "404 Not Found",
                ResponseAlert = "<b><span style='color:red'>HTTP 404 Not Found</span></b>",
                ResponseComments = "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.",

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