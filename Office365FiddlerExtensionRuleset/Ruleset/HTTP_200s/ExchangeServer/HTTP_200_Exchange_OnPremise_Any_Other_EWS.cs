using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_Exchange_OnPremise_Any_Other_EWS
    {
        internal Session session { get; set; }

        private static HTTP_200_Exchange_OnPremise_Any_Other_EWS _instance;

        public static HTTP_200_Exchange_OnPremise_Any_Other_EWS Instance => _instance ?? (_instance = new HTTP_200_Exchange_OnPremise_Any_Other_EWS());

        /// <summary>
        /// Exchange OnPremise Any Other Exchange Web Services.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            // Note: There are some organizations who have vanity domains for Office 365. They are the outliers for this scenario.

            this.session = session;
            
            // If this isn't an EWS call, return.
            if (!this.session.uriContains("ews/exchange.asmx"))
            {
                return;
            }

            // If the call goes to outlook.office.com, return.
            if (this.session.HostnameIs("outlook.office365.com"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 OnPremise EWS call.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OnPremise_Any_Other_EWS");
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

                SessionType = RulesetLangHelper.GetString("HTTP_200s_OnPremise_Exchange_EWS_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200s_OnPremise_Exchange_EWS_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200s_OnPremise_Exchange_EWS_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200s_OnPremise_Exchange_EWS_ResponseComments"),

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
