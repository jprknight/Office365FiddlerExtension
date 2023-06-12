using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Office365FiddlerExtension.Services.SessionFlagHandler;

namespace Office365FiddlerExtension.Services
{
    public class UpdateHandler
    {
        public async void CheckForExtensionUpdate()
        {
            var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            var ExtensionURLs = URLsHandler.Instance.GetDeserializedExtensionURLs();
            var ExtensionVersion = VersionHandler.Instance.GetDeserializedExtensionVersion();

            if (ExtensionSettings.NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: CheckForUpdate: NeverWebCall is true; Extension won't check for any updates.");
                return;
            }

            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            //var SettingsJson = JsonConvert.DeserializeObject<ExtensionSettingsFlags>(Properties.Settings.Default.UpdateURL, JsonSettings); 
            
            //DateTime LastUpdated = SettingsJson.SettingsJsonLastUpdated;

            // If an update check has been performed within 24 hours, return.
            /*
            if (LastUpdated > DateTime.Now.AddHours(-24))
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Updates for settings.json checked within 24 hours, no update check performed.");
                return;
            }*/

            // Connect to the Github and see if the settings.json file needs updating.
            using (var getSettings = new HttpClient())
            {
                try
                {
                    var response = await getSettings.GetAsync(ExtensionURLs.ExtensionVersionJson);

                    response.EnsureSuccessStatusCode();

                    var jsonString = string.Empty;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        stream.Position = 0;
                        using (var reader = new StreamReader(stream))
                        {
                            jsonString = await reader.ReadToEndAsync();
                        }

                        // Call function to update settings
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Error retrieving settings from Github {ex}");
                }
            }
        }
    }
}
