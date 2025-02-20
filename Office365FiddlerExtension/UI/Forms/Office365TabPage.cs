using Fiddler;
using System;
using System.Drawing;
using System.Windows.Forms;
using Office365FiddlerExtension.Services;
using static System.Windows.Forms.TabControl;

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

            if (extensionSettings.NeverWebCall)
            {
                CheckIPAddressGroupBox.Enabled = false;
                EnterIPAddressTextBox.Text = LangHelper.GetString("NeverWebCall_FeatureDisabled");
                CheckIPAddressResultTextBox.Text = LangHelper.GetString("NeverWebCall_FeatureDisabled");
            }
            else
            {
                CheckIPAddressGroupBox.Enabled = true;
                EnterIPAddressTextBox.Text = "";
                CheckIPAddressResultTextBox.Text = "";
                // Make sure the text box has placeholder text on load since it'll be empty.
                SetPlaceHolderText();
            }

            EnterIPAddressTextBox.GotFocus += RemovePlaceholderText;
            EnterIPAddressTextBox.LostFocus += AddPlaceholderText;

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

            CheckIPAddressGroupBox.Text = LangHelper.GetString("Check IP Address");
            CheckIPAddressButton.Text = LangHelper.GetString("Check");
            CheckIPAddressClearButton.Text = LangHelper.GetString("Clear");
            WhoisCheckBox.Checked = extensionSettings.Whois;

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
                CheckIPAddressResultTextBox.Text = $"{EnterIPAddressTextBox.Text} {LangHelper.GetString("IsNotAValidIPAddress")}";
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

                var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

                // If Whois lookups are enabled, go find out the organization name who owns this IP address.
                if (extensionSettings.Whois)
                {
                    CheckIPAddressResultTextBox.Text += $" {LangHelper.GetString("OwnedBy")} {NetworkingService.Instance.GetWhoisOrganizationName(EnterIPAddressTextBox.Text)}";
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

        private void SessionAnalysisOnImportCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetSessionAnlysisOnImport(SessionAnalysisOnImportCheckBox.Checked);
        }

        private void WarnBeforeAnalysingTextBox_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(WarnBeforeAnalysingTextBox.Text, "[^0-9]"))
            {
                string message = "This textbox only accepts numbers.";

                string caption = $"{LangHelper.GetString("Office 365 Fiddler Extension")}";

                MessageBox.Show(message, caption);
                WarnBeforeAnalysingTextBox.Text = WarnBeforeAnalysingTextBox.Text.Remove(WarnBeforeAnalysingTextBox.Text.Length - 1);
            }   
        }
    

        private void WhoisCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetWhois(WhoisCheckBox.Checked);
        }

        private void DebugModeUpdateButton_Click(object sender, EventArgs e)
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            ExtensionEnabledCheckBox.Checked = extensionSettings.ExtensionSessionProcessingEnabled;

            ExtensionSettingsTextbox.Text = Preferences.ExtensionSettings;
        }

        private void ElapsedTimeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetElapsedColumnEnabled(ElapsedTimeCheckbox.Checked);
            if (ElapsedTimeCheckbox.Checked)
            {
                ColumnUI.Instance.AddElapsedTimeColumn();
            }
        }

        private void SeverityCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetSeverityColumnEnabled(SeverityCheckbox.Checked);
            if (SeverityCheckbox.Checked)
            {
                ColumnUI.Instance.AddSeverityColumn();
            }
        }

        private void SessionTypeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetSessionTypeColumnEnabled(SessionTypeCheckbox.Checked);
            if (SessionTypeCheckbox.Checked)
            {
                ColumnUI.Instance.AddSessionTypeColumn();
            }
        }

        private void ResponseServerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetResponseServerColumnEnabled(ResponseServerCheckbox.Checked);
            if (ResponseServerCheckbox.Checked)
            {
                ColumnUI.Instance.AddResponseServerColumn();
            }
        }

        private void AuthenticationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsJsonService.Instance.SetAuthenticationColumnEnabled(AuthenticationCheckbox.Checked);
            if (AuthenticationCheckbox.Checked)
            {
                ColumnUI.Instance.AddAuthenticationColumn();
            }
        }

        private void HostIPCheckbox_CheckedChanged(object sender, EventArgs e)
        {
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

        // REVIEW THIS.
        //
        // Attempting to get the Enable/Disable menu item to effect the Tab Page.
        public void Refresh()
        {
            

            TabPageCollection tabPages = FiddlerApplication.UI.tabsViews.TabPages;

            foreach (TabPage tabpage in tabPages)
            {
                if (tabpage.Text.Equals(LangHelper.GetString("Office 365 Fiddler Extension")))
                {

                    //tabpage.Invalidate();
                    //tabpage.Dispose();
                    //tabpage.Update();
                    //tabpage.Refresh();

                    //tabpage.Select();

                    

                    //string message = $"You hit the refresh function. {tabpage.Text}";
                    //string caption = "Refresh";
                    //MessageBoxButtons buttons = MessageBoxButtons.OK;
                    //DialogResult result;

                    // Displays the MessageBox.
                    //result = MessageBox.Show(message, caption, buttons);

                    // This code gets into the tabpage, but none of these things can trigger a UI
                    // update of the enabled / disabled checkbox.
                    // The only thing I've successfully been able to do is trigger a dispose event
                    // which removes the tab page from the application.

                    //tabpage.Dispose();

                    //Office365TabPage.Instance.UpdateUIControls();

                    //Office365TabPage.Instance.InvertExtensionEnabledCheckbox();

                    //tabpage.Invalidate();
                    //tabpage.Update();

                    //tabpage.Controls.Clear();
                    //tabpage.Controls.Add(oView);
                    //tabpage.Controls.update();

                    //tabpage.Invalidate();
                    //tabpage.Refresh();

                }
            }
        }

        /// <summary>
        /// Throwing the kitchen sink at trying to get the tab control to update from outside of a direct click on the tab control.
        /// With the below the UI switches to the tab page, the value of the extension enabled checkbox is changing from start to
        /// finish, just that the UI does not update the checkbox to complete the process.
        /// </summary>
        public void UpdateOPage()
        {
            TabPageCollection tabPages = FiddlerApplication.UI.tabsViews.TabPages;

            foreach (TabPage tabpage in tabPages)
            {
                if (tabpage.Text.Equals(LangHelper.GetString("Office 365 Fiddler Extension")))
                {
                    FiddlerObject.prompt($"Updating: {tabpage.Text}: {Office365TabPage.Instance.GetExtensionEnabledCheckbox()}");

                    tabpage.Show();
                    tabpage.BringToFront();
                    tabpage.Select();
                    tabpage.Focus();
                    //tabpage.Invalidate();
                    tabpage.Refresh();
                    tabpage.Update();
                    
                    //oView.Invalidate();
                    oView.Focus();
                    oView.Select();
                    oView.Refresh();
                    oView.Update();

                    //oPage.Invalidate();
                    oPage.Focus();
                    oPage.Select();
                    oPage.Refresh();
                    oPage.Update();

                    oPage.Controls.Add(oView);

                    Office365TabPage.Instance.UpdateUIControls();
                    
                    FiddlerObject.prompt($"Updated: {tabpage.Text}: {Office365TabPage.Instance.GetExtensionEnabledCheckbox()}");
                }
            }
        }

        public void OnBeforeUnload()
        {
            TabPageCollection tabPages = FiddlerApplication.UI.tabsViews.TabPages;

            foreach (TabPage tabpage in tabPages)
            {
                if (tabpage.Text.Equals(LangHelper.GetString("Office 365 Fiddler Extension")))
                {
                    tabpage.Dispose();
                }

            }
        }

        public Office365FiddlerExtensionTabPage()
        {
            oPage = new TabPage($"{LangHelper.GetString("Office 365 Fiddler Extension")}");
            oPage.ImageIndex = (int)Fiddler.SessionIcons.HTML;

            oView.Dock = DockStyle.Fill;

            oPage.Controls.Add(oView);
        }
    }
}