using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_415 : ActivationService
    {
        private static HTTP_415 _instance;

        public static HTTP_415 Instance => _instance ?? (_instance = new HTTP_415());

        public void HTTP_415_UnSupported_Media_Type(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} HTTP 415 Unsupported Media Type (RFC 7231).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "415 Unsupported Media Type (RFC 7231)",
                ResponseCodeDescription = "415 Unsupported Media Type (RFC 7231)",
                ResponseAlert = "HTTP 415 Unsupported Media Type (RFC 7231).",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}