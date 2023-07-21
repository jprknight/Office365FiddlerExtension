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
    class HTTP_498
    {
        internal Session session { get; set; }

        private static HTTP_498 _instance;

        public static HTTP_498 Instance => _instance ?? (_instance = new HTTP_498());

        public void HTTP_498_Invalid_Token(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 498 Invalid Token (Esri).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_498s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "498 Invalid Token (Esri)",
                ResponseCodeDescription = "498 Invalid Token (Esri)",
                ResponseAlert = "HTTP 498 Invalid Token (Esri).",
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