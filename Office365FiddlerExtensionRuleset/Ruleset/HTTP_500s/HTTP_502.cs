using System;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;
using Office365FiddlerExtension.Services;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_502
    {
        internal Session session { get; set; }

        private static HTTP_502 _instance;

        public static HTTP_502 Instance => _instance ?? (_instance = new HTTP_502());

        /// <summary>
        /// Set session analysis values for a HTTP 502 response code.
        /// </summary>
        /// <param name="session"></param>
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

            HTTP_502_Bad_Gateway_AutoDiscover_Refused_By_EXO_Vanity_Domain(this.session);
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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_Telemetry_False_Positive");
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",

                SessionType = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Telemetry_False_Positive_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Telemetry_False_Positive_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Telemetry_False_Positive_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Telemetry_False_Positive_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);            
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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive");
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",

                SessionType = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_DNS_Lookup_False_Positive_ResponseComments"),
                ResponseServer = RulesetLangHelper.GetString("False Positive"),
                Authentication = RulesetLangHelper.GetString("False Positive"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);            
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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive");
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
                AutoDFalsePositiveDomain = $"<{RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_Domain_Not_Detected")}>";
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",

                SessionType = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseCommentsStart")
                + " "
                + AutoDFalsePositiveDomain
                + " "
                + RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_EXO_AutoDiscover_False_Positive_ResponseCommentsEnd"),
                ResponseServer = RulesetLangHelper.GetString("False Positive"),
                Authentication = RulesetLangHelper.GetString("False Positive"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, true);
        }


        /// <summary>
        /// Detect false positives for Autodiscover to the Microsoft cloud using a vanity domain.
        /// The rule here checks the IP address which refused the connection to confirm it is a Microsoft cloud IP address.
        /// If it is the inspector session analysis calls this out.
        /// This is a good example of where NeverWebCall impacts the extension analysis. If we can't make web calls, we can't
        /// retrieve the M365 URLs and IPs to check against and provide the most valuable analysis.
        /// </summary>
        /// <param name="session"></param>
        private void HTTP_502_Bad_Gateway_AutoDiscover_Refused_By_EXO_Vanity_Domain(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("The connection to 'autodiscover.", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("target machine actively refused it", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse("autodiscover", false) > 1))
            {
                return;
            }

            if (!(this.session.utilFindInResponse(":443", false) > 1))
            {
                return;
            }

            // Get the IP address from the response body.
            string ResponseBodyIPAddress = this.session.GetResponseBodyAsString();
            int start = this.session.GetResponseBodyAsString().IndexOf("target machine actively refused it") + 34;
            int end = this.session.GetResponseBodyAsString().LastIndexOf(":443");

            ResponseBodyIPAddress = ResponseBodyIPAddress.Substring(start, end - start);
            ResponseBodyIPAddress = ResponseBodyIPAddress.Trim();

            Tuple<bool, string> tupleIsMicrosoftIPAddress = Tuple.Create(false, "intialise");

            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                tupleIsMicrosoftIPAddress = Tuple.Create(false, "NeverWebCall true.");
            }
            else
            {
                tupleIsMicrosoftIPAddress = NetworkingService.Instance.IsMicrosoft365IPAddress(ResponseBodyIPAddress);
            }
            
            // IP address is a Microsoft 365 IP address.
            if (tupleIsMicrosoftIPAddress.Item1)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 502 Bad Gateway. Exchange Autodiscover.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_AutoDiscover_Cloud_Vanity_Domain_False_Positive");
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

                var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_502s",

                    SessionType = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_AutoDiscover_Cloud_Vanity_Domain_False_Positive_SessionType"),
                    ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_AutoDiscover_Cloud_Vanity_Domain_False_Positive_ResponseCodeDescription"),
                    ResponseAlert = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_AutoDiscover_Cloud_Vanity_Domain_False_Positive_ResponseAlert"),
                    ResponseComments = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_AutoDiscover_Cloud_Vanity_Domain_False_Positive_ResponseCommentsStart")
                    + " "
                    + ResponseBodyIPAddress
                    + " "
                    + RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_AutoDiscover_Cloud_Vanity_Domain_False_Positive_ResponseCommentsEnd"),

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

                // $"{EnterIPAddressTextBox.Text} is within the Microsoft 365 subnet {tupleIsMicrosoftIPAddress.Item2}";
            }
            // IP address is not a Microsoft 365 IP address.
            else
            {
                // $"{EnterIPAddressTextBox.Text} is a public IP address not within a Microsoft 365 subnet.";
            }
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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover");
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover",

                SessionType = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_AutoDiscover_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);            
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
                var sessionClassificationJson = RulesetSessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_502s|HTTP_502_Bad_Gateway_Anything_Else");
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

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "HTTP_502s",

                SessionType = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_SessionType"),
                ResponseCodeDescription = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_ResponseCodeDescription"),
                ResponseAlert = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_ResponseAlert"),
                ResponseComments = RulesetLangHelper.GetString("HTTP_502_Bad_Gateway_Anything_Else_ResponseComments"),

                SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                SessionSeverity = sessionSeverity
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
