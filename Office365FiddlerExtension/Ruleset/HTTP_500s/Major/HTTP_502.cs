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
    class HTTP_502 : ActivationService
    {
        private static HTTP_502 _instance;

        public static HTTP_502 Instance => _instance ?? (_instance = new HTTP_502());

        public void HTTP_502_Bad_Gateway_Telemetry_False_Positive(Session session)
        {
            // Telemetry false positive.

            this.session = session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            if (ExtensionSessionFlags.SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            if (!(this.session.oRequest["Host"] == "sqm.telemetry.microsoft.com:443") &&
                (!(this.session.utilFindInResponse("target machine actively refused it", false) > 1)))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 502 Bad Gateway. Telemetry False Positive.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",
                UIBackColour = "Blue",
                UITextColour = "Black",

                SessionType = "False Positive",
                ResponseCodeDescription = "502 Bad Gateway False Positive",
                ResponseAlert = "<b><span style='color:green'>False Positive</span></b>",
                ResponseComments = "Telemetry failing is unlikely the cause of significant Office 365 client issues.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);            
        }

        public void HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive(Session session)
        {
            // Exchange Online DNS lookup on contoso.onmicrosoft.com, False Positive.

            this.session = session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            if (ExtensionSessionFlags.SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            if (!(this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                !(this.session.utilFindInResponse("DNS Lookup for ", false) > 1) &&
                !(this.session.utilFindInResponse(" failed.", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 502 Bad Gateway. EXO DNS False Positive.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",
                UIBackColour = "Blue",
                UITextColour = "Black",

                SessionType = "False Positive",
                ResponseCodeDescription = "502 Bad Gateway False Positive",
                ResponseAlert = "<b><span style='color:green'>False Positive</span></b>",
                ResponseComments = "<b><span style='color:green'>False positive on HTTP 502</span></b>. "
                + "By design, the host only accepts connections on port 25, port 443 is not available."
                + "<p>To validate this above lookup the record, confirm it is a MX record and attempt to connect to the MX host on ports 25 and 443.</p>",
                ResponseServer = "False Positive",
                Authentication = "False Positive",

                SessionAuthenticationConfidenceLevel = 10,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);            
        }

        public void HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive(Session session)
        {
            // Exchange Online connection to autodiscover.contoso.mail.onmicrosoft.com, False Positive.

            this.session = session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            if (ExtensionSessionFlags.SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            if (!(this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                // Too specific, it looks as though we see ConnectionRefused or The socket connection to ... failed.
                //(this.session.utilFindInResponse("ConnectionRefused ", false) > 1) &&
                !(this.session.utilFindInResponse("target machine actively refused it", false) > 1))
            {
                return;
            }

            
            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 502 Bad Gateway. O365 AutoD onmicrosoft.com False Positive.");

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

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",
                UIBackColour = "Blue",
                UITextColour = "Black",

                SessionType = "False Positive",
                ResponseCodeDescription = "502 Bad Gateway False Positive",
                ResponseAlert = "<b><span style='color:green'>False Positive</span></b>",
                ResponseComments = "<b><span style='color:green'>False Positive</span></b>. By design Office 365 Autodiscover does not respond to "
                + AutoDFalsePositiveDomain
                + " on port 443. "
                + "<p>Validate this message by confirming the Host IP (if shown) is an Office 365 Host/IP address and perform a telnet to it on port 80.</p>"
                + "<p>If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design redirects "
                + "requests on port 80 to <a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a>"
                + "<p>The reason for this is Microsoft does not maintain SSL certificates for every tenant domain name registered on the .onmicrosoft.com domain."
                + "AutoDiscover redirects to autodiscover-s.outlook.com which does accept connections on 443 and Microsoft does maintain SSL certificates for this endpoint.</p>",
                ResponseServer = "False Positive",
                Authentication = "False Positive",

                SessionAuthenticationConfidenceLevel = 10,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void HTTP_502_Bad_Gateway_Vanity_Domain_M365_AutoDiscover_False_Positive(Session session)
        {
            // Vanity domain points to Office 365 autodiscover; false positive.

            this.session = session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            if (ExtensionSessionFlags.SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            if (!(this.session.utilFindInResponse("autodiscover.", false) > 1) &&
                !(this.session.utilFindInResponse("target machine actively refused it", false) > 1))
            {
                return;
            }

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

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 502 Bad Gateway. Vanity domain AutoD False Positive.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",
                UIBackColour = "Blue",
                UITextColour = "Black",

                SessionType = "AutoD False Positive",
                ResponseCodeDescription = "502 Bad Gateway False Positive",
                ResponseAlert = "<b><span style='color:orange'>Autodiscover Possible False Positive?</span></b>",
                ResponseComments = "Autoddiscover Possible False Positive. By design Office 365 endpoints such as "
                + "autodiscover.contoso.onmicrosoft.com do not respond on port 443. "
                + "<p>Validate this message by confirming this is an Office 365 Host/IP address and perform a telnet to it on port 80.</p>"
                + "<p>If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design "
                + "redirects requests on port 80 to <a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' "
                + "target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a>",
                ResponseServer = "AutoD False Positive",
                Authentication = "AutoD False Positive",

                SessionAuthenticationConfidenceLevel = 10,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);            
        }

        public void HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover(Session session)
        {
            // Anything else Exchange Autodiscover.

            this.session = session;

            var ExtensionSessionFlags = JsonConvert.DeserializeObject<SessionFlagProcessor.ExtensionSessionFlags>(SessionFlagProcessor.Instance.GetSessionJsonData(this.session));

            if (ExtensionSessionFlags.SessionTypeConfidenceLevel == 10)
            {
                return;
            }

            if (!(this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                        !(this.session.utilFindInResponse("autodiscover", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 502 Bad Gateway. Exchange Autodiscover.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "!AUTODISCOVER!",
                ResponseCodeDescription = "502 Bad Gateway",
                ResponseAlert = "<b><span style='color:red'>AUTODISCOVER</span></b>",
                ResponseComments = "This AutoDiscover request was refused by the server it was sent to. Check the raw tab for further details.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);            
        }

        public void HTTP_502_Bad_Gateway_Anything_Else(Session session)
        {
            // Everything else.

            this.session = session;

            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} HTTP 502 Bad Gateway.");

            var sessionFlags = new SessionFlagProcessor.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "!Bad Gateway!",
                ResponseCodeDescription = "502 Bad Gateway",
                ResponseAlert = "<b><span style='color:red'>HTTP 502 Bad Gateway</span></b>",
                ResponseComments = "Potential to cause the issue you are investigating. "
                + "Do you see expected responses beyond this session in the trace? Is the Host IP for the device issuing this response with a subnet "
                + "within your lan or something in a cloud provider's network?",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 5,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagProcessor.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}