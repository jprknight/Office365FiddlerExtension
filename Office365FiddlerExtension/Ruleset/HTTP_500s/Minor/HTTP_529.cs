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
    class HTTP_529 : ActivationService
    {
        private static HTTP_529 _instance;

        public static HTTP_529 Instance => _instance ?? (_instance = new HTTP_529());

        public void HTTP_529_Site_Is_Overloaded(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 529 Site is overloaded.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_529s",
                UIBackColour = "Gray",
                UITextColour = "Black",

                SessionType = "529 Site is overloaded",
                ResponseCodeDescription = "529 Site is overloaded",
                ResponseAlert = "HTTP 529 Site is overloaded.",
                ResponseComments = SessionFlagHandler.Instance.ResponseCommentsNoKnownIssue(),

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}