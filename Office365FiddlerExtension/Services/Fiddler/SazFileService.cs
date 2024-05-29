using Fiddler;
using System;
using System.Linq;
using System.Reflection;
using Office365FiddlerExtension.UI;
using System.Diagnostics;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Function to handle loading and saving Saz files.
    /// </summary>
    public class SazFileService
    {
        internal Session session { get; set; }

        private static SazFileService _instance;

        public static SazFileService Instance => _instance ?? (_instance = new SazFileService());

        /// <summary>
        /// Handle saving a SAZ file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SaveSaz(object sender, FiddlerApplication.WriteSAZEventArgs e)
        {
            // Remove the session flags the extension adds to save space in the file and
            // mitigate errors thrown when loading a SAZ file which was saved with the extension enabled.
            // https://github.com/jprknight/Office365FiddlerExtension/issues/45
            // 6/1/2023 Leaving all legacy session flags in here so the above issue isn't somehow reintroduced if 
            // users open an old Saz file saved with a legacy version of the extension enabled. This code will fix up the
            // file if re-saved.

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            foreach (var session in e.arrSessions)
            {
                session.oFlags.Remove("UI-BACKCOLOR");
                session.oFlags.Remove("UI-COLOR");
                session.oFlags.Remove("X-SESSIONTYPE");
                session.oFlags.Remove("X-ATTRIBUTENAMEIMMUTABLEID");
                session.oFlags.Remove("X-ATTRIBUTENAMEUPN");
                session.oFlags.Remove("X-AUTHENTICATION");
                session.oFlags.Remove("X-AUTHENTICATIONDESC");
                session.oFlags.Remove("X-ELAPSEDTIME");
                session.oFlags.Remove("X-RESPONSESERVER");
                session.oFlags.Remove("X-ISSUER");
                session.oFlags.Remove("X-NAMEIDENTIFIERFORMAT");
                session.oFlags.Remove("X-OFFICE365AUTHTYPE");
                session.oFlags.Remove("X-PROCESSNAME");
                session.oFlags.Remove("X-RESPONSEALERT");
                session.oFlags.Remove("X-RESPONSECOMMENTS");
                session.oFlags.Remove("X-RESPONSECODEDESCRIPTION");
                session.oFlags.Remove("X-DATAAGE");
                session.oFlags.Remove("X-DATACOLLECTED");
                session.oFlags.Remove("X-SERVERTHINKTIME");
                session.oFlags.Remove("X-TRANSITTIME");
                session.oFlags.Remove("X-CALCULATEDSESSIONAGE");
                session.oFlags.Remove("X-PROCESSINFO");
                session.oFlags.Remove("X-SACL");
                session.oFlags.Remove("X-STCL");
                session.oFlags.Remove("X-SRSCL");
                // Commenting out this last line, so analysis can be retained in a saved SAZ file.
                //session.oFlags.Remove("MICROSOFT365FIDDLEREXTENSIONJSON");
            }

            FiddlerApplication.UI.lvSessions.EndUpdate();
        }

        /// <summary>
        /// Handle loading a SAZ file. If the sessions already have session analysis with all three 
        /// confidence levels set to 10, use stored analysis for faster load times.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
        {
            if (!SettingsJsonService.Instance.ExtensionSessionProcessingEnabled)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz {e.sFilename}. Extension not enabled, returning.");
                return;
            }

            if (!SettingsJsonService.Instance.SessionAnalysisOnLoadSaz) {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz {e.sFilename}. SessionAnalysisOnLoadSaz not enabled, returning.");
                return;
            }

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz with Extension Enabled: {SettingsJsonService.Instance.ExtensionSessionProcessingEnabled}, {Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)}.");
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz processing: {e.sFilename}");

            var sw = Stopwatch.StartNew();

            foreach (Session session in e.arrSessions)
            {
                this.session = session;

                // If the session already has the Microsoft365FiddlerExtensionJson flag set with high confidence session classifications set,
                // enhance the session based on prior / stored analysis.
                if (SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionAuthenticationConfidenceLevel == 10
                    && SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionResponseServerConfidenceLevel == 10
                    && SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionTypeConfidenceLevel == 10)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"Enhancing {this.session.id} based on existing session flags ({SessionFlagService.Instance.GetDeserializedSessionFlags(this.session).SessionType}).");

                    EnhanceSessionUX.Instance.EnhanceSession(this.session);
                }
                else
                {
                    SessionService.Instance.OnPeekAtResponseHeaders(this.session);
                }
            }

            sw.Stop();
            //TimeSpan time = sw.Elapsed;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        $"LoadSaz processed {e.arrSessions.Count()} sessions in {sw.ElapsedMilliseconds}ms from {e.sFilename}.");

            FiddlerApplication.UI.lvSessions.EndUpdate();
        }
    }
}
