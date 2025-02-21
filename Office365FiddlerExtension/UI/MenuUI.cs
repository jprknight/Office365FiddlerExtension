using Office365FiddlerExtension.Services;
using Fiddler;
using System;
using System.Windows.Forms;
using Office365FiddlerExtension.UI;
using System.Reflection;
using Newtonsoft.Json;
using Office365FiddlerExtension.UI.Forms;

namespace Office365FiddlerExtension
{
    /// <summary>
    /// Add menu into Fiddler application UI and populate with data.
    /// </summary>
    public class MenuUI
    {
        private static MenuUI _instance;

        public static MenuUI Instance => _instance ?? (_instance = new MenuUI());

        public MenuUI() { }

        public MenuItem ExtensionMenu { get; set; }

        public MenuItem MiEnabled { get; set; }

        public MenuItem MiLanguage { get; set; }

        public MenuItem MiLanguage_EN { get; set; }

        public MenuItem MiLanguage_FR { get; set; }

        public MenuItem MiLanguage_DE { get; set; }

        public MenuItem MiLanguage_PT { get; set; }

        public MenuItem MiLanguage_ES { get; set; }

        public MenuItem MiAnalyseAllSessions { get; set; }

        public MenuItem MiClearAllSessionAnalysis { get; set; }

        public MenuItem MiCreateConsolidatedAnalysisReport { get; set; }

        public MenuItem MiReleasesDownloadWebpage { get; set; }

        public MenuItem MiWiki { get; set; }

        public MenuItem MiReportIssues { get; set; }

        public MenuItem MiAbout { get; set; }

        public string MenuEnabled = $"{LangHelper.GetString("Office 365")} ({LangHelper.GetString("Enabled")})";

        public string MenuDisabled = $"{LangHelper.GetString("Office 365")} ({LangHelper.GetString("Disabled")})";

        private bool IsInitialized { get; set; }

