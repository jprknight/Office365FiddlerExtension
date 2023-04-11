using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_403
    {
        internal Session session { get; set; }
        public void HTTP_403_Forbidden(Session session)
        {
            /////////////////////////////
            //
            //  HTTP 403: FORBIDDEN.
            //
            // Looking for the term "Access Denied" or "Access Blocked" in session response.
            // Specific scenario where a web proxy is blocking traffic from reaching the internet.
            if (session.utilFindInResponse("Access Denied", false) > 1 || session.utilFindInResponse("Access Blocked", false) > 1)
            {
                session["ui-backcolor"] = Preferences.HTMLColourRed;
                session["ui-color"] = "black";
                session["X-SessionType"] = "***WEB PROXY BLOCK***";

                session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 403 Access Denied - WEB PROXY BLOCK!</span></b>";
                session["X-ResponseComments"] = "<b><span style='color:red'>Is your firewall or web proxy blocking Outlook connectivity?</span></b> "
                    + "<p>To fire this message a HTTP 403 response code was detected and '<b><span style='color:red'>Access Denied</span></b>' was found in "
                    + "the response body.</p>"
                    + "<p>Check the WebView tab, do you see anything which indicates traffic is blocked?"
                    + "<p><b><span style='color:red'>Is there a message branded by or from "
                    + "your proxy device indiciating it blocked traffic?</span></b> "
                    + "A common scenario when first deploying Office365 / Exchange Online "
                    + "is a web proxy device blocking access to consumer webmail which can impact Outlook connectivity and potentially other Office 365 applications.</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                SessionProcessor.Instance.SetSACL(this.session, "5");
                SessionProcessor.Instance.SetSTCL(this.session, "10");
                SessionProcessor.Instance.SetSRSCL(this.session, "5");
            }
            else
            {
                // All other HTTP 403's.
                session["ui-backcolor"] = Preferences.HTMLColourRed;
                session["ui-color"] = "black";
                session["X-SessionType"] = "HTTP 403 FORBIDDEN";

                session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 403 Forbidden</span></b>";
                session["X-ResponseComments"] = "While HTTP 403's can be symptomatic of a proxy server blocking traffic, "
                    + "however the phrase 'Access Denied' was NOT detected in the response body."
                    + "<p>A small number of HTTP 403's can be seen in normal working scenarios. Check the Raw and WebView tabs to look for anything which looks suspect.</p>"
                    + "<p>If you are troubleshooting Free/Busy (Meeting availability info) or setting Out of Office messages then you may be more interested in these.</p>"
                    + "<p>See: <a href='https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd877045(v=exchg.140)' target='_blank'>"
                    + "https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd877045(v=exchg.140) </a></p>";

                // 3rd-party EWS application could not connect to Exchange Online mailbox until culture/language was set for the first time in OWA.
                if (session.fullUrl.Contains("outlook.office365.com/EWS") || session.fullUrl.Contains("outlook.office365.com/ews"))
                {
                    session["X-ResponseComments"] += "<p>EWS Scenario: If you are troubleshooting a 3rd party EWS application (using application impersonation) and the service account mailbox "
                        + "has been recently migrated into the cloud, ensure mailbox is licensed and to log into the service account mailbox for the first time using OWA at "
                        + "<a href='https://outlook.office365.com' target='_blank'>https://outlook.office365.com</a> to set the mailbox language / culture.</p>"
                        + "<p>Validate with: Get-Mailbox service-account@domain.com | FL Languages</p>"
                        + "<p>Without the language set on the mailbox, EWS will not work properly</p>";
                }

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + session.id + " HTTP 403 Forbidden.");

                // Possible something more to be found, let further processing try to pick up something.
                SessionProcessor.Instance.SetSACL(this.session, "5");
                SessionProcessor.Instance.SetSTCL(this.session, "5");
                SessionProcessor.Instance.SetSRSCL(this.session, "5");
            }

            session["X-ResponseCodeDescription"] = "403 Forbidden";
        }
    }
}
