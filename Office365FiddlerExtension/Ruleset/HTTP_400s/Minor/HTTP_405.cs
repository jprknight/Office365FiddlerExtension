using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_405 : ActivationService
    {
        private static HTTP_405 _instance;

        public static HTTP_405 Instance => _instance ?? (_instance = new HTTP_405());

        public void HTTP_405_Method_Not_Allowed(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 405 Method not allowed.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "405 Method Not Allowed");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 405: Method Not Allowed</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Method Not Allowed");
            // REVIEW THIS -- Add some more comments.

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}