using System;
using System.Windows.Forms;
using Fiddler;
using System.Linq;
using System.Xml;
using System.Net;
using System.Collections.Generic;

namespace EXOFiddlerInspector
{
    public class ColouriseWebSessions : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
    {
        #region MenuUI
        /////////////////
        // 
        // Setup for menu.
        //

        private MenuItem ExchangeOnlineTopMenu;

        private bool boolExtensionEnabled = false;
        /*
        // Testing use of pubboolExtensionEnabled in another class.
        public bool pubboolExtensionEnabled
        {
            get
            { return boolExtensionEnabled; }
            set
            { boolExtensionEnabled = value; }
        }
        */

        private MenuItem miEnabled;

        private bool boolResponseTimeColumnEnabled = false;
        private MenuItem miResponseTimeColumnEnabled;

        private bool boolResponseServerColumnEnabled = false;
        private MenuItem miResponseServerColumnEnabled;

        private bool boolExchangeTypeColumnEnabled = false;
        private MenuItem miExchangeTypeColumnEnabled;

        private bool boolAppLoggingEnabled = false;
        private MenuItem miAppLoggingEnabled;

        private bool boolManualCheckForUpdate = false;
        private MenuItem miCheckForUpdate;

        private bool boolHighlightOutlookOWAOnlyEnabled = false;
        private MenuItem miHighlightOutlookOWAOnly;

        private MenuItem miReleasesDownloadWebpage;

        private MenuItem miWiki;

        private MenuItem miReportIssues;

        // State Response Time column has not been created.
        private bool bResponseTimeColumnCreated = false;

        // State Response Server column has not been created.
        private bool bResponseServerColumnCreated = false;

        // State Exchange Type column has not been created.
        private bool bExchangeTypeColumnCreated = false;
        
        // Enable/disable switch for Fiddler Application Log entries from extension.
        public bool AppLoggingEnabled = true;

        private string searchTerm;
        private string RedirectAddress;
        private int HTTP200SkipLogic;
        private int HTTP200FreeBusy;

        Boolean DeveloperDemoMode = true;
        Boolean DeveloperDemoModeBreakScenarios = false;

        List<string> Developers = new List<string>(new string[] { "jeknight", "brandev", "jasonsla" });

        internal Session session { get; set; }
        public int ClientDoneResponseYear { get; private set; }

        private void InitializeMenu()
        {
            // Setup each menu item name and ordering.
            this.ExchangeOnlineTopMenu = new MenuItem("Exchange Online");

            this.miEnabled = new MenuItem("&Extension Enabled");
            this.miEnabled.Index = 0;

            this.miResponseTimeColumnEnabled = new MenuItem("Response &Time Column Enabled (Load SAZ only)");
            this.miResponseTimeColumnEnabled.Index = 1;


            this.miResponseServerColumnEnabled = new MenuItem("Response &Server Column Enabled");
            this.miResponseServerColumnEnabled.Index = 2;

            this.miExchangeTypeColumnEnabled = new MenuItem("Exchange T&ype Column Enabled");
            this.miExchangeTypeColumnEnabled.Index = 3;

            this.miAppLoggingEnabled = new MenuItem("Application &Logging Enabled");
            this.miAppLoggingEnabled.Index = 4;

            this.miHighlightOutlookOWAOnly = new MenuItem("&Highlight Outlook and OWA Only");
            this.miHighlightOutlookOWAOnly.Index = 5;

            this.miReleasesDownloadWebpage = new MenuItem("&Releases Download Page");
            this.miReleasesDownloadWebpage.Index = 6;

            this.miWiki = new MenuItem("Extension &Wiki");
            this.miWiki.Index = 7;

            this.miReportIssues = new MenuItem("&Report Issues");
            this.miReportIssues.Index = 8;

            this.miCheckForUpdate = new MenuItem("&Check For Update");
            this.miCheckForUpdate.Index = 9;

            // Add menu items to top level menu.
            this.ExchangeOnlineTopMenu.MenuItems.AddRange(new MenuItem[] { this.miEnabled,
                this.miResponseTimeColumnEnabled,
                this.miResponseServerColumnEnabled,
                this.miExchangeTypeColumnEnabled,
                this.miAppLoggingEnabled,
                this.miHighlightOutlookOWAOnly,
                this.miReleasesDownloadWebpage,
                this.miWiki,
                this.miReportIssues,
                this.miCheckForUpdate});

            // Setup event handlers for menu items.
            this.miEnabled.Click += new System.EventHandler(this.miEnabled_Click);
            this.miEnabled.Checked = boolExtensionEnabled;

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

        // Menu item event handlers.
        public void miEnabled_Click(object sender, EventArgs e)
        {
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

        public void miResponseTimeColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miResponseTimeColumnEnabled.Checked = !miResponseTimeColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            boolResponseTimeColumnEnabled = miResponseTimeColumnEnabled.Checked;
            // Set the application preference for this option.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", boolResponseTimeColumnEnabled);
        }

        public void miResponseServerColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miResponseServerColumnEnabled.Checked = !miResponseServerColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            boolResponseServerColumnEnabled = miResponseServerColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", boolResponseServerColumnEnabled);
        }

        public void miExchangeTypeColumnEnabled_Click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miExchangeTypeColumnEnabled.Checked = !miExchangeTypeColumnEnabled.Checked;
            // Match boolean variable on whether column is enabled or not.
            boolExchangeTypeColumnEnabled = miExchangeTypeColumnEnabled.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", boolExchangeTypeColumnEnabled);
        }

        public void miAppLoggingEnabled_Click(object sender, EventArgs e)
        {
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
            boolManualCheckForUpdate = true;
            // Call check for update function.
            CheckForUpdate();
        }

        public void miHighlightOutlookOWAOnly_click(object sender, EventArgs e)
        {
            // Invert selection when this menu item is clicked.
            miHighlightOutlookOWAOnly.Checked = !miHighlightOutlookOWAOnly.Checked;
            // Match boolean variable on whether column is enabled or not.
            boolHighlightOutlookOWAOnlyEnabled = miHighlightOutlookOWAOnly.Checked;
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnly", boolHighlightOutlookOWAOnlyEnabled);
        }

