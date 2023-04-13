using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_430 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_430_Request_Header_Feilds_Too_Large(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 430 Request Header Fields Too Large (Shopify).";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 430 Request Header Fields Too Large (Shopify).");

            this.session["X-ResponseCodeDescription"] = "430 Request Header Fields Too Large (Shopify)";

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}