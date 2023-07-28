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
    class HTTP_205
    {
        internal Session session { get; set; }

        private static HTTP_205 _instance;

        public static HTTP_205 Instance => _instance ?? (_instance = new HTTP_205());

        public void HTTP_205_Reset_Content(Session session)
        {
            this.session = session;

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP205s");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 205 Reset Content.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_205s",

                SessionType = "Reset Content",
                ResponseCodeDescription = "205 Reset Content",
                ResponseAlert = "HTTP 205 Reset Content.",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

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