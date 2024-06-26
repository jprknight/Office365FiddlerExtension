﻿using System;
using Office365FiddlerExtension.Services;
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

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_No_Auth_Headers",
                Authentication = LangHelper.GetString("Authentication_No_Auth_Headers"),
                AuthenticationDescription = LangHelper.GetString("Authentication_No_Auth_Headers"),
                AuthenticationType = LangHelper.GetString("Authentication_No_Auth_Headers"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

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

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "Authentication_SAML_Response_Parser",
                        SessionType = LangHelper.GetString("Authentication_SAML_Response_Parser_SessionType"),
                        ResponseComments = LangHelper.GetString("Authentication_SAML_Response_Parser_ResponseComments"),

                        Authentication = LangHelper.GetString("Authentication_SAML_Response_Parser_Authentication"),
                        AuthenticationType = LangHelper.GetString("Authentication_SAML_Response_Parser_AuthenticationType"),
                        AuthenticationDescription = LangHelper.GetString("Authentication_SAML_Response_Parser_AuthenticationDescription"),

                        SessionAuthenticationConfidenceLevel = 10,
                        SessionTypeConfidenceLevel = 10
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

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

                    var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                    {
                        SectionTitle = "Authentication_3rd_Party_Saml_Response",

                        SessionType = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_SessionType"),
                        ResponseComments = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_ResponseComments"),

                        AuthenticationType = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_AuthenticationType"),
                        AuthenticationDescription = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_AuthenticationDescription"),

                        SamlTokenIssuer = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),
                        SamlTokenSigningCertificate = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),
                        SamlTokenAttributeNameUPN = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),
                        SamlTokenNameIdentifierFormat = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),
                        SamlTokenAttributeNameImmutibleID = LangHelper.GetString("Authentication_3rd_Party_Saml_Response_SamlParserNotRunning"),

                        SessionAuthenticationConfidenceLevel = 10
                    };

                    var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                    SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
                }
            }
        }

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

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication_Modern_Auth_Disabled",

                    Authentication = LangHelper.GetString("Authentication_Modern_Auth_Disabled_Authentication"),
                    AuthenticationDescription = LangHelper.GetString("Authentication_Modern_Auth_Disabled_AuthenticationDescriptionStart")
                        + $" {today:yyyy} "
                        + LangHelper.GetString("Authentication_Modern_Auth_Disabled_AuthenticationDescriptionEnd"),

                    SessionAuthenticationConfidenceLevel = 10
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
        }

        private void SetAuthentication_Modern_Auth_Capable_Client(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Bearer"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Client Modern Auth.");

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_Modern_Auth_Capable_Client",
                
                Authentication = LangHelper.GetString("Authentication_Modern_Auth_Capable_Client_Authentication"),
                AuthenticationDescription = ExtensionSessionFlags.ProcessName 
                    + " "
                    + LangHelper.GetString("Authentication_Modern_Auth_Capable_Client_AuthenticationDescription"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetAuthentication_Basic_Auth_Capable_Client(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Basic"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Client Basic Auth.");

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_Basic_Auth_Capable_Client",

                Authentication = LangHelper.GetString("Authentication_Basic_Auth_Capable_Client_Authentication"),
                AuthenticationDescription = ExtensionSessionFlags.ProcessName 
                    + " "
                    + LangHelper.GetString("Authentication_Basic_Auth_Capable_Client_AuthenticationDescription"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetAuthentication_Modern_Auth_Client_Using_Token(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Bearer"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Client Modern Auth Token.");

            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_Modern_Auth_Client_Using_Token",

                Authentication = LangHelper.GetString("Authentication_Modern_Auth_Client_Using_Token_Authentication"),
                AuthenticationDescription = ExtensionSessionFlags.ProcessName 
                    + " "
                    + LangHelper.GetString("Authentication_Modern_Auth_Client_Using_Token_AuthenticationDescription"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);

        }

        private void SetAuthentication_Basic_Auth_Client_Using_Token(Session session)
        {
            this.session = session;

            if (!this.session.oRequest["Authorization"].Contains("Basic"))
            {
                return;
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): {this.session.id} Client Basic Auth Token.");
            
            var ExtensionSessionFlags = SessionFlagService.Instance.GetDeserializedSessionFlags(this.session);

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication_Basic_Auth_Client_Using_Token",

                Authentication = LangHelper.GetString("Authentication_Basic_Auth_Client_Using_Token_Authentication"),
                AuthenticationDescription = ExtensionSessionFlags.ProcessName 
                    + " "
                    + LangHelper.GetString("Authentication_Basic_Auth_Client_Using_Token_AuthenticationDescription"),

                SessionAuthenticationConfidenceLevel = 10
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetTokenIssuer(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("Issuer=", false) > 1) && !(this.session.utilFindInResponse("IssueInstant=", false) > 1))
            {
                var sessionFlagsNotDetermined = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (SetTokenIssuer)",

                    Authentication = LangHelper.GetString("TokenIssuer_Could_Not_Be_Determined")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

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
                    Issuer = LangHelper.GetString("TokenIssuer_Could_Not_Be_Determined");
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML token issuer could not be determined. {e}");

                Issuer = LangHelper.GetString("TokenIssuer_Could_Not_Be_Determined");
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SetTokenIssuer)",

                SamlTokenIssuer = Issuer
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetSigningCertificate(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;X509Certificate>", false) > 1) && !(this.session.utilFindInResponse("&lt;/X509Certificate>", false) > 1))
            {
                var sessionFlagsNotDetermined = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (SigningCertificate)",

                    SamlTokenSigningCertificate = LangHelper.GetString("SamlToken_SigningCertificate_Data_Points_Not_Found")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

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
                    x509SigningCertificate = LangHelper.GetString("SamlToken_SigningCertificate_Could_Not_Be_Determined");
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML signing certificate could not be determined. {e}");

                x509SigningCertificate = LangHelper.GetString("SamlToken_SigningCertificate_Could_Not_Be_Determined");
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SigningCertificate)",

                SamlTokenSigningCertificate = x509SigningCertificate
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetAttributeNameUPN(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:Attribute AttributeName=&quot;UPN", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;/saml:Attribute>", false) > 1))
            {

                var sessionFlagsNotDetermined = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (AttributeNameUPN)",

                    SamlTokenAttributeNameUPN = LangHelper.GetString("SamlToken_AttributeNameUPN_Data_Points_Not_Found")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

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
                        AttributeNameUPN = LangHelper.GetString("SamlToken_AttributeNameUPN_Could_Not_Be_Determined");
                    }
                }
                else
                {
                    AttributeNameUPN = LangHelper.GetString("SamlToken_AttributeNameUPN_Could_Not_Be_Determined");
                }

            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML AttributeNameUPN could not be determined. {e}");
                AttributeNameUPN = LangHelper.GetString("SamlToken_AttributeNameUPN_Could_Not_Be_Determined");
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (AttributeNameUPN)",
                SamlTokenAttributeNameUPN = AttributeNameUPN,
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetNameIdentifierFormat(Session session)
        {
            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
            {

                var sessionFlagsNotDetermined = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (NameIdentifierFormat)",
                    SamlTokenNameIdentifierFormat = LangHelper.GetString("SamlToken_NameIdentifierFormat_Data_Points_Not_Found")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

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
                    NameIdentifierFormat = LangHelper.GetString("SamlToken_NameIdentifierFormat_Data_Points_Not_Found");
                }
            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML NameIdentifierFormat could not be determined. {e}");
                NameIdentifierFormat = LangHelper.GetString("SamlToken_NameIdentifierFormat_Data_Points_Not_Found");
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (NameIdentifierFormat)",

                SamlTokenNameIdentifierFormat = NameIdentifierFormat,
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }

        private void SetAttributeNameImmutableID(Session session)
        {
            // AttributeNameImmutableID

            this.session = session;

            if (!(this.session.utilFindInResponse("&lt;saml:NameIdentifier Format", false) > 1) &&
                !(this.session.utilFindInResponse("&lt;saml:SubjectConfirmation>", false) > 1))
            {
                var sessionFlagsNotDetermined = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "Authentication (TokenAttributeNameImmutibleID)",

                    SamlTokenAttributeNameImmutibleID = LangHelper.GetString("SamlToken_TokenAttributeNameImmutibleID_Data_Points_Not_Found")
                };

                var sessionFlagsJsonNotDetermined = JsonConvert.SerializeObject(sessionFlagsNotDetermined);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJsonNotDetermined, false);

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
                        AttributeNameImmutibleID = LangHelper.GetString("SamlToken_AttributeNameImmutibleID_Could_Not_Be_Determined");
                    }
                    //AttributeNameImmutibleIDFirstLine = AttributeNameImmutibleID.Substring(0, SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleIDSecondLine = AttributeNameImmutibleID.Substring(SplitAttributeNameImmutibleIDStartIndex);
                    //AttributeNameImmutibleID = AttributeNameImmutibleIDFirstLine + Environment.NewLine + AttributeNameImmutibleIDSecondLine;
                }
                else
                {
                    AttributeNameImmutibleID = LangHelper.GetString("SamlToken_AttributeNameImmutibleID_Could_Not_Be_Determined");
                }
            }
            catch (Exception e)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"({this.GetType().Name}): {this.session.id} SAML AttributeNameImmutibleID could not be determined. {e}");
                AttributeNameImmutibleID = LangHelper.GetString("SamlToken_AttributeNameImmutibleID_Could_Not_Be_Determined");
            }

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (TokenAttributeNameImmutibleID)",

                SamlTokenAttributeNameImmutibleID = AttributeNameImmutibleID
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
        

        private void SAMLParserFieldsNoData(Session session)
        {
            this.session = session;

            var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
            {
                SectionTitle = "Authentication (SAMLParserFieldsNoData)",

                SamlTokenIssuer = LangHelper.GetString("SamlToken_No_Data"),
                SamlTokenAttributeNameUPN = LangHelper.GetString("SamlToken_No_Data"),
                SamlTokenNameIdentifierFormat = LangHelper.GetString("SamlToken_No_Data"),
                SamlTokenAttributeNameImmutibleID = LangHelper.GetString("SamlToken_No_Data"),
            };

            var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
            SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
        }
    }
}
