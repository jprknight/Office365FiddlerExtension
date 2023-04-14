using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_418 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_418_Im_A_Teapot(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 418 I'm a teapot (RFC 2324, RFC 7168).");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            getSetSessionFlags.SetUIBackColour(this.session, "Gray");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "418 I'm a teapot (RFC 2324, RFC 7168)");

            getSetSessionFlags.SetXResponseAlert(this.session, "HTTP 418 I'm a teapot (RFC 2324, RFC 7168).");
            getSetSessionFlags.SetXResponseCommentsNoKnownIssue(this.session);

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}