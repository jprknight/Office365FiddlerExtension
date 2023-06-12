using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Handlers
{
    public class VersionHandler
    {
        private static VersionHandler _instance;
        public static VersionHandler Instance => _instance ?? (_instance = new VersionHandler());

        public ExtensionVersionFlags GetDeserializedExtensionVersion()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<ExtensionVersionFlags>(Preferences.ExtensionVersion, JsonSettings);
        }

        public void UpdateExtensionVersionFiddlerSetting()
        {
            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            var VersionItems = new
            {
                UpdateMessage = "test", // REVIEW THIS. Needs to be pulled from ExtensionVersion.json in Github.
                ExtensionDLL = Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8),
                VersionMajor = applicationVersion.Major,
                VersionMinor = applicationVersion.Minor,
                VersionBuild = applicationVersion.Build,
                RulesetLastUpdated = ""
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(VersionItems);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionVersion = jsonData;
        }

        public class ExtensionVersionFlags
        {
            public string UpdateMessage { get; set; }

            public int VersionMajor { get; set; }

            public int VersionMinor { get; set; }

            public int VersionBuild { get; set; }

            public DateTime MasterRulesetVersion { get; set; }

            public DateTime BetaRulesetVersion { get; set; }

            public DateTime NextUpdateCheck { get; set; }
        }
    }
}
