using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.Services;
using System;
using System.Reflection;

namespace Office365FiddlerExtensionRuleset.Ruleset
{
    class HTTP_200_Unified_Groups_Settings
    {
        internal Session session { get; set; }

        private static HTTP_200_Unified_Groups_Settings _instance;

        public static HTTP_200_Unified_Groups_Settings Instance => _instance ?? (_instance = new HTTP_200_Unified_Groups_Settings());

        /// <summary>
        /// Exchange Online / Microsoft 365 Unified Groups Settings.
        /// </summary>
        /// <param name="session"></param>
        public void Run(Session session)
        {
            this.session = session;

            // If this session isn't for Microsoft 365 Unified Group Settings, return.
            if (!this.session.HostnameIs("outlook.office365.com") &&
                (!this.session.uriContains("ews/exchange.asmx") &&
                (!(this.session.utilFindInRequest("GetUnifiedGroupsSettings", false) > 1))))
            {
                return;
            }

            // User can create Office 365 gropus.
            if (this.session.utilFindInResponse("<GroupCreationEnabled>true</GroupCreationEnabled>", false) > 1)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 GetUnifiedGroupsSettings EWS call. User can create O365 Groups in Outlook.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Unified_Groups_Settings");
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
                    sessionSeverity = 30;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s",

                    SessionType = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_SessionType"),
                    ResponseCodeDescription = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_ResponseCodeDescription"),
                    ResponseAlert = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_ResponseAlert"),
                    ResponseComments = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_ResponseComments"),

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
            // User cannot create Office 365 groups. Not an error condition in and of itself.
            else if (this.session.utilFindInResponse("<GroupCreationEnabled>false</GroupCreationEnabled>", false) > 1)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 GetUnifiedGroupsSettings EWS call. User cannot create O365 Groups in Outlook.");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Unified_Groups_Settings_User_Cannot_Create_Groups");
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
                    SectionTitle = "HTTP_200s",

                    SessionType = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_User_Cannot_Create_Groups_SessionType"),
                    ResponseCodeDescription = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_User_Cannot_Create_Groups_ResponseCodeDescription"),
                    ResponseAlert = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_User_Cannot_Create_Groups_ResponseAlert"),
                    ResponseComments = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_User_Cannot_Create_Groups_ResponseComments"),

                    SessionAuthenticationConfidenceLevel = sessionAuthenticationConfidenceLevel,
                    SessionTypeConfidenceLevel = sessionTypeConfidenceLevel,
                    SessionResponseServerConfidenceLevel = sessionResponseServerConfidenceLevel,
                    SessionSeverity = sessionSeverity
                };

                var sessionFlagsJson = JsonConvert.SerializeObject(sessionFlags);
                SessionFlagService.Instance.UpdateSessionFlagJson(this.session, sessionFlagsJson, false);
            }
            // Did not see the expected keyword in the response body. This is the error condition.
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} HTTP 200 GetUnifiedGroupsSettings!");

                int sessionAuthenticationConfidenceLevel;
                int sessionTypeConfidenceLevel;
                int sessionResponseServerConfidenceLevel;
                int sessionSeverity;

                try
                {
                    var sessionClassificationJson = SessionClassificationService.Instance.GetSessionClassificationJsonSection("HTTP_200s|HTTP_200_Unified_Groups_Settings_Settings_Not_Found");
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
                    sessionSeverity = 40;
                }

                var sessionFlags = new SessionFlagService.ExtensionSessionFlags()
                {
                    SectionTitle = "HTTP_200s",

                    SessionType = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_Settings_Not_Found_SessionType"),
                    ResponseCodeDescription = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_Settings_Not_Found_ResponseCodeDescription"),
                    ResponseAlert = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_Settings_Not_Found_ResponseAlert"),
                    ResponseComments = LangHelper.GetString("HTTP_200_Unified_Groups_Settings_Settings_Not_Found_ResponseComments"),

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
}
