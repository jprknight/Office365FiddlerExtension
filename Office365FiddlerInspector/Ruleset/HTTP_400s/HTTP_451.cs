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

        public void HTTP_451_Unavailable_For_Legal_Reasons_or_IIS_Redirect(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect.");

            session["X-ResponseCodeDescription"] = "451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
