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
    class HTTP_416
    {
        internal Session session { get; set; }

        private static HTTP_416 _instance;

        public static HTTP_416 Instance => _instance ?? (_instance = new HTTP_416());

        public void HTTP_416_Range_Not_Satisfiable(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 416 Range Not Satisfiable (RFC 7233).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_416s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "416 Range Not Satisfiable (RFC 7233)",
                ResponseCodeDescription = "416 Range Not Satisfiable (RFC 7233)",
                ResponseAlert = "HTTP 416 Range Not Satisfiable (RFC 7233).",
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