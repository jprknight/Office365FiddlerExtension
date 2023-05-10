using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_204 : ActivationService
    {
        private static HTTP_204 _instance;

        public static HTTP_204 Instance => _instance ?? (_instance = new HTTP_204());

        public void HTTP_204_No_Content(Session session)
        {
            this.session = session;

            // Somewhat highlight these, they have been seen in Fiddler traces while troubleshooting Microsoft 365 issues.
            // Though they don't appear to directly contribute to anything of interest.
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 204 No content.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "204 No Content");
            GetSetSessionFlags.Instance.SetSessionType(this.session, "204 No Content");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 204 No Content.");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.");

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}