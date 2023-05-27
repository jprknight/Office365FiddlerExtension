using System;
using System.Windows.Forms;
using Fiddler;
using System.Xml;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtension
{
    class About
    {
        private static About _instance;
        public static About Instance => _instance ?? (_instance = new About());

        public void CheckForUpdate()
        {
            if (Preferences.DisableWebCalls)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: CheckForUpdate: DisableWebCalls is true; Extension won't check for any updates.");
                return;
            }

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: CheckForUpdate begin.");

            #region ReadVersionFromXML

            string downloadUrl;
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

            #endregion

            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            // Update available.
            if (applicationVersion.CompareTo(newVersion) < 0)
            {
                #region UpdateAvailable

                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {DateTime.Now}: About.cs : Update Available.");

                FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.UpdateMessage", $"Update Available{Environment.NewLine}----------------" +
                    $"{Environment.NewLine}Currently using version: v{applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}" +
                    $"{Environment.NewLine}New version available: v{newVersion.Major}.{newVersion.Minor}.{newVersion.Build}{Environment.NewLine} {Environment.NewLine}" +
                    $"Download the latest version: {Environment.NewLine}{Properties.Settings.Default.InstallerURL}{Environment.NewLine}{Environment.NewLine}");

                // Give user feedback on click 'About' menu item if no update is available.
                if (Preferences.ManualCheckForUpdate)
                {

                    string message = $"You are currently using v{applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}{Environment.NewLine}"+
                        $"A new version is available v{newVersion.Major}.{newVersion.Minor}.{newVersion.Build}{Environment.NewLine}" +
                        "Do you want to download the update?";

                    string caption = "Office 365 Fiddler Extension - Update Available";

                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    //Display the MessageBox.
                    result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                    if (result == DialogResult.Yes)
                    {
                        // Execute the installer MSI URL, which will open in the user's default browser.
                        System.Diagnostics.Process.Start(Properties.Settings.Default.InstallerURL);
                        FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Version installed. v{applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}");
                        FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: New Version Available. v{newVersion.Major}.{newVersion.Minor}.{newVersion.Build}");
                    }
                    // return this perference back to false, so we don't give this feedback unintentionally.
                    Preferences.ManualCheckForUpdate = false;
                }
                #endregion
            }
            // No update available.
            else
            {
                #region NoUpdateAvailable

                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: CheckForUpdate: No update available.");

                if (Preferences.ManualCheckForUpdate)
                {
                    MessageBox.Show("You already have the latest version installed." 
                        + Environment.NewLine 
                        + Environment.NewLine 
                        + $"Currently using: v{applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}{Environment.NewLine}"
                        + $"Newest available: v{newVersion.Major}.{newVersion.Minor}.{newVersion.Build}", "Office 365 Fiddler Extension");

                    FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.UpdateMessage", $"You already have the latest version installed."
                        + $"{Environment.NewLine}-----------"
                        + $"{Environment.NewLine}Currently using: v"
                        + $"{applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}"
                        + $"{Environment.NewLine}Newest available: v"
                        + $"{newVersion.Major}.{newVersion.Minor}.{newVersion.Build}{Environment.NewLine}{Environment.NewLine}");
                    // return this perference back to false, so we don't give this feedback unintentionally.
                    Preferences.ManualCheckForUpdate = false;
                }
                #endregion
            }
            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: CheckForUpdate done.");
        }
    }
}