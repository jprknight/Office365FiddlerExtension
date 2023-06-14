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
    class HTTP_422 : ActivationService
    {
        private static HTTP_422 _instance;

        public static HTTP_422 Instance => _instance ?? (_instance = new HTTP_422());

        public void HTTP_422_Unprocessable_Entry(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 422 Unprocessable Entity (WebDAV; RFC 4918).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_422s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "422 Unprocessable Entity (WebDAV; RFC 4918)",
                ResponseCodeDescription = "422 Unprocessable Entity (WebDAV; RFC 4918)",
                ResponseAlert = "HTTP 422 Unprocessable Entity (WebDAV; RFC 4918).",
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