using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Microsoft.AspNetCore.Http.Authentication;
using Newtonsoft.Json;
using Office365FiddlerInspector.Ruleset;
using static Office365FiddlerInspector.Ruleset.BroadLogicChecks;

namespace Office365FiddlerInspector.Services
{
    // The name SessionFlags is already taken by Fiddler, changed to resolve abiquity.
    class GetSetSessionFlags : ActivationService
    {

        private static GetSetSessionFlags _instance;
        public static GetSetSessionFlags Instance => _instance ?? (_instance = new GetSetSessionFlags());

        public void WriteToFiddlerLog(Session session, String Log)
        {
            this.session = session;
            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {this.session.id} {Log}");
        }

        public void WriteToFiddlerLogNoSession(String Log)
        {
            FiddlerApplication.Log.LogString($"Office365FiddlerExtension: {Log}");
        }

        // Colour codes for sessions. Softer tones, easier on the eye than standard red, orange and green.
        public void SetUIBackColour(Session session, String Colour)
        {
            this.session = session;

            switch (Colour.ToLower()) {
                case "blue":
                    this.session["ui-backcolor"] = "#81BEF7";
                    break;
                case "green":
                    this.session["ui-backcolor"] = "#81F7BA";
                    break;
                case "red":
                    this.session["ui-backcolor"] = "#F06141";
                    break;
                case "gray":
                    this.session["ui-backcolor"] = "#BDBDBD";
                    break;
                case "orange":
                    this.session["ui-backcolor"] = "#F59758";
                    break;
                case "black":
                    this.session["ui-backcolor"] = "#000000";
                    break;
                default:
                    // Default to pink, so we know if something isn't caught.
                    this.session["ui-backcolor"] = "#FFC0CB";
                    break;
            }
        }

        public void SetUITextColour(Session session, String Colour)
        {
            this.session = session;

            switch (Colour.ToLower())
            {
                case "black":
                    this.session["ui-color"] = "#000000";
                    break;
                case "red":
                    this.session["ui-color"] = "#F06141";
                    break;
                default:
                    this.session["ui-color"] = "#000000";
                    break;
            }
        }

        // SESSION CLASSIFICATIONS

        // How are session classifications used?
        // None; -5 : Session classification isn't set.
        // Low;  0  : Session classification has low confidence, any and all subsequent functions should be run to further attempt to classify the session.
        // Mid;  5  : Session classification has some confidence, but overriding functions should be run just in case.
        // High; 10 : Session classification has high level of confidence and any overriding functions should not be run.

        public Boolean GetAnySessionConfidenceLevelTen(Session session)
        {
            this.session = session;
            if (GetSessionAuthenticationConfidenceLevel(this.session) == 10 ||
                GetSessionResponseServerConfidenceLevel(this.session) == 10 ||
                GetSessionTypeConfidenceLevel(this.session) == 10)
            {
                return true;
            }
            return false;
        }

        public int GetSessionAuthenticationConfidenceLevel(Session session)
        {
            this.session = session;

            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-SACL"] == null || this.session["X-SACL"] == "")
            {
                this.session["X-SACL"] = "-5";
            }

            return int.Parse(session["X-SACL"]);
        }

        // Set Session Authentication Confidence Level.
        public void SetSessionAuthenticationConfidenceLevel(Session session, string SessionAuthenticationConfidenceLevel)
        {
            this.session = session;
            this.session["X-SACL"] = SessionAuthenticationConfidenceLevel;
        }

        // Get Session Type Confidence Level.
        public int GetSessionTypeConfidenceLevel(Session session)
        {
            this.session = session;
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-STCL"] == null || this.session["X-STCL"] == "")
            {
                this.session["X-STCL"] = "-5";
            }
            return int.Parse(session["X-STCL"]);
        }

        // Set Session Type Confidence Level.
        public void SetSessionTypeConfidenceLevel(Session session, string SessionTypeConfidenceLevel)
        {
            this.session = session;
            this.session["X-STCL"] = SessionTypeConfidenceLevel;
        }

