using Fiddler;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Class to update Version and URL Json data from Github repo.
    /// </summary>
    public class UpdateService
    {
        private static UpdateService _instance;
        public static UpdateService Instance => _instance ?? (_instance = new UpdateService());

        public void Initialize()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): NeverWebCall enabled, returning.");
                return;                    
            }
            
            /*
            REVIEW THIS -- UNCOMMENT THIS CODE BEFORE GOING PRODUCTION.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            if (DateTime.Now < extensionSettings.NextUpdateCheck)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Next update check timestamp not met ({extensionSettings.NextUpdateCheck}), returning.");
                return;
            }
            */

            UpdateURLsJsonFromGithub();
            UpdateVersionJsonFromGithub();
            // REVIEW THIS -- Uncomment this before going production.
            //UpdateSessionClassificationJsonFromGithub();
        }

        private async void UpdateSessionClassificationJsonFromGithub()
        {
            var extensionURLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();

            using (var getSettings = new HttpClient())
            {
                try
                {
                    var response = await getSettings.GetAsync(extensionURLs.SessionClassification);

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

                    // Save this new data into the SessionClassification Fiddler setting.
                    if (Preferences.SessionClassification != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): SessionClassification Fiddler setting updated.");
                        Preferences.SessionClassification = jsonString;

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                    else
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): SessionClassification Fiddler setting no update needed.");
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving SessionClassification from Github {ex}");
                }
            }
        }

        private async void UpdateVersionJsonFromGithub()
        {
            var extensionURLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();

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
                    if (VersionJsonService.ExtensionVersion != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeVersion Fiddler setting updated.");
                        VersionJsonService.ExtensionVersion = jsonString;

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                    else
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeVersion Fiddler setting no update needed.");
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ExchangeVersion from Github {ex}");
                }
            }
        }

        private async void UpdateURLsJsonFromGithub()
        {
            var extensionURLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();

            using (var getSettings = new HttpClient())
            {
                try
                {
                    var response = await getSettings.GetAsync(extensionURLs.ExtensionURL);

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

                    // Save this new data into the ExtensionURLs Fiddler setting.
                    if (URLsJsonService.ExtensionURLs != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExtensionURLs Fiddler setting updated.");
                        URLsJsonService.ExtensionURLs = jsonString;

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                    else
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExtensionURLs Fiddler setting no update needed.");
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ExtensionURLs from Github {URLsJsonService.ExtensionURLs}");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ExtensionURLs from Github {ex}");
                }
            }
        }
    }
}
