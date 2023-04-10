using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_202 : ActivationService
    {

        public void HTTP_202_Accepted(Session session)
        {
            session["ui-backcolor"] = Preferences.HTMLColourGreen;
            session["ui-color"] = "black";

            session["X-ResponseAlert"] = "202 Accepted";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 202 Accepted.");

            session["X-ResponseCodeDescription"] = "202 Accepted";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
