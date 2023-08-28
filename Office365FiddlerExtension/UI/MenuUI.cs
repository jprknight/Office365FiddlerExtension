﻿using Office365FiddlerExtension.Services;
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

        public MenuItem MiLanguage { get; set; }

        public MenuItem MiLanguageEnglishENGB { get; set; }

        public MenuItem MiLanguageEnglishENUS { get; set; }

        public MenuItem MiReleasesDownloadWebpage { get; set; }

        public MenuItem MiWiki { get; set; }

        public MenuItem MiReportIssues { get; set; }

        public MenuItem MiAbout { get; set; }

        public string MenuEnabled = $"{LangHelper.GetString("Office365")} ({LangHelper.GetString("Enabled")})";

        public string MenuDisabled = $"{LangHelper.GetString("Office365")} ({LangHelper.GetString("Disabled")})";

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

                this.MiEnabled = new MenuItem(LangHelper.GetString("Enable"), new EventHandler(this.MiEnabled_Click))
                {
                    Checked = SettingsJsonService.Instance.ExtensionSessionProcessingEnabled
                };

                this.MiLanguage = new MenuItem(LangHelper.GetString("Language"));

                this.MiLanguageEnglishENGB = new MenuItem($"{LangHelper.GetString("English")} (en-GB)", new EventHandler(this.MiLanguageEnglishENGB_Click))
                {
                    Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("en-GB")
                };
            

                this.MiLanguageEnglishENUS = new MenuItem($"{LangHelper.GetString("English")} (en-US)", new EventHandler(this.MiLanguageEnglishENUS_Click))
                {
                    Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("en-US")
                };

                this.MiReleasesDownloadWebpage = new MenuItem($"{LangHelper.GetString("Releases")}", new System.EventHandler(this.MiReleasesDownloadWebpage_click));

                this.MiWiki = new MenuItem($"{LangHelper.GetString("Wiki")}", new System.EventHandler(this.MiWiki_Click));

                this.MiReportIssues = new MenuItem($"{LangHelper.GetString("ReportIssues")}", new System.EventHandler(this.MiReportIssues_Click));

                this.MiAbout = new MenuItem($"{LangHelper.GetString("About")}", new System.EventHandler(this.MiAbout_Click));

                // Add menu items to top level menu.
                this.ExtensionMenu.MenuItems.AddRange(new MenuItem[] { this.MiEnabled,
                    this.MiLanguage,
                    new MenuItem("-"),
                    this.MiReleasesDownloadWebpage,
                    this.MiWiki,
                    this.MiReportIssues,
                    new MenuItem("-"),
                    this.MiAbout
                });

                this.MiLanguage.MenuItems.AddRange(new MenuItem[] {
                    this.MiLanguageEnglishENGB,
                    this.MiLanguageEnglishENUS
                });
                

                FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExtensionMenu);
                IsInitialized = true;
            }
        }

        private void MiLanguageEnglishENUS_Click(object sender, EventArgs e)
        {
            LangHelper.ChangeLanguage("en-US");

            MiLanguageEnglishENUS.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("en-US");
            MiLanguageEnglishENGB.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("en-GB");
        }

        private void MiLanguageEnglishENGB_Click(object sender, EventArgs e)
        {
            LangHelper.ChangeLanguage("en-GB");

            MiLanguageEnglishENUS.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("en-US");
            MiLanguageEnglishENGB.Checked = SettingsJsonService.Instance.GetPreferredLanguageBool("en-GB");
        }

        private void MiAbout_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void MiClearAllSessionProcessing_Click(object sender, EventArgs e)
        {
            SessionFlagService.Instance.ClearAnalysisSelectedSessions();
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
