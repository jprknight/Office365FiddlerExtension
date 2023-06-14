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
    class HTTP_503 : ActivationService
    {
        private static HTTP_503 _instance;

        public static HTTP_503 Instance => _instance ?? (_instance = new HTTP_503());

        public void HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(Session session)
        {
            //  HTTP 503: SERVICE UNAVAILABLE.

            this.Session = session;

            if (!(this.Session.utilFindInResponse("FederatedStsUnreachable", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 503 Service Unavailable. FederatedStsUnreachable in response body!");

            string RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.Session.oRequest["X-User-Identity"] + "&xml=1";

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_503s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "***FederatedSTSUnreachable***",
                ResponseCodeDescription = "503 Federation Service Unavailable",
                ResponseAlert = "<b><span style='color:red'>FederatedSTSUnreachable</span></b>",
                ResponseComments = "<b><span style='color:red'>HTTP 503: FederatedSTSUnreachable</span></b>."
                + "<b><span style='color:red'>The fedeation service is unreachable or unavailable</span></b>."
                + "<p><b><span style='color:red'>Troubleshoot this issue first before doing anything else.</span></b></p>"
                + "<p>Check the Raw tab for additional details.</p>"
                + "<p>Check the realm page for the authenticating domain. Check the below links from the Realm page to see if the IDP gives the "
                + "expected responses.</p>"
                + $"<a href='{RealmURL}' target='_blank'>{RealmURL}</a>"
                + "<p><b>Expected responses for ADFS</b> (other federation services such as Ping, OKTA may vary)</p>"
                + "<b>AuthURL</b>: Normally expected to show federation service logon page.<br />"
                + "<b>STSAuthURL</b>: Normally expected to show HTTP 400.<br />"
                + "<b>MEXURL</b>: Normally expected to show long stream of XML data.<br />"
                + "<p>If any of these show the HTTP 503 Service Unavailable this <b>confirms some kind of failure on the federation service</b>.</p>"
                + "<p>If however you get the expected responses, this <b>does not neccessarily mean the federation service / everything authentication is healthy</b>. "
                + "Further investigation is advised. You could try hitting these endpoints a few times and see if you get an intermittent failure.</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);           
        }

        public void HTTP_503_Service_Unavailable_Everything_Else(Session session)
        {
            // Everything else.

            this.Session = session;

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {this.Session.id} HTTP 503 Service Unavailable.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_503s",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "!Service Unavailable!",
                ResponseCodeDescription = "503 Service Unavailable",
                ResponseAlert = "<b><span style='color:red'>HTTP 503 Service Unavailable</span></b>",
                ResponseComments = "<b><span style='color:red'>Server that was contacted in this session reports "
                + "it is unavailable</span></b>. Look at the server that issued this response, it is healthy? Contactable? "
                + "Contactable consistently or intermittently? Consider other session server responses in the 500's (500, 502 or 503) in conjunction with this session.",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
        }
    }
}