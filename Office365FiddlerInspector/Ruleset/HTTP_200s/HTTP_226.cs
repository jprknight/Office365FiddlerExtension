using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_226 : ActivationService
    {

        public void HTTP_226_IM_Used(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 226 IM Used (RFC 3229).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 226 IM Used (RFC 3229).");

            session["X-ResponseCodeDescription"] = "226 IM Used (RFC 3229)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
