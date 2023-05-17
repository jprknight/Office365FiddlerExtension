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
    class HTTP_505 : ActivationService
    {
        private static HTTP_505 _instance;

        public static HTTP_505 Instance => _instance ?? (_instance = new HTTP_505());

        public void HTTP_505_HTTP_Version_Not_Supported(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 505 HTTP Version Not Supported.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_505s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "505 HTTP Version Not Supported",
                ResponseCodeDescription = "505 HTTP Version Not Supported",
                ResponseAlert = "HTTP 505 HTTP Version Not Supported.",
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