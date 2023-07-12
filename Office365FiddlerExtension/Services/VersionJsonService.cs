using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                return JsonConvert.DeserializeObject<ExtensionVersionFlags>(ExtensionVersion, JsonSettings);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing extension version.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        // Setting to store Json version information to run update checks against. Updated from remote.
#pragma warning disable IDE0052
        private static string _extensionVersion;

        public static string ExtensionVersion
        {
            get => _extensionVersion = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", null);
            set { _extensionVersion = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", value); }
        }

        public void CreateExtensionVersionFiddlerSetting()
        {
            if (Preferences.NeverWebCall)
            {
                var VersionItems = new
                {
                    UpdateMessage = "",
                    ExtensionMajor = "",
                    ExtensionMinor = "",
                    ExtensionBuild = "",
                    RulesetMajor = "",
                    RulesetMinor = "",
                    RulesetBuild = "",
                    ExtensionZip = "",
                    RulesetZip = "",
                    RulesetDLLPattern = ""
                };

                // Transform the object to a Json object.
                string jsonData = JsonConvert.SerializeObject(VersionItems);

                // Save the new Json to the Fiddler setting.
                ExtensionVersion = jsonData;
            }
            else
            {
                UpdateService.Instance.CreateVersionJsonFromGithub();
            }

        }

        public class ExtensionVersionFlags
        {
            public string UpdateMessage { get; set; }

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
