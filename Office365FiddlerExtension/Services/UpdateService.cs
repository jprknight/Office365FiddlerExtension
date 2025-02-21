using Fiddler;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;

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

        private bool ExtensionUpdateMessageLogged;

        private bool RulesetUpdateMessageLogged;

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

        /// <summary>
        /// Extension DLL / Assembly.
        /// </summary>
        /// <returns></returns>
        public string GetExtensionDLLVersion()
        {
            return $"{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}." +
                $"{Assembly.GetExecutingAssembly().GetName().Version.Build}";
        }

        /// <summary>
        /// Get executing assembly version info.
        /// </summary>
        /// <param name="versionPart"></param>
        /// <returns>int Major, Minor, Build versions.</returns>
        private int LocalExtensionDLLVerison(string versionPart)
        {
            switch (versionPart)
            {
                case "Major":
                    return Assembly.GetExecutingAssembly().GetName().Version.Major;
                case "Minor":
                    return Assembly.GetExecutingAssembly().GetName().Version.Minor;
                case "Build":
                    return Assembly.GetExecutingAssembly().GetName().Version.Build;
            }

            return 0;
        }

        /// <summary>
        /// Determines if an extension update is available.
        /// </summary>
        /// <returns>bool</returns>
        public string IsExtensionDLLUpdateAvailable()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                if (ExtensionUpdateMessageLogged)
                {
                    return "";
                }

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    $"Never Web Call stopping extension update check.");

                return "";
            }

            var extensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            string extensionMajor = extensionVersion.ExtensionMajor.ToString();

            var githubJsonVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            int LocalExtensionVersion = ConcatenateThreeIntegers(LocalExtensionDLLVerison("Major"),
                LocalExtensionDLLVerison("Minor"),
                LocalExtensionDLLVerison("Build"));

            int GithubExtensionVersion = ConcatenateThreeIntegers(githubJsonVersion.ExtensionMajor,
                githubJsonVersion.ExtensionMinor,
                githubJsonVersion.ExtensionBuild);

            // Extension running is up to date.
            if (LocalExtensionVersion == GithubExtensionVersion)
            {
                if (ExtensionUpdateMessageLogged)
                {
                    return "UpToDate";
                }

                // Update not available.
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    $"Extension DLL up to date; " +
                    $"Local Version: v{LocalExtensionDLLVerison("Major")}." +
                    $"{LocalExtensionDLLVerison("Minor")}." +
                    $"{LocalExtensionDLLVerison("Build")}. " +
                    $"Github Version: {githubJsonVersion.ExtensionMajor}." +
                    $"{githubJsonVersion.ExtensionMinor}." +
                    $"{githubJsonVersion.ExtensionBuild}.");

                ExtensionUpdateMessageLogged = true;

                return "UpToDate";
            }
            // Extension running is newer than Github version; future version.
            else if (LocalExtensionVersion >= GithubExtensionVersion)
            {
                if (ExtensionUpdateMessageLogged)
                {
                    return "FutureVersion";
                }

                // Update not available.
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    $"Extension DLL running future version; " +
                    $"Local Version: v{LocalExtensionDLLVerison("Major")}." +
                    $"{LocalExtensionDLLVerison("Minor")}." +
                    $"{LocalExtensionDLLVerison("Build")}. " +
                    $"Github Version: {githubJsonVersion.ExtensionMajor}." +
                    $"{githubJsonVersion.ExtensionMinor}." +
                    $"{githubJsonVersion.ExtensionBuild}.");

                ExtensionUpdateMessageLogged = true;

                return "FutureVersion";
            }
            // One of the local major, minor, or build are less than the Github versions, return true.
            // There is an update available.
            else
            {
                if (ExtensionUpdateMessageLogged)
                {
                    return "UpdateAvailable";
                }

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    $"Extension DLL update available; " +
                    $"Local Version: v{LocalExtensionDLLVerison("Major")}." +
                    $"{LocalExtensionDLLVerison("Minor")}." +
                    $"{LocalExtensionDLLVerison("Build")}. " +
                    $"Github Version: {githubJsonVersion.ExtensionMajor}." +
                    $"{githubJsonVersion.ExtensionMinor}." +
                    $"{githubJsonVersion.ExtensionBuild}.");

                ExtensionUpdateMessageLogged = true;

                return "UpdateAvailable";
            }
        }

        /// <summary>
        /// Throw a message box if there is an extension update available.
        /// </summary>
        public void NotifyUserIfExtensionUpdateIsAvailable()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    "Never Web Call preventing update checking.");
                return;
            }

            if (!IsExtensionDLLUpdateAvailable().Equals("UpdateAvailable"))
            {
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): No extension update available.");
                return;
            }

            string message = $"{LangHelper.GetString("Update Available - There is an update")}" +
                $"{Environment.NewLine}" +
                $"{LangHelper.GetString("Update Available - Currently using")} v" +
                $"{LocalExtensionDLLVerison("Major")}." +
                $"{LocalExtensionDLLVerison("Minor")}." +
                $"{LocalExtensionDLLVerison("Build")}" +
                $"{Environment.NewLine}" +
                $"{LangHelper.GetString("Update Available - A new version")} v" +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().ExtensionMajor}." +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().ExtensionMinor}." +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().ExtensionBuild}" +
                $"{Environment.NewLine}" +
                $"{LangHelper.GetString("Update Available - Go to download page?")}";

            string caption = $"{LangHelper.GetString("Office 365 Fiddler Extension")} - {LangHelper.GetString("Update Available - Extension Update Available")}";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            //Display the MessageBox.
            result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer);
            }
        }

        /// <summary>
        /// Get the extension ruleset version from the DLL.
        /// </summary>
        /// <returns>string Major.Minor.Build</returns>
        public string GetExtensionRulesetDLLVersion()
        {
            var ExtensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            var dirInfo = new DirectoryInfo(SettingsJsonService.AssemblyDirectory);
            string pattern = ExtensionVersion.RulesetDLLPattern;

            try
            {
                FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file.FullName);

                return $"{fileVersionInfo.FileMajorPart}.{fileVersionInfo.FileMinorPart}.{fileVersionInfo.FileBuildPart}";
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            }

            return null;
        }

        /// <summary>
        /// Determines if a ruleset DLL update is available.
        /// </summary>
        /// <returns></returns>
        public string IsRulesetDLLUpdateAvailable()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                if (RulesetUpdateMessageLogged)
                {
                    return "";
                }

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    $"Never Web Call stopping ruleset update check.");
                return "";
            }

            var extensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            string rulesetMajor = extensionVersion.RulesetMajor.ToString();

            if (rulesetMajor == "1776")
            {
                if (RulesetUpdateMessageLogged)
                {
                    return "";
                }

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"RulesetMajor is 1776, this is the first run of the extension after installation.");
                return "";
            }

            var githubJsonVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            var dirInfo = new DirectoryInfo(SettingsJsonService.AssemblyDirectory);
            string pattern = extensionVersion.RulesetDLLPattern;

            int LocalRulesetVersion = ConcatenateThreeIntegers(LocalRulesetDLLVerison("Major"),
                LocalRulesetDLLVerison("Minor"),
                LocalRulesetDLLVerison("Build"));

            int GithubRulesetVersion = ConcatenateThreeIntegers(githubJsonVersion.RulesetMajor,
                githubJsonVersion.RulesetMinor,
                githubJsonVersion.RulesetBuild);

            try
            {
                FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file.FullName);


                // If the local major, minor, and build are all equal to or more than the Github versions, return false.
                // There is no update available.
                if (LocalRulesetVersion == GithubRulesetVersion)
                {
                    if (RulesetUpdateMessageLogged)
                    {
                        return "UpToDate";
                    }

                    // Update not available.
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): " +
                        $"Ruleset DLL up to date; " +
                        $"Local Version: v{LocalRulesetDLLVerison("Major")}." +
                        $"{LocalRulesetDLLVerison("Minor")}." +
                        $"{LocalRulesetDLLVerison("Build")}. " +
                        $"Github Version: {githubJsonVersion.RulesetMajor}." +
                        $"{githubJsonVersion.RulesetMinor}." +
                        $"{githubJsonVersion.RulesetBuild}.");

                    RulesetUpdateMessageLogged = true;

                    return "UpToDate";
                }
                // If the local major, minor, and build are all equal to or more than the Github versions, return false.
                // There is no update available.
                else if (LocalRulesetVersion >= GithubRulesetVersion)
                {
                    if (RulesetUpdateMessageLogged)
                    {
                        return "FutureVersion";
                    }

                    // Update not available.
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): " +
                        $"Ruleset DLL future version; " +
                        $"Local Version: v{LocalRulesetDLLVerison("Major")}." +
                        $"{LocalRulesetDLLVerison("Minor")}." +
                        $"{LocalRulesetDLLVerison("Build")}. " +
                        $"Github Version: {githubJsonVersion.RulesetMajor}." +
                        $"{githubJsonVersion.RulesetMinor}." +
                        $"{githubJsonVersion.RulesetBuild}.");

                    RulesetUpdateMessageLogged = true;

                    return "FutureVersion";
                }
                // One of the local major, minor, or build are less than the Github versions, return true.
                // There is an update available.
                else
                {
                    if (RulesetUpdateMessageLogged)
                    {
                        return "UpdateAvailable";
                    }

                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): " +
                        $"Ruleset DLL update available; " +
                        $"Local Version: v{LocalRulesetDLLVerison("Major")}." +
                        $"{LocalRulesetDLLVerison("Minor")}." +
                        $"{LocalRulesetDLLVerison("Build")}. " +
                        $"Github Version: {githubJsonVersion.RulesetMajor}." +
                        $"{githubJsonVersion.RulesetMinor}." +
                        $"{githubJsonVersion.RulesetBuild}.");

                    RulesetUpdateMessageLogged = true;

                    return "UpdateAvailable";
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            }

            return "";
        }

        /// <summary>
        /// Gets the local ruleset DLL version info.
        /// </summary>
        /// <param name="versionPart"></param>
        /// <returns>int Major, Minor, Build versions.</returns>
        public int LocalRulesetDLLVerison(string versionPart)
        {
            var ExtensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            var dirInfo = new DirectoryInfo(SettingsJsonService.AssemblyDirectory);
            string pattern = ExtensionVersion.RulesetDLLPattern;

            try
            {
                FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file.FullName);

                switch (versionPart)
                {
                    case "Major":
                        return fileVersionInfo.FileMajorPart;
                    case "Minor":
                        return fileVersionInfo.FileMinorPart;
                    case "Build":
                        return fileVersionInfo.FileBuildPart;
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            }

            return 0;
        }

        /// <summary>
        /// Throw a message box if there is a ruleset update available.
        /// </summary>
        public void NotifyUserIfRulesetUpdateIsAvailable()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    "Never Web Call preventing update checking.");
                return;
            }

            if (!IsRulesetDLLUpdateAvailable().Equals("UpdateAvailable"))
            {
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): IsRulesetDLLUpdateAvailable returned false.");
                return;
            }

            string message = $"{LangHelper.GetString("Update Available - Ruleset - Ruleset Update Available")}" +
                $"{Environment.NewLine}" +
                $"{LangHelper.GetString("Update Available - Ruleset - Currently Using")} v" +
                $"{LocalRulesetDLLVerison("Major")}." +
                $"{LocalRulesetDLLVerison("Minor")}." +
                $"{LocalRulesetDLLVerison("Build")}" +
                $"{Environment.NewLine}" +
                $"{LangHelper.GetString("Update Available - A new version")} v" +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().RulesetMajor}." +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().RulesetMinor}." +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().RulesetBuild}" +
                $"{Environment.NewLine}" +
                $"{LangHelper.GetString("Update Available - Go to download page?")}";

            string caption = $"{LangHelper.GetString("Office 365 Fiddler Extension")} - {LangHelper.GetString("Update Available - Ruleset - Ruleset Update")}";

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            //Display the MessageBox.
            result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer);
            }
        }

        public int ConcatenateThreeIntegers(int a, int b, int c)
        {
            return int.Parse(a.ToString() + b.ToString() + c.ToString());
        }
    }
}
