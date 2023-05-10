using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_430 : ActivationService
    {
        private static HTTP_430 _instance;

        public static HTTP_430 Instance => _instance ?? (_instance = new HTTP_430());

        public void HTTP_430_Request_Header_Feilds_Too_Large(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 430 Request Header Fields Too Large (Shopify).");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Gray");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "430 Request Header Fields Too Large (Shopify)");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 430 Request Header Fields Too Large (Shopify).");
            GetSetSessionFlags.Instance.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}