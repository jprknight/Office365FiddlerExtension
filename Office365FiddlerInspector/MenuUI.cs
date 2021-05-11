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

        public MenuUI()
        { }

        public MenuItem ExchangeOnlineTopMenu { get; set; }


        public MenuItem miEnabled { get; set; }

        //public MenuItem miAppLoggingEnabled { get; set; }

        public MenuItem miCheckForUpdate { get; set; }

        public MenuItem miHighlightOutlookOWAOnly { get; set; }

        public MenuItem miReleasesDownloadWebpage { get; set; }

        public MenuItem miWiki { get; set; }

        public MenuItem miReportIssues { get; set; }

        public MenuItem miAbout { get; set; }

        private int iExecutionCount { get; set; }

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

                this.miEnabled = new MenuItem("Enable", new EventHandler(this.miEnabled_Click));
                this.miEnabled.Checked = Preferences.ExtensionEnabled;

                this.miReleasesDownloadWebpage = new MenuItem("&Releases Download Page", new System.EventHandler(this.miReleasesDownloadWebpage_click));

                this.miWiki = new MenuItem("Extension &Wiki", new System.EventHandler(this.miWiki_Click));

                this.miReportIssues = new MenuItem("&Report Issues", new System.EventHandler(this.miReportIssues_Click));

                //this.miCheckForUpdate = new MenuItem("&Check For Update", new System.EventHandler(this.miCheckForUpdate_Click));

                this.miAbout = new MenuItem("&About", new System.EventHandler(this.miAbout_Click));

                // Add menu items to top level menu.
                this.ExchangeOnlineTopMenu.MenuItems.AddRange(new MenuItem[] { this.miEnabled,
                new MenuItem("-"),
                this.miReleasesDownloadWebpage,
                this.miWiki,
                this.miReportIssues,
                new MenuItem("-"),
                //this.miCheckForUpdate,
                this.miAbout
            });

                FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExchangeOnlineTopMenu);

                IsInitialized = true;
            }
        }

        // Menu item event handlers.
        public void miEnabled_Click(object sender, EventArgs e)
        {
            miEnabled.Checked = !miEnabled.Checked;
            Preferences.ExtensionEnabled = miEnabled.Checked;
        }

        public void miWiki_Click(object sender, EventArgs e)
        {
            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(Properties.Settings.Default.WikiURL);
        }

        public void miReleasesDownloadWebpage_click(object sender, EventArgs e)
        {
            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(Properties.Settings.Default.InstallerURL);
        }

        public void miReportIssues_Click(object sender, EventArgs e)
        {
            // Fire up a web browser to the project issues URL.
            System.Diagnostics.Process.Start(Properties.Settings.Default.ReportIssuesURL);
        }

        public void miAbout_Click(object sender, EventArgs e)
        {
            // Since the user has manually clicked this menu item to check for updates,
            // set this boolean variable to true so we can give user feedback if no update available.
                        
            Preferences.ManualCheckForUpdate = true;

            // Check for app update.
            About.Instance.CheckForUpdate();
        }
    }
}
