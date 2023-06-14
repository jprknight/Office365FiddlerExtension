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
    class HTTP_449 : ActivationService
    {
        private static HTTP_449 _instance;

        public static HTTP_449 Instance => _instance ?? (_instance = new HTTP_449());

        public void HTTP_449_IIS_Retry_With(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 449 IIS Retry With.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_449s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "449 IIS Retry With",
                ResponseCodeDescription = "449 IIS Retry With",
                ResponseAlert = "HTTP 449 IIS Retry With.",
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