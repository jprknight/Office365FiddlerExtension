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
    class HTTP_428 : ActivationService
    {
        private static HTTP_428 _instance;

        public static HTTP_428 Instance => _instance ?? (_instance = new HTTP_428());

        public void HTTP_428_Precondition_Required(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 428 Precondition Required (RFC 6585).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_428s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "428 Precondition Required (RFC 6585)",
                ResponseCodeDescription = "428 Precondition Required (RFC 6585)",
                ResponseAlert = "HTTP 428 Precondition Required (RFC 6585).",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}