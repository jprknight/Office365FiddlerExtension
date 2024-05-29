using System.Reflection;
using Fiddler;
using Office365FiddlerExtensionRuleset.Ruleset;

namespace Office365FiddlerExtensionRuleset
{
    public class RunRuleSet
    {
        internal Session session { get; set; }

        /// <summary>
        /// This should be considered the main constructor for the extension ruleset DLL.
        /// </summary>
        /// <param name="session"></param>
        public void Initialize(Session session)
        {
            this.session = session;

            // Only want to see this once in the Fiddler log.
            if (this.session.id == 1)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): Starting v" +
                    $"{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                    $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}." +
                    $"{Assembly.GetExecutingAssembly().GetName().Version.Build}");
            }

            ///////////////////////////////
            /// Always run these functions on every session.
            /// Broad logic checks on sessions regardless of response code.
            FiddlerUpdateSessions.Instance.Run(this.session);

            ApacheAutodiscover.Instance.Run(this.session);
            
            ConnectTunnelTLSVersion.Instance.Run(this.session);
            
            LoopBackTunnel.Instance.Run(this.session);
            
            /// Populate session flag data.
            CalculateSessionAge.Instance.Run(this.session);
            
            ServerThinkTimeTransitTime.Instance.Run(this.session);
            
            SessionElapsedTime.Instance.Run(this.session);
            
            ProcessName.Instance.Run(this.session);
            
            HostIP.Instance.Run(this.session);

            ///
            ///////////////////////////////
            /// Run code based on response code in session.
            RunRulesetResponseCodes.Instance.Run(this.session);

            ///
            ///////////////////////////////
            /// Run the remainder of these classes if the session hasn't been completely classified yet.
            Authentication.Instance.Run(this.session);

            SessionType.Instance.Run(this.session);

            ResponseServer.Instance.Run(this.session);

            LongRunningSessions.Instance.Run(this.session);
        }
    }
}
