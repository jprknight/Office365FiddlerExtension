using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_431 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_431_Request_Header_Fields_Too_Large(Session session)
        {
            this.session = session;

            this.session["X-ResponseAlert"] = "HTTP 431 Request Header Fields Too Large (RFC 6585).";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            this.session["X-ResponseCodeDescription"] = "431 Request Header Fields Too Large (RFC 6585)";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 431 Request Header Fields Too Large (RFC 6585).");

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}