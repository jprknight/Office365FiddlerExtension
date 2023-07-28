using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Ruleset
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

            var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_REST_People_Request");

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} 200 REST - People Request.");

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

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_200s_REST_People_Request",

                SessionType = $"REST People {sessionType}",
                ResponseCodeDescription = "200 OK REST People Request",
                ResponseAlert = $"REST People {sessionType}",
                ResponseComments = $"{requestId} $search:{queryStrings["$search"]} $top:{queryStrings["$top"]} $skip:{queryStrings["$skip"]} $select:{queryStrings["$select"]} $filter:{queryStrings["$filter"]}",

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
