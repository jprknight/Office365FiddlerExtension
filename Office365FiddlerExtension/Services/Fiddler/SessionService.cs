using Fiddler;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtension.UI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Office365FiddlerExtension
{
    /// <summary>
    /// Function that calls ruleset to run on loaded sessions.
    /// The call to ActivationService here runs the application.
    /// </summary>
    public class SessionService : ActivationService
    {
        private static SessionService _instance;

        public static SessionService Instance => _instance ?? (_instance = new SessionService());

        /// <summary>
        /// Decode request & response, Run ruleset, Enhance sessions in UI.
        /// </summary>
        /// <param name="Session"></param>
        public void OnPeekAtResponseHeaders(Session Session)
        {
            this.session = Session;

            try
            {
                this.session.utilDecodeRequest(true);
            }
            catch (Exception ex) 
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error decoding session request.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

            try
            {
                this.session.utilDecodeResponse(true);
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error decoding session response.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }

            RulesetService.Instance.CallRunRuleSet(this.session);

            EnhanceSessionUX.Instance.EnhanceSession(this.session);
        }

        public bool ConfirmLargeSessionAnalysis(int sessionsCount)
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            // The number of sessions added into the Fiddler UI is smaller than the 'warn before analysing' threshold.
            // Return true so the session analysis happens without a prompt to confirm.
            if (sessionsCount <= extensionSettings.WarnBeforeAnalysing)
            {
                return true;
            }

            // REVIEW THIS - LangHelper this message box.

            // The number of sessions added into the Fiddler UI is larger than the 'warn before analysing' threshold.
            // Prompt the user on whether they want to perform session analysis, giving the user a choice to accept some delay.
            string message = $"The extension is about to analyse " +
                $"{sessionsCount} " +
                $"sessions, " +
                $"which is more than the threshold set within the extension of " +
                $"{extensionSettings.WarnBeforeAnalysing}." +
                Environment.NewLine +
                Environment.NewLine +
                $"If you proceed Fiddler may take some time to process all these sessions." +
                Environment.NewLine +
                Environment.NewLine +
                $"Do you want to continue or cancel the operation?";

            string caption = $"{LangHelper.GetString("Office 365 Fiddler Extension")}";

            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;

            DialogResult dialogResult = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (dialogResult == DialogResult.OK)
            {
                SettingsJsonService.Instance.SetLargeSessionAnalysisApproval(true);
                // User wants to continue with session analysis.
                return true;
                
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                SettingsJsonService.Instance.SetLargeSessionAnalysisApproval(false);
                // User doesn't want to continue with session analysis.
                return false;
            }
            
            return true;
        }

        public bool IsSessionImported(Session session)
        {
            this.session = session;

            if (this.session.isAnyFlagSet(SessionFlags.ImportedFromOtherTool))
            {
                return true;
            }
            return false;
        }

        // REVIEW THIS - Work around, implement fix so HTTP 200 Json, invalid & empty can properly detect.
        // Currently, valid and invalid work.

        /// <summary>
        /// Function to safely get the response body string from a session.
        /// 3/25/2025 I was running into issues with a session response body with content-encoding: gzip.
        /// Trying to work with this.session.GetResponseBodyAsString() was causing issues.
        /// This exception was being thrown which a catch statement couldn't handle.
        /// System.IO.InvalidDataException The content could not be ungzipped The magic number in GZip header is not correct. Make sure you are passing in a GZip stream.
        /// There's some clever things here: https://stackoverflow.com/questions/523930/sockets-in-c-how-to-get-the-response-stream
        /// For now creating a function to work around the issue.
        /// https://github.com/jprknight/Office365FiddlerExtension/issues/107
        /// </summary>
        /// <param name="session"></param>
        /// <returns>string on response body, bool on whether session response is gzip'ed or not.</returns>
        public Tuple<string, bool> GetSafeSessionResponseBodyString(Session session)
        {
            this.session = session;

            try
            {
                if (!this.session.oResponse.headers.ExistsAndContains("Content-Encoding", "gzip"))
                {
                    return Tuple.Create(this.session.GetResponseBodyAsString(), false);
                }
            }
            catch (Exception ex)
            {
                TelemetryService.CustomTrackException(ex);
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"{this.session.id} {ex}");
            }

            return Tuple.Create("",true);
        }
    }
}
