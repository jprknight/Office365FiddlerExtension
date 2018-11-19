using System;
using System.Windows.Forms;
using Fiddler;
using System.Xml;
using System.Diagnostics;

namespace EXOFiddlerInspector
{
    class CheckForAppUpdate
    {
        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.enabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.AppLoggingEnabled", false);

        /////////////////
        //
        // Check for updates.
        //
        // Called from Onload in ColouriseWebSessions.cs. 
        // Needs further investigation: Only implemented on loadSAZ), due to web call issue, as Fiddler substitutes in http://localhost:8888 as the proxy server.
        //

        public void CheckForUpdate()
        {
            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: CheckForAppUpdate.cs : CheckForUpdate()");

            string downloadUrl = "";
            Version newVersion = null;
            string xmlUrl = Properties.Settings.Default.UpdateURL;

            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(xmlUrl);
                reader.MoveToContent();
                string elementName = "";
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name == "EXOFiddlerInspector")
                {
                while (reader.Read())
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        elementName = reader.Name;
                    }
                    else
                    {
                        if ((reader.NodeType == XmlNodeType.Text) && reader.HasValue)
                        {
                            switch (elementName)
                            {
                                case "version":
                                    newVersion = new Version(reader.Value);
                                    break;
                                case "url":
                                    downloadUrl = reader.Value;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            /// <remarks>
            /// Update available.
            /// </remarks>
            if (applicationVersion.CompareTo(newVersion) < 0)
            {
                /// <remarks>
                /// Stop showing the message box. Allow user to enter trace without interuption.
                /// </remarks>
                // Setup message box options.
                /*
                string message = "You are currently using v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + "." + Environment.NewLine +
                    "A new version is available v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + Environment.NewLine +
                    "Do you want to download the update?";

                string caption = "EXO Fiddler Extension - Update Available";
                */
                /// <remarks>
                /// Set menu title to show user there is an update available.
                /// </remarks>
                Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: CheckForAppUpdate.cs : Update Available.");
                FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerInspector.MenuTitle", "Exchange Online (Update Available!)");
                /*
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                 Display the MessageBox.
                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    // Execute the installer MSI URL, which will open in the user's default browser.
                    System.Diagnostics.Process.Start(Properties.Settings.Default.InstallerURL);
                    if (bAppLoggingEnabled && bExtensionEnabled)
                    {
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: Version installed. v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + ".");
                        FiddlerApplication.Log.LogString("EXOFiddlerExtention: New Version Available. v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + ".");
                    }
                }
                */
                /// <remarks>
                /// Refresh the value of ManualCheckForUpdate and respond with feedback if needed.
                /// </remarks>

                Boolean ManualCheckForUpdateFeedback = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ManualCheckForUpdate", false);

                // Regardless of extension enabled or not, give the user feedback when they click the 'Check For Update' menu item if no update is available.
                if (ManualCheckForUpdateFeedback)
                {
                    MessageBox.Show("EXOFiddlerExtention: Update available. v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ".", "EXO Fiddler Extension");
                    // return this perference back to false, so we don't give this feedback unintentionally.
                    FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ManualCheckForUpdate", false);
                }
            }
            else
            {
                /// <remarks>
                /// No update available.
                /// </remarks>
                /// 
                Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: CheckForAppUpdate.cs : No update available.");
                FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerInspector.MenuTitle", "Exchange Online");

                if (bAppLoggingEnabled && bExtensionEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: Latest version installed. v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ".");
                }

                /// <remarks>
                /// Refresh the value of ManualCheckForUpdate and respond with feedback if needed.
                /// </remarks>
                
                Boolean ManualCheckForUpdateFeedback = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerInspector.ManualCheckForUpdate", false);

                // Regardless of extension enabled or not, give the user feedback when they click the 'Check For Update' menu item if no update is available.
                if (ManualCheckForUpdateFeedback)
                {
                    MessageBox.Show("EXOFiddlerExtention: You already have the latest version installed. v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ".", "EXO Fiddler Extension");
                    // return this perference back to false, so we don't give this feedback unintentionally.
                    FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerInspector.ManualCheckForUpdate", false);
                }
            }
        }
    }
}
