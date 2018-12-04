using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Fiddler;

namespace EXOFiddlerInspector
{
    public partial class Office365AuthUserControl : UserControl
    {
        public string SessionData;

        public bool bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);

        public Office365AuthUserControl()
        {
            // If the extension is not enabled, don't build the user controls.
            if (bExtensionEnabled)
            {
                InitializeComponent();
            }
            else
            {
                return;
            }
        }

        internal void SetAuthenticationResponseComments(string txt)
        {
            AuthenticationResponseCommentsTextbox.Text = txt;
        }

        internal void SetIssuerTextBox(string txt)
        {
            IssuerTextBox.Text = txt;
        }

        internal void SetAttributeNameUPNTextBox(string txt)
        {
            AttributeNameUPNTextBox.Text = txt;
        }

        internal void SetNameIdentifierFormatTextBox(string txt)
        {
            NameIdentifierFormatTextBox.Text = txt;
        }

        internal void SetAttributeNameImmutableIDTextBox(string txt)
        {
            AttributeNameImmutableIDTextBox.Text = txt;
        }

        internal void SetOffice365AuthenticationGroupboxVisible(bool txt)
        {
            Office365AuthenticationGroupbox.Visible = txt;
        }

        internal void SetSAMLResponseParserGroupboxVisible(bool txt)
        {
            SAMLResponseParserGroupbox.Visible = txt;
        }

        internal void SetSigningCertificateTextbox(string txt)
        {
            SigningCertificateTextbox.Text = txt;
        }

        private void OpenSAMLDataButton_Click(object sender, EventArgs e)
        {
            // As the user has elected to open the file instead of save somewhere specific, write data out to a text file in %TEMP% environment variable and open it up in Notepad.
            WriteSessionData();
            System.IO.File.WriteAllText(Environment.GetEnvironmentVariable("temp") + "\\FiddlerTrace-SAML-Response-Data.txt", SessionData);
            System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("temp") + "\\FiddlerTrace-SAML-Response-Data.txt");
        }

        private void SaveSAMLDataButton_Click(object sender, EventArgs e)
        {
            // Initialise new SaveFileDialog.
            SaveFileDialog save = new SaveFileDialog();

            // Use the user setting PreviousPath to determine if we open %USERPROFILE%\Documents or some other previous path.
            if (string.IsNullOrEmpty(Properties.Settings.Default.PreviousPath))
            {
                save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                save.InitialDirectory = Properties.Settings.Default.PreviousPath;
            }

            // Setup dialog.
            save.FileName = "FiddlerTrace-SAMLResponse.txt";
            save.RestoreDirectory = true;
            save.Filter = "Text File|*.txt";

            WriteSessionData();

            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.Write(SessionData);
                writer.Dispose();
                writer.Close();
            }
        }

        private void WriteSessionData()
        {
            SessionData = "SAML Response Data:" +
                Environment.NewLine +
                Environment.NewLine +
                IssuerTextBox.Text +
                Environment.NewLine +
                Environment.NewLine +
                AttributeNameUPNTextBox.Text +
                Environment.NewLine +
                Environment.NewLine +
                NameIdentifierFormatTextBox.Text +
                Environment.NewLine +
                Environment.NewLine +
                AttributeNameImmutableIDTextBox.Text;
        }

        private void OpenSigningCertificateButton_Click(object sender, EventArgs e)
        {
            // As the user has elected to open the file instead of save somewhere specific, write data out to a text file in %TEMP% environment variable and open it up in Notepad.
            System.IO.File.WriteAllText(Environment.GetEnvironmentVariable("temp") + "\\FiddlerTrace-SAML-Signing-Certificate.cer", SigningCertificateTextbox.Text);
            System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("temp") + "\\FiddlerTrace-SAML-Signing-Certificate.cer");
        }

        private void SaveSigningCertificateButton_Click(object sender, EventArgs e)
        {
            // Initialise new SaveFileDialog.
            SaveFileDialog save = new SaveFileDialog();

            // Use the user setting PreviousPath to determine if we open %USERPROFILE%\Documents or some other previous path.
            if (string.IsNullOrEmpty(Properties.Settings.Default.PreviousPath))
            {
                save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                save.InitialDirectory = Properties.Settings.Default.PreviousPath;
            }

            // Setup dialog.
            save.FileName = "Fiddler-SAML-Signing-Certificate.cer";
            save.RestoreDirectory = true;
            save.Filter = "SSL Certificate|*.txt,*.cer";

            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.Write(SigningCertificateTextbox.Text);
                writer.Dispose();
                writer.Close();
            }
        }

        private void SAMLResponseParserGroupbox_VisibleChanged(object sender, EventArgs e)
        {
            if (SAMLResponseParserGroupbox.Visible == true)
            {
                SAMLResponseParserGroupbox.Location = new Point(3, 3);
            }
            else
            {
                SAMLResponseParserGroupbox.Location = new Point(3, 300);
            }
        }

        private void Office365AuthenticationGroupbox_VisibleChanged(object sender, EventArgs e)
        {
            if (Office365AuthenticationGroupbox.Visible == true)
            {
                Office365AuthenticationGroupbox.Location = new Point(3, 3);
            }
            else
            {
                Office365AuthenticationGroupbox.Location = new Point(3, 300);
            }
        }
    }
}
