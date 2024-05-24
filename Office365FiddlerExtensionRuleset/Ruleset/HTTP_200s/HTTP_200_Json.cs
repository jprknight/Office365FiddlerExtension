using Fiddler;
using System;
using System.Reflection;
using Office365FiddlerExtension.Services;
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
            if (JsonValidatorService.Instance.IsValidJsonSession(this.session))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Json");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Json");
                    sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                    sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                    sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                    sessionSeverity = sessionClassificationJson.SessionSeverity;
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Severity: {sessionClassificationJson.SessionSeverity}");
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

                    SessionType = LangHelper.GetString("HTTP_200_Json_SessionType"),
                    ResponseCodeDescription = LangHelper.GetString("HTTP_200_Json_ResponseCodeDescription"),
                    ResponseAlert = LangHelper.GetString("HTTP_200_Json_ResponseAlert"),
                    ResponseComments = LangHelper.GetString("HTTP_200_Json_ResponseComments"),

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Json; severity: {SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionSeverity}");
            }
            // Invalid Json in response.
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 Invalid Json");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Json_Invalid");
                    sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                    sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                    sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                    sessionSeverity = sessionClassificationJson.SessionSeverity;
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {sessionClassificationJson.SessionSeverity}");
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                    sessionAuthenticationConfidenceLevel = 5;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 5;
                    sessionSeverity = 50;
                }

                // Empty response body.
                if (this.session.GetResponseBodyAsString() == null || this.session.GetResponseBodyAsString() == "")
                {
                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s",

                        SessionType = LangHelper.GetString("HTTP_200_Json_SessionType_EmptyResponseBody"),
                        ResponseCodeDescription = LangHelper.GetString("HTTP_200_Json_Invalid_ResponseCodeDescription"),
                        ResponseAlert = LangHelper.GetString("HTTP_200_Json_Invalid_ResponseAlert"),
                        ResponseComments = $"{LangHelper.GetString("HTTP_200_Json_EmptyResponseBody")}",

                        SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                        SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                        SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                        SessionSeverity = sessionSeverity
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
                // Something in the response body.
                else
                {
                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "HTTP_200s",

                        SessionType = LangHelper.GetString("HTTP_200_Json_Invalid_SessionType"),
                        ResponseCodeDescription = LangHelper.GetString("HTTP_200_Json_Invalid_ResponseCodeDescription"),
                        ResponseAlert = LangHelper.GetString("HTTP_200_Json_Invalid_ResponseAlert"),
                        ResponseComments = $"{LangHelper.GetString("HTTP_200_Json_Invalid_ResponseComments")} <p>{LangHelper.GetString("Response Body")}</p>{this.session.GetResponseBodyAsString()}",

                        SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                        SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                        SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                        SessionSeverity = sessionSeverity
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 invalid Json; severity: {SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionSeverity}");
            }
        }
    }
}
