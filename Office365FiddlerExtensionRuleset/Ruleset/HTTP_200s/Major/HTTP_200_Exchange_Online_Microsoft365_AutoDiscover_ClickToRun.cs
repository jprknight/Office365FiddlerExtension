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
    class HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun
    {
        internal Session session { get; set; }

        private static HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun _instance;

        public static HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun Instance => _instance ?? (_instance = new HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun());

        /// <summary>
        /// Exchange Online / Microsoft 365 AutoDiscover ClickToRun.
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

            // ClickToRun clients use to https://outlook.office365.com/Autodiscover/AutoDiscover.xml.
            if ((this.session.hostname == "outlook.office365.com") && (this.session.uriContains("autodiscover.xml")))
            {
                if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) &&
                    (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) &&
                    (this.session.utilFindInResponse("<MailStore>", false) > 1) &&
                    (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun");

                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook CTR Autodiscover. Expected XML found.");

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s_CTR_AutoDiscover",

                        SessionType = "EXO CTR Autodiscover",
                        ResponseCodeDescription = "200 OK",
                        ResponseAlert = "Exchange Online / Outlook CTR AutoDiscover.",
                        ResponseComments = "For AutoDiscover calls which go to outlook.office365.com this is likely an Outlook Click-To-Run (Downloaded or "
                        + "deployed from Office365) client being redirected from Exchange On-Premise to Exchange Online.",

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
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun_XML_Response_Not_Found");

                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook CTR Autodiscover. Expected XML NOT found!");

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s_CTR_AutoDiscover",

                        SessionType = "Outlook AutoDiscover XML NOT found!",
                        ResponseCodeDescription = "200 OK, !Unexpected XML response!",
                        ResponseAlert = "<b><span style='color:red'>Exchange Online / Outlook CTR Autodiscover - Unusual Autodiscover Response</span></b>",
                        ResponseComments = "This session was detected as an Autodiscover response from Exchange Online. However the response did not contain "
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
