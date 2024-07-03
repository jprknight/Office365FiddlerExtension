using Fiddler;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Class to provide feedback on local vs. Github version checks of the extension and ruleset.
    /// </summary>
    public class VersionService
    {
        private static VersionService _instance;
        public static VersionService Instance => _instance ?? (_instance = new VersionService());

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
        public Boolean IsExtensionDLLUpdateAvailable()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}) " +
                    $"Never Web Call stopping extension update check.");
                return false;
            }

            if (!UpdateService.Instance.EligibleForUpdateCheck())
            {
                return false;
            }

            var githubJsonVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            if (VersionService.Instance.LocalExtensionDLLVerison("Major") >= githubJsonVersion.ExtensionMajor
                && VersionService.Instance.LocalExtensionDLLVerison("Minor") >= githubJsonVersion.ExtensionMinor
                && VersionService.Instance.LocalExtensionDLLVerison("Build") >= githubJsonVersion.ExtensionBuild)
            {
                // Update not available.
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    $"Extension DLL up to date or newer; " +
                    $"Local Version: v{VersionService.Instance.LocalExtensionDLLVerison("Major")}." +
                    $"{VersionService.Instance.LocalExtensionDLLVerison("Minor")}." +
                    $"{VersionService.Instance.LocalExtensionDLLVerison("Build")}. " +
                    $"Github Version: {githubJsonVersion.ExtensionMajor}." +
                    $"{githubJsonVersion.ExtensionMinor}." + 
                    $"{githubJsonVersion.ExtensionBuild}.");
                return false;
            }
            // One of the local major, minor, or build are less than the Github versions, return true.
            // There is an update available.
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): " +
                    $"Extension DLL update available; " +
                    $"Local Version: v{VersionService.Instance.LocalExtensionDLLVerison("Major")}." +
                    $"{VersionService.Instance.LocalExtensionDLLVerison("Minor")}." +
                    $"{VersionService.Instance.LocalExtensionDLLVerison("Build")}. " +
                    $"Github Version: {githubJsonVersion.ExtensionMajor}." +
                    $"{githubJsonVersion.ExtensionMinor}." +
                    $"{githubJsonVersion.ExtensionBuild}.");
                return true;
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
                    $"({this.GetType().Name})" +
                    "Never Web Call preventing update checking.");
                return;
            }

            if (!VersionService.Instance.IsExtensionDLLUpdateAvailable())
            {
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): No extension update available.");
                return; 
            }

            string message = $"{LangHelper.GetString("Update Available - There is an update")}" +
                $"{Environment.NewLine}" +
                $"{LangHelper.GetString("Update Available - Currently using")} v" +
                $"{VersionService.Instance.LocalExtensionDLLVerison("Major")}." +
                $"{VersionService.Instance.LocalExtensionDLLVerison("Minor")}." +
                $"{VersionService.Instance.LocalExtensionDLLVerison("Build")}" +
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
        public Boolean IsRulesetDLLUpdateAvailable()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}) " +
                    $"Never Web Call stopping ruleset update check.");
                return false;
            }

            if (!UpdateService.Instance.EligibleForUpdateCheck())
            {
                return false;
            }

            var githubJsonVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();
            var extensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            var dirInfo = new DirectoryInfo(SettingsJsonService.AssemblyDirectory);
            string pattern = extensionVersion.RulesetDLLPattern;

            try
            {
                FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file.FullName);

                // If the local major, minor, and build are all equal to or more than the Github versions, return false.
                // There is no update available.
                if (VersionService.Instance.LocalRulesetDLLVerison("Major") >= githubJsonVersion.RulesetMajor
                    && VersionService.Instance.LocalRulesetDLLVerison("Minor") >= githubJsonVersion.RulesetMinor
                    && VersionService.Instance.LocalRulesetDLLVerison("Build") >= githubJsonVersion.RulesetBuild)
                {
                    // Update not available.
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): " +
                        $"Ruleset DLL up to date or newer; " +
                        $"Local Version: v{VersionService.Instance.LocalRulesetDLLVerison("Major")}." +
                        $"{VersionService.Instance.LocalRulesetDLLVerison("Minor")}." +
                        $"{VersionService.Instance.LocalRulesetDLLVerison("Build")}. " +
                        $"Github Version: {githubJsonVersion.RulesetMajor}." +
                        $"{githubJsonVersion.RulesetMinor}." +
                        $"{githubJsonVersion.RulesetBuild}.");
                    return false;
                }
                // One of the local major, minor, or build are less than the Github versions, return true.
                // There is an update available.
                else
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " + 
                        $"({this.GetType().Name}): " +
                        $"Ruleset DLL update available; " +
                        $"Local Version: v{VersionService.Instance.LocalRulesetDLLVerison("Major")}." +
                        $"{VersionService.Instance.LocalRulesetDLLVerison("Minor")}." +
                        $"{VersionService.Instance.LocalRulesetDLLVerison("Build")}. " +
                        $"Github Version: {githubJsonVersion.RulesetMajor}." +
                        $"{githubJsonVersion.RulesetMinor}." +
                        $"{githubJsonVersion.RulesetBuild}.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            }

            return false;
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

                switch(versionPart) {
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
                    $"({this.GetType().Name})" +
                    "Never Web Call preventing update checking.");
                return;
            }

            if (!VersionService.Instance.IsRulesetDLLUpdateAvailable())
            {
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): IsRulesetDLLUpdateAvailable returned false.");
                return;
            }

            string message = $"{LangHelper.GetString("Update Available - Ruleset - Ruleset Update Available")}" +
                $"{Environment.NewLine}" +
                $"{LangHelper.GetString("Update Available - Ruleset - Currently Using")} v" +
                $"{VersionService.Instance.LocalRulesetDLLVerison("Major")}." +
                $"{VersionService.Instance.LocalRulesetDLLVerison("Minor")}." +
                $"{VersionService.Instance.LocalRulesetDLLVerison("Build")}" +
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
    }
}
