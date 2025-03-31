using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    class HTTP_200_FreeBusy
    {
        internal Session session { get; set; }

        private static HTTP_200_FreeBusy _instance;

        public static HTTP_200_FreeBusy Instance => _instance ?? (_instance = new HTTP_200_FreeBusy());

        public void Run(Session session)
        {
            this.session = session;

            FreeBusy_ProxyWebRequestFailed(this.session);
            
            FreeBusy_The_Operation_Was_Cancelled(this.session);
            
            FreeBusy_Failure_Result_Set_Too_Many_Calendar_Entries(this.session);
            
            FreeBusy_Other(this.session);

            //Legacy_FreeBusy(this.session);

            //Outlook_For_Windows_FreeBusy(this.session);

            //OWA_FreeBusy(this.session);
        }

        private void FreeBusy_The_Operation_Was_Cancelled(Session session)
        {
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.IsFreeBusySession(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.SearchForPhrase(this.session, "The operation was canceled"))
            {
                return;
            }

            if (!RulesetUtilities.Instance.SearchForPhrase(this.session, "availabilityView"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running FreeBusy The operation was canceled.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_FreeBusy_The_Operation_Was_Cancelled");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Free/Busy_The_Operation_Was_Cancelled",

                SessionType = RulesetLangHelper.GetString("HTTP_200_FreeBusy_The_Operation_Was_Cancelled_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_FreeBusy_The_Operation_Was_Cancelled_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_FreeBusy_The_Operation_Was_Cancelled_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_FreeBusy_The_Operation_Was_Cancelled_ResponseComments"),

                SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                    sessionAuthenticationConfidenceLevelFallback),

                SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                    sessionTypeConfidenceLevelFallback),

                SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                    sessionResponseServerConfidenceLevelFallback),

                SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                    sessionSeverityFallback)
            };

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Updating session flags.");
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        private void FreeBusy_ProxyWebRequestFailed(Session session)
        {
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.IsFreeBusySession(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.SearchForPhrase(this.session, "Proxy web request failed"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running FreeBusy Proxy Web Request Failed.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_FreeBusy_Proxy_Web_Request_Failed");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Free/Busy_Proxy_Web_Request_Failed",

                SessionType = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Proxy_Web_Request_Failed_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Proxy_Web_Request_Failed_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Proxy_Web_Request_Failed_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Proxy_Web_Request_Failed_ResponseComments"),

                SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                    sessionAuthenticationConfidenceLevelFallback),

                SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                    sessionTypeConfidenceLevelFallback),

                SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                    sessionResponseServerConfidenceLevelFallback),

                SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                    sessionSeverityFallback)
            };

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Updating session flags.");
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        private void FreeBusy_Failure_Result_Set_Too_Many_Calendar_Entries(Session session)
        {
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.IsFreeBusySession(this.session))
            {
                return;
            }

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("outlook.office365.com")) 
            {
                return;
            }

            if (!RulesetUtilities.Instance.SearchForPhrase(this.session, "GetUserAvailability"))
            {
                return;
            }

            if (!RulesetUtilities.Instance.SearchForPhrase(this.session, "The result set contains too many calendar entries"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running FreeBusy Result set too many calendar items.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_FreeBusy_Result_Set_Too_Many_Calendar_Items");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Free/Busy_Result_Set_Too_Many_Calendar_Items",

                SessionType = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Result_Set_Too_Many_Calendar_Items_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Result_Set_Too_Many_Calendar_Items_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Result_Set_Too_Many_Calendar_Items_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Result_Set_Too_Many_Calendar_Items_ResponseComments"),

                SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                    sessionAuthenticationConfidenceLevelFallback),

                SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                    sessionTypeConfidenceLevelFallback),

                SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                    sessionResponseServerConfidenceLevelFallback),

                SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                    sessionSeverityFallback)
            };

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Updating session flags.");
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// All other Free/Busy sessions not detected as an issue.
        /// </summary>
        /// <param name="session"></param>
        private void FreeBusy_Other(Session session)
        {
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.IsFreeBusySession(this.session))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running FreeBusy_Other");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_FreeBusy_Other");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Free/Busy_Other",

                SessionType = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Other_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Other_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Other_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_FreeBusy_Other_ResponseComments"),

                SessionAuthenticationConfidenceLevel = RulesetUtilities.Instance.ValidateSessionAuthenticationConfidenceLevel(sessionAuthenticationConfidenceLevel,
                    sessionAuthenticationConfidenceLevelFallback),

                SessionTypeConfidenceLevel = RulesetUtilities.Instance.ValidateSessionTypeConfidenceLevel(sessionTypeConfidenceLevel,
                    sessionTypeConfidenceLevelFallback),

                SessionResponseServerConfidenceLevel = RulesetUtilities.Instance.ValidateSessionResponseServerConfidenceLevel(sessionResponseServerConfidenceLevel,
                    sessionResponseServerConfidenceLevelFallback),

                SessionSeverity = RulesetUtilities.Instance.ValidateSessionSeverity(sessionSeverity,
                    sessionSeverityFallback)
            };

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Updating session flags.");
            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// REVIEW THIS. Remove this function and associated content in lang files and sessionclassification.json file at some point in the future.
        /// </summary>
        /// <param name="session"></param>
        private void Legacy_FreeBusy(Session session)
        {
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.IsFreeBusySession(this.session))
            {
                return;
            }

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("WSSecurity")
                && (!this.session.fullUrl.Contains("GetUserAvailability")
                && !(this.session.utilFindInResponse("GetUserAvailability", false) > 1)))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running Legacy_FreeBusy");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Legacy_FreeBusy");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Legacy_Free/Busy",

                SessionType = RulesetLangHelper.GetString("HTTP_200_Legacy_FreeBusy_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_Legacy_FreeBusy_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_Legacy_FreeBusy_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_Legacy_FreeBusy_ResponseComments"),

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
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// REVIEW THIS. Remove this function and associated content in lang files and sessionclassification.json file at some point in the future.
        /// </summary>
        /// <param name="session"></param>
        private void Outlook_For_Windows_FreeBusy(Session session)
        {
            // If the session doesn't contain any of these features, return.
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.IsFreeBusySession(this.session))
            {
                return;
            }

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("outlook.office.com/CalendarService/api/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running Outlook_for_Windows_FreeBusy");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Outlook_For_Windows_FreeBusy");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Outlook_For_Windows_Free/Busy",

                SessionType = RulesetLangHelper.GetString("HTTP_200_Outlook_For_Windows_FreeBusy_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_Outlook_For_Windows_FreeBusy_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_Outlook_For_Windows_FreeBusy_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_Outlook_For_Windows_FreeBusy_ResponseComments"),

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
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        /// <summary>
        /// REVIEW THIS. Remove this function and associated content in lang files and sessionclassification.json file at some point in the future.
        /// </summary>
        /// <param name="session"></param>
        private void OWA_FreeBusy(Session session)
        {
            // If the session doesn't contain any of these features, return.
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            if (!RulesetUtilities.Instance.IsFreeBusySession(this.session))
            {
                return;
            }

            // If the session doesn't contain any of these features, return.
            if (!this.session.fullUrl.Contains("outlook.office.com/CalendarService/")
                && !this.session.fullUrl.Contains("outlook.office.com/outlookgatewayb2/")
                && !this.session.fullUrl.Contains("outlook.office.com/SchedulingB2/"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running OWA_FreeBusy");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_OWA_FreeBusy");

                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "OWA_Free/Busy",

                SessionType = RulesetLangHelper.GetString("HTTP_200_OWA_FreeBusy_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_OWA_FreeBusy_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_200_OWA_FreeBusy_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_200_OWA_FreeBusy_ResponseComments"),

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
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }
    }
}
