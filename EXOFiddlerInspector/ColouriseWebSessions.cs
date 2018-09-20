using System;
using System.Windows.Forms;
using Fiddler;
using System.Linq;
using System.Xml;
using System.Net;

namespace EXOFiddlerInspector
{
    public class ColouriseWebSessions : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
    {

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

        // Enable/disable switch for colourising sessions.
        private bool ColouriseSessionsEnabled = true;

        private string searchTerm;
        internal Session session { get; set; }

        private void InitializeMenu()
        {
            // Setup each menu item name and ordering.
            this.ExchangeOnlineTopMenu = new MenuItem("Exchange Online");

            this.miEnabled = new MenuItem("&Extension Enabled");
            this.miEnabled.Index = 0;

            this.miResponseTimeColumnEnabled = new MenuItem("Response &Time Column Enabled");
            this.miResponseTimeColumnEnabled.Index = 1;


            this.miResponseServerColumnEnabled = new MenuItem("Response &Server Column Enabled");
            this.miResponseServerColumnEnabled.Index = 2;

            this.miExchangeTypeColumnEnabled = new MenuItem("Exchange T&ype Column Enabled");
            this.miExchangeTypeColumnEnabled.Index = 3;

            this.miAppLoggingEnabled = new MenuItem("Application &Logging Enabled");
            this.miAppLoggingEnabled.Index = 4;

            this.miWiki = new MenuItem("Extension &Wiki");
            this.miWiki.Index = 5;

            this.miReportIssues = new MenuItem("&Report Issues");
            this.miReportIssues.Index = 6;

            this.miCheckForUpdate = new MenuItem("&Check For Update");
            this.miCheckForUpdate.Index = 7;

            // Add menu items to top level menu.
            this.ExchangeOnlineTopMenu.MenuItems.AddRange(new MenuItem[] { this.miEnabled,
                this.miResponseTimeColumnEnabled,
                this.miResponseServerColumnEnabled,
                this.miExchangeTypeColumnEnabled,
                this.miAppLoggingEnabled,
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

            this.miWiki.Click += new System.EventHandler(this.miWiki_Click);

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
            EnableDisableMenuItemsAccordingToExtensionStatus();
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

    /////////////////
    //
    // OnLoad
    //
    public void OnLoad()
        {
            // Get application preferences from settings saved from last set.
            this.boolExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
            this.boolResponseTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseTimeColumnEnabled", false);
            this.boolResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
            this.boolExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
            this.boolAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);

            // Call the FirstRun function to enable the extension and columns for users running the extension on their computer for the first time.
            FirstRunEnableMenuOptions();

            // Call the function to add column if the menu item is checked and if the extension is enabled.
            if (boolResponseTimeColumnEnabled && boolExtensionEnabled)
            {
                EnsureResponseTimeColumn();
            }

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
            EnableDisableMenuItemsAccordingToExtensionStatus();

            // Call function to process sessions only if the extension is enabled.
            if (boolExtensionEnabled)
            {
                FiddlerApplication.OnLoadSAZ += HandleLoadSaz;
            }
        }
        //
        /////////////////

        /////////////////
        // Read out an application preference and if not set we know this is the first 
        // time the extension has run on this machine. Enable all options to light up functionality
        // for first time users.
        public void FirstRunEnableMenuOptions()
        {
            // Anticipate extensions.EXFiddlerInspector.FirstRun will be null on first run. Thereafter it will be set to false.
            if ((FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.FirstRun", false) != false)) 
            {
                // Light up functionality for first run.
                this.boolExtensionEnabled = true;
                this.boolResponseTimeColumnEnabled = true;
                this.boolResponseServerColumnEnabled = true;
                this.boolExchangeTypeColumnEnabled = true;

                // Set this app preference as false so we don't execute the above after first run.
                FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.FirstRun", false);
            }
        }
        //
        /////////////////

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
            
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 2, 130, "@response.Server");
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

            FiddlerApplication.UI.lvSessions.AddBoundColumn("Exchange Type", 2, 130, "X-ExchangeType");
            bExchangeTypeColumnCreated = true;
            //
            /////////////////
        }
        //
        /////////////////

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
            
            FiddlerApplication.UI.lvSessions.BeginUpdate();
            int sessioncount = 0;
            foreach (var session in e.arrSessions)
            {
                sessioncount++;
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

                // Colourise sessions on load SAZ.
                if (ColouriseSessionsEnabled && boolExtensionEnabled)
                {
                    OnPeekAtResponseHeaders(session); //Run whatever function you use in IAutoTamper
                    session.RefreshUI();
                }
            }
            FiddlerApplication.UI.lvSessions.EndUpdate();
        }
        //
        /////////////////

        /////////////////
        //
        // Check for updates.
        //
        // Called from Onload. Not currently implemented, due to web call issue, as Fiddler substitutes in http://localhost:8888 as the proxy server.
        //
        public void CheckForUpdate()
        {
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

        private void OnPeekAtResponseHeaders(Session session)
        {

            this.session = session;

            // Colour codes for sessions. Softer tones, easier on the eye than standard red, orange and green.
            string HTMLColourBlue = "#81BEF7";
            string HTMLColourGreen = "#81f7ba"; 
            string HTMLColourRed = "#f78f81";
            string HTMLColourGrey = "#BDBDBD";
            string HTMLColourOrange = "#f7ac81";

            if (this.session.LocalProcess.Contains("outlook") ||
            this.session.LocalProcess.Contains("searchprotocolhost") ||
            this.session.LocalProcess.Contains("iexplore") ||
            this.session.LocalProcess.Contains("chrome") ||
            this.session.LocalProcess.Contains("firefox") ||
            this.session.LocalProcess.Contains("edge") ||
            this.session.LocalProcess.Contains("w3wp"))
            {

                this.session.utilDecodeRequest(true);
                this.session.utilDecodeResponse(true);
                
                int wordCount = 0;

                // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
                //
                // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
                //

                string text = this.session.ToString();

                //Convert the string into an array of words  
                string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', }, StringSplitOptions.RemoveEmptyEntries);

                // Create the query. Use ToLowerInvariant to match "data" and "Data"   
                var matchQuery = from word in source
                                 where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                 select word;

                // Query samples:
                //string searchTerm = "error";
                //string[] searchTerms = { "Error", "FederatedStsUnreachable" };



                #region switchstatement
                switch (this.session.responseCode)
                {
                    case 0:
                        #region HTTP0
                        /////////////////////////////
                        //
                        //  HTTP 0: No Response.
                        //
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 200:
                        #region HTTP200
                        /////////////////////////////
                        //
                        // HTTP 200
                        //

                        /////////////////////////////
                        // 1. Exchange On-Premise Autodiscover redirect.
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
                            string RedirectAddress = RedirectResponseBody.Substring(start, charcount).Replace("<RedirectAddr>", "");

                            if (RedirectAddress.Contains(".onmicrosoft.com"))
                            {
                                this.session["ui-backcolor"] = HTMLColourGreen;
                                this.session["ui-color"] = "black";
                                if(boolAppLoggingEnabled && boolExtensionEnabled)
                                {
                                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 200 Exchange On-Premise redirect address: " + RedirectAddress);
                                }
                            }
                            // Highlight if we got this far and do not have a redirect address which points to
                            // Exchange Online such as: contoso.mail.onmicrosoft.com.
                            else
                            {
                                this.session["ui-backcolor"] = HTMLColourRed;
                                this.session["ui-color"] = "black";
                                if(boolAppLoggingEnabled && boolExtensionEnabled)
                                {
                                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 200 Exchange On-Premise redirect address: " + RedirectAddress);
                                }
                            }
                        }

                        /////////////////////////////
                        //
                        // 2. Exchange On-Premise Autodiscover redirect - address can't be found
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
                            if(boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 200 Exchange On-Premise redirect address. Error code 500: The email address can't be found.");
                            }                           
                        }

                        /////////////////////////////
                        //
                        // 99. No other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.
                        else
                        {
                            searchTerm = "Error";

                            // Count the matches, which executes the query.  
                            wordCount = matchQuery.Count();

                            if (wordCount > 0)
                            {
                                // Special attention to HTTP 200's where the keyword 'error' is found.
                                // Red text on black background.
                                this.session["ui-backcolor"] = "black";
                                this.session["ui-color"] = "red";
                            }
                            else
                            {
                                // All good.
                                this.session["ui-backcolor"] = HTMLColourGreen;
                                this.session["ui-color"] = "black";
                            }
                        }
                        //
                        /////////////////////////////
                        #endregion
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
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        break;
                    case 301:
                        #region HTTP301
                        /////////////////////////////
                        //
                        //  HTTP 301: Moved Permanently.
                        //
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 302:
                        #region HTTP302
                        /////////////////////////////
                        //
                        //  HTTP 302: Found / Redirect.
                        //            
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 304:
                        #region HTTP304
                        /////////////////////////////
                        //
                        //  HTTP 304: Not modified.
                        //
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 307:
                        #region HTTP307
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
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 307 On-Prem Temp Redirect - Unexpected location!");
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
                        #endregion
                        break;
                    case 401:
                        #region HTTP401
                        /////////////////////////////
                        //
                        //  HTTP 401: UNAUTHORIZED.
                        //
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 403:
                        #region HTTP403
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
                            if(boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");
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
                        #endregion
                        break;
                    case 404:
                        #region HTTP404
                        /////////////////////////////
                        //
                        //  HTTP 404: Not Found.
                        //
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 429:
                        #region HTTP429
                        /////////////////////////////
                        //
                        //  HTTP 429: Too Many Requests.
                        //
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 440:
                        #region HTTP440
                        /////////////////////////////
                        //
                        // HTTP 440: Need to know more about these.
                        // For the moment do nothing.
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 500:
                        #region HTTP500
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
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 500 Internal Server Error.");
                        }
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 502:
                        #region HTTP502
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
                        }

                        /////////////////////////////
                        //
                        // 2. Exchange Online Autodiscover False Positive.
                        //
                        else if ((this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                            (this.session.utilFindInResponse("autodiscover", false) > 1) &&
                            (this.session.utilFindInResponse(":443", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourBlue;
                            this.session["ui-color"] = "black";
                            if(boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 502 Bad Gateway - False Positive.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 3. Exchange Online DNS lookup on contoso.onmicrosoft.com, False Positive!?
                        //
                        // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                        // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
                        else if ((session.utilFindInResponse("The requested name is valid, but no data of the requested type was found", false) > 1) &&
                            // Found Outlook is going root domain Autodiscover lookups. Vanity domain, which we have no way to key off of in logic here.
                            // Excluding this if statement to broaden DNS lookups we say are OK.
                            (this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                            (this.session.utilFindInResponse("failed. System.Net.Sockets.SocketException", false) > 1) &&
                            (this.session.utilFindInResponse("DNS Lookup for ", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourBlue;
                            this.session["ui-color"] = "black";
                            if(boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 502 Bad Gateway - False Positive.");
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
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 502 Bad Gateway.");
                            }
                        }
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 503:
                        #region HTTP503
                        /////////////////////////////
                        //
                        //  HTTP 503: SERVICE UNAVAILABLE.
                        //
                        // Call out all 503 Service Unavailable as something to focus on.
                        searchTerm = "FederatedStsUnreachable";
                        //"Service Unavailable"

                        // Count the matches, which executes the query.  
                        wordCount = matchQuery.Count();
                        if (wordCount > 0)
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            if(boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 503 Service Unavailable. Found keyword 'FederatedStsUnreachable' in response body!");
                            }
                        }
                        else
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            if(boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 503 Service Unavailable.");
                            }
                        }
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 504:
                        #region HTTP504
                        /////////////////////////////
                        //
                        //  HTTP 504: GATEWAY TIMEOUT.
                        //
                        // Call out all 504 Gateway Timeout as something to focus on.
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        if (boolAppLoggingEnabled && boolExtensionEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: Session " + this.session.id + " HTTP 504 Gateway Timeout.");
                        }
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    default:
                        break;
                }
                #endregion
                //}
            }
            else
            {
                // Everything which is not detected as related to Exchange, Outlook or OWA in some way.
                this.session["ui-backcolor"] = HTMLColourGrey;
                this.session["ui-color"] = "black";
            }
        }

        #endregion

        public void OnBeforeUnload() { }

        public void OnPeekAtResponseHeaders(IAutoTamper2 AllSessions) { }

        public void AutoTamperRequestBefore(Session oSession) { }

        public void AutoTamperRequestAfter(Session oSession) { }

        public void AutoTamperResponseBefore(Session oSession) { }

        public void AutoTamperResponseAfter(Session session)
        {
            /////////////////
            //
            // Call the function to colourise sessions for live traffic capture.
            //
            if (ColouriseSessionsEnabled && boolExtensionEnabled)
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
            // in reverse order for my preference on column order.

            //FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("#", 0, -1);
            //FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Result", 1, -1);

            // These get called on each session, seen strange behaviour on reordering on live trace due 
            // to setting each of these as ordering 2 to ensure column positions regardless of column enabled selections.
            // Use an if statement to fire these once per Fiddler application session.
            if (this.session.id == 1)
            {
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
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 11, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Comments", 12, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Custom", 13, -1);
            */
            //
            /////////////////

            /////////////////
            //
            // Call the function to populate the session type column on live trace, if the column is enabled.
            if (boolExchangeTypeColumnEnabled && boolExtensionEnabled)
            {
                SetExchangeType(session);
            }

            //
            // Populate the ResponseTime column on live trace, if the column is enabled.
            if (boolResponseTimeColumnEnabled && boolExtensionEnabled) {
                //session["X-iTTLB"] = session.oResponse.iTTLB.ToString() + "ms";
                session["X-iTTLB"] = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds) + "ms";
            }
            //
            /////////////////
        }

        public void OnBeforeReturningError(Session oSession) { }

        public void SetExchangeType(Session session)
        {
            this.session = session;

            // Outlook Connections.
            if (this.session.fullUrl.Contains("outlook.office365.com/mapi")) { session["X-ExchangeType"] = "EXO MAPI"; }
            // Exchange Online Autodiscover.
            else if (this.session.utilFindInRequest("autodiscover", false) > 1 && this.session.utilFindInRequest("onmicrosoft.com", false) > 1) { session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("autodiscover") && (this.session.fullUrl.Contains(".onmicrosoft.com"))) { session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("autodiscover-s.outlook.com")) { session["X-ExchangeType"] = "EXO Autodiscover"; }
            else if (this.session.fullUrl.Contains("onmicrosoft.com/autodiscover")) { session["X-ExchangeType"] = "EXO Autodiscover"; }
            // Exchange On-Premise Autodiscover Redirect.
            else if (this.session.utilFindInResponse("<Action>redirectAddr</Action>", false) > 1) { session["X-ExchangeType"] = "On-Prem AutoD Redirect"; }
            // Autodiscover.     
            else if (this.session.fullUrl.Contains("autodiscover")) { session["X-ExchangeType"] = "Autodiscover"; }
            else if (this.session.url.Contains("autodiscover")) { session["X-ExchangeType"] = "Autodiscover"; }
            else if (this.session.hostname.Contains("autodiscover")) { session["X-ExchangeType"] = "Autodiscover"; }
            // Free/Busy.
            else if (this.session.fullUrl.Contains("WSSecurity")) { session["X-ExchangeType"] = "Free/Busy"; }
            else if (this.session.fullUrl.Contains("GetUserAvailability")) { session["X-ExchangeType"] = "Free/Busy"; }
            else if (this.session.utilFindInResponse("GetUserAvailability", false) > 1) { session["X-ExchangeType"] = "Free/Busy"; }
            // EWS.
            else if (this.session.fullUrl.Contains("outlook.office365.com/EWS")) { session["X-ExchangeType"] = "EXO EWS"; }
            // Generic Office 365.
            else if (this.session.fullUrl.Contains(".onmicrosoft.com") && (!(this.session.hostname.Contains("live.com")))) { session["X -ExchangeType"] = "Exchange Online"; }
            else if (this.session.fullUrl.Contains("outlook.office365.com")) { session["X-ExchangeType"] = "Office 365"; }
            else if (this.session.fullUrl.Contains("outlook.office.com")) { session["X-ExchangeType"] = "Office 365"; }
            // Office 365 Authentication.
            else if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com")) { session["X-ExchangeType"] = "Office 365 Authentication"; }
            // ADFS Authentication.
            else if (this.session.fullUrl.Contains("adfs/services/trust/mex")) { session["X-ExchangeType"] = "ADFS Authentication"; }
            // Undetermined, but related to local process.
            else if (this.session.LocalProcess.Contains("outlook")) { session["X-ExchangeType"] = "Outlook"; }
            else if (this.session.LocalProcess.Contains("iexplore")) { session["X-ExchangeType"] = "Internet Explorer"; }
            else if (this.session.LocalProcess.Contains("chrome")) { session["X-ExchangeType"] = "Chrome"; }
            else if (this.session.LocalProcess.Contains("firefox")) { session["X-ExchangeType"] = "Firefox"; }
            // Everything else.
            else { session["X-ExchangeType"] = "Not Exchange"; }
        }
    }
}