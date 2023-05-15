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
    class HTTP_306 : ActivationService
    {
        private static HTTP_306 _instance;

        public static HTTP_306 Instance => _instance ?? (_instance = new HTTP_306());

        public void HTTP_306_Switch_Proxy(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 306 Switch Proxy.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_306s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "306 Switch Proxy",
                ResponseCodeDescription = "306 Switch Proxy",
                ResponseAlert = "306 Switch Proxy.",
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