using System;
using System.Windows.Forms;
using Fiddler;
using System.Linq;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;

namespace EXOFiddlerInspector
{
    public class ColouriseWebSessions : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
    {
        MenuUI calledMenuUI = new MenuUI();
        ColumnsUI calledColumnsUI = new ColumnsUI();

        /////////////////
        /// <summary>
        /// Developer Demo Mode. If enabled as much domain specific information as possible will be replaced with contoso.com.
        /// Note: This is not much right now, just outputs in response comments on the inspector tab.
        /// </summary>
        ///
        Boolean DeveloperDemoMode = false;
        Boolean DeveloperDemoModeBreakScenarios = false;
        /////////////////
        
        List<string> Developers = new List<string>(new string[] { "jeknight", "brandev", "jasonsla" });
        public List<string> GetDeveloperList()
        {
            return Developers;
        }

        internal Session session { get; set; }

        private bool boolExtensionEnabled = false;
        private bool boolColumnsEnableAllEnabled = false;
        private bool boolResponseTimeColumnEnabled = false;
        private bool boolResponseServerColumnEnabled = false;
        private bool boolExchangeTypeColumnEnabled = false;
        private bool boolAppLoggingEnabled = false;
        private bool boolHighlightOutlookOWAOnlyEnabled = false;
        private bool boolManualCheckForUpdate = false;

        private string searchTerm;
        private string RedirectAddress;
        private int HTTP200SkipLogic;
        private int HTTP200FreeBusy;

        // Enable/disable switch for Fiddler Application Log entries from extension.
        private bool AppLoggingEnabled = true;


        /////////////////

        #region OnLoad
        /////////////////
        //
        // OnLoad
        //
        public void OnLoad()
        {
            /////////////////
            //
            // Make sure that even if these are mistakenly left on from debugging, production users are not impacted.
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
                calledColumnsUI.EnsureResponseServerColumn();
            }

            // Call the function to add column if the menu item is checked and if the extension is enabled.
            if (boolExchangeTypeColumnEnabled && boolExtensionEnabled)
            {
                calledColumnsUI.EnsureExchangeTypeColumn();
            }

            // Initialise menu, called from MenuUI.cs.
            calledMenuUI.InitializeMenu();

            // Add the menu.
            FiddlerApplication.UI.mnuMain.MenuItems.Add(calledMenuUI.ExchangeOnlineTopMenu);

            // Call function to set Enable all columns check box to required setting.
            calledMenuUI.SetEnableAllMenuItem();

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
                // Check for app update.
                CheckForAppUpdate calledCheckForAppUpdate = new CheckForAppUpdate();
                calledCheckForAppUpdate.CheckForUpdate();
            }

            // Call the function to add column if the menu item is checked and if the extension is enabled.
            if (boolResponseTimeColumnEnabled && boolExtensionEnabled)
            {
                calledColumnsUI.EnsureResponseTimeColumn();
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
                    calledColumnsUI.SetExchangeType(session);
                }

                // Populate the ResponseServer column on load SAZ, if the column is enabled, and the extension is enabled
                if (boolResponseServerColumnEnabled && boolExtensionEnabled)
                {
                    calledColumnsUI.SetResponseServer(session);
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

        public void IncrementHTTP200FreeBusyCount()
        {
            HTTP200FreeBusy++;
            // Write the value of HTTP200SkipLogic into debug output.
            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: HTTP200FreeBusy Incremented {HTTP200FreeBusy.ToString()}");
        }


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
                    case 456:
                        /////////////////////////////
                        //
                        // HTTP 456: Multi-Factor Required.
                        //
                        /////////////////////////////
                        if (this.session.utilFindInResponse("you must use multi-factor authentication", false) > 1)
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "Multi-Factor Auth";
                        }
                        else
                        {
                            this.session["ui-backcolor"] = HTMLColourOrange;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "Multi-Factor Auth?";
                        }
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

                        /////////////////////////////
                        // 1. HTTP 504 Bad Gateway 'internet has been blocked'
                        if ((this.session.utilFindInResponse("access", false) > 1) &&
                            (this.session.utilFindInResponse("internet", false) > 1) &&
                            (this.session.utilFindInResponse("blocked", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "INTERNET BLOCKED!";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + "  HTTP 504 Gateway Timeout -- Internet Access Blocked.");
                            }
                        }

                        /////////////////////////////
                        // 99. Pick up any other 504 Gateway Timeout and write data into the comments box.
                        else
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            if (boolAppLoggingEnabled && boolExtensionEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 504 Gateway Timeout.");
                            }
                            //
                            /////////////////////////////
                        }
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
                calledColumnsUI.SetExchangeType(session);
            }

            /////////////////
            //
            // Call the function to populate the session type column on live trace, if the column is enabled.
            if (boolResponseServerColumnEnabled && boolExtensionEnabled)
            {
                calledColumnsUI.SetResponseServer(session);
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


        }
        //
        /////////////////////////////
        
        public void OnBeforeReturningError(Session oSession) { }
    }
}