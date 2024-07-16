using System;
using System.Reflection;
using Fiddler;

namespace Office365FiddlerExtension.Services.Fiddler
{
    public class LiveTraceService
    {
        internal Session session { get; set; }

        private static LiveTraceService _instance;

        public static LiveTraceService Instance => _instance ?? (_instance = new LiveTraceService());

        public void ProcessStreamedSessions(Session session)
        {
            this.session = session;

            if (!SettingsJsonService.Instance.ExtensionSessionProcessingEnabled)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Extension not enabled, returning.");
                return;
            }

            // If session analysis on live trace is enabled, run.
            if (SettingsJsonService.Instance.SessionAnalysisOnLiveTrace)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to analyse session {this.session.id}.");

                try
                {
                    SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                    this.session.RefreshUI();
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
                }
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"Live trace set to {SettingsJsonService.Instance.SessionAnalysisOnLiveTrace} preventing session analysis for session id {this.session.id}.");
            }
        }
    }
}
