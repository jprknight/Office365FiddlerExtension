﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class ApacheAutodiscover
    {
        internal Session session { get; set; }

        private static ApacheAutodiscover _instance;

        public static ApacheAutodiscover Instance => _instance ?? (_instance = new ApacheAutodiscover());

        /// <summary>
        /// Determine if the current session has an Apache server responding to an AutoDiscover call.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // From a scenario where Apache Web Server found to be answering Autodiscover calls and throwing HTTP 301 & 405 responses.
            // This is typically seen on the root domain Autodiscover call made from Outlook if GetO365Explicit is not used.
            //
            if ((this.session.url.Contains("autodiscover") && (this.session.oResponse["server"].Contains("Apache"))))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} Apache is answering Autodiscover requests! Investigate this first!.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("BroadLogicChecks|ApacheAutodiscover");

                    sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                    sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                    sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                    sessionSeverity = sessionClassificationJson.SessionSeverity;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): {this.session.id} {ex}");

                    sessionAuthenticationConfidenceLevel = 5;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 5;
                    sessionSeverity = 60;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = RulesetLangHelper.GetString("Broad Logic Checks"),

                    SessionType = RulesetLangHelper.GetString("BroadLogicChecks_APACHE AUTODISCOVER"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("BroadLogicChecks_200 OK - APACHE AUTODISCOVER"),
                    ResponseServer = RulesetLangHelper.GetString("BroadLogicChecks_APACHE"),
                    ResponseAlert = RulesetLangHelper.GetString("BroadLogicChecks_Apache is answering Autodiscover requests!"),
                    ResponseComments = RulesetLangHelper.GetString("BroadLogicChecks_Apache AutoDiscover Response Comments"),                   

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}
