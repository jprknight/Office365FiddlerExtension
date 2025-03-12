using Fiddler;
using Office365FiddlerExtension.UI;
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

                // Only if the LargeSessionAnalysisApproval is false, prompt the user.
                // Noticed an issue where the user is prompted multiple times during an import.
                // Need a way of ensuring the user is only prompted once.
                if (!SettingsJsonService.Instance.GetDeserializedExtensionSettings().LargeSessionAnalysisApproval)
                {
                    SessionService.Instance.ConfirmLargeSessionAnalysis(Sessions.Length);
                }

                // Don't return here. bConfirmLargeSessionAnalysis is used below to stop any new ruleset processing
                // with OnPeekAtResponseHeaders.

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
                    var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

                    // If the session already has the Microsoft365FiddlerExtensionJson flag set with high confidence session classifications set,
                    // enhance the session based on prior / stored analysis.
                    // Doubtful we hit this with an import, but left just to future proof the code.
                    if (ExtensionSessionFlags.SessionAuthenticationConfidenceLevel == 10
                        && ExtensionSessionFlags.SessionResponseServerConfidenceLevel == 10
                        && ExtensionSessionFlags.SessionTypeConfidenceLevel == 10)
                    {
                        EnhanceSessionUX.Instance.EnhanceSession(this.session);
                    }
                    else
                    {
                        // Only run session analysis if:
                        //   There's more sessions than the warning threshold AND the user has confirmed they want large session analysis.
                        //   There's fewer sessions than the warning threshold.
                        if (Sessions.Count() >= extensionSettings.WarnBeforeAnalysing 
                            && extensionSettings.LargeSessionAnalysisApproval)
                        {
                            SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                        }
                        else if (Sessions.Count() < extensionSettings.WarnBeforeAnalysing)
                        {
                            SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                        }

                    }

                    // Update status bar with import progress.
                    StatusBar.Instance.UpdateStatusBarOnSessionProgression(this.session.id, Sessions.Count());
                }

                sw.Stop();

                // Reset the LargeSessionAnalysisApproval to false, so the user is prompted on any subsequent imports.
                SettingsJsonService.Instance.SetLargeSessionAnalysisApproval(false);

                // Update status bar once completed.
                StatusBar.Instance.UpdateStatusBarOnSessionProcessComplete(sw, Sessions.Count());

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
