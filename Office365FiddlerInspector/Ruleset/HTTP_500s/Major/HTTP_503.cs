using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_503 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_503_Service_Unavailable_Federated_STS_Unreachable_or_Unavailable(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            //  HTTP 503: SERVICE UNAVAILABLE.
            //
            // 503.1. Call out all 503 Service Unavailable as something to focus on.
            String searchTerm = "FederatedStsUnreachable";
            //"Service Unavailable"

            // Count the occurrences of common search terms match up to certain HTTP response codes to highlight certain scenarios.
            //
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-count-occurrences-of-a-word-in-a-string-linq
            //

            int wordCount = 0;

            string text503 = session.ToString();

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
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 503 Service Unavailable. FederatedStsUnreachable in response body!");

                getSetSessionFlags.SetUIBackColour(this.session, "Red");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetResponseCodeDescription(this.session, "503 Federation Service Unavailable");

                this.session["X-SessionType"] = "***FederatedSTSUnreachable***";

                string RealmURL = "https://login.microsoftonline.com/GetUserRealm.srf?Login=" + this.session.oRequest["X-User-Identity"] + "&xml=1";

                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>FederatedSTSUnreachable</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "<b><span style='color:red'>HTTP 503: FederatedSTSUnreachable</span></b>."
                    + "<b><span style='color:red'>The fedeation service is unreachable or unavailable</span></b>."
                    + "<p><b><span style='color:red'>Troubleshoot this issue first before doing anything else.</span></b></p>"
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
                    + "Further investigation is advised. You could try hitting these endpoints a few times and see if you get an intermittent failure.</p>");

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }

        public void HTTP_503_Service_Unavailable_Everything_Else(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            // 503.99. Everything else.
            //
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 503 Service Unavailable (99).");

            getSetSessionFlags.SetUIBackColour(this.session, "Red");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "503 Service Unavailable");

            getSetSessionFlags.SetSessionType(this.session, "!Service Unavailable!");
            getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 503 Service Unavailable</span></b>");
            getSetSessionFlags.SetXResponseComments(this.session, "<b><span style='color:red'>Server that was contacted in this session reports "
                + "it is unavailable</span></b>. Look at the server that issued this response, it is healthy? Contactable? "
                + "Contactable consistently or intermittently? Consider other session server responses in the 500's (500, 502 or 503) in conjunction with this session.");

            // Possible something more to be found, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "5");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }
    }
}