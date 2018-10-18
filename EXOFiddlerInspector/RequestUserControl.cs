using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// UNUSED AT THIS TIME.

namespace EXOFiddlerInspector
{
    public partial class RequestUserControl : UserControl
    {
        public RequestUserControl()
        {
            InitializeComponent();
        }

        // Code to write to RequestHostTextBoxTextBox.Text value.
        internal void SetRequestHostTextBox(string txt)
        {
            RequestHostTextBox.Text = txt;
        }

        // Code to write to RequestURLTextBox.Text value.
        internal void SetRequestURLTextBox(string txt)
        {
            RequestURLTextBox.Text = txt;
        }

        // Code to write to RequestTypeTextBox.Text value.
        internal void SetRequestTypeTextBox(string txt)
        {
            RequestTypeTextBox.Text = txt;
        }

        // Code to write to RequestProcessTextBox.Text value.
        internal void SetRequestProcessTextBox(string txt)
        {
            RequestProcessTextBox.Text = txt;
        }
        
        // Code to write to RequestAlertTextBox.Text value.
        internal void SetRequestAlertTextBox(string txt)
        {
            RequestAlertTextBox.Text = txt;
        }


        private void RequestUserControl_Load(object sender, EventArgs e)
        {
            
        }

        private void RequestHostTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
