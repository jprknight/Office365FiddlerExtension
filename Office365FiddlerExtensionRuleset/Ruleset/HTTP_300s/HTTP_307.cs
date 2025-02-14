using System;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_307
    {
        internal Session session { get; set; }

        private static HTTP_307 _instance;

        public static HTTP_307 Instance => _instance ?? (_instance = new HTTP_307());

        /// <summary>
        /// Set session analysis values for a HTTP 307 response code.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            HTTP_307_AutoDiscover_Temporary_Redirect(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            HTTP_307_Other_AutoDiscover_Redirects(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            HTTP_307_All_Other_Redirects(this.session);
        }

        public void HTTP_307_AutoDiscover_Temporary_Redirect(Session session)
        {
            this.session = session;

            // Specific scenario where a HTTP 307 Temporary Redirect incorrectly send an EXO Autodiscover request to an On-Premise resource, breaking Outlook connectivity.
            if (this.session.hostname.Contains("autodiscover") &&
                (this.session.hostname.Contains("mail.onmicrosoft.com") &&
                (this.session.fullUrl.Contains("autodiscover") &&
                (this.session.ResponseHeaders["Location"] != "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml"))))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 307 On-Prem Temp Redirect - Unexpected location!");

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
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP307s|HTTP_307_AutoDiscover_Temporary_Redirect");
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
                    SectionTitle = "HTTP_307s",

                    SessionType = RulesetLangHelper.GetString("HTTP_307_AutoDiscover_Temporary_Redirect_SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_307_AutoDiscover_Temporary_Redirect_ResponseCodeDescription"),
                    ResponseServer = RulesetLangHelper.GetString("HTTP_307_AutoDiscover_Temporary_Redirect_ResponseServer"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_307_AutoDiscover_Temporary_Redirect_ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_307_AutoDiscover_Temporary_Redirect_ResponseComments"),

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

        public void HTTP_307_Other_AutoDiscover_Redirects(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 307 Temp Redirect.");

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 5;
            int sessionTypeConfidenceLevelFallback = 10;
            int sessionResponseServerConfidenceLevelFallback = 5;
            int sessionSeverityFallback = 40;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP307s|HTTP_307_Other_AutoDiscover_Redirects");
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
                SectionTitle = "HTTP_307s",

                SessionType = RulesetLangHelper.GetString("HTTP_307_Other_AutoDiscover_Redirects_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_307_Other_AutoDiscover_Redirects_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_307_Other_AutoDiscover_Redirects_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_307_Other_AutoDiscover_Redirects_ResponseComments"),

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

        public void HTTP_307_All_Other_Redirects(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 307 Temp Redirect.");

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 5;
            int sessionTypeConfidenceLevelFallback = 10;
            int sessionResponseServerConfidenceLevelFallback = 5;
            int sessionSeverityFallback = 10;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP307s|HTTP_307_All_Other_Redirects");
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
                SectionTitle = "HTTP_307s",

                SessionType = RulesetLangHelper.GetString("HTTP_307_All_Other_Redirects_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_307_All_Other_Redirects_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_307_All_Other_Redirects_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_307_All_Other_Redirects_ResponseComments"),

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
