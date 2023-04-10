using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_201 : ActivationService
    {

        public void HTTP_201_Created(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 201: Created.
            //
            session["ui-backcolor"] = Preferences.HTMLColourGreen;
            session["ui-color"] = "black";

            session["X-ResponseAlert"] = "HTTP 201 Created.";
            session["X-ResponseComments"] = Preferences.GetStrNoKnownIssue();

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 201 Created.");

            session["X-ResponseCodeDescription"] = "201 Created";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
