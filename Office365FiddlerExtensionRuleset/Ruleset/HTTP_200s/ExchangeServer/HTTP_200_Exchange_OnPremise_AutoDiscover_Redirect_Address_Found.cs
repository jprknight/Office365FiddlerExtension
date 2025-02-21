using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found
    {
        internal Session session { get; set; }

        private static HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found _instance;

        public static HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found Instance => _instance ?? (_instance = new HTTP_200_Exchange_OnPremise_AutoDiscover_Redirect_Address_Found());

        /// <summary>
        /// Exchange OnPremise AutoDiscover Redirect Address Found.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // if this session does not have redirectAddr in the response body, return.
            if (!(this.session.utilFindInResponse("<Action>redirectAddr</Action>", false) > 1))
            {
                return;
            }

            // If this autodiscover redirect is from Microsoft 365, return.
            if (this.session.HostnameIs("autodiscover-s.outlook.com") || this.session.HostnameIs("autodiscover.outlook.com"))
            {
                return;
            }

            /*
            <?xml version="1.0" encoding="utf-8"?>
            <Autodiscover xmlns="http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006">
                <Response xmlns="http://schemas.microsoft.com/exchange/autodiscover/outlook/responseschema/2006a">
                <Account>
                    <Action>redirectAddr</Action>
                    <RedirectAddr>user@contoso.mail.onmicrosoft.com</RedirectAddr>       
                </Account>
                </Response>
            </Autodiscover>
            */

            // Logic to detected the redirect address in this session.
            // 
            string RedirectResponseBody = this.session.GetResponseBodyAsString();
            int start = this.session.GetResponseBodyAsString().IndexOf("<RedirectAddr>");
            int end = this.session.GetResponseBodyAsString().IndexOf("</RedirectAddr>");
            int charcount = end - start;
            string RedirectAddress;

            if (charcount > 0)
            {
                RedirectAddress = RedirectResponseBody.Substring(start, charcount).Replace("<RedirectAddr>", "");
            }
            else
            {
                RedirectAddress = RulesetLangHelper.GetString("RedirectAddressNotFound");
            }

            if (RedirectAddress.Contains(".onmicrosoft.com"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " Exchange OnPremise Autodiscover redirect to Exchange Online / Microsoft365.");

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
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found");
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

                    SessionType = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_ResponseCodeDescription"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_ResponseCommentsStart")
                    + " "
                    + RedirectAddress
                    + RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_ResponseCommentsEnd"),

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
                // Highlight if we got this far and we don't have a redirect address which points to
                // Exchange Online / Microsoft365 such as: contoso.mail.onmicrosoft.com.

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 Exchange On-Premise AUTOD INCORRECT REDIRECT ADDR! : " + RedirectAddress);

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
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect");
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

                    SessionType = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseCodeDescription"),
                    ResponseServer = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseServer"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseCommentsStart")
                    + " "
                    + RedirectAddress
                    + RulesetLangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseCommentsEnd"),

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
