using Office365FiddlerExtension.Services;
using Fiddler;
using System;
using System.Windows.Forms;
using Microsoft.Extensions.FileSystemGlobbing;
using Office365FiddlerExtension.UI;
using System.Reflection;

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

        public MenuItem MiReleasesDownloadWebpage { get; set; }

        public MenuItem MiWiki { get; set; }

        public MenuItem MiReportIssues { get; set; }

        public MenuItem MiAbout { get; set; }

        public string MenuEnabled = $"Office 365 (Enabled)";

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
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Adding menu to UI.");

                this.ExtensionMenu = new MenuItem(SettingsJsonService.Instance.ExtensionSessionProcessingEnabled ? MenuEnabled : MenuDisabled);

                this.MiEnabled = new MenuItem("Enable", new EventHandler(this.MiEnabled_Click))
                {
                    Checked = SettingsJsonService.Instance.ExtensionSessionProcessingEnabled
                };

                this.MiReleasesDownloadWebpage = new MenuItem("&Releases Download Page", new System.EventHandler(this.MiReleasesDownloadWebpage_click));

                this.MiWiki = new MenuItem("Extension &Wiki", new System.EventHandler(this.MiWiki_Click));

                this.MiReportIssues = new MenuItem("&Report Issues", new System.EventHandler(this.MiReportIssues_Click));

                this.MiAbout = new MenuItem("&About", new System.EventHandler(this.MiAbout_Click));

                // Add menu items to top level menu.
                this.ExtensionMenu.MenuItems.AddRange(new MenuItem[] { this.MiEnabled,
                new MenuItem("-"),
                this.MiReleasesDownloadWebpage,
                this.MiWiki,
                this.MiReportIssues,
                new MenuItem("-"),
                this.MiAbout
            });

                FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExtensionMenu);
                IsInitialized = true;
            }
        }

        private void MiAbout_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private  void MiProcessSelectedSessions_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ProcessSelectedSessions();
        }

        private void MiClearAllSessionProcessing_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ClearAnalysisSelectedSessions();
        }

        private void MiProcessAllSessions_Click(object sender, EventArgs e)
        {
            if (SettingsJsonService.Instance.ExtensionSessionProcessingEnabled)
            {
                SessionFlagService.Instance.ProcessAllSessions();
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
                    SettingsJsonService.Instance.SetExtensionSessionProcessingEnabled(true);
                    this.MiEnabled.Checked = true;
                    SessionFlagService.Instance.ProcessAllSessions();
                }
            }
        }

        // Menu item event handlers.
        public void MiEnabled_Click(object sender, EventArgs e)
        {
            // Invert menu item checked.
            MiEnabled.Checked = !MiEnabled.Checked;
            // Set ExtensionEnabled according to menu item checked.
            SettingsJsonService.Instance.SetExtensionSessionProcessingEnabled(MiEnabled.Checked);
        }

        public void MiWiki_Click(object sender, EventArgs e)
        {
            var URLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();

            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(URLs.Wiki);
        }

        public void MiReleasesDownloadWebpage_click(object sender, EventArgs e)
        {
            var URLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();
            // Fire up a web browser to the project Wiki URL.
            System.Diagnostics.Process.Start(URLs.Installer);
        }

        public void MiReportIssues_Click(object sender, EventArgs e)
        {
            var URLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();
            // Fire up a web browser to the project issues URL.
            System.Diagnostics.Process.Start(URLs.ReportIssues);
        }
    }
}
