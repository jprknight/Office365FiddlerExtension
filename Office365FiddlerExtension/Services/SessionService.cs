using Fiddler;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtension.UI;

namespace Office365FiddlerExtension
{
    /// <summary>
    /// Function that calls ruleset to run on loaded sessions.
    /// </summary>
    public class SessionService : ActivationService
    {
        private static SessionService _instance;

        public static SessionService Instance => _instance ?? (_instance = new SessionService());

        public void OnPeekAtResponseHeaders(Session Session)
        {
            this.Session = Session;

            this.Session.utilDecodeRequest(true);
            this.Session.utilDecodeResponse(true);

            RulesetService.RunRuleSet(this.Session);

            EnhanceSessionUX.Instance.EnhanceSession(this.Session);
        }
    }
}