using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_430
    {
        internal Session session { get; set; }
        public void HTTP_430_Request_Header_Feilds_Too_Large(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 430 Request Header Fields Too Large (Shopify).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 430 Request Header Fields Too Large (Shopify).");

            session["X-ResponseCodeDescription"] = "430 Request Header Fields Too Large (Shopify)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
