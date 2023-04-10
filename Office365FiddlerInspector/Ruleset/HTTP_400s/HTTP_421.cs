using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_421
    {

        public void HTTP_421_Misdirected_Request(Session session)
        {
            session["X-ResponseAlert"] = "HTTP 421 Misdirected Request (RFC 7540).";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 421 Misdirected Request (RFC 7540).");

            session["X-ResponseCodeDescription"] = "421 Misdirected Request (RFC 7540)";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
