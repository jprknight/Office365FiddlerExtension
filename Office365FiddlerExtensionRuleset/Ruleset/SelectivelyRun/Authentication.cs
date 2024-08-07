﻿using System;
using Office365FiddlerExtensionRuleset.Services;
using Fiddler;
using Newtonsoft.Json;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class Authentication
    {
        internal Session session { get; set; }

        private static Authentication _instance;

        public static Authentication Instance => _instance ?? (_instance = new Authentication());

        /// <summary>
        /// Set authentication based on data available in the session. Used in the UI column and response inspector.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            SetAuthentication_NoAuthHeaders(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionAuthenticationConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetAuthentication_SAML_Parser(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionAuthenticationConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetAuthentication_Basic_Modern_Auth_Disabled(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionAuthenticationConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetAuthentication_Modern_Auth_Capable_Client(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionAuthenticationConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetAuthentication_Modern_Auth_Client_Using_Token(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionAuthenticationConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetAuthentication_Basic_Auth_Capable_Client(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionAuthenticationConfidenceLevel_Ten(this.session))
            {
                return;
            }

            SetAuthentication_Basic_Auth_Client_Using_Token(this.session);
            if (RulesetUtilities.Instance.StopProcessing_SessionAuthenticationConfidenceLevel_Ten(this.session))
            {
                return;
            }
        }

        /// <summary>
        /// Set authentication Json session flags when there are no authentication headers in the session.
        /// </summary>
        /// <param name="session"></param>
        private void SetAuthentication_NoAuthHeaders(Session session)
        {
            this.session = session;

            if (this.session.oRequest["Authorization"].Contains("Bearer") 
                || this.session.oRequest["Authorization"].Contains("Basic")
                || this.session.uriContains("adfs/ls"))
            {
                // Do nothing here, this is a session which is detected to have auth headers.
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Running SetAuthentication_NoAuthHeaders.");

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_No_Auth_Headers",
                Authentication = RulesetLangHelper.GetString("Authentication_No_Auth_Headers"),
                AuthenticationDescription = RulesetLangHelper.GetString("Authentication_No_Auth_Headers"),
                AuthenticationType = RulesetLangHelper.GetString("Authentication_No_Auth_Headers"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set authentication Json session flags for the SAML Parser.
        /// </summary>
        /// <param name="session"></param>
        private void SetAuthentication_SAML_Parser(Session session)
        {
            this.session = session;

            // Determine if this session contains a SAML response.
            if (this.session.utilFindInResponse("Issuer=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1 &&
                this.session.utilFindInResponse("NameIdentifier Format=", false) > 1 &&
                this.session.utilFindInResponse("Attribute AttributeName=", false) > 1)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} ADFS SAML Request/Response.");

                // wrap all of this in a check to see if the SAML token came back from an ADFS endpoint.
                // If it didn't we don't have the labs setup to validate how 3rd-party IDPs format things
                // out for SAML tokens.
                if (this.session.uriContains("adfs/ls"))
                {

                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "Authentication_SAML_Response_Parser",
                        SessionType = RulesetLangHelper.GetString("Authentication_SAML_Response_Parser_SessionType"),
                        ResponseComments = RulesetLangHelper.GetString("Authentication_SAML_Response_Parser_ResponseComments"),

                        Authentication = RulesetLangHelper.GetString("Authentication_SAML_Response_Parser_Authentication"),
                        AuthenticationType = RulesetLangHelper.GetString("Authentication_SAML_Response_Parser_AuthenticationType"),
                        AuthenticationDescription = RulesetLangHelper.GetString("Authentication_SAML_Response_Parser_AuthenticationDescription"),

                        SessionAuthenticationConfidenceLevel = 10,
                        SessionTypeConfidenceLevel = 10
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

                    // JK 6/30/2021
                    // All the below logic was build with an ADFS SAML token from a lab environment.
                    // Discovered the makeup of SAML tokens from other providers do not follow the exact
                    // same structure.
                    // Added try catch statements and validation checks on string lengths prior to attempting
                    // substring operations to prevent running into "Length cannot be less than zero" exceptions.

                    SetTokenIssuer(this.session);

                    SetSigningCertificate(this.session);

                    SetAttributeNameUPN(this.session);

                    SetNameIdentifierFormat(this.session);

                    SetAttributeNameImmutableID(this.session);
                }
                else
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                        $"({this.GetType().Name}): {this.session.id} Third-party SAML response found. SAML response parser not running.");

                    var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "Authentication_3rd_Party_Saml_Response",

                        SessionType = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_SessionType"),
                        ResponseComments = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_ResponseComments"),

                        AuthenticationType = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_AuthenticationType"),
                        AuthenticationDescription = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_AuthenticationDescription"),

                        SamlTokenIssuer = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),
                        SamlTokenSigningCertificate = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),
                        SamlTokenAttributeNameUPN = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),
                        SamlTokenNameIdentifierFormat = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),
                        SamlTokenAttributeNameImmutibleID = RulesetLangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),

                        SessionAuthenticationConfidenceLevel = 10
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
            }
        }

        /// <summary>
        /// Set authentication Json session flags when a client is doing basic authentication, modern authentication is disabled.
        /// A depreciated scenario for Exchange Online, leaving for Exchange OnPremise.
        /// </summary>
        /// <param name="session"></param>
        private void SetAuthentication_Basic_Modern_Auth_Disabled(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Basic")) {
                return;
            }

            SAMLParserFieldsNoData(this.session);

            int KeywordFourMillion = RulesetUtilities.Instance.SearchForWord(this.session, "4000000");
            int KeywordFlighting = RulesetUtilities.Instance.SearchForWord(this.session, "Flighting");
            int Keywordenabled = RulesetUtilities.Instance.SearchForWord(this.session, "enabled");
            int Keyworddomain = RulesetUtilities.Instance.SearchForWord(this.session, "domain");
            int Keywordoauth_not_available = RulesetUtilities.Instance.SearchForWord(this.session, "oauth_not_available");

            if (KeywordFourMillion == 0 && KeywordFlighting == 0 && Keywordenabled == 0 &&
                Keyworddomain == 0 && Keywordoauth_not_available == 0)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} Modern Auth Disabled.");

                DateTime today = DateTime.Today;

                var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication_Modern_Auth_Disabled",

                    Authentication = RulesetLangHelper.GetString("Authentication_Modern_Auth_Disabled_Authentication"),
                    AuthenticationDescription = RulesetLangHelper.GetString("Authentication_Modern_Auth_Disabled_AuthenticationDescriptionStart")
                        + $" {today:yyyy} "
                        + RulesetLangHelper.GetString("Authentication_Modern_Auth_Disabled_AuthenticationDescriptionEnd"),

                    SessionAuthenticationConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }

        /// <summary>
        /// Set authentication Json session flags when a client is using trying to use modern (bearer) authentication.
        /// </summary>
        /// <param name="session"></param>
        private void SetAuthentication_Modern_Auth_Capable_Client(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Bearer"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Client Modern Auth.");

            var ExtensionSessionFlags = RulesetSessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_Modern_Auth_Capable_Client",
                
                Authentication = RulesetLangHelper.GetString("Authentication_Modern_Auth_Capable_Client_Authentication"),
                AuthenticationDescription = ExtensionSessionFlags.ProcessName 
                    + " "
                    + RulesetLangHelper.GetString("Authentication_Modern_Auth_Capable_Client_AuthenticationDescription"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set authentication Json session flags when a client is trying to use basic authentication.
        /// </summary>
        /// <param name="session"></param>
        private void SetAuthentication_Basic_Auth_Capable_Client(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Basic"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Client Basic Auth.");

            var ExtensionSessionFlags = RulesetSessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_Basic_Auth_Capable_Client",

                Authentication = RulesetLangHelper.GetString("Authentication_Basic_Auth_Capable_Client_Authentication"),
                AuthenticationDescription = ExtensionSessionFlags.ProcessName 
                    + " "
                    + RulesetLangHelper.GetString("Authentication_Basic_Auth_Capable_Client_AuthenticationDescription"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set authentication Json session flags when a client is using modern authentication with a token.
        /// </summary>
        /// <param name="session"></param>
        private void SetAuthentication_Modern_Auth_Client_Using_Token(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Bearer"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Client Modern Auth Token.");

            var ExtensionSessionFlags = RulesetSessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_Modern_Auth_Client_Using_Token",

                Authentication = RulesetLangHelper.GetString("Authentication_Modern_Auth_Client_Using_Token_Authentication"),
                AuthenticationDescription = ExtensionSessionFlags.ProcessName 
                    + " "
                    + RulesetLangHelper.GetString("Authentication_Modern_Auth_Client_Using_Token_AuthenticationDescription"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set authentication Json session flags when a client is using basic authentication with a token.
        /// </summary>
        /// <param name="session"></param>
        private void SetAuthentication_Basic_Auth_Client_Using_Token(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Basic"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Client Basic Auth Token.");
            
            var ExtensionSessionFlags = RulesetSessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_Basic_Auth_Client_Using_Token",

                Authentication = RulesetLangHelper.GetString("Authentication_Basic_Auth_Client_Using_Token_Authentication"),
                AuthenticationDescription = ExtensionSessionFlags.ProcessName 
                    + " "
                    + RulesetLangHelper.GetString("Authentication_Basic_Auth_Client_Using_Token_AuthenticationDescription"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set token issuer within the SAML Parser.
        /// </summary>
        /// <param name="session"></param>
        private void SetTokenIssuer(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("Issuer=", false) > 1) && !(this.session.utilFindInResponse("IssueInstant=", false) > 1))
            {
                var sessionFlagsNotDetermined = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (SetTokenIssuer)",

                    Authentication = RulesetLangHelper.GetString("TokenIssuer_Could_Not_Be_Determined")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} SAML token issuer.");

            string Issuer;
            try
            {
                // Pull issuer data from response.
                string IssuerSessionBody = this.session.ToString();
                int IssuerStartIndex = IssuerSessionBody.IndexOf("Issuer=");
                int IssuerEndIndex = IssuerSessionBody.IndexOf("IssueInstant=");
                int IssuerLength = IssuerEndIndex - IssuerStartIndex;
                if (IssuerLength > 0)
                {
                    Issuer = IssuerSessionBody.Substring(IssuerStartIndex, IssuerLength);
                    Issuer = Issuer.Replace("&quot;", "");
                    Issuer = Issuer.Replace("Issuer=", "");
                }
                else
                {
                    Issuer = RulesetLangHelper.GetString("TokenIssuer_Could_Not_Be_Determined");
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML token issuer could not be determined. {e}");

                Issuer = RulesetLangHelper.GetString("TokenIssuer_Could_Not_Be_Determined");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SetTokenIssuer)",

                SamlTokenIssuer = Issuer
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set signing certificate within the SAML Parser.
        /// </summary>
        /// <param name="session"></param>
        private void SetSigningCertificate(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;X509Certificate>", false) > 1) && !(this.session.utilFindInResponse("&lt;/X509Certificate>", false) > 1))
            {
                var sessionFlagsNotDetermined = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (SigningCertificate)",

                    SamlTokenSigningCertificate = RulesetLangHelper.GetString("SamlToken_SigningCertificate_Data_Points_Not_Found")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} SAML signing certificate.");

            string x509SigningCertificate;
            try
            {
                string x509SigningCertSessionBody = this.session.ToString();
                int x509SigningCertificateStartIndex = x509SigningCertSessionBody.IndexOf("&lt;X509Certificate>") + 20; // 20 to shift to start of the selection.
                int x509SigningCertificateEndIndex = x509SigningCertSessionBody.IndexOf("&lt;/X509Certificate>");
                int x509SigningCertificateLength = x509SigningCertificateEndIndex - x509SigningCertificateStartIndex;
                if (x509SigningCertificateLength > 0)
                {
                    x509SigningCertificate = x509SigningCertSessionBody.Substring(x509SigningCertificateStartIndex, x509SigningCertificateLength);
                }
                else
                {
                    x509SigningCertificate = RulesetLangHelper.GetString("SamlToken_SigningCertificate_Could_Not_Be_Determined");
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML signing certificate could not be determined. {e}");

                x509SigningCertificate = RulesetLangHelper.GetString("SamlToken_SigningCertificate_Could_Not_Be_Determined");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SigningCertificate)",

                SamlTokenSigningCertificate = x509SigningCertificate
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set AttributeNameUPN within the SAML Parser.
        /// </summary>
        /// <param name="session"></param>
        private void SetAttributeNameUPN(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:Attribute AttributeName=&quot;UPN", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;/saml:Attribute>", false) > 1))
            {

                var sessionFlagsNotDetermined = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (AttributeNameUPN)",

                    SamlTokenAttributeNameUPN = RulesetLangHelper.GetString("SamlToken_AttributeNameUPN_Data_Points_Not_Found")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} SAML AttributeNameUPN.");

            string AttributeNameUPN;
            try
            {
                string AttributeNameUPNSessionBody = this.session.ToString();
                int AttributeNameUPNStartIndex = AttributeNameUPNSessionBody.IndexOf("&lt;saml:Attribute AttributeName=&quot;UPN");
                int AttributeNameUPNEndIndex = AttributeNameUPNSessionBody.IndexOf("&lt;/saml:Attribute>");
                int AttributeNameUPNLength = AttributeNameUPNEndIndex - AttributeNameUPNStartIndex;
                if (AttributeNameUPNLength > 0)
                {
                    AttributeNameUPN = AttributeNameUPNSessionBody.Substring(AttributeNameUPNStartIndex, AttributeNameUPNLength);
                    AttributeNameUPN = AttributeNameUPN.Replace("&quot;", "\"");
                    AttributeNameUPN = AttributeNameUPN.Replace("&lt;", "<");
                    // Now split the two lines with a new line for easier reading in the user control.
                    int SplitAttributeNameUPNStartIndex = AttributeNameUPN.IndexOf("<saml:AttributeValue>") + 21;

                    int SplitAttributeNameUPNEndIndex = AttributeNameUPN.IndexOf("</saml:AttributeValue>");
                    int SplitAttributeNameLength = SplitAttributeNameUPNEndIndex - SplitAttributeNameUPNStartIndex;

                    //string AttributeNameUPNFirstLine = AttributeNameUPN.Substring(0, SplitAttributeNameUPNStartIndex);
                    //string AttributeNameUPNSecondLine = AttributeNameUPN.Substring(SplitAttributeNameUPNStartIndex);

                    if (SplitAttributeNameLength > 0)
                    {
                        AttributeNameUPN = AttributeNameUPN.Substring(SplitAttributeNameUPNStartIndex, SplitAttributeNameLength);
                    }
                    else
                    {
                        AttributeNameUPN = RulesetLangHelper.GetString("SamlToken_AttributeNameUPN_Could_Not_Be_Determined");
                    }
                }
                else
                {
                    AttributeNameUPN = RulesetLangHelper.GetString("SamlToken_AttributeNameUPN_Could_Not_Be_Determined");
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML AttributeNameUPN could not be determined. {e}");
                AttributeNameUPN = RulesetLangHelper.GetString("SamlToken_AttributeNameUPN_Could_Not_Be_Determined");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (AttributeNameUPN)",
                SamlTokenAttributeNameUPN = AttributeNameUPN,
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set NameIdentifierFormat within the SAML Parser.
        /// </summary>
        /// <param name="session"></param>
        private void SetNameIdentifierFormat(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
            {

                var sessionFlagsNotDetermined = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (NameIdentifierFormat)",
                    SamlTokenNameIdentifierFormat = RulesetLangHelper.GetString("SamlToken_NameIdentifierFormat_Data_Points_Not_Found")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} SAML NameIdentifierFormat.");

            string NameIdentifierFormat;
            try
            {
                string NameIdentifierFormatSessionBody = this.session.ToString();
                int NameIdentifierFormatStartIndex = NameIdentifierFormatSessionBody.IndexOf("&lt;saml:NameIdentifier Format");
                int NameIdentifierFormatEndIndex = NameIdentifierFormatSessionBody.IndexOf("&lt;saml:SubjectConfirmation>");
                int NameIdentifierFormatLength = NameIdentifierFormatEndIndex - NameIdentifierFormatStartIndex;
                if (NameIdentifierFormatLength > 0)
                {
                    NameIdentifierFormat = NameIdentifierFormatSessionBody.Substring(NameIdentifierFormatStartIndex, NameIdentifierFormatLength);
                    NameIdentifierFormat = NameIdentifierFormat.Replace("&quot;", "\"");
                    NameIdentifierFormat = NameIdentifierFormat.Replace("&lt;", "<");
                }
                else
                {
                    NameIdentifierFormat = RulesetLangHelper.GetString("SamlToken_NameIdentifierFormat_Data_Points_Not_Found");
                }
            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML NameIdentifierFormat could not be determined. {e}");
                NameIdentifierFormat = RulesetLangHelper.GetString("SamlToken_NameIdentifierFormat_Data_Points_Not_Found");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (NameIdentifierFormat)",

                SamlTokenNameIdentifierFormat = NameIdentifierFormat,
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        /// <summary>
        /// Set AttributeNameImmutableID within the SAML Parser.
        /// </summary>
        /// <param name="session"></param>
        private void SetAttributeNameImmutableID(Session session)
        {
            // AttributeNameImmutableID

            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
            {
                var sessionFlagsNotDetermined = new RulesetSessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (TokenAttributeNameImmutibleID)",

                    SamlTokenAttributeNameImmutibleID = RulesetLangHelper.GetString("SamlToken_TokenAttributeNameImmutibleID_Data_Points_Not_Found")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} SAML AttributeNameImmutableID.");

            string AttributeNameImmutibleID;
            try
            {
                string AttributeNameImmutableIDSessionBody = this.session.ToString();
                int AttributeNameImmutableIDStartIndex = AttributeNameImmutableIDSessionBody.IndexOf("AttributeName=&quot;ImmutableID");
                int AttributeNameImmutibleIDEndIndex = AttributeNameImmutableIDSessionBody.IndexOf("&lt;/saml:AttributeStatement>");
                int AttributeNameImmutibleIDLength = AttributeNameImmutibleIDEndIndex - AttributeNameImmutableIDStartIndex;

                if (AttributeNameImmutibleIDLength > 0)
                {
                    AttributeNameImmutibleID = AttributeNameImmutableIDSessionBody.Substring(AttributeNameImmutableIDStartIndex, AttributeNameImmutibleIDLength);
                    AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&quot;", "\"");
                    AttributeNameImmutibleID = AttributeNameImmutibleID.Replace("&lt;", "<");
                    // Now split out response with a newline for easier reading.
                    int SplitAttributeNameImmutibleIDStartIndex = AttributeNameImmutibleID.IndexOf("<saml:AttributeValue>") + 21;
                    // Add 21 characters to shift where the newline is placed.
                    //string AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                    //string AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;
                    // Second split
                    int SplitAttributeNameImmutibleIDEndIndex = AttributeNameImmutibleID.IndexOf("</saml:AttributeValue></saml:Attribute>");
                    int SubstringLength = SplitAttributeNameImmutibleIDEndIndex - SplitAttributeNameImmutibleIDStartIndex;

                    if (SubstringLength > 0)
                    {
                        AttributeNameImmutibleID = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex, SubstringLength);
                    }
                    else
                    {
                        AttributeNameImmutibleID = RulesetLangHelper.GetString("SamlToken_AttributeNameImmutibleID_Could_Not_Be_Determined");
                    }
                    //AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;
                }
                else
                {
                    AttributeNameImmutibleID = RulesetLangHelper.GetString("SamlToken_AttributeNameImmutibleID_Could_Not_Be_Determined");
                }
            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML AttributeNameImmutibleID could not be determined. {e}");
                AttributeNameImmutibleID = RulesetLangHelper.GetString("SamlToken_AttributeNameImmutibleID_Could_Not_Be_Determined");
            }

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (TokenAttributeNameImmutibleID)",

                SamlTokenAttributeNameImmutibleID = AttributeNameImmutibleID
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
        
        /// <summary>
        /// Set no data within the SAML Parser.
        /// </summary>
        /// <param name="session"></param>
        private void SAMLParserFieldsNoData(Session session)
        {
            this.session = session;

            var sessionFlags = new RulesetSessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SAMLParserFieldsNoData)",

                SamlTokenIssuer = RulesetLangHelper.GetString("SamlToken_No_Data"),
                SamlTokenAttributeNameUPN = RulesetLangHelper.GetString("SamlToken_No_Data"),
                SamlTokenNameIdentifierFormat = RulesetLangHelper.GetString("SamlToken_No_Data"),
                SamlTokenAttributeNameImmutibleID = RulesetLangHelper.GetString("SamlToken_No_Data"),
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            RulesetSessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
