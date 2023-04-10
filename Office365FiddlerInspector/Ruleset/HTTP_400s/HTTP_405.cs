using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_405 : ActivationService
    {

        public void HTTP_405_Method_Not_Allowed(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 405: Method Not Allowed.
            //
            session["ui-backcolor"] = Preferences.HTMLColourOrange;
            session["ui-color"] = "black";

            session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 405: Method Not Allowed</span></b>";
            session["X-ResponseComments"] = "Method Not Allowed";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 405 Method not allowed.");

            session["X-ResponseCodeDescription"] = "405 Method Not Allowed";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
