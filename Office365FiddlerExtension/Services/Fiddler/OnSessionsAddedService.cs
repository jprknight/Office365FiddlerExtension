using System.Reflection;
using Fiddler;

namespace Office365FiddlerExtension.Services
{
    public class OnSessionsAddedService
    {
        internal Session session { get; set; }

        private static OnSessionsAddedService _instance;

        public static OnSessionsAddedService Instance => _instance ?? (_instance = new OnSessionsAddedService());

        public void ProcessSessions()
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            // If the extension isn't enabled, don't do anything here.
            if (!extensionSettings.ExtensionSessionProcessingEnabled)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Extension not enabled, not allowing compute intensive tasks.");
                return;
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"Processing {SessionsLoadedFromSazCount()} session(s) loaded from Saz file, " +
                    $"{SessionsImportedFromOtherToolCount()} session(s) imported, " +
                    $"{SessionsStreamedCount()} session(s) live traced in the Fiddler view.");
            }

            // OnSessionsAdded doesn't appear to have any overloads for the sessions which were just added.
            // For this reason the extension has to make this assumption:
            // Session(s) just added are all of the same source LoadSaz, Import, or Streamed (Live Trace).
            // These three types should be mutually exclusive, the event OnImportSession could / should clarify recently added events against others.
            // As of now (2024) this event doesn't exist in Fiddler.

            // This is where the 'cumbersome' kicks in: https://feedback.telerik.com/fiddler/1657770-fiddler-classic-should-expose-onimportsessions-event
            // Without OnImportSession, need to do extra work to determine what sessions are loaded from a Saz file and which are not.

            // To overcome any challenges, the following process checks for the method the sessions in the Fiddler view were captured with,
            // ignoring any which the extension has already analysed along the way.

            if (SessionsImportedFromOtherToolCount() != 0)
            {
                ImportService.Instance.ProcessImportedSessions();
            }
            else if (SessionsLoadedFromSazCount() != 0)
            {
                // Do nothing here, SazFileService will pick up these sessions.
            }
            else if (SessionsStreamedCount() != 0)
            {
                // Do nothing here, LiveTraceService will pick up these sessions.
            }
            else
            {
                // Do nothing here.
            }
        }

        private int SessionsLoadedFromSazCount()
        {
            var Sessions = FiddlerApplication.UI.GetAllSessions();

            int iSessions = 0;

            foreach (var Session in Sessions)
            {
                this.session = Session;

                // If the current session has been analysed with the extension already, skip over it for the purposes of determining if
                // any new sessions have been loaded from a Saz file.
                if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10
                    && SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10
                    && SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                {
                    continue;
                }

                if (this.session.isAnyFlagSet(SessionFlags.LoadedFromSAZ))
                {
                    iSessions++;
                }
            }

            return iSessions;
        }

        private int SessionsImportedFromOtherToolCount()
        {
            var Sessions = FiddlerApplication.UI.GetAllSessions();

            int iSessions = 0;

            foreach (var Session in Sessions)
            {
                this.session = Session;

                // If the current session has been analysed with the extension already, skip over it for the purposes of determining if
                // any new sessions have been imported.
                if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10
                    && SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10
                    && SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                {
                    continue;
                }

                if (this.session.isAnyFlagSet(SessionFlags.ImportedFromOtherTool))
                {
                    iSessions++;
                }
            }

            return iSessions;
        }

        private int SessionsStreamedCount()
        {
            var Sessions = FiddlerApplication.UI.GetAllSessions();

            int iSessions = 0;

            foreach (var Session in Sessions)
            {
                this.session = Session;

                // If the current session has been analysed with the extension already, skip over it for the purposes of determining if
                // any new sessions have been streamed (live trace).
                if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10
                    && SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10
                    && SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                {
                    continue;
                }

                if (this.session.isAnyFlagSet(SessionFlags.ResponseStreamed) && this.session.isAnyFlagSet(SessionFlags.ImportedFromOtherTool))
                {
                    iSessions++;
                }
            }

            return iSessions;
        }
    }
}
