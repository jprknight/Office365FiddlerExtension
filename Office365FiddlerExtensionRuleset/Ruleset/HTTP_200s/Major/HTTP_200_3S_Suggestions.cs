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

            // If this isn't a 3G Suggestions call, return.
            if (!this.session.uriContains("search/api/v1/suggestions"))
            {
                return;
            }

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_3S_Suggestions");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} 200 3S Suggestions call.");

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

                SessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel,
                SessionSeverity = sessionClassificationJson.SessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
