﻿using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_FreeBusy
    {
        internal Session session { get; set; }

        private static HTTP_200_FreeBusy _instance;

        public static HTTP_200_FreeBusy Instance => _instance ?? (_instance = new HTTP_200_FreeBusy());

        public void Run(Session session)
        {
            this.session = session;

            Legacy_FreeBusy(this.session);
            Outlook_For_Windows_FreeBusy(this.session);
            OWA_FreeBusy(this.session);
        }

        private void Legacy_FreeBusy(Session session)
        {
            this.session = session;

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("WSSecurity")
                && (!this.session.fullUrl.Contains("GetUserAvailability")
                && !(this.session.utilFindInResponse("GetUserAvailability", false) > 1)))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running Legacy_FreeBusy");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Legacy_FreeBusy");

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
                SectionTitle = "Legacy_Free/Busy",

                SessionType = LangHelper.GetString("HTTP_200_Legacy_FreeBusy_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_200_Legacy_FreeBusy_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_200_Legacy_FreeBusy_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_200_Legacy_FreeBusy_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void Outlook_For_Windows_FreeBusy(Session session)
        {
            // If the session doesn't contain any of these features, return.
            this.session = session;

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("outlook.office.com/CalendarService/api/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running Outlook_for_Windows_FreeBusy");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Outlook_For_Windows_FreeBusy");

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
                SectionTitle = "Outlook_For_Windows_Free/Busy",

                SessionType = LangHelper.GetString("HTTP_200_Outlook_For_Windows_FreeBusy_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_200_Outlook_For_Windows_FreeBusy_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_200_Outlook_For_Windows_FreeBusy_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_200_Outlook_For_Windows_FreeBusy_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void OWA_FreeBusy(Session session)
        {
            // If the session doesn't contain any of these features, return.
            this.session = session;

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("outlook.office.com/CalendarService/")
                && !this.session.fullUrl.Contains("outlook.office.com/outlookgatewayb2/")
                && !this.session.fullUrl.Contains("outlook.office.com/SchedulingB2/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running OWA_FreeBusy");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OWA_FreeBusy");

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
                SectionTitle = "OWA_Free/Busy",

                SessionType = LangHelper.GetString("HTTP_200_OWA_FreeBusy_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_200_OWA_FreeBusy_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_200_OWA_FreeBusy_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_200_OWA_FreeBusy_ResponseComments"),

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