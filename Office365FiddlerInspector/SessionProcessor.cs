using System;
using Fiddler;
using System.Linq;
using Office365FiddlerInspector.Services;

namespace Office365FiddlerInspector
{
    public class SessionProcessor : ActivationService
    {
        private static SessionProcessor _instance;

        public static SessionProcessor Instance => _instance ?? (_instance = new SessionProcessor());

        private bool IsInitialized { get; set; }

        internal Session session { get; set; }

        private string searchTerm;     
       
        public SessionProcessor() {}

        public void Initialize()
        {
            // Stop HandleLoadSaz and further processing if the extension is not enabled.
            if (!Preferences.ExtensionEnabled)
                return;

            FiddlerApplication.OnLoadSAZ += HandleLoadSaz;

            FiddlerApplication.OnSaveSAZ += HandleSaveSaz;

            if (!IsInitialized)
            {
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Custom", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Comments", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Content-Type", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Caching", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Body", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("URL", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Protocol", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Elapsed Time", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Session Type", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Authentication", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host IP", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Server", 2, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Result", 1, -1);
                FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("#", 0, -1);

                IsInitialized = true;
            }
        }

        // Function to handle saving a SAZ file.
        private void HandleSaveSaz(object sender, FiddlerApplication.WriteSAZEventArgs e)
        {
            #region SaveSAZ

            // Remove the session flags the extension adds to save space in the file and
            // mitigate errors thrown when loading a SAZ file which was saved with the extension enabled.
            // https://github.com/jprknight/Office365FiddlerExtension/issues/45

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            foreach (var session in e.arrSessions)
            {
                session.oFlags.Remove("UI-BACKCOLOR");
                session.oFlags.Remove("UI-COLOR");
                session.oFlags.Remove("X-SESSIONTYPE");
                session.oFlags.Remove("X-ATTRIBUTENAMEIMMUTABLEID");
                session.oFlags.Remove("X-ATTRIBUTENAMEUPN");
                session.oFlags.Remove("X-AUTHENTICATION");
                session.oFlags.Remove("X-AUTHENTICATIONDESC");
                session.oFlags.Remove("X-ELAPSEDTIME");
                session.oFlags.Remove("X-RESPONSESERVER");
                session.oFlags.Remove("X-ISSUER");
                session.oFlags.Remove("X-NAMEIDENTIFIERFORMAT");
                session.oFlags.Remove("X-OFFICE365AUTHTYPE");
                session.oFlags.Remove("X-PROCESSNAME");
                session.oFlags.Remove("X-RESPONSEALERT");
                session.oFlags.Remove("X-RESPONSECOMMENTS");
                session.oFlags.Remove("X-RESPONSECODEDESCRIPTION");
                session.oFlags.Remove("X-DATAAGE");
                session.oFlags.Remove("X-DATACOLLECTED");
                session.oFlags.Remove("X-SERVERTHINKTIME");
                session.oFlags.Remove("X-TRANSITTIME");
            }

            FiddlerApplication.UI.lvSessions.EndUpdate();
            #endregion
        }

        // Function to handle loading a SAZ file.
        private void HandleLoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
        {
            #region LoadSAZ

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            Preferences.IsLoadSaz = true;
            
            MenuUI.Instance.MiEnabled.Checked = Preferences.ExtensionEnabled;

            foreach (var session in e.arrSessions)
            {
                
                if (Preferences.ExtensionEnabled)
                {
                    SessionProcessor.Instance.SetElapsedTime(session);

                    SessionProcessor.Instance.SetResponseServer(session);

                    SessionProcessor.Instance.SetAuthentication(session);

                    SessionProcessor.Instance.SetSessionType(session);

                    SessionProcessor.Instance.CalculateSessionAge(session);

                    SessionProcessor.Instance.SetInspectorElapsedTime(session);

                    SessionProcessor.Instance.SetServerThinkTime(session);

                    SessionProcessor.Instance.SetTransitTime(session);

                    SessionProcessor.Instance.OnPeekAtResponseHeaders(session);

                    session.RefreshUI();
                }
            }
            FiddlerApplication.UI.lvSessions.EndUpdate();
            #endregion
        }


        public void OnPeekAtResponseHeaders(Session session)
        {
            this.session = session;

            // Various response code logic checks will set this to true.
            // This will stop any overrides firing on a session, which may not provide as much value to troubleshooting.
            bool SkipFurtherProcessing = false;

            // Colour codes for sessions. Softer tones, easier on the eye than standard red, orange and green.
            string HTMLColourBlue = "#81BEF7";
            string HTMLColourGreen = "#81F7BA";
            string HTMLColourRed = "#F06141";
            string HTMLColourGrey = "#BDBDBD";
            string HTMLColourOrange = "#F59758";

            // Decode session requests/responses.
            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            // Code section containing broad logic checks on sessions regardless of response code.
            #region BroadLogicChecks
            /////////////////////////////
            //
            //  Broader logic checks, where the response code cannot or should not be used as in the switch statement.
            //

            // Very likely the first session captured when running Fiddler.
            if (this.session.hostname == "www.fiddler2.com")
            {
                this.session["ui-backcolor"] = HTMLColourGrey;
                this.session["ui-color"] = "black";

                this.session["X-SessionType"] = "Fiddler Update Check";

                this.session["X-ResponseAlert"] = "Fiddler Update Check";

                this.session["X-ResponseComments"] = "This is Fiddler itself checking for updates. It has nothing to do with the Office 365 Fiddler Extension.";

                return;
            }

            /////////////////////////////
            //
            // Connect Tunnel.
            //
            // Check for connect tunnel with no usable data in the response body.
            //
            if (this.session.isTunnel) {

                // Trying to check session response body for a string value using !this.session.bHasResponse does not impact performance, but is not reliable.
                // Using this.session.GetResponseBodyAsString().Length == 0 kills performance. Fiddler wouldn't even load with this code in place.
                // Ideally looking to do: if (this.session.utilFindInResponse("CONNECT tunnel, through which encrypted HTTPS traffic flows", false) > 1)
                // Only works reliably when loading a SAZ file and request/response data is immediately available to do logic checks against.

                // Different code paths based on whether the session has been loaded from a SAZ file or not.

                // If this session was loaded from a SAZ file, check it for usable data in the response body.
                // If not usable data is found, mark it up and return. No further processing needed.
                if (Preferences.IsLoadSaz)
                {
                    if (this.session.utilFindInResponse("CONNECT tunnel, through which encrypted HTTPS traffic flows", false) > 1)
                    {
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";

                        this.session["X-SessionType"] = "Connect Tunnel";

                        this.session["X-ResponseAlert"] = "Connect Tunnel";
                        this.session["X-ResponseComments"] = "This is an encrypted tunnel. If all or most of the sessions are connect tunnels "
                            + "the sessions collected did not have decryption enabled. Setup Fiddler to 'Decrypt HTTPS traffic', click Tools -> Options -> HTTPS tab."
                            + "<p>If in any doubt see instructions at https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/DecryptHTTPS. </p>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " has connect tunnel in response.");

                        return;
                    }
                }
                // Otherwise this is a live data collection.
                // Don't bother trying to check the response body, it isn't reliable (notes above).
                // Mark it up anyway. In all likelihood code below in overrides will alter the session headers to replace this data.
                else
                {
                    this.session["ui-backcolor"] = HTMLColourOrange;
                    this.session["ui-color"] = "black";

                    this.session["X-SessionType"] = "Connect Tunnel";

                    this.session["X-ResponseAlert"] = "Connect Tunnel";
                    this.session["X-ResponseComments"] = "This is an encrypted tunnel. If all or most of the sessions are connect tunnels "
                        + "the sessions collected did not have decryption enabled. Setup Fiddler to 'Decrypt HTTPS traffic', click Tools -> Options -> HTTPS tab."
                        + "<p>If in any doubt see instructions at https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/DecryptHTTPS. </p>";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtention: " + this.session.id + " is a connect tunnel.");
                }
            }

            /////////////////////////////
            //
            // From a scenario where Apache Web Server found to be answering Autodiscover calls and throwing HTTP 301 & 405 responses.
            //
            if ((this.session.url.Contains("autodiscover") && (this.session.oResponse["server"].Contains("Apache"))))
            {
                this.session["ui-backcolor"] = HTMLColourRed;
                this.session["ui-color"] = "black";

                this.session["X-ResponseAlert"] = "Apache is answering Autodiscover requests!";
                this.session["X-ResponseComments"] = "An Apache Web Server(Unix/Linux) is answering Autodiscover requests!"
                    + "<p>This should not be happening. Consider disabling Root Domain Autodiscover lookups.</p>"
                    + "<p>See ExcludeHttpsRootDomain on </p>"
                    + "<p>https://support.microsoft.com/en-us/help/2212902/unexpected-autodiscover-behavior-when-you-have-registry-settings-under </p>"
                    + "<p>Beyond this the web administrator responsible for the server needs to stop the Apache web server from answering these requests.</p>";

                this.session["X-SessionType"] = "!APACHE AUTODISCOVER!";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Apache is answering Autodiscover requests! Investigate this first!");
                return;
            }
            #endregion

            // Code section containing switch statement for response code logic.
            // https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
            #region ResponseCodeLogic
            switch (this.session.responseCode)
            {
                #region HTTP0
                case 0:
                    /////////////////////////////
                    //
                    //  HTTP 0: No Response.
                    this.session["ui-backcolor"] = HTMLColourRed;
                    this.session["ui-color"] = "black";
                    this.session["X-SessionType"] = "!NO RESPONSE!";

                    this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 0 - No Response</span></b>";
                    
                    this.session["X-ResponseComments"] = "The quantity of these types of server errors need to be considered in context with what you are "
                        + "troubleshooting and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could "
                        + "be cause for concern."
                        + "<p>If you are not seeing expected client traffic, consider if network traces should be collected to review if there is an underlying "
                        + "network issue such as congestion, which could be causing issues. The Network Connection Status Indicator (NCSI) might also be an "
                        + "area to investigate.</p>";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 0 No response");

                    this.session["X-ResponseCodeDescription"] = "0 No Response";
                    //
                    /////////////////////////////
                    break;
                #endregion

                #region HTTP200s
                case 200:
                    /////////////////////////////
                    //
                    // 200.1. Connection blocked by Client Access Rules.
                    // 

                    if (this.session.fullUrl.Contains("outlook.office365.com/mapi")
                        && this.session.utilFindInResponse("Connection blocked by Client Access Rules", false) > 1)
                    {
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";

                        this.session["X-SessionType"] = "!CLIENT ACCESS RULE!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>CLIENT ACCESS RULE</span></b>";
                        this.session["X-ResponseComments"] = "A <b>client access rule has blocked MAPI connectivity to the mailbox</b>. "
                            + "Check if the <b>client access rule includes OutlookAnywhere</b>."
                            + "<p>Per https://docs.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/client-access-rules/client-access-rules, <br />"
                            + "OutlookAnywhere includes MAPI over HTTP.<p>"
                            + "<p>Remove OutlookAnywhere from the client access rule, wait 1 hour, then test again.</p>";

                        SkipFurtherProcessing = true;
                        // Break out of the switch statement. No further processing needed here.

                        this.session["X-ResponseCodeDescription"] = "200 OK";

                        break;
                    }

                    /////////////////////////////
                    //
                    // 200.2. Outlook MAPI traffic.
                    //
                    if (this.session.HostnameIs("outlook.office365.com") && (this.session.uriContains("/mapi/emsmdb/?MailboxId=")))
                    {
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";

                        this.session["X-SessionType"] = "Outlook MAPI";

                        this.session["X-ResponseAlert"] = "Outlook for Windows MAPI traffic";
                        this.session["X-ResponseComments"] = "Outlook for Windows MAPI traffic.";

                        SkipFurtherProcessing = true;
                        // Break out of the switch statement.
                        break;
                    }

                    /////////////////////////////
                    // 200.3. Exchange On-Premise Autodiscover redirect.
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
                        string RedirectAddress;

                        RedirectAddress = RedirectResponseBody.Substring(start, charcount).Replace("<RedirectAddr>", "");

                        if (RedirectAddress.Contains(".onmicrosoft.com"))
                        {
                            this.session["ui-backcolor"] = HTMLColourGreen;
                            this.session["ui-color"] = "black";
                            this.session["X-SessionType"] = "On-Prem AutoD Redirect";

                            this.session["X-ResponseAlert"] = "Exchange On-Premise Autodiscover redirect.";
                            this.session["X-ResponseComments"] = "Exchange On-Premise Autodiscover redirect address to Exchange Online found."
                                + "<p>RedirectAddress: "
                                + RedirectAddress
                                + "</p><p>This is what we want to see, the mail.onmicrosoft.com redirect address (you may know this as the <b>target address</b> or "
                                + "<b>remote routing address</b>) from On-Premise sends Outlook to Office 365.</p>";

                            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange On-Premise redirect address: " + RedirectAddress);
                            
                            SkipFurtherProcessing = true;

                        }
                        // Highlight if we got this far and do not have a redirect address which points to
                        // Exchange Online such as: contoso.mail.onmicrosoft.com.
                        else
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-SessionType"] = "!AUTOD REDIRECT ADDR!";

                            this.session["X-ResponseAlert"] = "!Exchange On-Premise Autodiscover redirect!";
                            this.session["X-ResponseComments"] = "Exchange On-Premise Autodiscover redirect address found, which does not contain .onmicrosoft.com." +
                                "<p>RedirectAddress: " + RedirectAddress +
                                "</p><p>If this is an Office 365 mailbox the <b>targetAddress from On-Premise is not sending Outlook to Office 365</b>!</p>";

                            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange On-Premise AUTOD REDIRECT ADDR! : " + RedirectAddress);
                            
                            SkipFurtherProcessing = true;
                        }
                    }

