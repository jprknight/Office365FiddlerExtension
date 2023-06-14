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
    class HTTP_507 : ActivationService
    {
        private static HTTP_507 _instance;

        public static HTTP_507 Instance => _instance ?? (_instance = new HTTP_507());

        public void HTTP_507_Insufficient_Storage(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 507 Insufficient Storage (WebDAV; RFC 4918).");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_507s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "507 Insufficient Storage (WebDAV; RFC 4918)",
                ResponseCodeDescription = "507 Insufficient Storage (WebDAV; RFC 4918)",
                ResponseAlert = "HTTP 507 Insufficient Storage (WebDAV; RFC 4918).",
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