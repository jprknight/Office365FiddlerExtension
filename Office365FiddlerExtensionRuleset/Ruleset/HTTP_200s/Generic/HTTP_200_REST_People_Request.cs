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

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

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
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 5;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 30;
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
