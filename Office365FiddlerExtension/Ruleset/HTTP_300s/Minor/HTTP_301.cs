using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_301 : ActivationService
    {
        private static HTTP_301 _instance;

        public static HTTP_301 Instance => _instance ?? (_instance = new HTTP_301());

        public void HTTP_301_Permanently_Moved(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 301 Moved Permanently.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "301 Moved Permanently");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 301 Moved Permanently");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Nothing of concern here at this time.");

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}