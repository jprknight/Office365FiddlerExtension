using Office365FiddlerExtension.Services;
using Fiddler;
using System;
using System.Windows.Forms;
using Microsoft.Extensions.FileSystemGlobbing;
using Office365FiddlerExtension.UI;

namespace Office365FiddlerExtension
{
    public class MenuUI
    {

        private static MenuUI _instance;

        public static MenuUI Instance => _instance ?? (_instance = new MenuUI());

        public MenuUI() { }

        public MenuItem ExtensionMenu { get; set; }

        public MenuItem MiEnabled { get; set; }

        public MenuItem MiSessionAnalysis { get; set; }

        public MenuItem MiOnFiddlerLoad { get; set; }

        public MenuItem MiOnLoadSaz { get; set; }

        public MenuItem MiOnLiveTrace { get; set; }

        public MenuItem MiProcessAllSessions { get; set; }

        public MenuItem MiClearAllSessionProcessing { get; set; }

        public MenuItem MiReleasesDownloadWebpage { get; set; }

        public MenuItem MiWiki { get; set; }

        public MenuItem MiReportIssues { get; set; }

        public MenuItem MiAbout { get; set; }

        public MenuItem MiAbout2 { get; set; }

        public string MenuEnabled = "Office 365 (Enabled)";

        public string MenuDisabled = "Office 365 (Disabled)";

        private bool IsInitialized { get; set; }

        public void Initialize()
        {
            /// <remarks>
            /// If this is the first time the extension has been run, make sure all extension options are enabled.
            /// Beyond do nothing other than keep a running count of the number of extension executions.
            /// </remarks>
            /// 
            if (!IsInitialized)
            {

                this.ExtensionMenu = new MenuItem(SettingsHandler.Instance.ExtensionEnabled ? MenuEnabled : MenuDisabled);

                this.MiEnabled = new MenuItem("Enable", new EventHandler(this.MiEnabled_Click));
                this.MiEnabled.Checked = SettingsHandler.Instance.ExtensionEnabled;

                this.MiSessionAnalysis = new MenuItem("Session Analysis");

                this.MiOnFiddlerLoad = MiSessionAnalysis.MenuItems.Add("On Fiddler Load", new EventHandler(this.MiSessionAnalysisOnFiddlerLoad_Click));

                this.MiOnLoadSaz = MiSessionAnalysis.MenuItems.Add("On Load Saz", new EventHandler(this.MiSessionAnalysisOnLoadSaz_Click));

                this.MiOnLiveTrace = MiSessionAnalysis.MenuItems.Add("On Live Trace", new EventHandler(this.MiSessionAnalysisOnLiveTrace_Click));

                this.MiProcessAllSessions = new MenuItem("Process All Sessions", new EventHandler(this.MiProcessAllSessions_Click));

                this.MiClearAllSessionProcessing = new MenuItem("Clear All Session Processing", new EventHandler(this.MiClearAllSessionProcessing_Click));

                this.MiReleasesDownloadWebpage = new MenuItem("&Releases Download Page", new System.EventHandler(this.MiReleasesDownloadWebpage_click));

                this.MiWiki = new MenuItem("Extension &Wiki", new System.EventHandler(this.MiWiki_Click));

                this.MiReportIssues = new MenuItem("&Report Issues", new System.EventHandler(this.MiReportIssues_Click));

                this.MiAbout = new MenuItem("&About", new System.EventHandler(this.MiAbout_Click));

                this.MiAbout2 = new MenuItem("&About2", new System.EventHandler(this.MiAbout2_Click));

                // Add menu items to top level menu.
                this.ExtensionMenu.MenuItems.AddRange(new MenuItem[] { this.MiEnabled,
                this.MiSessionAnalysis,
                new MenuItem("-"),
                this.MiProcessAllSessions,
                this.MiClearAllSessionProcessing,
                new MenuItem("-"),
                this.MiReleasesDownloadWebpage,
                this.MiWiki,
                this.MiReportIssues,
                new MenuItem("-"),
                this.MiAbout,
                this.MiAbout2
            });

                FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExtensionMenu);
                UpdateMenuItems();
                IsInitialized = true;
            }
        }

