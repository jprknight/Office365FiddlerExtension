using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EXOFiddlerInspector
{
    public partial class RequestUserControl : UserControl
    {
        public RequestUserControl()
        {
            InitializeComponent();
        }

        private void RequestUserControl_Load(object sender, EventArgs e)
        {
            
        }

        public void SetRequestCommentsTextBoxText(string txt)
        {
            this.RequestCommentsTextBox.Text = txt;
        }

         

        //public static implicit operator RequestUserControl(ResponseUserControl v)
        //{
        //   throw new NotImplementedException();
        //}
    }
}
