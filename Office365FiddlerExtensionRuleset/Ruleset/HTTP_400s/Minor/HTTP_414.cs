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
    class HTTP_414
    {
        internal Session session { get; set; }

        private static HTTP_414 _instance;

        public static HTTP_414 Instance => _instance ?? (_instance = new HTTP_414());

        public void HTTP_414_URI_Too_Long(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 414 URI Too Long (RFC 7231).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_414s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "414 URI Too Long (RFC 7231)",
                ResponseCodeDescription = "414 URI Too Long (RFC 7231)",
                ResponseAlert = "HTTP 414 URI Too Long (RFC 7231).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}