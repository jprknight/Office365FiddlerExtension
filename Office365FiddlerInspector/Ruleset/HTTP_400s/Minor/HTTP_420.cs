using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_420 : ActivationService
    {
        private static HTTP_420 _instance;

        public static HTTP_420 Instance => _instance ?? (_instance = new HTTP_420());

        public void HTTP_420_Method_Failure_or_Enchance_Your_Calm(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter).");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Gray");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter)");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter).");
            GetSetSessionFlags.Instance.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}