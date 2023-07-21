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
    class HTTP_421
    {
        internal Session session { get; set; }

        private static HTTP_421 _instance;

        public static HTTP_421 Instance => _instance ?? (_instance = new HTTP_421());

        public void HTTP_421_Misdirected_Request(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 421 Misdirected Request (RFC 7540).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_421s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "421 Misdirected Request (RFC 7540)",
                ResponseCodeDescription = "421 Misdirected Request (RFC 7540)",
                ResponseAlert = "HTTP 421 Misdirected Request (RFC 7540).",
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