        /*
        public void EnableDisableMenuItemsAccordingToExtensionStatus()
        {
            // Enable / disable menu items according to extension enabled.
            if (boolExtensionEnabled)
            {
                this.miResponseTimeColumnEnabled.Enabled = true;
                this.miResponseServerColumnEnabled.Enabled = true;
                this.miExchangeTypeColumnEnabled.Enabled = true;
                this.miAppLoggingEnabled.Enabled = true;
            }
            else
            {
                this.miResponseTimeColumnEnabled.Enabled = false;
                this.miResponseServerColumnEnabled.Enabled = false;
                this.miExchangeTypeColumnEnabled.Enabled = false;
                this.miAppLoggingEnabled.Enabled = false;
            }
        }
        */
        //
        /////////////////
        #endregion

        #region OnLoad
        /////////////////
        //
        // OnLoad
        //
        public void OnLoad()
        {
            /////////////////
            //
            // Set demo mode. If enabled as much domain specific information as possible will be replaced with contoso.com.
            // Ensure this is disabled before build and deploy!!!
            //

            //
            /////////////////
            //
            // Make sure that even if these are mistakenly left on from debugging, production users are not impacted.
            //if (Environment.UserName == "jeknight" || Environment.UserName == "brandev" && DemoMode == true)
            if (Developers.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.DemoMode", true);
            }
            //else if (Environment.UserName == "jeknight" || Environment.UserName == "brandev" && DemoMode == false)
            else if (Developers.Any(Environment.UserName.Contains) && DeveloperDemoMode == false)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.DemoMode", false);
            }

