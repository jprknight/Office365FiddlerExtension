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

namespace Office365FiddlerExtension.Handler
{
    /// <summary>
    /// Functions to ensure ExtensionVersion Json is always created and populated with data.
    /// </summary>
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
        private static string _extensionVersion;

        public static string ExtensionVersion
        {
            get => _extensionVersion = FiddlerApplication.Prefs.GetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", null);
            set { _extensionVersion = value; FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.ExtensionVersion", value); }
        }

        public void CreateExtensionVersionFiddlerSetting()
        {
            var VersionItems = new
            {
                UpdateMessage = "",
                VersionMajor = "",
                VersionMinor = "",
                VersionBuild = ""
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(VersionItems);

            // Save the new Json to the Fiddler setting.
            ExtensionVersion = jsonData;
        }

        public class ExtensionVersionFlags
        {
            public string UpdateMessage { get; set; }

            public int VersionMajor { get; set; }

            public int VersionMinor { get; set; }

            public int VersionBuild { get; set; }

            public DateTime MasterRulesetVersion { get; set; }

            public DateTime BetaRulesetVersion { get; set; }
        }

        public async void UpdateVersionJsonFromGithub()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeVersion Fiddler setting update check started.");

            var extensionURLs = URLsHandler.Instance.GetDeserializedExtensionURLs();
            //var extensionVerison = VersionHandler.Instance.GetDeserializedExtensionVersion();
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();

            // If the current timestamp is less than the next update check timestamp, return.
            if (DateTime.Now < extensionSettings.NextUpdateCheck)
            {
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Next update check timestamp no met, returning.");
                //return;
            }

            using (var getSettings = new HttpClient())
            {
                try
                {
                    var response = await getSettings.GetAsync(extensionURLs.ExtensionVersion);

                    response.EnsureSuccessStatusCode();

                    var jsonString = string.Empty;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        stream.Position = 0;
                        using (var reader = new StreamReader(stream))
                        {
                            jsonString = await reader.ReadToEndAsync();
                        }
                    }

                    // Save this new data into the ExtensionVerison Fiddler setting.
                    if (ExtensionVersion != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeVersion Fiddler setting updated.");
                        ExtensionVersion = jsonString;
                        
                        // Update the next update check timestamp.
                        SettingsHandler.Instance.SetNextUpdateTimestamp();
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving settings from Github {ex}");
                }
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeVersion Fiddler setting update check finished.");
        }
    }
}
