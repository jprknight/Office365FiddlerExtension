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
    class HTTP_429 : ActivationService
    {
        private static HTTP_429 _instance;

        public static HTTP_429 Instance => _instance ?? (_instance = new HTTP_429());

        public void HTTP_429_Too_Many_Requests(Session session)
        {
            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 429 Too many requests.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_429s",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "429 Too Many Requests (RFC 6585)",
                ResponseCodeDescription = "429 Too Many Requests (RFC 6585)",
                ResponseAlert = "<b><span style='color:red'>HTTP 429 Too Many Requests</span></b>",
                ResponseComments = "These responses need to be taken into context with the rest of the "
                + "sessions in the trace. A small number is probably not an issue, larger numbers of these could be cause for concern.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}