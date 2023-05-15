using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_400 : ActivationService
    {
        private static HTTP_400 _instance;

        public static HTTP_400 Instance => _instance ?? (_instance = new HTTP_400());

        public void HTTP_400_Bad_Request(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 400 Bad Request.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_400s",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "400 Bad Request",
                ResponseCodeDescription = "400 Bad Request",
                ResponseAlert = "<b><span style='color:red'>HTTP 400 Bad Request</span></b>",
                ResponseComments = "HTTP 400: Bad Request. Seeing small numbers of these may not be an issue. However, if many are seen this should be investigated further.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}