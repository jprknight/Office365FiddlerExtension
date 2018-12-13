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
            if (!(FiddlerApplication.Prefs.GetBoolPref("extensions.EXOFiddlerExtension.LoadSaz", false))) return;

            this.session = session;

            _Office365AuthUserControl.SetAuthenticationResponseComments(this.session["X-AuthenticationDesc"]);

            _Office365AuthUserControl.SetIssuerTextBox(this.session["X-Issuer"]);
            _Office365AuthUserControl.SetAttributeNameUPNTextBox(this.session["X-AttributeNameUPNTextBox"]);
            _Office365AuthUserControl.SetNameIdentifierFormatTextBox(this.session["X-NameIdentifierFormatTextBox"]);
            _Office365AuthUserControl.SetAttributeNameImmutableIDTextBox(this.session["X-AttributeNameImmutableIDTextBox"]);

            // Make the Office365 Authentication Groupbox visible on the Office365 Auth inspector tab.
            if (this.session["X-Office365AuthType"] == "SAMLResponseParser")
            {
                _Office365AuthUserControl.SetSAMLResponseParserGroupboxVisible(true);
                _Office365AuthUserControl.SetOffice365AuthenticationGroupboxVisible(false);
            }
            // Make the SAML Response Parser visible on the Office365 Auth inspector tab.
            else
            {
                _Office365AuthUserControl.SetSAMLResponseParserGroupboxVisible(false);
                _Office365AuthUserControl.SetOffice365AuthenticationGroupboxVisible(true);
            }

            _Office365AuthUserControl.SetSigningCertificateTextbox(this.session["X-SigningCertificate"]);

        }
    }
}
