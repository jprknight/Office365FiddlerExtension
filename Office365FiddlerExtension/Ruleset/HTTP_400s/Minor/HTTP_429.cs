using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_429 : ActivationService
    {
        private static HTTP_429 _instance;

        public static HTTP_429 Instance => _instance ?? (_instance = new HTTP_429());

        public void HTTP_429_Too_Many_Requests(Session session)
        {
            this.session = session;

            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 429 Too many requests.");

            // Setting to gray, to be convinced these are important to Microsoft 365 traffic.
            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Orange");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "429 Too Many Requests (RFC 6585)");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "HTTP 429 Too Many Requests");

            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 429 Too Many Requests</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session,"These responses need to be taken into context with the rest of the " + 
                "sessions in the trace. A small number is probably not an issue, larger numbers of these could be cause for concern.");

            // Nothing meaningful here, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "0");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "0");
        }
    }
}