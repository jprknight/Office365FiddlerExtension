﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_3S_Suggestions
    {
        internal Session session { get; set; }

        private static HTTP_200_3S_Suggestions _instance;

        public static HTTP_200_3S_Suggestions Instance => _instance ?? (_instance = new HTTP_200_3S_Suggestions());

        /// <summary>
        /// 3S Suggestions call.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If this isn't a 3G Suggestions call, return.
            if (!this.session.uriContains("search/api/v1/suggestions"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} 200 3S Suggestions call.");

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 5;
            int sessionTypeConfidenceLevelFallback = 5;
            int sessionResponseServerConfidenceLevelFallback = 5;
            int sessionSeverityFallback = 30;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_3S_Suggestions");
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

            Uri uri = new Uri(this.session.fullUrl);
            var queryStrings = HttpUtility.ParseQueryString(uri.Query);
            var scenario = queryStrings["scenario"] ?? "scenario not specified in url";
            var entityTypes = queryStrings["entityTypes"] ?? "entityTypes not specified in url";
            var clientRequestId = this.session.RequestHeaders.Where(x => x.Name.Equals("client-request-id")).FirstOrDefault();

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s",

                SessionType = RulesetLangHelper.GetString("HTTP_200_3S_Suggestions"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_3S_Suggestions_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_3S_Suggestions"),
                ResponseComments = $"{RulesetLangHelper.GetString("Scenario")}: {scenario} {RulesetLangHelper.GetString("Types")}: {entityTypes} {clientRequestId}",

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
