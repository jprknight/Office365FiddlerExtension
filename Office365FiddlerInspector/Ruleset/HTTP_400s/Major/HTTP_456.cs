using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerInspector.Services;
using Fiddler;

namespace Office365FiddlerInspector.Ruleset
{
    class HTTP_456 : ActivationService
    {
        GetSetSessionFlags getSetSessionFlags = new GetSetSessionFlags();

        public void HTTP_456_Multi_Factor_Required(Session session)
        {
            this.session = session;

            if (this.session.utilFindInResponse("you must use multi-factor authentication", false) > 1)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 456 Multi-Factor Required!");

                getSetSessionFlags.SetUIBackColour(this.session, "Red");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetResponseCodeDescription(this.session, "HTTP 456 !Multi-Factor Auth!");

                getSetSessionFlags.SetSessionType(this.session, "!Multi-Factor Auth!");
                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 456 Multi-Factor Authentication</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "See details on Raw tab. Look for the presence of 'you must use multi-factor authentication'." +
                    "<p>This has been seen where users have <b><span style='color:red'>MFA enabled/enforced, but Modern Authentication is not enabled</span></b> in the Office 365 workload being connected to</p?" +
                    "<p>See <a href='https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662' target='_blank'>" +
                    "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>" +
                    "<p><a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>" +
                    "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a>");

                // Set confidence level for Session Authentication, Session Type, and Session Response Server.
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
            else if (session.utilFindInResponse("oauth_not_available", false) > 1)
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 456 Multi-Factor Required!");

                getSetSessionFlags.SetUIBackColour(this.session, "Red");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetSessionType(this.session, "!Multi-Factor Auth!");

                this.session["X-ResponseAlert"] = "<b><span style='color:red'>HTTP 456 Multi-Factor Authentication</span></b>";
                this.session["X-ResponseComments"] = "See details on Raw tab. Look for the presence of 'oauth_not_available'."
                    + "<p>This has been seen where users have <b><span style='color:red'>MFA enabled/enforced, but Modern Authentication</span></b> is not enabled in the Office 365 workload being connected to</p>"
                    + "<p>See <a href='https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662' target='_blank'>" +
                    "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>"
                    + "<p><a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>" +
                    "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a></p>";

                // Set confidence level for Session Authentication (SACL), Session Type (STCL), and Session Response Server (SRSCL).
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "10");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
            else
            {
                FiddlerApplication.Log.LogString("Office365FiddlerExtension: " + this.session.id + " HTTP 456 Multi-Factor Required.");

                getSetSessionFlags.SetUIBackColour(this.session, "Orange");
                getSetSessionFlags.SetUITextColour(this.session, "Black");

                getSetSessionFlags.SetResponseCodeDescription(this.session, "HTTP 456 Multi-Factor Authentication?");

                getSetSessionFlags.SetSessionType(this.session, "Multi-Factor Auth?");
                getSetSessionFlags.SetXResponseAlert(this.session, "<b><span style='color:red'>HTTP 456 Multi-Factor Authentication?</span></b>");
                getSetSessionFlags.SetXResponseComments(this.session, "See details on Raw tab. Is Modern Authentication disabled?"
                    + "<p>This has been seen where users have <b><span style='color:red'>MFA enabled/enforced, but Modern Authentication is not enabled</span></b> in the Office 365 workload being connected to.</p>"
                    + "<p>See <a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>"
                    + "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>"
                    + "<a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>"
                    + "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a></p>");
                
                // Possible something more to be found, let further processing try to pick up something.
                getSetSessionFlags.SetSessionAuthenticationConfidenceLevel(this.session, "5");
                getSetSessionFlags.SetSessionTypeConfidenceLevel(this.session, "5");
                getSetSessionFlags.SetSessionResponseServerConfidenceLevel(this.session, "5");
            }
        }
    }
}