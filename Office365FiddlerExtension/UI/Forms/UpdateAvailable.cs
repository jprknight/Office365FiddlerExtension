using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Office365FiddlerExtension.UI
{
    public partial class UpdateAvailable : Form
    {
        public UpdateAvailable()
        {
            InitializeComponent();
        }

        private void UpdateAvailable_Load(object sender, EventArgs e)
        {
            var extensionVersion = VersionJsonService.Instance.GetDeserializedExtensionVersion();

            String LocalExtensionAssemblyVersion = $"{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}.{Assembly.GetExecutingAssembly().GetName().Version.Build}";

            ExtensionVersionTextbox.Text = LocalExtensionAssemblyVersion;

            // If any of Major, Minor, or Build running locally are less than what's available from Github.
            if (Assembly.GetExecutingAssembly().GetName().Version.Major < extensionVersion.ExtensionMajor ||
                Assembly.GetExecutingAssembly().GetName().Version.Minor < extensionVersion.ExtensionMinor ||
                Assembly.GetExecutingAssembly().GetName().Version.Build < extensionVersion.ExtensionBuild) 
            {
                ExtensionUpdateMessageLabel.Text = "Update Available.";
                ExtensionUpdateMessageLabel.ForeColor = Color.Red;
            }
            else
            {
                ExtensionUpdateMessageLabel.Text = "Up to date.";
                ExtensionUpdateMessageLabel.ForeColor = Color.Green;
            }
            


        }
    }
}
