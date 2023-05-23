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
    class HTTP_301 : ActivationService
    {
        private static HTTP_301 _instance;

        public static HTTP_301 Instance => _instance ?? (_instance = new HTTP_301());

        public void HTTP_301_Permanently_Moved(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 301 Moved Permanently.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_301s",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "301 Moved Permanently",
                ResponseCodeDescription = "301 Moved Permanently",
                ResponseAlert = "HTTP 301 Moved Permanently",
                ResponseComments = SessionProcessor.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}