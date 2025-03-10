using Fiddler;
using System;
using System.Drawing;
using System.Windows.Forms;
using Office365FiddlerExtension.Services;
using System.Runtime.CompilerServices;

namespace Office365FiddlerExtension.UI.Forms
{
    public partial class Office365TabPage : UserControl
    {
        private static Office365TabPage _instance;

        public static Office365TabPage Instance => _instance ?? (_instance = new Office365TabPage());

        string strPlaceHolderText = LangHelper.GetString("Check IP Address Placeholder Text");


        public Office365TabPage()
        {
            InitializeComponent();
        }

        public bool GetExtensionEnabledCheckbox()
        {
            return ExtensionEnabledCheckBox.Checked;
        }

        public void UpdateUIControls()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
            
            ExtensionEnabledCheckBox.Checked = extensionSettings.ExtensionSessionProcessingEnabled;

            AnalyseAllSessionsButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            AnalyseSelectedSessionsButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            ClearAllSessionAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            ClearSelectedSessionAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;

            CreateConsolidatedAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
        }



        private void Office365TabPage_Load(object sender, EventArgs e)
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            ///////////////////
            ///
            /// Set labels and text according to preferred language set.
            ///

            if (extensionSettings.DebugMode)
            {
                ExtensionOptionsGroupBox.Text = $"{LangHelper.GetString("Extension Options")} (Debug Mode)";
                DebugModeCheckBox.Visible = true;
                NeverWebCallCheckBox.Visible = true;
                ExtensionSettingsTextbox.Text = Preferences.ExtensionSettings;
            }
            else
            {
                ExtensionOptionsGroupBox.Text = LangHelper.GetString("Extension Options");
                DebugModeCheckBox.Visible = false;
                NeverWebCallCheckBox.Visible = false;
            }

            ExtensionEnabledCheckBox.Text = LangHelper.GetString("Extension Enabled");

            ColumnsUIGroupbox.Text = LangHelper.GetString("Columns Enabled");

            ElapsedTimeCheckbox.Text = LangHelper.GetString("Elapsed Time");
            ElapsedTimeCheckbox.Checked = extensionSettings.ElapsedTimeColumnEnabled;

            SeverityCheckbox.Text = LangHelper.GetString("Severity");
            SeverityCheckbox.Checked = extensionSettings.SeverityColumnEnabled;

            SessionTypeCheckbox.Text = LangHelper.GetString("Session Type");
            SessionTypeCheckbox.Checked = extensionSettings.SessionTypeColumnEnabled;

            AuthenticationCheckbox.Text = LangHelper.GetString("Authentication");
            AuthenticationCheckbox.Checked = extensionSettings.AuthenticationColumnEnabled;

            ResponseServerCheckbox.Text = LangHelper.GetString("Response Server");
            ResponseServerCheckbox.Checked = extensionSettings.ResponseServerColumnEnabled;

            HostIPCheckbox.Text = LangHelper.GetString("Host IP");
            HostIPCheckbox.Checked = extensionSettings.HostIPColumnEnabled;

            if (extensionSettings.ExtensionSessionProcessingEnabled)
            {
                WarnBeforeProcessingGroupBox.Enabled = true;
                WhenToAnalyseSessionsGroupBox.Enabled = true;
            }
            else
            {
                WarnBeforeProcessingGroupBox.Enabled = false;
                WhenToAnalyseSessionsGroupBox.Enabled = false;
            }

            SessionAnalysisOnLoadSazCheckBox.Text = LangHelper.GetString("On Load Saz");
            SessionAnalysisOnLoadSazCheckBox.Checked = extensionSettings.SessionAnalysisOnLoadSaz;

            SessionAnalysisOnLiveTraceCheckBox.Text = LangHelper.GetString("On Live Trace");
            SessionAnalysisOnLiveTraceCheckBox.Checked = extensionSettings.SessionAnalysisOnLiveTrace;

            SessionAnalysisOnImportCheckBox.Text = LangHelper.GetString("On Import");
            SessionAnalysisOnImportCheckBox.Checked = extensionSettings.SessionAnalysisOnImport;

