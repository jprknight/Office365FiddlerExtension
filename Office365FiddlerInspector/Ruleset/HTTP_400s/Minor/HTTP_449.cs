using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_449 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();
        public void HTTP_449_IIS_Retry_With(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 449 IIS Retry With.";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            this.session["X-ResponseCodeDescription"] = "449 IIS Retry With";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 449 IIS Retry With");

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}