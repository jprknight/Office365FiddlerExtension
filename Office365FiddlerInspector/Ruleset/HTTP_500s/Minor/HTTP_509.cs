using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_509 : ActivationService
    {
        private static HTTP_509 _instance;

        public static HTTP_509 Instance => _instance ?? (_instance = new HTTP_509());

        public void HTTP_509_Bandwidth_Limit_Exceeeded(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Gray");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 509 Bandwidth Limit Exceeded (Apache Web Server/cPanel).");
            GetSetSessionFlags.Instance.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}