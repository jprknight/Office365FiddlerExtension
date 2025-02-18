using Office365FiddlerExtension.Services;
using System;
using System.Reflection;
using System.Windows.Forms;
using Fiddler;

namespace Office365FiddlerExtension.UI
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            var extensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            this.Text = $"{LangHelper.GetString("About")}: {Assembly.GetExecutingAssembly().GetName().Name} v" +
                $"{VersionService.Instance.GetExtensionDLLVersion()}";

            ///////////////////
            ///
            /// Set labels and text according to preferred language set.
            ///

            this.InfoGroupbox.Text = LangHelper.GetString("Locally Installed Information");
            this.ExtensionPathLabel.Text = LangHelper.GetString("Extension Path");
            this.ExtensionDLLLabel.Text = LangHelper.GetString("Extension DLL");
            this.LocalExtensionVersionLabel.Text = LangHelper.GetString("Local Extension Version");
            this.LocalRulesetVersionLabel.Text = LangHelper.GetString("Local Ruleset Version");

            this.GithubInfoGroupbox.Text = LangHelper.GetString("Github Information");
            this.GithubExtensionVersionLabel.Text = LangHelper.GetString("Github Extension Version");
            this.GithubRulesetVersionLabel.Text = LangHelper.GetString("Github Ruleset Version");
            this.NextUpdateCheckTimestampLabel.Text = LangHelper.GetString("Next Update Check");
            this.InstructionsLabel.Text = LangHelper.GetString("Click the link below for update instructions");
            this.UpdateLinkLabel.Text = URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer;

            ExtensionPathTextbox.Text = extensionSettings.ExtensionPath;

            ExtensionDLLTextbox.Text = extensionSettings.ExtensionDLL;

            LocalDLLVersionTextbox.Text = VersionService.Instance.GetExtensionDLLVersion();

            LocalRulesetVersionTextbox.Text = VersionService.Instance.GetExtensionRulesetDLLVersion();

            if (extensionSettings.DebugMode)
            {
                DebugModeLabel.Text = $"DebugMode: {extensionSettings.DebugMode}";
            }
            else
            {
                DebugModeLabel.Text = "";
            }

            ///////////////////
            /// Extension Update Information

            if (extensionSettings.NeverWebCall)
            {
                LocalExtensionVersionUpdateMessageLabel.Text = "";
                LocalRulesetVersionUpdateMessageLabel.Text = "";
                GithubExtensionVersionUpdateMessageLabel.Text = "";
                GithubRulesetVersionUpdateMessageLabel.Text = "";

                GithubExtensionVersionLabel.Enabled = false;
                GithubDLLVersionTextbox.Enabled = false;

                GithubRulesetVersionLabel.Enabled = false;
                GithubRulesetVersionTextbox.Enabled = false;

                NextUpdateCheckTimestampLabel.Enabled = false;
                NextUpdateCheckTimestampTextbox.Enabled = false;

                GithubInfoGroupbox.Text += $" ({LangHelper.GetString("NeverWebCall_FeatureDisabled")})";

                return;
            }

            ///////////////////
            /// Extension Update Information.

            if (VersionService.Instance.IsExtensionDLLUpdateAvailable().Equals("UpdateAvailable"))
            {
                LocalExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Update Available");
                LocalExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else if (VersionService.Instance.IsExtensionDLLUpdateAvailable().Equals("UpToDate"))
            {
                LocalExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Up To Date");
                LocalExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
            }
            else if (VersionService.Instance.IsExtensionDLLUpdateAvailable().Equals("FutureVersion"))
            {
                LocalExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Future Version");
                LocalExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Blue;
            }

            if (VersionService.Instance.IsRulesetDLLUpdateAvailable())
            {
                LocalRulesetVersionUpdateMessageLabel.Text = LangHelper.GetString("Update Available");
                LocalRulesetVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                LocalRulesetVersionUpdateMessageLabel.Text = LangHelper.GetString("Up To Date");
                LocalRulesetVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
            }

            ///////////////////
            /// Ruleset Update Information.

            GithubDLLVersionTextbox.Text = $"{extensionVersion.ExtensionMajor}.{extensionVersion.ExtensionMinor}.{extensionVersion.ExtensionBuild}";

            if (VersionService.Instance.IsExtensionDLLUpdateAvailable().Equals("UpdateAvailable"))
            {
                GithubExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Update Available");
                GithubExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else if (VersionService.Instance.IsExtensionDLLUpdateAvailable().Equals("UpToDate"))
            {
                GithubExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Up To Date");
                GithubExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
            }
            else if (VersionService.Instance.IsExtensionDLLUpdateAvailable().Equals("FutureVersion"))
            {
                GithubExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Future Version");
                GithubExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Blue;
            }

            GithubRulesetVersionTextbox.Text = $"{extensionVersion.RulesetMajor}.{extensionVersion.RulesetMinor}.{extensionVersion.RulesetBuild}";

            if (VersionService.Instance.IsRulesetDLLUpdateAvailable())
            {
                GithubRulesetVersionUpdateMessageLabel.Text = LangHelper.GetString("Update Available");
                GithubRulesetVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                GithubRulesetVersionUpdateMessageLabel.Text = LangHelper.GetString("Up To Date");
                GithubRulesetVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
            }

            NextUpdateCheckTimestampTextbox.Text = extensionSettings.NextUpdateCheck.ToString();
        }

        private void SessionTimeThresholdLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(URLsJsonService.Instance.GetDeserializedExtensionURLs().WikiSessionTimeThresholds);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): About unable to open SesssionTimeThreshold link: {URLsJsonService.Instance.GetDeserializedExtensionURLs().WikiSessionTimeThresholds}");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }

        private void UpdateLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): About unable to open Installer link: {URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer}");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }
    }
}