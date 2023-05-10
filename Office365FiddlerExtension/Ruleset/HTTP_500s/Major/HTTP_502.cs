using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_502 : ActivationService
    {
        private static HTTP_502 _instance;

        public static HTTP_502 Instance => _instance ?? (_instance = new HTTP_502());

        public void HTTP_502_Bad_Gateway_Telemetry_False_Positive(Session session)
        {
            this.session = session;

            // Specific scenario on Outlook & OFffice 365 Autodiscover false positive on connections to:
            //      autodiscover.domain.onmicrosoft.com:443

            // Testing because I am finding colourisation based in the nested if statement below is not working.
            // Strangely the same HTTP 502 nested if statement logic works fine in Office365FiddlerInspector.cs to write
            // response alert and comment.
            // From further testing this seems to come down to timing, clicking the sessions as they come into Fiddler
            // I see the responsecode / response body unavailable, it then populates after a few sessions. I presume 
            // since the UI has moved on already the session cannot be colourised. 

            // On testing with loadSAZ instead this same code colourises sessions fine.

            // Altered if statements from being bested to using && to see if this inproves here.
            // This appears to be the only section in this code which has a session colourisation issue.

            /////////////////////////////
            //
            // 502.1. telemetry false positive. <Need to validate in working scenarios>
            //
            if ((this.session.oRequest["Host"] == "sqm.telemetry.microsoft.com:443") &&
                (this.session.utilFindInResponse("target machine actively refused it", false) > 1))
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 502 Bad Gateway. Telemetry False Positive.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Blue");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "502 Bad Gateway False Positive");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "False Positive");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:green'>False Positive</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Telemetry failing is unlikely the cause of significant Office 365 client issues.");
                
                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 502.2. Exchange Online DNS lookup on contoso.onmicrosoft.com, False Positive!?
            //
            // Specific scenario on Outlook and Office 365 invalid DNS lookup.
            // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
            if ((this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                (this.session.utilFindInResponse("DNS Lookup for ", false) > 1) &&
                (this.session.utilFindInResponse(" failed.", false) > 1))
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 502 Bad Gateway. EXO DNS False Positive.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Blue");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "502 Bad Gateway False Positive");

                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "False Positive");
                GetSetSessionFlags.Instance.SetSessionType(this.session, "False Positive");
                GetSetSessionFlags.Instance.SetXResponseServer(this.session, "False Positive");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:green'>False Positive</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<b><span style='color:green'>False positive on HTTP 502</span></b>. "
                    + "By design, the host only accepts connections on port 25, port 443 is not available."
                    + "<p>To validate this above lookup the record, confirm it is a MX record and attempt to connect to the MX host on ports 25 and 443.</p>");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
        }

        public void HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 502.3. Exchange Online connection to autodiscover.contoso.mail.onmicrosoft.com, False Positive!
            //
            // Specific scenario on Outlook and Office 365 invalid connection to contoso.mail.onmicrosoft.com
            // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
            if ((this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                // Too specific, it looks as though we see ConnectionRefused or The socket connection to ... failed.
                //(this.session.utilFindInResponse("ConnectionRefused ", false) > 1) &&
                (this.session.utilFindInResponse("target machine actively refused it", false) > 1))
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 502 Bad Gateway. O365 AutoD onmicrosoft.com False Positive.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Blue");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "502 Bad Gateway False Positive");

                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "False Positive");
                GetSetSessionFlags.Instance.SetSessionType(this.session, "False Positive");
                GetSetSessionFlags.Instance.SetXResponseServer(this.session, "False Positive");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "False Positive");

                string AutoDFalsePositiveDomain;
                string AutoDFalsePositiveResponseBody = this.session.GetResponseBodyAsString();
                int start = this.session.GetResponseBodyAsString().IndexOf("'");
                int end = this.session.GetResponseBodyAsString().LastIndexOf("'");
                int charcount = end - start;
                if (charcount > 0)
                {
                    AutoDFalsePositiveDomain = AutoDFalsePositiveResponseBody.Substring(start, charcount).Replace("'", "");
                }
                else
                {
                    AutoDFalsePositiveDomain = "<Domain not detected by extension>";
                }

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:green'>False Positive</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<b><span style='color:green'>False Positive</span></b>. By design Office 365 Autodiscover does not respond to "
                    + AutoDFalsePositiveDomain
                    + " on port 443. "
                    + "<p>Validate this message by confirming the Host IP (if shown) is an Office 365 Host/IP address and perform a telnet to it on port 80.</p>"
                    + "<p>If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design redirects "
                    + "requests on port 80 to <a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a>"
                    + "<p>The reason for this is Microsoft does not maintain SSL certificates for every tenant domain name registered on the .onmicrosoft.com domain."
                    + "AutoDiscover redirects to autodiscover-s.outlook.com which does accept connections on 443 and Microsoft does maintain SSL certificates for this endpoint.</p>");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
        }

        public void HTTP_502_Bad_Gateway_Vanity_Domain_M365_AutoDiscover_False_Positive(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 502.4. Vanity domain points to Office 365 autodiscover; false positive.
            //

            /*
            HTTP/1.1 502 Fiddler - Connection Failed
            Date: Mon, 12 Nov 2018 09:47:06 GMT
            Content-Type: text/html; charset=UTF-8
            Connection: close
            Cache-Control: no-cache, must-revalidate
            Timestamp: 04:47:06.295

            [Fiddler] The connection to 'autodiscover.contoso.com' failed. <br />Error: ConnectionRefused (0x274d). <br />
            System.Net.Sockets.SocketException No connection could be made because the target machine actively refused it 40.97.100.8:443                                                                                                                                                                                                                                                                                  
            */

            if ((this.session.utilFindInResponse("autodiscover.", false) > 1) &&
                (this.session.utilFindInResponse("target machine actively refused it", false) > 1))
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 502 Bad Gateway. Vanity domain AutoD False Positive.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Blue");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "502 Bad Gateway False Positive");

                GetSetSessionFlags.Instance.SetXAuthentication(this.session, "AutoD False Positive");
                GetSetSessionFlags.Instance.SetSessionType(this.session, "AutoD False Positive");
                GetSetSessionFlags.Instance.SetXResponseServer(this.session, "AutoD False Positive");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:orange'>Autodiscover Possible False Positive?</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Autoddiscover Possible False Positive. By design Office 365 endpoints such as "
                    + "autodiscover.contoso.onmicrosoft.com do not respond on port 443. "
                    + "<p>Validate this message by confirming this is an Office 365 Host/IP address and perform a telnet to it on port 80.</p>"
                    + "<p>If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design "
                    + "redirects requests on port 80 to <a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' "
                    + "target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a>");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
        }

        public void HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 502.5. Anything else Exchange Autodiscover.
            //
            if ((this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                        (this.session.utilFindInResponse("autodiscover", false) > 1))
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 502 Bad Gateway. Exchange Autodiscover.");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

                GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "502 Bad Gateway");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "!AUTODISCOVER!");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>AUTODISCOVER</span></b>");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This AutoDiscover request was refused by the server it was sent to. Check the raw tab for further details.");

                // Possible something more to be found, let further processing try to pick up something.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_502_Bad_Gateway_Anything_Else(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 502.99. Everything else.
            //

            // Pick up any other 502 Bad Gateway call it out.

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 502 Bad Gateway (99).");

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "502 Bad Gateway");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "!Bad Gateway!");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 502 Bad Gateway</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "Potential to cause the issue you are investigating. "
                + "Do you see expected responses beyond this session in the trace? Is the Host IP for the device issuing this response with a subnet "
                + "within your lan or something in a cloud provider's network?");

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}