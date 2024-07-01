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

            this.Text = $"{LangHelper.GetString("About")}: {Assembly.GetExecutingAssembly().GetName().Name} v{extensionVersion.ExtensionMajor}.{extensionVersion.ExtensionMinor}.{extensionVersion.ExtensionBuild}";

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

            this.ExtensionOptionsGroupbox.Text = LangHelper.GetString("Extension Options");
            this.ExtensionEnabledCheckbox.Text = LangHelper.GetString("Extension Enabled");
            this.AllSessionAnalysisRadioButton.Text = LangHelper.GetString("All Session Analysis");
            this.SomeSessionAnalysisRadioButton.Text = LangHelper.GetString("Some Session analysis");
            this.SessionAnalysisOnLoadSazCheckbox.Text = LangHelper.GetString("On Load Saz");
            this.SessionAnalysisOnLiveTraceCheckbox.Text = LangHelper.GetString("On Live Trace");
            this.NextUpdateCheckLabel.Text = LangHelper.GetString("Check for updates every");
            this.HoursLabel.Text = LangHelper.GetString("Check for updates every hours");
            this.LanguageLabel.Text = LangHelper.GetString("Language");
            this.LanguageTextBox.Text = SettingsJsonService.Instance.GetDeserializedExtensionSettings().PreferredLanguage;

            this.ObscureSettingsGroupbox.Text = LangHelper.GetString("Obscure Settings");
            this.ScoreForSessionLabel.Text = LangHelper.GetString("ScoreForSession");
            this.WhatIsScoreForSessionLinkLabel.Text = LangHelper.GetString("What is ScoreForSession");
            this.WarningSessionTimeThresholdLabel.Text = LangHelper.GetString("Warning Session Time Threshold");
            this.SlowRunningSessionThresholdLabel.Text = LangHelper.GetString("Slow Running Session Threshold");
            this.SessionTimeThresholdLinkLabel.Text = LangHelper.GetString("What are these two thresholds");

            this.SaveButton.Text = LangHelper.GetString("Save");
            this.CloseButton.Text = LangHelper.GetString("Close");

            ///////////////////
            /// Extension Information

            ExtensionPathTextbox.Text = extensionSettings.ExtensionPath;

            ExtensionDLLTextbox.Text = extensionSettings.ExtensionDLL;

            LocalDLLVersionTextbox.Text = VersionService.Instance.GetExtensionDLLVersion();

            if (VersionService.Instance.IsExtensionDLLUpdateAvailable())
            {
                LocalExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Update Available");
                LocalExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                LocalExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Up To Date");
                LocalExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
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

            LocalRulesetVersionTextbox.Text = VersionService.Instance.GetExtensionRulesetDLLVersion();

            ///////////////////
            /// Github Information.

            GithubDLLVersionTextbox.Text = $"{extensionVersion.ExtensionMajor}.{extensionVersion.ExtensionMinor}.{extensionVersion.ExtensionBuild}";

            if (VersionService.Instance.IsExtensionDLLUpdateAvailable())
            {
                GithubExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Update Available");
                GithubExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                GithubExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("Up To Date");
                GithubExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
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

            ///////////////////
            /// Extension Options

            ExtensionEnabledCheckbox.Checked = extensionSettings.ExtensionSessionProcessingEnabled;

            NextUpdateCheckTextbox.Text = extensionSettings.UpdateCheckFrequencyHours.ToString();

            if (extensionSettings.SessionAnalysisOnLoadSaz == true &&
                extensionSettings.SessionAnalysisOnLiveTrace == true)
            {
                AllSessionAnalysisRadioButton.Checked = true;
                SomeSessionAnalysisRadioButton.Checked = false;

                SessionAnalysisOnLoadSazCheckbox.Checked = true;
                SessionAnalysisOnLoadSazCheckbox.Enabled = false;

                SessionAnalysisOnLiveTraceCheckbox.Checked = true;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = false;
            }
            else
            {
                AllSessionAnalysisRadioButton.Checked = false;
                SomeSessionAnalysisRadioButton.Checked = true;

                if (SettingsJsonService.Instance.SessionAnalysisOnLoadSaz)
                {
                    SessionAnalysisOnLoadSazCheckbox.Checked = true;
                }
                else
                {
                    SessionAnalysisOnLoadSazCheckbox.Checked = false;
                }

                if (SettingsJsonService.Instance.SessionAnalysisOnLiveTrace)
                {
                    SessionAnalysisOnLiveTraceCheckbox.Checked = true;
                }
                else
                {
                    SessionAnalysisOnLiveTraceCheckbox.Checked = false;
                }
            }

            if (AllSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnLiveTraceCheckbox.Enabled = false;
                SessionAnalysisOnLoadSazCheckbox.Enabled = false;
            }

            if (SomeSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnLiveTraceCheckbox.Enabled = true;
                SessionAnalysisOnLoadSazCheckbox.Enabled = true;
            }

            ///////////////////
            /// Obscure Settings

            ScoreForSessionTextbox.Text = extensionSettings.InspectorScoreForSession.ToString();

            WarningSessionTimeThresholdTextbox.Text = extensionSettings.WarningSessionTimeThreshold.ToString();
            SlowRunningSessionThresholdTextbox.Text = extensionSettings.SlowRunningSessionThreshold.ToString();

        }

        private void ExtensionEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetExtensionSessionProcessingEnabled(ExtensionEnabledCheckbox.Checked);

            if (ExtensionEnabledCheckbox.Checked)
            {
                AllSessionAnalysisRadioButton.Enabled = true;
                SomeSessionAnalysisRadioButton.Enabled = true;
                SessionAnalysisOnLoadSazCheckbox.Enabled = SomeSessionAnalysisRadioButton.Checked;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = SomeSessionAnalysisRadioButton.Checked;
            }
            else
            {
                AllSessionAnalysisRadioButton.Enabled = false;
                SomeSessionAnalysisRadioButton.Enabled = false;
                SessionAnalysisOnLoadSazCheckbox.Enabled = SomeSessionAnalysisRadioButton.Checked;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = SomeSessionAnalysisRadioButton.Checked;
            }
        }

        private void AllSessionAnalysisRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (AllSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnLoadSazCheckbox.Enabled = false;
                SessionAnalysisOnLoadSazCheckbox.Checked = true;

                SessionAnalysisOnLiveTraceCheckbox.Enabled = false;
                SessionAnalysisOnLiveTraceCheckbox.Checked = true;
            }
        }

        private void SomeSessionAnalysisRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SomeSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnLoadSazCheckbox.Enabled = true;
                SessionAnalysisOnLiveTraceCheckbox.Enabled= true;
            }
        }

        private void SessionAnalysisOnLoadSazCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetSessionAnalysisOnLoadSaz(SessionAnalysisOnLoadSazCheckbox.Checked);
        }

        private void SessionAnalysisOnLiveTraceCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetSessionAnalysisOnLiveTrace(SessionAnalysisOnLiveTraceCheckbox.Checked);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
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

        private void WhatIsScoreForSessionLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(URLsJsonService.Instance.GetDeserializedExtensionURLs().WikiScoreForSession);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): About unable to open ScoreForSession link: {URLsJsonService.Instance.GetDeserializedExtensionURLs().WikiScoreForSession}");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetSlowRunningSessionThreshold(SlowRunningSessionThresholdTextbox.Text);

            SettingsJsonService.Instance.SetWarningSessionTimeThreshold(WarningSessionTimeThresholdTextbox.Text);

            SettingsJsonService.Instance.SetUpdateCheckFrequencyHours(NextUpdateCheckTextbox.Text);
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
