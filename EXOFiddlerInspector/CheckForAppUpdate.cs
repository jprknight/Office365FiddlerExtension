using System;
using System.Windows.Forms;
using Fiddler;
using System.Xml;
using System.Diagnostics;
using EXOFiddlerInspector.Services;

namespace EXOFiddlerInspector
{
    class CheckForAppUpdate
    {
        public Boolean bExtensionEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.enabled", false);
        public Boolean bAppLoggingEnabled = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.AppLoggingEnabled", false);

        /////////////////
        //
        // Check for updates.
        //
        // Called from Onload in ColouriseWebSessions.cs. 
        // Needs further investigation: Only implemented on loadSAZ), due to web call issue, as Fiddler substitutes in http://localhost:8888 as the proxy server.
        //

        public void CheckForUpdate()
        {
            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: CheckForAppUpdate.cs : CheckForUpdate begin.");

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
            /// 

            if (applicationVersion.CompareTo(newVersion) < 0)
            {

                Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: CheckForAppUpdate.cs : Update Available.");
                //FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerExtension.MenuTitle", "Exchange Online (Update Available)");

                /// <remarks>
                /// Refresh the value of ManualCheckForUpdate and respond with feedback if needed.
                /// </remarks>

                Boolean ManualCheckForUpdateFeedback = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ManualCheckForUpdate", false);

                if (applicationVersion.Build >= 1000)
                {
                    FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerExtension.UpdateMessage", $"Update Available{Environment.NewLine}----------------" +
                        $"{Environment.NewLine}You should update from this beta build to the latest production build." +
                        $"{Environment.NewLine}Currently using beta version: {applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}" +
                        $"{Environment.NewLine}New production version available: {newVersion.Major}.{newVersion.Minor}.{newVersion.Build} {Environment.NewLine} {Environment.NewLine}" +
                        $"Download the latest version: {Environment.NewLine}https://aka.ms/EXOFiddlerExtension {Environment.NewLine} {Environment.NewLine}");
                }
                else
                {
                    FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerExtension.UpdateMessage", $"Update Available{Environment.NewLine}----------------" +
                        $"{Environment.NewLine}Currently using version: {applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}" +
                        $"{Environment.NewLine}New version available: {newVersion.Major}.{newVersion.Minor}.{newVersion.Build} {Environment.NewLine} {Environment.NewLine}" +
                        $"Download the latest version: {Environment.NewLine}https://aka.ms/EXOFiddlerExtension {Environment.NewLine} {Environment.NewLine}");
                }

                // Regardless of extension enabled or not, give the user feedback when they click the 'Check For Update' menu item if no update is available.
                if (ManualCheckForUpdateFeedback)
                {
                    //MessageBox.Show("EXOFiddlerExtention: Update available. v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ".", "EXO Fiddler Extension");

                    string message = "You are currently using v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + "." + Environment.NewLine +
                    "A new version is available v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + Environment.NewLine +
                    "Do you want to download the update?";

                    string caption = "EXO Fiddler Extension - Update Available";

                    /// <remarks>
                    /// Set menu title to show user there is an update available.
                    /// </remarks>

                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    //Display the MessageBox.
                    result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                    if (result == DialogResult.Yes)
                    {
                        // Execute the installer MSI URL, which will open in the user's default browser.
                        System.Diagnostics.Process.Start(Properties.Settings.Default.InstallerURL);
                        if (bAppLoggingEnabled)
                        {
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: Version installed. v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + ".");
                            FiddlerApplication.Log.LogString("EXOFiddlerExtention: New Version Available. v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + ".");
                        }
                    }
                    // return this perference back to false, so we don't give this feedback unintentionally.
                    FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ManualCheckForUpdate", false);
                }
            }
            else
            {
                /// <remarks>
                /// No update available.
                /// </remarks>
                /// 
                Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: CheckForAppUpdate.cs : No update available.");
                //FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerExtension.MenuTitle", "Exchange Online");

                FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerExtension.UpdateMessage", "");

                if (bAppLoggingEnabled)
                {
                    FiddlerApplication.Log.LogString("EXOFiddlerExtention: Latest version installed.");
                }

                /// <remarks>
                /// Refresh the value of ManualCheckForUpdate and respond with feedback if needed.
                /// </remarks>

                Boolean ManualCheckForUpdateFeedback = FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.ManualCheckForUpdate", false);

                // Tell user if they are either on a beta build.
                if (applicationVersion.Build >= 1000 && ManualCheckForUpdateFeedback)
                {
                    MessageBox.Show("EXOFiddlerExtention: You are using a beta build. Thanks for the testing!" + Environment.NewLine +
                        "You are currently using v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + "." + Environment.NewLine +
                        "Newest production build available v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ".", "EXO Fiddler Extension - Beta Version!");

                    FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerExtension.UpdateMessage", $"Beta Build!{Environment.NewLine}-----------" +
                        $"{Environment.NewLine}Currently using version: {applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}" +
                        $"{Environment.NewLine}Newest production build available: {newVersion.Major}.{newVersion.Minor}.{newVersion.Build} {Environment.NewLine} {Environment.NewLine}" +
                        $"Raise any issues at: {Environment.NewLine}http://aka.ms/EXOFiddlerExtensionIssues {Environment.NewLine} {Environment.NewLine}");
                }
                // Update the UpdateMessage if user is on beta build.
                else if (applicationVersion.Build >= 1000)
                {
                    FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerExtension.UpdateMessage", $"Beta Build!{Environment.NewLine}-----------" +
                        $"{Environment.NewLine}Currently using version: {applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}" +
                        $"{Environment.NewLine}Newest production build available: {newVersion.Major}.{newVersion.Minor}.{newVersion.Build} {Environment.NewLine} {Environment.NewLine}" +
                        $"Raise any issues at: {Environment.NewLine}http://aka.ms/EXOFiddlerExtensionIssues {Environment.NewLine} {Environment.NewLine}");
                }
                // Tell user if they are on latest production build.
                else if (ManualCheckForUpdateFeedback)
                {
                    MessageBox.Show("EXOFiddlerExtention: You already have the latest version installed." + Environment.NewLine +
                        "You are currently using v" + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build + "." + Environment.NewLine +
                        "Newest available v" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + ".", "EXO Fiddler Extension");
                    // return this perference back to false, so we don't give this feedback unintentionally.
                    FiddlerApplication.Prefs.SetBoolPref("extensions.EXOFiddlerExtension.ManualCheckForUpdate", false);
                }
            }

            /// <remarks>
            /// If DeveloperDemoMode set the menu title regardless of the availability of an update.
            /// </remarks>
            //if (Preferences.GetDeveloperMode())
            //{
            //    FiddlerApplication.Prefs.SetStringPref("extensions.EXOFiddlerExtension.MenuTitle", "Exchange Online (Update Available!)");
            //}
            Debug.WriteLine($"EXCHANGE ONLINE EXTENSION: {DateTime.Now}: CheckForAppUpdate.cs : CheckForUpdate done.");
        }
    }
}
