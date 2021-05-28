using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;
using Office365FiddlerInspector.Services;
using Office365FiddlerInspector.UI;

namespace Office365FiddlerInspector
{ 
    public class Office365FiddlerExtensionTabPage : IFiddlerExtension
    {

        //private static Office365FiddlerExtensionTabPage _instance;

        //public static Office365FiddlerExtensionTabPage Instance => _instance ?? (_instance = new Office365FiddlerExtensionTabPage());

        //public Office365FiddlerExtensionTabPage() { }

        TabPage oPage = null;

        public StringBuilder TabPageResultsString { get; set; }

        //TabPageResultsString = new StringBuilder();

        public Office365FiddlerExtensionTabPage()
        {
            // Add tab page to Fiddler.
            Office365TabPage oView = new Office365TabPage();

            oPage = new TabPage("Office 365 Fiddler Extension");
            oPage.ImageIndex = (int)Fiddler.SessionIcons.Post;
            
            oView.Dock = DockStyle.Fill;

            oPage.Controls.Add(oView);
        }

        public void OnBeforeUnload()
        {
            // Do nothing here.  
        }

        public void OnLoad()
        {
            // Load the UI.
            FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);

            WriteToTabPage();

        }

        public void WriteToTabPage()
        {
            // Clear ResultsString.
            Clear();

            if (!Preferences.ExtensionEnabled)
            {
                if(Preferences.DisableWebCalls)
                {
                    TabPageResultsString.AppendLine("<h2>Office 365 Fiddler Extension</h2>");

                    TabPageResultsString.AppendLine("<p><b>The extension is currently disabled</b>.</p>"
                        + "In the menu Click 'Office 365 (Disabled)', and check the enable option.</p>");
                    
                    Office365TabPage.TabPageResultsOutput.DocumentText = TabPageResultsString.ToString();
                }
                else
                {
                    TabPageResultsString.AppendLine("<h2>Office 365 Fiddler Extension</h2>");

                    TabPageResultsString.AppendLine("<p><b>The extension is currently disabled</b>.</p>"
                        + "In the menu Click 'Office 365 (Disabled)', and check the enable option.</p>");

                    TabPageResultsString.AppendLine("<img src='https://github.com/jprknight/Office365FiddlerExtension/blob/master/docs/Office365FiddlerExtensionDisabled.png?raw=true'>");

                    Office365TabPage.TabPageResultsOutput.DocumentText = TabPageResultsString.ToString();
                }
            }
            else
            {
                if (Preferences.DisableWebCalls)
                {
                    TabPageResultsString.AppendLine("<h2>Office 365 Fiddler Extension</h2>");

                    TabPageResultsString.AppendLine("<p>To access the Inspector in the extension click the Inspectors tab.</p>");

                    TabPageResultsString.AppendLine("<p>Once you click on a session in the left hand panel, you will see the Inspectors available.</p>");

                    TabPageResultsString.AppendLine("<p>The top right panel is the request data sent out from the application(s) recorded by Fiddler</p>");

                    TabPageResultsString.AppendLine("<p>The bottom right panel is the server response data received back for the application(s) recorded by Fiddler</p>");

                    TabPageResultsString.AppendLine("<p>Click the Office365 response Inspector for detailed analysis of each session response in relation to how Office 365 "
                        + "are expected to behave and what action to take if something is not working as expected.</p>");

                    Office365TabPage.TabPageResultsOutput.DocumentText = TabPageResultsString.ToString();
                }
                else
                {
                    TabPageResultsString.AppendLine("<h2>Office 365 Fiddler Extension</h2>");

                    TabPageResultsString.AppendLine("<p>To access the Inspector in the extension click the Inspectors tab, shown below.</p>");

                    TabPageResultsString.AppendLine("<img src='https://github.com/jprknight/Office365FiddlerExtension/blob/master/docs/Inspectors.png?raw=true'>");

                    TabPageResultsString.AppendLine("<p>Once you click on a session in the left hand panel, you will see the Inspectors available.</p>");

                    TabPageResultsString.AppendLine("<p>The top right panel is the request data sent out from the application(s) recorded by Fiddler</p>");

                    TabPageResultsString.AppendLine("<p>The bottom right panel is the server response data received back for the application(s) recorded by Fiddler</p>");

                    TabPageResultsString.AppendLine("<p>Click the Office365 response Inspector for detailed analysis of each session response in relation to how Office 365 "
                        + "are expected to behave and what action to take if something is not working as expected.</p>");

                    TabPageResultsString.AppendLine("<img src='https://github.com/jprknight/Office365FiddlerExtension/blob/master/docs/Office365Inspector.png?raw=true'>");

                    Office365TabPage.TabPageResultsOutput.DocumentText = TabPageResultsString.ToString();
                }
            }
        }


        public void Clear()
        {
            
            Office365TabPage.TabPageResultsOutput.DocumentText = "";

            TabPageResultsString = new StringBuilder();
        }
    }
}
