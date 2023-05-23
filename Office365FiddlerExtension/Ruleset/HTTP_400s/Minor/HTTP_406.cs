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
    class HTTP_406 : ActivationService
    {
        private static HTTP_406 _instance;

        public static HTTP_406 Instance => _instance ?? (_instance = new HTTP_406());

        public void HTTP_406_Not_Acceptable(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 406 Not Acceptable.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_406s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "406 Not Acceptable",
                ResponseCodeDescription = "406 Not Acceptable",
                ResponseAlert = "HTTP 406 Not Acceptable.",
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