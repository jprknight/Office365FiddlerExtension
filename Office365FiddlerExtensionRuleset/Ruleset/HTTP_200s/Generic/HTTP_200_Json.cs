using Fiddler;
using System;
using System.Reflection;
using Office365FiddlerExtensionRuleset.Services;
using Newtonsoft.Json;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    public class HTTP_200_Json
    {
        internal Session session { get; set; }

        private static HTTP_200_Json _instance;

        public static HTTP_200_Json Instance => _instance ?? (_instance = new HTTP_200_Json());

        public void Run(Session session)
        {
            this.session = session;

            if (!this.session.ResponseHeaders["Content-Type"].Contains("json"))
            {
                return;
            }

            // Valid Json in response.
            if (Office365FiddlerExtension.Services.JsonValidatorService.Instance.IsValidJsonSession(this.session))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Json");

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
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Json");
                    sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                    sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                    sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                    sessionSeverity = sessionClassificationJson.SessionSeverity;
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Severity: {sessionClassificationJson.SessionSeverity}");
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
                }

                var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s",

                    SessionType = RulesetLangHelper.GetString("HTTP_200_Json_SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_Json_ResponseCodeDescription"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_200_Json_ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_200_Json_ResponseComments"),

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

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} HTTP 200 Json; severity: {RulesetSessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionSeverity}");
            }
            // Invalid Json in response.
            else
            {
                // Empty response body.
                if (this.session.GetResponseBodyAsString() == null || this.session.GetResponseBodyAsString() == "")
                {

                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 No Json");

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
                        var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Json_Empty");
                        sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                        sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                        sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                        sessionSeverity = sessionClassificationJson.SessionSeverity;
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                            $"{this.session.id} {sessionClassificationJson.SessionSeverity}");
                    }
                    catch (Exception ex)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                            $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
                    }


                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s",

                        SessionType = RulesetLangHelper.GetString("HTTP_200_Json_EmptyResponseBody_SessionType"),
                        ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_Json_EmptyResponseBody_ResponseCodeDescription"),
                        ResponseAlert = RulesetLangHelper.GetString("HTTP_200_Json_EmptyResponseBody_ResponseAlert"),
                        ResponseComments = $"{RulesetLangHelper.GetString("HTTP_200_Json_EmptyResponseBody_ResponseComments")}",

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
                // Non-empty invalid Json response body.
                else
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Invalid Json");

                    int sessionAuthenticationConfidenceLevel = 0;
                    int sessionTypeConfidenceLevel = 0;
                    int sessionResponseServerConfidenceLevel = 0;
                    int sessionSeverity = 0;

                    int sessionAuthenticationConfidenceLevelFallback = 5;
                    int sessionTypeConfidenceLevelFallback = 10;
                    int sessionResponseServerConfidenceLevelFallback = 5;
                    int sessionSeverityFallback = 50;

                    try
                    {
                        var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Json_Invalid");
                        sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                        sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                        sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                        sessionSeverity = sessionClassificationJson.SessionSeverity;
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                            $"{this.session.id} {sessionClassificationJson.SessionSeverity}");
                    }
                    catch (Exception ex)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                            $"{this.session.id} SESSION CLASSIFICATION EXTERNAL JSON FILE EXCEPTION: {ex}");
                    }


                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s",

                        SessionType = RulesetLangHelper.GetString("HTTP_200_Json_Invalid_SessionType"),
                        ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_Json_Invalid_ResponseCodeDescription"),
                        ResponseAlert = RulesetLangHelper.GetString("HTTP_200_Json_Invalid_ResponseAlert"),
                        ResponseComments = $"{RulesetLangHelper.GetString("HTTP_200_Json_Invalid_ResponseComments")}",

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
}
