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
    class HTTP_423
    {
        internal Session session { get; set; }

        private static HTTP_423 _instance;

        public static HTTP_423 Instance => _instance ?? (_instance = new HTTP_423());

        public void HTTP_423_Locked(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 423 Locked (WebDAV; RFC 4918).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_423s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "423 Locked (WebDAV; RFC 4918)",
                ResponseCodeDescription = "423 Locked (WebDAV; RFC 4918)",
                ResponseAlert = "HTTP 423 Locked (WebDAV; RFC 4918).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}