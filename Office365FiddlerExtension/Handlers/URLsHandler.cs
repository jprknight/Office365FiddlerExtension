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

        public ExtensionURLs GetDeserializedExtensionURLs()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<ExtensionURLs>(Preferences.ExtensionURLs, JsonSettings);
        }

        public void CreateExtensionURLFiddlerSetting()
        {
            // If the Extension URLs Json already exists, none of this needs to run.
            if (Preferences.ExtensionURLs != null || Preferences.ExtensionURLs == "")
            {
                return;
            }

            // REVIEW THIS. URLs needs to move to master once it's a valid URL.

            var URLs = new
            {
                TelemetryInstrumentationKey = "87fb55ab-0052-4970-9318-7c740220e3c0",
                ExtensionVerisonJson = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/ExtensionVersion.json",
                UpdateJson = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/settings.json",
                MasterRuleSet = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Master/RulesetVersion",
                BetaRuleSet = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/RulesetVersion",
                Installer = "https://github.com/jprknight/EXOFiddlerExtension/releases/latest",
                Wiki = "https://github.com/jprknight/Office365FiddlerExtension/wiki",
                ReportIssues = "https://github.com/jprknight/Office365FiddlerExtension/issues"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(URLs);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionURLs = jsonData;
        }

        public class ExtensionURLs
        {
            public string TelemetryInstrumentationKey { get; set; }

            public string ExtensionVersionJson { get; set; }

            public string UpdateJson { get; set; }

            public string MasterRuleSet { get; set; }

            public string BetaRuleSet { get; set; }

            public string Installer { get; set; }

            public string Wiki { get; set; }

            public string WikiSessionTimeThresholds { get; set; }

            public string ReportIssues { get; set; }
        }
    }
}
