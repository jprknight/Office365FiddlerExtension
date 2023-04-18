using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_302 : ActivationService
    {
        private static HTTP_302 _instance;

        public static HTTP_302 Instance => _instance ?? (_instance = new HTTP_302());

        public void HTTP_302_Redirect(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 302 Found / Redirect.");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Green");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "302 Redirect / Found");

            // Exchange Autodiscover redirects.
            if (this.session.uriContains("autodiscover"))
            {
                GetSetSessionFlags.Instance.SetSessionType(this.session, "Autodiscover Redirect");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:green'>Exchange Autodiscover redirect.</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This type of traffic is typically an Autodiscover redirect response from "
                    + "Exchange On-Premise sending the Outlook client to connect to Exchange Online.");
            }
            // All other HTTP 302 Redirects.
            else
            {
                GetSetSessionFlags.Instance.SetSessionType(this.session, "Redirect");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:green'>Redirect.</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Redirects within Office 365 client applications or servers are not unusual. "
                    + "The only potential downfall is too many of them. However if this happens you would normally see a too many "
                    + "redirects exception thrown as a server response.");
            }

            // Possible something more to be found, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }
    }
}