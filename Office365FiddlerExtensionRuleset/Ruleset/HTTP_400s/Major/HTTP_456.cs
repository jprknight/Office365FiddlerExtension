﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_456
    {
        internal Session session { get; set; }

        private static HTTP_456 _instance;

        public static HTTP_456 Instance => _instance ?? (_instance = new HTTP_456());

        public void HTTP_456_Multi_Factor_Required(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("you must use multi-factor authentication", false) > 1))
            {
                return;
            }
             
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 456 Multi-Factor Required!");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "!Multi-Factor Auth!",
                ResponseCodeDescription = "HTTP 456 !Multi-Factor Auth!",
                ResponseAlert = "<b><span style='color:red'>HTTP 456 Multi-Factor Authentication</span></b>",
                ResponseComments = "See details on Raw tab. Look for the presence of 'you must use multi-factor authentication'." +
                "<p>This has been seen where users have <b><span style='color:red'>MFA enabled/enforced, but Modern Authentication is not enabled</span></b> in the Office 365 workload being connected to</p?" +
                "<p>See <a href='https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662' target='_blank'>" +
                "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>" +
                "<p><a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>" +
                "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void HTTP_456_OAuth_Not_Available(Session session)
        {
            this.session = session;

            if (!(session.utilFindInResponse("oauth_not_available", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 456 Multi-Factor Required!");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "!Multi-Factor Auth!",
                ResponseCodeDescription = "",
                ResponseAlert = "<b><span style='color:red'>HTTP 456 Multi-Factor Authentication</span></b>",
                ResponseComments = "See details on Raw tab. Look for the presence of 'oauth_not_available'."
                    + "<p>This has been seen where users have <b><span style='color:red'>MFA enabled/enforced, but Modern Authentication</span></b> is not enabled in the Office 365 workload being connected to</p>"
                    + "<p>See <a href='https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662' target='_blank'>" +
                    "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>"
                    + "<p><a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>" +
                    "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a></p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);
        }

        public void HTTP_456_Anything_Else(Session session)
        {
            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 456 Multi-Factor Required.");

            var sessionFlags = new SessionFlagHandler.ExtensionSessionFlags()
            {
                SectionTitle = "",
                UIBackColour = "Red",
                UITextColour = "Black",

                SessionType = "Multi-Factor Auth?",
                ResponseCodeDescription = "HTTP 456 Multi-Factor Authentication?",
                ResponseAlert = "<b><span style='color:red'>HTTP 456 Multi-Factor Authentication?</span></b>",
                ResponseComments = "See details on Raw tab. Is Modern Authentication disabled?"
                    + "<p>This has been seen where users have <b><span style='color:red'>MFA enabled/enforced, but Modern Authentication is not enabled</span></b> in the Office 365 workload being connected to.</p>"
                    + "<p>See <a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>"
                    + "https://support.office.com/en-us/article/Enable-or-disable-modern-authentication-in-Exchange-Online-58018196-f918-49cd-8238-56f57f38d662 </a></p>"
                    + "<a href='https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx' target='_blank'>"
                    + "https://social.technet.microsoft.com/wiki/contents/articles/36101.office-365-enable-modern-authentication.aspx </a></p>",

                SessionAuthenticationConfidenceLevel = 5,
                SessionTypeConfidenceLevel = 10,
                SessionResponseServerConfidenceLevel = 5
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagHandler.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson);

        }
    }
}