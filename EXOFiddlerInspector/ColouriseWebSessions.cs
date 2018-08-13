using System;
using System.Windows.Forms;
using Fiddler;
using System.Linq;

public class Violin : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
{
    string sUserAgent = "";
    //private object fSessions;
    private bool bCreatedColumn = false;
    
    //public object GetAllSessions { get ; private set; }

    public Violin()
    {
        /* NOTE: It's possible that Fiddler UI isn't fully loaded yet, so don't add any UI in the constructor.

           But it's also possible that AutoTamper* methods are called before OnLoad (below), so be
           sure any needed data structures are initialized to safe values here in this constructor */

        sUserAgent = "Violin";
    }


    public void OnLoad()
    {
        FiddlerApplication.OnLoadSAZ += HandleLoadSaz;
    }

    private void HandleLoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
    {
        FiddlerApplication.UI.lvSessions.BeginUpdate();
        foreach (var session in e.arrSessions)
        {
            OnPeekAtResponseHeaders(session); //Run whatever function you use in IAutoTamper
            session.RefreshUI();
        }
        FiddlerApplication.UI.lvSessions.EndUpdate();
    }

    private void OnPeekAtResponseHeaders(Session session)
    {
        int wordCount = 0;

        // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
        //
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
        //

        string text = session.ToString();

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
            if (session.responseCode == 200)
            {
                // Looking for errors lurking in HTTP 200 OK responses.
                if (searchTerm == "Error")
                {
                    string result = "After splitting all words in the response body the word 'error' was found " + wordCount + " time(s).";

                    if (wordCount > 0)
                    {
                        session["ui-backcolor"] = "red";
                        session["ui-color"] = "black";
                    }
                    else
                    {
                        //session["ui-backcolor"] = "red";
                    }
                }

                // Autodiscover redirect Address from Exchange On-Premise.
                if (session.utilFindInResponse("<RedirectAddr>", false) > 1)
                {
                    if (session.utilFindInResponse("</RedirectAddr>", false) > 1)
                    {
                        session["ui-backcolor"] = "green";
                        session["ui-color"] = "black";
                    }
                }
            }
            //
            //  HTTP 401: UNAUTHORIZED.
            //
            else if (session.responseCode == 401)
            {
                session["ui-backcolor"] = "orange";
                session["ui-color"] = "black";
            }
            //
            //  HTTP 403: FORBIDDEN.
            //
            // Simply looking for the term "Access Denied" works fine using utilFindInResponse.
            else if (session.responseCode == 403)
            {
                // Specific scenario where a web proxy is blocking traffic.
                if (session.utilFindInResponse("Access Denied", false) > 1)
                {
                    session["ui-backcolor"] = "red";
                    session["ui-color"] = "black";
                }
                else
                {
                    // Pick up any 403 Forbidden and write data into the comments box.
                    session["ui-backcolor"] = "red";
                    session["ui-color"] = "black";
                }
            }
            //
            //  HTTP 404: Not Found.
            //
            else if (session.responseCode == 404)
            {
                // Pick up any 404 Not Found and write data into the comments box.
                session["ui-backcolor"] = "orange";
                session["ui-color"] = "black";
            }
            //
            //  HTTP 500: Internal Server Error.
            //
            else if (session.responseCode == 500)
            {
                // Pick up any 500 Internal Server Error and write data into the comments box.
                // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in green? >
                session["ui-backcolor"] = "red";
                session["ui-color"] = "black";
            }
            //
            //  HTTP 502: BAD GATEWAY.
            //
            else if (session.responseCode == 502)
            {
                // Specific scenario on Outlook & OFffice 365 Autodiscover false positive on connections to:
                //      autodiscover.domain.onmicrosoft.com:443
                if (session.utilFindInResponse("autodiscover", false) > 1)
                {
                    if (session.utilFindInResponse("target machine actively refused it", false) > 1)
                    {
                        if (session.utilFindInResponse(":443", false) > 1)
                        {
                            session["ui-backcolor"] = "green";
                            session["ui-color"] = "black";
                        }
                    }
                }
                // Specific scenario on Outlook and Office 365 invalid DNS lookup.
                // < Discuss and confirm thinking here, validate with a working trace. Is this a true false positive? Highlight in green? >
                else if (session.utilFindInResponse("DNS Lookup for ", false) > 1)
                {
                    if (session.utilFindInResponse("mail.onmicrosoft.com", false) > 1)
                    {
                        if (session.utilFindInResponse("failed.System.Net.Sockets.SocketException", false) > 1)
                        {
                            if (session.utilFindInResponse("The requested name is valid, but no data of the requested type was found", false) > 1)
                            {
                                session["ui-backcolor"] = "green";
                                session["ui-color"] = "black";
                            }
                        }
                    }
                }
                else
                {
                    // Pick up any other 502 Bad Gateway call it out.
                    session["ui-backcolor"] = "red";
                    session["ui-color"] = "black";
                }
            }
            //
            //  HTTP 503: SERVICE UNAVAILABLE.
            //
            // Using utilFindInResponse to find FederatedStsUnreachable did not work for some reason.
            // So instead split all words in the response body and check them with Linq.
            else if (session.responseCode == 503)
            {
                // Specific scenario where Federation service is unavailable, preventing authentication, preventing access to Office 365 mailbox.
                if (searchTerm == "FederatedStsUnreachable")
                {
                    session["ui-backcolor"] = "red";
                    session["ui-color"] = "black";
                }
                else
                {
                    // Pick up any other 503 Service Unavailable call it out.
                    session["ui-backcolor"] = "red";
                    session["ui-color"] = "black";
                }
            }
            //
            //  HTTP 504: GATEWAY TIMEOUT.
            //
            else if (session.responseCode == 504)
            {
                // Pick up any 504 Gateway Timeout and call it out.
                session["ui-backcolor"] = "red";
                session["ui-color"] = "black";
            }

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

    public void OnBeforeUnload() { }

    // Make sure the Columns are added to the UI.
    private void EnsureColumn()
    {
        if (bCreatedColumn) return;

        FiddlerApplication.UI.lvSessions.AddBoundColumn("Response Time", 2, 110, "X-iTTLB");
        
        bCreatedColumn = true;
    }

    //oSession["X-Privacy"] = "Sets cookies & P3P";



    public void OnPeekAtResponseHeaders(IAutoTamper2 AllSessions)
    {

    }

    public void AutoTamperRequestBefore(Session oSession)
    {
        //if (oSession.hostname.Contains("Outlook"))
        
            
            //oSession.oRequest["User-Agent"] = sUserAgent;

    }
    public void AutoTamperRequestAfter(Session oSession) { }
    public void AutoTamperResponseBefore(Session oSession) { }
    public void AutoTamperResponseAfter(Session oSession) {
        oSession["X-iTTLB"] = oSession.oResponse.iTTLB.ToString();
    }


    public void OnBeforeReturningError(Session oSession) { }



}