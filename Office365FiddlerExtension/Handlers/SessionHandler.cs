using Fiddler;
using Office365FiddlerExtension.Handler;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtension.UI;

namespace Office365FiddlerExtension
{
    /// <summary>
    /// Function that calls ruleset to run on loaded sessions.
    /// </summary>
    public class SessionHandler : ActivationService
    {
        private static SessionHandler _instance;

        public static SessionHandler Instance => _instance ?? (_instance = new SessionHandler());

        public void OnPeekAtResponseHeaders(Session Session)
        {
            this.Session = Session;

            this.Session.utilDecodeRequest(true);
            this.Session.utilDecodeResponse(true);

            RuleSetHandler.RunRuleSet(this.Session);

            EnhanceSessionUX.Instance.EnhanceSession(this.Session);
        }
    }
}