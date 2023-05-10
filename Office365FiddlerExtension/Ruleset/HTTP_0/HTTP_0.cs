using Office365FiddlerExtension.Services;
using Fiddler;

namespace Office365FiddlerExtension.Ruleset
{
    class HTTP_0 : ActivationService
    {
        private static HTTP_0 _instance;

        public static HTTP_0 Instance => _instance ?? (_instance = new HTTP_0());

        public void HTTP_0_NoSessionResponse(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            //  HTTP 0: No Response.
            //
            GetSetSessionFlags.Instance.WriteToFiddlerLog(this.session, "HTTP 0 No response"); 

            GetSetSessionFlags.Instance.SetUIBackColour(this.session, "Red");
            GetSetSessionFlags.Instance.SetUITextColour(this.session, "Black");

            GetSetSessionFlags.Instance.SetResponseCodeDescription(this.session, "0 No Response");

            GetSetSessionFlags.Instance.SetSessionType(this.session, "!NO RESPONSE!");
            GetSetSessionFlags.Instance.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 0 - No Response</span></b>");
            GetSetSessionFlags.Instance.SetXResponseComments(this.session, "The quantity of these types of server errors need to be considered in context with what you are "
                + "troubleshooting and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could "
                + "be cause for concern."
                + "<p>If you are not seeing expected client traffic, consider if network traces should be collected. Review if there is an underlying "
                + "network issue such as congestion on routers, which could be causing issues. The Network Connection Status Indicator (NCSI) on the "
                + "client computer might also be an area to investigate.</p>");

            // This actually isn't very useful, let further processing try to pick up something.
            GetSetSessionFlags.Instance.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionTypeConfidenceLevel(this.session, "5");
            GetSetSessionFlags.Instance.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }   
    }
}