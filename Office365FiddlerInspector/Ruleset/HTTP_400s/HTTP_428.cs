using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_428 : ActivationService
    {

        public void HTTP_428_Precondition_Required(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 428 Precondition Required (RFC 6585).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 428 Precondition Required (RFC 6585).");

            session["X-ResponseCodeDescription"] = "428 Precondition Required (RFC 6585)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
