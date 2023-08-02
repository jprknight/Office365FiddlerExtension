using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_Lurking_Errors
    {
        internal Session session { get; set; }

        private static HTTP_200_Lurking_Errors _instance;

        public static HTTP_200_Lurking_Errors Instance => _instance ?? (_instance = new HTTP_200_Lurking_Errors());

        /// <summary>
        /// Function to look for lurking errors, failures, and exceptions in HTTP 200s.
        /// Exclude any session which contains a content-type of javascript.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // REVIEW THIS -- Call SessionWordSearch utility function and clean this up.
            // REVEIW THIS -- Clean up all this commented code once this tests out as working.

            //string searchTerm = "empty";

            /////////////////////////////
            //
            // All other specific scenarios, fall back to looking for errors lurking in HTTP 200 OK responses.

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

            //string text200 = this.session.ToString();

            // Convert the string into an array of words
            // 7/15/2021 Added '"' to split out "Error" and count these.
            //string[] source200 = text200.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '"' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
            //var matchQuery200 = from word in source200
            //                   where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
            //                 select word;

            //searchTerm = "Error";

            // Count the matches, which executes the query.  
            //wordCountError = matchQuery200.Count();

            //searchTerm = "failed";

            // Count the matches, which executes the query.  
            //wordCountFailed = matchQuery200.Count();

            //searchTerm = "exception";

            // Count the matches, which executes the query.  
            //wordCountException = matchQuery200.Count();

            // If either the keyword searches give us a result.
            if (SessionWordSearch.Instance.Search(this.session, "Error") > 0 ||
                SessionWordSearch.Instance.Search(this.session, "failed") > 0 ||
                SessionWordSearch.Instance.Search(this.session, "exception") > 0)
            {
                if (wordCountError == 0)
                {
                    wordCountErrorText = $"<b><span style='color:green'>Keyword 'Error' found {wordCountError} times.</span></b>";
                }
                else if (wordCountError == 1)
                {
                    wordCountErrorText = $"<b><span style='color:red'>Keyword 'Error' found {wordCountError} time.</span></b>";
                }
                else
                {
                    wordCountErrorText = $"<b><span style='color:red'>Keyword 'Error' found {wordCountError} times.</span></b>";
                }

                if (wordCountFailed == 0)
                {
                    wordCountFailedText = $"<b><span style='color:green'>Keyword 'Failed' found {wordCountFailed} times.</span></b>";
                }
                else if (wordCountFailed == 1)
                {
                    wordCountFailedText = $"<b><span style='color:red'>Keyword 'Failed' found {wordCountFailed} time.</span></b>";
                }
                else
                {
                    wordCountFailedText = $"<b><span style='color:red'>Keyword 'Failed' found {wordCountFailed} times.</span></b>";
                }

                if (wordCountException == 0)
                {
                    wordCountExceptionText = $"<b><span style='color:green'>Keyword 'Exception' found {wordCountException} times.</span></b>";
                }
                else if (wordCountException == 1)
                {
                    wordCountExceptionText = $"<b><span style='color:red'>Keyword 'Exception' found {wordCountException} time.</span></b>";
                }
                else
                {
                    wordCountExceptionText = $"<b><span style='color:red'>Keyword 'Exception' found {wordCountException} times.</span></b>";
                }

                // Special attention to HTTP 200's where the keyword 'error' or 'failed' is found.

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 FAILURE LURKING!?");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP200s|HTTP_200_Lurking_Errors");
                    sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                    sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                    sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                    sessionSeverity = sessionClassificationJson.SessionSeverity;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                    sessionAuthenticationConfidenceLevel = 5;
                    sessionTypeConfidenceLevel = 10;
                    sessionResponseServerConfidenceLevel = 5;
                    sessionSeverity = 50;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s_Lurking_Errors",

                    SessionType = "!FAILURE LURKING!",
                    ResponseCodeDescription = "200 OK, but possibly bad.",
                    ResponseAlert = "<b><span style='color:red'>'error', 'failed' or 'exception' found in response body</span></b>",
                    ResponseComments = "<p>Session response body was scanned and errors or failures were found in response body. "
                    + "Check the Raw tab, click 'View in Notepad' button bottom right, and search for error in the response to review.</p>"
                    + "<p>After splitting all words in the response body the following were found:</p>"
                    + "<p>" + wordCountErrorText + "</p>"
                    + "<p>" + wordCountFailedText + "</p>"
                    + "<p>" + wordCountExceptionText + "</p>"
                    + "<p>Check the content body of the response for any failures you recognise. You may find <b>false positives, "
                    + "if lots of Javascript or other web code</b> is being loaded.</p>",

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }
    }
}
