using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_400 : ActivationService
    {

        public void HTTP_400_Bad_Request(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 400: BAD REQUEST.
            //
            session["ui-backcolor"] = Preferences.HTMLColourOrange;
            session["ui-color"] = "black";
            session["X-SessionType"] = "Bad Request";

            session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 400 Bad Request</span></b>";
            session["X-ResponseComments"] = "HTTP 400: Bad Request. Seeing small numbers of these may not be an issue. However, if many are seen this should be investigated further.";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 400 Bad Request.");

            session["X-ResponseCodeDescription"] = "400 Bad Request";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
