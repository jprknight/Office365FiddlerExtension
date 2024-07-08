using Fiddler;
using System;
using System.Drawing;
using System.Windows.Forms;
using Office365FiddlerExtension.Services;
using System.Reflection;

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

        public void UpdateUIControls()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            ExtensionEnabledCheckBox.Checked = extensionSettings.ExtensionSessionProcessingEnabled;

            AnalyseAllSessionsButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            AnalyseSelectedSessionsButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            ClearAllSessionAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            ClearSelectedSessionAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;

            CreateConsolidatedAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;

            //Office365FiddlerExtensionTabPage.Instance.UpdateTabPageUI();
        }

        private void Office365TabPage_Load(object sender, EventArgs e)
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            EnterIPAddressTextBox.GotFocus += RemovePlaceholderText;
            EnterIPAddressTextBox.LostFocus += AddPlaceholderText;

            // Make sure the text box has placeholder text on load since it'll be empty.
            SetPlaceHolderText();

            LanguageTextBox.Text = extensionSettings.PreferredLanguage;

            ///////////////////
            ///
            /// Set labels and text according to preferred language set.
            ///

            if (extensionSettings.DebugMode)
            {
                ExtensionOptionsGroupBox.Text = $"{LangHelper.GetString("Extension Options")} ({LangHelper.GetString("Debug Mode")})";
            }
            else
            {
                ExtensionOptionsGroupBox.Text = LangHelper.GetString("Extension Options");
            }
            
            ExtensionEnabledCheckBox.Text = LangHelper.GetString("Extension Enabled");
            AllSessionAnalysisRadioButton.Text = LangHelper.GetString("All Session Analysis");
            SomeSessionAnalysisRadioButton.Text = LangHelper.GetString("Some Session analysis");
            SessionAnalysisOnLoadSazCheckBox.Text = LangHelper.GetString("On Load Saz");
            SessionAnalysisOnLiveTraceCheckBox.Text = LangHelper.GetString("On Live Trace");

            CaptureTrafficCheckBox.Text = LangHelper.GetString("Capture Traffic");

            SessionAnalysisGroupBox.Text = LangHelper.GetString("Session Analysis");
            AnalyseAllSessionsButton.Text = LangHelper.GetString("Analyse All Sessions");
            AnalyseSelectedSessionsButton.Text = LangHelper.GetString("Analyse Selected Sessions");
            ClearAllSessionAnalysisButton.Text = LangHelper.GetString("Clear All Session Analysis");
            ClearSelectedSessionAnalysisButton.Text = LangHelper.GetString("Clear Selected Sessions Anaysis");

            CreateConsolidatedAnalysisButton.Text = LangHelper.GetString("Create Consolidated Analysis Report");

            CheckIPAddressGroupBox.Text = LangHelper.GetString("Check IP Address");
            CheckIPAddressButton.Text = LangHelper.GetString("Check");
            CheckIPAddressClearButton.Text = LangHelper.GetString("Clear");

            ExtensionVersionInformationGroupBox.Text = LangHelper.GetString("Extension Version Information");


            ///////////////////
            /// Extension Options

            ExtensionEnabledCheckBox.Checked = extensionSettings.ExtensionSessionProcessingEnabled;

            AnalyseAllSessionsButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            AnalyseSelectedSessionsButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            ClearAllSessionAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            ClearSelectedSessionAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;

            CaptureTrafficCheckBox.Checked = extensionSettings.CaptureTraffic;

            NeverWebCallCheckBox.Checked = extensionSettings.NeverWebCall;

            DebugModeCheckBox.Checked = extensionSettings.DebugMode;

            if (extensionSettings.DebugMode)
            {
                DebugGroupBox.Visible = true;
            }
            else {
                DebugGroupBox.Visible = false;
            }

            ExecutionCountTextBox.Text = extensionSettings.ExecutionCount.ToString();
            NextUpdateCheckTextBox.Text = extensionSettings.NextUpdateCheck.ToString();

            CreateConsolidatedAnalysisButton.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;

            if (extensionSettings.SessionAnalysisOnLoadSaz == true &&
                extensionSettings.SessionAnalysisOnLiveTrace == true)
            {
                AllSessionAnalysisRadioButton.Checked = true;
                SomeSessionAnalysisRadioButton.Checked = false;

                SessionAnalysisOnLoadSazCheckBox.Checked = true;
                SessionAnalysisOnLoadSazCheckBox.Enabled = false;

                SessionAnalysisOnLoadSazCheckBox.Checked = true;
                SessionAnalysisOnLiveTraceCheckBox.Enabled = false;
            }
            else
            {
                AllSessionAnalysisRadioButton.Checked = false;
                SomeSessionAnalysisRadioButton.Checked = true;

                if (SettingsJsonService.Instance.SessionAnalysisOnLoadSaz)
                {
                    SessionAnalysisOnLoadSazCheckBox.Checked = true;
                }
                else
                {
                    SessionAnalysisOnLoadSazCheckBox.Checked = false;
                }

                if (SettingsJsonService.Instance.SessionAnalysisOnLiveTrace)
                {
                    SessionAnalysisOnLiveTraceCheckBox.Checked = true;
                }
                else
                {
                    SessionAnalysisOnLiveTraceCheckBox.Checked = false;
                }
            }

            if (AllSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnLiveTraceCheckBox.Enabled = false;
                SessionAnalysisOnLoadSazCheckBox.Enabled = false;
            }

            if (SomeSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnLiveTraceCheckBox.Enabled = true;
                SessionAnalysisOnLoadSazCheckBox.Enabled = true;
            }

            if (VersionService.Instance.IsExtensionDLLUpdateAvailable())
            {
                ExtensionVersionLabel.Text = $"{LangHelper.GetString("Extension")} v" +
                    $"{VersionService.Instance.GetExtensionDLLVersion()} - " +
                    LangHelper.GetString("Update Available");
                ExtensionVersionLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                ExtensionVersionLabel.Text = $"{LangHelper.GetString("Extension")} v" +
                    $"{VersionService.Instance.GetExtensionDLLVersion()} - " + 
                    LangHelper.GetString("Up To Date");
                ExtensionVersionLabel.ForeColor = System.Drawing.Color.Green;
            }

            if (VersionService.Instance.IsRulesetDLLUpdateAvailable())
            {
                RulesetVersionLabel.Text = $"{LangHelper.GetString("Ruleset")} v" +
                    $"{VersionService.Instance.GetExtensionRulesetDLLVersion()} - " +
                    LangHelper.GetString("Update Available");
                RulesetVersionLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                RulesetVersionLabel.Text = $"{LangHelper.GetString("Ruleset")} v" +
                    $"{VersionService.Instance.GetExtensionRulesetDLLVersion()} - " +
                    LangHelper.GetString("Up To Date");
                RulesetVersionLabel.ForeColor = System.Drawing.Color.Green;
            }

            // Follow up the above with overrides if never web call is true.

            if (extensionSettings.NeverWebCall)
            {
                ExtensionVersionLabel.Text = $"{LangHelper.GetString("Extension")} v" +
                    $"{VersionService.Instance.GetExtensionDLLVersion()} - " +
                    LangHelper.GetString("Never Web Call");
                ExtensionVersionLabel.ForeColor = System.Drawing.Color.Black;

                RulesetVersionLabel.Text = $"{LangHelper.GetString("Ruleset")} v" +
                    $"{VersionService.Instance.GetExtensionRulesetDLLVersion()} - " +
                    LangHelper.GetString("Never Web Call");
                RulesetVersionLabel.ForeColor = System.Drawing.Color.Black;
            }

            UpdateLinkLabel.Text = URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer;
        }

        public void AddPlaceholderText(object sender, EventArgs e)
        {
            SetPlaceHolderText();
        }

        public void RemovePlaceholderText(object sender, EventArgs e)
        {
            SetPlaceHolderText();
        }

        public void SetPlaceHolderText()
        {
            if (String.IsNullOrWhiteSpace(EnterIPAddressTextBox.Text))
            {
                EnterIPAddressTextBox.ForeColor = Color.Gray;
                EnterIPAddressTextBox.Font = new Font(EnterIPAddressTextBox.Font, FontStyle.Italic);
                EnterIPAddressTextBox.Text = strPlaceHolderText;
            }
            else if (EnterIPAddressTextBox.Text == strPlaceHolderText)
            {
                EnterIPAddressTextBox.ForeColor = Color.Black;
                EnterIPAddressTextBox.Font = new Font(EnterIPAddressTextBox.Font, FontStyle.Regular);
                EnterIPAddressTextBox.Text = "";
            }
        }

        private void ExtensionEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetExtensionSessionProcessingEnabled(ExtensionEnabledCheckBox.Checked);

            if (ExtensionEnabledCheckBox.Checked)
            {
                AllSessionAnalysisRadioButton.Enabled = true;
                SomeSessionAnalysisRadioButton.Enabled = true;
                //SessionAnalysisOnLoadSazCheckBox.Enabled = SomeSessionAnalysisRadioButton.Checked;
                //SessionAnalysisOnLiveTraceCheckBox.Enabled = SomeSessionAnalysisRadioButton.Checked;                
            }
            else
            {
                AllSessionAnalysisRadioButton.Enabled = false;
                SomeSessionAnalysisRadioButton.Enabled = false;
                //SessionAnalysisOnLoadSazCheckBox.Enabled = SomeSessionAnalysisRadioButton.Checked;
                //SessionAnalysisOnLiveTraceCheckBox.Enabled = SomeSessionAnalysisRadioButton.Checked;
            }

            this.UpdateUIControls();
            MenuUI.Instance.UpdateUIControls();
            ContextMenuUI.Instance.UpdateUIControls();
        }

        private void AllSessionAnalysisRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (AllSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnLoadSazCheckBox.Enabled = false;
                SessionAnalysisOnLoadSazCheckBox.Checked = true;

                SessionAnalysisOnLiveTraceCheckBox.Enabled = false;
                SessionAnalysisOnLiveTraceCheckBox.Checked = true;
            }
        }

        private void SomeSessionAnalysisRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SomeSessionAnalysisRadioButton.Checked)
            {
                SessionAnalysisOnLoadSazCheckBox.Enabled = true;
                SessionAnalysisOnLiveTraceCheckBox.Enabled = true;
            }
        }

        private void SessionAnalysisOnLoadSazCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetSessionAnalysisOnLoadSaz(SessionAnalysisOnLoadSazCheckBox.Checked);
        }

        private void SessionAnalysisOnLiveTraceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetSessionAnalysisOnLiveTrace(SessionAnalysisOnLiveTraceCheckBox.Checked);
        }

        private void AnalyseAllSessionsButton_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.AnalyseAllSessions();
        }

        private void ClearAllSessionAnalysisButton_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ClearAnalysisAllSessions();
        }

        private void AnalyseSelectedSessionsButton_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.AnalyseSelectedSessions();
        }

        private void ClearSelectedSessionAnalysisButton_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ClearAnalysisSelectedSessions();
        }

        private void CreateConsolidatedAnalysisButton_Click(object sender, EventArgs e)
        {
            ConsolidatedAnalysisReportService.Instance.CreateCAR();
        }

        private void CheckIPAddressButton_Click(object sender, EventArgs e)
        {
            if (!NetworkingService.Instance.IsValidIPAddress(EnterIPAddressTextBox.Text))
            {
                CheckIPAddressResultTextBox.Text = $"{EnterIPAddressTextBox.Text} is not a valid IP address.";
                EnterIPAddressTextBox.Text = "";
                SetPlaceHolderText();
                return;
            }

            Tuple<bool, string> tupleIsPrivateIPAddress = NetworkingService.Instance.IsPrivateIPAddress(EnterIPAddressTextBox.Text);

            // IP address is in a private subnet.
            if (tupleIsPrivateIPAddress.Item1)
            {
                CheckIPAddressResultTextBox.Text = $"{EnterIPAddressTextBox.Text} is within a private {tupleIsPrivateIPAddress.Item2} network";
            }
            // IP address is not in a private subnet.
            else
            {
                Tuple<bool, string> tupleIsMicrosoftIPAddress = NetworkingService.Instance.IsMicrosoft365IPAddress(EnterIPAddressTextBox.Text);

                // IP address is a Microsoft 365 IP address.
                if (tupleIsMicrosoftIPAddress.Item1)
                {
                    CheckIPAddressResultTextBox.Text = $"{EnterIPAddressTextBox.Text} is within the Microsoft 365 subnet {tupleIsMicrosoftIPAddress.Item2}";
                }
                // IP address is not a Microsoft 365 IP address.
                else
                {
                    CheckIPAddressResultTextBox.Text = $"{EnterIPAddressTextBox.Text} is a public IP address not within a Microsoft 365 subnet.";
                }
            }
        }

        private void CheckIPAddressClearButton_Click(object sender, EventArgs e)
        {
            EnterIPAddressTextBox.Text = "";
            SetPlaceHolderText();
            CheckIPAddressResultTextBox.Text = "";
        }

        private void UpdateLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(URLsJsonService.Instance.GetDeserializedExtensionURLs().Installer);
        }

        private void NeverWebCallCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetNeverWebCall(NeverWebCallCheckBox.Checked);
        }

        private void DebugModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetDebugMode(DebugModeCheckBox.Checked);
        }

        private void CaptureTrafficCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetCaptureOnStartup(CaptureTrafficCheckBox.Checked);

            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().CaptureTraffic)
            {
                FiddlerApplication.UI.actAttachProxy();
            }
            else
            {
                FiddlerApplication.UI.actDetachProxy();
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
            // Add tab page to Fiddler.
            oPage = new TabPage($"{LangHelper.GetString("Office 365 Fiddler Extension")}");
            oPage.ImageIndex = (int)Fiddler.SessionIcons.HTML;

            oView.Dock = DockStyle.Fill;

            oPage.Controls.Add(oView);
        }
    }
}
