using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
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
                RedirectAddress = LangHelper.GetString("RedirectAddressNotFound");
            }

            if (RedirectAddress.Contains(".onmicrosoft.com"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " Exchange OnPremise Autodiscover redirect to Exchange Online / Microsoft365.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found");
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
                    sessionSeverity = 30;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s",

                    SessionType = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_SessionType"),
                    ResponseCodeDescription = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_ResponseCodeDescription"),
                    ResponseAlert = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_ResponseAlert"),
                    ResponseComments = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_ResponseCommentsStart")
                    + " "
                    + RedirectAddress
                    + LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found_ResponseCommentsEnd"),

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
            else
            {
                // Highlight if we got this far and we don't have a redirect address which points to
                // Exchange Online / Microsoft365 such as: contoso.mail.onmicrosoft.com.

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 Exchange On-Premise AUTOD INCORRECT REDIRECT ADDR! : " + RedirectAddress);

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect");
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

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s",

                    SessionType = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_SessionType"),
                    ResponseCodeDescription = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseCodeDescription"),
                    ResponseServer = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseServer"),
                    ResponseAlert = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseAlert"),
                    ResponseComments = LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseCommentsStart")
                    + " "
                    + RedirectAddress
                    + LangHelper.GetString("HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect_ResponseCommentsEnd"),

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
