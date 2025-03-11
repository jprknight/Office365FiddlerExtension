using System;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;
using Office365FiddlerExtension.Services;
using System.Diagnostics;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_504
    {
        internal Session session { get; set; }

        private static HTTP_504 _instance;

        public static HTTP_504 Instance => _instance ?? (_instance = new HTTP_504());

        /// <summary>
        /// Set session analysis values for a HTTP 504 response code.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            var sw_HTTP_504_Gateway_Timeout_Internet_Access_Blocked = Stopwatch.StartNew();

            HTTP_504_Gateway_Timeout_Internet_Access_Blocked(this.session);

            sw_HTTP_504_Gateway_Timeout_Internet_Access_Blocked.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_504_Gateway_Timeout_Internet_Access_Blocked");
                TelemetryService.CustomTrackMetric("RS_HTTP_504_Gateway_Timeout_Internet_Access_Blocked", sw_HTTP_504_Gateway_Timeout_Internet_Access_Blocked.ElapsedMilliseconds);
            }

            ///////////////////////////////

            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            var sw_HTTP_504_Gateway_Timeout_Anything_Else = Stopwatch.StartNew();

            HTTP_504_Gateway_Timeout_Anything_Else(this.session);

            sw_HTTP_504_Gateway_Timeout_Anything_Else.Stop();

            if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                TelemetryService.CustomTrackEvent("RS_HTTP_504_Gateway_Timeout_Anything_Else");
                TelemetryService.CustomTrackMetric("RS_HTTP_504_Gateway_Timeout_Anything_Else", sw_HTTP_504_Gateway_Timeout_Anything_Else.ElapsedMilliseconds);
            }
        }

        private void HTTP_504_Gateway_Timeout_Internet_Access_Blocked(Session session)
        {
            // HTTP 504 Bad Gateway 'internet has been blocked'

            this.session = session;

            if (!(this.session.utilFindInResponse("internet", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("access", false) > 1))
            {
                return;
            }

            if(!(this.session.utilFindInResponse("blocked", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 504 Gateway Timeout -- Internet Access Blocked.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_504s|HTTP_504_Gateway_Timeout_Internet_Access_Blocked");
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
                SectionTitle = "HTTP_504s",

                SessionType = RulesetLangHelper.GetString("HTTP_504_Gateway_Timeout_Internet_Access_Blocked_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_504_Gateway_Timeout_Internet_Access_Blocked_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_504_Gateway_Timeout_Internet_Access_Blocked_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_504_Gateway_Timeout_Internet_Access_Blocked_ResponseComments"),

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

        private void HTTP_504_Gateway_Timeout_Anything_Else(Session session)
        {
            // Pick up any other 504 Gateway Timeout and write data into the comments box.

            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 504 Gateway Timeout.");

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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_504s|HTTP_504_Gateway_Timeout_Anything_Else");
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
                SectionTitle = "HTTP_504s",

                SessionType = RulesetLangHelper.GetString("HTTP_504_Gateway_Timeout_Anything_Else_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_504_Gateway_Timeout_Anything_Else_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_504_Gateway_Timeout_Anything_Else_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_504_Gateway_Timeout_Anything_Else_ResponseComments"),

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
