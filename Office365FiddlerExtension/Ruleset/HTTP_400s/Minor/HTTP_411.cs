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
    class HTTP_411 : ActivationService
    {
        private static HTTP_411 _instance;

        public static HTTP_411 Instance => _instance ?? (_instance = new HTTP_411());

        public void HTTP_411_Length_Required(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 411 Length Required.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Green",
                UITextColour = "Black",

                SessionType = "",
                ResponseCodeDescription = "",
                ResponseAlert = "",
                ResponseComments = "",

                SessionAuthenticationConfidenceLevel = 10,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Gray");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "411 Length Required");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 411 Length Required.");
            GetSetSessionFlags.Instance.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}