        // Get Session Response Server Confidence Level.
        public int GetSessionResponseServerConfidenceLevel(Session session)
        {
            this.session = session;
            // Avoid null object exceptions by setting this session flag to something
            // rather than nothing.
            if (this.session["X-SRSCL"] == null || this.session["X-SRSCL"] == "")
            {
                this.session["X-SRSCL"] = "-5";
            }
            return int.Parse(this.session["X-SRSCL"]);
        }

        // Set Session Response Server Confidence Level.
        public void SetSessionResponseServerConfidenceLevel(Session session, string SessionResponseServerConfidenceLevel)
        {
            this.session = session;
            this.session["X-SRSCL"] = SessionResponseServerConfidenceLevel;
        }

        public String GetProcess(Session session)
        {
            this.session = session;
            return this.session["X-ProcessName"];
        }

        public void SetProcess(Session session)
        {
            this.session = session;
            // Set process name, split and exclude port used.
            if (session.LocalProcess != String.Empty)
            {
                string[] ProcessName = session.LocalProcess.Split(':');
                session["X-ProcessName"] = ProcessName[0];
            }
            // No local process to split.
            else
            {
                session["X-ProcessName"] = "Remote Capture";
            }
        }

        public String GetResponseCodeDescription(Session session)
        {
            this.session = session;
            return this.session["X-ResponseCodeDescription"];
        }

        public void SetResponseCodeDescription(Session session, String ResponseCodeDescription)
        {
            this.session = session;
            this.session["X-ResponseCodeDescription"] = ResponseCodeDescription;
        }

        public String GetSessionType(Session session)
        {
            this.session = session;
            return this.session["X-Session-Type"];
        }

        public void SetSessionType(Session session, String SessionType)
        {
            this.session = session;
            this.session["X-SessionType"] = SessionType;
        }

        public string GetXAuthentication(Session session)
        {
            this.session = session;
            return this.session["X-Authentication"];
        }

        public void SetXAuthentication(Session session, String Authentication)
        {
            this.session = session;
            this.session["X-Authentication"] = Authentication;
        }

        public string GetXAuthenticationDescription(Session session)
        {
            this.session = session;
            return this.session["X-AuthenticationDesc"];
        }

        public void SetXAuthenticationDescription(Session session, String AuthenticationDescription)
        {
            this.session = session;
            this.session["X-AuthenticationDesc"] = AuthenticationDescription;
        }

        public string GetXOffice365AuthType(Session session)
        {
            this.session = session;
            return this.session["X-Office365AuthType"];
        }

        public void SetXOffice365AuthType(Session session, String AuthType)
        {
            this.session = session;
            this.session["X-Office365AuthType"] = AuthType;
        }
        
        public string GetSamlTokenIssuer(Session session)
        {
            this.session = session;
            return this.session["X-Issuer"];
        }

        public void SetSamlTokenIssuer(Session session, String SAMLTokenIssuer)
        {
            this.session = session;
            this.session["X-Issuer"] = SAMLTokenIssuer;
        }

        public string GetSamlTokenSigningCertificate(Session session)
        {
            this.session = session;
            return this.session["X-SigningCertificate"];
        }

        public void SetSamlTokenSigningCertificate(Session session, String  SamlTokenSigningCertificate)
        {
            this.session = session;
            this.session["X-SigningCertificate"] = SamlTokenSigningCertificate;
        }

        public string GetSamlTokenAttributeNameUPN(Session session)
        {
            this.session = session;
            return this.session["X-AttributeNameUPN"];
        }

        public void SetSamlTokenAttributeNameUPN(Session session, String SamlTokenAttributeNameUPN)
        {
            this.session = session;
            this.session["X-AttributeNameUPN"] = SamlTokenAttributeNameUPN;
        }

        public string GetSamlTokenNameIdentifierFormat(Session session)
        {
            this.session = session;
            return this.session["X-NameIdentifierFormat"];
        }

