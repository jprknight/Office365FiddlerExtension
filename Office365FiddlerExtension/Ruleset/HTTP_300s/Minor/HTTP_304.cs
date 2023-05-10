using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_304 : ActivationService
    {
        private static HTTP_304 _instance;

        public static HTTP_304 Instance => _instance ?? (_instance = new HTTP_304());

        public void HTTP_304_Not_Modified(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 304 Not modified.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "304 Not Modified (RFC 7232)");
            GetSetSessionFlags.Instance.SetSessionType(this.session, "304 Not Modified");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "HTTP 304 Not Modified");
            GetSetSessionFlags.Instance.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}