            // Make sure that even if these are mistakenly left on from debugging, production users are not impacted.
            //if (Environment.UserName == "jeknight" || Environment.UserName == "brandev" && DemoModeBreakScenarios == true)
            if (Developers.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == true)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.DemoModeBreakScenarios", true);
            }
            //else if (Environment.UserName == "jeknight" || Environment.UserName == "brandev" && DemoMode == false)
            else if (Developers.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == false)
            {
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.DemoModeBreakScenarios", false);
            }
            //
            /////////////////
            //

            // Throw a message box to alert demo mode is running.
            if (Developers.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
            {
                MessageBox.Show("Developer / Demo mode is running!");
            }
            //
            /////////////////

            // If the FirstRun application preference is set to false, then the extension has previously run.
            // The function FirstRunEnableMenuOptions sets the FirstRun app preference to false.
            // If the above ... then collect the column preferences off of last preferences set.
            // The below logic check does not work for new installations. Needs a fix.
            if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.FirstRun", false) == false) {
                this.boolExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
                this.boolResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);
                this.boolResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
                this.boolExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
                this.boolAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
                this.boolHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnly", false);
            }
            // If the FirstRun application preference is not set, then go run the FirstRunEnableMenuOptions function to light up features for first use.
            else
            {
                FirstRunEnableMenuOptions();
            }

            // Response Time column function is no longer called here. Only in OnLoadSAZ.

            // Call the function to add column if the menu item is checked and if the extension is enabled.
            if (boolResponseServerColumnEnabled && boolExtensionEnabled)
            {
                EnsureResponseServerColumn();
            }

            // Call the function to add column if the menu item is checked and if the extension is enabled.
            if (boolExchangeTypeColumnEnabled && boolExtensionEnabled)
            {
                EnsureExchangeTypeColumn();
            }
            
            // Initialise menu.
            InitializeMenu();
            // Add the menu.
            FiddlerApplication.UI.mnuMain.MenuItems.Add(ExchangeOnlineTopMenu);

            // Make sure the menu items are available / not available depending on extension status.
            // Turned off as this is a PITA.
            // EnableDisableMenuItemsAccordingToExtensionStatus();

            // Call function to process sessions only if the extension is enabled.
            if (boolExtensionEnabled)
            {
                FiddlerApplication.OnLoadSAZ += HandleLoadSaz;
            }
        }
        //
        /////////////////
        #endregion

        #region FirstRunMenuOptions
        /////////////////
        // Read out an application preference and if not set we know this is the first 
        // time the extension has run on this machine. Enable all options to light up functionality
        // for first time users.
        public void FirstRunEnableMenuOptions()
        {
            // FirstRun will be null on first run. Thereafter it will be set to false.
            // Light up functionality for first run.
            this.boolExtensionEnabled = true;
            this.boolResponseTimeColumnEnabled = true;
            this.boolResponseServerColumnEnabled = true;
            this.boolExchangeTypeColumnEnabled = true;

            // Set this app preference as false so we don't execute the above after first run.
            FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.FirstRun", false);
        }
        //
        /////////////////
        #endregion

        #region EnsureColumns
        /////////////////
        //
        // Make sure the Columns are added to the UI as enabled and if Extension is enabled.
        //
        public void EnsureResponseTimeColumn()
        {
            /////////////////
            // Response Time column.
            //
            // If the column is already created exit.
            if (bResponseTimeColumnCreated) return;

            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Time", 2, 110, "X-iTTLB");
            bResponseTimeColumnCreated = true;
            //
            /////////////////
        }

        public void EnsureResponseServerColumn()
        {
            /////////////////
            // Response Server column.
            //
            // If the column is already created exit.
            if (bResponseServerColumnCreated) return;
            
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 2, 130, "X-ResponseServer");
            bResponseServerColumnCreated = true;
            //
            /////////////////
        }

        public void EnsureExchangeTypeColumn()
        {
            /////////////////
            // Response Server column.
            //
            // If the column is already created exit.
            if (bExchangeTypeColumnCreated) return;

            FiddlerApplication.UI.lvSessions.AddBoundColumn("Exchange Type", 2, 150, "X-ExchangeType");
            bExchangeTypeColumnCreated = true;
            //
            /////////////////
        }
        //
        /////////////////
        #endregion

        #region LoadSAZ
        /////////////////
        // 
        // Handle loading a SAZ file.
        //
        private void HandleLoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
        {
            // At this point in time only checking for updates when SAZ file is loaded.
            // Doing this on a live trace is problematic and has hung Fiddler in my testing.
            // Only do this if the extension is enabled.
            if (boolExtensionEnabled)
            {
                // Only check for updates on LoadSAZ if the extension is enabled.
                CheckForUpdate();
            }

            // Call the function to add column if the menu item is checked and if the extension is enabled.
            if (boolResponseTimeColumnEnabled && boolExtensionEnabled)
            {
                EnsureResponseTimeColumn();
            }

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            foreach (var session in e.arrSessions)
            {
                // Populate the ResponseTime column on load SAZ, if the column is enabled, and the extension is enabled.
                if (boolResponseTimeColumnEnabled && boolExtensionEnabled)
                {
                    //session["X-iTTLB"] = session.oResponse.iTTLB.ToString() + "ms";
                    session["X-iTTLB"] = Math.Round((session.Timers.ClientDoneResponse - session.Timers.ClientBeginRequest).TotalMilliseconds) + "ms";
                }

                // Populate the ExchangeType column on load SAZ, if the column is enabled, and the extension is enabled
                if (boolExchangeTypeColumnEnabled && boolExtensionEnabled)
                {
                    SetExchangeType(session);
                }

                // Populate the ResponseServer column on load SAZ, if the column is enabled, and the extension is enabled
                if (boolResponseServerColumnEnabled && boolExtensionEnabled)
                {
                    SetResponseServer(session);
                }

                // Colourise sessions on load SAZ.
                if (boolExtensionEnabled)
                {
                    OnPeekAtResponseHeaders(session); //Run whatever function you use in IAutoTamper
                    session.RefreshUI();
                }
            }
            FiddlerApplication.UI.lvSessions.EndUpdate();
        }
        //
        /////////////////
        #endregion

        #region CheckForUpdates
        /////////////////
        //
        // Check for updates.
        //
        // Called from Onload. Not currently implemented, due to web call issue, as Fiddler substitutes in http://localhost:8888 as the proxy server.
        //
        public void CheckForUpdate()
        {
            // If the Fiddler application is attached as the system proxy and an update check is triggered via menu.
            // First Try detach.
            //FiddlerApplication.oProxy.Detach();
            // Wait some time.
            //System.Threading.Thread.Sleep(5000);
            // Continue processing.
            // FiddlerApplication.OnDetach??

            string downloadUrl = "";
            Version newVersion = null;
            string xmlUrl = Properties.Settings.Default.UpdateURL;

            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(xmlUrl);
                reader.MoveToContent();
                string elementName = "";
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name == "EXOFiddlerInspector")
                {
                    while (reader.Read())
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            elementName = reader.Name;
                        }
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) && reader.HasValue)
                            {
                                switch (elementName)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;
                                    case "url":
                                        downloadUrl = reader.Value;
                                        break;
                                }
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (applicationVersion.CompareTo(newVersion) < 0)
            {
                // Setup message box options.
                string message = "You are currently using v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + "." + Environment.NewLine +
                    "A new version is available v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + Environment.NewLine +
                    "Do you want to download the update?";

                string caption = "EXO Fiddler Extension - Update Available";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Display the MessageBox.
                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    // Execute the installer MSI URL, which will open in the user's default browser.
                    System.Diagnostics.Process.Start(Properties.Settings.Default.InstallerURL);
                    if (boolAppLoggingEnabled && boolExtensionEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: Version installed. v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + ".");
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: New Version Available. v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + ".");
                    }
                }
            }
            else
            {
                if (boolAppLoggingEnabled && boolExtensionEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: Latest version installed. v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ".");
                }
                // Regardless of extension enabled or not, give the user feedback when they click the 'Check For Update' menu item if no update is available.
                if (boolManualCheckForUpdate)
                {
                    MessageBox.Show("EXOFiddlerExtention: Latest version installed. v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ".", "EXO Fiddler Extension");
                    // return this value back to false, so we don't give this feedback unintentionally.
                    boolManualCheckForUpdate = false;
                }
            }
        }
        //
        // Check for updates end.
        //
        /////////////////

        #endregion

        #region ColouriseRuleSet

        /////////////////////////////
        //
        // Function where all session colourisation happens.
        //
        private void OnPeekAtResponseHeaders(Session session)
        {
            // Reset these session counters.
            HTTP200SkipLogic = 0;
            HTTP200FreeBusy = 0;

            this.session = session;

            // Colour codes for sessions. Softer tones, easier on the eye than standard red, orange and green.
            string HTMLColourBlue = "#81BEF7";
            string HTMLColourGreen = "#81f7ba";
            // Previous red and orange values too similar when not shown in the same trace.
            //string HTMLColourRed = "#f78f81";
            string HTMLColourRed = "#f06141";
            string HTMLColourGrey = "#BDBDBD";
            // Previous red and orange values too similar when not shown in the same trace.
            //string HTMLColourOrange = "#f7ac81";
            string HTMLColourOrange = "#f59758";

            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            int wordCount = 0;
            int wordCountError = 0;
            int wordCountFailed = 0;
            int wordCountException = 0;

            #region ColouriseSessionsSwitchStatement
            /////////////////////////////
            //
            //  Broader code logic for sessions, where the response code cannot be used as in the switch statement.
            //

            /////////////////////////////
            //
            // From a scenario where Apache Web Server found to be answering Autodiscover calls and throwing HTTP 301 & 405 responses.
            //
            if ((this.session.url.Contains("autodiscover") && (this.session.oResponse["server"] == "Apache")))
            {
                this.session["ui-backcolor"] = HTMLColourRed;
                this.session["ui-color"] = "black";
                if (boolAppLoggingEnabled && boolExtensionEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 405 Method Not Allowed; Apache is answering Autodiscover requests!");
                }
            }
            // If the above is not true, then drop into the switch statement based on individual response codes.
            else
            {
                /////////////////////////////
                //
                // Response code logic.
                //
                switch (this.session.responseCode)
                {
                    #region HTTP0
                    case 0:
                        /////////////////////////////
                        //
                        //  HTTP 0: No Response.
                        //
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-ExchangeType"] = "NO RESPONSE!";
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region HTTP200s
                    case 200:
                        /////////////////////////////
                        //
                        // HTTP 200
                        //

                        /////////////////////////////
                        // 1. Connect Tunnel.
                        if (this.session.isTunnel == true)
                        {
                            // Skip 99 response body word split and keyword search with Linq code.
                            // Mark as green, not expecting to find anything noteworthy in these sessions.
                            this.session["ui-backcolor"] = HTMLColourGreen;
                            this.session["ui-color"] = "black";
                            HTTP200SkipLogic++;
                        }

                        /////////////////////////////
                        // 2. Exchange On-Premise Autodiscover redirect.
                        if (this.session.utilFindInResponse("<Action>redirectAddr</Action>", false) > 1)
                        {
                            /*
                            <?xml version="1.0" encoding="utf-8"?>
                            <Autodiscover xmlns="http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006">
                              <Response xmlns="http://schemas.microsoft.com/exchange/autodiscover/outlook/responseschema/2006a">
                                <Account>
                                  <Action>redirectAddr</Action>
                                  <RedirectAddr>user@contoso.mail.onmicrosoft.com</RedirectAddr>       
                                </Account>
                              </Response>
                            </Autodiscover>
                            */

                            // Logic to detected the redirect address in this session.
                            // 
                            string RedirectResponseBody = this.session.GetResponseBodyAsString();
                            int start = this.session.GetResponseBodyAsString().IndexOf("<RedirectAddr>");
                            int end = this.session.GetResponseBodyAsString().IndexOf("</RedirectAddr>");
                            int charcount = end - start;

                            if (Developers.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
                            {
                                // If as well as being in demo mode, demo mode break scenarios is enabled. Show fault through incorrect direct
                                // address for an Exchange Online mailbox.
                                if (Developers.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == true)
                                {
                                    RedirectAddress = "user@contoso.com";
                                }
                                else
                                {
                                    RedirectAddress = "user@contoso.mail.onmicrosoft.com";
                                }
                            }
                            else
                            {
                                // If demo mode is not running, set RedirectAddress detected from the session.
                                RedirectAddress = RedirectResponseBody.Substring(start, charcount).Replace("<RedirectAddr>", "");
                            }

                            if (RedirectAddress.Contains(".onmicrosoft.com"))
                            {
                                this.session["ui-backcolor"] = HTMLColourGreen;
                                this.session["ui-color"] = "black";
                                this.session["X-ExchangeType"] = "On-Prem AutoD Redirect";
                                HTTP200SkipLogic++;
                                if (boolAppLoggingEnabled && boolExtensionEnabled)
                                {
                                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 Exchange On-Premise redirect address: " + RedirectAddress);
                                }
                            }
                            // Highlight if we got this far and do not have a redirect address which points to
                            // Exchange Online such as: contoso.mail.onmicrosoft.com.
                            else
                            {
                                this.session["ui-backcolor"] = HTMLColourRed;
                                this.session["ui-color"] = "black";
                                this.session["X-ExchangeType"] = "AUTOD REDIRECT ADDR!";
                                // Increment HTTP200SkipLogic so that 99 does not run below.
                                HTTP200SkipLogic++;
                                if (boolAppLoggingEnabled && boolExtensionEnabled)
                                {
                                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 Exchange On-Premise AUTOD REDIRECT ADDR! : " + RedirectAddress);
                                }
                            }
                        }

                        /////////////////////////////
                        //
                        // 3. Exchange On-Premise Autodiscover redirect - address can't be found
                        //
                        if ((this.session.utilFindInResponse("<Message>The email address can't be found.</Message>", false) > 1) &&
                            (this.session.utilFindInResponse("<ErrorCode>500</ErrorCode>", false) > 1))
                        {
                            /*
                            <?xml version="1.0" encoding="utf-8"?>
                            <Autodiscover xmlns="http://schemas.microsoft.com/exchange/autodiscover/responseschema/2006">
                              <Response>
                                <Error Time="12:03:32.8803744" Id="2422600485">
                                  <ErrorCode>500</ErrorCode>
                                  <Message>The email address can't be found.</Message>
                                  <DebugData />
                                </Error>
                              </Response>
                            </Autodiscover>
                            */
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "NO AUTOD REDIRECT ADDR!";
                            // Increment HTTP200SkipLogic so that 99 does not run below.
                            HTTP200SkipLogic++;
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 Exchange On-Premise redirect address. Error code 500: The email address can't be found.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 4. Exchange Online Autodiscover
                        //

                        // Make sure this session if an Exchange Online Autodiscover request.
                        if ((this.session.hostname == "autodiscover-s.outlook.com") && (this.session.uriContains("autodiscover.xml"))) {
                            if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) && 
                                (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) && 
                                (this.session.utilFindInResponse("<MailStore>", false) > 1) && 
                                (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                            {
                                this.session["ui-backcolor"] = HTMLColourGreen;
                                this.session["ui-color"] = "black";
                                // Increment HTTP200SkipLogic so that 99 does not run below.
                                HTTP200SkipLogic++;
                            }
                            // If we got this far and those strings do not exist in the response body something is wrong.
                            else
                            {
                                this.session["ui-backcolor"] = HTMLColourGreen;
                                this.session["ui-color"] = "black";
                                // Don't use skip logic here, we want to dig deeper and see if there are errors, failures, or exceptions.
                                //HTTP200SkipLogic++;

                            }
                        }

                        /////////////////////////////
                        //
                        // 5. Outlook MAPI traffic.
                        //
                        if (this.session.HostnameIs("outlook.office365.com") && (this.session.uriContains("/mapi/emsmdb/?MailboxId=")))
                        {
                            this.session["ui-backcolor"] = HTMLColourGreen;
                            this.session["ui-color"] = "black";
                            // Increment HTTP200SkipLogic so that 99 does not run below.
                            HTTP200SkipLogic++;
                        }

                        /////////////////////////////
                        //
                        // 6. GetUnifiedGroupsSettings EWS call.
                        //
                        if (this.session.HostnameIs("outlook.office365.com") &&
                            (this.session.uriContains("ews/exchange.asmx") &&
                            (this.session.utilFindInRequest("GetUnifiedGroupsSettings", false) > 1)))
                        {
                            // User can create Office 365 gropus.
                            if (this.session.utilFindInResponse("<GroupCreationEnabled>true</GroupCreationEnabled>", false) > 1)
                            {
                                this.session["ui-backcolor"] = HTMLColourGreen;
                                this.session["ui-color"] = "black";
                                this.session["X-ExchangeType"] = "EWS GetUnifiedGroupsSettings";
                                HTTP200SkipLogic++;
                            }
                            // User cannot create Office 365 groups. Not an error condition in and of itself.
                            else if (this.session.utilFindInResponse("<GroupCreationEnabled>false</GroupCreationEnabled>", false) > 1)
                            {
                                this.session["ui-backcolor"] = HTMLColourGreen;
                                this.session["ui-color"] = "black";
                                this.session["X-ExchangeType"] = "EWS GetUnifiedGroupsSettings";
                                HTTP200SkipLogic++;
                            }
                            // Did not see the expected keyword in the response body. This is the error condition.
                            else
                            {
                                this.session["ui-backcolor"] = HTMLColourRed;
                                this.session["ui-color"] = "black";
                                this.session["X-ExchangeType"] = "EWS GetUnifiedGroupsSettings";
                                // Do not do HTTP200SkipLogic here, expected response not found. Run keyword search on response for deeper inpsection of response.
                                // HTTP200SkipLogic++;
                            }
                        }
                        
                        // Exchange On-Premise redirect to Exchange Online Autodiscover.
                        // 7.Location: https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml

                        /////////////////////////////
                        //
                        // 99. All other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.
                        else
                        {
                            // Only fire the Linq response body word split and keyword search if:
                            // HTTP200SkipLogic has not been incremented above = Session has been classified as something else and this is not necessary.
                            // OR...
                            // HTTP200FreeBusy is greater than zero = Session is marked as Free/Busy and we want deep inspection for errors, failed or exception keywords.
                            if (HTTP200SkipLogic == 0 || HTTP200FreeBusy > 0)
                            {

                                // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
                                //
                                // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
                                //

                                string text200 = this.session.ToString();

                                //Convert the string into an array of words  
                                string[] source200 = text200.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

                                // Create the query. Use ToLowerInvariant to match "data" and "Data"   
                                var matchQuery200 = from word in source200
                                                    where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                                    select word;

                                searchTerm = "Error";

                                // Count the matches, which executes the query.  
                                wordCountError = matchQuery200.Count();

                                searchTerm = "failed";

                                // Count the matches, which executes the query.  
                                wordCountFailed = matchQuery200.Count();

                                searchTerm = "exception";

                                // Count the matches, which executes the query.  
                                wordCountException = matchQuery200.Count();

                                // If either the keyword searches give us a result.
                                if (wordCountError > 0 || wordCountFailed > 0 || wordCountException > 0)
                                {
                                    // Special attention to HTTP 200's where the keyword 'error' or 'failed' is found.
                                    // Red text on black background.
                                    this.session["ui-backcolor"] = "black";
                                    this.session["ui-color"] = "red";
                                    this.session["X-ExchangeType"] = "FAILURE LURKING!";
                                }
                                else
                                {
                                    // All good.
                                    this.session["ui-backcolor"] = HTMLColourGreen;
                                    this.session["ui-color"] = "black";
                                }
                            }
                            // HTTP200SkipLogic is >= 1 or HTTP200FreeBusy is 0.
                            else
                            {
                                // Since we use HTTP200SkipLogic and skipped the code above to split words and search for keywords, and we have also not detected any other conditions
                                // mark the remaining sessions as yellow, not detected.
                                if (string.IsNullOrEmpty(this.session["UI-BACKCOLOR"]) && string.IsNullOrEmpty(this.session["UI-COLOR"])) {
                                    this.session["ui-backcolor"] = "Yellow";
                                    this.session["ui-color"] = "black";
                                }
                            }
                        }
                        //
                        /////////////////////////////
                        break;
                    case 201:
                        /////////////////////////////
                        //
                        //  HTTP 201: Created.
                        //
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    case 204:
                        /////////////////////////////
                        //
                        //  HTTP 204: No Content.
                        //
                        // Somewhat highlight these.
                        this.session["ui-backcolor"] = "Yellow";
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region HTTP300s
                    case 301:
                        /////////////////////////////
                        //
                        //  HTTP 301: Moved Permanently.
                        //
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    case 302:
                        /////////////////////////////
                        //
                        //  HTTP 302: Found / Redirect.
                        //            
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    case 304:
                        /////////////////////////////
                        //
                        //  HTTP 304: Not modified.
                        //
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    case 307:
                        /////////////////////////////
                        //
                        //  HTTP 307: Temporary Redirect.
                        //

                        // Specific scenario where a HTTP 307 Temporary Redirect incorrectly send an EXO Autodiscover request to an On-Premise resource, breaking Outlook connectivity.
                        if (this.session.hostname.Contains("autodiscover") &&
                            (this.session.hostname.Contains("mail.onmicrosoft.com") &&
                            (this.session.fullUrl.Contains("autodiscover") &&
                            (this.session.ResponseHeaders["Location"] != "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml"))))
                        {
                            // Redirect location has been found to send the Autodiscover connection somewhere else other than'
                            // Exchange Online, highlight.
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "UNEXPECTED LOCATION!";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 307 On-Prem Temp Redirect - Unexpected location!");
                            }
                        }
                        else
                        {
                            // The above scenario is not seem, however Temporary Redirects are not exactly normally expected to be seen.
                            // Highlight as a warning.
                            this.session["ui-backcolor"] = HTMLColourOrange;
                            this.session["ui-color"] = "black";
                        }
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region HTTP400s
                    case 401:

                        /////////////////////////////
                        //
                        //  HTTP 401: UNAUTHORIZED.
                        //
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";
                        this.session["X-ExchangeType"] = "Auth Challenge";
                        //
                        /////////////////////////////
                        break;
                    case 403:
                        /////////////////////////////
                        //
                        //  HTTP 403: FORBIDDEN.
                        //
                        // Looking for the term "Access Denied" works fine using utilFindInResponse.
                        // Specific scenario where a web proxy is blocking traffic.
                        if (this.session.utilFindInResponse("Access Denied", false) > 1)
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "WEB PROXY BLOCK!";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");
                            }
                        }
                        else
                        {
                            // Potentially nothing to worry about. Not marking in log.
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                        }
                        //
                        /////////////////////////////
                        break;
                    case 404:
                        /////////////////////////////
                        //
                        //  HTTP 404: Not Found.
                        //
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    case 405:
                        /////////////////////////////
                        //
                        //  HTTP 405: Method Not Allowed.
                        //
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    case 429:
                        /////////////////////////////
                        //
                        //  HTTP 429: Too Many Requests.
                        //
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    case 440:
                        /////////////////////////////
                        //
                        // HTTP 440: Need to know more about these.
                        // For the moment do nothing.
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region HTTP500s
                    case 500:
                        /////////////////////////////
                        //
                        //  HTTP 500: Internal Server Error.
                        //
                        // Pick up any 500 Internal Server Error and write data into the comments box.
                        // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in green? >
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        if (boolAppLoggingEnabled && boolExtensionEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 500 Internal Server Error.");
                        }
                        //
                        /////////////////////////////
                        break;
                    case 502:
                        /////////////////////////////
                        //
                        //  HTTP 502: BAD GATEWAY.
                        //


                        // Specific scenario on Outlook & OFffice 365 Autodiscover false positive on connections to:
                        //      autodiscover.domain.onmicrosoft.com:443

                        // Testing because I am finding colourisation based in the nested if statement below is not working.
                        // Strangely the same HTTP 502 nested if statement logic works fine in EXOFiddlerInspector.cs to write
                        // response alert and comment.
                        // From further testing this seems to come down to timing, clicking the sessions as they come into Fiddler
                        // I see the responsecode / response body unavailable, it then populates after a few sessions. I presume 
                        // since the UI has moved on already the session cannot be colourised. 

                        // On testing with loadSAZ instead this same code colourises sessions fine.

                        // Altered if statements from being bested to using && to see if this inproves here.
                        // This appears to be the only section in this code which has a session colourisation issue.

                        /////////////////////////////
                        //
                        // 1. telemetry false positive. <Need to validate in working scenarios>
                        //
                        if ((this.session.oRequest["Host"] == "sqm.telemetry.microsoft.com:443") &&
                            (this.session.utilFindInResponse("target machine actively refused it", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourBlue;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "False Positive";
                        }

                        /////////////////////////////
                        //
                        // 2. Exchange Online DNS lookup on contoso.onmicrosoft.com, False Positive!?
                        //
                        // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
                        else if ((this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                                (this.session.utilFindInResponse("DNS Lookup for ", false) > 1) &&
                                (this.session.utilFindInResponse(" failed.", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourBlue;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "False Positive";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway - False Positive.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 3. Exchange Online connection to autodiscover.contoso.mail.onmicrosoft.com, False Positive!
                        //
                        // Specific scenario on Outlook and Office 365 invalid connection to contoso.mail.onmicrosoft.com
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
                        else if ((this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                            // Too specific, it looks as though we see ConnectionRefused or The socket connection to ... failed.
                            //(this.session.utilFindInResponse("ConnectionRefused ", false) > 1) &&
                            (this.session.utilFindInResponse("target machine actively refused it", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourBlue;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "False Positive";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway - False Positive.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 4. Anything else Exchange Autodiscover.
                        //
                        else if ((this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                            (this.session.utilFindInResponse("autodiscover", false) > 1) &&
                            (this.session.utilFindInResponse(":443", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "AUTODISCOVER!";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 99. Everything else.
                        //
                        else
                        {
                            // Pick up any other 502 Bad Gateway call it out.
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway.");
                            }
                        }
                        //
                        /////////////////////////////
                        break;
                    case 503:
                        /////////////////////////////
                        //
                        //  HTTP 503: SERVICE UNAVAILABLE.
                        //
                        // Call out all 503 Service Unavailable as something to focus on.
                        searchTerm = "FederatedStsUnreachable";
                        //"Service Unavailable"

                        // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
                        //
                        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
                        //

                        string text503 = this.session.ToString();

                        //Convert the string into an array of words  
                        string[] source503 = text503.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // Create the query. Use ToLowerInvariant to match "data" and "Data"   
                        var matchQuery503 = from word in source503
                                            where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                            select word;

                        // Count the matches, which executes the query.  
                        wordCount = matchQuery503.Count();
                        if (wordCount > 0)
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "FEDERATION!";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 503 Service Unavailable. Found keyword 'FederatedStsUnreachable' in response body!");
                            }
                        }
                        else
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 503 Service Unavailable.");
                            }
                        }
                        //
                        /////////////////////////////
                        break;
                    case 504:
                        /////////////////////////////
                        //
                        //  HTTP 504: GATEWAY TIMEOUT.
                        //
                        // Call out all 504 Gateway Timeout as something to focus on.
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        if (boolAppLoggingEnabled && boolExtensionEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 504 Gateway Timeout.");
                        }


                        /////////////////////////////
                        // 1. HTTP 504 Bad Gateway 'internet has been blocked'
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-ExchangeType"] = "INTERNET BLOCKED!";
                        if (boolAppLoggingEnabled && boolExtensionEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + "  HTTP 504 Gateway Timeout -- Internet Access Blocked.");
                        }
                        /////////////////////////////
                        // 99. Pick up any other 504 Gateway Timeout and write data into the comments box.
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        if (boolAppLoggingEnabled && boolExtensionEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 504 Gateway Timeout.");
                        }
                        //
                        /////////////////////////////
                        //
                        /////////////////////////////
                        break;
                    #endregion

                    #region Default
                        /////////////////////////////
                        // Fallen into default, so undefined in the extension.
                        // Mark the session as such.
                    default:
                        this.session["ui-backcolor"] = "Yellow";
                        this.session["ui-color"] = "black";
                        this.session["X-ExchangeType"] = "Undefined";
                        break;
                        //
                        /////////////////////////////
                        #endregion
                }
                //
                /////////////////////////////
            }
            #endregion
            //
            /////////////////////////////

            /////////////////////////////
            //
            #region ColouriseSessionsOverrides
            // First off if the local process is nullor blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS
            // So don't pay any attention to overrides for this type of traffic.
            if (this.session.hostname == "www.fiddler2.com")
            {
                this.session["ui-backcolor"] = HTMLColourGrey;
                this.session["ui-color"] = "black";
                this.session["X-ExchangeType"] = "Not Exchange";
            }
            else if ((this.session.LocalProcess == null) || (this.session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                // No overrides needed in this scenario.
            }
            else
            {
                // If the menu item Highlight Outlook and OWA Only is enabled then grey out all the other traffic.
                if (boolHighlightOutlookOWAOnlyEnabled == true)
                {
                    // With that out of the way,  if the traffic is not related to any of the below processes, then mark it as grey to
                    // de-emphasise it.
                    // So if for example lync.exe is the process de-emphasise the traffic with grey.
                    if (!(this.session.LocalProcess.Contains("outlook") ||
                        this.session.LocalProcess.Contains("searchprotocolhost") ||
                        this.session.LocalProcess.Contains("iexplore") ||
                        this.session.LocalProcess.Contains("chrome") ||
                        this.session.LocalProcess.Contains("firefox") ||
                        this.session.LocalProcess.Contains("edge") ||
                        this.session.LocalProcess.Contains("w3wp")))
                    {
                        // Everything which is not detected as related to Exchange, Outlook or OWA in some way.
                        this.session["ui-backcolor"] = HTMLColourGrey;
                        this.session["ui-color"] = "black";
                        this.session["X-ExchangeType"] = "Not Exchange";
                    }
                }
            }
            #endregion
            //
            /////////////////////////////
        }
        //
        /////////////////////////////
        #endregion

        public void OnBeforeUnload() { }

        public void OnPeekAtResponseHeaders(IAutoTamper2 AllSessions) { }

        public void AutoTamperRequestBefore(Session oSession) { }

        public void AutoTamperRequestAfter(Session oSession) { }

        public void AutoTamperResponseBefore(Session oSession) { }

        /////////////////////////////
        //
        // Function where live tracing is processed.
        //
        public void AutoTamperResponseAfter(Session session)
        {

            this.session = session;

            /////////////////
            //
            // Call the function to populate the session type column on live trace, if the column is enabled.
            if (boolExchangeTypeColumnEnabled && boolExtensionEnabled)
            {
                SetExchangeType(session);
            }

            /////////////////
            //
            // Call the function to populate the session type column on live trace, if the column is enabled.
            if (boolResponseServerColumnEnabled && boolExtensionEnabled)
            {
                SetResponseServer(session);
            }

            /////////////////
            //
            // Call the function to colourise sessions for live traffic capture.
            //
            // Making sure this is called after SetExchangeType and SetResponseServer, so we can use overrides
            // in OnPeekAtResponseHeaders function.
            //
            if (boolExtensionEnabled)
            {
                OnPeekAtResponseHeaders(session);
                session.RefreshUI();
            }
            //
            /////////////////

            /////////////////
            //
            // For some reason setting the column ordering when adding the columns did not work.
            // Adding the ordering here instead does work.
            // For column ordering to work on disabe/enable it seems neccessary to set ordering here
            // in reverse order for my preference on column order as I want each to be set to priority 2
            // so that other standard columns do not get put into the Exchange Online column grouping.

            //FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("#", 0, -1);
            //FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Result", 1, -1);

            // These get called on each session, seen strange behaviour on reordering on live trace due 
            // to setting each of these as ordering 2 to ensure column positions regardless of column enabled selections.
            // Use an if statement to fire these once per Fiddler application session.
            if (this.session.id == 1)
            {
                if (boolExtensionEnabled)
                {
                    // Move the process column further to the left.
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 2, 100);
                }
                else
                {
                    // Since the extension is not enabled return the process column back to its original location.
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 8, -1);
                }
                if (boolExchangeTypeColumnEnabled && boolExtensionEnabled)
                {
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Exchange Type", 2, -1);
                }

                if (boolResponseServerColumnEnabled && boolExtensionEnabled)
                {
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Server", 2, -1);
                }

                if (boolResponseTimeColumnEnabled && boolExtensionEnabled)
                {
                    FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Time", 2, -1);
                }
            }

            /*
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Protocol", 5, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host", 6, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("URL", 7, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Body", 8, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Caching", 9, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Content-Type", 10, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Comments", 12, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Custom", 13, -1);
            */
            //
            /////////////////

            //
            // Populate the ResponseTime column on live trace, if the column is enabled.
            if (boolResponseTimeColumnEnabled && boolExtensionEnabled) {
                // Realised this.session.oResponse.iTTLB.ToString() + "ms" is not the value I want to display as Response Time.
                // More desirable figure is created from:
                // Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds)
                // 
                // For some reason in AutoTamperResponseAfter this.session.Timers.ClientDoneResponse has a default timestamp of 01/01/0001 12:00
                // Messing up any math. By the time the inspector gets to loading the same math.round statement the correct value is displayed in the 
                // inspector Exchange Online tab.
                //
                // This needs more thought, read through Fiddler book some more on what could be happening and whether this can work or if the Response time
                // column is removed from the extension in favour of the response time on the inspector tab.
                //

                // *** For the moment disabled the Response Time column when live tracing. Only displayed on LoadSAZ. ***
                
                /*
                // Trying out delaying the process, waiting for the ClientDoneResponse to be correctly populated.
                // Did not work out, Fiddler process hangs / very slow.
                while (this.session.Timers.ClientDoneResponse.Year < 2000)
                {
                    if (this.session.Timers.ClientDoneResponse.Year > 2000)
                    {
                        break;
                    }
                }
                //session["X-iTTLB"] = this.session.oResponse.iTTLB.ToString() + "ms"; // Known to give inaccurate results.

                //MessageBox.Show("ClientDoneResponse: " + this.session.Timers.ClientDoneResponse + Environment.NewLine + "ClientBeginRequest: " + this.session.Timers.ClientBeginRequest
                //    + Environment.NewLine + "iTTLB: " + this.session.oResponse.iTTLB);
                // The below is not working in a live trace scenario. Reverting back to the previous configuration above as this works for now.
                session["X-iTTLB"] = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds) + "ms";
                */
            }
            //
            /////////////////
        }
        //
        /////////////////////////////
        
        public void OnBeforeReturningError(Session oSession) { }



        /////////////////////////////
        //
        // Function where the Response Server column is populated.
        //
        public void SetResponseServer(Session session)
        {        
            this.session = session;

            // Populate Response Server on session in order of preference from common to obsure.

            // If the response server header is not null or blank then populate it into the response server value.
            if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != "")) {
                this.session["X-ResponseServer"] = this.session.oResponse["Server"];
            }
            // Else if the reponnse Host header is not null or blank then populate it into the response server value
            // Some traffic identifies a host rather than a response server.
            else if ((this.session.oResponse["Host"] != null && (this.session.oResponse["Host"] != "")))
            {
                this.session["X-ResponseServer"] = "Host: " + this.session.oResponse["Host"];
            }
            // Else if the response PoweredBy header is not null or blank then populate it into the response server value.
            // Some Office 365 servers respond as X-Powered-By ASP.NET.
            else if ((this.session.oResponse["X-Powered-By"] != null) && (this.session.oResponse["X-Powered-By"] != "")) {
                this.session["X-ResponseServer"] = "X-Powered-By: " + this.session.oResponse["X-Powered-By"];
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Served-By"] != null && (this.session.oResponse["X-Served-By"] != "")))
            {
                this.session["X-ResponseServer"] = "X-Served-By: " + this.session.oResponse["X-Served-By"];
            }
            // Else if the response X-Served-By header is not null or blank then populate it into the response server value.
            else if ((this.session.oResponse["X-Server-Name"] != null && (this.session.oResponse["X-Server-Name"] != "")))
            {
                this.session["X-ResponseServer"] = "X-Served-Name: " + this.session.oResponse["X-Server-Name"];
            }
            else if (this.session.isTunnel == true)
            {
                this.session["X-ResponseServer"] = "Connect Tunnel";
            }
        }

        /////////////////////////////
        //
        // Function where the Exchange Type column is populated.
        //
        public void SetExchangeType(Session session)
        {
            this.session = session;

            // Outlook Connections.
            if (this.session.fullUrl.Contains("outlook.office365.com/mapi")) { this.session["X-ExchangeType"] = "EXO MAPI"; }
            // Exchange Online Autodiscover.
            else if (this.session.utilFindInRequest("autodiscover", false) > 1 && this.session.utilFindInRequest("onmicrosoft.com", false) > 1) { this.session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("autodiscover") && (this.session.fullUrl.Contains(".onmicrosoft.com"))) { this.session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("autodiscover-s.outlook.com")) { this.session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("onmicrosoft.com/autodiscover")) { this.session["X-ExchangeType"] = "EXO Autodiscover"; }
            // Autodiscover.     
            else if ((this.session.fullUrl.Contains("autodiscover") && (!(this.session.hostname == "outlook.office365.com")))) { this.session["X-ExchangeType"] = "On-Prem Autodiscover"; }
            else if (this.session.hostname.Contains("autodiscover")) { this.session["X-ExchangeType"] = "On-Prem Autodiscover"; }
            // Free/Busy.
            else if (this.session.fullUrl.Contains("WSSecurity"))
            {
                this.session["X-ExchangeType"] = "Free/Busy";
                // Increment HTTP200FreeBusy counter to assist with session classification further on down the line.
                HTTP200FreeBusy++;
            }
            else if (this.session.fullUrl.Contains("GetUserAvailability"))
            {
                this.session["X-ExchangeType"] = "Free/Busy";
                // Increment HTTP200FreeBusy counter to assist with session classification further on down the line.
                HTTP200FreeBusy++;
            }
            else if (this.session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                this.session["X-ExchangeType"] = "Free/Busy";
                // Increment HTTP200FreeBusy counter to assist with session classification further on down the line.
                HTTP200FreeBusy++;
            }
            // EWS.
            else if (this.session.fullUrl.Contains("outlook.office365.com/EWS")) { this.session["X-ExchangeType"] = "EXO EWS"; }
            // Generic Office 365.
            else if (this.session.fullUrl.Contains(".onmicrosoft.com") && (!(this.session.hostname.Contains("live.com")))) { this.session["X -ExchangeType"] = "Exchange Online"; }
            else if (this.session.fullUrl.Contains("outlook.office365.com")) { this.session["X-ExchangeType"] = "Office 365"; }
            else if (this.session.fullUrl.Contains("outlook.office.com")) { this.session["X-ExchangeType"] = "Office 365"; }
            // Office 365 Authentication.
            else if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com")) { this.session["X-ExchangeType"] = "Office 365 Authentication"; }
            // ADFS Authentication.
            else if (this.session.fullUrl.Contains("adfs/services/trust/mex")) { this.session["X-ExchangeType"] = "ADFS Authentication"; }
            // Undetermined, but related to local process.
            else if (this.session.LocalProcess.Contains("outlook")) { this.session["X-ExchangeType"] = "Outlook"; }
            else if (this.session.LocalProcess.Contains("iexplore")) { this.session["X-ExchangeType"] = "Internet Explorer"; }
            else if (this.session.LocalProcess.Contains("chrome")) { this.session["X-ExchangeType"] = "Chrome"; }
            else if (this.session.LocalProcess.Contains("firefox")) { this.session["X-ExchangeType"] = "Firefox"; }
            // Everything else.
            else { this.session["X-ExchangeType"] = "Not Exchange"; }

            /////////////////////////////
            //
            // Exchange Type overrides
            //
            // First off if the local process is nullor blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS
            // So don't pay any attention to overrides for this type of traffic.
            if ((this.session.LocalProcess == null) || (this.session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                this.session["X-ExchangeType"] = "Remote Capture";
            }
            else
            {
                // With that out of the way,  if the traffic is not related to any of the below processes call it out.
                // So if for example lync.exe is the process write that to the Exchange Type column.
                if (!(this.session.LocalProcess.Contains("outlook") ||
                    this.session.LocalProcess.Contains("searchprotocolhost") ||
                    this.session.LocalProcess.Contains("iexplore") ||
                    this.session.LocalProcess.Contains("chrome") ||
                    this.session.LocalProcess.Contains("firefox") ||
                    this.session.LocalProcess.Contains("edge") ||
                    this.session.LocalProcess.Contains("w3wp")))
                {
                    // Everything which is not detected as related to Exchange, Outlook or OWA in some way.
                    { this.session["X-ExchangeType"] = this.session.LocalProcess; }
                }
            }
        }
        //
        /////////////////////////////
    }
}