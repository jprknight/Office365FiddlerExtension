﻿using Fiddler;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Functions to ensure ExtensionURLs Json is created and populated with data.
    /// </summary>
    public class URLsJsonService
    {
        private static URLsJsonService _instance;
        public static URLsJsonService Instance => _instance ?? (_instance = new URLsJsonService());

        /// <summary>
        /// Get Json deserialised extension URLs.
        /// </summary>
        /// <returns></returns>
        public ExtensionURLsJson GetDeserializedExtensionURLs()
        {
            if (!JsonValidatorService.Instance.IsValidJsonString(Preferences.ExtensionURLs))
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Invalid Json in ExtensionURLs: {Preferences.ExtensionURLs}.");
                return null;
            }

            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            try
            {
                return JsonConvert.DeserializeObject<ExtensionURLsJson>(Preferences.ExtensionURLs, JsonSettings);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing extension URLs.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        /// <summary>
        /// If it doesn't already exist, create the extension URLs application preference.
        /// </summary>
        public void CreateExtensionURLFiddlerApplicationPreference()
        {
            // If the Extension URLs Json already exists, none of this needs to run.
            if (Preferences.ExtensionURLs != null || Preferences.ExtensionURLs == "")
            {
                return;
            }

            var URLs = new
            {
                TelemetryInstrumentationKey = "87fb55ab-0052-4970-9318-7c740220e3c0",
                ExtensionURL = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/master/Office365FiddlerExtension/ExtensionURLs.json",
                ExtensionVersion = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/master/Office365FiddlerExtension/ExtensionVersion.json",
                Installer = "https://github.com/jprknight/Office365FiddlerExtension/releases/latest",
                MicrosoftURLsIPs = "https://learn.microsoft.com/en-us/microsoft-365/enterprise/urls-and-ip-address-ranges?view=o365-worldwide",
                MicrosoftURLsIPsWebService = "https://endpoints.office.com/endpoints/worldwide?clientrequestid=b10c5ed1-bad1-445f-b386-b919946339a7",
                ReportIssues = "https://github.com/jprknight/Office365FiddlerExtension/issues",
                ResponseCodes = "https://en.wikipedia.org/wiki/List_of_HTTP_status_codes",
                SessionClassification = "https://raw.githubusercontent.com/jprknight/Office365FiddlerExtension/master/Office365FiddlerExtensionRuleset/SessionClassification.json",
                Wiki = "https://github.com/jprknight/Office365FiddlerExtension/wiki",
                WikiSessionTimeThresholds = "https://github.com/jprknight/Office365FiddlerExtension/wiki/Session-Time-Thresholds",
                WikiScoreForSession = "https://github.com/jprknight/Office365FiddlerExtension/wiki/What-is-ScoreForSession%3F"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(URLs);

            try
            {
                // Save the new Json to the Fiddler setting.
                Preferences.ExtensionURLs = jsonData;
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

        public string Installer { get; set; }

        public string MicrosoftURLsIPsWebService { get; set; }

        public string ResponseCodes { get; set; }

        public string Wiki { get; set; }

        public string WikiSessionTimeThresholds { get; set; }

        public string WikiScoreForSession { get; set; }

        public string ReportIssues { get; set; }

        public string SessionClassification { get; set; }
    }
}