        public void SetSamlTokenNameIdentifierFormat(Session session, String NameIdentifierFormat)
        {
            this.session = session;
            this.session["X-NameIdentifierFormat"] = NameIdentifierFormat;
        }

        public string GetSamlTokenAttributeNameImmutibleID(Session session)
        {
            this.session = session;
            return this.session["X-AttributeNameImmutableID"];
        }

        public void SetSamlTokenAttributeNameImmutibleID(Session session, String AttributeNameImmutibleID)
        {
            this.session = session;
            this.session["X-AttributeNameImmutableID"] = AttributeNameImmutibleID;
        }

        public String GetXResponseServer(Session session)
        {
            this.session = session;
            return this.session["X-ResponseServer"];
        }

        public void SetXResponseServer(Session session, String ResponseServer)
        {
            this.session = session;
            this.session["X-ResponseServer"] = ResponseServer;
        }
        
        public void SetXResponseAlert(Session session, String ResponseAlert)
        {
            this.session = session;
            this.session["X-ResponseAlert"] = ResponseAlert;
        }

        public string GetXResponseComments (Session session)
        {
            this.session = session;
            return this.session["X-ResponseComments"];
        }

        public void SetXResponseComments(Session session, String ResponseComments)
        {
            this.session = session;
            this.session["X-ResponseComments"] = ResponseComments;
        }

        public void SetXResponseCommentsNoKnownIssue(Session session)
        {
            this.session = session;
            this.session["X-ResponseComments"] = "<p>No known issue with Office 365 and this type of session. If you have a suggestion for an improvement, "
                + "create an issue or better yet a pull request in the project Github repository: "
                + "<a href='https://aka.ms/Office365FiddlerExtension' target='_blank'>https://aka.ms/Office365FiddlerExtension</a>.</p>";
        }

        public String GetXDataCollected(Session session)
        {
            this.session = session;
            return this.session["X-DataCollected"];
        }

        public void SetXDateDataCollected(Session session, String DateDataCollected)
        {
            this.session = session;
            this.session["X-DataCollected"] = DateDataCollected;
        }

        public String GetXDataAge(Session session)
        {
            this.session = session;
            return this.session["X-DataAge"];
        }

        public void SetXDataAge(Session session, String DataAge)
        {
            this.session = session;
            this.session["X-DataAge"] = DataAge;
        }

        public string GetXCalculatedSessionAge(Session session)
        {
            this.session = session;
            return this.session["X-CalculatedSessionAge"];
        }

        public void SetXCalculatedSessionAge(Session session, String CalculatedSessionAge)
        {
            this.session = session;
            this.session["X-CalculatedSessionAge"] = CalculatedSessionAge;
        }

        public string GetXSessionTimersDescription(Session session)
        {
            this.session = session;
            return this.session["X-SessionTimersDescription"];
        }

        public void SetXSessionTimersDescription(Session session, String SessionTimersDescription)
        {
            this.session = session;
            this.session["X-SessionTimersDescription"] = SessionTimersDescription;
        }

        public string GetXServerThinkTime(Session session)
        {
            this.session = session;
            return this.session["X-ServerThinkTime"];
        }

        public void SetXServerThinkTime(Session session, String ServerThinkTime)
        {
            this.session = session;
            this.session["X-ServerThinkTime"] = ServerThinkTime;
        }

        public string GetXTransitTime(Session session)
        {
            this.session = session;
            return this.session["X-TransitTime"];
        }

        public void SetXTransitTime(Session session, String TransitTime)
        {
            this.session = session;
            this.session["X-TransitTime"] = TransitTime;
        }

        public void SetXElapsedTime(Session session, String ElapsedTime)
        {
            this.session = session;
            this.session["X-ElapsedTime"] = ElapsedTime;
        }

        public String GetXInspectorElapsedTime(Session session)
        {
            this.session = session;
            return this.session["X-InspectorElapsedTime"];
        }

        public void SetXInspectorElapsedTime(Session session, String InspectorElapsedTime)
        {
            this.session = session;
            this.session["X-InspectorElapsedTime"] = InspectorElapsedTime;
        }
    }
}