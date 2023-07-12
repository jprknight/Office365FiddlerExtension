﻿using Fiddler;
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
    public class UpdateService
    {
        private static UpdateService _instance;
        public static UpdateService Instance => _instance ?? (_instance = new UpdateService());

        public void initialize()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): NeverWebCall enabled, returning.");
                return;                    
            }

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            if (DateTime.Now < extensionSettings.NextUpdateCheck)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Next update check timestamp not met ({extensionSettings.NextUpdateCheck}), returning.");
                return;
            }

            UpdateURLsJsonFromGithub();
            UpdateVersionJsonFromGithub();
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
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeVersion Fiddler setting not update needed.");
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving settings from Github {ex}");
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
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeURLs Fiddler setting updated.");
                        URLsJsonService.ExtensionURLs = jsonString;

                        // Update the next update check timestamp.
                        SettingsJsonService.Instance.SetNextUpdateTimestamp();
                    }
                    else
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeURLs Fiddler setting no update needed.");
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
