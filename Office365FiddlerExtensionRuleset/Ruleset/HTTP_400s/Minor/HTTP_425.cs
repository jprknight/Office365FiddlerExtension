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
    class HTTP_425
    {
        internal Session session { get; set; }

        private static HTTP_425 _instance;

        public static HTTP_425 Instance => _instance ?? (_instance = new HTTP_425());

        public void HTTP_425_Too_Early(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 425 Too Early (RFC 8470).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_425s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "425 Too Early (RFC 8470)",
                ResponseCodeDescription = "425 Too Early (RFC 8470)",
                ResponseAlert = "HTTP 425 Too Early (RFC 8470).",
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