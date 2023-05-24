using System.IO;
using System.Windows.Forms;
using Fiddler;

namespace Office365FiddlerExtension.UI
{
    public partial class Office365ResponseControl : UserControl
    {

        public static WebBrowser ResultsOutput { get; set; }
        public Office365ResponseControl()
        {
            InitializeComponent();
           
            ResultsOutput = webBrowserControl;
        }
        private void ResetPrefs()
        {
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.enabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ColumnsEnableAll");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.DemoMode");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.DemoModeBreakScenarios");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ElapsedTimeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ResponseServerColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ExchangeTypeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.AppLoggingEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.HighlightOutlookOWAOnlyEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ExecutionCount");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.ManualCheckForUpdate");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.MenuTitle");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.HostIPColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.EXOFiddlerExtension.AuthColumnEnabled");

            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.enabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ColumnsEnableAll");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.DemoMode");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.DemoModeBreakScenarios");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ElapsedTimeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ResponseServerColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ExchangeTypeColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.AppLoggingEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.HighlightOutlookOWAOnlyEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ExecutionCount");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.ManualCheckForUpdate");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.MenuTitle");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.HostIPColumnEnabled");
            FiddlerApplication.Prefs.RemovePref("extensions.O365FiddlerExtension.AuthColumnEnabled");
        }

        private void webBrowserControl_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void Save_Click(object sender, System.EventArgs e)
        {
            if (webBrowserControl.DocumentText.Length == 0)
            {
                MessageBox.Show("Nothing to save.");
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = " Webpage, HTML only |*.html;*.htm";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        // Remove <br /> from output, not needed in HTML, introduced spacing for save button.
                        string HTMLOutput = webBrowserControl.DocumentText.Replace("<br />", "");
                        HTMLOutput += "<p>Data created from the <a href='https://aka.ms/Office365FiddlerExtensionUpdateURL' target='_blank'>Office 365 Fiddler Extension.</a></p>";
                        sw.Write(HTMLOutput);
                    }
                }
            }
        }
    }
}
