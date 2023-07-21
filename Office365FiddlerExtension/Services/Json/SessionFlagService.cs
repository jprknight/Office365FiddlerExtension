using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;
using Newtonsoft.Json;
using Office365FiddlerExtension.UI;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Function to stamp all session flags the extension uses.
    /// </summary>
    public class SessionFlagService
    {
        internal Session session { get; set; }

        private static SessionFlagService _instance;
        public static SessionFlagService Instance => _instance ?? (_instance = new SessionFlagService());

        /// <summary>
        /// Return deserialised Json session flags stored in each session in the Fiddler UI.
        /// </summary>
        /// <param name="Session"></param>
        /// <returns></returns>
        public ExtensionSessionFlags GetDeserializedSessionFlags(Session Session)
        {
            this.session = Session;

            try
            {
                return JsonConvert.DeserializeObject<SessionFlagService.ExtensionSessionFlags>(SessionFlagService.Instance.GetSessionJsonData(this.session));
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing session flags.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        /// <summary>
        /// Return the raw Json data from the session flag.
        /// </summary>
        /// <param name="Session"></param>
        /// <returns></returns>
        public string GetSessionJsonData(Session Session)
        {
            this.session = Session;

            // Make sure the extension session flag is created if it doesn't exist.
            CreateExtensionSessionFlag(this.session);

            return this.session["Microsoft365FiddlerExtensionJson"];
        }

        /// <summary>
        /// Creates the session flags on each session. Avoid null exceptions.
        /// </summary>
        /// <param name="Session"></param>
        public void CreateExtensionSessionFlag(Session Session)
        {
            this.session = Session;

            if (this.session["Microsoft365FiddlerExtensionJson"] != null)
            {
                return;
            }

            //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.Session.id} Json extension session flag not found. Creating.");

            var SessionFlagsData = new
            {
                SectionTitle = "",
                UIBackColour = "",
                UITextColour = "",
                SessionType = "",
                ResponseCodeDescription = "",
                ResponseServer = "",
                ResponseAlert = "",
                ResponseComments = "",
                DataAge = "",
                CalculatedSessionAge = "",
                DateDataCollected = "",
                SessionTimersDescription = "",
                ServerThinkTime = "",
                TransitTime = "",
                ElapsedTime = "",
                InspectorElapsedTime = "",
                Authentication = "",
                AuthenticationType = "",
                AuthenticationDescription = "",
                SamlTokenIssuer = "",
                SamlTokenSigningCertificate = "",
                SamlTokenAttributeNameUPN = "",
                SamlTokenNameIdentifierFormat = "",
                SamlTokenAttributeNameImmutibleID = "",
                ProcessName = "",
                SessionAuthenticationConfidenceLevel = "0",
                SessionTypeConfidenceLevel = "0",
                SessionResponseServerConfidenceLevel = "0"
            };

            // Transform the object to a Json object.
            string jsonData = JsonConvert.SerializeObject(SessionFlagsData, Formatting.Indented);

            // Save the new Json to the session flag.
            this.session["Microsoft365FiddlerExtensionJson"] = jsonData;
        }

        /// <summary>
        /// Processes the selected sessions in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void ProcessSelectedSessions()
        {
            var Sessions = FiddlerApplication.UI.GetSelectedSessions();

            foreach (var Session in Sessions)
            {
                this.session = Session;
                SessionService.Instance.OnPeekAtResponseHeaders(this.session);
            }
        }

        /// <summary>
        /// Processes all sessions loaded in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void ProcessAllSessions()
        {
            var Sessions = FiddlerApplication.UI.GetAllSessions();
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Processing all {Sessions.Count()} current sessions.");

            foreach (var Session in Sessions)
            {
                this.session = Session;
                SessionService.Instance.OnPeekAtResponseHeaders(this.session);
            }

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Processed {Sessions.Count()} sessions.");
        }

        /// <summary>
        /// Clears processing from all sessions loaded in Fiddler. Called from the MenuUI and ContextMenuUI.
        /// </summary>
        public void ClearAllSessionProcessing()
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Clearing all current session procesing.");

            var Sessions = FiddlerApplication.UI.GetAllSessions();
            foreach (var Session in Sessions)
            {
                this.session = Session;

                EnhanceSessionUX.Instance.NormaliseSession(this.session);

                this.session.RefreshUI();
            }
        }

        public class ExtensionSessionFlags
        {
            public string SectionTitle { get; set; }

            public string UIBackColour { get; set; }

            public string UITextColour { get; set; }

            public string SessionType { get; set; }

            public string ResponseCodeDescription { get; set; }

            public string ResponseServer { get; set; }

            public string ResponseAlert { get; set; }

            public string ResponseComments { get; set; }

            public string DataAge { get; set; }

            public string CalculatedSessionAge { get; set; }

            public string DateDataCollected { get; set; }

            public string SessionTimersDescription { get; set; }

            public string ServerThinkTime { get; set; }

            public string TransitTime { get; set; }

            public string ElapsedTime { get; set; }

            public string InspectorElapsedTime { get; set; }

            public string Authentication { get; set; }

            public string AuthenticationType { get; set; }

            public string AuthenticationDescription { get; set; }

            public string SamlTokenIssuer { get; set; }

            public string SamlTokenSigningCertificate { get; set; }

            public string SamlTokenAttributeNameUPN { get; set; }

            public string SamlTokenNameIdentifierFormat { get; set; }

            public string SamlTokenAttributeNameImmutibleID { get; set; }

            public string ProcessName { get; set; }

            public int SessionAuthenticationConfidenceLevel { get; set; }

            public int SessionTypeConfidenceLevel { get; set; }

            public int SessionResponseServerConfidenceLevel { get; set; }
        }
    }
}
