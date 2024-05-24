using System;
using Office365FiddlerExtension.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_502
    {
        internal Session session { get; set; }

        private static HTTP_502 _instance;

        public static HTTP_502 Instance => _instance ?? (_instance = new HTTP_502());

        public void Run(Session session)
        {
            this.session = session;

            HTTP_502_Bad_Gateway_Telemetry_False_Positive(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }
            
            HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionTypeConfidenceLevel_Ten(this.session))
            {
                return;
            }

            HTTP_502_Bad_Gateway_Anything_Else(this.session);
        }

        private void HTTP_502_Bad_Gateway_Telemetry_False_Positive(Session session)
        {
            // Telemetry false positive.

            this.session = session;

            if (this.session.oRequest["Host"] != "sqm.telemetry.microsoft.com:443")
            {
                return;
            }

            if (this.session.utilFindInResponse("target machine actively refused it", false) <= 1)
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 502 Bad Gateway. Telemetry False Positive.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_Telemetry_False_Positive");
                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 20;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",

                SessionType = LangHelper.GetString("HTTP_502_Bad_Gateway_Telemetry_False_Positive_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_502_Bad_Gateway_Telemetry_False_Positive_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_502_Bad_Gateway_Telemetry_False_Positive_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_502_Bad_Gateway_Telemetry_False_Positive_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);            
        }

        private void HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive(Session session)
        {
            // Exchange Online DNS lookup on contoso.onmicrosoft.com, False Positive.

            this.session = session;

            if (!(this.session.utilFindInResponse("DNS Lookup for ", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse(".onmicrosoft.com", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse(" failed.", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 502 Bad Gateway. EXO DNS False Positive.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive");
                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 20;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",

                SessionType = LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseComments"),
                ResponseServer = LangHelper.GetString("False Positive"),
                Authentication = LangHelper.GetString("False Positive"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);            
        }

        private void HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive(Session session)
        {
            // Exchange Online connection to autodiscover.contoso.mail.onmicrosoft.com, False Positive.

            this.session = session;

            if (!(this.session.utilFindInResponse(".onmicrosoft.com", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("autodiscover", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("target machine actively refused it", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 502 Bad Gateway. O365 AutoD onmicrosoft.com False Positive.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive");
                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 20;
            }

            string AutoDFalsePositiveDomain;
            string AutoDFalsePositiveResponseBody = this.session.GetResponseBodyAsString();
            int start = this.session.GetResponseBodyAsString().IndexOf("'");
            int end = this.session.GetResponseBodyAsString().LastIndexOf("'");
            int charcount = end - start;
            if (charcount > 0)
            {
                AutoDFalsePositiveDomain = AutoDFalsePositiveResponseBody.Substring(start, charcount).Replace("'", "");
            }
            else
            {
                AutoDFalsePositiveDomain = $"<{LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_Domain_Not_Detected")}>";
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",

                SessionType = LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseCommentsStart")
                + " "
                + AutoDFalsePositiveDomain
                + " "
                + LangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseCommentsEnd"),
                ResponseServer = LangHelper.GetString("False Positive"),
                Authentication = LangHelper.GetString("False Positive"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);
        }

        private void HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover(Session session)
        {
            // Anything else Exchange Autodiscover.

            this.session = session;

            if (!(this.session.utilFindInResponse("target machine actively refused it", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("autodiscover", false) > 1))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 502 Bad Gateway. Exchange Autodiscover.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover");
                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 60;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover",

                SessionType = LangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);            
        }

        private void HTTP_502_Bad_Gateway_Anything_Else(Session session)
        {
            // Everything else.

            this.session = session;

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 502 Bad Gateway.");

            int sessionAuthenticationConfidenceLevel;
            int sessionTypeConfidenceLevel;
            int sessionResponseServerConfidenceLevel;
            int sessionSeverity;

            try
            {
                var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_Anything_Else");
                sessionAuthenticationConfidenceLevel = sessionClassificationJson.SessionAuthenticationConfidenceLevel;
                sessionTypeConfidenceLevel = sessionClassificationJson.SessionTypeConfidenceLevel;
                sessionResponseServerConfidenceLevel = sessionClassificationJson.SessionResponseServerConfidenceLevel;
                sessionSeverity = sessionClassificationJson.SessionSeverity;
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} USING HARDCODED SESSION CLASSIFICATION VALUES.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} {ex}");

                sessionAuthenticationConfidenceLevel = 5;
                sessionTypeConfidenceLevel = 10;
                sessionResponseServerConfidenceLevel = 5;
                sessionSeverity = 60;
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",

                SessionType = LangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_SessionType"),
                ResponseCodeDescription = LangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_ResponseCodeDescription"),
                ResponseAlert = LangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_ResponseAlert"),
                ResponseComments = LangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
