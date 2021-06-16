using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Office365FiddlerInspector.UI
{
    public partial class Office365TabPage : UserControl
    { 
        public static WebBrowser TabPageResultsOutput { get; set; }

        public Office365TabPage()
        {
            InitializeComponent();

            TabPageResultsOutput = TabPageWebBrowser;

            TabPageWebBrowser.DocumentText = "<h2>Click a session for analysis.</h2>";
        }
    }
}
