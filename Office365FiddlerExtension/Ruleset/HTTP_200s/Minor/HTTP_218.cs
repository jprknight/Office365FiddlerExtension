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
    class HTTP_218 : ActivationService
    {        
        private static HTTP_218 _instance;

        public static HTTP_218 Instance => _instance ?? (_instance = new HTTP_218());

        public void HTTP_218_This_Is_Fine_Apache_Web_Server(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 218 This is fine (Apache Web Server).");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_218s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "218 This is fine (Apache Web Server)",
                ResponseCodeDescription = "218 This is fine (Apache Web Server)",
                ResponseAlert = "HTTP 218 This is fine (Apache Web Server).",
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