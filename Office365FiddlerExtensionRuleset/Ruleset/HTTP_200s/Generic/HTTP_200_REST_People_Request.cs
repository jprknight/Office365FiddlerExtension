using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtensionRuleset.Services;
using System;
using System.Linq;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset.HTTP_200s
{
    internal class HTTP_200_REST_People_Request
    {
        internal Session session { get; set; }

        private static HTTP_200_REST_People_Request _instance;

        public static HTTP_200_REST_People_Request Instance => _instance ?? (_instance = new HTTP_200_REST_People_Request());

        /// <summary>
        /// REST People Request.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // if the session Uri isn't for People, return;
            if (!this.session.uriContains("people"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} 200 REST - People Request.");

            int sessionAuthenticationConfidenceLevel = 0;
            int sessionTypeConfidenceLevel = 0;
            int sessionResponseServerConfidenceLevel = 0;
            int sessionSeverity = 0;

            int sessionAuthenticationConfidenceLevelFallback = 5;
            int sessionTypeConfidenceLevelFallback = 5;
            int sessionResponseServerConfidenceLevelFallback = 5;
            int sessionSeverityFallback = 30;

            try
            {
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_REST_People_Request");
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

            Uri uri = new Uri(this.session.fullUrl);
            var queryStrings = System.Web.HttpUtility.ParseQueryString(uri.Query);

            string sessionType = "";

            // /me/people : : Private FindPeople Request
            if (this.session.uriContains("/me/people"))
            {
                sessionType = "Private";
            }

            // /users()/people : Public FindPeople Request
            else if (this.session.uriContains("/users(") && this.session.uriContains("/people"))
            {
                sessionType = "Public";
            }

            var requestId = this.session.ResponseHeaders.Where(x => x.Name.Equals("request-id")).FirstOrDefault();

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s",

                SessionType = $"{RulesetLangHelper.GetString("HTTP_200_REST_People_Request_SessionType")} {sessionType}",
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_200_REST_People_Request_ResponseCodeDescription"),
                ResponseAlert = $"{RulesetLangHelper.GetString("HTTP_200_REST_People_Request_ResponseAlert")} {sessionType}",
                ResponseComments = $"{requestId} $search:{queryStrings["$search"]} $top:{queryStrings["$top"]} " +
                    $"$skip:{queryStrings["$skip"]} $select:{queryStrings["$select"]} $filter:{queryStrings["$filter"]}",

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
