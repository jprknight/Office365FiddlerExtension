using Fiddler;
using Office365FiddlerExtension.Handler;
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

        public async void UpdateVersionJsonFromGithub()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeVersion Fiddler setting update check started.");

            var extensionURLs = URLsHandler.Instance.GetDeserializedExtensionURLs();
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();

            // If the current timestamp is less than the next update check timestamp, return.
            if (DateTime.Now < extensionSettings.NextUpdateCheck)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Next update check timestamp not met ({extensionSettings.NextUpdateCheck}), returning.");
                return;
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
                    if (VersionHandler.ExtensionVersion != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeVersion Fiddler setting updated.");
                        VersionHandler.ExtensionVersion = jsonString;

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

        public async void UpdateURLsJsonFromGithub()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeURLs Fiddler setting update check started.");

            var extensionURLs = URLsHandler.Instance.GetDeserializedExtensionURLs();
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();

            // If the current timestamp is less than the next update check timestamp, return.
            if (DateTime.Now < extensionSettings.NextUpdateCheck)
            {
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Next update check timestamp not met ({extensionSettings.NextUpdateCheck}), returning.");
                //return;
            }

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
                    if (URLsHandler.ExtensionURLs != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeURLs Fiddler setting updated.");
                        URLsHandler.ExtensionURLs = jsonString;

                        // Update the next update check timestamp.
                        SettingsHandler.Instance.SetNextUpdateTimestamp();
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving ExtensionURLs from Github {ex}");
                }
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): ExchangeURLs Fiddler setting update check finished.");
        }
    }
}
