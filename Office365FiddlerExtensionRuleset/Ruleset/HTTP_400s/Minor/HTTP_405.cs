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
    class HTTP_405
    {
        internal Session session { get; set; }

        private static HTTP_405 _instance;

        public static HTTP_405 Instance => _instance ?? (_instance = new HTTP_405());

        public void HTTP_405_Method_Not_Allowed(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 405 Method not allowed.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_405s",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "",
                ResponseCodeDescription = "405 Method Not Allowed",
                ResponseAlert = "<b><span style='color:red'>HTTP 405: Method Not Allowed</span></b>",
                ResponseComments = "Was there a GET when only a POST is allowed or vice-versa, or was HTTP tried when HTTPS is required?",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5,
                SessionSeverity = 50
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}