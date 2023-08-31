using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;
using Fiddler;

namespace Office365FiddlerExtension.Services
{
    public static class LangHelper
    {
        private static ResourceManager _resourcemanager;

        static LangHelper()
        {
            _resourcemanager = new ResourceManager("Office365FiddlerExtension.Language.strings", Assembly.GetExecutingAssembly());

        }

        public static string GetString(string name)
        {
            return _resourcemanager.GetString(name);
        }

        public static void ChangeLanguage(string language)
        {
            var cultureInfo = new CultureInfo(language);

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            // Pull & Deserialize Json from ExtensionSettings.
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            extensionSettings.PreferredLanguage = language;
            // Serialize the object back into Json.
            // Write the Json into the ExtensionSettings Fiddler setting.
            Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (LanguageService): Language set to: " +
                $"{SettingsJsonService.Instance.GetDeserializedExtensionSettings().PreferredLanguage}.");

        }
    }
}
