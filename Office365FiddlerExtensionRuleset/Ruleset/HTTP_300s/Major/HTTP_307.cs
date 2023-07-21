using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_307
    {
        internal Session session { get; set; }

        private static HTTP_307 _instance;

        public static HTTP_307 Instance => _instance ?? (_instance = new HTTP_307());

        public void HTTP_307_AutoDiscover_Temporary_Redirect(Session session)
        {
            this.session = session;

            // Specific scenario where a HTTP 307 Temporary Redirect incorrectly send an EXO Autodiscover request to an On-Premise resource, breaking Outlook connectivity.
            if (this.session.hostname.Contains("autodiscover") &&
                (this.session.hostname.Contains("mail.onmicrosoft.com") &&
                (this.session.fullUrl.Contains("autodiscover") &&
                (this.session.ResponseHeaders["Location"] != "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml"))))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 307 On-Prem Temp Redirect - Unexpected location!");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_307s",
                    UIBackColour = "Red",
                    UITextColour = "Black",

                    SessionType = "***UNEXPECTED LOCATION***",
                    ResponseCodeDescription = "!307 Temporary Redirect!",
                    ResponseServer = "***UNEXPECTED LOCATION***",
                    ResponseAlert = "<b><span style='color:red'>HTTP 307 Temporary Redirect</span></b>",
                    ResponseComments = "<b>Temporary Redirects have been seen to redirect Exchange Online Autodiscover "
                    + "calls back to On-Premise resources, breaking Outlook connectivity</b>. Likely cause is a local networking device. Test outside of the LAN to confirm."
                    + "<p>This session is an Autodiscover request for Exchange Online which has not been sent to "
                    + "<a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a> as expected.</p>"
                    + "<p>Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place.</p>",
                    Authentication = "***UNEXPECTED LOCATION***",

                    SessionAuthenticationConfidenceLevel = 5,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 5
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
            }
        }

        public void HTTP_307_Other_AutoDiscover_Redirects(Session session)
        {

            this.session = session;

            // The above scenario is not seem, however Temporary Redirects are not normally expected to be seen.
            // Highlight as a warning.
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 307 Temp Redirect.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_307s",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "",
                ResponseCodeDescription = "307 Temporary Redirect",
                ResponseAlert = "HTTP 307 Temporary Redirect",
                ResponseComments = "Temporary Redirects have been seen to redirect Exchange Online Autodiscover calls "
                + "back to On-Premise resources, breaking Outlook connectivity. "
                + "<p>Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place. </p>"
                + "<p>If this session is not for an Outlook process then the information above may not be relevant to the issue under investigation.</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };
        }

        public void HTTP_307_All_Other_Redirects(Session session)
        {

            this.session = session;

            // The above scenario is not seem, however Temporary Redirects are not normally expected to be seen.
            // Highlight as a warning.
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 307 Temp Redirect.");

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_307s",
                UIBackColour = "Orange",
                UITextColour = "Black",

                SessionType = "",
                ResponseCodeDescription = "307 Temporary Redirect",
                ResponseAlert = "HTTP 307 Temporary Redirect",
                ResponseComments = "<p>Temporary Redirects might be an indication of an issue, but aren't in themselves a smoking gun.</p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };
        }
    }
}