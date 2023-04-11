using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_429
    {
        internal Session session { get; set; }
        public void HTTP_429_Too_Many_Requests(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 429: Too Many Requests.
            //
            session["ui-backcolor"] = Preferences.HTMLColourOrange;
            session["ui-color"] = "black";
            session["X-SessionType"] = "HTTP 429 Too Many Requests";

            session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 429 Too Many Requests</span></b>";
            session["X-ResponseComments"] = "These responses need to be taken into context with the rest of the sessions in the trace. " +
                "A small number is probably not an issue, larger numbers of these could be cause for concern.";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 429 Too many requests.");

            session["X-ResponseCodeDescription"] = "429 Too Many Requests (RFC 6585)";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
