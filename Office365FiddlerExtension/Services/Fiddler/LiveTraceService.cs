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

            // If the session has the imported session flag on it, presedence is given to the ImportService class.
            if (SessionService.Instance.IsSessionImported(this.session))
            {
                return;
            }

            // If the session has the loaded from SAZ flag on it, presedence is given to the SazFileService class.
            if (SessionService.Instance.IsSessionLoadedFromSaz(this.session))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Attempting to analyse session {this.session.id}.");

            try
            {
                SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                this.session.RefreshUI();
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
        }
    }
}
