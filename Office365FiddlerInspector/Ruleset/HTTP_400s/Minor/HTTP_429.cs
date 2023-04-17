using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_429 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_429_Too_Many_Requests(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 429 Too many requests.");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            getSetSessionFlags.SetUIBackColour(this.session, "Orange");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "429 Too Many Requests (RFC 6585)");

            getSetSessionFlags.SetSessionType(this.session, "HTTP 429 Too Many Requests");

            getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 429 Too Many Requests</span></b>");
            getSetSessionFlags.SetXResponseComments(this.session,"These responses need to be taken into context with the rest of the " + 
                "sessions in the trace. A small number is probably not an issue, larger numbers of these could be cause for concern.");

            // Nothing meaningful here, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}