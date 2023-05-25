using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_403 : ActivationService
    {
        private static HTTP_403 _instance;

        public static HTTP_403 Instance => _instance ?? (_instance = new HTTP_403());

        public void HTTP_403_Forbidden_Proxy_Block(Session session)
        {
            this.session = session;

            // Looking for the term "Access Denied" or "Access Blocked" in session response.
            // Specific scenario where a web proxy is blocking traffic from reaching the internet.
            if (this.session.utilFindInResponse("Access Denied", false) > 1 || session.utilFindInResponse("Access Blocked", false) > 1)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");

                var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_403s_Proxy_Block",
                    UIBackColour = "Red",
                    UITextColour = "Black",

                    SessionType = "***WEB PROXY BLOCK***",
                    ResponseCodeDescription = "HTTP 403 Forbidden; !WEB PROXY BLOCK!",
                    ResponseAlert = "<b><span style='color:red'>HTTP 403 Access Denied - WEB PROXY BLOCK!</span></b>",
                    ResponseComments = "<b><span style='color:red'>Is your firewall or web proxy blocking Outlook connectivity?</span></b> "
                    + "<p>To fire this message a HTTP 403 response code was detected and '<b><span style='color:red'>Access Denied</span></b>' was found in "
                    + "the response body.</p>"
                    + "<p>Check the WebView tab, do you see anything which indicates traffic is blocked?"
                    + "<p><b><span style='color:red'>Is there a message branded by or from "
                    + "your proxy device indiciating it blocked traffic?</span></b> "
                    + "A common scenario when first deploying Office365 / Exchange Online "
                    + "is a web proxy device blocking access to consumer webmail which can impact Outlook connectivity and potentially other Office 365 applications.</p>",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set(Session session)
        {
            // 3rd-party EWS application could not connect to Exchange Online mailbox until culture/language was set for the first time in OWA.

            this.session = session;

            if (this.session.fullUrl.Contains("outlook.office365.com/EWS") || this.session.fullUrl.Contains("outlook.office365.com/ews"))
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 403 Forbidden. EWS Language not set on mailbox.");
                
                var sessionFlags_HTTP403_EWS = new SessionFlagProcessor.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_403s_EWS_Mailbox_Language",
                    UIBackColour = "Red",
                    UITextColour = "Black",

                    SessionType = "HTTP 403 EWS FORBIDDEN",
                    ResponseCodeDescription = "403 EWS Forbidden",
                    ResponseAlert = "<b><span style='color:red'>HTTP 403 Forbidden</span></b>",
                    ResponseComments = "<p>If you are troubleshooting a 3rd party EWS application (using application impersonation) and the service account mailbox "
                    + "has been recently migrated into the cloud, ensure mailbox is licensed and to log into the service account mailbox for the first time using OWA at "
                    + "<a href='https://outlook.office365.com' target='_blank'>https://outlook.office365.com</a> to set the mailbox language / culture.</p>"
                    + "<p>Validate with: Get-Mailbox service-account@domain.com | FL Languages</p>"
                    + "<p>Without the language set on the mailbox, EWS will not work properly.</p>",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5
                };
                var sessionFlagsJson_HTTP403_EWS = JsonConvert.SerializeObject(sessionFlags_HTTP403_EWS);
                SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson_HTTP403_EWS);
            }
        }

        public void HTTP_403_Forbidden_Everything_Else(Session session)
        {
            // All other HTTP 403's.

            this.session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.session.id} HTTP 403 Forbidden.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_403s_Generic",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "HTTP 403 FORBIDDEN",
                ResponseCodeDescription = "403 Forbidden",
                ResponseAlert = "<b><span style='color:red'>HTTP 403 Forbidden</span></b>",
                ResponseComments = "While HTTP 403's can be symptomatic of a proxy server blocking traffic, "
                + "however the phrase 'Access Denied' was not detected in the response body."
                + "<p>A small number of HTTP 403's can be seen in normal working scenarios. Check the Raw and WebView tabs to look for anything which looks suspect.</p>"
                + "<p>If you are troubleshooting Free/Busy (Meeting availability info) or setting Out of Office messages then you may be more interested in these.</p>"
                + "<p>See: <a href='https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd877045(v=exchg.140)' target='_blank'>"
                + "https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd877045(v=exchg.140) </a></p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);          
        }
    }
}