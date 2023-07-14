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
                        // REVIEW THIS - Pull URL from Json.
                        string HTMLOutput = webBrowserControl.DocumentText.Replace("<br />", "");
                        HTMLOutput += "<p>Data created from the <a href='https://aka.ms/Office365FiddlerExtensionUpdateURL' target='_blank'>Office 365 Fiddler Extension.</a></p>";
                        sw.Write(HTMLOutput);
                    }
                }
            }
        }
    }
}
