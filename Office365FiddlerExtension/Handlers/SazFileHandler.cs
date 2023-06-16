using Fiddler;
using Office365FiddlerExtension.Ruleset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtension.Handler
{
    /// <summary>
    /// Function to handle loading and saving Saz files.
    /// </summary>
    public class SazFileHandler
    {
        internal Session Session { get; set; }

        private static SazFileHandler _instance;

        public static SazFileHandler Instance => _instance ?? (_instance = new SazFileHandler());

        /// <summary>
        /// Function to handle saving a SAZ file.
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
                session.oFlags.Remove("MICROSOFT365FIDDLEREXTENSIONJSON");
            }

            FiddlerApplication.UI.lvSessions.EndUpdate();
        }

        /// <summary>
        /// Function to handle loading a SAZ file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadSaz(object sender, FiddlerApplication.ReadSAZEventArgs e)
        {
            if (!SettingsHandler.Instance.ExtensionSessionProcessingEnabled)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz {e.sFilename}. Extension not enabled, returning.");
                return;
            }

            if (!SettingsHandler.Instance.SessionAnalysisOnLoadSaz) {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz {e.sFilename}. SessionAnalysisOnLoadSaz not enabled, returning.");
                return;
            }

            FiddlerApplication.UI.lvSessions.BeginUpdate();

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz with Extension Enabled: {SettingsHandler.Instance.ExtensionSessionProcessingEnabled}, {Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)}.");
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz processing: {e.sFilename}");

            // Testing to make sure LoadSaz function is called only once when the Fiddler application is opened by loading a SAZ file.
            //MessageBox.Show($"LoadSaz event fired on {e.sFilename} from {Assembly.GetExecutingAssembly().GetName().CodeBase}");

            foreach (var session in e.arrSessions)
            {
                this.Session = session;
                SessionHandler.Instance.OnPeekAtResponseHeaders(this.Session);
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): LoadSaz processed: {e.sFilename}");

            FiddlerApplication.UI.lvSessions.EndUpdate();
        }
    }
}