        /// <summary>
        /// Create and add menu into Fiddler UI.
        /// </summary>
        public void Initialize()
        {
            if (!IsInitialized)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to add menu to Fiddler UI.");

                try
                {
                    this.ExtensionMenu = new MenuItem(SettingsJsonService.Instance.ExtensionSessionProcessingEnabled ? MenuEnabled : MenuDisabled);

                    this.MiEnabled = new MenuItem(LangHelper.GetString("Enable"), new EventHandler(this.MiEnabled_Click))
                    {
                        Checked = SettingsJsonService.Instance.ExtensionSessionProcessingEnabled
                    };

                    this.MiLanguage = new MenuItem(LangHelper.GetString("Language"));

                    this.MiLanguage_EN = new MenuItem($"{LangHelper.GetString("English")}", new EventHandler(this.MiLanguage_EN_Click))
                    {
                        Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("EN-GB")
                    };

                    this.MiLanguage_FR = new MenuItem($"{LangHelper.GetString("French")}", new EventHandler(this.MiLanguage_FR_Click))
                    {
                        Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("FR-FR")
                    };

                    this.MiLanguage_DE = new MenuItem($"{LangHelper.GetString("German")}", new EventHandler(this.MiLanguage_DE_Click))
                    {
                        Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("DE-DE")
                    };

                    this.MiLanguage_PT = new MenuItem($"{LangHelper.GetString("Portuguese")}", new EventHandler(this.MiLanguage_PT_Click))
                    {
                        Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("PT-BR")
                    };

                    this.MiLanguage_ES = new MenuItem($"{LangHelper.GetString("Spanish")}", new EventHandler(this.MiLanguage_ES_Click))
                    {
                        Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("ES-US")
                    };

                    this.MiAnalyseAllSessions = new MenuItem($"{LangHelper.GetString("Analyse All Sessions")}", new System.EventHandler(this.MiAnalyseAllSessions_Click))
                    {
                        Enabled = SettingsJsonService.Instance.ExtensionSessionProcessingEnabled
                    };

                    this.MiClearAllSessionAnalysis = new MenuItem($"{LangHelper.GetString("Clear All Session Analysis")}", new System.EventHandler(this.MiClearAllSessionAnalysis_Click))
                    {
                        Enabled = SettingsJsonService.Instance.ExtensionSessionProcessingEnabled
                    };

                    this.MiCreateConsolidatedAnalysisReport = new MenuItem($"{LangHelper.GetString("Create Consolidated Analysis Report")}", new System.EventHandler(this.MiCreateConsolidatedAnalysisReport_Click))
                    {
                        Enabled = SettingsJsonService.Instance.ExtensionSessionProcessingEnabled
                    };

                    this.MiReleasesDownloadWebpage = new MenuItem($"{LangHelper.GetString("Releases")}", new System.EventHandler(this.MiReleasesDownloadWebpage_click));

                    this.MiWiki = new MenuItem($"{LangHelper.GetString("Wiki")}", new System.EventHandler(this.MiWiki_Click));

                    this.MiReportIssues = new MenuItem($"{LangHelper.GetString("Report Issues")}", new System.EventHandler(this.MiReportIssues_Click));

                    this.MiAbout = new MenuItem($"{LangHelper.GetString("About")}", new System.EventHandler(this.MiAbout_Click));

                    // Add menu items to top level menu.
                    this.ExtensionMenu.MenuItems.AddRange(new MenuItem[] { //this.MiEnabled,
                    //new MenuItem("-"),
                    this.MiAnalyseAllSessions,
                    this.MiClearAllSessionAnalysis,
                    new MenuItem("-"),
                    this.MiCreateConsolidatedAnalysisReport,
                    new MenuItem ("-"),
                    this.MiReleasesDownloadWebpage,
                    this.MiWiki,
                    this.MiReportIssues,
                    new MenuItem("-"),
                    this.MiLanguage,
                    new MenuItem("-"),
                    this.MiAbout
                });

                    this.MiLanguage.MenuItems.AddRange(new MenuItem[] {
                    this.MiLanguage_EN,
                    this.MiLanguage_FR,
                    this.MiLanguage_DE,
                    this.MiLanguage_PT,
                    this.MiLanguage_ES
                });

                    FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExtensionMenu);

                    this.MiLanguage_FR.Enabled = false;
                    this.MiLanguage_DE.Enabled = false;
                    //this.MiLanguage_PT.Enabled = false;
                    //this.MiLanguage_ES.Enabled = false;

                    IsInitialized = true;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
                }

            }
        }

        public void RemoveMenu()
        {
            FiddlerApplication.UI.mnuMain.MenuItems.Remove(this.ExtensionMenu);
        }

        private void CheckLanguageSelection()
        {
            MiLanguage_EN.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("EN-GB");
            MiLanguage_FR.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("FR-FR");
            MiLanguage_DE.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("DE-DE");
            MiLanguage_PT.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("PT-BR");
            MiLanguage_ES.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("ES-US");
        }

        private void MiLanguage_EN_Click(object sender, EventArgs e)
        {
            LangHelper.ChangeLanguage("EN-GB");

            CheckLanguageSelection();
        }


        private void MiLanguage_FR_Click(object sender, EventArgs e)
        {
            LangHelper.ChangeLanguage("FR-FR");

            CheckLanguageSelection();
        }

        private void MiLanguage_DE_Click(object sender, EventArgs e)
        {
            LangHelper.ChangeLanguage("DE-DE");

            CheckLanguageSelection();
        }

        private void MiLanguage_PT_Click(object sender, EventArgs e)
        {
            LangHelper.ChangeLanguage("PT-BR");

            CheckLanguageSelection();
        }

        private void MiLanguage_ES_Click(object sender, EventArgs e)
        {
            LangHelper.ChangeLanguage("ES-US");

            CheckLanguageSelection();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiAbout_Click(object sender, EventArgs e)
        {
            // Back door to enable debug mode from the extension frontend.
            if (Control.ModifierKeys == Keys.Shift)
            {
                var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();
                extensionSettings.DebugMode = !extensionSettings.DebugMode;
                Preferences.ExtensionSettings = JsonConvert.SerializeObject(extensionSettings);
            }

            About about = new About();
            about.Show();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiEnabled_Click(object sender, EventArgs e)
        {
            // Invert the menu item checked.
            MiEnabled.Checked = !MiEnabled.Checked;

            // Set ExtensionEnabled according to menu item checked.
            SettingsJsonService.Instance.SetExtensionSessionProcessingEnabled(MiEnabled.Checked);

            MenuUI.Instance.UpdateUIControls();
            ContextMenuUI.Instance.UpdateUIControls();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiWiki_Click(object sender, EventArgs e)
        {
            var URLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();

            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(URLs.Wiki);
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiReleasesDownloadWebpage_click(object sender, EventArgs e)
        {
            var URLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();
            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(URLs.Installer);
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiReportIssues_Click(object sender, EventArgs e)
        {
            var URLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();
            // Fire up a web browser to the project issues URL.
            System.Diagnostics.Process.Start(URLs.ReportIssues);
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiAnalyseAllSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.AnalyseAllSessions();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiClearAllSessionAnalysis_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ClearAnalysisAllSessions();
        }

        /// <summary>
        /// Action performed on menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiCreateConsolidatedAnalysisReport_Click(object sender, EventArgs e)
        {
            ConsolidatedAnalysisReportService.Instance.CreateCAR();
        }

        /// <summary>
        /// Function to update Menu UI controls.
        /// </summary>
        public void UpdateUIControls()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            MiEnabled.Checked = extensionSettings.ExtensionSessionProcessingEnabled;

            MiAnalyseAllSessions.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            MiClearAllSessionAnalysis.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
            MiCreateConsolidatedAnalysisReport.Enabled = extensionSettings.ExtensionSessionProcessingEnabled;
        }
    }
}
