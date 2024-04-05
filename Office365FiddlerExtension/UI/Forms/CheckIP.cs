using Office365FiddlerExtension.Services;
using System;
using System.Windows.Forms;

namespace Office365FiddlerExtension.UI.Forms
{
    public partial class CheckIP : Form
    {
        public CheckIP()
        {
            InitializeComponent();
        }

        private void CheckIPButton_Click(object sender, EventArgs e)
        {
            if (!NetworkingService.Instance.IsValidIPAddress(IPAddressTextbox.Text))
            {
                ResultTextbox.Text = $"{IPAddressTextbox.Text} is not a valid IP address.";
                return;
            }

            Tuple<bool, string> tupleIsPrivateIPAddress = NetworkingService.Instance.IsPrivateIPAddress(IPAddressTextbox.Text);

            // IP address is in a private subnet.
            if (tupleIsPrivateIPAddress.Item1)
            {
                ResultTextbox.Text = tupleIsPrivateIPAddress.Item2;
            }
            // IP address is not in a private subnet.
            else
            {
                Tuple<bool, string> tupleIsMicrosoftIPAddress = NetworkingService.Instance.IsMicrosoft365IPAddress(IPAddressTextbox.Text);

                // IP address is a Microsoft 365 IP address.
                if (tupleIsMicrosoftIPAddress.Item1)
                {
                    ResultTextbox.Text = $"{IPAddressTextbox.Text} is within the Microsoft 365 subnet {tupleIsMicrosoftIPAddress.Item2}";
                }
                // IP address is not a Microsoft 365 IP address.
                else
                {
                    ResultTextbox.Text = $"{IPAddressTextbox.Text} is a public IP address not within a Microsoft 365 subnet.";
                }
            }
        }
    }
}
