using Fiddler;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    /// <summary>
    /// Found that 'this.session.utilFindInResponse' does not work for HTTP 503 finding 'FederatedSTSUnreachable'.
    /// Oddly it works for the ADAL Saml response searches, but I know doubt how robust it is.
    /// REVIEW THIS -- Switch all utilFindInResponse to use this utility function instead.
    /// Instead this utility function serves as a way to search the session for keywords.
    /// </summary>
    internal class SessionWordSearch
    {
        internal Session session { get; set; }

        private static SessionWordSearch _instance;
        public static SessionWordSearch Instance => _instance ?? (_instance = new SessionWordSearch());

        public int Search(Session session, String searchTerm)
        {
            this.session = session;

            string sessionString = session.ToString();

            //Convert the string into an array of words  
            string[] source = sessionString.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Create the query. Use ToLowerInvariant to match "data" and "Data"   
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                             select word;

            // Count the matches, which executes the query.  
            return matchQuery.Count();
        }
    }
}
