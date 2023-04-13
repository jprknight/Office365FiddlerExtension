using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_451 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.");

            this.session["X-ResponseCodeDescription"] = "451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect";

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}