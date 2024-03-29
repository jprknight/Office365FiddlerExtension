﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class FiddlerUpdateSessions
    {
        internal Session session { get; set; }

        private static FiddlerUpdateSessions _instance;

        public static FiddlerUpdateSessions Instance => _instance ?? (_instance = new FiddlerUpdateSessions());

        public void Run(Session session)
        {
            this.session = session;

            if (this.session.hostname == "www.fiddler2.com" && this.session.uriContains("UpdateCheck.aspx"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} Fiddler Updates.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("BroadLogicChecks|FiddlerUpdateSessions");
                    
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

                    sessionAuthenticationConfidenceLevel = 10;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 10;
                    sessionSeverity = 10;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = LangHelper.GetString("Broad Logic Checks"),

                    SessionType = LangHelper.GetString("BroadLogicChecks_Fiddler Update Check"),
                    ResponseCodeDescription = LangHelper.GetString("BroadLogicChecks_Fiddler Update Check"),
                    ResponseServer = LangHelper.GetString("BroadLogicChecks_Fiddler Update Check"),
                    ResponseAlert = LangHelper.GetString("BroadLogicChecks_Fiddler Update Check"),                    
                    ResponseComments = LangHelper.GetString("BroadLogicChecks_This is Fiddler itself checking for updates."),
                    Authentication = LangHelper.GetString("BroadLogicChecks_Fiddler Update Check"),

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
