using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_524 : ActivationService
    {
        private static HTTP_524 _instance;

        public static HTTP_524 Instance => _instance ?? (_instance = new HTTP_524());

        public void HTTP_524_A_Timeout_Occurred(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 524 Cloudflare A Timeout Occurred");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Gray");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "524 Cloudflare A Timeout Occurred");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 524 Cloudflare A Timeout Occurred.");
            GetSetSessionFlags.Instance.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}