﻿using System;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_403
    {
        internal Session session { get; set; }

        private static HTTP_403 _instance;

        public static HTTP_403 Instance => _instance ?? (_instance = new HTTP_403());

        /// <summary>
        /// Set session analysis values for a HTTP 403 response code.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            HTTP_403_Forbidden_Proxy_Block(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            HTTP_403_FreeBusy_Request_Failed_Forbidden(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }
            HTTP_403_Forbidden_Everything_Else(this.session);
        }

        private void HTTP_403_Forbidden_Proxy_Block(Session session)
        {
            this.session = session;

            // Looking for the term "Access Denied" or "Access Blocked" in session response.
            // Specific scenario where a web proxy is blocking traffic from reaching the internet.
            if (this.session.utilFindInResponse("Access Denied", false) > 1 || session.utilFindInResponse("Access Blocked", false) > 1)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_403s|HTTP_403_Forbidden_Proxy_Block");
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
                    SectionTitle = "HTTP_403s",

                    SessionType = RulesetLangHelper.GetString("HTTP_403_Forbidden_Proxy_Block_SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_403_Forbidden_Proxy_Block_ResponseCodeDescription"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_403_Forbidden_Proxy_Block_ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_403_Forbidden_Proxy_Block_ResponseComments"),

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }

        private void HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set(Session session)
        {
            // 3rd-party EWS application could not connect to Exchange Online mailbox until culture/language was set for the first time in OWA.

            this.session = session;

            if (this.session.fullUrl.Contains("outlook.office365.com/EWS") || this.session.fullUrl.Contains("outlook.office365.com/ews"))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 403 Forbidden. EWS Language not set on mailbox.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_403s|HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set");
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

                var sessionFlags_HTTP403_EWS = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_403s_EWS_Mailbox_Language",

                    SessionType = RulesetLangHelper.GetString("HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set_SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set_ResponseCodeDescription"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set_ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_403_Forbidden_EWS_Mailbox_Language_Not_Set_ResponseComments"),

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };
                var sessionFlagsJson_HTTP403_EWS = JsonConvert.SerializeObject(sessionFlags_HTTP403_EWS);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson_HTTP403_EWS, false);
            }
        }

        private void HTTP_403_FreeBusy_Request_Failed_Forbidden(Session session)
        {
            // [{"data":{"getSchedule":{"schedules":[{"availabilityView":"","error":{"message":"Request failed with http code Forbidden","responseCode":"403","diagnosticData":"CalculatedRequestType:External_Substrate;LID:38070;FailureMessage:Request failed with http code Forbidden;ResponseCode:403;"},

            this.session = session;

            // If this isn't a Microsoft cloud Free/Busy call, return.
            if (!this.session.fullUrl.Contains("outlook.office365.com"))
            {
                return;
            }

            // If the url doesn't contain one of these well known Free/Busy URLs for the Microsoft cloud, return.
            if (!this.session.fullUrl.Contains("CalendarService")
                && !this.session.fullUrl.Contains("outlookgatewayb2")
                && !this.session.fullUrl.Contains("SchedulingB2"))
            {
                return;
            }

            // If the session doesn't contain this error text, return.
            if (!RulesetUtilities.Instance.SearchForPhrase(this.session, "Request failed with http code Forbidden"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 403 Forbidden. Free/Busy Request failed with http code Forbidden.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_403s|HTTP_403_FreeBusy_Request_failed_with_http_code_Forbidden");
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

            var sessionFlags_HTTP403_FreeBusyForbidden = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_403s_FreeBusy_Request_failed_with_http_code_Forbidden",

                SessionType = RulesetLangHelper.GetString("HTTP_403s_FreeBusy_Request_failed_with_http_code_Forbidden_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_403s_FreeBusy_Request_failed_with_http_code_Forbidden_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_403s_FreeBusy_Request_failed_with_http_code_Forbidden_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_403s_FreeBusy_Request_failed_with_http_code_Forbidden_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };
            var sessionFlagsJson_HTTP403_FreeBusyForbidden = JsonConvert.SerializeObject(sessionFlags_HTTP403_FreeBusyForbidden);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson_HTTP403_FreeBusyForbidden, false);
        }

        private void HTTP_403_Forbidden_Everything_Else(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 403 Forbidden.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_403s|HTTP_403_Forbidden_Everything_Else");
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
                SectionTitle = "HTTP_403s",

                SessionType = RulesetLangHelper.GetString("HTTP_403_Forbidden_Everything_Else_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_403_Forbidden_Everything_Else_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_403_Forbidden_Everything_Else_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_403_Forbidden_Everything_Else_ResponseComments"),

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
