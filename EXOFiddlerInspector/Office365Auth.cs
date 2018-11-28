using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace EXOFiddlerInspector
{
    public class Office365Auth : EXOBaseFiddlerInspector, IResponseInspector2
    {
        public Office365AuthUserControl _Office365AuthUserControl;

        private HTTPResponseHeaders responseHeaders;

        public HTTPResponseHeaders headers
        {
            get { return responseHeaders; }
            set { responseHeaders = value; }
        }

        
        public byte[] body
        {
            get { return rawBody; }
            set
            {
                if (bExtensionEnabled)
                {
                    SetOffice365AuthenticationValues(this.session);
                }
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public override int GetOrder()
        {
            return 0;
        }

        /////////////////////////////
        // Add the EXO Response tab into the inspector tab.
        public override void AddToTab(TabPage o)
        {
            _Office365AuthUserControl = new Office365AuthUserControl();
            o.Text = "Office365 Auth";
            o.ToolTipText = "Office365 Auth";
            o.Controls.Add(_Office365AuthUserControl);
            o.Controls[0].Dock = DockStyle.Fill;
        }

        public void SetOffice365AuthenticationValues(Session session)
        {
            this.session = session;

            _Office365AuthUserControl.SetAuthTextBox(this.session["X-Authentication"]);

            _Office365AuthUserControl.SetAuthenticationResponseComments(this.session["X-AuthenticationDesc"]);

        }
    }
}
