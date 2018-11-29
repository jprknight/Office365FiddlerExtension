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

namespace EXOFiddlerInspector
{
    public partial class Office365AuthUserControl : UserControl
    {
        public string SessionData;

        public Office365AuthUserControl()
        {
            InitializeComponent();
        }

        internal void SetAuthenticationResponseComments(string txt)
        {
            AuthenticationResponseComments.Text = txt;
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
    }
}
