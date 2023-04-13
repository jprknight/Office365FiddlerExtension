using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_463 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();
     
        public void HTTP_463_X_Forwarded_For_Header(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 463 AWS X-Forwarded-For Header > 30 IP addresses.";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 463 AWS X-Forwarded-For Header > 30 IP addresses");

            this.session["X-ResponseCodeDescription"] = "463 AWS X-Forwarded-For Header > 30 IP addresses";

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}