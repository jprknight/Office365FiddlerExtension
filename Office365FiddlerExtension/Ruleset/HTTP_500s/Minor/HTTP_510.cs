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
    class HTTP_510 : ActivationService
    {
        private static HTTP_510 _instance;

        public static HTTP_510 Instance => _instance ?? (_instance = new HTTP_510());

        public void HTTP_510_Not_Extended(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 510 Not Extended (RFC 2774).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_510s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "510 Not Extended (RFC 2774)",
                ResponseCodeDescription = "510 Not Extended (RFC 2774)",
                ResponseAlert = "HTTP 510 Not Extended (RFC 2774).",
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