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
    class HTTP_499
    {
        internal Session session { get; set; }

        private static HTTP_499 _instance;

        public static HTTP_499 Instance => _instance ?? (_instance = new HTTP_499());

        public void HTTP_499_Token_Required_or_Client_Closed_Request(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 499 Token Required (Esri) or nginx Client Closed Request.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_499s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "499 Token Required (Esri) or nginx Client Closed Request",
                ResponseCodeDescription = "499 Token Required (Esri) or nginx Client Closed Request",
                ResponseAlert = "HTTP 499 Token Required (Esri) or nginx Client Closed Request.",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}