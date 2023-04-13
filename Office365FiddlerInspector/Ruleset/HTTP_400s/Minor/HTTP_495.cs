using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_495 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_495_SSL_Certificate_Error(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 495 nginx SSL Certificate Error.";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 495 nginx SSL Certificate Error");

            this.session["X-ResponseCodeDescription"] = "495 nginx SSL Certificate Error";

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}