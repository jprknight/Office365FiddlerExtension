using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
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

            // Make sure this session is an Exchange Online Autodiscover request.
            // Non-ClickToRun clients redirect to https://autodiscover-s.outlook.com/Autodiscover/AutoDiscover.xml
            if ((this.session.hostname == "autodiscover-s.outlook.com") && (this.session.uriContains("autodiscover.xml")))
            {
                if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) &&
                    (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) &&
                    (this.session.utilFindInResponse("<MailStore>", false) > 1) &&
                    (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook MSI Autodiscover. Expected XML found.");

                    int sessionAuthenticationConfidenceLevel = 0;
                    int sessionTypeConfidenceLevel = 0;
                    int sessionResponseServerConfidenceLevel = 0;
                    int sessionSeverity = 0;

                    int sessionAuthenticationConfidenceLevelFallback = 5;
                    int sessionTypeConfidenceLevelFallback = 10;
                    int sessionResponseServerConfidenceLevelFallback = 5;
                    int sessionSeverityFallback = 30;

                    try
                    {
                        var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun");
                        sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                        sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                        sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                        sessionSeverity = sessionClassificationJson.SessionSeverity;
                    }
                    catch (Exception ex)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                            $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
                    }

                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s",

                        SessionType = RulesetLangHelper.GetString("HTTP_200s_EXO_MSI_Autodiscover_SessionType"),
                        ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200s_EXO_MSI_Autodiscover_ResponseCodeDescription"),
                        ResponseAlert = RulesetLangHelper.GetString("HTTP_200s_EXO_MSI_Autodiscover_ResponseAlert"),
                        ResponseComments = RulesetLangHelper.GetString("HTTP_200s_EXO_MSI_Autodiscover_ResponseComments"),

                        SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                            sessionAuthenticationConfidenceLevelFallback),

                        SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                            sessionTypeConfidenceLevelFallback),

                        SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                            sessionResponseServerConfidenceLevelFallback),

                        SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                            sessionSeverityFallback)
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
                else
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook MSI Autodiscover. Expected XML NOT found!");

                    int sessionAuthenticationConfidenceLevel = 0;
                    int sessionTypeConfidenceLevel = 0;
                    int sessionResponseServerConfidenceLevel = 0;
                    int sessionSeverity = 0;

                    int sessionAuthenticationConfidenceLevelFallback = 5;
                    int sessionTypeConfidenceLevelFallback = 10;
                    int sessionResponseServerConfidenceLevelFallback = 5;
                    int sessionSeverityFallback = 60;

                    try
                    {
                        var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_MSI_Non_ClickToRun_Unexpected_XML_Response");
                        sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                        sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                        sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                        sessionSeverity = sessionClassificationJson.SessionSeverity;
                    }
                    catch (Exception ex)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                            $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
                    }

                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s",

                        SessionType = RulesetLangHelper.GetString("HTTP_200s_MSI_AutoDiscover_SessionType"),
                        ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200s_MSI_AutoDiscover_ResponseCodeDescription"),
                        ResponseAlert = RulesetLangHelper.GetString("HTTP_200s_MSI_AutoDiscover_ResponseAlert"),
                        ResponseComments = RulesetLangHelper.GetString("HTTP_200s_MSI_AutoDiscover_ResponseComments"),

                        SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                            sessionAuthenticationConfidenceLevelFallback),

                        SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                            sessionTypeConfidenceLevelFallback),

                        SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                            sessionResponseServerConfidenceLevelFallback),

                        SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                            sessionSeverityFallback)
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
            }
        }
    }
}
