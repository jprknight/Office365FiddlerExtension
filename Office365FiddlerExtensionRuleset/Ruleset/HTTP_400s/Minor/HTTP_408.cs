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
    class HTTP_408
    {
        internal Session session { get; set; }

        private static HTTP_408 _instance;

        public static HTTP_408 Instance => _instance ?? (_instance = new HTTP_408());

        public void HTTP_408_Request_Timeout(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 408 Request Timeout.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_408s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "408 Request Timeout",
                ResponseCodeDescription = "408 Request Timeout",
                ResponseAlert = "HTTP 408 Request Timeout.",
                ResponseComments = "",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}