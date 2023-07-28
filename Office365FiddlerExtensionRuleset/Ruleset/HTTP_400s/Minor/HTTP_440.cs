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
    class HTTP_440
    {
        internal Session session { get; set; }

        private static HTTP_440 _instance;

        public static HTTP_440 Instance => _instance ?? (_instance = new HTTP_440());

        public void HTTP_440_IIS_Login_Timeout(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 440 IIS Login Time-out.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_440s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "440 IIS Login Time-out",
                ResponseCodeDescription = "440 IIS Login Time-out",
                ResponseAlert = "HTTP 440 IIS Login Time-out.",
                ResponseComments = SessionFlagService.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}