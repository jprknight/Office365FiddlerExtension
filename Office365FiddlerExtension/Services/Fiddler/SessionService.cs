using Fiddler;
using Office365FiddlerExtension.Services;
using Office365FiddlerExtension.UI;
using System;
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

            this.session.utilDecodeRequest(true);
            this.session.utilDecodeResponse(true);

            RulesetService.Instance.CallRunRuleSet(this.session);

            EnhanceSessionUX.Instance.EnhanceSession(this.session);
        }

        public bool ConfirmLargeSessionAnalysis(int sessionsCount)
        {
            var extensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            // The number of sessions added into the Fiddler UI is larger than the 'warn before analysing' threshold.
            // Prompt the user on whether they want to perform session analysis, giving the user a choice to accept some delay.
            if (sessionsCount >= extensionSettings.WarnBeforeAnalysing)
            {
                string message = $"The extension is about to analyse {sessionsCount} sessions, " +
                    $"which is more than the threshold set within the extension of {extensionSettings.WarnBeforeAnalysing}." +
                    Environment.NewLine +
                    Environment.NewLine +
                    $"If you proceed you may see Fiddler appear to freeze while all sessions are processed." +
                    Environment.NewLine +
                    Environment.NewLine +
                    $"Do you want to continue or cancel the operation?";

                string caption = $"{LangHelper.GetString("Office 365 Fiddler Extension")}";

                MessageBoxButtons buttons = MessageBoxButtons.OKCancel;

                DialogResult dialogResult = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.OK)
                {
                    // User wants to continue with session analysis.
                    return true;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    // User doesn't want to continue with session analysis.
                    return false;
                }
            }

            // The number of sessions added into the Fiddler UI is smaller than the 'warn before analysing' threshold.
            // Just analyse the sessions.
            return true;
        }
    }
}
