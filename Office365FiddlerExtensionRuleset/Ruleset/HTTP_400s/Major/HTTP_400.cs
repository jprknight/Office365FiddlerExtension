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
    class HTTP_400
    {
        internal Session session { get; set; }

        private static HTTP_400 _instance;

        public static HTTP_400 Instance => _instance ?? (_instance = new HTTP_400());

        public void HTTP_400_Bad_Request(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 400 Bad Request.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_400s",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "400 Bad Request",
                ResponseCodeDescription = "400 Bad Request",
                ResponseAlert = "<b><span style='color:red'>HTTP 400 Bad Request</span></b>",
                ResponseComments = "HTTP 400: Bad Request. Seeing small numbers of these may not be an issue. However, if many are seen this should be investigated further.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}