            WarnBeforeProcessingSessionsLabel.Text = LangHelper.GetString("S Capitalised Sessions");
            WarnBeforeProcessingGroupBox.Text = LangHelper.GetString("Warn Before Analysing");
            WarnBeforeAnalysingTextBox.Text = extensionSettings.WarnBeforeAnalysing.ToString();

            WhenToAnalyseSessionsGroupBox.Text = LangHelper.GetString("Choose When To Analyse Sessions");

            SessionAnalysisGroupBox.Text = LangHelper.GetString("Session Analysis");
            AnalyseAllSessionsButton.Text = LangHelper.GetString("Analyse All Sessions");
            AnalyseSelectedSessionsButton.Text = LangHelper.GetString("Analyse Selected Sessions");
            ClearAllSessionAnalysisButton.Text = LangHelper.GetString("Clear All Session Analysis");
            ClearSelectedSessionAnalysisButton.Text = LangHelper.GetString("Clear Selected Sessions Anaysis");

            CreateConsolidatedAnalysisButton.Text = LangHelper.GetString("Create Consolidated Analysis Report");

            ExtensionVersionInformationGroupBox.Text = LangHelper.GetString("Extension Version Information");


            ///////////////////
            /// Extension Options

            ExtensionEnabledCheckBox.Checked = extensionSettings.ExtensionSessionProcessingEnabled;

            AnalyseAllSessionsButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            AnalyseSelectedSessionsButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            ClearAllSessionAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            ClearSelectedSessionAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;

            NeverWebCallCheckBox.Checked = extensionSettings.NeverWebCall;

            DebugModeCheckBox.Checked = extensionSettings.DebugMode;

            if (extensionSettings.DebugMode)
            {
                DebugGroupBox.Visible = true;
            }
            else
            {
                DebugGroupBox.Visible = false;
            }

            CreateConsolidatedAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;

            UpdateLinkLabel.Text = URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer;

            if (extensionSettings.NeverWebCall)
            {
                ExtensionVersionLabel.Text = $"{LangHelper.GetString("Extension")}: v{UpdateService.Instance.GetExtensionDLLVersion()}";
                ExtensionVersionLabel.ForeColor = System.Drawing.Color.Black;

                RulesetVersionLabel.Text = $"{LangHelper.GetString("Ruleset")}: v{UpdateService.Instance.GetExtensionRulesetDLLVersion()}";
                RulesetVersionLabel.ForeColor = System.Drawing.Color.Black;

                return;
            }
            
            if (UpdateService.Instance.IsExtensionDLLUpdateAvailable().Equals("UpdateAvailable"))
            {
                ExtensionVersionLabel.Text = $"{LangHelper.GetString("Extension")}: v" +
                    $"{UpdateService.Instance.GetExtensionDLLVersion()} - " +
                    LangHelper.GetString("Update Available");
                ExtensionVersionLabel.ForeColor = System.Drawing.Color.Red;
            }
            else if (UpdateService.Instance.IsExtensionDLLUpdateAvailable().Equals("UpToDate"))
            {
                ExtensionVersionLabel.Text = $"{LangHelper.GetString("Extension")}: v" +
                    $"{UpdateService.Instance.GetExtensionDLLVersion()} - " +
                    LangHelper.GetString("Up To Date");
                ExtensionVersionLabel.ForeColor = System.Drawing.Color.Green;
            }
            else if(UpdateService.Instance.IsExtensionDLLUpdateAvailable().Equals("FutureVersion"))
            {
                ExtensionVersionLabel.Text = $"{LangHelper.GetString("Extension")}: v" +
                    $"{UpdateService.Instance.GetExtensionDLLVersion()} - " +
                    LangHelper.GetString("Future Version");
                ExtensionVersionLabel.ForeColor = System.Drawing.Color.Black;
            }

