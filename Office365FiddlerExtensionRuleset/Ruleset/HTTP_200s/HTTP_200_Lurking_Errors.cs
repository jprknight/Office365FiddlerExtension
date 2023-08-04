﻿using Fiddler;
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

            int wordCountError = SessionWordSearch.Instance.Search(this.session, "Error");
            int wordCountFailed = SessionWordSearch.Instance.Search(this.session, "failed");
            int wordCountException = SessionWordSearch.Instance.Search(this.session, "exception");

            string wordCountErrorText;
            string wordCountFailedText;
            string wordCountExceptionText;

            if (wordCountError == 0 && wordCountFailed == 0 && wordCountException == 0)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 FAILURE LURKING!?");

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

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Lurking_Errors");
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