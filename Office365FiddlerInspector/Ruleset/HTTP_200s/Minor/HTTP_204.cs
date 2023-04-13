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
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_204_No_Content(Session session)
        {
            this.session = session;

            // Somewhat highlight these, they have been seen in Fiddler traces while troubleshooting Microsoft 365 issues.
            // Though they don't appear to directly contribute to anything of interest.
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 204 No content.");

            getSetSessionFlags.SetUIBackColour(this.session, "Orange");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "204 No Content");

            getSetSessionFlags.SetXResponseAlert(this.session, "HTTP 204 No Content.");
            getSetSessionFlags.SetXResponseComments(this.session, "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.");

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}