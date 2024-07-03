using Fiddler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtension.UI.Forms
{
    public partial class Office365TabPage : UserControl
    {
        public Office365TabPage()
        {
            InitializeComponent();
        }
    }

    public class Office365FiddlerExtensionTabPage : IFiddlerExtension
    {
        TabPage oPage;

        public void OnLoad()
        {
            // Load the UI.
            FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);
        }

        public void OnBeforeUnload()
        {
            // Some things.
        }
    public Office365FiddlerExtensionTabPage()
    {
        // Add tab page to Fiddler.
            Office365TabPage oView = new Office365TabPage();

            oPage = new TabPage($"{LangHelper.GetString("Office 365 Fiddler Extension")}");
            oPage.ImageIndex = (int)Fiddler.SessionIcons.HTML;

            oView.Dock = DockStyle.Fill;

            oPage.Controls.Add(oView);
        }
    }
}
