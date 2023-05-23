using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_506 : ActivationService
    {
        private static HTTP_506 _instance;

        public static HTTP_506 Instance => _instance ?? (_instance = new HTTP_506());

        public void HTTP_506_Variant_Also_Negociates(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 506 Variant Also Negotiates (RFC 2295).");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_506s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "506 Variant Also Negotiates (RFC 2295)",
                ResponseCodeDescription = "506 Variant Also Negotiates (RFC 2295)",
                ResponseAlert = "HTTP 506 Variant Also Negotiates (RFC 2295).",
                ResponseComments = SessionProcessor.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}