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
    class HTTP_414 : ActivationService
    {
        private static HTTP_414 _instance;

        public static HTTP_414 Instance => _instance ?? (_instance = new HTTP_414());

        public void HTTP_414_URI_Too_Long(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 414 URI Too Long (RFC 7231).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_414s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "414 URI Too Long (RFC 7231)",
                ResponseCodeDescription = "414 URI Too Long (RFC 7231)",
                ResponseAlert = "HTTP 414 URI Too Long (RFC 7231).",
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