using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_430
    {
        internal Session session { get; set; }

        private static HTTP_430 _instance;

        public static HTTP_430 Instance => _instance ?? (_instance = new HTTP_430());

        public void HTTP_430_Request_Header_Feilds_Too_Large(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 430 Request Header Fields Too Large (Shopify).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_430s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "430 Request Header Fields Too Large (Shopify)",
                ResponseCodeDescription = "430 Request Header Fields Too Large (Shopify)",
                ResponseAlert = "HTTP 430 Request Header Fields Too Large (Shopify).",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}