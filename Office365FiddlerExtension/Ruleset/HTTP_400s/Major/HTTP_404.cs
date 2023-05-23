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
    class HTTP_404 : ActivationService
    {
        private static HTTP_404 _instance;

        public static HTTP_404 Instance => _instance ?? (_instance = new HTTP_404());

        public void HTTP_404_Not_Found(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 404 Not found.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "HTTP 404 Not Found",
                ResponseCodeDescription = "404 Not Found",
                ResponseAlert = "<b><span style='color:red'>HTTP 404 Not Found</span></b>",
                ResponseComments = "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.",

                SessionAuthenticationConfidenceLevel = 0,
                SessionTypeConfidenceLevel = 0,
                SessionResponseServerConfidenceLevel = 0
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}