        private void MiAbout2_Click(object sender, EventArgs e)
        {
            AboutNew about = new AboutNew();
            about.Show();
        }

        private void MiSessionAnalysisOnLiveTrace_Click(object sender, EventArgs e)
        {
            //Preferences.SessionAnalysisOnLiveTrace = MiOnLiveTrace.Checked;
        }

        private void MiSessionAnalysisOnLoadSaz_Click(object sender, EventArgs e)
        {
            //Preferences.SessionAnalysisOnLoadSaz = MiOnLoadSaz.Checked;
        }

        private void MiSessionAnalysisOnFiddlerLoad_Click(object sender, EventArgs e)
        {
            //Preferences.SessionAnalysisOnFiddlerLoad = MiOnFiddlerLoad.Checked;
        }

        private void MiClearAllSessionProcessing_Click(object sender, EventArgs e)
        {
            SessionFlagHandler.Instance.ClearAllSessionProcessing();
        }

        private void MiProcessAllSessions_Click(object sender, EventArgs e)
        {
            if (SettingsHandler.Instance.ExtensionEnabled)
            {
                SessionFlagHandler.Instance.ProcessAllSessions();
            }
            else
            {
                string message = "The extension is currently disabled. Do you want to enable it to be able to process the currently loaded sessions?";

                string caption = "Process all sessions: Enable the extension?";

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                MessageBoxIcon icon = MessageBoxIcon.Question;

                DialogResult result;

                result = MessageBox.Show(message, caption, buttons, icon);
                
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    SettingsHandler.Instance.ExtensionEnabled = true;
                    this.MiEnabled.Checked = true;
                    //this.ExtensionMenu.Text = MenuEnabled;
                    SessionFlagHandler.Instance.ProcessAllSessions();
                }
            }
        }

        // Menu item event handlers.
        public void MiEnabled_Click(object sender, EventArgs e)
        {
            MiEnabled.Checked = !MiEnabled.Checked;
            SettingsHandler.Instance.ExtensionEnabled = MiEnabled.Checked;
            UpdateMenuItems();
        }

        public void UpdateEnabledChecked()
        {

        }

        public void UpdateMenuItems()
        {
            if (SettingsHandler.Instance.ExtensionEnabled)
            {
                //this.ExtensionMenu.Text = MenuEnabled;
                this.MiProcessAllSessions.Enabled = true;
            }
            else
            {
                //this.ExtensionMenu.Text = MenuDisabled;
                this.MiProcessAllSessions.Enabled = false;
            }
        }

        public void MiWiki_Click(object sender, EventArgs e)
        {
            var URLs = SettingsHandler.Instance.GetDeserializedExtensionURLs();

            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(URLs.Wiki);
        }

        public void MiReleasesDownloadWebpage_click(object sender, EventArgs e)
        {
            var URLs = SettingsHandler.Instance.GetDeserializedExtensionURLs();
            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(URLs.Installer);
        }

        public void MiReportIssues_Click(object sender, EventArgs e)
        {
            var URLs = SettingsHandler.Instance.GetDeserializedExtensionURLs();
            // Fire up a web browser to the project issues URL.
            System.Diagnostics.Process.Start(URLs.ReportIssues);
        }

        public void MiAbout_Click(object sender, EventArgs e)
        {
            // Since the user has manually clicked this menu item, check for updates,
            // set this boolean variable to true so we can give user feedback if no update available.

            var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();

            // REVIEW THIS. No need for an update check to be directly linked to a menu click.

            // Check for app update.
            if (!ExtensionSettings.NeverWebCall)
            {
                //Preferences.ManualCheckForUpdate = true;
                About.Instance.CheckForUpdate();
            }
        }
    }
}
