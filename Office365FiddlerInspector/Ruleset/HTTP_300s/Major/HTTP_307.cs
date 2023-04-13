using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_307 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_307_Temporary_Redirect(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 307 On-Prem Temp Redirect - Unexpected location!");

            // Specific scenario where a HTTP 307 Temporary Redirect incorrectly send an EXO Autodiscover request to an On-Premise resource, breaking Outlook connectivity.
            if (this.session.hostname.Contains("autodiscover") &&
                (this.session.hostname.Contains("mail.onmicrosoft.com") &&
                (this.session.fullUrl.Contains("autodiscover") &&
                (this.session.ResponseHeaders["Location"] != "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml"))))
            {
                // Redirect location has been found to send the Autodiscover connection somewhere else other than'
                // Exchange Online, highlight.
                getSetSessionFlags.SetUIBackColour(this.session, "Red");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetXAuthentication(this.session, "***UNEXPECTED LOCATION***");
                getSetSessionFlags.SetSessionType(this.session, "***UNEXPECTED LOCATION***");
                getSetSessionFlags.SetXResponseServer(this.session, "***UNEXPECTED LOCATION***");

                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 307 Temporary Redirect</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "<b>Temporary Redirects have been seen to redirect Exchange Online Autodiscover "
                    + "calls back to On-Premise resources, breaking Outlook connectivity</b>. Likely cause is a local networking device. Test outside of the LAN to confirm."
                    + "<p>This session is an Autodiscover request for Exchange Online which has not been sent to "
                    + "<a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a> as expected.</p>"
                    + "<p>Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place.</p>");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
            else
            {
                // The above scenario is not seem, however Temporary Redirects are not normally expected to be seen.
                // Highlight as a warning.
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 307 Temp Redirect.");

                getSetSessionFlags.SetUIBackColour(this.session, "Orange");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetResponseCodeDescription(this.session, "307 Temporary Redirect");

                getSetSessionFlags.SetXResponseAlert(this.session, "HTTP 307 Temporary Redirect");
                getSetSessionFlags.SetXResponseComments(this.session, "Temporary Redirects have been seen to redirect Exchange Online Autodiscover calls " +
                    "back to On-Premise resources, breaking Outlook connectivity. " +
                    "<p>Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place. </p>" +
                    "<p>If this session is not for an Outlook process then the information above may not be relevant to the issue under investigation.</p>");

                // Nothing meaningful here, let further processing try to pick up something.
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "0");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "0");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "0");
            }
        }
    }
}