using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EXOFiddlerInspector 
{
    public class MenuUI : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
    {
        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public Boolean bResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
        public Boolean bColumnsEnableAllEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ColumnsEnableAllEnabled", false);
        
        public MenuItem ExchangeOnlineTopMenu;
        public MenuItem miColumnsMenu;

        public MenuItem miEnabled;

        public MenuItem miSeperator1;
        public MenuItem miSeperator2;
        public MenuItem miSeperator3;
        public MenuItem miSeperator4;
        public MenuItem miColumnSeperator1;

        public MenuItem miColumnsEnableAll;

        public MenuItem miResponseTimeColumnEnabled;

        public MenuItem miResponseServerColumnEnabled;

        public MenuItem miExchangeTypeColumnEnabled;

        public MenuItem miAppLoggingEnabled;

        public MenuItem miCheckForUpdate;

        public MenuItem miHighlightOutlookOWAOnly;

        public MenuItem miReleasesDownloadWebpage;

        public MenuItem miWiki;

        public MenuItem miReportIssues;
        private int iExecutionCount;

        public void InitializeMenu()
        {
            /// <remarks>
            /// Start by refreshing these variables to take into account first run code.
            /// </remarks>
            bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
            bResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);
            bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
            bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
            bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
            bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
            bColumnsEnableAllEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ColumnsEnableAllEnabled", false);

            // Setup each menu item name and ordering.
            this.ExchangeOnlineTopMenu = new MenuItem("Exchange Online");

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

            this.miResponseTimeColumnEnabled = new MenuItem("Response &Time (Load SAZ only)");
            this.miResponseTimeColumnEnabled.Index = 2;

            this.miResponseServerColumnEnabled = new MenuItem("Response &Server");
            this.miResponseServerColumnEnabled.Index = 3;

            this.miExchangeTypeColumnEnabled = new MenuItem("Exchange T&ype");
            this.miExchangeTypeColumnEnabled.Index = 4;

            this.miColumnsMenu.MenuItems.AddRange(new MenuItem[]
            {
                this.miColumnsEnableAll,
                this.miColumnSeperator1,
                this.miResponseTimeColumnEnabled,
                this.miResponseServerColumnEnabled,
                this.miExchangeTypeColumnEnabled
            });

            // Setup event handlers for menu items.
            this.miEnabled.Click += new System.EventHandler(this.miEnabled_Click);
            this.miEnabled.Checked = bExtensionEnabled;

            this.miColumnsEnableAll.Click += new System.EventHandler(this.miColumnsEnableAll_Click);
            this.miColumnsEnableAll.Checked = bColumnsEnableAllEnabled;

            this.miResponseTimeColumnEnabled.Click += new System.EventHandler(this.miResponseTimeColumnEnabled_Click);
            this.miResponseTimeColumnEnabled.Checked = bResponseTimeColumnEnabled;

            this.miResponseServerColumnEnabled.Click += new System.EventHandler(this.miResponseServerColumnEnabled_Click);
            this.miResponseServerColumnEnabled.Checked = bResponseServerColumnEnabled;

            this.miExchangeTypeColumnEnabled.Click += new System.EventHandler(this.miExchangeTypeColumnEnabled_Click);
            this.miExchangeTypeColumnEnabled.Checked = bExchangeTypeColumnEnabled;

            this.miAppLoggingEnabled.Click += new System.EventHandler(this.miAppLoggingEnabled_Click);
            this.miAppLoggingEnabled.Checked = bAppLoggingEnabled;

            this.miHighlightOutlookOWAOnly.Click += new System.EventHandler(this.miHighlightOutlookOWAOnly_click);
            this.miHighlightOutlookOWAOnly.Checked = bHighlightOutlookOWAOnlyEnabled;

            this.miWiki.Click += new System.EventHandler(this.miWiki_Click);

            this.miReleasesDownloadWebpage.Click += new System.EventHandler(this.miReleasesDownloadWebpage_click);

            this.miReportIssues.Click += new System.EventHandler(this.miReportIssues_Click);

            this.miCheckForUpdate.Click += new System.EventHandler(this.miCheckForUpdate_Click);
        }

        public void SetEnableAllMenuItem()
        {
            if (bResponseTimeColumnEnabled && bResponseServerColumnEnabled && bExchangeTypeColumnEnabled)
            {
                miColumnsEnableAll.Checked = true;
            }
            else
            {
                miColumnsEnableAll.Checked = false;
            }
            // Regardless of the above, set the application preferences here for function called in OnLoad.
            //FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", boolResponseTimeColumnEnabled);
            //FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", boolResponseServerColumnEnabled);
            //FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", boolExchangeTypeColumnEnabled);
        }

        // Menu item event handlers.
        public void miEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miEnabled.Checked = !miEnabled.Checked;
            // Match boolean variable on whether extension is enabled or not.
            bExtensionEnabled = miEnabled.Checked;
            // Set the application preference for this option.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.enabled", bExtensionEnabled);
        }

        // Enable/disable all columns.
        public void miColumnsEnableAll_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miColumnsEnableAll.Checked = !miColumnsEnableAll.Checked;
            miResponseTimeColumnEnabled.Checked = miColumnsEnableAll.Checked;
            miResponseServerColumnEnabled.Checked = miColumnsEnableAll.Checked;
            miExchangeTypeColumnEnabled.Checked = miColumnsEnableAll.Checked;

            // Match boolean variable on menu selection.
            // Do it for all colums.
            bColumnsEnableAllEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ColumnsEnableAll", bColumnsEnableAllEnabled);
            bResponseTimeColumnEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", bResponseTimeColumnEnabled);
            bResponseServerColumnEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", bResponseServerColumnEnabled);
            bExchangeTypeColumnEnabled = miExchangeTypeColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", bExchangeTypeColumnEnabled);
        }

        public void miResponseTimeColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miResponseTimeColumnEnabled.Checked = !miResponseTimeColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            bResponseTimeColumnEnabled = miResponseTimeColumnEnabled.Checked;
            // Set the application preference for this option.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", bResponseTimeColumnEnabled);
            // Update the enable all columns UI selection based on a click here.
            SetEnableAllMenuItem();
        }

        public void miResponseServerColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miResponseServerColumnEnabled.Checked = !miResponseServerColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            bResponseServerColumnEnabled = miResponseServerColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", bResponseServerColumnEnabled);
            // Update the enable all columns UI selection based on a click here.
            SetEnableAllMenuItem();
        }

        public void miExchangeTypeColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miExchangeTypeColumnEnabled.Checked = !miExchangeTypeColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            bExchangeTypeColumnEnabled = miExchangeTypeColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", bExchangeTypeColumnEnabled);
            SetEnableAllMenuItem();
        }

        public void miAppLoggingEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miAppLoggingEnabled.Checked = !miAppLoggingEnabled.Checked;
            // Match boolean variable on whether app logging is enabled or not.
            bAppLoggingEnabled = miAppLoggingEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", bAppLoggingEnabled);
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
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ManualCheckForUpdate", true);

            // Check for app update.
            CheckForAppUpdate calledCheckForAppUpdate = new CheckForAppUpdate();
            calledCheckForAppUpdate.CheckForUpdate();
        }

        public void miHighlightOutlookOWAOnly_click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miHighlightOutlookOWAOnly.Checked = !miHighlightOutlookOWAOnly.Checked;
            // Match boolean variable on whether column is enabled or not.
            bHighlightOutlookOWAOnlyEnabled = miHighlightOutlookOWAOnly.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", bHighlightOutlookOWAOnlyEnabled);
        }

        public void AutoTamperRequestBefore(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperRequestAfter(Session oSession)
        {
            // new NotImplementedException();
        }

        public void AutoTamperResponseBefore(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseAfter(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnBeforeReturningError(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnLoad()
        {

            this.FirstRunEnableMenuOptions();

            /////////////////
            /// <remarks>
            /// Initialise menu, called from MenuUI.cs.
            /// </remarks> 
            this.InitializeMenu();
            ///
            /////////////////

            /////////////////
            /// <remarks>
            /// Add the menu to the Fiddler UI.
            /// </remarks> 
            FiddlerApplication.UI.mnuMain.MenuItems.Add(this.ExchangeOnlineTopMenu);
            ///
            /////////////////




            /// <remarks>
            /// Call to function in MenuUI.cs to make sure menu items for columns are set per previous preferences.
            /// </remarks>
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
            if (FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0) == 0)
            {
                // Set the preferences up for the extension, enable the extension options as this is the first run.
                /// <remarks>
                /// Set the runtime variables up for this execution of Fiddler. 
                /// Also set the Fiddler Application Preferences for the next execution.
                /// </remarks>

                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.enabled", true);
                //bExtensionEnabled = true;
                //this.miEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ColumnsEnableAll", true);
                //bColumnsEnableAllEnabled = true;
                //this.miColumnsEnableAll.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", true);
                //bResponseTimeColumnEnabled = true;
                //this.miResponseTimeColumnEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", true);
                //bResponseServerColumnEnabled = true;
                //this.miResponseServerColumnEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", true);
                //bExchangeTypeColumnEnabled = true;
                //this.miExchangeTypeColumnEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", true);
                //bAppLoggingEnabled = true;
                //this.miAppLoggingEnabled.Checked = true;
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", true);
                //bHighlightOutlookOWAOnlyEnabled = true;
                // Increment the int iExecutionCount.
                iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);
                iExecutionCount++;
                // Update the Fiddler Application Preference.
                FiddlerApplication.Prefs.SetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", iExecutionCount);
            }
            // If this isn't the first run of the extension, just increnent the running count of executions.
            else
            {
                // Increment the int iExecutionCount.
                iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);
                iExecutionCount++;
                // Update the Fiddler Application Preference.
                FiddlerApplication.Prefs.SetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", iExecutionCount);
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
