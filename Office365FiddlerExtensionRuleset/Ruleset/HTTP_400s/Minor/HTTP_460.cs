using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_460
    {
        internal Session session { get; set; }

        private static HTTP_460 _instance;

        public static HTTP_460 Instance => _instance ?? (_instance = new HTTP_460());

        public void HTTP_460_Load_Balancer_Timeout(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 460 AWS Load balancer Timeout.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_460s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "460 AWS Load balancer Timeout",
                ResponseCodeDescription = "460 AWS Load balancer Timeout",
                ResponseAlert = "HTTP 460 AWS Load balancer Timeout.",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}