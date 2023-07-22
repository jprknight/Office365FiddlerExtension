using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Office365FiddlerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Office365FiddlerExtension.Services.VersionJsonService;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionClassificationService
    {
        internal Session session { get; set; }

        private static SessionClassificationService _instance;
        public static SessionClassificationService Instance => _instance ?? (_instance = new SessionClassificationService());

        public SessionClassificationFlags GetDeserializedSessionFlags(Session Session)
        {
            this.session = Session;

            try
            {
                return JsonConvert.DeserializeObject<SessionClassificationFlags>(GetSessionClassificationJsonData(this.session));
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing session flags.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        public void CreateSessionClassificationFiddlerSetting()
        {
            /*if (Preferences.SessionClassification != null)
            {
                return;
            }*/

            string AssemblyShippedJsonData = "ewogICJCcm9hZExvZ2ljQ2hlY2tzIjogewogICAgIkZpZGRsZXJVcGRhdGVTZXNzaW9ucyI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDM1MCwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDAKICAgIH0sCiAgICAiQ29ubmVjdFR1bm5lbFNlc3Npb25zMjAwIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJDb25uZWN0VHVubmVsU2Vzc2lvbnNEZWZhdWx0IjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJBcGFjaGVBdXRvZGlzY292ZXIiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDUwCiAgICB9LAogICAgIkxvb3BCYWNrVHVubmVsIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfQogIH0KfQ==";

            var base64EncodedBytes = Convert.FromBase64String(AssemblyShippedJsonData);
 
            Preferences.SessionClassification = Encoding.UTF8.GetString(base64EncodedBytes); ;

        }

        public string GetSessionClassificationJsonData(Session Session)
        {
            this.session = Session;



            return null;
            
        }

        public SessionClassificationFlags GetDeserializedSessionClassification()
        {
            var JsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            try
            {
                return JsonConvert.DeserializeObject<SessionClassificationFlags>(Preferences.SessionClassification, JsonSettings);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing extension version.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        public class SessionClassificationFlags
        {
            public string BroadLogicChecks { get; set; }

            public string FiddlerUpdateSessions { get; set; }
        }
    }
}
