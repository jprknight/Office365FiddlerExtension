using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Office365FiddlerInspector.Services;
using Newtonsoft.Json;

namespace Office365FiddlerInspector.Ruleset
{
    class BroadLogicChecks : ActivationService
    {
        private static BroadLogicChecks _instance;

        public static BroadLogicChecks Instance => _instance ?? (_instance = new BroadLogicChecks());

        public void FiddlerUpdateSessions (Session session)
        {
            this.session = session;

            if (this.session.hostname == "www.fiddler2.com" && this.session.uriContains("UpdateCheck.aspx"))
            {

                var mySessionFlags = new SessionFlags()
                {
                    SectionTitle = "Broad Logic Checks (www.fiddler2.com).",
                    UIBackColour = "Gray",
                    UITextColour = "Black",
                    SessionType = "Fiddler Update Check",
                    ResponseServer = "Fiddler Update Check",
                    ResponseAlert = "Fiddler Update Check",
                    ResponseComments = "This is Fiddler itself checking for updates. It has nothing to do with the Office 365 Fiddler Extension.",
                    SessionAuthenticationConfidenceLevel = 10,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 10
                };
                    
                var mySessionFlagsJson = JsonConvert.SerializeObject(mySessionFlags);
                GetSetSessionFlags.Instance.SetOffice365FiddlerExtensionJson(this.session, mySessionFlagsJson);
            }
        }

        public class SessionFlags
        {
            public string SectionTitle { get; set; }

            public string UIBackColour { get; set;}

            public string UITextColour { get;set;}

            public string SessionType { get; set;}

            public string ResponseServer { get; set;}

            public string ResponseAlert { get; set;}
            
            public string ResponseComments { get; set;}

            public int SessionAuthenticationConfidenceLevel { get; set;}

            public int SessionTypeConfidenceLevel { get; set;}

            public int SessionResponseServerConfidenceLevel { get;set;}
        }

        public void ConnectTunnelSessions(Session session)
        {
            this.session = session;

            // Connect Tunnel.
            //
            // Check for connect tunnel with no usable data in the response body.
            //
            // This check does not work for sessions which have not been loaded from a SAZ file.
            // My best guess is this is a timing issue, where the data is not immediately available when this check runs.
            // SetSessionType makes exactly the same call later on down the code path and it works.
            if (this.session.isTunnel)
            {
                string TLS;

                GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "Broad Logic Checks (connect tunnel).");
                // TLS 1.0 in request/response pair.

                // Request:
                //   Version: 3.1 (TLS/1.0)

                //Response:
                //   Secure Protocol: Tls
                //   Cipher: Aes256 256bits
                //   Hash Algorithm: Sha1 160bits

                if (this.session.utilFindInResponse("Secure Protocol: Tls10", false) > 1 || this.session.utilFindInResponse("(TLS/1.0)", false) > 1)
                {
                    TLS = "TLS 1.0";
                }
                // TLS 1.1 in request/response pair.
                else if (this.session.utilFindInResponse("Secure Protocol: Tls11", false) > 1 || this.session.utilFindInRequest("(TLS/1.1)", false) > 1)
                {
                    TLS = "TLS 1.1";
                }
                // TLS 1.2 in request/response pair.
                else if (this.session.utilFindInRequest("Secure Protocol: Tls12", false) > 1 || this.session.utilFindInRequest("(TLS/1.2)", false) > 1)
                {
                    TLS = "TLS 1.2";
                }
                else
                {
                    // If we cannot determine the TLS version do nothing.
                    // This can happen when live tracing traffic. The request/responses cannot be read fast enough to get accurate results.
                    TLS = "TLS Unknown";
                }

                // 11/1/2022 -- There was some old code accompanying this comment, leaving this as it might be useful information for the future.

                // Trying to check session response body for a string value using !this.session.bHasResponse does not impact performance, but is not reliable.
                // Using this.session.GetResponseBodyAsString().Length == 0 kills performance. Fiddler wouldn't even load with this code in place.
                // Ideally looking to do: if (this.session.utilFindInResponse("CONNECT tunnel, through which encrypted HTTPS traffic flows", false) > 1)
                // Only works reliably when loading a SAZ file and request/response data is immediately available to do logic checks against.

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "black");

                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Connect Tunnel");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "This is an encrypted tunnel. If all or most of the sessions are connect tunnels "
                    + "the sessions collected did not have decryption enabled. Setup Fiddler to 'Decrypt HTTPS traffic', click Tools -> Options -> HTTPS tab."
                    + "<p>If in any doubt see instructions at https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/DecryptHTTPS. </p>");

                switch (this.session.responseCode)
                {
                    case 403:
                        // If this is a HTTP 403 we need analysis on this session.
                        // I have seen HTTP 403 connect tunnels actually show interesting data in authentication scenarios.
                        GetSetSessionFlags.Instance.SetSessionType(this.session, "Connect Tunnel: " + TLS);
                        GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                        GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
                        GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
                        break;
                    case 200:
                        GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "200 OK");

                        GetSetSessionFlags.Instance.SetSessionType(this.session, "Connect Tunnel: " + TLS);
                        GetSetSessionFlags.Instance.SetXAuthentication(this.session, "Connect Tunnel: " + TLS);
                        GetSetSessionFlags.Instance.SetXResponseServer(this.session, "Connect Tunnel: " + TLS);

                        // Absolute certainly we don't want to do anything further with this session.
                        GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                        GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                        GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "10");
                        break;
                }
            }
        }

        public void ApacheAutodiscover(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // From a scenario where Apache Web Server found to be answering Autodiscover calls and throwing HTTP 301 & 405 responses.
            // This is typically seen on the root domain Autodiscover call made from Outlook if GetO365Explicit is not used.
            //
            if ((this.session.url.Contains("autodiscover") && (this.session.oResponse["server"].Contains("Apache"))))
            {
                GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "Apache is answering Autodiscover requests! Investigate this first!");

                GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
                GetSetSessionFlags.Instance.SetUITextColour(this.session, "black");

                GetSetSessionFlags.Instance.SetSessionType(this.session, "***APACHE AUTODISCOVER***");
                GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "Apache is answering Autodiscover requests!");
                GetSetSessionFlags.Instance.SetXResponseComments(this.session, "<b><span style='color:red'>An Apache Web Server(Unix/Linux) is answering Autodiscover requests!</span></b>"
                    + "<p>This should not be happening. Consider disabling Root Domain Autodiscover lookups.</p>"
                    + "<p>See ExcludeHttpsRootDomain on </p>"
                    + "<p><a href='https://support.microsoft.com/en-us/help/2212902/unexpected-autodiscover-behavior-when-you-have-registry-settings-under' target='_blank'>"
                    + "https://support.microsoft.com/en-us/help/2212902/unexpected-autodiscover-behavior-when-you-have-registry-settings-under </a></p>"
                    + "<p>Beyond this the web administrator responsible for the server needs to stop the Apache web server from answering these requests.</p>");

                // Absolute certainly we don't want to do anything further with this session.
                GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "10");
                GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "10");
            }
        }
    }
}