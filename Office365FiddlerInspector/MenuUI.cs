using Office365FiddlerInspector.Services;
using Fiddler;
using System;
using System.Windows.Forms;

namespace Office365FiddlerInspector
{
    public class MenuUI
    {

        private static MenuUI _instance;

        public static MenuUI Instance => _instance ?? (_instance = new MenuUI());

        public MenuUI() { }

        public MenuItem ExchangeOnlineTopMenu { get; set; }

        public MenuItem MiEnabled { get; set; }

        public MenuItem MiReleasesDownloadWebpage { get; set; }

        public MenuItem MiWiki { get; set; }

        public MenuItem MiReportIssues { get; set; }

        public MenuItem MiAbout { get; set; }

        //private int iExecutionCount { get; set; }

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

                this.ExchangeOnlineTopMenu = new MenuItem(Preferences.ExtensionEnabled ? "Office 365" : "Office 365 (Disabled)");

                this.MiEnabled = new MenuItem("Enable", new EventHandler(this.MiEnabled_Click));
                this.MiEnabled.Checked = Preferences.ExtensionEnabled;

                this.MiReleasesDownloadWebpage = new MenuItem("&Releases Download Page", new System.EventHandler(this.MiReleasesDownloadWebpage_click));

                this.MiWiki = new MenuItem("Extension &Wiki", new System.EventHandler(this.MiWiki_Click));

                this.MiReportIssues = new MenuItem("&Report Issues", new System.EventHandler(this.MiReportIssues_Click));

                this.MiAbout = new MenuItem("&About", new System.EventHandler(this.MiAbout_Click));

                // Add menu items to top level menu.
                this.ExchangeOnlineTopMenu.MenuItems.AddRange(new MenuItem[] { this.MiEnabled,
                new MenuItem("-"),
                this.MiReleasesDownloadWebpage,
                this.MiWiki,
                this.MiReportIssues,
                new MenuItem("-"),
                this.MiAbout
            });

                FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExchangeOnlineTopMenu);

                IsInitialized = true;
            }
        }

        // Menu item event handlers.
        public void MiEnabled_Click(object sender, EventArgs e)
        {
            MiEnabled.Checked = !MiEnabled.Checked;
            Preferences.ExtensionEnabled = MiEnabled.Checked;

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
