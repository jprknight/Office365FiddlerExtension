using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace EXOFiddlerInspector
{
    public class WebTrafficRuleSet
    {

        Session session { get; set; }

        public WebTrafficRuleSet(Session session)
        {
            this.session = session;
        }

        public void RunWebTrafficRuleSet()
        {
            if (null == this.session)
            {
                throw new ArgumentException("Session cannot be null.");
            }
            else
            {

                int wordCount = 0;

                // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
                //
                // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
                //

                string text = this.session.ToString();

                //Convert the string into an array of words  
                string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '"' }, StringSplitOptions.RemoveEmptyEntries);

                //string searchTerm = "error";
                string[] searchTerms = { "Error", "FederatedStsUnreachable" };

                foreach (string searchTerm in searchTerms)
                {
                    // Create the query.  Use ToLowerInvariant to match "data" and "Data"   
                    var matchQuery = from word in source
                                     where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                     select word;

                    // Count the matches, which executes the query.  
                    wordCount = matchQuery.Count();

                    //
                    //  HTTP 200.
                    //
                    if (this.session.responseCode == 200)
                    {
                        // Looking for errors lurking in HTTP 200 OK responses.
                        if (searchTerm == "Error")
                        {
                            string result = "After splitting all words in the response body the word 'error' was found " + wordCount + " time(s).";

                            if (wordCount > 0)
                            {
                                _displayControl.SetResponseAlertTextBox("Word Search 'Error' found in respone body.");
                                _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP200ErrorsFound + "<br /><br />" + result);
                            }
                            else
                            {
                                _displayControl.SetResponseAlertTextBox("Word Search 'Error' Not found in response body.");
                                _displayControl.SetResponseCommentsRichTextboxText(result);
                            }
                        }

                        // Autodiscover redirect Address from Exchange On-Premise.
                        if (this.session.utilFindInResponse("<RedirectAddr>", false) > 1)
                        {
                            if (this.session.utilFindInResponse("</RedirectAddr>", false) > 1)
                            {
                                _displayControl.SetResponseAlertTextBox("Exchange On-Premise Autodiscover redirect Address found.");
                                _displayControl.SetResponseCommentsRichTextboxText("Exchange On-Premise Autodiscover redirect Address found.");
                            }
                        }
                    }
                    //
                    //  HTTP 401: UNAUTHORIZED.
                    //
                    else if (this.session.responseCode == 401)
                    {
                        _displayControl.SetResponseAlertTextBox("HTTP 401 Unauthorized");
                        _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP401Unauthorized);
                    }
                    //
                    //  HTTP 403: FORBIDDEN.
                    //
                    // Simply looking for the term "Access Denied" works fine using utilFindInResponse.
                    else if (this.session.responseCode == 403)
                    {
                        // Specific scenario where a web proxy is blocking traffic.
                        if (this.session.utilFindInResponse("Access Denied", false) > 1)
                        {
                            _displayControl.SetResponseAlertTextBox("Panic Stations!!!");
                            _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP403WebProxyBlocking);
                        }
                        else
                        {
                            // Pick up any 403 Forbidden and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("HTTP 403 Forbidden");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 403 Forbidden");
                        }
                    }
                    //
                    //  HTTP 404: Not Found.
                    //
                    else if (this.session.responseCode == 404)
                    {
                        // Pick up any 404 Not Found and write data into the comments box.
                        _displayControl.SetResponseAlertTextBox("HTTP 404 Not Found");
                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 404 Not Found");
                    }

                    // HTTP 440 ???

                    //
                    //  HTTP 500: Internal Server Error.
                    //
                    else if (this.session.responseCode == 500)
                    {
                        // Pick up any 500 Internal Server Error and write data into the comments box.
                        _displayControl.SetResponseAlertTextBox("HTTP 500 Internal Server Error");
                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 500 Internal Server Error");
                    }
                    //
                    //  HTTP 502: BAD GATEWAY.
                    //
                    else if (this.session.responseCode == 502)
                    {
                        // Specific scenario on Outlook & OFffice 365 Autodiscover false positive on connections to:
                        //      autodiscover.domain.onmicrosoft.com:443
                        if (this.session.utilFindInResponse("autodiscover", false) > 1)
                        {
                            if (this.session.utilFindInResponse("target machine actively refused it", false) > 1)
                            {
                                if (this.session.utilFindInResponse(":443", false) > 1)
                                {
                                    _displayControl.SetResponseAlertTextBox("These aren't the droids your looking for.");
                                    _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP502AutodiscoverFalsePositive);
                                }
                            }
                            // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                            // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in green? >
                        }
                        else if (this.session.utilFindInResponse("DNS Lookup for ", false) > 1)
                        {
                            if (this.session.utilFindInResponse("mail.onmicrosoft.com", false) > 1)
                            {
                                if (this.session.utilFindInResponse("failed.System.Net.Sockets.SocketException", false) > 1)
                                {
                                    if (this.session.utilFindInResponse("The requested name is valid, but no data of the requested type was found", false) > 1)
                                    {
                                        _displayControl.SetResponseAlertTextBox("These aren't the droids your looking for.");
                                        _displayControl.SetResponseCommentsRichTextboxText("DNS record does not exist. Connection on port 443 will not work by design.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Pick up any other 502 Bad Gateway and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("HTTP 502 Bad Gateway");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 502 Bad Gateway");
                        }
                    }
                    //
                    //  HTTP 503: SERVICE UNAVAILABLE.
                    //
                    // Using utilFindInResponse to find FederatedStsUnreachable did not work for some reason.
                    // So instead split all words in the response body and check them with Linq.
                    else if (this.session.responseCode == 503)
                    {
                        // Specific scenario where Federation service is unavailable, preventing authentication, preventing access to Office 365 mailbox.
                        if (searchTerm == "FederatedStsUnreachable")
                        {
                            if (wordCount > 0)
                            {
                                _displayControl.SetResponseAlertTextBox("The federation service is unreachable or unavailable.");
                                _displayControl.SetResponseCommentsRichTextboxText(Properties.Settings.Default.HTTP503FederatedSTSUnreachable);
                            }
                            // Testing code.
                            //else
                            //{
                            //    _displayControl.SetResponseAlertTextBox("Federation failure error missed.");
                            //}
                        }
                        else
                        {
                            // Pick up any other 503 Service Unavailable and write data into the comments box.
                            _displayControl.SetResponseAlertTextBox("HTTP 503 Service Unavailable.");
                            _displayControl.SetResponseCommentsRichTextboxText("HTTP 503 Service Unavailable.");
                        }
                    }
                    //
                    //  HTTP 504: GATEWAY TIMEOUT.
                    //
                    else if (this.session.responseCode == 504)
                    {
                        // Pick up any 504 Gateway Timeout and write data into the comments box.
                        _displayControl.SetResponseAlertTextBox("HTTP 504 Gateway Timeout");
                        _displayControl.SetResponseCommentsRichTextboxText("HTTP 504 Gateway Timeout");
                    }
                }

                /* public void OnLoad() {
                    var oSessions = FiddlerApplication.UI.GetAllSessions();
                    foreach (var fsess in oSessions)
                    {
                        fsess["ui-backcolor"] = "blue";
                    }
                }
                */

            }
        }
    }
}
