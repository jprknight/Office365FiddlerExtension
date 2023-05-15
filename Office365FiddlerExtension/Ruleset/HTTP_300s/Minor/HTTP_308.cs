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
    class HTTP_308 : ActivationService
    {
        private static HTTP_308 _instance;

        public static HTTP_308 Instance => _instance ?? (_instance = new HTTP_308());

        public void HTTP_308_Permenant_Redirect(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 308 Permanent Redirect (RFC 7538).");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_308s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "HTTP 308 Permanent Redirect (RFC 7538)",
                ResponseCodeDescription = "HTTP 308 Permanent Redirect (RFC 7538)",
                ResponseAlert = "HTTP 308 Permanent Redirect (RFC 7538).",
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