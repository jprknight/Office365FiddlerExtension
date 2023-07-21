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
    class HTTP_506
    {
        internal Session session { get; set; }

        private static HTTP_506 _instance;

        public static HTTP_506 Instance => _instance ?? (_instance = new HTTP_506());

        public void HTTP_506_Variant_Also_Negociates(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 506 Variant Also Negotiates (RFC 2295).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_506s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "506 Variant Also Negotiates (RFC 2295)",
                ResponseCodeDescription = "506 Variant Also Negotiates (RFC 2295)",
                ResponseAlert = "HTTP 506 Variant Also Negotiates (RFC 2295).",
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