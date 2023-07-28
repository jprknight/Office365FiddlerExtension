using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                RedirectAddress = "Redirect address not found by extension.";
            }

            if (RedirectAddress.Contains(".onmicrosoft.com"))
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_OnPremise_AutoDiscover_Redirect_Address_Found");

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " Exchange OnPremise Autodiscover redirect to Exchange Online / Microsoft365.");

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Redirect_Address",

                    SessionType = "On-Prem AutoD Redirect",
                    ResponseCodeDescription = "200 OK Redirect Address",
                    ResponseAlert = "Exchange On-Premise Autodiscover redirect.",
                    ResponseComments = "Exchange On-Premise Autodiscover redirect address to Exchange Online found."
                    + "<p>RedirectAddress: "
                    + RedirectAddress
                    + "</p><p>This is what we want to see, the mail.onmicrosoft.com redirect address (you may know this as the <b>target address</b> or "
                    + "<b>remote routing address</b>) from On-Premise sends Outlook (MSI / Perpetual license) to Office 365 / Exchange Online.</p>",

                    SessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionClassificationJson.SessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
            else
            {
                // Highlight if we got this far and we don't have a redirect address which points to
                // Exchange Online / Microsoft365 such as: contoso.mail.onmicrosoft.com.

                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_OnPremise_AutoDiscover_IncorrectRedirect");

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " + this.session.id + " HTTP 200 Exchange On-Premise AUTOD INCORRECT REDIRECT ADDR! : " + RedirectAddress);

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Redirect_Address_Not_Found",

                    SessionType = "!AUTOD INCORRECT REDIRECT!",
                    ResponseCodeDescription = "200 OK, Incorrect Redirect Address!",
                    ResponseServer = "Fiddler Update Check",
                    ResponseAlert = "!Exchange On-Premise Autodiscover redirect!",
                    ResponseComments = "Exchange On-Premise Autodiscover redirect address found, which does not contain .onmicrosoft.com." +
                    "<p>RedirectAddress: " + RedirectAddress +
                    "</p><p>If this is an Office 365 mailbox the <b>targetAddress from On-Premise is not sending Outlook to Office 365</b>!</p>",

                    SessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionClassificationJson.SessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}
