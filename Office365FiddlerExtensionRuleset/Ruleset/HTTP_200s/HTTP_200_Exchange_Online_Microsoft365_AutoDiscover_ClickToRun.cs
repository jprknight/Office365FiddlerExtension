﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
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
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook CTR Autodiscover. Expected XML found.");

                    int sessionAuthenticationConfidenceLevel;
                    int sessionTypeConfidenceLevel;
                    int sessionResponseServerConfidenceLevel;
                    int sessionSeverity;

                    try
                    {
                        var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun");
                        sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                        sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                        sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                        sessionSeverity = sessionClassificationJson.SessionSeverity;
                    }
                    catch (Exception ex)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                        sessionAuthenticationConfidenceLevel = 5;
                        sessionTypeConfidenceLevel = 5;
                        sessionResponseServerConfidenceLevel = 5;
                        sessionSeverity = 30;
                    }

                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s",

                        SessionType = RulesetLangHelper.GetString("HTTP_200s_CTR_AutoDiscover_SessionType"),
                        ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200s_CTR_AutoDiscover_ResponseCodeDescription"),
                        ResponseAlert = RulesetLangHelper.GetString("HTTP_200s_CTR_AutoDiscover_ResponseAlert"),
                        ResponseComments = RulesetLangHelper.GetString("HTTP_200s_CTR_AutoDiscover_ResponseComments"),

                        SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                        SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                        SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                        SessionSeverity = sessionSeverity
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
                else
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange Online / Outlook CTR Autodiscover. Expected XML NOT found!");

                    int sessionAuthenticationConfidenceLevel;
                    int sessionTypeConfidenceLevel;
                    int sessionResponseServerConfidenceLevel;
                    int sessionSeverity;

                    try
                    {
                        var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Exchange_Online_Microsoft365_AutoDiscover_ClickToRun_XML_Response_Not_Found");
                        sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                        sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                        sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                        sessionSeverity = sessionClassificationJson.SessionSeverity;
                    }
                    catch (Exception ex)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                        sessionAuthenticationConfidenceLevel = 5;
                        sessionTypeConfidenceLevel = 10;
                        sessionResponseServerConfidenceLevel = 5;
                        sessionSeverity = 60;
                    }

                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s",

                        SessionType = RulesetLangHelper.GetString("HTTP_200s_CTR_AutoDiscover_NotFound_SessionType"),
                        ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200s_CTR_AutoDiscover_NotFound_ResponseCodeDescription"),
                        ResponseAlert = RulesetLangHelper.GetString("HTTP_200s_CTR_AutoDiscover_NotFound_ResponseAlert"),
                        ResponseComments = RulesetLangHelper.GetString("HTTP_200s_CTR_AutoDiscover_NotFound_ResponseComments"),

                        SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                        SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                        SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                        SessionSeverity = sessionSeverity
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
            }
        }
    }
}
