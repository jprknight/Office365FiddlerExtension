using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_400 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_400_Bad_Request(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 400 Bad Request.");

            getSetSessionFlags.SetUIBackColour(this.session, "Orange");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "400 Bad Request");

            getSetSessionFlags.SetSessionType(this.session, "Bad Request");

            getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 400 Bad Request</span></b>");
            getSetSessionFlags.SetXResponseComments(this.session, "HTTP 400: Bad Request. Seeing small numbers of these may not be an issue. However, if many are seen this should be investigated further.");

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}