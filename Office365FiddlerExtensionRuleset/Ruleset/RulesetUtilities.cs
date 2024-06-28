using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Linq;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    /// <summary>
    /// Found that 'this.session.utilFindInResponse' does not work for HTTP 503 finding 'FederatedSTSUnreachable'.
    /// Oddly it works for the ADAL Saml response searches, but I know doubt how robust it is.
    /// REVIEW THIS - Checked March 2024 - Simplied search function, found this works in some places where symbols aren't in the search string.
    /// Leaving utilFindInResponse used where I don't have test data to validate a switch over.
    /// Instead this utility function serves as a way to search the session for keywords.
    /// </summary>
    internal class RulesetUtilities
    {
        internal Session session { get; set; }

        private static RulesetUtilities _instance;
        public static RulesetUtilities Instance => _instance ?? (_instance = new RulesetUtilities());

        /// <summary>
        /// Search for a word in a string. Split words in the string by spaces and these symbols: . ? ! ; : ,
        /// </summary>
        /// <param name="session"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public int SearchForWord(Session session, String searchTerm)
        {
            this.session = session;

            //Convert the string into an array of words  
            string[] source = this.session.ToString().Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                             select word;

            // Count the matches, which executes the query.  
            return matchQuery.Count();
        }

        /// <summary>
        /// Search for a phrase in a string.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public bool SearchForPhrase(Session session, String searchTerm)
        {
            this.session = session;

            if (!this.session.ToString().Contains(searchTerm))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Used to return a boolean value on whether the session type confidence level has already been set to 10 or not.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool StopProcessing_SessionTypeConfidenceLevel_Ten(Session session)
        {
            this.session = session;

            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Used to return a boolean value on whether the session authentication confidence level has already been set to 10 or not.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool StopProcessing_SessionAuthenticationConfidenceLevel_Ten(Session session)
        {
            this.session = session;

            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Used to return a boolean value on whether the session response server confidence level has already been set to 10 or not.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool StopProcessing_SessionResponseServerConfidenceLevel_Ten(Session session)
        {
            this.session = session;

            if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Used to return a boolean value on whether the session type and response server confidence levels have both already been set to 10 or not.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool SessionAnalysisCompleted(Session session)
        {
            this.session = session;

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            // Session Analysis IS completed.

            // Session analysis here means only SessionTypeConfidenceLevel and SessionResponseServerConfidenceLevel.
            // SessionAuthenticationConfidenceLevel is the last thing to be worked out so isn't used here.
            if (ExtensionSessionFlags.SessionTypeConfidenceLevel == 10
                && ExtensionSessionFlags.SessionResponseServerConfidenceLevel == 10)
            //&& ExtensionSessionFlags.SessionAuthenticationConfidenceLevel < 10)
            {
                return true;
            }

            // Session Analysis is NOT completed.
            return false;
        }
    }
}
