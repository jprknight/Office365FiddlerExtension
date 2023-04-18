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
        private static HTTP_404 _instance;

        public static HTTP_404 Instance => _instance ?? (_instance = new HTTP_404());

        public void HTTP_404_Not_Found(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 404 Not found.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "404 Not Found");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "HTTP 404 Not Found");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 404 Not Found</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "The quantity of these types of server errors need to be considered in context with what you are troubleshooting " +
                "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.");
            
            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}