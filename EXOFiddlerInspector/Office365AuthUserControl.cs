using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace EXOFiddlerInspector
{
    public partial class Office365AuthUserControl : UserControl
    {
        public Office365AuthUserControl()
        {
            InitializeComponent();
        }

        // Code to write to ResponseProcessTextBox.Text value.
        internal void SetAuthTextBox(string txt)
        {
            AuthTextBox.Text = txt;
        }

        internal void SetAuthenticationResponseComments(string txt)
        {
            AuthenticationResponseComments.Text = txt;
        }
    }
}
