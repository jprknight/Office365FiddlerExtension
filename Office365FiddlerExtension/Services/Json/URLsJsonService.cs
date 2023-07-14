using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Functions to ensure ExtensionURLs Json is created and populated with data.
    /// </summary>
    public class URLsJsonService
    {
        private static URLsJsonService _instance;
        public static URLsJsonService Instance => _instance ?? (_instance = new URLsJsonService());

        public ExtensionURLsJson GetDeserializedExtensionURLs()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            try
            {
                return JsonConvert.DeserializeObject<ExtensionURLsJson>(ExtensionURLs, JsonSettings);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing extension URLs.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        // Setting to store Json extension URLs. Update from remote.
#pragma warning disable IDE0052
        private static string _extensionURLs;

        public static string ExtensionURLs
        {
            get => _extensionURLs = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", null);
            set { _extensionURLs = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionURLs", value); }
        }

        public void CreateExtensionURLFiddlerSetting()
        {
            // If the Extension URLs Json already exists, none of this needs to run.
            if (ExtensionURLs != null || ExtensionURLs == "")
            {
                return;
            }

            // REVIEW THIS. URLs needs to move to master once it's a valid URL.
            // Requires pull request of this branch into master.

            var URLs = new
            {
                TelemetryInstrumentationKey = "87fb55ab-0052-4970-9318-7c740220e3c0",
                ExtensionURL = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/ExtensionURLs.json",
                ExtensionVersion = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/Code-Hygiene/Office365FiddlerExtension/ExtensionVersion.json",
                ExtensionInstaller = "https://github.com/jprknight/EXOFiddlerExtension/releases/latest",
                Wiki = "https://github.com/jprknight/Office365FiddlerExtension/wiki",
                WikiSessionTimeThresholds = "https://github.com/jprknight/Office365FiddlerExtension/wiki/Session-Time-Thresholds",
                WikiScoreForSession = "https://github.com/jprknight/Office365FiddlerExtension/wiki/What-is-ScoreForSession%3F",
                ReportIssues = "https://github.com/jprknight/Office365FiddlerExtension/issues"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(URLs);

            try
            {
                // Save the new Json to the Fiddler setting.
                ExtensionURLs = jsonData;
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): CreateExtensionURLFiddlerSetting written to ExtensionURLs Fiddler setting.");
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): CreateExtensionURLFiddlerSetting unable to write to ExtensionURLs Fiddler setting.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }
    }

    // Function has getters only. Individual elements are read-only.
    public class ExtensionURLsJson
    {
        public string TelemetryInstrumentationKey { get; set; }

        public string ExtensionURL { get; set; }

        public string ExtensionVersion { get; set; }

        public string ExtensionInstaller { get; set; }

        public string Wiki { get; set; }

        public string WikiSessionTimeThresholds { get; set; }

        public string WikiScoreForSession { get; set; }

        public string ReportIssues { get; set; }

        public string Ruleset { get; set; }
    }
}
