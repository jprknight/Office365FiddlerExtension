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
        private bool bCreatedColumn = false;
        private string searchTerm;

        private MenuItem mnuCookieTag;
        private object proxy;

        internal Session session { get; set; }

        public void OnLoad()
        {
            EnsureColumn();
            FiddlerApplication.OnLoadSAZ += HandleLoadSaz;
        }

        // Make sure the Columns are added to the UI.
        public void EnsureColumn()
        {
            if (bCreatedColumn) return;

            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Time", 2, 110, "X-iTTLB");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Server", 3, 110, "@response.Server");
            FiddlerApplication.UI.lvSessions.AddBoundColumn("Exchange Type", 4, 110, "X-ExchangeType");

            //FiddlerApplication.UI.Refresh();

            bCreatedColumn = true;
        }

        #region LoadSAZ
        /////////////////
        // 
        // Handle loading a SAZ file.
        //


        private void HandleLoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
        {
            CheckForUpdate();
            FiddlerApplication.UI.lvSessions.BeginUpdate();
            foreach (var session in e.arrSessions)
            {
                // Populate the ResponseTime column on load SAZ.
                session["X-iTTLB"] = session.oResponse.iTTLB.ToString() + "ms";
                // Populate the ExchangeType column on load SAZ.
                SetExchangeType(session);
                // Colourise sessions on load SAZ.
                OnPeekAtResponseHeaders(session); //Run whatever function you use in IAutoTamper
                session.RefreshUI();
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
                string message = "An extension update is available: " + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ". Do you want to download the update?";
                string caption = "EXO Fiddler Extension";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(Properties.Settings.Default.InstallerURL);
                }             
            }
            else
            {
                MessageBox.Show("EXO Fiddler Extension up to date.");
            }
        }
        //
        /////////////////

        #endregion

        #region ColouriseRuleSet

        private void OnPeekAtResponseHeaders(Session session)
        {

            this.session = session;

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

                #region switchstatement
                switch (this.session.responseCode)
                {
                    case 0:
                        #region HTTP0
                        /////////////////////////////
                        //
                        //  HTTP 0: No Response.
                        //
                        this.session["ui-backcolor"] = "red";
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
                                this.session["ui-backcolor"] = "green";
                                this.session["ui-color"] = "black";
                            }
                            // Highlight if we got this far and do not have a redirect address which points to
                            // Exchange Online such as: contoso.mail.onmicrosoft.com.
                            else
                            {
                                this.session["ui-backcolor"] = "red";
                                this.session["ui-color"] = "black";
                            }
                        }

                        /////////////////////////////
                        //
                        // 99. No other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.
                        else
                        {
                            int wordCount = 0;

                            // Due to compute cost attempting to minimise when this function is used.
                            //
                            // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
                            //
                            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
                            //

                            string text = this.session.ToString();

                            //Convert the string into an array of words  
                            string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '<', '>' }, StringSplitOptions.RemoveEmptyEntries);

                            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
                            var matchQuery = from word in source
                                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                             select word;

                            searchTerm = "Error";

                            // Count the matches, which executes the query.  
                            wordCount = matchQuery.Count();

                            if (wordCount > 0)
                            {
                                this.session["ui-backcolor"] = "red";
                                this.session["ui-color"] = "black";
                            }
                            else
                            {
                                this.session["ui-backcolor"] = "green";
                                this.session["ui-color"] = "black";
                            }
                        }
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 204:
                        #region HTTP204
                        /////////////////////////////
                        //
                        //  HTTP 204: No Content.
                        //
                        this.session["ui-backcolor"] = "green";
                        this.session["ui-color"] = "black";
                        //
                        /////////////////////////////
                        #endregion
                        break;
                    case 301:
                        #region HTTP301
                        /////////////////////////////
                        //
                        //  HTTP 301: Moved Permanently.
                        //
                        this.session["ui-backcolor"] = "green";
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
                        this.session["ui-backcolor"] = "green";
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
                        this.session["ui-backcolor"] = "green";
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
                            this.session["ui-backcolor"] = "red";
                            this.session["ui-color"] = "black";
                        }
                        else
                        {
                            // The above scenario is not seem, however Temporary Redirects are not exactly normally expected to be seen.
                            // Highlight as a warning.
                            this.session["ui-backcolor"] = "orange";
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
                        this.session["ui-backcolor"] = "orange";
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
                        this.session["ui-backcolor"] = "red";
                        this.session["ui-color"] = "black";
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
                        this.session["ui-backcolor"] = "orange";
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
                        this.session["ui-backcolor"] = "orange";
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
                        this.session["ui-backcolor"] = "red";
                        this.session["ui-color"] = "black";
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
                            this.session["ui-backcolor"] = "blue";
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
                            this.session["ui-backcolor"] = "blue";
                            this.session["ui-color"] = "black";
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
                            // (this.session.utilFindInResponse(".onmicrosoft.com", false) > 1)
                            (this.session.utilFindInResponse("failed. System.Net.Sockets.SocketException", false) > 1) &&
                            (this.session.utilFindInResponse("DNS Lookup for ", false) > 1))
                        {
                            this.session["ui-backcolor"] = "blue";
                            this.session["ui-color"] = "black";
                        }

                        /////////////////////////////
                        //
                        // 99. Everything else.
                        //
                        else
                        {
                            // Pick up any other 502 Bad Gateway call it out.
                            this.session["ui-backcolor"] = "red";
                            this.session["ui-color"] = "black";
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
                        this.session["ui-backcolor"] = "red";
                        this.session["ui-color"] = "black";
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
                        this.session["ui-backcolor"] = "red";
                        this.session["ui-color"] = "black";
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
                this.session["ui-backcolor"] = "gray";
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
            OnPeekAtResponseHeaders(session);
            session.RefreshUI();
            //
            /////////////////

            /////////////////
            //
            // For some reason setting the column ordering when adding the columns did not work.
            // Adding the ordering here instead does work.
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("#", 0, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Result", 1, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Time", 2, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Response Server", 3, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Exchange Type", 4, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Protocol", 5, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Host", 6, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("URL", 7, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Body", 8, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Caching", 9, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Content-Type", 10, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Process", 11, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Comments", 12, -1);
            FiddlerApplication.UI.lvSessions.SetColumnOrderAndWidth("Custom", 13, -1);
            //
            /////////////////

            /////////////////
            //
            // Call the function to populate the session type column on live trace.
            SetExchangeType(session);
            //
            // Populate the ResponseTime column on live trace.
            session["X-iTTLB"] = session.oResponse.iTTLB.ToString() + "ms";
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