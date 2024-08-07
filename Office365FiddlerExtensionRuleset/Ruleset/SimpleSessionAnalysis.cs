﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class SimpleSessionAnalysis
    {
        internal Session session { get; set; }

        private static SimpleSessionAnalysis _instance;

        public static SimpleSessionAnalysis Instance => _instance ?? (_instance = new SimpleSessionAnalysis());

        /// <summary>
        /// Run simple session analysis, where there is no conditional logic for the response code.
        /// Pull session analysis values from SessionClassification.json and set simple session analysis in session headers.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="ResponseCodeSection"></param>
        public void Run(Session session, String ResponseCodeSection)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} {LangHelper.GetString("Simple Session Analysis")}");

            string sessionSectionTitle;
            string sessionType;
            string sessionResponseAlert;

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection(ResponseCodeSection);

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} {sessionClassificationJson.SessionType}");

                sessionSectionTitle = sessionClassificationJson.SectionTitle;
                sessionType = sessionClassificationJson.SessionType;
                sessionResponseAlert = sessionClassificationJson.SessionResponseAlert;
                
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

                sessionSectionTitle = RulesetLangHelper.GetString("Simple Session Analysis - Json Read Failure.");
                sessionType = RulesetLangHelper.GetString("Simple Session Analysis");
                sessionResponseAlert = RulesetLangHelper.GetString("Simple Session Analysis - Json Read Failure.");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 10;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = sessionSectionTitle,
                SessionType = sessionType,

                ResponseCodeDescription = sessionType,
                ResponseAlert = sessionResponseAlert,
                ResponseComments = RulesetLangHelper.GetString("Response Comments No Known Issue"),

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
