using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound
    {
        internal Session session { get; set; }

        private static HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound _instance;

        public static HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound Instance => _instance ?? (_instance = new HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_AddressNotFound());

        /// <summary>
        /// Exchange OnPremise AutoDiscover Redirect Address Not Found.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If this autodiscover redirect is from Microsoft 365, return.
            if (this.session.HostnameIs("autodiscover-s.outlook.com") || this.session.HostnameIs("autodiscover.outlook.com"))
            {
                return;
            }

            if ((this.session.utilFindInResponse("<Message>The email address can't be found.</Message>", false) > 1) &&
                (this.session.utilFindInResponse("<ErrorCode>500</ErrorCode>", false) > 1))
            {
                /*
                <?xml version="1.0" encoding="utf-8"?>
                <Autodiscover xmlns="http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006">
                    <Response>
                    <Error Time="12:03:32.8803744" Id="2422600485">
                        <ErrorCode>500</ErrorCode>
                        <Message>The email address can't be found.</Message>
                        <DebugData />
                    </Error>
                    </Response>
                </Autodiscover>
                */

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Exchange On-Premise redirect address. Error code 500: The email address can't be found.");

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
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound");
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

                    SessionType = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound_SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound_ResponseCodeDescription"),
                    ResponseServer = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound_ResponseServer"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound_ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound_ResponseComments"),

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
