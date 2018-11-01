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
        
        public void InitializeMenu()
        {
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

            // Get the Boolean values from ColouriseWebSessions.cs.
            /*ColouriseWebSessions CWS = new ColouriseWebSessions();
            Boolean boolExtensionEnabled = CWS.GetboolEntensionEnabled();
            Boolean boolColumnsEnableAllEnabled = CWS.GetboolColumnsEnableAllEnabled();
            Boolean boolResponseTimeColumnEnabled = CWS.GetboolResponseTimeColumnEnabled();
            Boolean boolResponseServerColumnEnabled = CWS.GetboolResponseServerColumnEnabled();
            Boolean boolExchangeTypeColumnEnabled = CWS.GetboolExchangeTypeColumnEnabled();
            Boolean boolAppLoggingEnabled = CWS.GetboolAppLoggingEnabled();
            Boolean boolHighlightOutlookOWAOnlyEnabled = CWS.GetboolHighlightOutlookOWAOnlyEnabled();
            */
            Boolean boolExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
            Boolean boolResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);
            Boolean boolResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
            Boolean boolExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
            Boolean boolAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
            Boolean boolHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnly", false);
            Boolean boolColumnsEnableAllEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ColumnsEnableAllEnabled", false);
            //Boolean boolResponseTimeColumnEnabled = CWS.GetboolResponseTimeColumnEnabled();

            // Setup event handlers for menu items.
            this.miEnabled.Click += new System.EventHandler(this.miEnabled_Click);
            this.miEnabled.Checked = boolExtensionEnabled;

            this.miColumnsEnableAll.Click += new System.EventHandler(this.miColumnsEnableAll_Click);
            this.miColumnsEnableAll.Checked = boolColumnsEnableAllEnabled;

            this.miResponseTimeColumnEnabled.Click += new System.EventHandler(this.miResponseTimeColumnEnabled_Click);
            this.miResponseTimeColumnEnabled.Checked = boolResponseTimeColumnEnabled;

            this.miResponseServerColumnEnabled.Click += new System.EventHandler(this.miResponseServerColumnEnabled_Click);
            this.miResponseServerColumnEnabled.Checked = boolResponseServerColumnEnabled;

            this.miExchangeTypeColumnEnabled.Click += new System.EventHandler(this.miExchangeTypeColumnEnabled_Click);
            this.miExchangeTypeColumnEnabled.Checked = boolExchangeTypeColumnEnabled;

            this.miAppLoggingEnabled.Click += new System.EventHandler(this.miAppLoggingEnabled_Click);
            this.miAppLoggingEnabled.Checked = boolAppLoggingEnabled;

            this.miHighlightOutlookOWAOnly.Click += new System.EventHandler(this.miHighlightOutlookOWAOnly_click);
            this.miHighlightOutlookOWAOnly.Checked = boolHighlightOutlookOWAOnlyEnabled;

            this.miWiki.Click += new System.EventHandler(this.miWiki_Click);

            this.miReleasesDownloadWebpage.Click += new System.EventHandler(this.miReleasesDownloadWebpage_click);

            this.miReportIssues.Click += new System.EventHandler(this.miReportIssues_Click);

            this.miCheckForUpdate.Click += new System.EventHandler(this.miCheckForUpdate_Click);
        }

        public void SetEnableAllMenuItem()
        {
            Boolean boolExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
            Boolean boolResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);
            Boolean boolResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
            Boolean boolExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);

            if (boolResponseTimeColumnEnabled && boolResponseServerColumnEnabled && boolExchangeTypeColumnEnabled)
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
            Boolean boolExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);

            // Invert selection when this menu item is clicked.
            miEnabled.Checked = !miEnabled.Checked;
            // Match boolean variable on whether extension is enabled or not.
            boolExtensionEnabled = miEnabled.Checked;
            // Set the application preference for this option.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.enabled", boolExtensionEnabled);

            // Make sure the menu items are available / not available depending on extension status.
            // Turned off as this is a PITA.
            // EnableDisableMenuItemsAccordingToExtensionStatus();
        }

        // Enable/disable all columns.
        public void miColumnsEnableAll_Click(object sender, EventArgs e)
        {
            // Get the Boolean values from ColouriseWebSessions.cs.
            ColouriseWebSessions CWS = new ColouriseWebSessions();
            Boolean boolColumnsEnableAllEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ColumnsEnableAllEnabled", false);
            Boolean boolResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);
            Boolean boolResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
            Boolean boolExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
            
            // Invert selection when this menu item is clicked.
            miColumnsEnableAll.Checked = !miColumnsEnableAll.Checked;
            miResponseTimeColumnEnabled.Checked = miColumnsEnableAll.Checked;
            miResponseServerColumnEnabled.Checked = miColumnsEnableAll.Checked;
            miExchangeTypeColumnEnabled.Checked = miColumnsEnableAll.Checked;
            // Match boolean variable on menu selection.
            // Do it for all colums.
            boolColumnsEnableAllEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ColumnsEnableAll", boolColumnsEnableAllEnabled);
            boolResponseTimeColumnEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", boolResponseTimeColumnEnabled);
            boolResponseServerColumnEnabled = miColumnsEnableAll.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", boolResponseServerColumnEnabled);
            boolExchangeTypeColumnEnabled = miExchangeTypeColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", boolExchangeTypeColumnEnabled);
        }

        public void miResponseTimeColumnEnabled_Click(object sender, EventArgs e)
        {
            Boolean boolResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);

            // Invert selection when this menu item is clicked.
            miResponseTimeColumnEnabled.Checked = !miResponseTimeColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            boolResponseTimeColumnEnabled = miResponseTimeColumnEnabled.Checked;
            // Set the application preference for this option.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", boolResponseTimeColumnEnabled);
            // Update the enable all columns UI selection based on a click here.
            SetEnableAllMenuItem();
        }

        public void miResponseServerColumnEnabled_Click(object sender, EventArgs e)
        {
            Boolean boolResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
            
            // Invert selection when this menu item is clicked.
            miResponseServerColumnEnabled.Checked = !miResponseServerColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            boolResponseServerColumnEnabled = miResponseServerColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", boolResponseServerColumnEnabled);
            // Update the enable all columns UI selection based on a click here.
            SetEnableAllMenuItem();
        }

        public void miExchangeTypeColumnEnabled_Click(object sender, EventArgs e)
        {
            Boolean boolExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
            
            // Invert selection when this menu item is clicked.
            miExchangeTypeColumnEnabled.Checked = !miExchangeTypeColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            boolExchangeTypeColumnEnabled = miExchangeTypeColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", boolExchangeTypeColumnEnabled);
            SetEnableAllMenuItem();
        }

        public void miAppLoggingEnabled_Click(object sender, EventArgs e)
        {
            Boolean boolAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);

            // Invert selection when this menu item is clicked.
            miAppLoggingEnabled.Checked = !miAppLoggingEnabled.Checked;
            // Match boolean variable on whether app logging is enabled or not.
            boolAppLoggingEnabled = miAppLoggingEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", boolAppLoggingEnabled);
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

            // Call check for update function.
            ColouriseWebSessions CWS = new ColouriseWebSessions();
            CWS.CheckForUpdate();
        }

        public void miHighlightOutlookOWAOnly_click(object sender, EventArgs e)
        {
            Boolean boolHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);

            // Invert selection when this menu item is clicked.
            miHighlightOutlookOWAOnly.Checked = !miHighlightOutlookOWAOnly.Checked;
            // Match boolean variable on whether column is enabled or not.
            boolHighlightOutlookOWAOnlyEnabled = miHighlightOutlookOWAOnly.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnly", boolHighlightOutlookOWAOnlyEnabled);
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
            //throw new NotImplementedException();
        }

        public void OnBeforeUnload()
        {
            //throw new NotImplementedException();
        }
    }
}
