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
    class HTTP_420
    {
        internal Session session { get; set; }

        private static HTTP_420 _instance;

        public static HTTP_420 Instance => _instance ?? (_instance = new HTTP_420());

        public void HTTP_420_Method_Failure_or_Enchance_Your_Calm(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter).");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_420s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter)",
                ResponseCodeDescription = "420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter)",
                ResponseAlert = "HTTP 420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter).",
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