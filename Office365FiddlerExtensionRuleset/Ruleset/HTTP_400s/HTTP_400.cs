using System;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_400
    {
        internal Session session { get; set; }

        private static HTTP_400 _instance;

        public static HTTP_400 Instance => _instance ?? (_instance = new HTTP_400());

        /// <summary>
        /// Set session analysis values for a HTTP 400 response code.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_HTTP_400_Cloud_Authentication = Stopwatch.StartNew();

            HTTP_400_Cloud_Authentication(this.session);

            sw_HTTP_400_Cloud_Authentication.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_400_Cloud_Authentication");
                TelemetryService.CustomTrackMetric("RS_HTTP_400_Cloud_Authentication", sw_HTTP_400_Cloud_Authentication.ElapsedMilliseconds);
            }

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            ///////////////////////////////

            var sw_HTTP_400_Everything_Else = Stopwatch.StartNew();

            HTTP_400_Everything_Else(this.session);

            sw_HTTP_400_Everything_Else.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_400_Everything_Else");
                TelemetryService.CustomTrackMetric("RS_HTTP_400_Everything_Else", sw_HTTP_400_Everything_Else.ElapsedMilliseconds);
            }
        }

        public void HTTP_400_Cloud_Authentication(Session session)
        {
            this.session = session;

            if (this.session.hostname != "login.microsoftonline.com")
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 400 Bad Request, cloud authentication.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_400s|HTTP_400_Cloud_Authentication");

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

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_400s",

                SessionType = RulesetLangHelper.GetString("HTTP_400_Cloud_Authentication_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_400_Cloud_Authentication_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_400_Cloud_Authentication_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_400_Cloud_Authentication_ResponseComments"),

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
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        public void HTTP_400_Everything_Else(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                $"{this.session.id} HTTP 400 Bad Request, everything else.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_400s|HTTP_400_Everything_Else");
                
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

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_400s",

                SessionType = RulesetLangHelper.GetString("HTTP_400s_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_400s_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_400s_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_400s_ResponseComments"),

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
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}