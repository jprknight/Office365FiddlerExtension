using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_204 : ActivationService
    {

        public void HTTP_204_No_Content(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 204: No Content.
            //
            // Somewhat highlight these.
            session["ui-backcolor"] = Preferences.HTMLColourOrange;
            session["ui-color"] = "black";

            session["X-ResponseAlert"] = "HTTP 204 No Content.";
            session["X-ResponseComments"] = "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 204 No content.");

            session["X-ResponseCodeDescription"] = "204 No Content";

            // Nothing meaningful here, let further processing try to pick up something.
            Preferences.SetSACL(session, "0");
            Preferences.SetSTCL(session, "0");
            Preferences.SetSRSCL(session, "0");
        }
    }
}
