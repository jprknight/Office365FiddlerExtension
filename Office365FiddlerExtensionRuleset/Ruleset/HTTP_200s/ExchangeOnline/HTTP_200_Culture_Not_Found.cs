using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_Culture_Not_Found
    {
        internal Session session { get; set; }

        private static HTTP_200_Culture_Not_Found _instance;

        public static HTTP_200_Culture_Not_Found Instance => _instance ?? (_instance = new HTTP_200_Culture_Not_Found());
        /*
        HTTP 200 Outlook MAPI session.
        
        System.Globalization.CultureNotFoundException: Culture is not supported. (Parameter 'culture')
        4096 (0x1000) is an invalid culture identifier.

        */
        public void Run(Session session)
        {
            this.session = session;

            if (!this.session.uriContains("outlook.office365.com"))
            {
                return;
            }

            if (!this.session.uriContains("mapi/emsmdb/"))
            {
                return;
            }

            if (!RulesetUtilities.Instance.SearchForPhrase(this.session, "Culture is not supported"))
            {
                return;
            }

            var ExtensionSessionFlags = RulesetSessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_CultureNotFound");

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

                SessionType = RulesetLangHelper.GetString("HTTP_200_CultureNotFound_Session_Type"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_CultureNotFound_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_CultureNotFound_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_CultureNotFound_ResponseComments"),

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
