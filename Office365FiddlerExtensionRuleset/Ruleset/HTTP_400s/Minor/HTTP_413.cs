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
    class HTTP_413
    {
        internal Session session { get; set; }

        private static HTTP_413 _instance;

        public static HTTP_413 Instance => _instance ?? (_instance = new HTTP_413());

        public void HTTP_413_Payload_Too_Large(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 413 Payload Too Large (RFC 7231).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_413s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "413 Payload Too Large (RFC 7231)",
                ResponseCodeDescription = "413 Payload Too Large (RFC 7231)",
                ResponseAlert = "HTTP 413 Payload Too Large (RFC 7231).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}