using System;
using System.Windows.Forms;
using Fiddler;
using System.Xml;
using Office365FiddlerExtension.Services;
using Newtonsoft.Json;
using static Office365FiddlerExtension.Services.SessionFlagHandler;
using Office365FiddlerExtension.Handlers;

namespace Office365FiddlerExtension
{
    class AboutOld
    {
        private static AboutOld _instance;
        public static AboutOld Instance => _instance ?? (_instance = new AboutOld());

        public void CheckForUpdate()
        {
            var ExtensionSettings = SettingsHandler.Instance.GetDeserializedExtensionSettings();
            var URLs = URLsHandler.Instance.GetDeserializedExtensionURLs();

            if (ExtensionSettings.NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: CheckForUpdate: NeverWebCall is true; Extension won't check for any updates.");
                return;
            }

            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: CheckForUpdate begin.");

            // REVIEW THIS. STRIP OUT XML AND REPLACE WITH JSON.

            #region ReadVersionFromXML

            string downloadUrl;
            Version newVersion = null;
            string xmlUrl = URLs.UpdateJson; // Properties.Settings.Default.UpdateURL;

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
                /*
                #region UpdateAvailable

                FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: {DateTime.Now}: About.cs : Update Available.");

                FiddlerApplication.Prefs.SetStringPref("extensions.Office365FiddlerExtension.UpdateMessage", $"Update Available{Environment.NewLine}----------------" +
                    $"{Environment.NewLine}Currently using version: v{applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}" +
                    $"{Environment.NewLine}New version available: v{newVersion.Major}.{newVersion.Minor}.{newVersion.Build}{Environment.NewLine} {Environment.NewLine}" +
                    //$"Download the latest version: {Environment.NewLine}{Properties.Settings.Default.InstallerURL}{Environment.NewLine}{Environment.NewLine}");
                    $"Download the latest version: {Environment.NewLine}{URLs.Installer}{Environment.NewLine}{Environment.NewLine}");

                // Give user feedback on click 'About' menu item if no update is available.
                // REVIEW THIS. ManualCheckForUpdate is redundant. There's no reason to check for an update on a button click. Do it in the background in NeverWebCall is disabled.
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
                        //System.Diagnostics.Process.Start(Properties.Settings.Default.InstallerURL);
                        System.Diagnostics.Process.Start(URLs.Installer);
                        FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: Version installed. v{applicationVersion.Major}.{applicationVersion.Minor}.{applicationVersion.Build}");
                        FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: New Version Available. v{newVersion.Major}.{newVersion.Minor}.{newVersion.Build}");
                    }
                    // return this perference back to false, so we don't give this feedback unintentionally.
                    Preferences.ManualCheckForUpdate = false;
                }
                #endregion
                */
            }
            // No update available.
            else
            {
                #region NoUpdateAvailable
                /*
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
                */
                #endregion
            }
            FiddlerApplication.Log.LogString($"{Preferences.LogPrepend()}: CheckForUpdate done.");
        }
    }
}