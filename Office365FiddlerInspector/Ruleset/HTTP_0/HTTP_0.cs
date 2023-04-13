using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_0 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        //private static HTTP_0 _instance;

        //public static HTTP_0 Instance => _instance ?? (_instance = new HTTP_0());

        // internal Session session { get; set; }
        public void HTTP_0_NoSessionResponse(Session session)
        {
            this.session = session;

            /////////////////////////////
            //
            //  HTTP 0: No Response.
            FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 0 No response");

            getSetSessionFlags.SetUIBackColour(this.session, "Red");
            getSetSessionFlags.SetUITextColour(this.session, "Black");

            getSetSessionFlags.SetResponseCodeDescription(this.session, "0 No Response");

            getSetSessionFlags.SetSessionType(this.session, "!NO RESPONSE!");
            getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 0 - No Response</span></b>");
            getSetSessionFlags.SetXResponseComments(this.session, "The quantity of these types of server errors need to be considered in context with what you are "
                + "troubleshooting and whether these are relevant or not. A small number is probably not an issue, larger numbers of these could "
                + "be cause for concern."
                + "<p>If you are not seeing expected client traffic, consider if network traces should be collected. Review if there is an underlying "
                + "network issue such as congestion on routers, which could be causing issues. The Network Connection Status Indicator (NCSI) on the "
                + "client computer might also be an area to investigate.</p>");

            // This actually isn't very useful, let further processing try to pick up something.
            getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
            getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "5");
            getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
        }   
    }
}