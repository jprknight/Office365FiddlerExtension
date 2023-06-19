using Fiddler;
using Office365FiddlerExtension.Services;
using Newtonsoft.Json;

namespace Office365FiddlerExtension.Ruleset
{
    class FiddlerUpdateSessions
    {
        internal Session Session { get; set; }

        public void FUS(Session session)
        {
            this.Session = session;

            if (this.Session.hostname == "www.fiddler2.com" && this.Session.uriContains("UpdateCheck.aspx"))
            {
                var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
                {
                    SectionTitle = "Broad Logic Checks",
                    UIBackColour = "Gray",
                    UITextColour = "Black",

                    SessionType = "Fiddler Update Check",
                    ResponseServer = "Fiddler Update Check",
                    ResponseAlert = "Fiddler Update Check",
                    ResponseCodeDescription = "Fiddler Update Check",
                    ResponseComments = "This is Fiddler itself checking for updates. It has nothing to do with the Office 365 Fiddler Extension.",
                    Authentication = "Fiddler Update Check",

                    SessionAuthenticationConfidenceLevel = 10,
                    SessionTypeConfidenceLevel = 10,
                    SessionResponseServerConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagHandler.Instance.UpdateSessionFlagJson(this.Session, sessionFlagsJson);
            }
        }
    }
}