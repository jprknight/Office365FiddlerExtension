using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_3S_Suggestions
    {
        internal Session session { get; set; }

        private static HTTP_200_3S_Suggestions _instance;

        public static HTTP_200_3S_Suggestions Instance => _instance ?? (_instance = new HTTP_200_3S_Suggestions());

        /// <summary>
        /// 3S Suggestions call.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} 200 3S Suggestions call.");

            // If this isn't a 3G Suggestions call, return.
            if (!this.session.uriContains("search/api/v1/suggestions"))
            {
                return;
            }

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_3S_Suggestions");
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
            var queryStrings = HttpUtility.ParseQueryString(uri.Query);
            var scenario = queryStrings["scenario"] ?? "scenario not specified in url";
            var entityTypes = queryStrings["entityTypes"] ?? "entityTypes not specified in url";
            var clientRequestId = this.session.RequestHeaders.Where(x => x.Name.Equals("client-request-id")).FirstOrDefault();

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_3S_Suggestions",

                SessionType = "3S Suggestions",
                ResponseCodeDescription = "200 OK 3S Suggestions",
                ResponseAlert = "3S Suggestions",
                ResponseComments = $"Scenario: {scenario} Types: {entityTypes} {clientRequestId}",

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
