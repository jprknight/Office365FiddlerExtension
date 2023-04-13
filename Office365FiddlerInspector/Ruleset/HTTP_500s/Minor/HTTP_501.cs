using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_501 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_501_Not_Implemented(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 501 Not Implemented.";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 501 Not Implemented.");

            this.session["X-ResponseCodeDescription"] = "501 Not Implemented";

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}