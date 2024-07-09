using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Services
{
    /// <summary>
    /// Process the session classification Json the extension uses for sessions per response code.
    /// </summary>
    public class RulesetSessionClassificationService
    {
        internal Session session { get; set; }

        private static RulesetSessionClassificationService _instance;
        public static RulesetSessionClassificationService Instance => _instance ?? (_instance = new RulesetSessionClassificationService());

        /// <summary>
        /// Expecting a "Section|Section" to be passed into function.
        /// Function allows multiple depths to be passed in. Expecting 2 or 3 is the most likely use case.
        /// </summary>
        /// <param name="section"></param>
        public SessionClassificationJsonSection GetSessionClassificationJsonSection(string section)
        {
            string sectionPiece0 = "";
            string sectionPiece1 = "";

            var jsonSection = "";

            var parsedObject = JObject.Parse(Preferences.SessionClassification);

            if (section.Contains('|')) {
                string[] sectionPieces = section.Split('|');

                sectionPiece0 = sectionPieces[0];
                sectionPiece1 = sectionPieces[1];
                jsonSection = parsedObject[sectionPiece0][sectionPiece1].ToString();
            }
            else
            {
                jsonSection = parsedObject[section].ToString();
            }

            return JsonConvert.DeserializeObject<SessionClassificationJsonSection>(jsonSection);
        }
    }

    public class SessionClassificationJsonSection
    {
        public string SectionTitle { get; set; }

        public string SessionType { get; set; }

        public string SessionResponseCodeDescription { get; set; }

        public string SessionResponseComments { get; set; }

        public string SessionResponseServer { get; set; }

        public string SessionResponseAlert { get; set; }

        public string SessionAuthentication { get; set; }

        public int SessionAuthenticationConfidenceLevel { get; set; }

        public int SessionTypeConfidenceLevel { get; set; }

        public int SessionResponseServerConfidenceLevel { get; set; }

        public int SessionSeverity { get; set; }
    }
}
