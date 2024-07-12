using Fiddler;
using Office365FiddlerExtension.UI;
using System.Diagnostics;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    public class ImportService
    {
        internal Session session { get; set; }

        private static ImportService _instance;

        public static ImportService Instance => _instance ?? (_instance = new ImportService());

        public void ImportSessions()
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

            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            var Sessions = FiddlerApplication.UI.GetAllSessions();

            // This is where the 'cumbersome' kicks in: https://feedback.telerik.com/fiddler/1657770-fiddler-classic-should-expose-onimportsessions-event
            // Without OnImportSession, need to do extra work to determine what sessions are loaded from a Saz file and which are not.

            // Start out saying sessions haven't been loaded from SAZ file.
            bool bSessionsLoadedFromSAZ = false;
            bool bSessionsResponseStreamed = false;

            // Inspect loaded sessions to determine if they have been loaded from SAZ file.
            // If any have been loaded from SAZ file, we'll have to make the assumption they all were,
            // and SazFileService will pick them all up.

            foreach (var Session in Sessions)
            {
                this.session = Session;

                if (this.session.isAnyFlagSet(SessionFlags.ResponseStreamed))
                {
                    bSessionsResponseStreamed = true;
                }

                if (bSessionsLoadedFromSAZ)
                {
                    continue;
                }

                // If the session have the LoadedFromSAZ flag set.
                // If they do store this for further logic checks down the line, so we don't redo tasks.
                if (this.session.isAnyFlagSet(SessionFlags.LoadedFromSAZ))
                {
                    bSessionsLoadedFromSAZ = true;
                }
            }

            // If we determined sessions were loaded from a SAZ file. return.
            if (bSessionsLoadedFromSAZ)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"Import Service detected sessions loaded from Saz file {bSessionsLoadedFromSAZ}, passing processing over to SazFileService.");
                return;
            }

            // Start out by saying user is happy to perform large session analysis.
            bool bConfirmLargeSessionAnalysis = true;

            // If there are more sessions to analyse than the warning threshold, confirm with the user they want to continue.
            // Also confirm the sessions added are not FromLoadSAZ, these should be processed within the SazFileService.
            if (Sessions.Length > extensionSettings.WarnBeforeAnalysing && !bSessionsLoadedFromSAZ)
            {
                // If the sessions have the response streamed (live traced) session flag, don't do this.
                if (!bSessionsResponseStreamed)
                {
                    bConfirmLargeSessionAnalysis = SessionService.Instance.ConfirmLargeSessionAnalysis(Sessions.Length);
                }
            }

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
            }

            sw.Stop();

            FiddlerApplication.UI.lvSessions.EndUpdate();

            if (!bSessionsLoadedFromSAZ)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"Import Service processed {Sessions.Length} sessions in {sw.ElapsedMilliseconds}ms.");
            }
        }
    }
}
