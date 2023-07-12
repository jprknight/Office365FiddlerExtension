﻿using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

            ExtensionEnabledCheckbox.Checked = extensionSettings.ExtensionSessionProcessingEnabled;
            ExtensionPathTextbox.Text = extensionSettings.ExtensionPath;
            
            ExtensionDLLTextbox.Text = extensionSettings.ExtensionDLL;
            LocalDLLVersionTextbox.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            GithubDLLVersionTextbox.Text = extensionVersion.ExtensionMajor + "." + extensionVersion.ExtensionMinor + "." + extensionVersion.ExtensionBuild;

            NextUpdateCheckTextbox.Text = extensionSettings.UpdateCheckFrequencyHours.ToString();

            ScoreForSessionTextbox.Text = extensionSettings.InspectorScoreForSession.ToString();

            NextUpdateCheckTimestampTextbox.Text = extensionSettings.NextUpdateCheck.ToString();

            if (extensionSettings.UseBetaRuleSet)
            {
                LocalRulesetVersionTextbox.Text = extensionSettings.LocalBetaRulesetLastUpdated.ToString();
                //GithubRulesetVersionTextbox.Text = extensionVersion.BetaRulesetVersion.ToString();
            }
            else
            {
                LocalRulesetVersionTextbox.Text = extensionSettings.LocalMasterRulesetLastUpdated.ToString();
                //GithubRulesetVersionTextbox.Text = extensionVersion.MasterRulesetVersion.ToString();
            }
            

            WarningSessionTimeThresholdTextbox.Text = extensionSettings.WarningSessionTimeThreshold.ToString();
            SlowRunningSessionThresholdTextbox.Text = extensionSettings.SlowRunningSessionThreshold.ToString();

            this.Text = $"About: {Assembly.GetExecutingAssembly().GetName().Name} v{extensionVersion.ExtensionMajor}.{extensionVersion.ExtensionMinor}.{extensionVersion.ExtensionBuild}";

            if (extensionSettings.SessionAnalysisOnFiddlerLoad == true &&
                extensionSettings.SessionAnalysisOnLoadSaz == true &&
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
            SettingsJsonService.Instance.UpdateSlowRunningSessionThreshold(SlowRunningSessionThresholdTextbox.Text);

            SettingsJsonService.Instance.UpdateWarningSessionTimeThreshold(WarningSessionTimeThresholdTextbox.Text);

            SettingsJsonService.Instance.SetUpdateCheckFrequencyHours(NextUpdateCheckTextbox.Text);
        }
    }
}