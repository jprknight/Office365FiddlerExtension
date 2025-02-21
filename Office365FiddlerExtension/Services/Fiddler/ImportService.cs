using Fiddler;
using Office365FiddlerExtension.UI;
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

            var Sessions = FiddlerApplication.UI.GetAllSessions();

            // Start out by saying user is happy to perform large session analysis.
            bool bConfirmLargeSessionAnalysis;

            bConfirmLargeSessionAnalysis = SessionService.Instance.ConfirmLargeSessionAnalysis(Sessions.Length);

            // Start the stopwatch. This should be the last thing that happens before we start the foreach loop through sessions.
            var sw = Stopwatch.StartNew();

            foreach (var Session in Sessions)
            {
                this.session = Session;

                // If the session doesn't have the ImportedFromOtherTool flag set, ignore it.
                if (!this.session.isAnyFlagSet(SessionFlags.ImportedFromOtherTool))
                {
                    continue;
                }

                var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

                // If the session already has the Microsoft365FiddlerExtensionJson flag set with high confidence session classifications set,
                // enhance the session based on prior / stored analysis.
                if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel == 10
                    && ExtensionSessionFlags.SessionResponseServerConfidenceLevel == 10
                    && ExtensionSessionFlags.SessionTypeConfidenceLevel == 10)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"Enhancing {this.session.id} based on existing session flags ({ExtensionSessionFlags.SessionType}).");

                    EnhanceSessionUX.Instance.EnhanceSession(this.session);
                }
                else
                {
                    // Check ensures the user has confirmed they want to continue for large session analysis.
                    if (bConfirmLargeSessionAnalysis)
                    {
                        SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                    }
                }

                // Update status bar with import progress.
                StatusBar.Instance.UpdateStatusBarOnSessionProgression(this.session.id, Sessions.Count());
            }

            sw.Stop();

            // Update status bar once completed.
            StatusBar.Instance.UpdateStatusBarOnSessionProcessComplete(sw, Sessions.Count());

            FiddlerApplication.UI.lvSessions.EndUpdate();
        }
    }
}
