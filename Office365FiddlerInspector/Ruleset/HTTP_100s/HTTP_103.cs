using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_103 : ActivationService
    {
        private static HTTP_103 _instance;

        public static HTTP_103 Instance => _instance ?? (_instance = new HTTP_103());

        public void HTTP_103_Checkpoint(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 103 Checkpoint.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Gray");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "103 Checkpoint");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 103 Checkpoint.");
            GetSetSessionFlags.Instance.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}