                    /////////////////////////////
                    //
                    // 200.4. Exchange On-Premise Autodiscover redirect - address can't be found
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
                        this.session["X-SessionType"] = "!NO AUTOD REDIRECT ADDR!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>Exchange On-Premise Autodiscover Redirect</span></b>";
                        this.session["X-ResponseComments"] = "Exchange On-Premise Autodiscover redirect address can't be found. "
                            + "Look for other On-Premise Autodiscover responses, we may have a "
                            + "valid Autodiscover targetAddress from On-Premise in another session in this trace."
                            + "Seeing some redirects return a HTTP 500 from Exchange OnPremise can be normal. "
                            + "This has been seen when Outlook is able to connect to the Office 365 mailbox.";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 Exchange On-Premise redirect address. Error code 500: The email address can't be found.");
                        
                        SkipFurtherProcessing = true;
                    }

                    /////////////////////////////
                    //
                    // 200.5. Exchange Online Autodiscover
                    //

                    // Make sure this session is an Exchange Online Autodiscover request.
                    // I *think* I am now seeing non-ClickToRun clients resolve to autodiscover-s.outlook.com and ClickToRun clients resolve to autodiscover.office365.com.
                    // Whatever the scenario they both need including.
                    if ((this.session.hostname == "autodiscover-s.outlook.com")
                        || (this.session.hostname == "autodiscover.office365.com")
                        && (this.session.uriContains("autodiscover.xml")))
                    {
                        if ((this.session.utilFindInResponse("<DisplayName>", false) > 1) &&
                            (this.session.utilFindInResponse("<MicrosoftOnline>", false) > 1) &&
                            (this.session.utilFindInResponse("<MailStore>", false) > 1) &&
                            (this.session.utilFindInResponse("<ExternalUrl>", false) > 1))
                        {
                            this.session["ui-backcolor"] = HTMLColourGreen;
                            this.session["ui-color"] = "black";
                            this.session["X-SessionType"] = "EXO Autodiscover";

                            this.session["X-ResponseAlert"] = "Exchange Online Autodiscover.";
                            this.session["X-ResponseComments"] = "Exchange Online Autodiscover.";

                            SkipFurtherProcessing = true;
                        }
                    }

                    /////////////////////////////
                    //
                    // 200.6. GetUnifiedGroupsSettings EWS call.
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
                            this.session["X-SessionType"] = "EWS GetUnifiedGroupsSettings";

                            this.session["X-ResponseAlert"] = "GetUnifiedGroupsSettings EWS call.";
                            this.session["X-ResponseComments"] = "<GroupCreationEnabled>true</GroupCreationEnabled> found in response body. " 
                                + "Expect user to be able to create Office 365 groups in Outlook.";

                            SkipFurtherProcessing = true;
                        }
                        // User cannot create Office 365 groups. Not an error condition in and of itself.
                        else if (this.session.utilFindInResponse("<GroupCreationEnabled>false</GroupCreationEnabled>", false) > 1)
                        {
                            this.session["ui-backcolor"] = HTMLColourGreen;
                            this.session["ui-color"] = "black";
                            this.session["X-SessionType"] = "EWS GetUnifiedGroupsSettings";

                            this.session["X-ResponseAlert"] = "<b><span style=color:'red'>GetUnifiedGroupsSettings EWS call</span></b>";
                            this.session["X-ResponseComments"] = "<GroupCreationEnabled>false</GroupCreationEnabled> found in response body. " 
                                + "Expect user to <b>NOT be able to create Office 365 groups</b> in Outlook.";

                            SkipFurtherProcessing = true;
                        }
                        // Did not see the expected keyword in the response body. This is the error condition.
                        else
                        {
                            this.session["ui-backcolor"] = HTMLColourRed;
                            this.session["ui-color"] = "black";
                            this.session["X-SessionType"] = "!EWS GetUnifiedGroupsSettings!";

                            this.session["X-ResponseAlert"] = "GetUnifiedGroupsSettings EWS call";
                            this.session["X-ResponseComments"] = "Though GetUnifiedGroupsSettings scenario was detected neither <GroupCreationEnabled>true</GroupCreationEnabled> or" 
                                + "<GroupCreationEnabled>false</GroupCreationEnabled> was found in the response body. Check the Raw tab for more details.";

                            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 GetUnifiedGroupsSettings!");
                                                        
                            SkipFurtherProcessing = true;
                        }
                    }

                    /////////////////////////////
                    //
                    // 200.7. 3S Suggestions call.
                    //
                    if (this.session.uriContains("search/api/v1/suggestions"))
                    {
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "3S Suggestions";

                        Uri uri = new Uri(this.session.fullUrl);
                        var queryStrings = System.Web.HttpUtility.ParseQueryString(uri.Query);
                        var scenario = queryStrings["scenario"] ?? "scenario not specified in url";
                        var entityTypes = queryStrings["entityTypes"] ?? "entityTypes not specified in url";
                        var clientRequestId = this.session.RequestHeaders.Where(x => x.Name.Equals("client-request-id")).FirstOrDefault();

                        this.session["X-ResponseAlert"] = "3S Suggestions";
                        this.session["X-ResponseComments"] = $"Scenario: {scenario} Types: {entityTypes} {clientRequestId}";

                        SkipFurtherProcessing = true;
                    }

                    /////////////////////////////
                    //
                    // 200.8. REST - People Request.
                    //
                    if (this.session.uriContains("people"))
                    {
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";

                        Uri uri = new Uri(this.session.fullUrl);
                        var queryStrings = System.Web.HttpUtility.ParseQueryString(uri.Query);

                        string sessionType = "";

                        // /me/people : : Private FindPeople Request
                        if (this.session.uriContains("/me/people"))
                        {
                            sessionType = "Private";
                        }

                        // /users()/people : Public FindPeople Request
                        else if (this.session.uriContains("/users(") && this.session.uriContains("/people"))
                        {
                            sessionType = "Public";
                        }

                        var requestId = this.session.ResponseHeaders.Where(x => x.Name.Equals("request-id")).FirstOrDefault();

                        this.session["X-SessionType"] = $"REST People {sessionType}";
                        this.session["X-ResponseAlert"] = $"REST People {sessionType}";
                        this.session["X-ResponseComments"] = $"{requestId} $search:{queryStrings["$search"]} $top:{queryStrings["$top"]} $skip:{queryStrings["$skip"]} $select:{queryStrings["$select"]} $filter:{queryStrings["$filter"]}";

                        SkipFurtherProcessing = true;
                    }


                    /////////////////////////////
                    //
                    // 200.99. All other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.
                    else
                    {                        
                        int wordCountError = 0;
                        int wordCountFailed = 0;
                        int wordCountException = 0;

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
                            this.session["X-SessionType"] = "!FAILURE LURKING!";

                            this.session["X-ResponseAlert"] = "<b><span style=color:'red'>'error', 'failed' or 'exception' found in response body</span></b>";
                            this.session["X-ResponseComments"] = "Session response body was scanned and errors or failures were found in response body. "
                                + "Check the Raw tab, click 'View in Notepad' button bottom right, and search for error in the response to review."
                                + "<p>After splitting all words in the response body the following were found:<br />"
                                + "Keyword 'Error' found " + wordCountErrorText + "<br />"
                                + "Keyword 'Failed' found " + wordCountFailedText + "<br />"
                                + "Keyword 'Exception' found " + wordCountExceptionText + "<br /></p>"
                                + "<p>Check the content body of the response for any failures you recognise. You may find <b>false positives, "
                                + "if lots of Javascript or other web code</b> is being loaded.</p>";

                            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 200 FAILURE LURKING!?");
                        }
                        else
                        {
                            // All good.
                            this.session["ui-backcolor"] = HTMLColourGreen;
                            this.session["ui-color"] = "black";

                            this.session["X-ResponseAlert"] = "<b><span style=color:'green'>No failure keywords detected in response body.</span></b>";
                            this.session["X-ResponseComments"] = "Session response body was scanned and no failure keywords ('error', 'failed' or "
                                + "'exception') detected in response body.";
                        }
                    }
                    this.session["X-ResponseCodeDescription"] = "200 OK";
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

                    this.session["X-ResponseAlert"] = "HTTP 201 Created.";
                    this.session["X-ResponseComments"] = "Not expecting this to be anything which needs attention for troubleshooting.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 201 Created.");

                    this.session["X-ResponseCodeDescription"] = "201 Created";
                    //
                    /////////////////////////////
                    break;
                case 202:
                    this.session["X-ResponseCodeDescription"] = "202 Accepted";
                    break;
                case 203:
                    this.session["X-ResponseCodeDescription"] = "203 Non-Authoritative Information";
                    break;
                case 204:
                    /////////////////////////////
                    //
                    //  HTTP 204: No Content.
                    //
                    // Somewhat highlight these.
                    this.session["ui-backcolor"] = HTMLColourOrange;
                    this.session["ui-color"] = "black";

                    this.session["X-ResponseAlert"] = "HTTP 204 No Content.";
                    this.session["X-ResponseComments"] = "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                        + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 204 No content.");
                
                    this.session["X-ResponseCodeDescription"] = "204 No Content";
                    //
                    /////////////////////////////
                    break;
                case 205:
                    this.session["X-ResponseCodeDescription"] = "205 Reset Content";
                    break;
                case 206:
                    this.session["X-ResponseCodeDescription"] = "206 Partial Content";
                    break;
                case 207:
                    this.session["X-ResponseCodeDescription"] = "207 Multi-Status (WebDAV; RFC 4918)";
                    break;
                case 208:
                    this.session["X-ResponseCodeDescription"] = "208 Already Reported (WebDAV; RFC 5842)";
                    break;
                case 226:
                    this.session["X-ResponseCodeDescription"] = "226 IM Used (RFC 3229)";
                    break;
                #endregion

                #region HTTP300s
                case 300:
                    this.session["X-ResponseCodeDescription"] = "300 Multiple Choices";
                    break;
                case 301:
                    /////////////////////////////
                    //
                    //  HTTP 301: Moved Permanently.
                    //
                    this.session["ui-backcolor"] = HTMLColourGreen;
                    this.session["ui-color"] = "black";

                    this.session["X-ResponseAlert"] = "HTTP 301 Moved Permanently";
                    this.session["X-ResponseComments"] = "Nothing of concern here at this time.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 301 Moved Permanently.");

                    this.session["X-ResponseCodeDescription"] = "301 Moved Permanently";
                    //
                    /////////////////////////////
                    break;
                case 302:
                    /////////////////////////////
                    //
                    //  HTTP 302: Found / Redirect.
                    //            

                    // Exchange Autodiscover redirects.
                    if (this.session.uriContains("autodiscover")) {
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "Autodiscover Redirect";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'green'>Exchange Autodiscover redirect.</span></b>";
                        this.session["X-ResponseComments"] = "This type of traffic is typically an Autodiscover redirect response from Exchange On-Premise "
                            + "sending the Outlook client to connect to Exchange Online.";
                    }
                    // All other HTTP 302 Redirects.
                    else
                    {
                        this.session["ui-backcolor"] = HTMLColourGreen;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "Autodiscover Redirect";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'green'>Redirect.</span></b>";
                        this.session["X-ResponseComments"] = "Redirects within Office 365 client applications or servers are not unusual. "
                            + "The only potential downfall is too many of them. However if this happens you would normally see a too many "
                            + "redirects exception thrown as a server response.";
                    }

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 302 Found / Redirect.");

                    this.session["X-ResponseCodeDescription"] = "302 Found";
                    //
                    /////////////////////////////
                    break;
                case 303:
                    this.session["X-ResponseCodeDescription"] = "303 See Other";
                    break;
                case 304:
                    /////////////////////////////
                    //
                    //  HTTP 304: Not modified.
                    //
                    this.session["ui-backcolor"] = HTMLColourGreen;
                    this.session["ui-color"] = "black";

                    this.session["X-ResponseAlert"] = "HTTP 304 Not Modified";
                    this.session["X-ResponseComments"] = "Nothing of concern here at this time.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 304 Not modified.");

                    this.session["X-ResponseCodeDescription"] = "304 Not Modified (RFC 7232)";
                    //
                    /////////////////////////////
                    break;
                case 305:
                    this.session["X-ResponseCodeDescription"] = "305 Use Proxy";
                    break;
                case 306:
                    this.session["X-ResponseCodeDescription"] = "306 Switch Proxy";
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
                        this.session["X-SessionType"] = "!UNEXPECTED LOCATION!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 307 Temporary Redirect</span></b>";
                        this.session["X-ResponseComments"] = "<b>Temporary Redirects have been seen to redirect Exchange Online Autodiscover " 
                            + "calls back to On-Premise resources, breaking Outlook connectivity</b>. Likely cause is a networking device within the local "
                            + "lan which is causing this. Test outside of the lan to confirm."
                            + "<p>This session has enough data points to be an Autodiscover request for Exchange Online which has not been sent to "
                            + "<a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a> as expected.</p>"
                            + "<p>Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place.</p>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 307 On-Prem Temp Redirect - Unexpected location!");
                    }
                    else
                    {
                        // The above scenario is not seem, however Temporary Redirects are not normally expected to be seen.
                        // Highlight as a warning.
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";

                        this.session["X-ResponseAlert"] = "HTTP 307 Temporary Redirect";
                        this.session["X-ResponseComments"] = "Temporary Redirects have been seen to redirect Exchange Online Autodiscover calls " +
                            "back to On-Premise resources, breaking Outlook connectivity. " +
                            "<p>Check the Headers or Raw tab and the Location to ensure the Autodiscover call is going to the correct place. </p>" +
                            "<p>If this session is not for an Outlook process then the information above may not be relevant to the issue under investigation.</p>";
                        
                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 307 Temp Redirect.");
                        
                        this.session["X-ResponseCodeDescription"] = "307 Temporary Redirect";
                    }
                    //
                    /////////////////////////////
                    break;
                case 308:
                    this.session["X-ResponseCodeDescription"] = "308 Permanent Redirect (RFC 7538)";
                    break;
                #endregion

                #region HTTP400s
                case 400:

                    /////////////////////////////
                    //
                    //  HTTP 400: BAD REQUEST.
                    //
                    this.session["ui-backcolor"] = HTMLColourOrange;
                    this.session["ui-color"] = "black";
                    this.session["X-SessionType"] = "Bad Request";

                    this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 401 Bad Request</span></b>";
                    this.session["X-ResponseComments"] = "HTTP 401: Bad Request. Seeing 1 or 2 of these may not be an issue. Any more than this should be investigated further.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 400 Bad Request.");

                    this.session["X-ResponseCodeDescription"] = "400 Bad Request";
                    //
                    /////////////////////////////
                    break;
                case 401:

                    /////////////////////////////
                    //
                    //  HTTP 401: UNAUTHORIZED.
                    //
                    this.session["ui-backcolor"] = HTMLColourOrange;
                    this.session["ui-color"] = "black";
                    this.session["X-SessionType"] = "Auth Challenge";

                    this.session["X-ResponseAlert"] = "<b><span style=color:'orange'>Authentication Challenge</span></b>";
                    this.session["X-ResponseComments"] = "Authentication Challenge. <b>These are expected</b> and are not an issue as long as a subsequent " 
                        + "HTTP 200 is seen for authentication to the server which issued the HTTP 401 unauthorized security challenge. "
                        + "<p>If you do not see HTTP 200's following HTTP 401's look for a wider authentication issue.</p>";

                    SkipFurtherProcessing = true;

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 401 Auth Challenge.");

                    this.session["X-ResponseCodeDescription"] = "401 Unauthorized (RFC 7235)";
                    //
                    /////////////////////////////
                    break;
                case 402:
                    this.session["X-ResponseCodeDescription"] = "402 Payment Required";
                    break;
                case 403:
                    /////////////////////////////
                    //
                    //  HTTP 403: FORBIDDEN.
                    //
                    // Looking for the term "Access Denied" or "Access Blocked" in session response.
                    // Specific scenario where a web proxy is blocking traffic from reaching the internet.
                    if (this.session.utilFindInResponse("Access Denied", false) > 1 || this.session.utilFindInResponse("Access Blocked", false) > 1)
                    {
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "WEB PROXY BLOCK";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 403 Access Denied - WEB PROXY BLOCK!</span></b>";
                        this.session["X-ResponseComments"] = "<b><span style=color:'red'>Is your firewall or web proxy blocking Outlook connectivity?</span></b> "
                            + "<p>To fire this message a HTTP 403 response code was detected and '<b><span style=color:'red'>Access Denied</span></b>' was found in "
                            + "the response body.</p>"
                            + "<p>Check the Raw and WebView tabs, do you see anything which indicates traffic is blocked? <b>Is there a message from "
                            + "your proxy device indiciating it blocked traffic any webmail related traffic?</b> A common scenario when first setting "
                            + "up Outlook with an Office 365 mailbox is a web proxy device blocking access to consumer webmail which can impact "
                            + "Outlook and potentially other Office 365 applications.</p>";
                            
                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 403 Forbidden; Phrase 'Access Denied' found in response body. Web Proxy blocking traffic?");
                        
                        SkipFurtherProcessing = true;
                    }
                    else
                    {
                        // All other HTTP 403's.
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "HTTP 403 FORBIDDEN";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 403 Forbidden</span></b>";
                        this.session["X-ResponseComments"] = "While HTTP 403's can be symptomatic of a proxy server blocking traffic, " 
                            + "however the phrase 'Access Denied' was NOT detected in the response body."
                            + "<p>A small number of HTTP 403's can be seen in normal working scenarios. Check the Raw and WebView tabs to look for anything which looks suspect.</p>"
                            + "<p>If you are troubleshooting Free/Busy (Meeting availability info) or setting Out of Office messages then you may be more interested in these.</p>"
                            + "<p>See: <a href='https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd877045(v=exchg.140)' target='_blank'>"
                            + "https://docs.microsoft.com/en-us/previous-versions/office/developer/exchange-server-2010/dd877045(v=exchg.140) </a></p>";

                        // 3rd-party EWS application could not connect to Exchange Online mailbox until culture/language was set for the first time in OWA.
                        if (this.session.fullUrl.Contains("outlook.office365.com/EWS") || this.session.fullUrl.Contains("outlook.office365.com/ews"))
                        {
                            this.session["X-ResponseComments"] += "<p>EWS Scenario: If you are troubleshooting a 3rd party EWS application (using application impersonation) and the service account mailbox "
                                + "has been recently migrated into the cloud, ensure mailbox is licensed and to log into the service account mailbox for the first time using OWA at "
                                + "<a href='https://outlook.office365.com' target='_blank'>https://outlook.office365.com</a> to set the mailbox culture.</p>"
                                + "<p>Validate with: Get-Mailbox service-account@domain.com | FL Languages</p>";
                        }

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 403 Forbidden.");
                    }

                    this.session["X-ResponseCodeDescription"] = "403 Forbidden";
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
                    this.session["X-SessionType"] = "HTTP 404 Not Found";

                    this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 404 Not Found</span></b>";
                    this.session["X-ResponseComments"] = "The quantity of these types of server errors need to be considered in context with what you are troubleshooting " +
                        "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 404 Not found.");

                    this.session["X-ResponseCodeDescription"] = "404 Not Found";
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

                    this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 405: Method Not Allowed</span></b>";
                    this.session["X-ResponseComments"] = "Method Not Allowed";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 405 Method not allowed.");

                    this.session["X-ResponseCodeDescription"] = "405 Method Not Allowed";
                    //
                    /////////////////////////////
                    break;
                case 406:
                    this.session["X-ResponseCodeDescription"] = "406 Not Acceptable";
                    break;
                case 407:
                    /////////////////////////////
                    //
                    // HTTP 407: Proxy Authentication Required.
                    //
                    this.session["ui-backcolor"] = HTMLColourRed;
                    this.session["ui-color"] = "black";
                    this.session["X-SessionType"] = "HTTP 407 Proxy Auth Required";

                    this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 407: Proxy Authentication Required</span></b>";
                    this.session["X-ResponseComments"] = "<b><span style=color:'red'>Proxy Authentication Required</span></b>"
                        + "<p>Seeing these in a trace when investigating Office 365 connectivity is a <b>big indicator of an issue</b>.</p>"
                        + "<p>Look to engage the network or security team who is responsible for the proxy infrastructure and give them "
                        + "the information from these HTTP 407 sessions to troubleshoot with.</p>"
                        + "<p>Office 365 application traffic should be exempt from proxy authentication or better yet follow Microsoft's recommendation "
                        + "to bypass the proxy for Office365 traffic.</p>";
                        
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 407 Proxy Authentication Required.");

                    this.session["X-ResponseCodeDescription"] = "407 Proxy Authentication Required (RFC 7235)";
                    //
                    /////////////////////////////
                    break;
                case 408:
                    this.session["X-ResponseCodeDescription"] = "408 Request Timeout";
                    break;
                case 409:
                    this.session["X-ResponseCodeDescription"] = "409 Conflict";
                    break;
                case 410:
                    this.session["X-ResponseCodeDescription"] = "410 Gone";
                    break;
                case 411:
                    this.session["X-ResponseCodeDescription"] = "411 Length Required";
                    break;
                case 412:
                    this.session["X-ResponseCodeDescription"] = "412 Precondition Failed (RFC 7232)";
                    break;
                case 413:
                    this.session["X-ResponseCodeDescription"] = "413 Payload Too Large (RFC 7231)";
                    break;
                case 414:
                    this.session["X-ResponseCodeDescription"] = "414 URI Too Long (RFC 7231)";
                    break;
                case 415:
                    this.session["X-ResponseCodeDescription"] = "415 Unsupported Media Type (RFC 7231)";
                    break;
                case 416:
                    this.session["X-ResponseCodeDescription"] = "416 Range Not Satisfiable (RFC 7233)";
                    break;
                case 417:
                    this.session["X-ResponseCodeDescription"] = "417 Expectation Failed";
                    break;
                case 418:
                    this.session["X-ResponseCodeDescription"] = "418 I'm a teapot (RFC 2324, RFC 7168)";
                    break;
                case 421:
                    this.session["X-ResponseCodeDescription"] = "421 Misdirected Request (RFC 7540)";
                    break;
                case 422:
                    this.session["X-ResponseCodeDescription"] = "422 Unprocessable Entity (WebDAV; RFC 4918)";
                    break;
                case 423:
                    this.session["X-ResponseCodeDescription"] = "423 Locked (WebDAV; RFC 4918)";
                    break;
                case 424:
                    this.session["X-ResponseCodeDescription"] = "424 Failed Dependency (WebDAV; RFC 4918)";
                    break;
                case 425:
                    this.session["X-ResponseCodeDescription"] = "425 Too Early (RFC 8470)";
                    break;
                case 426:
                    this.session["X-ResponseCodeDescription"] = "426 Upgrade Required";
                    break;
                case 428:
                    this.session["X-ResponseCodeDescription"] = "428 Precondition Required (RFC 6585)";
                    break;
                case 429:
                    /////////////////////////////
                    //
                    //  HTTP 429: Too Many Requests.
                    //
                    this.session["ui-backcolor"] = HTMLColourOrange;
                    this.session["ui-color"] = "black";
                    this.session["X-SessionType"] = "HTTP 429 Too Many Requests";

                    this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 429 Too Many Requests</span></b>";
                    this.session["X-ResponseComments"] = "These responses need to be taken into context with the rest of the sessions in the trace. " +
                        "A small number is probably not an issue, larger numbers of these could be cause for concern.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 429 Too many requests.");

                    this.session["X-ResponseCodeDescription"] = "429 Too Many Requests (RFC 6585)";
                    //
                    /////////////////////////////
                    break;
                case 431:
                    this.session["X-ResponseCodeDescription"] = "431 Request Header Fields Too Large (RFC 6585)";
                    break;
                case 451:
                    this.session["X-ResponseCodeDescription"] = "451 Unavailable For Legal Reasons (RFC 7725) or 451 IIS Redirect";
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
                        this.session["X-SessionType"] = "!Multi-Factor Auth!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 456 Multi-Factor Authentication</span></b>";
                        this.session["X-ResponseComments"] = "See details on Raw tab. Look for the presence of 'you must use multi-factor authentication'." +
                            "<p>This has been seen where users have <b>MFA enabled/enforced, but Modern Authentication is not enabled</b> in the Office 365 workload being connected to</p?" +
                            "<p>See <a href='https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662' target='_blank'>" +
                            "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>" +
                            "<p><a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>" +
                            "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 456 Multi-Factor Required!");
                        
                        SkipFurtherProcessing = true;
                    }
                    else if (this.session.utilFindInResponse("oauth_not_available", false) > 1)
                    {
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "!Multi-Factor Auth!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 456 Multi-Factor Authentication</span></b>";
                        this.session["X-ResponseComments"] = "See details on Raw tab. Look for the presence of 'oauth_not_available'."
                            + "<p>This has been seen where users have <b>MFA enabled/enforced, but Modern Authentication</b> is not enabled in the Office 365 workload being connected to</p>"
                            + "<p>See <a href='https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662' target='_blank'>" +
                            "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>"
                            + "<p><a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>" +
                            "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a></p>";
                            
                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 456 Multi-Factor Required!");
                        
                        SkipFurtherProcessing = true;
                    }
                    else
                    {
                        this.session["ui-backcolor"] = HTMLColourOrange;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "Multi-Factor Auth?";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'orange'>HTTP 456 Multi-Factor Authentication?</span></b>";
                        this.session["X-ResponseComments"] = "See details on Raw tab. Is Modern Authentication disabled?"
                            + "<p>This has been seen where users have <b>MFA enabled/enforced, but Modern Authentication is not enabled</b> in the Office 365 workload being connected to.</p>"
                            + "<p>See <a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>"
                            + "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>"
                            + "<a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>"
                            + "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a></p>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 456 Multi-Factor Required.");
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
                    this.session["X-SessionType"] = "!HTTP 500 Internal Server Error!";

                    this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 500 Internal Server Error</span></b>";
                    this.session["X-ResponseComments"] = "Consider the server that issued this response, "
                        + "look at the IP address in the 'Host IP' column and lookup where it is hosted to know who should be looking at "
                        + "the issue.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 500 Internal Server Error.");

                    this.session["X-ResponseCodeDescription"] = "500 Internal Server Error";
                    //
                    /////////////////////////////
                    break;
                case 501:
                    this.session["X-ResponseCodeDescription"] = "501 Not Implemented";
                    break;
                case 502:
                    /////////////////////////////
                    //
                    //  HTTP 502: BAD GATEWAY.
                    //


                    // Specific scenario on Outlook & OFffice 365 Autodiscover false positive on connections to:
                    //      autodiscover.domain.onmicrosoft.com:443

                    // Testing because I am finding colourisation based in the nested if statement below is not working.
                    // Strangely the same HTTP 502 nested if statement logic works fine in Office365FiddlerInspector.cs to write
                    // response alert and comment.
                    // From further testing this seems to come down to timing, clicking the sessions as they come into Fiddler
                    // I see the responsecode / response body unavailable, it then populates after a few sessions. I presume 
                    // since the UI has moved on already the session cannot be colourised. 

                    // On testing with loadSAZ instead this same code colourises sessions fine.

                    // Altered if statements from being bested to using && to see if this inproves here.
                    // This appears to be the only section in this code which has a session colourisation issue.

                    /////////////////////////////
                    //
                    // 502.1. telemetry false positive. <Need to validate in working scenarios>
                    //
                    if ((this.session.oRequest["Host"] == "sqm.telemetry.microsoft.com:443") &&
                        (this.session.utilFindInResponse("target machine actively refused it", false) > 1))
                    {
                        this.session["ui-backcolor"] = HTMLColourBlue;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "False Positive";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'green'>False Positive</span></b>";
                        this.session["X-ResponseComments"] = "Telemetry failing is unlikely the cause of Outlook / OWA connectivity or other issues.";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 502 Bad Gateway. Telemetry False Positive.");
                        
                        SkipFurtherProcessing = true;
                    }

                    /////////////////////////////
                    //
                    // 502.2. Exchange Online DNS lookup on contoso.onmicrosoft.com, False Positive!?
                    //
                    // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                    // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in blue? >
                    else if ((this.session.utilFindInResponse(".onmicrosoft.com", false) > 1) &&
                            (this.session.utilFindInResponse("DNS Lookup for ", false) > 1) &&
                            (this.session.utilFindInResponse(" failed.", false) > 1))
                    {
                        this.session["ui-backcolor"] = HTMLColourBlue;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "False Positive";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'green'>False Positive</span></b>";
                        this.session["X-ResponseComments"] = "False positive on HTTP 502; From the data in the response body this failure is likely due to a Microsoft DNS MX record "
                            + "which points to an Exchange Online Protection mail host that accepts connections only on port 25. Connection on port 443 will not work by design."
                            + "<p>To validate this above lookup the record, confirm it is a MX record and attempt to connect to the MX host on ports 25 and 443.</p>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 502 Bad Gateway. EXO DNS False Positive.");
                        
                        SkipFurtherProcessing = true;
                    }

                    /////////////////////////////
                    //
                    // 502.3. Exchange Online connection to autodiscover.contoso.mail.onmicrosoft.com, False Positive!
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
                        this.session["X-SessionType"] = "False Positive";

                        string AutoDFalsePositiveResponseBody = this.session.GetResponseBodyAsString();
                        int start = this.session.GetResponseBodyAsString().IndexOf("'");
                        int end = this.session.GetResponseBodyAsString().LastIndexOf("'");
                        int charcount = end - start;
                        string AutoDFalsePositiveDomain = AutoDFalsePositiveResponseBody.Substring(start, charcount).Replace("'", "");

                        this.session["X-ResponseAlert"] = "<b><span style=color:'green'>False Positive</span></b>";
                        this.session["X-ResponseComments"] = "By design Office 365 Autodiscover does not respond to "
                            + AutoDFalsePositiveDomain 
                            + " on port 443. "                            
                            + "<p>Validate this message by confirming the Host IP (if shown) is an Office 365 Host/IP address and perform a telnet to it on port 80.</p>"
                            + "<p>If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design redirects "
                            + "requests to <a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a>"
                            + " or <a href='https://autodiscover.office365.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover.office365.com/autodiscover/autodiscover.xml</a></p>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 502 Bad Gateway. O365 AutoD onmicrosoft.com False Positive.");
                        
                        SkipFurtherProcessing = true;
                    }

                    /////////////////////////////
                    //
                    // 502.4. Vanity domain points to Office 365 autodiscover; false positive.
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
                            (this.session.utilFindInResponse("target machine actively refused it", false) > 1))
                    {
                        this.session["ui-backcolor"] = HTMLColourBlue;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "Autodiscover Possible False Positive?";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'orange'>Autodiscover Possible False Positive?</span></b>";
                        this.session["X-ResponseComments"] = "Autoddiscover Possible False Positive. By design Office 365 endpoints such as autodiscover.contoso.onmicrosoft.com "
                            + "do not respond on port 443. "
                            + "<p>Validate this message by confirming this is an Office 365 Host/IP address and perform a telnet to it on port 80.</p>"
                            + "<p>If you get a response on port 80 and no response on port 443, this is more than likely an Autodiscover VIP which by design "
                            + "redirects requests to <a href='https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml</a>"
                            + " or <a href='https://autodiscover.office365.com/autodiscover/autodiscover.xml' target='_blank'>https://autodiscover.office365.com/autodiscover/autodiscover.xml</a></p>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 502 Bad Gateway. Vanity domain AutoD False Positive.");
                        
                        SkipFurtherProcessing = true;
                    }

                    /////////////////////////////
                    //
                    // 502.5. Anything else Exchange Autodiscover.
                    //
                    else if ((this.session.utilFindInResponse("target machine actively refused it", false) > 1) &&
                        (this.session.utilFindInResponse("autodiscover", false) > 1))
                    {
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "!AUTODISCOVER!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>AUTODISCOVER</span></b>";
                        this.session["X-ResponseComments"] = "This AutoDiscover request was refused by the server it was sent to. Check the raw tab for further details.";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 502 Bad Gateway. Exchange Autodiscover.");
                        
                        SkipFurtherProcessing = true;
                    }

                    /////////////////////////////
                    //
                    // 502.99. Everything else.
                    //
                    else
                    {
                        // Pick up any other 502 Bad Gateway call it out.
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "!Bad Gateway!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 502 Bad Gateway</span></b>";
                        this.session["X-ResponseComments"] = "Potential to cause the issue you are investigating. "
                            + "Do you see expected responses beyond this session in the trace? Is the Host IP for the device issuing this response with a subnet "
                            + "within your lan or something in a cloud provider's network?";
                            
                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 502 Bad Gateway (99).");
                    }

                    this.session["X-ResponseCodeDescription"] = "502 Bad Gateway";
                    //
                    /////////////////////////////
                    break;
                case 503:
                    /////////////////////////////
                    //
                    //  HTTP 503: SERVICE UNAVAILABLE.
                    //
                    // 503.1. Call out all 503 Service Unavailable as something to focus on.
                    searchTerm = "FederatedStsUnreachable";
                    //"Service Unavailable"

                    // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
                    //
                    // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
                    //

                    int wordCount = 0;

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
                        this.session["X-SessionType"] = "!FederatedSTSUnreachable!";

                        string RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>FederatedSTSUnreachable</span></b>";
                        this.session["X-ResponseComments"] = "<b><span style=color:'red'>HTTP 503: FederatedSTSUnreachable</span></b>."
                            + "<b><span style=color:'red'>The fedeation service is unreachable or unavailable</span></b>."
                            + "<p><b><span style=color:'red'>Troubleshoot this issue first before doing anything else.</span></b></p>"
                            + "<p>Check the Raw tab for additional details.</p>"
                            + "<p>Check the realm page for the authenticating domain. Check the below links from the Realm page to see if the IDP gives the "
                            + "expected responses.</p>"
                            + $"<a href='{RealmURL}' target='_blank'>{RealmURL}</a>"
                            + "<p><b>Expected responses for ADFS</b> (other federation services such as Ping, OKTA may vary)</p>" 
                            + "<b>AuthURL</b>: Normally expected to show federation service logon page.<br />" 
                            + "<b>STSAuthURL</b>: Normally expected to show HTTP 400.<br />" 
                            + "<b>MEXURL</b>: Normally expected to show long stream of XML data.<br />" 
                            + "<p>If any of these show the HTTP 503 Service Unavailable this <b>confirms some kind of failure on the federation service</b>.</p>" 
                            + "<p>If however you get the expected responses, this <b>does not neccessarily mean the federation service / everything authentication is healthy</b>. "
                            + "Further investigation is advised.</p>";

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 503 Service Unavailable. FederatedStsUnreachable in response body!");
                        
                        SkipFurtherProcessing = true;
                    }
                    /////////////////////////////
                    //
                    // 503.99. Everything else.
                    //
                    else
                    {
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "!Service Unavailable!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 503 Service Unavailable</span></b>";
                        this.session["X-ResponseComments"] = "<b>Server that was contacted in this session reports it is unavailable</b>. Look at the server that issued this response, "
                            + "it is healthy? Contactable? Contactable consistently or intermittently?";

                        SkipFurtherProcessing = true;

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 503 Service Unavailable (99).");
                    }

                    this.session["X-ResponseCodeDescription"] = "503 Service Unavailable";
                    //
                    /////////////////////////////
                    break;
                case 504:
                    /////////////////////////////
                    //
                    //  HTTP 504: GATEWAY TIMEOUT.
                    //

                    /////////////////////////////
                    // 504.1. HTTP 504 Bad Gateway 'internet has been blocked'
                    if ((this.session.utilFindInResponse("access", false) > 1) &&
                        (this.session.utilFindInResponse("internet", false) > 1) &&
                        (this.session.utilFindInResponse("blocked", false) > 1))
                    {
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";
                        this.session["X-SessionType"] = "!INTERNET BLOCKED!";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 504 Gateway Timeout -- Internet Access Blocked</span></b>";
                        this.session["X-ResponseComments"] = "Detected the keywords 'internet' and 'access' and 'blocked'. Potentially the computer this trace was collected "
                            + "from has been <b><span style=color:'red'>quaratined for internet access by a lan based network security device</span></b>."
                            + "<p>Validate this by checking the webview and raw tabs for more information.</p>";

                        SkipFurtherProcessing = true;

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + "  HTTP 504 Gateway Timeout -- Internet Access Blocked.");
                    }

                    /////////////////////////////
                    // 504.99. Pick up any other 504 Gateway Timeout and write data into the comments box.
                    else
                    {
                        this.session["ui-backcolor"] = HTMLColourRed;
                        this.session["ui-color"] = "black";

                        this.session["X-ResponseAlert"] = "<b><span style=color:'red'>HTTP 504 Gateway Timeout</span></b>";
                        this.session["X-ResponseComments"] = "The quantity of these types of server errors need to be considered in context with what you are troubleshooting "
                            + "and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could be cause for concern.";

                        this.session["X-SessionType"] = "Gateway Timeout";

                        SkipFurtherProcessing = true;

                        FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 504 Gateway Timeout (99).");
                        //
                        /////////////////////////////
                    }

                    this.session["X-ResponseCodeDescription"] = "504 Gateway Timeout";
                    break;
                case 505:
                    this.session["X-ResponseCodeDescription"] = "505 HTTP Version Not Supported";
                    break;
                case 506:
                    this.session["X-ResponseCodeDescription"] = "506 Variant Also Negotiates (RFC 2295)";
                    break;
                case 507:
                    this.session["X-ResponseCodeDescription"] = "507 Insufficient Storage (WebDAV; RFC 4918)";
                    break;
                case 508:
                    this.session["X-ResponseCodeDescription"] = "508 Loop Detected (WebDAV; RFC 5842)";
                    break;
                case 510:
                    this.session["X-ResponseCodeDescription"] = "510 Not Extended (RFC 2774)";
                    break;
                case 511:
                    this.session["X-ResponseCodeDescription"] = "511 Network Authentication Required (RFC 6585)";
                    break;
                #endregion

                #region Unofficials
                case 103:
                    this.session["X-ResponseCodeDescription"] = "103 Checkpoint";
                    break;
                case 218:
                    this.session["X-ResponseCodeDescription"] = "218 This is fine (Apache Web Server)";
                    break;
                case 419:
                    this.session["X-ResponseCodeDescription"] = "419 Page Expired (Laravel Framework)";
                    break;
                case 420:
                    this.session["X-ResponseCodeDescription"] = "420 Method Failure (Spring Framework) or Enhance Your Calm (Twitter)";
                    break;
                case 430:
                    this.session["X-ResponseCodeDescription"] = "430 Request Header Fields Too Large (Shopify)";
                    break;
                case 450:
                    this.session["X-ResponseCodeDescription"] = "450 Blocked by Windows Parental Controls (Microsoft)";
                    break;
                case 498:
                    this.session["X-ResponseCodeDescription"] = "498 Invalid Token (Esri)";
                    break;
                case 499:
                    this.session["X-ResponseCodeDescription"] = "499 Token Required (Esri) or nginx Client Closed Request";
                    break;
                case 509:
                    this.session["X-ResponseCodeDescription"] = "509 Bandwidth Limit Exceeded (Apache Web Server/cPanel)";
                    break;
                case 529:
                    this.session["X-ResponseCodeDescription"] = "529 Site is overloaded";
                    break;
                case 530:
                    this.session["X-ResponseCodeDescription"] = "530 Site is frozen or Cloudflare Error returned with 1xxx error.";
                    break;
                case 598:
                    this.session["X-ResponseCodeDescription"] = "598 (Informal convention) Network read timeout error";
                    break;
                #endregion

                #region HTTPIIS
                case 440:
                    this.session["X-ResponseCodeDescription"] = "440 IIS Login Time-out";
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 440 IIS Login Time-out");
                    break;
                case 449:
                    this.session["X-ResponseCodeDescription"] = "449 IIS Retry With";
                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 449 IIS Retry With");
                    break;
                #endregion

                #region nginx
                case 494:
                    this.session["X-ResponseCodeDescription"] = "494 nginx Request header too large";
                    break;
                case 495:
                    this.session["X-ResponseCodeDescription"] = "495 nginx SSL Certificate Error";
                    break;
                case 496:
                    this.session["X-ResponseCodeDescription"] = "496 nginx SSL Certificate Required";
                    break;
                case 497:
                    this.session["X-ResponseCodeDescription"] = "497 nginx HTTP Request Sent to HTTPS Port";
                    break;
                #endregion

                #region Cloudflare
                case 520:
                    this.session["X-ResponseCodeDescription"] = "520 Cloudflare Web Server Returned an Unknown Error";
                    break;
                case 521:
                    this.session["X-ResponseCodeDescription"] = "521 Cloudflare Web Server Is Down";
                    break;
                case 522:
                    this.session["X-ResponseCodeDescription"] = "522 Cloudflare Connection Timed Out";
                    break;
                case 523:
                    this.session["X-ResponseCodeDescription"] = "523 Cloudflare Origin Is Unreachable";
                    break;
                case 524:
                    this.session["X-ResponseCodeDescription"] = "524 Cloudflare A Timeout Occurred";
                    break;
                case 525:
                    this.session["X-ResponseCodeDescription"] = "525 Cloudflare SSL Handshake Failed";
                    break;
                case 526:
                    this.session["X-ResponseCodeDescription"] = "526 Cloudflare Invalid SSL Certificate";
                    break;
                case 527:
                    this.session["X-ResponseCodeDescription"] = "527 Cloudflare Railgun Error";
                    break;
                #endregion

                #region AWS
                case 460:
                    this.session["X-ResponseCodeDescription"] = "460 AWS Load balancer Timeout";
                    break;
                case 463:
                    this.session["X-ResponseCodeDescription"] = "463 AWS X-Forwarded-For Header > 30 IP addresses";
                    break;
                case 561:
                    this.session["X-ResponseCodeDescription"] = "561 AWS Unauthorized";
                    break;
                #endregion

                #region Default
                /////////////////////////////
                // Fallen into default, so undefined in the extension.
                // Mark the session as such.
                default:
                    this.session["ui-backcolor"] = "Yellow";
                    this.session["ui-color"] = "black";
                    this.session["X-SessionType"] = "Undefined";

                    this.session["X-ResponseAlert"] = "Undefined.";
                    this.session["X-ResponseComments"] = "No specific information on this session in the Office 365 Fiddler Extension.";

                    SkipFurtherProcessing = true;

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Session undefined in extension.");

                    this.session["X-ResponseCodeDescription"] = "Defaulted. HTTP Response Code undefined.";

                    break;
                    //
                    /////////////////////////////
                #endregion
            }
            // If a response code logic check set SkipFurtherProcesssing to true stop any further processing on this session.
            if (SkipFurtherProcessing) return;
            #endregion

            // Code section for response code logic overrides (long running sessions).
            #region LongRunningSessions

            double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);

            double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);

            // Warn on a 2.5 second roundtrip time.
            if (ClientMilliseconds > Preferences.GetWarningSessionTimeThreshold() && ClientMilliseconds < Preferences.GetSlowRunningSessionThreshold())
            {
                this.session["ui-backcolor"] = HTMLColourOrange;
                this.session["ui-color"] = "black";

                this.session["X-SessionType"] = "Roundtrip Time Warning";

                this.session["X-ResponseAlert"] = "<b><span style=color:'orange'>Roundtrip Time Warning</span></b>";
                this.session["X-ResponseComments"] = "This session took more than 2.5 seconds to complete. "
                    + "A small number of sessions completing roundtrip in this timeframe is not necessary sign of an issue.";
            }
            // If the overall session time runs longer than 5,000ms or 5 seconds.
            else if (ClientMilliseconds > Preferences.GetSlowRunningSessionThreshold())
            {
                this.session["ui-backcolor"] = HTMLColourRed;
                this.session["ui-color"] = "black";

                this.session["X-SessionType"] = "Long Running Client Session";

                this.session["X-ResponseAlert"] = "<b><span style=color:'red'>Long Running Client Session</span></b>";
                this.session["X-ResponseComments"] = "Long running session found. A small number of long running sessions in the < 10 "
                    + "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue."
                    + "<p>If, however, you are troubleshooting an application performance issue, consider any proxy device in your network, "
                    + "or any other device sitting between the client computer and access to the application server the data resides on."
                    + "Try the divide and conquer approach. What can you remove or bypass from the equation to see if the application then performs "
                    + "normally?</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Long running client session.");
            }
            // If the Office 365 server think time runs longer than 5,000ms or 5 seconds.
            else if (ServerMilliseconds > Preferences.GetSlowRunningSessionThreshold())
            {
                this.session["ui-backcolor"] = HTMLColourRed;
                this.session["ui-color"] = "black";

                this.session["X-SessionType"] = "Long Running Server Session";

                this.session["X-ResponseAlert"] = "<b><span style=color:'red'>Long Running Server Session</span></b>";
                this.session["X-ResponseComments"] = "Long running Server session found. A small number of long running sessions in the < 10 " +
                    "seconds time frame have been seen on normal working scenarios. This does not necessary signify an issue." + 
                    "<p>If, however, you are troubleshooting an Office 365 application performance issue, consider any proxy device in your network, " +
                    "or any other device sitting between the client computer and access to the internet." +
                    "Try the divide and conquer approach. What can you remove or bypass from the equation to see if the application then performs " +
                    "normally?</p>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Long running Office 365 session.");
            }
            #endregion
        }

        // Function to set Session Type column data.
        public void SetSessionType(Session session)
        {
            #region SetSessionTypeColumn

            /////////////////////////////
            ///
            /// Set Session Type
            /// 
            if (this.session.fullUrl.Contains("WSSecurity"))
            {
                this.session["X-SessionType"] = "Free/Busy";
            }
            else if (this.session.fullUrl.Contains("GetUserAvailability"))
            {
                this.session["X-SessionType"] = "Free/Busy";
            }
            else if (this.session.utilFindInResponse("GetUserAvailability", false) > 1)
            {
                this.session["X-SessionType"] = "Free/Busy";
            }
            // EWS.
            else if (this.session.fullUrl.Contains("outlook.office365.com/EWS")) { this.session["X-SessionType"] = "Exchange Web Services"; }
            // Generic Office 365.
            else if (this.session.fullUrl.Contains(".onmicrosoft.com") && (!(this.session.hostname.Contains("live.com")))) { this.session["X-SessionType"] = "Office 365 Authentication"; }
            else if (this.session.fullUrl.Contains("outlook.office365.com")) { this.session["X-SessionType"] = "Office 365"; }
            else if (this.session.fullUrl.Contains("outlook.office.com")) { this.session["X-SessionType"] = "Office 365"; }
            // Office 365 Authentication.
            else if (this.session.url.Contains("login.microsoftonline.com") || this.session.HostnameIs("login.microsoftonline.com")) { this.session["X-SessionType"] = "Office 365 Authentication"; }
            // ADFS Authentication.
            else if (this.session.fullUrl.Contains("adfs/services/trust/mex")) { this.session["X-SessionType"] = "ADFS Authentication"; }
            // Undetermined, but related to local process.
            else if (this.session.LocalProcess.Contains("outlook")) { this.session["X-SessionType"] = "Outlook"; }
            else if (this.session.LocalProcess.Contains("iexplore")) { this.session["X-SessionType"] = "Internet Explorer"; }
            else if (this.session.LocalProcess.Contains("chrome")) { this.session["X-SessionType"] = "Chrome"; }
            else if (this.session.LocalProcess.Contains("firefox")) { this.session["X-SessionType"] = "Firefox"; }
            // Everything else.
            else
            {
                this.session["X-SessionType"] = "Not Classified";
                this.session["ui-backcolor"] = "yellow";
                this.session["ui-color"] = "black";

                this.session["X-ResponseAlert"] = "Unclassified";
                this.session["X-ResponseComments"] = "The Office 365 Fiddler Extension does not have a way to classify this session."
                    + "<p>If you have a suggestion for an improvement, create an issue or better yet a pull request in the project Github repository: "
                    + "<a href='https://github.com/jprknight/Office365FiddlerExtension' target='_blank'>https://github.com/jprknight/Office365FiddlerExtension</a>.</p>";
            }

            /////////////////////////////
            //
            // Session Type overrides
            //
            // If the local process is null or blank, then we are analysing traffic from a remote client such as a mobile device.
            // Fiddler was acting as remote proxy when the data was captured: https://docs.telerik.com/fiddler/Configure-Fiddler/Tasks/ConfigureForiOS

            if ((this.session.LocalProcess == null) || (this.session.LocalProcess == ""))
            {
                // Traffic has a null or blank local process value.
                this.session["X-SessionType"] = "Remote Capture";
            }
            else
            {
                // If the traffic is not related to any of the below processes call it out.
                // So if for example lync.exe is the process write that to the Session Type column.
                if (!(this.session.LocalProcess.Contains("outlook") ||
                    this.session.LocalProcess.Contains("searchprotocolhost") ||
                    this.session.LocalProcess.Contains("iexplore") ||
                    this.session.LocalProcess.Contains("chrome") ||
                    this.session.LocalProcess.Contains("firefox") ||
                    this.session.LocalProcess.Contains("edge") ||
                    this.session.LocalProcess.Contains("w3wp")))
                {
                    // Everything which is not detected as related to Exchange, Outlook or OWA in some way.
                    { this.session["X-SessionType"] = this.session.LocalProcess; }
                }
            }
            #endregion
        }

        // Function to calculate session age on Inspector.
        public void CalculateSessionAge(Session session)
        {
            #region CalculateAge
            String TimeSpanDaysText;
            String TimeSpanHoursText;
            String TimeSpanMinutesText;

            DateTime SessionDateTime = this.session.Timers.ClientBeginRequest;
            DateTime DateTimeNow = DateTime.Now;
            TimeSpan CalcDataAge = DateTimeNow - SessionDateTime;
            int TimeSpanDays = CalcDataAge.Days;
            int TimeSpanHours = CalcDataAge.Hours;
            int TimeSpanMinutes = CalcDataAge.Minutes;

            if (TimeSpanDays == 1)
            {
                TimeSpanDaysText = TimeSpanDays + " day, ";
            }
            else
            {
                TimeSpanDaysText = TimeSpanDays + " days, ";
            }

            if (TimeSpanHours == 1)
            {
                TimeSpanHoursText = TimeSpanHours + " hour, ";
            }
            else
            {
                TimeSpanHoursText = TimeSpanHours + " hours, ";
            }

            if (TimeSpanMinutes == 1)
            {
                TimeSpanMinutesText = TimeSpanMinutes + " minute ago.";
            }
            else
            {
                TimeSpanMinutesText = TimeSpanMinutes + " minutes ago.";
            }

            String DataAge = TimeSpanDaysText + TimeSpanHoursText + TimeSpanMinutesText;

            this.session["X-DataCollected"] = SessionDateTime.ToString("dddd, MMMM dd, yyyy h:mm tt");

            if (TimeSpanDays <= 7)
            {
                this.session["X-DataAge"] = $"<b><span style=color:'green'>{DataAge}</span></b>";
            }
            else if (TimeSpanDays > 7 && TimeSpanDays < 14)
            {
                this.session["X-DataAge"] = $"<b><span style=color:'orange'>{DataAge}</span></b>";
            }
            else
            {
                this.session["X-DataAge"] = $"<b><span style=color:'red'>{DataAge}</span></b>";
            }
            #endregion
        }

        // Function where the Response Server column is populated.
        public void SetResponseServer(Session session)
        {
            #region ResponseServerColumn

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
            #endregion
        }

        // Function to set the Elapsed Time for the inspector. HTML mark up.
        public void SetInspectorElapsedTime(Session session)
        {
            #region InspectorElapsedTime
            // ClientDoneResponse can be blank. If so do not try to calculate and output Elapsed Time, we end up with a hideously large number.
            if (this.session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {
                double ClientMilliseconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalMilliseconds);
                double ClientSeconds = Math.Round((this.session.Timers.ClientDoneResponse - this.session.Timers.ClientBeginRequest).TotalSeconds);

                // If the roundtrip time is less than 1 second show the result in milliseconds.
                if (ClientMilliseconds == 0)
                {
                    this.session["X-InspectorElapsedTime"] = "Insufficient data";
                }
                else if (ClientMilliseconds < 1000)
                {
                    this.session["X-InspectorElapsedTime"] = $"{ClientMilliseconds}ms";
                }
                // If the roundtrip is over warning and under slow running thresholds; orange.
                else if (ClientMilliseconds > Preferences.GetWarningSessionTimeThreshold() && ClientMilliseconds < Preferences.GetSlowRunningSessionThreshold())
                {
                    this.session["X-InspectorElapsedTime"] = $"<b><span style=color:'orange'>{ClientSeconds} seconds ({ClientMilliseconds}ms).</span></b>";
                }
                // If roundtrip is over slow running threshold; red.
                else if (ClientMilliseconds > Preferences.GetSlowRunningSessionThreshold())
                {
                    this.session["X-InspectorElapsedTime"] = $"<b><span style=color:'red'>{ClientSeconds} seconds ({ClientMilliseconds}ms).</span></b>";
                    FiddlerApplication.Log.LogString("O365FiddlerExtention: " + this.session.id + " Long running session.");
                }
                // If the roundtrip time is more than 1 second show the result in seconds.
                else
                {
                    if (ClientSeconds == 1)
                    {
                        this.session["X-InspectorElapsedTime"] = $"{ClientSeconds} second({ClientMilliseconds}ms).";
                    }
                    else
                    {
                        this.session["X-InspectorElapsedTime"] = $"{ClientSeconds} seconds ({ClientMilliseconds}ms).";
                    }
                }
            }
            else
            {
                this.session["X-InspectorElapsedTime"] = "Insufficient data";
            }
            #endregion
        }

        // Function to set Server Think Time and Transit Time for use within Inspector.
        public void SetServerThinkTime(Session session)
        {
            #region ServerThinkTime
            // ServerGotRequest, ServerBeginResponse or ServerDoneResponse can be blank. If so do not try to calculate and output 'Server Think Time' or 'Transmit Time', we end up with a hideously large number.
            if (this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {

                double ServerMilliseconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalMilliseconds);
                double ServerSeconds = Math.Round((this.session.Timers.ServerBeginResponse - this.session.Timers.ServerGotRequest).TotalSeconds);

                if (ServerMilliseconds == 0)
                {
                    this.session["X-ServerThinkTime"] = "Insufficient data";
                }
                else if (ServerMilliseconds < 1000)
                {
                    this.session["X-ServerThinkTime"] = $"{ServerMilliseconds}ms";
                }
                else if (ServerMilliseconds > Preferences.GetWarningSessionTimeThreshold() && ServerMilliseconds < Preferences.GetSlowRunningSessionThreshold())
                {
                    this.session["X-ServerThinkTime"] = $"<b><span style=color:'orange'>{ServerSeconds} seconds ({ServerMilliseconds}ms).</span></b>";
                }
                else if (ServerMilliseconds >= Preferences.GetSlowRunningSessionThreshold())
                {
                    this.session["X-ServerThinkTime"] = $"<b><span style=color:'red'>{ServerSeconds} seconds ({ServerMilliseconds}ms).</span></b>";
                }
                else
                {
                    this.session["X-ServerThinkTime"] = $"{ServerSeconds}s ({ServerMilliseconds}ms).";
                }

                if (ServerMilliseconds > Preferences.GetSlowRunningSessionThreshold())
                {
                    FiddlerApplication.Log.LogString("O365FiddlerExtention: " + this.session.id + " Long running Office 365 session.");
                }
            }
            #endregion
        }

        public void SetTransitTime(Session session)
        {
            #region TransitTime
            // ServerGotRequest, ServerBeginResponse or ServerDoneResponse can be blank. If so do not try to calculate and output 'Server Think Time' or 'Transmit Time', we end up with a hideously large number.
            if (this.session.Timers.ServerGotRequest.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                this.session.Timers.ServerBeginResponse.ToString("H:mm:ss.fff") != "0:00:00.000" &&
                this.session.Timers.ServerDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {

                double ServerMilliseconds = Math.Round((this.session.Timers.ServerDoneResponse - this.session.Timers.ServerBeginResponse).TotalMilliseconds);
                double ServerSeconds = Math.Round((this.session.Timers.ServerDoneResponse - this.session.Timers.ServerBeginResponse).TotalSeconds);

                if (ServerMilliseconds == 0)
                {
                    this.session["X-TransitTime"] = "Insufficient data";
                }
                else if (ServerMilliseconds < 1000)
                {
                    this.session["X-TransitTime"] = $"{ServerMilliseconds} ms";
                }
                else if (ServerMilliseconds > Preferences.GetWarningSessionTimeThreshold() && ServerMilliseconds < Preferences.GetSlowRunningSessionThreshold())
                {
                    this.session["X-TransitTime"] = $"<b><span style=color:'orange'>{ServerSeconds} seconds ({ServerMilliseconds} ms).</span></b>";
                }
                else if (ServerMilliseconds >= Preferences.GetSlowRunningSessionThreshold())
                {
                    this.session["X-TransitTime"] = $"<b><span style=color:'red'>{ServerSeconds} seconds ({ServerMilliseconds} ms).</span></b>";
                }
                else
                {
                    this.session["X-TransitTime"] = $"{ServerSeconds} seconds ({ServerMilliseconds} ms)";
                }
            }
            #endregion
        }

        // Function where Elapsed Time column data is populated.
        public void SetElapsedTime(Session session)
        {
            #region ElapsedTimeColumn
            // Populate the ElapsedTime column.
            if (session.Timers.ClientBeginRequest.ToString("H:mm:ss.fff") != "0:00:00.000" && session.Timers.ClientDoneResponse.ToString("H:mm:ss.fff") != "0:00:00.000")
            {
                double Milliseconds = Math.Round((session.Timers.ClientDoneResponse - session.Timers.ClientBeginRequest).TotalMilliseconds);

                session["X-ElapsedTime"] = Milliseconds + "ms";
            }
            else
            {
                session["X-ElapsedTime"] = "No Data";
            }
            #endregion
        }

        // Function used for searching for strings in session responses.
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

        // Functions where Authentication column is populated and SAML parser code lives.
        #region SetAuthenticationSAMLParser

        public void SAMLParserFieldsNoData()
        {
            this.session["X-Issuer"] = "No SAML Data in session";
            this.session["X-AttributeNameUPN"] = "No SAML Data in session";
            this.session["X-NameIdentifierFormat"] = "No SAML Data in session";
            this.session["X-AttributeNameImmutableID"] = "No SAML Data in session";
        }

        /// <summary>
        /// Set Authentication column values.
        /// </summary>
        /// <param name="session"></param>
        public void SetAuthentication(Session session)
        {
            Boolean OverrideFurtherAuthChecking = false;

            this.session["X-Office365AuthType"] = "";

            DateTime today = DateTime.Today;

            this.session = session;

            // Set process name, split and exclude port used.
            if (this.session.LocalProcess != String.Empty) {
                string[] ProcessName = this.session.LocalProcess.Split(':');
                this.session["X-ProcessName"] = ProcessName[0];
            }
            // No local process to split.
            else
            {
                this.session["X-ProcessName"] = "Remote Capture";
            }
            
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
                    // Pull issuer data from response.
                    string IssuerSessionBody = this.session.ToString();
                    int IssuerStartIndex = IssuerSessionBody.IndexOf("Issuer=");
                    int IssuerEndIndex = IssuerSessionBody.IndexOf("IssueInstant=");
                    int IssuerLength = IssuerEndIndex - IssuerStartIndex;
                    string Issuer = IssuerSessionBody.Substring(IssuerStartIndex, IssuerLength);
                    Issuer = Issuer.Replace("&quot;", "");
                    Issuer = Issuer.Replace("Issuer=", "");

                    // Populate X flag on session.
                    this.session["X-Issuer"] = Issuer;
                }
                else
                {
                    this.session["X-Issuer"] = "Data points not found for issuer";
                }

                // Pull the x509 signing certificate data.
                if ((this.session.utilFindInResponse("&lt;X509Certificate>", false) > 1) && (this.session.utilFindInResponse("&lt;/X509Certificate>", false) > 1))
                {
                    string x509SigningCertSessionBody = this.session.ToString();
                    int x509SigningCertificateStartIndex = x509SigningCertSessionBody.IndexOf("&lt;X509Certificate>") + 20; // 20 to shift to start of the selection.
                    int x509SigningCertificateEndIndex = x509SigningCertSessionBody.IndexOf("&lt;/X509Certificate>");
                    int x509SigningCertificateLength = x509SigningCertificateEndIndex - x509SigningCertificateStartIndex;
                    string x509SigningCertificate = x509SigningCertSessionBody.Substring(x509SigningCertificateStartIndex, x509SigningCertificateLength);

                    this.session["X-SigningCertificate"] = x509SigningCertificate;
                }

                /////////////////////////////
                //
                // AttributeNameUPN.

                // Error handling, if we don't have the expected values in the session body, don't do this work.
                // Avoid null object reference errors at runtime.
                if ((this.session.utilFindInResponse("&lt;saml:Attribute AttributeName=&quot;UPN", false) > 1) &&
                    (this.session.utilFindInResponse("&lt;/saml:Attribute>", false) > 1))
                {
                    string AttributeNameUPNSessionBody = this.session.ToString();
                    int AttributeNameUPNStartIndex = AttributeNameUPNSessionBody.IndexOf("&lt;saml:Attribute AttributeName=&quot;UPN");
                    int AttributeNameUPNEndIndex = AttributeNameUPNSessionBody.IndexOf("&lt;/saml:Attribute>");
                    int AttributeNameUPNLength = AttributeNameUPNEndIndex - AttributeNameUPNStartIndex;
                    string AttributeNameUPN = AttributeNameUPNSessionBody.Substring(AttributeNameUPNStartIndex, AttributeNameUPNLength);
                    AttributeNameUPN = AttributeNameUPN.Replace("&quot;", "\"");
                    AttributeNameUPN = AttributeNameUPN.Replace("&lt;", "<");
                    // Now split the two lines with a new line for easier reading in the user control.
                    int SplitAttributeNameUPNStartIndex = AttributeNameUPN.IndexOf("<saml:AttributeValue>") + 21;

                    int SplitAttributeNameUPNEndIndex = AttributeNameUPN.IndexOf("</saml:AttributeValue>");
                    int SplitAttributeNameLength = SplitAttributeNameUPNEndIndex - SplitAttributeNameUPNStartIndex;

                    //string AttributeNameUPNFirstLine = AttributeNameUPN.Substring(0, SplitAttributeNameUPNStartIndex);
                    //string AttributeNameUPNSecondLine = AttributeNameUPN.Substring(SplitAttributeNameUPNStartIndex);
                    AttributeNameUPN = AttributeNameUPN.Substring(SplitAttributeNameUPNStartIndex, SplitAttributeNameLength);

                    // Populate X flag on session.
                    this.session["X-AttributeNameUPN"] = AttributeNameUPN;
                }
                else
                {
                    this.session["X-AttributeNameUPN"] = "Data points not found for AttributeNameUPN";
                }

                /////////////////////////////
                //
                // NameIdentifierFormat.

                if ((this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                    (this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
                {
                    string NameIdentifierFormatSessionBody = this.session.ToString();
                    int NameIdentifierFormatStartIndex = NameIdentifierFormatSessionBody.IndexOf("&lt;saml:NameIdentifier Format");
                    int NameIdentifierFormatEndIndex = NameIdentifierFormatSessionBody.IndexOf("&lt;saml:SubjectConfirmation>");
                    int NameIdentifierFormatLength = NameIdentifierFormatEndIndex - NameIdentifierFormatStartIndex;
                    string NameIdentifierFormat = NameIdentifierFormatSessionBody.Substring(NameIdentifierFormatStartIndex, NameIdentifierFormatLength);
                    NameIdentifierFormat = NameIdentifierFormat.Replace("&quot;", "\"");
                    NameIdentifierFormat = NameIdentifierFormat.Replace("&lt;", "<");

                    // Populate X flag on session.
                    this.session["X-NameIdentifierFormat"] = NameIdentifierFormat;
                }
                else
                {
                    this.session["X-NameIdentifierFormat"] = "Data points not found for NameIdentifierFormat";
                }

                /////////////////////////////
                //
                // AttributeNameImmutableID.

                if ((this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                    (this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
                {
                    string AttributeNameImmutableIDSessionBody = this.session.ToString();
                    int AttributeNameImmutableIDStartIndex = AttributeNameImmutableIDSessionBody.IndexOf("AttributeName=&quot;ImmutableID");
                    int AttributeNameImmutibleIDEndIndex = AttributeNameImmutableIDSessionBody.IndexOf("&lt;/saml:AttributeStatement>");
                    int AttributeNameImmutibleIDLength = AttributeNameImmutibleIDEndIndex - AttributeNameImmutableIDStartIndex;
                    string AttributeNameImmutibleID = AttributeNameImmutableIDSessionBody.Substring(AttributeNameImmutableIDStartIndex, AttributeNameImmutibleIDLength);
                    AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&quot;", "\"");
                    AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&lt;", "<");
                    // Now split out response with a newline for easier reading.
                    int SplitAttributeNameImmutibleIDStartIndex = AttributeNameImmutibleID.IndexOf("<saml:AttributeValue>") + 21; // Add 21 characters to shift where the newline is placed.
                    //string AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                    //string AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;
                    // Second split
                    int SplitAttributeNameImmutibleIDEndIndex = AttributeNameImmutibleID.IndexOf("</saml:AttributeValue></saml:Attribute>");
                    int SubstringLength = SplitAttributeNameImmutibleIDEndIndex - SplitAttributeNameImmutibleIDStartIndex;
                    AttributeNameImmutibleID = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex, SubstringLength);
                    
                    //AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;

                    // Populate X flag on session.
                    this.session["X-AttributeNameImmutableID"] = AttributeNameImmutibleID;
                }
                else
                {
                    this.session["X-AttributeNameImmutableID"] = "Data points not found for AttributeNameImmutibleID";
                }
            }
            // Determine if Modern Authentication is enabled in session request.
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
                // If they do, then the Office 365 workload (Exchange Online / Skype etc) is configured with Modern Authentication disabled.
                if (KeywordFourMillion > 0 && KeywordFlighting > 0 && Keywordenabled > 0 &&
                    Keyworddomain > 0 && Keywordoauth_not_available > 0 && this.session.HostnameIs("autodiscover-s.outlook.com"))
                {
                    this.session["X-Authentication"] = "Modern Auth Disabled";
                    
                    this.session["X-AuthenticationDesc"] = "Office 365 workload has Modern Authentication disabled. "
                        + $"At this point in {today:yyyy} there isn't a good reason to not have Modern Authentication turned on or having a plan to turn it on."
                        + "<p>MutiFactor Authentication will not work as expected while Modern Authentication "
                        + "is disabled in the Office 365 workload."
                        + "For Exchange Online, the following is important for Outlook connectivity:</p>"
                        + "<p>Outlook 2010 and older do not support Modern Authentication and by extension MutliFactor Authentication.</p>"
                        + "<p>Outlook 2013 supports modern authentication with updates and the EnableADAL registry key set to 1.</p>"
                        + "<p>See https://support.microsoft.com/en-us/help/4041439/modern-authentication-configuration-requirements-for-transition-from-o </p>"
                        + "<p>Outlook 2016 or newer. No updates or registry keys needed for Modern Authentication.</p>";

                    // Set the OverrideFurtherAuthChecking to true; Office 365 workload Modern Auth Disabled is a more important message in these sessions,
                    // than Outlook client auth capabilities. Other sessions are expected to show client auth capabilities.
                    OverrideFurtherAuthChecking = true;

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Modern Auth Disabled.");
                }
                else
                {
                    // Do nothing right now.
                }

                // Now get specific to find out what the client can do.
                // If the session request header Authorization equals Bearer this is a Modern Auth capable client.
                // Note OverrideFurtherAuthChecking which is set above if we detected Office 365 workload has Modern Auth disabled.
                if (this.session.oRequest["Authorization"] == "Bearer" && !(OverrideFurtherAuthChecking))
                {
                    this.session["X-Authentication"] = "Client Modern Auth Capable";

                    this.session["X-AuthenticationDesc"] = this.session["X-ProcessName"] + " is stating it is Modern Authentication capable. "
                        + "Whether it is used or not will depend on whether Modern Authentication is enabled in the Office 365 service.";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Client Modern Auth.");
                }
                // If the session request header Authorization equals Basic this is a Basic Auth capable client.
                // Note OverrideFurtherAuthChecking which is set above if we detected Office 365 worload has Modern Auth disabled.
                else if (this.session.oRequest["Authorization"] == "Basic" && !(OverrideFurtherAuthChecking))
                {
                    this.session["X-Authentication"] = "Client Basic Auth Capable";

                    this.session["X-AuthenticationDesc"] = this.session["X-ProcessName"] + " is stating it is Basic Authentication capable. "
                        + "Whether it is used or not will depend on whether Basic Authentication is enabled in the Office 365 service."
                        + "<p>If this is Outlook, in all likelihood this is an Outlook 2013 (updated prior to Modern Auth), Outlook 2010 or an "
                        + "older Outlook client, which does not support Modern Authentication.<br />"
                        + "MutiFactor Authentication will not work as expected with Basic Authentication only capable Outlook clients</p>";

                    FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Outlook Basic Auth.");
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

                this.session["X-AuthenticationDesc"] = this.session["X-ProcessName"] + " accessing resources with a Modern Authentication security token.";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Modern Auth Token.");
            }
            // Basic == Basic Authentication.
            else if (this.session.oRequest["Authorization"].Contains("Basic"))
            {
                SAMLParserFieldsNoData();

                this.session["X-Authentication"] = "<b>Basic Auth Token</b>";

                this.session["X-AuthenticationDesc"] = $"Process '{this.session["X-ProcessName"]}' accessing resources with a Basic Authentication security token. "
                    + "<b>It's time to think about Modern Authentication!</b>";

                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " Basic Auth Token.");
            }
            else
            {
                SAMLParserFieldsNoData();
                // Change which control appears for this session on the Office365 Auth inspector tab.
                this.session["X-Office365AuthType"] = "Office365Auth";

                this.session["X-Authentication"] = "No Auth Headers";
                this.session["X-AuthenticationDesc"] = "No Auth Headers";
            }
        }
        #endregion
    }
}