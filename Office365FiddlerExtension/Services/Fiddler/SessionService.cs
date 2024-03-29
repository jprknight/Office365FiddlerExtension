using Fiddler;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtension.UI;
using System.Linq;
using System.Reflection;

namespace Office365FiddlerExtension
{
    /// <summary>
    /// Function that calls ruleset to run on loaded sessions.
    /// The call to ActivationService here runs the application.
    /// </summary>
    public class SessionService : ActivationService
    {
        private static SessionService _instance;

        public static SessionService Instance => _instance ?? (_instance = new SessionService());

        public void OnPeekAtResponseHeaders(Session Session)
        {
            this.session = Session;

            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            RulesetService.Instance.RunRuleSet(this.session);

            EnhanceSessionUX.Instance.EnhanceSession(this.session);
        }

        public int AllSessionsCount()
        {
            return FiddlerApplication.UI.GetAllSessions().Count();
        }
    }
}