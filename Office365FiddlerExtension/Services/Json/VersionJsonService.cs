using Fiddler;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Functions to ensure ExtensionVersion Json is always created and populated with data.
    /// </summary>
    public class VersionJsonService
    {
        private static VersionJsonService _instance;
        public static VersionJsonService Instance => _instance ?? (_instance = new VersionJsonService());

        public ExtensionVersionFlags GetDeserializedExtensionVersion()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            try
            {
                return JsonConvert.DeserializeObject<ExtensionVersionFlags>(Preferences.ExtensionVersion, JsonSettings);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing extension version.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        public void CreateExtensionVersionFiddlerSetting()
        {
            var VersionItems = new
            {
                ExtensionMajor = Assembly.GetExecutingAssembly().GetName().Version.Major,
                ExtensionMinor = Assembly.GetExecutingAssembly().GetName().Version.Minor,
                ExtensionBuild = Assembly.GetExecutingAssembly().GetName().Version.Build,
                RulesetMajor = "1776",
                RulesetMinor = "7",
                RulesetBuild = "4",
                ExtensionZip = "Office365FiddlerExtension.zip",
                RulesetZip = "Office365FiddlerExtensionRuleset.zip",
                RulesetDLLPattern = "Office365FiddlerExtensionRuleset*.dll"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(VersionItems);

            // Save the new Json to the Fiddler setting.
            Preferences.ExtensionVersion = jsonData;
        }

        public class ExtensionVersionFlags
        {
            public int ExtensionMajor { get; set; }

            public int ExtensionMinor { get; set; }

            public int ExtensionBuild { get; set; }

            public int RulesetMajor { get; set; }

            public int RulesetMinor { get; set;}

            public int RulesetBuild { get; set;}

            public string ExtensionZip { get; set;}

            public string RulesetZip { get; set;}

            public string RulesetDLLPattern { get; set; }
        }
    }
}
