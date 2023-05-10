using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_407 : ActivationService
    {
        private static HTTP_407 _instance;

        public static HTTP_407 Instance => _instance ?? (_instance = new HTTP_407());

        public void HTTP_407_Proxy_Auth_Required(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 407 Proxy Authentication Required.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "407 Proxy Authentication Required (RFC 7235)");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "HTTP 407 Proxy Auth Required");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 407: Proxy Authentication Required</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<b><span style='color:red'>Proxy Authentication Required</span></b>"
                + "<p>Seeing these when investigating an Office 365 connectivity is a <b>big indicator of an issue</b>.</p>"
                + "<p>Look to engage the network or security team who is responsible for the proxy infrastructure and give them "
                + "the information from these HTTP 407 sessions to troubleshoot with.</p>"
                + "<p>Office 365 application traffic should be exempt from proxy authentication or better yet follow Microsoft's recommendation "
                + "to bypass the proxy for Office365 traffic.</p>"
                + "<p>See Microsoft 365 Connectivity Principals in <a href='https://docs.microsoft.com/en-us/microsoft-365/enterprise/microsoft-365-network-connectivity-principles?view=o365-worldwide#microsoft-365-connectivity-principles' target='_blank'>"
                + "https://docs.microsoft.com/en-us/microsoft-365/enterprise/microsoft-365-network-connectivity-principles?view=o365-worldwide#microsoft-365-connectivity-principles </a></p>");

            // Set confidence level for Session Authentication, Session Type, and Session Response Server.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }
    }
}