            if (UpdateService.Instance.IsRulesetDLLUpdateAvailable().Equals("UpdateAvailable"))
            {
                RulesetVersionLabel.Text = $"{LangHelper.GetString("Ruleset")}: v" +
                    $"{UpdateService.Instance.GetExtensionRulesetDLLVersion()} - " +
                    LangHelper.GetString("Update Available");
                RulesetVersionLabel.ForeColor = System.Drawing.Color.Red;
            }
            else if (UpdateService.Instance.IsRulesetDLLUpdateAvailable().Equals("FutureVersion"))
            {
                RulesetVersionLabel.Text = $"{LangHelper.GetString("Ruleset")}: v" +
                    $"{UpdateService.Instance.GetExtensionRulesetDLLVersion()} - " +
                    LangHelper.GetString("Future Version");
                RulesetVersionLabel.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                RulesetVersionLabel.Text = $"{LangHelper.GetString("Ruleset")}: v" +
                    $"{UpdateService.Instance.GetExtensionRulesetDLLVersion()} - " +
                    LangHelper.GetString("Up To Date");
                RulesetVersionLabel.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void ExtensionEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetExtensionSessionProcessingEnabled(ExtensionEnabledCheckBox.Checked);

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            // Enable / Disable these controls according to whether the extension is enabled or not.
            WarnBeforeProcessingGroupBox.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            WhenToAnalyseSessionsGroupBox.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;

            // REVIEW THIS 2.20.2025: Unable to update the tabpage controls outside of a direct interaction with the tabpage.
            this.UpdateUIControls();
            //MenuUI.Instance.UpdateUIControls();
            //ContextMenuUI.Instance.UpdateUIControls();
        }

        private void SessionAnalysisOnLoadSazCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_SessionAnalysisOnLoadSazCheckBox_CheckedChanged");
            SettingsJsonService.Instance.SetSessionAnalysisOnLoadSaz(SessionAnalysisOnLoadSazCheckBox.Checked);
        }

        private void SessionAnalysisOnLiveTraceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_SessionAnalysisOnLiveTraceCheckBox_CheckedChanged");
            SettingsJsonService.Instance.SetSessionAnalysisOnLiveTrace(SessionAnalysisOnLiveTraceCheckBox.Checked);
        }

        private void AnalyseAllSessionsButton_Click(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_AnalyseAllSessionsButton_Click");
            SessionFlagService.Instance.AnalyseAllSessions();
        }

        private void ClearAllSessionAnalysisButton_Click(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_ClearAllSessionAnalysisButton_Click");
            SessionFlagService.Instance.ClearAnalysisAllSessions();
        }

        private void AnalyseSelectedSessionsButton_Click(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_AnalyseSelectedSessionsButton_Click");
            SessionFlagService.Instance.AnalyseSelectedSessions();
        }

        private void ClearSelectedSessionAnalysisButton_Click(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_ClearSelectedSessionAnalysisButton_Click");
            SessionFlagService.Instance.ClearAnalysisSelectedSessions();
        }

        private void CreateConsolidatedAnalysisButton_Click(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_CreateConsolidatedAnalysisButton_Click");
            ConsolidatedAnalysisReportService.Instance.CreateCAR();
        }

        private void UpdateLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_UpdateLinkLabel_LinkClicked");
            System.Diagnostics.Process.Start(URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer);
        }

        private void NeverWebCallCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_NeverWebCallCheckBox_CheckedChanged");
            SettingsJsonService.Instance.SetNeverWebCall(NeverWebCallCheckBox.Checked);
        }

        private void DebugModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_DebugModeCheckBox_CheckedChanged");
            SettingsJsonService.Instance.SetDebugMode(DebugModeCheckBox.Checked);
        }

        private void SessionAnalysisOnImportCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_SessionAnalysisOnImportCheckBox_CheckedChanged");
            SettingsJsonService.Instance.SetSessionAnlysisOnImport(SessionAnalysisOnImportCheckBox.Checked);
        }

        private void WarnBeforeAnalysingTextBox_TextChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_WarnBeforeAnalysingTextBox_TextChanged");

            if (System.Text.RegularExpressions.Regex.IsMatch(WarnBeforeAnalysingTextBox.Text, "[^0-9]"))
            {
                string message = "This textbox only accepts numbers.";

                string caption = $"{LangHelper.GetString("Office 365 Fiddler Extension")}";

                MessageBox.Show(message, caption);
                WarnBeforeAnalysingTextBox.Text = WarnBeforeAnalysingTextBox.Text.Remove(WarnBeforeAnalysingTextBox.Text.Length - 1);
            }
            else
            {
                SettingsJsonService.Instance.SetWarnBeforeAnalysing(int.Parse(WarnBeforeAnalysingTextBox.Text));
            }
        }

        private void DebugModeUpdateButton_Click(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_DebugModeUpdateButton_Click");

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            ExtensionEnabledCheckBox.Checked = extensionSettings.ExtensionSessionProcessingEnabled;

            ExtensionSettingsTextbox.Text = Preferences.ExtensionSettings;
        }

        private void ElapsedTimeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_ElapsedTimeCheckbox_CheckedChanged");

            SettingsJsonService.Instance.SetElapsedColumnEnabled(ElapsedTimeCheckbox.Checked);
            if (ElapsedTimeCheckbox.Checked)
            {
                ColumnUI.Instance.AddElapsedTimeColumn();
            }
        }

        private void SeverityCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_SeverityCheckbox_CheckedChanged");

            SettingsJsonService.Instance.SetSeverityColumnEnabled(SeverityCheckbox.Checked);
            if (SeverityCheckbox.Checked)
            {
                ColumnUI.Instance.AddSeverityColumn();
            }
        }

        private void SessionTypeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_SessionTypeCheckbox_CheckedChanged");

            SettingsJsonService.Instance.SetSessionTypeColumnEnabled(SessionTypeCheckbox.Checked);
            if (SessionTypeCheckbox.Checked)
            {
                ColumnUI.Instance.AddSessionTypeColumn();
            }
        }

        private void ResponseServerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_ResponseServerCheckbox_CheckedChanged");

            SettingsJsonService.Instance.SetResponseServerColumnEnabled(ResponseServerCheckbox.Checked);
            if (ResponseServerCheckbox.Checked)
            {
                ColumnUI.Instance.AddResponseServerColumn();
            }
        }

        private void AuthenticationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_AuthenticationCheckbox_CheckedChanged");

            SettingsJsonService.Instance.SetAuthenticationColumnEnabled(AuthenticationCheckbox.Checked);
            if (AuthenticationCheckbox.Checked)
            {
                ColumnUI.Instance.AddAuthenticationColumn();
            }
        }

        private void HostIPCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            TelemetryService.CustomTrackEvent("UI_TabPage_HostIPCheckbox_CheckedChanged");

            SettingsJsonService.Instance.SetHostIPColumnEnabled(HostIPCheckbox.Checked);
            if (HostIPCheckbox.Checked)
            {
                ColumnUI.Instance.AddHostIPColumn();
            }
        }
    }

    public class Office365FiddlerExtensionTabPage : IFiddlerExtension
    {
        TabPage oPage;

        Office365TabPage oView = new Office365TabPage();

        private static Office365FiddlerExtensionTabPage _instance;

        public static Office365FiddlerExtensionTabPage Instance => _instance ?? (_instance = new Office365FiddlerExtensionTabPage());

        public void OnLoad()
        {
            // Load the UI.
            FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);
        }

        public void OnBeforeUnload()
        {
            oPage.Dispose();
        }

        public Office365FiddlerExtensionTabPage()
        {
            oPage = new TabPage($"{LangHelper.GetString("Office 365 Fiddler Extension")}");
            oPage.ImageIndex = (int)Fiddler.SessionIcons.HTML;

            oView.Dock = DockStyle.Fill;

            oPage.Controls.Add(oView);
        }

        public static void UIInvoke()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            //FiddlerObject.prompt($"{extensionSettings.ExtensionSessionProcessingEnabled}");

            TabPageUIInvoke(Office365TabPage.Instance.UpdateUIControls);

            var extensionSettings2 = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            //FiddlerObject.prompt($"{extensionSettings2.ExtensionSessionProcessingEnabled}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public static void TabPageUIInvoke(MethodInvoker target)
        {
            if (FiddlerApplication.isClosing)
            {
                return;
            }

            if (FiddlerApplication.UI.InvokeRequired)
            {
                //FiddlerObject.prompt($"Invoke REQUIRED; invoking.");
                FiddlerApplication.UI.Invoke(target);
                FiddlerApplication.UI.BeginInvoke(target);
            }
            else
            {
                target.Invoke();
                //FiddlerObject.prompt($"Invoke NOT REQUIRED; invoking.");
            }
        }
    }
}