using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;
using Office365FiddlerExtension.Services;
using Office365FiddlerInspector.UI;

namespace Office365FiddlerExtension
{ 
    public class Office365FiddlerExtensionTabPage : IFiddlerExtension
    {
        TabPage oPage;

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

            // Clear ResultsString.
            Clear();
                
            if (Preferences.DisableWebCalls)
            {
                TabPageResultsString.AppendLine(StrTitle());

                TabPageResultsString.AppendLine(StrDisableWebCalls());

                TabPageResultsString.AppendLine(StrExtensionAdds());

                TabPageResultsString.AppendLine("<h3>Columns</h3><p>The extension adds five columns. Elapsed Time, Session Type, Authentication Host IP and Response Server.</p>");

                TabPageResultsString.AppendLine(StrAccessInspector());

                TabPageResultsString.AppendLine(StrAccessInspector2());

                TabPageResultsString.AppendLine(StrProjectLinks());

                Office365TabPage.TabPageResultsOutput.DocumentText = TabPageResultsString.ToString();
            }
            else
            {
                TabPageResultsString.AppendLine(StrTitle());

                TabPageResultsString.AppendLine(StrExtensionAdds());

                TabPageResultsString.AppendLine("<h3>Columns</h3><p>The extension adds five columns.</p>");

                TabPageResultsString.AppendLine("<img src='https://github.com/jprknight/Office365FiddlerExtension/blob/master/docs/Office365FiddlerExtensionColumns.png?raw=true'>");

                TabPageResultsString.AppendLine(StrAccessInspector());

                TabPageResultsString.AppendLine("<img src='https://github.com/jprknight/Office365FiddlerExtension/blob/master/docs/Inspectors.png?raw=true'>");

                TabPageResultsString.AppendLine(StrAccessInspector2());

                TabPageResultsString.AppendLine("<img src='https://github.com/jprknight/Office365FiddlerExtension/blob/master/docs/Office365Inspector.png?raw=true'>");

                TabPageResultsString.AppendLine(StrProjectLinks());

                Office365TabPage.TabPageResultsOutput.DocumentText = TabPageResultsString.ToString();
            }
            
        }

        public string StrTitle()
        {
            return "<h2>Office 365 Fiddler Extension</h2>";
        }

        public string StrDisableWebCalls ()
        {
            return "<p><b>You have web calls disabled</b>. That means the extension won't let you know when updates are available.</p>";
        }

        public string StrExtensionDisabled()
        {
            return "<p><b>The extension is currently disabled</b>.</p> In the menu Click 'Office 365 (Disabled)', and check the enable option.</p>";
        }

        public string StrExtensionAdds()
        {
            return "<p>The extension adds a menu, additional columns, this tab, and a response inspector.</p>";
        }

        public string StrAccessInspector()
        {
            return "<h3>Inspector</h3><p>To access the Inspector in the extension click the Inspectors tab.</p>";
        }

        public string StrAccessInspector2()
        {
            return "<p>Once you click on a session in the left hand panel, you will see the Inspectors available.</p>"
                + "<p>The top right panel is the request data sent out from client applications and recorded by Fiddler.</p>"
                + "<p>The bottom right panel is the server data received back in response and recorded by Fiddler.</p>"
                + "<p>Click the Office365 response Inspector for detailed analysis of each session response. Detailed information "
                + "is available on how Office 365 client applications are expected to behave and how to troubleshoot issues.</p>";
        }

        public string StrProjectLinks()
        {
            return "<h3>Links</h3><p>Releases download page: <a href='https://aka.ms/Office365FiddlerExtensionUpdateUrl' target='_blank'>https://aka.ms/Office365FiddlerExtensionUpdateUrl</a><br />"
                + "Extension wiki page: <a href='https://aka.ms/Office365FiddlerExtensionWiki' target='_blank'>https://aka.ms/Office365FiddlerExtensionWiki</a><br />"
                + "Report Issues page: <a href='https://aka.ms/Office365FiddlerExtensionIssues' target='_blank'>https://aka.ms/Office365FiddlerExtensionIssues</a></p>";
        }

        public void Clear()
        {
            Office365TabPage.TabPageResultsOutput.DocumentText = "";

            TabPageResultsString = new StringBuilder();
        }
    }
}
