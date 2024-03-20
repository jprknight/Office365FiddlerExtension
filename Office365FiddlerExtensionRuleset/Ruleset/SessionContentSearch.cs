using Fiddler;
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
    internal class SessionContentSearch
    {
        internal Session session { get; set; }

        private static SessionContentSearch _instance;
        public static SessionContentSearch Instance => _instance ?? (_instance = new SessionContentSearch());

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

        public bool SearchForPhrase(Session session, String searchTerm)
        {
            this.session = session;

            if (!this.session.ToString().Contains(searchTerm))
            {
                return false;
            }

            return true;
        }
    }
}
