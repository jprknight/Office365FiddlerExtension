using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Handlers
{
    public class URLsHandler
    {
        private static URLsHandler _instance;
        public static URLsHandler Instance => _instance ?? (_instance = new URLsHandler());

        public ExtensionURLsJson GetDeserializedExtensionURLs()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<ExtensionURLsJson>(Preferences.ExtensionURLs, JsonSettings);
        }

        public void CreateExtensionURLFiddlerSetting()
        {
            // If the Extension URLs Json already exists, none of this needs to run.
            if (Preferences.ExtensionURLs != null || Preferences.ExtensionURLs == "")
            {
                return;
            }

            // REVIEW THIS. URLs needs to move to master once it's a valid URL.
            // Requires pull request of this branch into master.

            var URLs = new
            {
                TelemetryInstrumentationKey = "87fb55ab-0052-4970-9318-7c740220e3c0",
                ExtensionVerison = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/ExtensionVersion.json",
                UpdateJson = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/settings.json",
                MasterRuleSet = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Master/RulesetVersion",
                BetaRuleSet = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/RulesetVersion",
                Installer = "https://github.com/jprknight/EXOFiddlerExtension/releases/latest",
                Wiki = "https://github.com/jprknight/Office365FiddlerExtension/wiki",
                WikiSessionTimeThresholds = "https://github.com/jprknight/Office365FiddlerExtension/wiki/Session-Time-Thresholds",
                ReportIssues = "https://github.com/jprknight/Office365FiddlerExtension/issues"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(URLs);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionURLs = jsonData;
        }

        public String TelemetryInstrumentationKey
        {
            get
            {
                return GetDeserializedExtensionURLs().TelemetryInstrumentationKey;
            }
        }

        public string ExtensionVersion
        {
            get
            {
                return URLsHandler.Instance.GetDeserializedExtensionURLs().ExtensionVersion;
            }
        }

        // Function has getters only. Individual elements are read-only.
        public class ExtensionURLsJson
        {
            public string TelemetryInstrumentationKey { get; }

            // Used for the URL to the ExtensionVersion Json resource.
            public string ExtensionVersion { get; }

            public string UpdateJson { get; }

            public string MasterRuleSet { get; }

            public string BetaRuleSet { get; }

            public string Installer { get; }

            public string Wiki { get; }

            public string WikiSessionTimeThresholds { get; }

            public string ReportIssues { get; }
        }
    }
}
