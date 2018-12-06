using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace EXOFiddlerInspector
{
    /// <summary>
    /// SessionRuleSet class. All extension session logic lives here.
    /// Anything which involves extensive logic for session values the extension uses should live here.
    /// </summary>
    public class SessionRuleSet : IAutoTamper
    {
        // References to other classes.
        //MenuUI calledMenuUI = new MenuUI();
        ColumnsUI calledColumnsUI = new ColumnsUI();
        Preferences calledPreferences = new Preferences();

        internal Session session { get; set; }

        private string searchTerm;
        private string RedirectAddress;
        private int HTTP200SkipLogic;
        private int HTTP200FreeBusy;
        private int FalsePositive;

        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public Boolean bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled", false);
        public Boolean bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public Boolean bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public Boolean bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
        public Boolean bAuthColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AuthColumnEnabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public Boolean bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
        public int iExecutionCount = FiddlerApplication.Prefs.GetInt32Pref("extensions.EXOFiddlerInspector.ExecutionCount", 0);

        public void AutoTamperRequestAfter(Session oSession)
        {
            // Not used here.
            //throw new NotImplementedException();
        }

        public void AutoTamperRequestBefore(Session oSession)
        {
            // Not used here.
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseAfter(Session session)
        {
            calledColumnsUI.AddAllEnabledColumns();
            calledColumnsUI.OrderColumns();
        }

        public void AutoTamperResponseBefore(Session oSession)
        {
            // Not used here.
            //throw new NotImplementedException();
        }

        public void OnBeforeReturningError(Session oSession)
        {
            // Not used here.
            //throw new NotImplementedException();
        }

        public void OnBeforeUnload()
        {
            // Not used here.
            //throw new NotImplementedException();
        }

        public void OnLoad()
        {
            calledColumnsUI.AddAllEnabledColumns();
            calledColumnsUI.OrderColumns();
            // Not used here.
            //throw new NotImplementedException();
        }

        /////////////////////////////
        //
        // Function where all session colourisation happens.
        //
        public void OnPeekAtResponseHeaders(Session session)
        {
            // Developer list is actually set in Preferences.cs.
            List<string> calledDeveloperList = calledPreferences.GetDeveloperList();
            Boolean DeveloperDemoMode = calledPreferences.GetDeveloperMode();
            Boolean DeveloperDemoModeBreakScenarios = calledPreferences.GetDeveloperDemoModeBreakScenarios();

            // Reset these session counters.
            HTTP200SkipLogic = 0;
            HTTP200FreeBusy = 0;
            FalsePositive = 0;

            this.session = session;

            // Colour codes for sessions. Softer tones, easier on the eye than standard red, orange and green.
            string HTMLColourBlue = "#81BEF7";
            string HTMLColourGreen = "#81f7ba";
            string HTMLColourRed = "#f06141";
            string HTMLColourGrey = "#BDBDBD";
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

                this.session["X-ResponseAlertTextBox"] = "Apache is answering Autodiscover requests!";
                this.session["X-ResponseCommentsRichTextboxText"] = "An Apache Web Server(Unix/Linux) is answering Autodiscover requests!" +
                    Environment.NewLine +
                    "This should not be happening. Consider disabling Root Domain Autodiscover lookups." +
                    Environment.NewLine +
                    "See ExcludeHttpsRootDomain on https://support.microsoft.com/en-us/help/2212902/unexpected-autodiscover-behavior-when-you-have-registry-settings-under" +
                    Environment.NewLine +
                    "Beyond this the web administrator responsible for the server needs to stop the Apache web server from answering these requests.";

                if (bAppLoggingEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 405 Method Not Allowed; Apache is answering Autodiscover requests!");
                }
            }
            /////////////////////////////
            // If the above is not true, then drop into the switch statement based on individual response codes
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
                        this.session["X-ExchangeType"] = "!NO RESPONSE!";

                        this.session["X-ResponseAlertTextBox"] = "!HTTP 0 No Response!";
                        this.session["X-ResponseCommentsRichTextboxText"] = (Properties.Settings.Default.HTTPQuantity);

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 0 No response");
                        }

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

                            this.session["X-ResponseAlertTextBox"] = "Connect Tunnel";
                            this.session["X-ResponseCommentsRichTextboxText"] = "Encrypted HTTPS traffic flows through this CONNECT tunnel. " +
                                "HTTPS Decryption is enabled in Fiddler, so decrypted sessions running in this tunnel will be shown in the Web Sessions list.";

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

                            if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
                            {
                                // If as well as being in demo mode, demo mode break scenarios is enabled. Show fault through incorrect direct
                                // address for an Exchange Online mailbox.
                                if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoModeBreakScenarios == true)
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

                                this.session["X-ResponseAlertTextBox"] = "Exchange On-Premise Autodiscover redirect.";
                                this.session["X-ResponseCommentsRichTextboxText"] = "Exchange On-Premise Autodiscover redirect address to Exchange Online found." +
                                    Environment.NewLine +
                                    Environment.NewLine +
                                    "RedirectAddress: " + RedirectAddress +
                                    Environment.NewLine +
                                    Environment.NewLine +
                                    "This is what we want to see, the mail.onmicrosoft.com redirect address (you may know this as the target address or remote " +
                                    "routing address) from On-Premise sends Outlook to Office 365.";

                                HTTP200SkipLogic++;

                                if (bAppLoggingEnabled)
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
                                this.session["X-ExchangeType"] = "!AUTOD REDIRECT ADDR!";

                                this.session["X-ResponseAlertTextBox"] = "!Exchange On-Premise Autodiscover redirect!";
                                this.session["X-ResponseCommentsRichTextboxText"] = "Exchange On-Premise Autodiscover redirect address found, which does not contain .onmicrosoft.com." +
                                    Environment.NewLine +
                                    Environment.NewLine +
                                    "RedirectAddress: " + RedirectAddress +
                                    Environment.NewLine +
                                    Environment.NewLine +
                                    "If this is an Office 365 mailbox the targetAddress from On-Premise is not sending Outlook to Office 365!";

                                // Increment HTTP200SkipLogic so that 99 does not run below.
                                HTTP200SkipLogic++;

                                if (bAppLoggingEnabled)
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
                            this.session["X-ExchangeType"] = "!NO AUTOD REDIRECT ADDR!";

                            this.session["X-ResponseAlertTextBox"] = "!Exchange On-Premise Autodiscover redirect: Error Code 500!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "Exchange On-Premise Autodiscover redirect address can't be found. "
                                + "Look for other On-Premise Autodiscover responses, we may have a " +
                                "valid Autodiscover targetAddress from On-Premise in another session in this trace.";

                            // Increment HTTP200SkipLogic so that 99 does not run below.
                            HTTP200SkipLogic++;

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 Exchange On-Premise redirect address. Error code 500: The email address can't be found.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 4. Exchange Online Autodiscover
                        //

                        // Make sure this session if an Exchange Online Autodiscover request.
                        if ((this.session.hostname == "autodiscover-s.outlook.com") && (this.session.uriContains("autodiscover.xml")))
                        {
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

                                this.session["X-ResponseAlertTextBox"] = "Exchange Online Autodiscover";
                                this.session["X-ResponseCommentsRichTextboxText"] = "Exchange Online Autodiscover.";

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

                            this.session["X-ResponseAlertTextBox"] = "Outlook for Windows MAPI traffic";
                            this.session["X-ResponseCommentsRichTextboxText"] = "Outlook for Windows MAPI traffic.";

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

                                this.session["X-ResponseAlertTextBox"] = "GetUnifiedGroupsSettings EWS call.";
                                this.session["X-ResponseCommentsRichTextboxText"] = "<GroupCreationEnabled>true</GroupCreationEnabled> found in response body. " +
                                    "Expect user to be able to create Office 365 groups in Outlook.";

                                HTTP200SkipLogic++;
                            }
                            // User cannot create Office 365 groups. Not an error condition in and of itself.
                            else if (this.session.utilFindInResponse("<GroupCreationEnabled>false</GroupCreationEnabled>", false) > 1)
                            {
                                this.session["ui-backcolor"] = HTMLColourGreen;
                                this.session["ui-color"] = "black";
                                this.session["X-ExchangeType"] = "EWS GetUnifiedGroupsSettings";

                                this.session["X-ResponseAlertTextBox"] = "GetUnifiedGroupsSettings EWS call!";
                                this.session["X-ResponseCommentsRichTextboxText"] = "<GroupCreationEnabled>false</GroupCreationEnabled> found in response body. " +
                                    "Expect user to NOT be able to create Office 365 groups in Outlook.";

                                HTTP200SkipLogic++;
                            }
                            // Did not see the expected keyword in the response body. This is the error condition.
                            else
                            {
                                this.session["ui-backcolor"] = HTMLColourRed;
                                this.session["ui-color"] = "black";
                                this.session["X-ExchangeType"] = "!EWS GetUnifiedGroupsSettings!";

                                this.session["X-ResponseAlertTextBox"] = "!GetUnifiedGroupsSettings EWS call!";
                                this.session["X-ResponseCommentsRichTextboxText"] = "Though GetUnifiedGroupsSettings scenario was detected neither <GroupCreationEnabled>true</GroupCreationEnabled> or" +
                                    "<GroupCreationEnabled>false</GroupCreationEnabled> was found in the response body. Check the Raw tab for more details.";

                                // Do not do HTTP200SkipLogic here, expected response not found. Run keyword search on response for deeper inpsection of response.
                                // HTTP200SkipLogic++;
                                {
                                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 GetUnifiedGroupsSettings!");
                                }
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
                                string wordCountErrorText;
                                string wordCountFailedText;
                                string wordCountExceptionText;

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
                                    if (wordCountError == 1)
                                    {
                                        wordCountErrorText = wordCountError + " time.";
                                    }
                                    else
                                    {
                                        wordCountErrorText = wordCountError + " times.";
                                    }

                                    if (wordCountFailed == 1)
                                    {
                                        wordCountFailedText = wordCountFailed + " time.";
                                    }
                                    else
                                    {
                                        wordCountFailedText = wordCountFailed + " times.";
                                    }

                                    if (wordCountException == 1)
                                    {
                                        wordCountExceptionText = wordCountException + " time.";
                                    }
                                    else
                                    {
                                        wordCountExceptionText = wordCountException + " times.";
                                    }

                                    // Special attention to HTTP 200's where the keyword 'error' or 'failed' is found.
                                    // Red text on black background.
                                    this.session["ui-backcolor"] = "black";
                                    this.session["ui-color"] = "red";
                                    this.session["X-ExchangeType"] = "!FAILURE LURKING!";

                                    this.session["X-ResponseAlertTextBox"] = "!'error', 'failed' or 'exception' found in respone body!";
                                    this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 200: Errors or failures found in response body. " +
                                        "Check the Raw tab, click 'View in Notepad' button bottom right, and search for error in the response to review." +
                                        Environment.NewLine + 
                                        Environment.NewLine +
                                        "After splitting all words in the response body the following were found:" + 
                                        Environment.NewLine +
                                        Environment.NewLine + 
                                        "Keyword 'Error' found " + wordCountErrorText +
                                        Environment.NewLine + 
                                        "Keyword 'Failed' found " + wordCountFailedText +
                                        Environment.NewLine + 
                                        "Keyword 'Exception' found " + wordCountExceptionText;

                                    if (bAppLoggingEnabled)
                                    {
                                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 FAILURE LURKING!");
                                    }
                                }
                                else
                                {
                                    // All good.
                                    this.session["ui-backcolor"] = HTMLColourGreen;
                                    this.session["ui-color"] = "black";

                                    this.session["X-ResponseAlertTextBox"] = "No failures keywords detected in respone body.";
                                    this.session["X-ResponseCommentsRichTextboxText"] = "No failures keywords ('error', 'failed' or 'exception') detected in respone body.";
                                }
                            }
                            // HTTP200SkipLogic is >= 1 or HTTP200FreeBusy is 0.
                            else
                            {
                                // Since we use HTTP200SkipLogic and skipped the code above to split words and search for keywords, and we have also not detected any other conditions
                                // mark the remaining sessions as yellow, not detected.
                                if (string.IsNullOrEmpty(this.session["UI-BACKCOLOR"]) && string.IsNullOrEmpty(this.session["UI-COLOR"]))
                                {
                                    this.session["ui-backcolor"] = "Yellow";
                                    this.session["ui-color"] = "black";

                                    this.session["X-ResponseAlertTextBox"] = "Undefined";
                                    this.session["X-ResponseCommentsRichTextboxText"] = "Undefined";

                                    if (bAppLoggingEnabled)
                                    {
                                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 200 ; 99 Undefined.");
                                    }
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

                        this.session["X-ResponseAlertTextBox"] = "HTTP 201 Created.";
                        this.session["X-ResponseCommentsRichTextboxText"] = "Not expecting this to be anything which needs attention for troubleshooting.";

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 201 Created.");
                        }
                        //
                        /////////////////////////////
                        break;
                    case 204:
                        /////////////////////////////
                        //
                        //  HTTP 204: No Content.
                        //
                        // Somewhat highlight these.
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";

                        this.session["X-ResponseAlertTextBox"] = "HTTP 204 No Content.";
                        this.session["X-ResponseCommentsRichTextboxText"] = Properties.Settings.Default.HTTPQuantity;

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 204 No content.");
                        }
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

                        this.session["X-ResponseAlertTextBox"] = "HTTP 301 Moved Permanently";
                        this.session["X-ResponseCommentsRichTextboxText"] = "Nothing of concern here at this time.";

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 301 Moved Permanently.");
                        }
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

                        this.session["X-ResponseAlertTextBox"] = "Exchange On-Premise Autodiscover redirect to Exchange Online.";
                        this.session["X-ResponseCommentsRichTextboxText"] = "Exchange On-Premise Autodiscover redirect to Exchange Online.";

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 302 Found / Redirect.");
                        }
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

                        this.session["X-ResponseAlertTextBox"] = "HTTP 304 Not Modified";
                        this.session["X-ResponseCommentsRichTextboxText"] = "Nothing of concern here at this time.";

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 304 Not modified.");
                        }
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
                            this.session["X-ExchangeType"] = "!UNEXPECTED LOCATION!";

                            this.session["X-ResponseAlertTextBox"] = "!HTTP 307 Temporary Redirect!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 307: Temporary Redirects have been seen to redirect Exchange Online Autodiscover " +
                                "calls back to On-Premise resources, breaking Outlook connectivity." + Environment.NewLine +
                                "This session has enough data points to be an Autodiscover request for Exchange Online which has not been sent to " +
                                "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml as expected." + Environment.NewLine +
                                "Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place.";

                            if (bAppLoggingEnabled)
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

                            this.session["X-ResponseAlertTextBox"] = "HTTP 307 Temporary Redirect";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 307: Temporary Redirects have been seen to redirect Exchange Online Autodiscover calls " +
                                "back to On-Premise resources, breaking Outlook connectivity. " +
                                Environment.NewLine +
                                "Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place. " +
                                Environment.NewLine +
                                "If this session is not for an Outlook process then the information above may not be relevant to the issue under investigation.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 307 Temp Redirect.");
                            }
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

                        this.session["X-ResponseAlertTextBox"] = "HTTP 401 Unauthorized";
                        this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 401: Unauthorized / Authentication Challenge. These are expected and are not an issue as long as a subsequent " +
                            "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. " +
                            Environment.NewLine +
                            Environment.NewLine +
                            "If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.";

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 401 Auth Challenge.");
                        }
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
                            this.session["X-ExchangeType"] = "!WEB PROXY BLOCK!";

                            this.session["X-ResponseAlertTextBox"] = "!HTTP 403 Access Denied!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 403: Forbidden. Is your firewall or web proxy blocking Outlook connectivity?" + Environment.NewLine +
                                "To fire this message a HTTP 403 response code was detected and 'Access Denied' was found in the response body." + Environment.NewLine +
                                "Check the Raw and WebView tabs, do you see anything which indicates traffic is blocked?";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");
                            }
                        }
                        else
                        {
                            // All other HTTP 403's.
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";

                            this.session["X-ResponseAlertTextBox"] = "!HTTP 403 Forbidden!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "While HTTP 403's can be symptomatic of a proxy server blocking traffic, " +
                                "however the phrase 'Access Denied' was NOT detected in the response body." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "A small number of HTTP 403's can be seen in normal working scenarios. Check the Raw and WebView tabs to look for anything which looks suspect." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "If you are troubleshooting Free/Busy (Meeting availability info) or setting Out of Office messages then you may be more interested in these." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "See: https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd877045(v=exchg.140)";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 403 Forbidden.");
                            }
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

                        this.session["X-ResponseAlertTextBox"] = "!HTTP 404 Not Found!";
                        this.session["X-ResponseCommentsRichTextboxText"] = Properties.Settings.Default.HTTPQuantity;

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 404 Not found.");
                        }
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

                        this.session["X-ResponseAlertTextBox"] = "!HTTP 405: Method Not Allowed!";
                        this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 405: Method Not Allowed";

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 405 Method not allowed.");
                        }
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

                        this.session["X-ResponseAlertTextBox"] = "!HTTP 429 Too Many Requests!";
                        this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 429: These responses need to be taken into context with the rest of the sessions in the trace. " +
                            "A small number is probably not an issue, larger numbers of these could be cause for concern.";

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 429 Too many requests.");
                        }
                        //
                        /////////////////////////////
                        break;
                    case 440:
                        /////////////////////////////
                        //
                        // HTTP 440: Need to know more about these.
                        // For the moment do nothing.
                        //
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";

                        // Need comments.

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 440.");
                        }
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
                            this.session["X-ExchangeType"] = "!Multi-Factor Auth!";

                            this.session["X-ResponseAlertTextBox"] = "HTTP 456 Multi-Factor Authentication";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 429: See details on Raw tab. Look for the presence of 'you must use multi-factor authentication'." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "This has been seen where users have MFA enabled/enforced, but Modern Authentication is not enabled in Exchange Online" +
                                Environment.NewLine +
                                Environment.NewLine +
                                "See https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 456 Multi-Factor Required!");
                            }
                        }
                        else
                        {
                            this.session["ui-backcolor"] = HTMLColourOrange;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "Multi-Factor Auth?";

                            this.session["X-ResponseAlertTextBox"] = "!HTTP 456 Multi-Factor Authentication!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 429: See details on Raw tab.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 456 Multi-Factor Required.");
                            }
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

                        this.session["X-ResponseAlertTextBox"] = "!HTTP 500 Internal Server Error!";
                        this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 500 Internal Server Error";

                        if (bAppLoggingEnabled)
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

                            // Increment false positive count to prevent long running session overrides.
                            FalsePositive++;

                            this.session["X-ResponseAlertTextBox"] = "False Positive";
                            this.session["X-ResponseCommentsRichTextboxText"] = "Telemetry failing is unlikely the cause of Outlook / OWA connectivity or other issues.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. Telemetry False Positive.");
                            }
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

                            // Increment false positive count to prevent long running session overrides.
                            FalsePositive++;

                            this.session["X-ResponseAlertTextBox"] = "False Positive";
                            this.session["X-ResponseCommentsRichTextboxText"] = "From the data in the response body this failure is likely due to a Microsoft DNS MX record " +
                                Environment.NewLine +
                                "which points to an Exchange Online Protection mail host that accepts connections only on port 25. Connection on port 443 will not work by design." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "To validate this above lookup the record, confirm it is a MX record and attempt to connect to the MX host on ports 25 and 443.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. EXO DNS False Positive.");
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

                            // Increment false positive count to prevent long running session overrides.
                            FalsePositive++;

                            string AutoDFalsePositiveResponseBody = this.session.GetResponseBodyAsString();
                            int start = this.session.GetResponseBodyAsString().IndexOf("'");
                            int end = this.session.GetResponseBodyAsString().LastIndexOf("'");
                            int charcount = end - start;
                            string AutoDFalsePositiveDomain = AutoDFalsePositiveResponseBody.Substring(start, charcount).Replace("'", "");

                            this.session["X-ResponseAlertTextBox"] = "False Positive";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 502: False Positive. By design Office 365 Autodiscover does not respond to " +
                                AutoDFalsePositiveDomain + " on port 443. " +
                                Environment.NewLine +
                                Environment.NewLine +
                                "Validate this message by confirming this is an Office 365 Host/IP address and perform a telnet to it on port 80." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design redirects " +
                                "requests to https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. O365 AutoD onmicrosoft.com False Positive.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 4. Vanity domain points to Office 365 autodiscover; false positive.
                        //

                        /*
                        HTTP/1.1 502 Fiddler - Connection Failed
                        Date: Mon, 12 Nov 2018 09:47:06 GMT
                        Content-Type: text/html; charset=UTF-8
                        Connection: close
                        Cache-Control: no-cache, must-revalidate
                        Timestamp: 04:47:06.295

                        [Fiddler] The connection to 'autodiscover.contoso.com' failed. <br />Error: ConnectionRefused (0x274d). <br />
                        System.Net.Sockets.SocketException No connection could be made because the target machine actively refused it 40.97.100.8:443                                                                                                                                                                                                                                                                                  
                        */

                        else if ((this.session.utilFindInResponse("autodiscover.", false) > 1) &&
                                (this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                                (this.session.utilFindInResponse("40.97.", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourBlue;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "False Positive";

                            // Increment false positive count to prevent long running session overrides.
                            FalsePositive++;

                            this.session["X-ResponseAlertTextBox"] = "Office 365 Autodiscover False Positive";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 502: False Positive. By design Office 365 certain IP addresses used for " +
                                "Autodiscover do not respond on port 443. " +
                                Environment.NewLine +
                                Environment.NewLine +
                                "Validate this message by confirming this is an Office 365 Host/IP address and perform a telnet to it on port 80." +
                                Environment.NewLine +
                                Environment.NewLine +
                                "If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design " +
                                "redirects requests to https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. Vanity domain AutoD False Positive.");
                            }
                        }

                        /////////////////////////////
                        //
                        // 5. Anything else Exchange Autodiscover.
                        //
                        else if ((this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                            (this.session.utilFindInResponse("autodiscover", false) > 1) &&
                            (this.session.utilFindInResponse(":443", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-ExchangeType"] = "!AUTODISCOVER!";

                            this.session["X-ResponseAlertTextBox"] = "!AUTODISCOVER!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "Autodiscover request detected, which failed.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway. Exchange Autodiscover.");
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

                            this.session["X-ResponseAlertTextBox"] = "!HTTP 502 Bad Gateway!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "Potential to cause the issue you are investigating. " +
                                "Do you see expected responses beyond this session in the trace? Is this an Exchange On - Premise, Exchange Online or other device ?";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 502 Bad Gateway (99).");
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
                            this.session["X-ExchangeType"] = "!FEDERATION!";

                            string RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";
                            if (FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.DemoMode", false) == true)
                            {
                                RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=user@contoso.com&xml=1";
                            }

                            this.session["X-ResponseAlertTextBox"] = "!FederatedSTSUnreachable!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 503: FederatedSTSUnreachable." + Environment.NewLine +
                                "The fedeation service is unreachable or unavailable. Check the Raw tab for additional details." + Environment.NewLine +
                                "Check the realm page for the authenticating domain." + Environment.NewLine + RealmURL + Environment.NewLine + Environment.NewLine +
                                "Expected responses:" + Environment.NewLine +
                                "AuthURL: Normally expected to show federation service logon page." + Environment.NewLine +
                                "STSAuthURL: Normally expected to show HTTP 400." + Environment.NewLine +
                                "MEXURL: Normally expected to show long stream of XML data." + Environment.NewLine + Environment.NewLine +
                                "If any of these show the HTTP 503 Service Unavailable this confirms a consistent failure on the federation service." + Environment.NewLine +
                                "If however you get the expected responses, this does not neccessarily mean the federation service / everything authentication is healthy. Further investigation is advised.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 503 Service Unavailable. FederatedStsUnreachable in response body!");
                            }
                        }
                        /////////////////////////////
                        //
                        // 99. Everything else.
                        //
                        else
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";

                            this.session["X-ResponseAlertTextBox"] = "!HTTP 503 Service Unavailable!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "HTTP 503 Service Unavailable.";

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 503 Service Unavailable (99).");
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
                            this.session["X-ExchangeType"] = "!INTERNET BLOCKED!";

                            this.session["X-ResponseAlertTextBox"] = "!HTTP 504 Gateway Timeout -- Internet Access Blocked!";
                            this.session["X-ResponseCommentsRichTextboxText"] = "Detected the keywords 'internet' and 'access' and 'blocked'. Potentially the computer this trace was collected " +
                                "from has been quaratined for internet access on the customer's network." + Environment.NewLine + Environment.NewLine +
                                "Validate this by checking the webview and raw tabs for more information.";

                            if (bAppLoggingEnabled)
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

                            this.session["X-ResponseAlertTextBox"] = "!HTTP 504 Gateway Timeout!";
                            this.session["X-ResponseCommentsRichTextboxText"] = Properties.Settings.Default.HTTPQuantity;

                            if (bAppLoggingEnabled)
                            {
                                FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " HTTP 504 Gateway Timeout (99).");
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

                        this.session["X-ResponseAlertTextBox"] = "Undefined.";
                        this.session["X-ResponseCommentsRichTextboxText"] = "No specific information on this session in the EXO Fiddler Extension.";

                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Session undefined in extension.");
                        }
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
            // ColouriseSessionsOverrides

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

            int SlowRunningSessionThreshold = calledPreferences.GetSlowRunningSessionThreshold();


            // Very likely the first session captured when running Fiddler.
            if (this.session.hostname == "www.fiddler2.com")
            {
                this.session["ui-backcolor"] = HTMLColourGrey;
                this.session["ui-color"] = "black";
            }
            // If the local process is nullor blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS
            // So don't pay any attention to overrides for this type of traffic.
            else if ((this.session.LocalProcess == null) || (this.session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                // No overrides needed in this scenario.
            }
            // If the overall session time runs longer than 5,000ms or 5 seconds 
            //  AND this is not determined to be a false positive.
            else if (ClientMilliseconds > SlowRunningSessionThreshold && FalsePositive == 0)
            {
                this.session["ui-backcolor"] = HTMLColourRed;
                this.session["ui-color"] = "black";

                this.session["X-ExchangeType"] = "Long Running Session";

                this.session["X-ResponseAlertTextBox"] = "!Long Running Session!";
                this.session["X-ResponseCommentsRichTextboxText"] = "Long running session found. A small number of long running sessions in the < 10 " +
                    "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue.";

                if (bAppLoggingEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running session.");
                }
            }
            // If the EXO server think time runs longer than 5,000ms or 5 seconds 
            //  AND this is not determined to be a false positive.
            else if (ServerMilliseconds > SlowRunningSessionThreshold && FalsePositive == 0)
            {
                this.session["ui-backcolor"] = HTMLColourRed;
                this.session["ui-color"] = "black";

                this.session["X-ExchangeType"] = "Long Running EXO Session";

                this.session["X-ResponseAlertTextBox"] = "!Long Running EXO Session!";
                this.session["X-ResponseCommentsRichTextboxText"] = "Long running EXO session found. A small number of long running sessions in the < 10 " +
                    "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue.";

                if (bAppLoggingEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Long running EXO session.");
                }
            }
            else
            {
                //bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
                // If the menu item Highlight Outlook and OWA Only is enabled then grey out all the other traffic.
                if (bHighlightOutlookOWAOnlyEnabled)
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
            //
            /////////////////////////////
        }

        /// <summary>
        /// Function where the Response Server column is populated.
        /// </summary>
        /// <param name="session"></param>
        public void SetResponseServer(Session session)
        {
            this.session = session;

            // Populate Response Server on session in order of preference from common to obsure.

            // If the response server header is not null or blank then populate it into the response server value.
            if ((this.session.oResponse["Server"] != null) && (this.session.oResponse["Server"] != ""))
            {
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
            else if ((this.session.oResponse["X-Powered-By"] != null) && (this.session.oResponse["X-Powered-By"] != ""))
            {
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

        /// <summary>
        /// Function where the Exchange Type column is populated.
        /// </summary>
        /// <param name="session"></param>
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
                //calledColouriseWebSessions.IncrementHTTP200FreeBusyCount();
            }
            else if (this.session.fullUrl.Contains("GetUserAvailability"))
            {
                this.session["X-ExchangeType"] = "Free/Busy";
                // Increment HTTP200FreeBusy counter to assist with session classification further on down the line.
                //calledColouriseWebSessions.IncrementHTTP200FreeBusyCount();
            }
            else if (this.session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                this.session["X-ExchangeType"] = "Free/Busy";
                // Increment HTTP200FreeBusy counter to assist with session classification further on down the line.
                //calledColouriseWebSessions.IncrementHTTP200FreeBusyCount();
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
            // First off if the local process is null or blank, then we are analysing traffic from a remote client such as a mobile device.
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

        /// <summary>
        /// Used specifically for Authentication sessions.
        /// Inclusion of '"' may not be compatible with say HTTP 503 response body word split.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="searchTerm"></param>
        /// <returns>wordCount</returns>
        public int SearchSessionForWord(Session session, string searchTerm)
        {
            this.session = session;

            // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
            //
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
            //

            string text = this.session.ToString();

            //Convert the string into an array of words  
            string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '"' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                             select word;

            // Count the matches, which executes the query.  
            int wordCount = matchQuery.Count();

            //MessageBox.Show(this.session.id + " " + searchTerm + " " + wordCount);

            return wordCount;
        }

        public void SAMLParserFieldsNoData()
        {
            this.session["X-Issuer"] = "No SAML Data in session";
            this.session["X-AttributeNameUPNTextBox"] = "No SAML Data in session";
            this.session["X-NameIdentifierFormatTextBox"] = "No SAML Data in session";
            this.session["X-AttributeNameImmutableIDTextBox"] = "No SAML Data in session";
        }

        /// <summary>
        /// Set Authentication column values.
        /// </summary>
        /// <param name="session"></param>
        public void SetAuthentication(Session session)
        {
            Boolean OverrideFurtherAuthChecking = false;

            List<string> calledDeveloperList = calledPreferences.GetDeveloperList();
            Boolean DeveloperDemoMode = calledPreferences.GetDeveloperMode();
            Boolean DeveloperDemoModeBreakScenarios = calledPreferences.GetDeveloperDemoModeBreakScenarios();

            this.session["X-Office365AuthType"] = "";

            this.session = session;

            // Determine if this session contains a SAML response.
            if (this.session.utilFindInResponse("Issuer=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1 &&
                this.session.utilFindInResponse("NameIdentifier Format=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1)
            {
                // Used in Auth column and Office365 Auth inspector tab.
                this.session["X-Authentication"] = "SAML Request/Response";
                this.session["X-AuthenticationDesc"] = "See below for SAML response parser.";

                // Change which control appears for this session on the Office365 Auth inspector tab.
                this.session["X-Office365AuthType"] = "SAMLResponseParser";

                // Error handling, if we don't have the expected values in the session body, don't do this work.
                // Avoid null object reference errors at runtime.
                if ((this.session.utilFindInResponse("Issuer=", false) > 1) && (this.session.utilFindInResponse("IssueInstant=", false) > 1)) 
                {
                    if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
                    {
                        this.session["X-Issuer"] = "Issuer = \"http://sts.contoso.com/adfs/services/trust\"";
                    }
                    else
                    {
                        // Pull issuer data from response.
                        string IssuerSessionBody = this.session.ToString();
                        int IssuerStartIndex = IssuerSessionBody.IndexOf("Issuer=");
                        int IssuerEndIndex = IssuerSessionBody.IndexOf("IssueInstant=");
                        int IssuerLength = IssuerEndIndex - IssuerStartIndex;
                        string Issuer = IssuerSessionBody.Substring(IssuerStartIndex, IssuerLength);
                        Issuer = Issuer.Replace("&quot;", "\"");

                        // Populate X flag on session.
                        this.session["X-Issuer"] = Issuer;
                    }
                }
                else
                {
                    this.session["X-Issuer"] = "Data points not found for issuer";
                }

                // Pull the x509 signing certificate data.
                if ((this.session.utilFindInResponse("&lt;X509Certificate>", false) > 1) && (this.session.utilFindInResponse("&lt;/X509Certificate>", false) > 1))
                {
                    if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
                    {
                        // portal.office.com certificate for demo.
                        this.session["X-SigningCertificate"] = "-----BEGIN CERTIFICATE-----" +
                            "MIIJyTCCB7GgAwIBAgITFgAC+95Ht0cIYnGqEwAAAAL73jANBgkqhkiG9w0BAQsF" +
                            "ADCBizELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcT" +
                            "B1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEVMBMGA1UE" +
                            "CxMMTWljcm9zb2Z0IElUMR4wHAYDVQQDExVNaWNyb3NvZnQgSVQgVExTIENBIDQw" +
                            "HhcNMTgwOTI0MjExNTQwWhcNMjAwOTI0MjExNTQwWjArMSkwJwYDVQQDEyBzdGFt" +
                            "cDIubG9naW4ubWljcm9zb2Z0b25saW5lLmNvbTCCASIwDQYJKoZIhvcNAQEBBQAD" +
                            "ggEPADCCAQoCggEBAKXmyCmQ9dko2PQkmJE1Rd4oEE92VcoYWTKqnoiKfxz4yNAY" +
                            "ATTLWyKH+SId+YeQw/aqVIwFZbeuAUocpWszyisOAEh76cgc7nZgh9mqzaMBVClb" +
                            "VVoTNvhVoauh1ovbZ6yDOJhXgVDP2NODJKxi7ThbpfJwCXt78OzI+Z0EvzpdZxk9" +
                            "PAVAveXf+bHRaD7ctsvyheuOjE/fVkUTotBppLsrKadc5mO+nvi6RYvO0h+ExLYL" +
                            "WKLBtFXOB9xo85b2CxnuMoRGVDKWI+H3HYddKCC3EldFHHj2TOA/y0otK3xHFnNg" +
                            "AMpKowBBBZJziUXw611AfGqKZVQE4Rzwoe5vxRsCAwEAAaOCBYMwggV/MIIB9gYK" +
                            "KwYBBAHWeQIEAgSCAeYEggHiAeAAdgCkuQmQtBhYFIe7E6LMZ3AKPDWYBPkb37jj" +
                            "d80OyA3cEAAAAWYNeUqlAAAEAwBHMEUCIQCPEAlU1m5COhI5vsyrbPIXgpCWjj2L" +
                            "uXKm+xI8eHPxGQIgDx3QiH5hCQB+jc6h12fuY8GzLOk1kzb8oDMRZ3FyeD0AdgC7" +
                            "2d+8H4pxtZOUI5eqkntHOFeVCqtS6BqQlmQ2jh7RhQAAAWYNeUvwAAAEAwBHMEUC" +
                            "IQCaTXkt9/yf1PLY9o8k1lYbBxmKJzxxXFSQCNt0n9HbNwIgRRCY4Ge+DMCOKtqZ" +
                            "2dSPY+WdEiimhily0qjQaDr451sAdgBWFAaaL9fC7NP14b1Esj7HRna5vJkRXMDv" +
                            "lJhV1onQ3QAAAWYNeUtnAAAEAwBHMEUCIGt3YAa3D4OQLD7Jne2CZp1W/NVW9AQs" +
                            "ZVJKl97FXF7jAiEAtGM91DzsBYv4Udh3QtI0FouhQ4rUZ0TSPwrjTcn+XIQAdgBe" +
                            "p3P531bA57U2SH3QSeAyepGaDIShEhKEGHWWgXFFWAAAAWYNeUuCAAAEAwBHMEUC" +
                            "IHcT7a0iybDc0TFFQWi6cWLzTyfILxjWFnEJgD44j/paAiEA6dKc86wqUV+xGyCb" +
                            "CM1BqR2IwmrlxtAiByG956kduYUwJwYJKwYBBAGCNxUKBBowGDAKBggrBgEFBQcD" +
                            "AjAKBggrBgEFBQcDATA+BgkrBgEEAYI3FQcEMTAvBicrBgEEAYI3FQiH2oZ1g+7Z" +
                            "AYLJhRuBtZ5hhfTrYIFdhNLfQoLnk3oCAWQCAR0wgYUGCCsGAQUFBwEBBHkwdzBR" +
                            "BggrBgEFBQcwAoZFaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraS9tc2NvcnAv" +
                            "TWljcm9zb2Z0JTIwSVQlMjBUTFMlMjBDQSUyMDQuY3J0MCIGCCsGAQUFBzABhhZo" +
                            "dHRwOi8vb2NzcC5tc29jc3AuY29tMB0GA1UdDgQWBBSUvDb8x1PhIWg2aXbaJeBh" +
                            "RQEjoTALBgNVHQ8EBAMCBLAwggEmBgNVHREEggEdMIIBGYIZbG9naW4ubWljcm9z" +
                            "b2Z0b25saW5lLmNvbYIbbG9naW4ubWljcm9zb2Z0b25saW5lLXAuY29tghtsb2dp" +
                            "bmV4Lm1pY3Jvc29mdG9ubGluZS5jb22CGmxvZ2luMi5taWNyb3NvZnRvbmxpbmUu" +
                            "Y29tgiRzdGFtcDIubG9naW4ubWljcm9zb2Z0b25saW5lLWludC5jb22CHWxvZ2lu" +
                            "Lm1pY3Jvc29mdG9ubGluZS1pbnQuY29tgh9sb2dpbmV4Lm1pY3Jvc29mdG9ubGlu" +
                            "ZS1pbnQuY29tgh5sb2dpbjIubWljcm9zb2Z0b25saW5lLWludC5jb22CIHN0YW1w" +
                            "Mi5sb2dpbi5taWNyb3NvZnRvbmxpbmUuY29tMIGsBgNVHR8EgaQwgaEwgZ6ggZug" +
                            "gZiGS2h0dHA6Ly9tc2NybC5taWNyb3NvZnQuY29tL3BraS9tc2NvcnAvY3JsL01p" +
                            "Y3Jvc29mdCUyMElUJTIwVExTJTIwQ0ElMjA0LmNybIZJaHR0cDovL2NybC5taWNy" +
                            "b3NvZnQuY29tL3BraS9tc2NvcnAvY3JsL01pY3Jvc29mdCUyMElUJTIwVExTJTIw" +
                            "Q0ElMjA0LmNybDBNBgNVHSAERjBEMEIGCSsGAQQBgjcqATA1MDMGCCsGAQUFBwIB" +
                            "FidodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL21zY29ycC9jcHMwHwYDVR0j" +
                            "BBgwFoAUenuMwc/noMoc1Gv6++Ezww8aop0wHQYDVR0lBBYwFAYIKwYBBQUHAwIG" +
                            "CCsGAQUFBwMBMA0GCSqGSIb3DQEBCwUAA4ICAQAKbW1c/c8p8Y6F79AxCcVV8DRq" +
                            "kndFgBans8sOJmOVvLurpIsMvd4C6JKrB6yDK8fYxS5PtwQDVXW6b2C6EDPUrOQm" +
                            "4Fkj4hApQMOyOxKcaUnfRDLZqEpbZ4oIxQ2rnxY9yEmegHBJ4+5qnlTLY+hlODpK" +
                            "oiTkNKSpwj7rzIBnhTTSW4E2TI9RiG1KgviiJFVDdLQKH4/aPou0YBUXf6JNLX6X" +
                            "wFFKWm/AYHZ9E4W97AQ7BQw9fvEZ0uE8bmsV6Y1dJrFl3/KmxDYDyJ4nFPhY0vHR" +
                            "Kb9/H9/W6qb++j0zGejSGeVSmj/Xr1Y3Py9BL9unkKHx77ERycJS0WQGMDA7BEju" +
                            "sZI1MQhzQe+vm/5Kn68ETPC1bI7o370wluf6ZoRHYPJtpD8WBceamoCMALUnyKe2" +
                            "RzD8ZMOY0L8VHr0b8hNcCJaCRpiAGxkmbGu3v/dHRQ9YVddZa+7ROYH4teFhs7Bp" +
                            "ffVM+zeHhD/oJ2q1iMKwhvXUL5aiBOkg+TpZC4YrWsNeNPdMIOKTMkR0yi3z1WSF" +
                            "xxTfQPX0tmTagQKFUv3fATLnY47gt4UfUgOeGyfkV7r4K3clO/Vyj840fzCqtlro" +
                            "vXcEStLU744fvnFYyvwvJCY2NRN7ByFKoPaG8E5tto7eYmyZbYd5SyIZ5X+1V+N5" +
                            "8KdwU29EM46onpLRnQ==" +
                            "-----END CERTIFICATE-----";
                    }
                    else
                    {
                        string x509SigningCertSessionBody = this.session.ToString();
                        int x509SigningCertificateStartIndex = x509SigningCertSessionBody.IndexOf("&lt;X509Certificate>") + 20; // 20 to shift to start of the selection.
                        int x509SigningCertificateEndIndex = x509SigningCertSessionBody.IndexOf("&lt;/X509Certificate>");
                        int x509SigningCertificateLength = x509SigningCertificateEndIndex - x509SigningCertificateStartIndex;
                        string x509SigningCertificate = x509SigningCertSessionBody.Substring(x509SigningCertificateStartIndex, x509SigningCertificateLength);

                        this.session["X-SigningCertificate"] = x509SigningCertificate;
                    }
                }
                

                // Error handling, if we don't have the expected values in the session body, don't do this work.
                // Avoid null object reference errors at runtime.
                if ((this.session.utilFindInResponse("&lt;saml:Attribute AttributeName=&quot;UPN", false) > 1) && 
                    (this.session.utilFindInResponse("&lt;/saml:Attribute>", false) > 1))
                {
                    if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
                    {
                        this.session["X-AttributeNameUPNTextBox"] = "<saml:Attribute AttributeName=\"UPN\"" +
                            "AttributeNamespace=\"http://schemas.xmlsoap.org/claims\">" +
                            "<saml:AttributeValue>user@contoso.com</saml:AttributeValue></saml:Attribute>";
                    }
                    else
                    {
                        // AttributeNameUPN.
                        string AttributeNameUPNSessionBody = this.session.ToString();
                        int AttributeNameUPNStartIndex = AttributeNameUPNSessionBody.IndexOf("&lt;saml:Attribute AttributeName=&quot;UPN");
                        int AttributeNameUPNEndIndex = AttributeNameUPNSessionBody.IndexOf("&lt;/saml:Attribute>");
                        int AttributeNameUPNLength = AttributeNameUPNEndIndex - AttributeNameUPNStartIndex;
                        string AttributeNameUPN = AttributeNameUPNSessionBody.Substring(AttributeNameUPNStartIndex, AttributeNameUPNLength);
                        AttributeNameUPN = AttributeNameUPN.Replace("&quot;", "\"");
                        AttributeNameUPN = AttributeNameUPN.Replace("&lt;", "<");
                        // Now split the two lines with a new line for easier reading in the user control.
                        int SplitAttributeNameUPNStartIndex = AttributeNameUPN.IndexOf("><") + 1;
                        string AttributeNameUPNFirstLine = AttributeNameUPN.Substring(0, SplitAttributeNameUPNStartIndex);
                        string AttributeNameUPNSecondLine = AttributeNameUPN.Substring(SplitAttributeNameUPNStartIndex);
                        AttributeNameUPN = AttributeNameUPNFirstLine + Environment.NewLine + AttributeNameUPNSecondLine;

                        // Populate X flag on session.
                        this.session["X-AttributeNameUPNTextBox"] = AttributeNameUPN;
                    }
                }
                else
                {
                    this.session["X-AttributeNameUPNTextBox"] = "Data points not found for AttributeNameUPN";
                }


                if ((this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                    (this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
                {
                    if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
                    {
                        this.session["X-NameIdentifierFormatTextBox"] = "<saml:NameIdentifier Format=\"urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified\">" +
                            "+qwerty123456789qwerty==</saml:NameIdentifier>";
                    }
                    else
                    {
                        // NameIdentifierFormat.
                        string NameIdentifierFormatSessionBody = this.session.ToString();
                        int NameIdentifierFormatStartIndex = NameIdentifierFormatSessionBody.IndexOf("&lt;saml:NameIdentifier Format");
                        int NameIdentifierFormatEndIndex = NameIdentifierFormatSessionBody.IndexOf("&lt;saml:SubjectConfirmation>");
                        int NameIdentifierFormatLength = NameIdentifierFormatEndIndex - NameIdentifierFormatStartIndex;
                        string NameIdentifierFormat = NameIdentifierFormatSessionBody.Substring(NameIdentifierFormatStartIndex, NameIdentifierFormatLength);
                        NameIdentifierFormat = NameIdentifierFormat.Replace("&quot;", "\"");
                        NameIdentifierFormat = NameIdentifierFormat.Replace("&lt;", "<");

                        // Populate X flag on session.
                        this.session["X-NameIdentifierFormatTextBox"] = NameIdentifierFormat;
                    } 
                }
                else
                {
                    this.session["X-NameIdentifierFormatTextBox"] = "Data points not found for NameIdentifierFormat";
                }

                if ((this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                    (this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
                {
                    if (calledDeveloperList.Any(Environment.UserName.Contains) && DeveloperDemoMode == true)
                    {
                        this.session["X-AttributeNameImmutableIDTextBox"] = "<saml:Attribute AttributeName=\"ImmutableID\" " +
                            "AttributeNamespace=\"http://schemas.microsoft.com/LiveID/Federation/2008/05\">" +
                            "<saml:AttributeValue>+qwerty123456789qwerty==</saml:AttributeValue>";
                    }
                    else
                    {
                        // AttributeNameImmutableID.
                        string AttributeNameImmutableIDSessionBody = this.session.ToString();
                        int AttributeNameImmutableIDStartIndex = AttributeNameImmutableIDSessionBody.IndexOf("AttributeName=&quot;ImmutableID");
                        int AttributeNameImmutibleIDEndIndex = AttributeNameImmutableIDSessionBody.IndexOf("&lt;/saml:AttributeStatement>");
                        int AttributeNameImmutibleIDLength = AttributeNameImmutibleIDEndIndex - AttributeNameImmutableIDStartIndex;
                        string AttributeNameImmutibleID = AttributeNameImmutableIDSessionBody.Substring(AttributeNameImmutableIDStartIndex, AttributeNameImmutibleIDLength);
                        AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&quot;", "\"");
                        AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&lt;", "<");
                        // Now split out response with a newline for easier reading.
                        int SplitAttributeNameImmutibleIDStartIndex = AttributeNameImmutibleID.IndexOf("<saml:AttributeValue>") + 21; // Add 21 characters to shift where the newline is placed.
                        string AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                        string AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                        AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;
                        // Second split
                        SplitAttributeNameImmutibleIDStartIndex = AttributeNameImmutibleID.IndexOf("</saml:AttributeValue></saml:Attribute>");
                        AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                        AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                        AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;

                        // Populate X flag on session.
                        this.session["X-AttributeNameImmutableIDTextBox"] = AttributeNameImmutibleID;
                    } 
                }
                else
                {
                    this.session["X-AttributeNameImmutableIDTextBox"] = "Data points not found for AttributeNameImmutibleID";
                }
            }
            // Determine if Modern Authentication is enabled in Exchange Online.
            else if (this.session.oRequest["Authorization"] == "Bearer" || this.session.oRequest["Authorization"] == "Basic")
            {
                SAMLParserFieldsNoData();

                // Change which control appears for this session on the Office365 Auth inspector tab.
                this.session["X-Office365AuthType"] = "Office365Auth";

                // Looking for the following in a response body:

                // x-ms-diagnostics: 4000000;reason="Flighting is not enabled for domain 'user@contoso.com'.";error_category="oauth_not_available"

                int KeywordFourMillion = SearchSessionForWord(this.session, "4000000");
                int KeywordFlighting = SearchSessionForWord(this.session, "Flighting");
                int Keywordenabled = SearchSessionForWord(this.session, "enabled");
                int Keyworddomain = SearchSessionForWord(this.session, "domain");
                int Keywordoauth_not_available = SearchSessionForWord(this.session, "oauth_not_available");

                // Check if all the above checks have a value of at least 1. 
                // If they do, then Exchange Online is configured with Modern Authentication disabled.
                if (KeywordFourMillion > 0 && KeywordFlighting > 0 && Keywordenabled > 0 &&
                    Keyworddomain > 0 && Keywordoauth_not_available > 0 && this.session.HostnameIs("autodiscover-s.outlook.com"))
                {
                    this.session["X-Authentication"] = "EXO Modern Auth Disabled";

                    this.session["X-AuthenticationDesc"] = "EXO Modern Auth Disabled" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Exchange Online has Modern Authentication disabled. " +
                        "This is not necessarily a bad thing, but something to make note of during troubleshooting." +
                        Environment.NewLine +
                        "MutiFactor Authentication will not work as expected while Modern Authentication " +
                        "is disabled in Exchange Online" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Outlook 2010 and older do not support Modern Authentication and by extension MutliFactor Authentication." +
                        Environment.NewLine +
                        "Outlook 2013 supports modern authentication with updates and the EnableADAL registry key set to 1." +
                        Environment.NewLine +
                        "See https://support.microsoft.com/en-us/help/4041439/modern-authentication-configuration-requirements-for-transition-from-o" +
                        Environment.NewLine +
                        "Outlook 2016 or newer. No updates or registry keys needed for Modern Authentication.";

                    // Set the OverrideFurtherAuthChecking to true; EXO Modern Auth Disabled is a more important message in these sessions,
                    // than Outlook client auth capabilities. Other sessions are expected to show client auth capabilities.
                    OverrideFurtherAuthChecking = true;

                    if (bAppLoggingEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " EXO Modern Auth Disabled.");
                    }
                }
                else
                {
                    // Do nothing right now.
                }

                // Now get specific to find out what the client can do.
                // If the session request header Authorization equals Bearer this is a Modern Auth capable client.
                // Note OverrideFurtherAuthChecking which is set above if we detected EXO has Modern Auth disabled.
                if (this.session.oRequest["Authorization"] == "Bearer" && !(OverrideFurtherAuthChecking))
                {
                    this.session["X-Authentication"] = "Outlook Modern Auth";

                    this.session["X-AuthenticationDesc"] = "Outlook Modern Auth" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Outlook is stating it can do Modern Authentication. " +
                        "Whether it is used or not will depend on whether Modern Authentication is enabled in Exchange Online.";

                    if (bAppLoggingEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Outlook Modern Auth.");
                    }
                }
                // If the session request header Authorization equals Basic this is a Basic Auth capable client.
                // Note OverrideFurtherAuthChecking which is set above if we detected EXO has Modern Auth disabled.
                else if (this.session.oRequest["Authorization"] == "Basic" && !(OverrideFurtherAuthChecking))
                {
                    this.session["X-Authentication"] = "Outlook Basic Auth";

                    this.session["X-AuthenticationDesc"] = "Outlook Basic Auth" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Outlook is stating it can do Basic Authentication. " +
                        "Whether or not Modern Authentication is enabled in Exchange Online this client session will use Basic Authentication." +
                        Environment.NewLine +
                        "In all likelihood this is an Outlook 2013 (updated prior to Modern Auth), Outlook 2010 or an older Outlook client, " +
                        "which does not support Modern Authentication." +
                        "MutiFactor Authentication will not work as expected with Basic Authentication only capable Outlook clients";

                    if (bAppLoggingEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Outlook Basic Auth.");
                    }
                }
            }
            // Now we can check for Authorization headers which contain Bearer or Basic, signifying security tokens are being passed
            // from the Outlook client to Office 365 for resource access.
            //
            // Bearer == Modern Authentication.
            else if (this.session.oRequest["Authorization"].Contains("Bearer"))
            {
                SAMLParserFieldsNoData();

                this.session["X-Authentication"] = "Modern Auth Token";

                this.session["X-AuthenticationDesc"] = "Modern Auth Token" +
                        Environment.NewLine +
                        Environment.NewLine +
                        "Outlook accessing resources with a Modern Authentication security token.";

                if (bAppLoggingEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Modern Auth Token.");
                }
            }
            // Basic == Basic Authentication.
            else if (this.session.oRequest["Authorization"].Contains("Basic"))
            {
                SAMLParserFieldsNoData();

                this.session["X-Authentication"] = "Basic Auth Token";

                this.session["X-AuthenticationDesc"] = "Basic Auth Token" +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Outlook accessing resources with a Basic Authentication security token.";

                if (bAppLoggingEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: " + this.session.id + " Basic Auth Token.");
                }
            }
            else
            {
                SAMLParserFieldsNoData();
                // Change which control appears for this session on the Office365 Auth inspector tab.
                this.session["X-Office365AuthType"] = "Office365Auth";

                this.session["X-Authentication"] = "--No Auth Headers";
                this.session["X-AuthenticationDesc"] = "--No Auth Headers";
            }
        }
    }
}