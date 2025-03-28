using Fiddler;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    public class ImportService
    {
        internal Session session { get; set; }

        private static ImportService _instance;

        public static ImportService Instance => _instance ?? (_instance = new ImportService());

        public void ProcessImportedSessions()
        {
            if (!SettingsJsonService.Instance.ExtensionSessionProcessingEnabled)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Extension not enabled, not allowing compute intensive tasks.");
                return;
            }

            if (!SettingsJsonService.Instance.SessionAnalysisOnImport)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Session analysis on import not enabled, returning.");
                return;
            }

            try
            {
                var Sessions = FiddlerApplication.UI.GetAllSessions();

                // Start the stopwatch. This should be the last thing that happens before we start the foreach loop through sessions.
                var sw = Stopwatch.StartNew();

                int SessionsProcessedCount = 0;

                foreach (var Session in Sessions)
                {
                    this.session = Session;

                    // If the session doesn't have the ImportedFromOtherTool flag set, ignore it.
                    if (!this.session.isAnyFlagSet(SessionFlags.ImportedFromOtherTool))
                    {
                        continue;
                    }

                    // User interruption of session processing.
                    if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().InterruptSessionProcessing)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz processing interrupted by user.");
                        break;
                    }
                    
                    SessionService.Instance.OnPeekAtResponseHeaders(this.session);

                    SessionsProcessedCount++;

                    // Update status bar with import progress.
                    StatusBar.Instance.UpdateStatusBarOnSessionProgression(SessionsProcessedCount, Sessions.Count());
                }

                sw.Stop();

                // Reset the interrupt session processing flag.
                if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().InterruptSessionProcessing)
                {
                    SettingsJsonService.Instance.SetInterruptSessionProcessing(false);

                }

                // Update status bar once completed.
                StatusBar.Instance.UpdateStatusBarOnSessionProcessComplete(sw, SessionsProcessedCount);

                FiddlerApplication.UI.lvSessions.EndUpdate();
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }            
        }
    }
}
