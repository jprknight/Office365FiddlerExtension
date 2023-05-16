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
    class HTTP_430 : ActivationService
    {
        private static HTTP_430 _instance;

        public static HTTP_430 Instance => _instance ?? (_instance = new HTTP_430());

        public void HTTP_430_Request_Header_Feilds_Too_Large(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 430 Request Header Fields Too Large (Shopify).");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_430s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "430 Request Header Fields Too Large (Shopify)",
                ResponseCodeDescription = "430 Request Header Fields Too Large (Shopify)",
                ResponseAlert = "HTTP 430 Request Header Fields Too Large (Shopify).",
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