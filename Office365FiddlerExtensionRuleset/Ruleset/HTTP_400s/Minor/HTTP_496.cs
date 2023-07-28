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
    class HTTP_496
    {
        internal Session session { get; set; }

        private static HTTP_496 _instance;

        public static HTTP_496 Instance => _instance ?? (_instance = new HTTP_496());

        public void HTTP_496_SSL_Certificate_Required(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 496 nginx SSL Certificate Required.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_496s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "496 nginx SSL Certificate Required",
                ResponseCodeDescription = "496 nginx SSL Certificate Required",
                ResponseAlert = "HTTP 496 nginx SSL Certificate Required.",
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