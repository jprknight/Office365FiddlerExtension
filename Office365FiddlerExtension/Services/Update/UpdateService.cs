using Fiddler;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Class which is responsible for actually updating Version, URL, and Session Classification Json data from Github repo, 
    /// and Microsoft 365 URLs and IPs.
    /// </summary>
    public class UpdateService
    {
        private static UpdateService _instance;
        public static UpdateService Instance => _instance ?? (_instance = new UpdateService());

        /// <summary>
        /// Check for updates for URLs, extension version, session classification data, and Microsoft 365 URLs web service data from the web.
        /// </summary>
        public void Initialize()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): NeverWebCall enabled, returning.");
                return;
            }

            if (!EligibleForUpdateCheck())
            {
                var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

                if (extensionSettings.DebugMode)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"NextUpdateCheck is {extensionSettings.NextUpdateCheck}, but checking for updates now anyway.");
                }
                else
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"Waiting until {extensionSettings.NextUpdateCheck} before checking for any updates.");
                    return;
                }
            }
            
            UpdateURLsJsonFromGithub();
            UpdateVersionJsonFromGithub();
            UpdateSessionClassificationJsonFromGithub();
            UpdateMicrosft365URLsIPsFromWeb();
        }

        /// <summary>
        /// Determine if enough time has passed between the last update check and now.
        /// </summary>
        /// <returns></returns>
        public bool EligibleForUpdateCheck()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            if (DateTime.Now > extensionSettings.NextUpdateCheck.ToLocalTime())
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"Next update check timestamp met ({extensionSettings.NextUpdateCheck}), allowing application to check for updates.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Update session classification Json from Github repo.
        /// </summary>
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

                    if (!JsonValidatorService.Instance.IsValidJsonString(jsonString))
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Invalid Json in SessionClassification from Github, not updating locally.");
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {jsonString}");
                        return;
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
                        
                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving SessionClassification from Github {ex}");
                }
            }
        }

        /// <summary>
        /// Update version Json from Github repo.
        /// </summary>
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

                    if (!JsonValidatorService.Instance.IsValidJsonString(jsonString))
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Invalid Json in ExtensionVersion from Github, not updating locally.");
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {jsonString}");
                        return;
                    }

                    // Save this new data into the ExtensionVerison Fiddler setting.
                    if (Preferences.ExtensionVersion != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExtensionVersion Fiddler setting updated.");
                        
                        Preferences.ExtensionVersion = jsonString;

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                    else
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExtensionVersion Fiddler setting no update needed.");

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ExtensionVersion from Github {ex}");
                }
            }
        }

        /// <summary>
        /// Update URLs Json from Github repo.
        /// </summary>
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

                    if (!JsonValidatorService.Instance.IsValidJsonString(jsonString))
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Invalid Json in ExtensionURLs from Github, not updating locally.");
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {jsonString}");                        
                        return;
                    }

                    // Save this new data into the ExtensionURLs Fiddler setting.
                    if (Preferences.ExtensionURLs != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExtensionURLs Fiddler setting updated.");
                        Preferences.ExtensionURLs = jsonString;

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                    else
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExtensionURLs Fiddler setting no update needed.");

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ExtensionURLs from Github {Preferences.ExtensionURLs}");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ExtensionURLs from Github {ex}");
                }
            }
        }

        /// <summary>
        /// Update the Microsoft 365 URLs and IP addresses data from the web. Store it in an application preference for use in session analysis.
        /// Function intended to only be run once per Fiddler session to avoid any 429 "Too Many Requests" from the data source.
        /// </summary>
        private async void UpdateMicrosft365URLsIPsFromWeb()
        {
            var extensionURLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();

            using (var getSettings = new HttpClient())
            {
                try
                {
                    var response = await getSettings.GetAsync(extensionURLs.MicrosoftURLsIPsWebService);

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

                    if (!JsonValidatorService.Instance.IsValidJsonString(jsonString))
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Invalid Json in MicrosoftURLsIPsWebService from Github, not updating locally.");
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {jsonString}");
                        return;
                    }

                    // Save this new data into the SessionClassification Fiddler setting.
                    if (Preferences.MicrosoftURLsIPsWebService != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): MicrosoftURLsIPsWebService Fiddler setting updated.");
                        Preferences.MicrosoftURLsIPsWebService = jsonString;
                    }
                    else
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): MicrosoftURLsIPsWebService Fiddler setting no update needed.");

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving MicrosoftURLsIPsWebService from Github {ex}");
                }
            }
        }
    }
}
