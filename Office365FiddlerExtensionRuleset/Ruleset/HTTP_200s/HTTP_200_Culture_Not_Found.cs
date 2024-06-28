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

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

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

                SessionType = RulesetLangHelper.GetString("HTTP_200_CultureNotFound_Session_Type"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_CultureNotFound_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_CultureNotFound_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_CultureNotFound_ResponseComments"),

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
