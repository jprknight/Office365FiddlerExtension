using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_508
    {
        internal Session session { get; set; }

        private static HTTP_508 _instance;

        public static HTTP_508 Instance => _instance ?? (_instance = new HTTP_508());

        public void HTTP_508_Loop_Detected(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 508 Loop Detected (WebDAV; RFC 5842).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_508s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "508 Loop Detected (WebDAV; RFC 5842)",
                ResponseCodeDescription = "508 Loop Detected (WebDAV; RFC 5842)",
                ResponseAlert = "HTTP 508 Loop Detected (WebDAV; RFC 5842).",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}