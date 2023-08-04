using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Class to provide feedback on local vs. Github versions of the extension and ruleset.
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

        public int LocalExtensionDLLVerison(string versionPart)
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

        public Boolean IsExtensionDLLUpdateAvailable()
        {
            var githubJsonVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            if (Assembly.GetExecutingAssembly().GetName().Version.Major < githubJsonVersion.ExtensionMajor ||
                Assembly.GetExecutingAssembly().GetName().Version.Minor < githubJsonVersion.ExtensionMinor ||
                Assembly.GetExecutingAssembly().GetName().Version.Build < githubJsonVersion.ExtensionBuild)
            {
                return true;
            }

            return false;
        }

        public void NotifyUserIfExtensionUpdateIsAvailable()
        {
            if (!VersionService.Instance.IsExtensionDLLUpdateAvailable())
            { 
                return; 
            }
            
            string message = $"There's an update available for the Office 365 Fiddler Extension." +
                $"{Environment.NewLine}" +
                $"You are currently using extension v{VersionService.Instance.LocalExtensionDLLVerison("Major")}." +
                $"{VersionService.Instance.LocalExtensionDLLVerison("Minor")}." +
                $"{VersionService.Instance.LocalExtensionDLLVerison("Build")}" +
                $"{Environment.NewLine}" +
                $"A new version is available v" +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().ExtensionMajor}." +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().ExtensionMinor}." +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().ExtensionBuild}" +
                $"{Environment.NewLine}" +
                "Do you want to go to the update download page?";

            string caption = "Office 365 Fiddler Extension - Extension Update Available";

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
        /// Ruleset
        /// </summary>
        /// <returns></returns>
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

        public Boolean IsRulesetDLLUpdateAvailable()
        {
            var githubJsonVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();
            var extensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            var dirInfo = new DirectoryInfo(SettingsJsonService.AssemblyDirectory);
            string pattern = extensionVersion.RulesetDLLPattern;

            try
            {
                FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file.FullName);

                if (VersionService.Instance.LocalRulesetDLLVerison("Major") < githubJsonVersion.RulesetMajor ||
                VersionService.Instance.LocalRulesetDLLVerison("Minor") < githubJsonVersion.RulesetMinor ||
                VersionService.Instance.LocalRulesetDLLVerison("Build") < githubJsonVersion.RulesetBuild)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
            }

            return false;
        }

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

        public void NotifyUserIfRulesetUpdateIsAvailable()
        {
            if (!VersionService.Instance.IsRulesetDLLUpdateAvailable())
            {
                return;
            }

            string message = $"There's an update available for the Office 365 Fiddler Extension ruleset." +
                $"{Environment.NewLine}" +
                $"You are currently using ruleset v{VersionService.Instance.LocalRulesetDLLVerison("Major")}." +
                $"{VersionService.Instance.LocalRulesetDLLVerison("Minor")}." +
                $"{VersionService.Instance.LocalRulesetDLLVerison("Build")}" +
                $"{Environment.NewLine}" +
                $"A new version is available v" +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().RulesetMajor}." +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().RulesetMinor}." +
                $"{VersionJsonService.Instance.GetDeserializedExtensionVersion().RulesetBuild}" +
                $"{Environment.NewLine}" +
                "Do you want to go to the update download page?";

            string caption = "Office 365 Fiddler Extension - Extension Update Available";

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
