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
            /// Extension Information

            ExtensionPathTextbox.Text = extensionSettings.ExtensionPath;

            ExtensionDLLTextbox.Text = extensionSettings.ExtensionDLL;

            LocalDLLVersionTextbox.Text = VersionService.Instance.GetExtensionDLLVersion();

            if (VersionService.Instance.IsExtensionDLLUpdateAvailable())
            {
                LocalExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("UpdateAvailable");
                LocalExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                LocalExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("UpToDate");
                LocalExtensionVersionUpdateMessageLabel.ForeColor= System.Drawing.Color.Green;
            }

            if (VersionService.Instance.IsRulesetDLLUpdateAvailable())
            {
                LocalRulesetVersionUpdateMessageLabel.Text = LangHelper.GetString("UpdateAvailable");
                LocalRulesetVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                LocalRulesetVersionUpdateMessageLabel.Text = LangHelper.GetString("UpToDate");
                LocalRulesetVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
            }

            LocalRulesetVersionTextbox.Text = VersionService.Instance.GetExtensionRulesetDLLVersion();

            ///////////////////
            /// Github Information.

            GithubDLLVersionTextbox.Text = $"{extensionVersion.ExtensionMajor}.{extensionVersion.ExtensionMinor}.{extensionVersion.ExtensionBuild}";

            if (VersionService.Instance.IsExtensionDLLUpdateAvailable())
            {
                GithubExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("UpdateAvailable");
                GithubExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                GithubExtensionVersionUpdateMessageLabel.Text = LangHelper.GetString("UpToDate");
                GithubExtensionVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
            }

            GithubRulesetVersionTextbox.Text = $"{extensionVersion.RulesetMajor}.{extensionVersion.RulesetMinor}.{extensionVersion.RulesetBuild}";

            if (VersionService.Instance.IsRulesetDLLUpdateAvailable())
            {
                GithubRulesetVersionUpdateMessageLabel.Text = LangHelper.GetString("UpdateAvailable");
                GithubRulesetVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                GithubRulesetVersionUpdateMessageLabel.Text = LangHelper.GetString("UpToDate");
                GithubRulesetVersionUpdateMessageLabel.ForeColor = System.Drawing.Color.Green;
            }

            NextUpdateCheckTimestampTextbox.Text = extensionSettings.NextUpdateCheck.ToString();

            UpdateLinkLabel.Text = URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer;

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
                SessionAnalysisOnLoadSazCheckbox.Enabled = true;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = true;
            }
            else
            {
                AllSessionAnalysisRadioButton.Enabled = false;
                SomeSessionAnalysisRadioButton.Enabled = false;
                SessionAnalysisOnLoadSazCheckbox.Enabled = false;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = false;
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
