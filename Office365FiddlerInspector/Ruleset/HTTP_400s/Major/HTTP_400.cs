using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_400
    {
        internal Session session { get; set; }
        public void HTTP_400_Bad_Request(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 400: BAD REQUEST.
            //
            session["ui-backcolor"] = Preferences.HTMLColourOrange;
            session["ui-color"] = "black";
            session["X-SessionType"] = "Bad Request";

            session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 400 Bad Request</span></b>";
            session["X-ResponseComments"] = "HTTP 400: Bad Request. Seeing small numbers of these may not be an issue. However, if many are seen this should be investigated further.";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 400 Bad Request.");

            session["X-ResponseCodeDescription"] = "400 Bad Request";

            // Nothing meaningful here, let further processing try to pick up something.
            SessionProcessor.Instance.SetSACL(this.session, "0");
            SessionProcessor.Instance.SetSTCL(this.session, "0");
            SessionProcessor.Instance.SetSRSCL(this.session, "0");
        }
    }
}
