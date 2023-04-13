using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_404 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_404_Not_Found(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 404 Not found.");

            getSetSessionFlags.SetUIBackColour(this.session, "Orange");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "404 Not Found");

            getSetSessionFlags.SetSessionType(this.session, "HTTP 404 Not Found");
            getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 404 Not Found</span></b>");
            getSetSessionFlags.SetXResponseComments(this.session, "The quantity of these types of server errors need to be considered in context with what you are troubleshooting " +
                "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.");
            
            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}