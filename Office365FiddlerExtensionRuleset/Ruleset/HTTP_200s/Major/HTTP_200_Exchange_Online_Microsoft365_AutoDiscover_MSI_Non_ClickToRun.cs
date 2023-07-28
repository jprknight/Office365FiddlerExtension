using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun
    {
        internal Session session { get; set; }

        private static HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun _instance;

        public static HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun Instance => _instance ?? (_instance = new HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun());

        /// <summary>
        /// Exchange Online / Microsoft 365 AutoDiscover MSI Non-ClickToRun.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If this session isn't a Autodiscover session, return; 
            if (!this.session.uriContains("autodiscover.xml"))
            {
                return;
            }

            // 200 Exchange Online Autodiscover

            // Make sure this session is an Exchange Online Autodiscover request.
            // Non-ClickToRun clients redirect to https://autodiscover-s.outlook.com/Autodiscover/AutoDiscover.xml
            if ((this.session.hostname == "autodiscover-s.outlook.com") && (this.session.uriContains("autodiscover.xml")))
            {
                if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) &&
                    (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) &&
                    (this.session.utilFindInResponse("<MailStore>", false) > 1) &&
                    (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun");

                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook MSI Autodiscover. Expected XML found.");

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s_MSI_AutoDiscover",

                        SessionType = "EXO MSI Autodiscover",
                        ResponseCodeDescription = "200 OK Outlook MSI AutoDiscover",
                        ResponseAlert = "Exchange Online / Outlook MSI AutoDiscover.",
                        ResponseComments = "For AutoDiscover calls which go to autodiscover-s.outlook.com this is likely an Outlook (MSI / perpetual license) client"
                        + " being redirected from Exchange On-Premise to Exchange Online.",

                        SessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel,
                        SessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel,
                        SessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel,
                        SessionSeverity = sessionClassificationJson.SessionSeverity
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
                else
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun_Unexpected_XML_Response");

                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook MSI Autodiscover. Expected XML NOT found!");

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s_MSI_AutoDiscover",

                        SessionType = "!EXO MSI AutoDiscover!",
                        ResponseCodeDescription = "200 OK, Unexpected AutoDiscover XML response.",
                        ResponseAlert = "<b><span style='color:red'>Exchange Online / Outlook MSI AutoDiscover - Unusual AutoDiscover Response</span></b>",
                        ResponseComments = "This session was detected as an AutoDiscover response from Exchange Online. However the response did not contain "
                        + "the expected XML data. Check if a device in-between the perimeter of your network and the client computer can / has altered the data in the response.",

                        SessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel,
                        SessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel,
                        SessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel,
                        SessionSeverity = sessionClassificationJson.SessionSeverity
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
            }
        }
    }
}
