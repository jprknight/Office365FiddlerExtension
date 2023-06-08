using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Office365FiddlerExtension.UI
{
    public partial class AboutNew : Form
    {
        public AboutNew()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            var extensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            var extensionVersion = SettingsHandler.Instance.GetDeserializedExtentionVersion();

            ExtensionEnabledCheckbox.Checked = extensionSettings.ExtensionEnabled;
            ExtensionDLLTextbox.Text = extensionSettings.ExtensionDLL;

            WarningSessionTimeThresholdTextbox.Text = extensionSettings.WarningSessionTimeThreshold.ToString();
            SlowRunningSessionThresholdTextbox.Text = extensionSettings.SlowRunningSessionThreshold.ToString();

            AboutNew.ActiveForm.Text = $"{Preferences.LogPrepend()} v{extensionVersion.VersionMajor}.{extensionVersion.VersionMinor}.{extensionVersion.VersionBuild}";

            if (extensionSettings.SessionAnalysisOnFiddlerLoad == true &&
                extensionSettings.SessionAnalysisOnLoadSaz == true &&
                extensionSettings.SessionAnalysisOnLiveTrace == true)
            {
                AllSessionAnalysisRadioButton.Checked = true;
                SomeSessionAnalysisRadioButton.Checked = false;

                SessionAnalysisOnFiddlerLoadCheckbox.Checked = true;
                SessionAnalysisOnFiddlerLoadCheckbox.Enabled = false;

                SessionAnalysisOnLoadSazCheckbox.Checked = true;
                SessionAnalysisOnLoadSazCheckbox.Enabled = false;

                SessionAnalysisOnLiveTraceCheckbox.Checked = true;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = false;
            }
            else
            {
                AllSessionAnalysisRadioButton.Checked = false;
                SomeSessionAnalysisRadioButton.Checked = true;

                if (SettingsHandler.Instance.SessionAnalysisOnFiddlerLoad)
                {
                    SessionAnalysisOnFiddlerLoadCheckbox.Checked = true;
                }
                else
                {
                    SessionAnalysisOnFiddlerLoadCheckbox.Checked = false;
                }

                if (SettingsHandler.Instance.SessionAnalysisOnLoadSaz)
                {
                    SessionAnalysisOnLoadSazCheckbox.Checked = true;
                }
                else
                {
                    SessionAnalysisOnLoadSazCheckbox.Checked = false;
                }

                if (SettingsHandler.Instance.SessionAnalysisOnLiveTrace)
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
                SessionAnalysisOnFiddlerLoadCheckbox.Enabled = false;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = false;
                SessionAnalysisOnLoadSazCheckbox.Enabled = false; 
            }

            if (SomeSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnFiddlerLoadCheckbox.Enabled = true;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = true;
                SessionAnalysisOnLoadSazCheckbox.Enabled = true;
            }

        }

        private void ExtensionEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsHandler.Instance.SetExtensionEnabled(ExtensionEnabledCheckbox.Checked);

            if (ExtensionEnabledCheckbox.Checked)
            {
                AllSessionAnalysisRadioButton.Enabled = true;
                SomeSessionAnalysisRadioButton.Enabled = true;
                SessionAnalysisOnFiddlerLoadCheckbox.Enabled = true;
                SessionAnalysisOnLoadSazCheckbox.Enabled = true;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = true;
            }
            else
            {
                AllSessionAnalysisRadioButton.Enabled = false;
                SomeSessionAnalysisRadioButton.Enabled = false;
                SessionAnalysisOnFiddlerLoadCheckbox.Enabled = false;
                SessionAnalysisOnLoadSazCheckbox.Enabled = false;
                SessionAnalysisOnLiveTraceCheckbox.Enabled = false;
            }
        }

        private void AllSessionAnalysisRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (AllSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnFiddlerLoadCheckbox.Enabled = false;
                SessionAnalysisOnFiddlerLoadCheckbox.Checked = true;

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
                SessionAnalysisOnFiddlerLoadCheckbox.Enabled = true;
                SessionAnalysisOnLoadSazCheckbox.Enabled = true;
                SessionAnalysisOnLiveTraceCheckbox.Enabled= true;
            }
        }

        private void SessionAnalysisOnFiddlerLoadCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsHandler.Instance.SetSessionAnalysisOnFiddlerLoad(SessionAnalysisOnFiddlerLoadCheckbox.Checked);
        }

        private void SessionAnalysisOnLoadSazCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsHandler.Instance.SetSessionAnalysisOnLoadSaz(SessionAnalysisOnLoadSazCheckbox.Checked);
        }

        private void SessionAnalysisOnLiveTraceCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsHandler.Instance.SetSessionAnalysisOnLiveTrace(SessionAnalysisOnLiveTraceCheckbox.Checked);
        }

        private void WarningSessionTimeThresholdUpdateButton_Click(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
