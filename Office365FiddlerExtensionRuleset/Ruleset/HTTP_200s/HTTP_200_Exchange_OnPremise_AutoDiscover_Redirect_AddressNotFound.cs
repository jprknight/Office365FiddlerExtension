﻿using Fiddler;
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

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OnPremise_AutoDiscover_Redirect_AddressNotFound");
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
                    SectionTitle = "HTTP_200s_Redirect_Address_Not_Found",

                    SessionType = "!NO AUTOD REDIRECT ADDR!",
                    ResponseCodeDescription = "200 OK, !Email address not found!",
                    ResponseServer = "Fiddler Update Check",
                    ResponseAlert = "<b><span style='color:red'>Exchange On-Premise Autodiscover Redirect</span></b>",
                    ResponseComments = "Exchange On-Premise Autodiscover redirect address can't be found. "
                    + "Look for other On-Premise Autodiscover responses, we may have a "
                    + "valid Autodiscover targetAddress from On-Premise in another session in this trace."
                    + "Seeing some redirects return a HTTP 500 from Exchange OnPremise have been seen in a normal, working Outlook "
                    + "client which can connect to the Exchange Online mailbox.",

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