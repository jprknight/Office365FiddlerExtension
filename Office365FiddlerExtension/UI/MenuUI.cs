using Office365FiddlerExtension.Services;
using Fiddler;
using System;
using System.Windows.Forms;

namespace Office365FiddlerExtension
{
    public class MenuUI
    {

        private static MenuUI _instance;

        public static MenuUI Instance => _instance ?? (_instance = new MenuUI());

        public MenuUI() { }

        public MenuItem ExtensionMenu { get; set; }

        public MenuItem MiEnabled { get; set; }

        public MenuItem MiProcessAllSessions { get; set; }

        public MenuItem MiClearAllSessionProcessing { get; set; }

        public MenuItem MiReleasesDownloadWebpage { get; set; }

        public MenuItem MiWiki { get; set; }

        public MenuItem MiReportIssues { get; set; }

        public MenuItem MiAbout { get; set; }

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

                this.ExtensionMenu = new MenuItem(Preferences.ExtensionEnabled ? MenuEnabled : MenuDisabled);

                this.MiEnabled = new MenuItem("Enable", new EventHandler(this.MiEnabled_Click));
                this.MiEnabled.Checked = Preferences.ExtensionEnabled;

                this.MiProcessAllSessions = new MenuItem("Process All Sessions", new EventHandler(this.MiProcessAllSessions_Click));

                this.MiClearAllSessionProcessing = new MenuItem("Clear All Session Processing", new EventHandler(this.MiClearAllSessionProcessing_Click));

                this.MiReleasesDownloadWebpage = new MenuItem("&Releases Download Page", new System.EventHandler(this.MiReleasesDownloadWebpage_click));

                this.MiWiki = new MenuItem("Extension &Wiki", new System.EventHandler(this.MiWiki_Click));

                this.MiReportIssues = new MenuItem("&Report Issues", new System.EventHandler(this.MiReportIssues_Click));

                this.MiAbout = new MenuItem("&About", new System.EventHandler(this.MiAbout_Click));

                // Add menu items to top level menu.
                this.ExtensionMenu.MenuItems.AddRange(new MenuItem[] { this.MiEnabled,
                new MenuItem("-"),
                this.MiProcessAllSessions,
                this.MiClearAllSessionProcessing,
                new MenuItem("-"),
                this.MiReleasesDownloadWebpage,
                this.MiWiki,
                this.MiReportIssues,
                new MenuItem("-"),
                this.MiAbout
            });

                FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExtensionMenu);
                UpdateMenuItems();
                IsInitialized = true;
            }
        }

        private void MiClearAllSessionProcessing_Click(object sender, EventArgs e)
        {
            SessionFlagProcessor.Instance.ClearAllSessionProcessing();
        }

        private void MiProcessAllSessions_Click(object sender, EventArgs e)
        {
            if (Preferences.ExtensionEnabled)
            {
                SessionFlagProcessor.Instance.ProcessAllSessions();
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
                    Preferences.ExtensionEnabled = true;
                    this.MiEnabled.Checked = true;
                    this.ExtensionMenu.Text = MenuEnabled;
                    SessionFlagProcessor.Instance.ProcessAllSessions();
                }
            }
        }

        // Menu item event handlers.
        public void MiEnabled_Click(object sender, EventArgs e)
        {
            MiEnabled.Checked = !MiEnabled.Checked;
            Preferences.ExtensionEnabled = MiEnabled.Checked;
            UpdateMenuItems();
        }

        public void UpdateMenuItems()
        {
            if (Preferences.ExtensionEnabled)
            {
                this.ExtensionMenu.Text = MenuEnabled;
                this.MiProcessAllSessions.Enabled = true;
            }
            else
            {
                this.ExtensionMenu.Text = MenuDisabled;
                this.MiProcessAllSessions.Enabled = false;
            }
        }

        public void MiWiki_Click(object sender, EventArgs e)
        {
            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(Properties.Settings.Default.WikiURL);
        }

        public void MiReleasesDownloadWebpage_click(object sender, EventArgs e)
        {
            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(Properties.Settings.Default.InstallerURL);
        }

        public void MiReportIssues_Click(object sender, EventArgs e)
        {
            // Fire up a web browser to the project issues URL.
            System.Diagnostics.Process.Start(Properties.Settings.Default.ReportIssuesURL);
        }

        public void MiAbout_Click(object sender, EventArgs e)
        {
            // Since the user has manually clicked this menu item, check for updates,
            // set this boolean variable to true so we can give user feedback if no update available.

            // Check for app update.
            if (!Preferences.DisableWebCalls)
            {
                Preferences.ManualCheckForUpdate = true;
                About.Instance.CheckForUpdate();
            }
        }
    }
}
