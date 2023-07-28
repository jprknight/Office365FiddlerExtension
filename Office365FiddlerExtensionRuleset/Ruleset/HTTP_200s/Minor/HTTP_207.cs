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
    class HTTP_207
    {
        internal Session session { get; set; }

        private static HTTP_207 _instance;

        public static HTTP_207 Instance => _instance ?? (_instance = new HTTP_207());

        public void HTTP_207_Multi_Status(Session session)
        {
            this.session = session;

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP207s");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 207 Multi-Status (WebDAV; RFC 4918).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_207s",

                SessionType = "Multi-Status",
                ResponseCodeDescription = "207 Multi-Status (WebDAV; RFC 4918)",
                ResponseAlert = "207 Multi-Status (WebDAV; RFC 4918).",
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