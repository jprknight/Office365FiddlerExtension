using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_415 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_415_UnSupported_Media_Type(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 415 Unsupported Media Type (RFC 7231).");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            getSetSessionFlags.SetUIBackColour(this.session, "Gray");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "415 Unsupported Media Type (RFC 7231)");

            this.session["X-ResponseAlert"] = "HTTP 415 Unsupported Media Type (RFC 7231).";
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}