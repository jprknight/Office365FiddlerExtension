using EXOFiddlerInspector.Services;
using Fiddler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EXOFiddlerInspector 
{
    public class MenuUI : ActivationService    // Ensure class is public, or Fiddler won't see it!
    {
        private static MenuUI _instance;

        public static MenuUI Instance => _instance ?? (_instance = new MenuUI());

        public MenuUI()
        {

        }

        public MenuItem ExchangeOnlineTopMenu;
        public MenuItem miColumnsMenu;

        public MenuItem miEnabled;

        public MenuItem miSeperator1;
        public MenuItem miSeperator2;
        public MenuItem miSeperator3;
        public MenuItem miSeperator4;
        public MenuItem miColumnSeperator1;

        public MenuItem miColumnsEnableAll;

        public MenuItem miElapsedTimeColumnEnabled;

        public MenuItem miResponseServerColumnEnabled;

        public MenuItem miExchangeTypeColumnEnabled;

        public MenuItem miHostIPColumnEnabled;

        public MenuItem miAuthColumnEnabled;

        public MenuItem miAppLoggingEnabled;

        public MenuItem miCheckForUpdate;

        public MenuItem miHighlightOutlookOWAOnly;

        public MenuItem miReleasesDownloadWebpage;

        public MenuItem miWiki;

        public MenuItem miReportIssues;
        private int iExecutionCount;

        public void InitializeMenu()
        {
            // If the extension is enabled then take the menu title from the Fiddler application preference.
            if (Preferences.ExtensionEnabled)
            {
                this.ExchangeOnlineTopMenu = new MenuItem(FiddlerApplication.Prefs.GetStringPref("extensions.EXOFiddlerExtension.MenuTitle", "Exchange Online"));
            }
            // If th extension is disabled then use this instead.
            else
            {
                this.ExchangeOnlineTopMenu = new MenuItem("Exchange Online (Disabled)");
            }

            this.miEnabled = new MenuItem("&Extension Enabled");
            this.miEnabled.Index = 0;

            this.miSeperator1 = new MenuItem("-");
            this.miSeperator1.Index = 1;

            this.miColumnsMenu = new MenuItem("&Columns (Off/On)");
            this.miColumnsMenu.Index = 2;

            this.miSeperator2 = new MenuItem("-");
            this.miSeperator2.Index = 3;

            this.miAppLoggingEnabled = new MenuItem("Application &Logging Enabled");
            this.miAppLoggingEnabled.Index = 4;

            this.miHighlightOutlookOWAOnly = new MenuItem("&Highlight Outlook and OWA Only");
            this.miHighlightOutlookOWAOnly.Index = 5;

            this.miSeperator3 = new MenuItem("-");
            this.miSeperator3.Index = 6;

            this.miReleasesDownloadWebpage = new MenuItem("&Releases Download Page");
            this.miReleasesDownloadWebpage.Index = 7;

            this.miWiki = new MenuItem("Extension &Wiki");
            this.miWiki.Index = 8;

            this.miReportIssues = new MenuItem("&Report Issues");
            this.miReportIssues.Index = 9;


            this.miSeperator4 = new MenuItem("-");
            this.miSeperator4.Index = 10;

            this.miCheckForUpdate = new MenuItem("&Check For Update");
            this.miCheckForUpdate.Index = 11;

            // Add menu items to top level menu.
            this.ExchangeOnlineTopMenu.MenuItems.AddRange(new MenuItem[] { this.miEnabled,
                this.miSeperator1,
                this.miColumnsMenu,
                this.miSeperator2,
                this.miAppLoggingEnabled,
                this.miHighlightOutlookOWAOnly,
                this.miSeperator3,
                this.miReleasesDownloadWebpage,
                this.miWiki,
                this.miReportIssues,
                this.miSeperator4,
                this.miCheckForUpdate
            });

            // Columns menu items.

            this.miColumnsEnableAll = new MenuItem("Enable &All");
            this.miColumnsEnableAll.Index = 0;

            this.miColumnSeperator1 = new MenuItem("-");
            this.miColumnSeperator1.Index = 1;

            this.miElapsedTimeColumnEnabled = new MenuItem("Elapsed &Time (Load SAZ only)");
            this.miElapsedTimeColumnEnabled.Index = 2;

            this.miResponseServerColumnEnabled = new MenuItem("Response &Server");
            this.miResponseServerColumnEnabled.Index = 3;

            this.miExchangeTypeColumnEnabled = new MenuItem("Exchange T&ype");
            this.miExchangeTypeColumnEnabled.Index = 4;

            this.miHostIPColumnEnabled = new MenuItem("&Host IP");
            this.miHostIPColumnEnabled.Index = 5;

            this.miAuthColumnEnabled = new MenuItem("A&uthentication");
            this.miAuthColumnEnabled.Index = 6;

            this.miColumnsMenu.MenuItems.AddRange(new MenuItem[]
            {
                this.miColumnsEnableAll,
                this.miColumnSeperator1,
                this.miElapsedTimeColumnEnabled,
                this.miResponseServerColumnEnabled,
                this.miExchangeTypeColumnEnabled,
                this.miHostIPColumnEnabled,
                this.miAuthColumnEnabled
            });

            // Setup event handlers for menu items.
            this.miEnabled.Click += new System.EventHandler(this.miEnabled_Click);
            this.miEnabled.Checked = Preferences.ExtensionEnabled;

            this.miColumnsEnableAll.Click += new System.EventHandler(this.miColumnsEnableAll_Click);
            this.miColumnsEnableAll.Checked = Preferences.ColumnsAllEnabled;

            this.miElapsedTimeColumnEnabled.Click += new System.EventHandler(this.miElapsedTimeColumnEnabled_Click);
            this.miElapsedTimeColumnEnabled.Checked = Preferences.ElapsedTimeColumnEnabled;

            this.miResponseServerColumnEnabled.Click += new System.EventHandler(this.miResponseServerColumnEnabled_Click);
            this.miResponseServerColumnEnabled.Checked = Preferences.ResponseServerColumnEnabled;

            this.miExchangeTypeColumnEnabled.Click += new System.EventHandler(this.miExchangeTypeColumnEnabled_Click);
            this.miExchangeTypeColumnEnabled.Checked = Preferences.ExchangeTypeColumnEnabled;

            this.miHostIPColumnEnabled.Click += new System.EventHandler(this.miHostIPColumnEnabled_Click);
            this.miHostIPColumnEnabled.Checked = Preferences.HostIPColumnEnabled;

            this.miAuthColumnEnabled.Click += new System.EventHandler(this.miAuthColumnEnabled_Click);
            this.miAuthColumnEnabled.Checked = Preferences.AuthColumnEnabled;

            this.miAppLoggingEnabled.Click += new System.EventHandler(this.miAppLoggingEnabled_Click);
            this.miAppLoggingEnabled.Checked = Preferences.AppLoggingEnabled;

            this.miHighlightOutlookOWAOnly.Click += new System.EventHandler(this.miHighlightOutlookOWAOnly_click);
            this.miHighlightOutlookOWAOnly.Checked = Preferences.HighlightOutlookOWAOnlyEnabled;

            this.miWiki.Click += new System.EventHandler(this.miWiki_Click);

            this.miReleasesDownloadWebpage.Click += new System.EventHandler(this.miReleasesDownloadWebpage_click);

            this.miReportIssues.Click += new System.EventHandler(this.miReportIssues_Click);

            this.miCheckForUpdate.Click += new System.EventHandler(this.miCheckForUpdate_Click);
        }

        public void SetEnableAllMenuItem()
        {
            if (Preferences.ElapsedTimeColumnEnabled && Preferences.ResponseServerColumnEnabled && Preferences.ExchangeTypeColumnEnabled && Preferences.HostIPColumnEnabled)
            {
                miColumnsEnableAll.Checked = true;
            }
            else
            {
                miColumnsEnableAll.Checked = false;
            }
            // Regardless of the above, set the application preferences here for function called in OnLoad.
            //FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled", boolElapsedTimeColumnEnabled);
            //FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled", boolResponseServerColumnEnabled);
            //FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled", boolExchangeTypeColumnEnabled);
        }

        // Menu item event handlers.
        public void miEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miEnabled.Checked = !miEnabled.Checked;
            // Match boolean variable on whether extension is enabled or not.
            Preferences.ExtensionEnabled = miEnabled.Checked;
            // Set the application preference for this option.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.enabled", Preferences.ExtensionEnabled);

            TelemetryService.TrackEvent($"ExtensionIsEnabled_{miEnabled.Checked}");
        }

        // Enable/disable all columns.
        public void miColumnsEnableAll_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miColumnsEnableAll.Checked = !miColumnsEnableAll.Checked;
            miElapsedTimeColumnEnabled.Checked = miColumnsEnableAll.Checked;
            miResponseServerColumnEnabled.Checked = miColumnsEnableAll.Checked;
            miExchangeTypeColumnEnabled.Checked = miColumnsEnableAll.Checked;
            miHostIPColumnEnabled.Checked = miColumnsEnableAll.Checked;

            // Match boolean variable on menu selection.
            // Do it for all colums.
            Preferences.ColumnsAllEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ColumnsEnableAll", Preferences.ColumnsAllEnabled);
            Preferences.ElapsedTimeColumnEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled", Preferences.ElapsedTimeColumnEnabled);
            Preferences.ResponseServerColumnEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled", Preferences.ResponseServerColumnEnabled);
            Preferences.ExchangeTypeColumnEnabled = miExchangeTypeColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled", Preferences.ExchangeTypeColumnEnabled);
            Preferences.HostIPColumnEnabled = miHostIPColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.HostIPColumnEnabled", Preferences.HostIPColumnEnabled);
        }

        public void miElapsedTimeColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miElapsedTimeColumnEnabled.Checked = !miElapsedTimeColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            Preferences.ElapsedTimeColumnEnabled = miElapsedTimeColumnEnabled.Checked;
            // Set the application preference for this option.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled", Preferences.ElapsedTimeColumnEnabled);
            // Update the enable all columns UI selection based on a click here.
            SetEnableAllMenuItem();
        }

        public void miResponseServerColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miResponseServerColumnEnabled.Checked = !miResponseServerColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            Preferences.ResponseServerColumnEnabled = miResponseServerColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled", Preferences.ResponseServerColumnEnabled);
            // Update the enable all columns UI selection based on a click here.
            SetEnableAllMenuItem();
        }

        public void miExchangeTypeColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miExchangeTypeColumnEnabled.Checked = !miExchangeTypeColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            Preferences.ExchangeTypeColumnEnabled = miExchangeTypeColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled", Preferences.ExchangeTypeColumnEnabled);
            SetEnableAllMenuItem();
        }
        
        public void miHostIPColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miHostIPColumnEnabled.Checked = !miHostIPColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            Preferences.HostIPColumnEnabled = miHostIPColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.HostIPColumnEnabled", Preferences.HostIPColumnEnabled);
            SetEnableAllMenuItem();
        }

        public void miAuthColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miAuthColumnEnabled.Checked = !miAuthColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            Preferences.AuthColumnEnabled = miAuthColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.AuthColumnEnabled", Preferences.AuthColumnEnabled);
            SetEnableAllMenuItem();
        }

        public void miAppLoggingEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miAppLoggingEnabled.Checked = !miAppLoggingEnabled.Checked;
            // Match boolean variable on whether app logging is enabled or not.
            Preferences.AppLoggingEnabled = miAppLoggingEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.AppLoggingEnabled", Preferences.AppLoggingEnabled);
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

        public void miCheckForUpdate_Click(object sender, EventArgs e)
        {
            // Since the user has manually clicked this menu item to check for updates,
            // set this boolean variable to true so we can give user feedback if no update available.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ManualCheckForUpdate", true);

            // Check for app update.
            CheckForAppUpdate calledCheckForAppUpdate = new CheckForAppUpdate();
            calledCheckForAppUpdate.CheckForUpdate();
        }

        public void miHighlightOutlookOWAOnly_click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miHighlightOutlookOWAOnly.Checked = !miHighlightOutlookOWAOnly.Checked;
            // Match boolean variable on whether column is enabled or not.
            Preferences.HighlightOutlookOWAOnlyEnabled = miHighlightOutlookOWAOnly.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.HighlightOutlookOWAOnlyEnabled", Preferences.HighlightOutlookOWAOnlyEnabled);
        }

        public void OnLoad()
        {
            FirstRunEnableMenuOptions();
            InitializeMenu();
            FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExchangeOnlineTopMenu);
            this.SetEnableAllMenuItem();
        }

        /////////////////
        // Read out an application preference and if not set we know this is the first 
        // time the extension has run on this machine. Enable all options to light up functionality
        // for first time users.
        public void FirstRunEnableMenuOptions()
        {
            /////////////////
            /// <remarks>
            /// If this is the first time the extension has been run, make sure all extension options are enabled.
            /// Beyond do nothing other than keep a running count of the number of extension executions.
            /// </remarks>
            /// 
            if (FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", 0) == 0)
            {
                // Set the preferences up for the extension, enable the extension options as this is the first run.
                /// <remarks>
                /// Set the runtime variables up for this execution of Fiddler. 
                /// Also set the Fiddler Application Preferences for the next execution.
                /// </remarks>

                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.enabled", true);
                //Preferences.ExtensionEnabled = true;
                //this.miEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ColumnsEnableAll", true);
                //Preferences.bColumnsEnableAllEnabled = true;
                //this.miColumnsEnableAll.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled", true);
                //bElapsedTimeColumnEnabled = true;
                //this.miElapsedTimeColumnEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled", true);
                //bResponseServerColumnEnabled = true;
                //this.miResponseServerColumnEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled", true);
                //bExchangeTypeColumnEnabled = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.AuthColumnEnabled", true);
                //this.miExchangeTypeColumnEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.HostIPColumnEnabled", true);

                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.AppLoggingEnabled", true);
                //bAppLoggingEnabled = true;
                //this.miAppLoggingEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.HighlightOutlookOWAOnlyEnabled", true);
                //bHighlightOutlookOWAOnlyEnabled = true;
                // Increment the int iExecutionCount.
                iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", 0);
                iExecutionCount++;
                // Update the Fiddler Application Preference.
                FiddlerApplication.Prefs.SetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", iExecutionCount);
            }
            // If this isn't the first run of the extension, just increnent the running count of executions.
            else
            {
                // Increment the int iExecutionCount.
                iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", 0);
                iExecutionCount++;
                // Update the Fiddler Application Preference.
                FiddlerApplication.Prefs.SetInt32Pref("extensions.EXOFiddlerExtension.ExecutionCount", iExecutionCount);
            }
            ///
            /////////////////
        }
        //
        /////////////////

        public void OnBeforeUnload()
        {
            //throw new NotImplementedException();
        }
    }
}
