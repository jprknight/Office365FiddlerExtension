using EXOFiddlerInspector.Services;
/// <summary>
/// SessionRuleSet class. All extension session logic lives here.
/// Anything which involves extensive logic for session values the extension uses should live here.
/// </summary>
public class SessionRuleSet : ActivationService
{
    private static SessionRuleSet _instance;

    public static SessionRuleSet Instance => _instance ?? (_instance = new SessionRuleSet());

    public SessionRuleSet()
    {

    }

}