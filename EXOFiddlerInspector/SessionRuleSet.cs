using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

namespace EXOFiddlerInspector
{
    /// <summary>
    /// SessionRuleSet class. All extension session logic lives here.
    /// </summary>
    public class SessionRuleSet : IAutoTamper
    {
        // References to other classes.
        MenuUI calledMenuUI = new MenuUI();
        ColumnsUI calledColumnsUI = new ColumnsUI();
        Preferences calledPreferences = new Preferences();

        internal Session session { get; set; }

        private string searchTerm;
        private string RedirectAddress;
        private int HTTP200SkipLogic;
        private int HTTP200FreeBusy;

        public bool bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public bool bElapsedTimeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ElapsedTimeColumnEnabled", false);
        public bool bResponseServerColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ResponseServerColumnEnabled", false);
        public bool bExchangeTypeColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ExchangeTypeColumnEnabled", false);
        public bool bXHostIPColumnEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.XHostIPColumnEnabled", false);
        public bool bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);
        public bool bHighlightOutlookOWAOnlyEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.HighlightOutlookOWAOnlyEnabled", false);
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

        public void AutoTamperResponseAfter(Session oSession)
        {
            // Not used here.
            //throw new NotImplementedException();
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
                    "Beyond this, the customer needs their web administrator responsible for the server answering the calls to stop the Apache web server from answering to Autodiscover.";

                if (bAppLoggingEnabled)
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
                this.session["X-ExchangeType"] = "Not Exchange";
            }
            // If the local process is nullor blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS
            // So don't pay any attention to overrides for this type of traffic.
            else if ((this.session.LocalProcess == null) || (this.session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                // No overrides needed in this scenario.
            }
            else if (ClientMilliseconds > SlowRunningSessionThreshold)
            {
                this.session["ui-backcolor"] = HTMLColourRed;
                this.session["ui-color"] = "black";
            }
            else if (ServerMilliseconds > SlowRunningSessionThreshold)
            {
                this.session["ui-backcolor"] = HTMLColourRed;
                this.session["ui-color"] = "black";
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
    }
}