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
    class HTTP_416 : ActivationService
    {
        private static HTTP_416 _instance;

        public static HTTP_416 Instance => _instance ?? (_instance = new HTTP_416());

        public void HTTP_416_Range_Not_Satisfiable(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 416 Range Not Satisfiable (RFC 7233).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_416s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "416 Range Not Satisfiable (RFC 7233)",
                ResponseCodeDescription = "416 Range Not Satisfiable (RFC 7233)",
                ResponseAlert = "HTTP 416 Range Not Satisfiable (RFC 7233).",
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