using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_407 : ActivationService
    {

        public void HTTP_407_Proxy_Auth_Required(Session session)
        {
            /////////////////////////////
            //
            // HTTP 407: Proxy Authentication Required.
            //
            session["ui-backcolor"] = Preferences.HTMLColourRed;
            session["ui-color"] = "black";

            session["X-SessionType"] = "HTTP 407 Proxy Auth Required";

            session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 407: Proxy Authentication Required</span></b>";
            session["X-ResponseComments"] = "<b><span style='color:red'>Proxy Authentication Required</span></b>"
                + "<p>Seeing these when investigating an Office 365 connectivity is a <b>big indicator of an issue</b>.</p>"
                + "<p>Look to engage the network or security team who is responsible for the proxy infrastructure and give them "
                + "the information from these HTTP 407 sessions to troubleshoot with.</p>"
                + "<p>Office 365 application traffic should be exempt from proxy authentication or better yet follow Microsoft's recommendation "
                + "to bypass the proxy for Office365 traffic.</p>"
                + "<p>See Microsoft 365 Connectivity Principals in <a href='https://docs.microsoft.com/en-us/microsoft-365/enterprise/microsoft-365-network-connectivity-principles?view=o365-worldwide#microsoft-365-connectivity-principles' target='_blank'>"
                + "https://docs.microsoft.com/en-us/microsoft-365/enterprise/microsoft-365-network-connectivity-principles?view=o365-worldwide#microsoft-365-connectivity-principles </a></p>";

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 407 Proxy Authentication Required.");

            session["X-ResponseCodeDescription"] = "407 Proxy Authentication Required (RFC 7235)";

            // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
            Preferences.SetSACL(session, "5");
            Preferences.SetSTCL(session, "10");
            Preferences.SetSRSCL(session, "5");
        }
    }
}
