using Fiddler;
using Fiddler.WebFormats;
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

        /// <summary>
        /// Expecting a "Section|Section" to be passed into function.
        /// Function allows multiple depths to be passed in. Expecting 2 or 3 is the most likely use case.
        /// </summary>
        /// <param name="section"></param>
        public SessionClassificationJsonSection GetSessionClassificationJsonSection(string section)
        {
            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): GetSessionClassificationJsonSection ENTRY");

            string sectionPiece0 = "";
            string sectionPiece1 = "";
            //string sectionPiece2 = "";

            var jsonSection = "";

            var parsedObject = JObject.Parse(Preferences.SessionClassification);

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): GetSessionClassificationJsonSection PARSEDOBJECT");

            if (section.Contains('|')) {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): GetSessionClassificationJsonSection PIPE: {section}");

                string[] sectionPieces = section.Split('|');

                //if (sectionPieces.Length == 2)
                //{
                    foreach (string piece in sectionPieces)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {piece}");
                    }

                    sectionPiece0 = sectionPieces[0];
                    sectionPiece1 = sectionPieces[1];
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {sectionPieces.Length} " +
                        $"sectionPiece0: {sectionPiece0} sectionPiece1: {sectionPiece1} : {parsedObject[sectionPiece0][sectionPiece1].ToString()}");
                    jsonSection = parsedObject[sectionPiece0][sectionPiece1].ToString();
                //}
                /*else if (sectionPieces.Length == 3)
                {
                    foreach (string piece in sectionPieces)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {piece}");
                    }

                    sectionPiece0 = sectionPieces[0];
                    sectionPiece1 = sectionPieces[1];
                    sectionPiece2 = sectionPieces[2];
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {sectionPieces.Length} " +
                        $"sectionPiece0: {sectionPiece0} sectionPiece1: {sectionPiece1} sectionPiece2: {sectionPiece2} : {parsedObject[sectionPiece0][sectionPiece1][sectionPiece2].ToString()}");
                    jsonSection = parsedObject[sectionPiece0][sectionPiece1][sectionPiece2].ToString();
                }*/
            }
            else
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): GetSessionClassificationJsonSection NO PIPE");

                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    $"section: {section}: {parsedObject[section]}");
                jsonSection = parsedObject[section].ToString();
            }

            return JsonConvert.DeserializeObject<SessionClassificationJsonSection>(jsonSection);
        }

        /*public SessionClassificationFlags GetDeserializedSessionFlags(Session Session)
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
        }*/

        public void CreateSessionClassificationFiddlerSetting()
        {
            // REVIEW THIS
            if (Preferences.SessionClassification != null)
            {
                return;
            }

            string AssemblyShippedJsonData = "ewogICJEYXRhU3RydWN0dXJlSGVscGVyIjogewogICAgIkFib3V0IjogIlRoaXMgaGVscGVyIGlzbid0IHVzZWQgaW4gY29kZSwgaXQncyBqdXN0IGhlcmUgdG8gZGVzY3JpYmUgdGhlIGRhdGEuIiwKICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiAiUnVucyBmcm9tIDAgLSAxMCwgMTAgYmVpbmcgYWJzb2x1dGVseSBjZXJ0YWluIHRoZSBjb3JyZWN0IGF1dGhlbnRpY2F0aW9uIGhhcyBiZWVuIGRldGVybWluZWQgZm9yIHRoZSBzZXNzaW9uLiIsCiAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAiUnVucyBmcm9tIDAgLSAxMCwgMTAgYmVpbmcgYWJzb2x1dGVseSBjZXJ0YWluIHRoZSBjb3JyZWN0IHR5cGUgaGFzIGJlZW4gZGV0ZXJtaW5lZCBmb3IgdGhlIHNlc3Npb24uIiwKICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAiUnVucyBvbiBhIHNjYWxlIG9mIDAgLSA2MCBhbmQgZGV0ZXJtaW5lcyB0aGUgY29sb3VycyBvZiBzZXNzaW9ucyB1c2VkIGJ5IHRoZSBleHRlbnNpb24uIiwKICAgICJTZXNzaW9uQ29sb3VycyI6ICJBY2NvcmRpbmcgdG8gc2Vzc2lvbiBzZXZlcml0eSIsCiAgICAiLT0tPS09LT0tPS0iOiAiLT0tPS09LT0tPS0iLAogICAgIlNlc3Npb25TZXZlcml0eVRlbiI6ICIxMCBHcmF5IChVbmludGVyZXN0aW5nKSAtIEZvciBzZXNzaW9ucyB3aGljaCBkb24ndCBoYXZlIGFueSByZWxhdGlvbiB0byB0aGUgY3VycmVudCB0cmFjZSBiZWluZyBhbmFseXNlZCBmb3IgdGhlIGlzc3VlIGJlaW5nIHdvcmtlZCBvbi4iLAogICAgIlNlc3Npb25TZXZlcml0eVR3ZW50eSI6ICIyMCAtIEJsdWUgKEZhbHNlIFBvc2l0aXZlKSAtIEZvciBzZXNzaW9ucyB3aGVyZSBhbiBlcnJvciBjb3VsZCBiZSBmYWxzZWx5IGJsYW1lZCBmb3IgdGhlIGlzc3VlIGJlaW5nIGludmVzdGlnYXRlZC4iLAogICAgIlNlc3Npb25TZXZlcml0eVRoaXJ0eSI6ICIzMCAtIEdyZWVuIChOb3JtYWwpIC0gRm9yIHNlc3Npb25zIHdoZXJlIG5vIGVycm9yIGNvbmRpdGlvbiBpcyBmb3VuZC4iLAogICAgIlNlc3Npb25TZXZlcml0eUZvdXJ0eSI6ICI0MCAtIE9yYW5nZSAoV2FybmluZykgLSBGb3Igc2Vzc2lvbnMgd2hpY2ggY291bGQgcHJlc2VudCBhbiBpc3N1ZSwgYnV0IGFyZSBzdGlsIGV4cGVjdGVkIHRvIGJlIHNlZW4gaW4gbm9ybWFsIHdvcmtpbmcgc2NlbmFyaW9zLiBIVFRQIDQwMXMgYXV0aGVudGljYXRpb24gY2hhbGxlbmdlcyBhcmUgYSBnb29kIGV4YW1wbGUgb2YgdGhpcy4iLAogICAgIlNlc3Npb25TZXZlcml0eUZpZnR5IjogIjUwIC0gQmxhY2sgKENvbmNlcm5pbmcpIC0gRm9yIHNlc3Npb25zIHdoaWNoIGNvdWxkIGJlIGludGVycHJldHRlZCBhcyBhIGZhbHNlIG5lZ2F0aXZlLiBIVFRQIDIwMHMgY29udGFpbmluZyBhbiBlcnJvciBvciBmYWlsdXJlIGFyZSBhIGdvb2QgZXhhbXBsZSBvZiB0aGlzLiIsCiAgICAiU2Vzc2lvblNldmVyaXR5U2l4dHkiOiAiNjAgLSBSZWQgKFNldmVyZSkgLSBGb3Igc2Vzc2lvbnMgd2hlcmUgYSBrbm93biBlcnJvciBjb25kaXRpb24gaGFzIGJlZW4gZGV0ZWN0ZWQuIFVzdWFsbHkgY2FsbGluZyBvdXQgJ1N0YXJ0IGhlcmUgZmlyc3QnLiIKICB9LAogICJCcm9hZExvZ2ljQ2hlY2tzIjogewogICAgIkZpZGRsZXJVcGRhdGVTZXNzaW9ucyI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblNldmVyaXR5IjogMTAKICAgIH0sCiAgICAiQ29ubmVjdFR1bm5lbFNlc3Npb25zMjAwIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA0MAogICAgfSwKICAgICJDb25uZWN0VHVubmVsU2Vzc2lvbnNEZWZhdWx0IjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA0MAogICAgfSwKICAgICJBcGFjaGVBdXRvZGlzY292ZXIiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDYwCiAgICB9LAogICAgIkxvb3BCYWNrVHVubmVsIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA0MAogICAgfQogIH0sCiAgIkhUVFAwcyI6IHsKICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAiU2Vzc2lvblNldmVyaXR5IjogNjAKICB9LAogICJIVFRQMTAzcyI6IHsKICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAxMAogIH0sCiAgIkhUVFAyMDBzIjogewogICAgIkhUVFBfMjAwX0NsaWVudEFjY2Vzc1J1bGUiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA2MAogICAgfSwKICAgICJIVFRQXzIwMF9PdXRsb29rX01hcGlfTWljcm9zb2Z0MzY1X1Byb3RvY29sX0Rpc2FibGVkIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblNldmVyaXR5IjogNjAKICAgIH0sCiAgICAiSFRUUF8yMDBfT3V0bG9va19FeGNoYW5nZV9PbmxpbmVfTWljcm9zb2Z0XzM2NV9NYXBpIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblNldmVyaXR5IjogMzAKICAgIH0sCiAgICAiSFRUUF8yMDBfT3V0bG9va19FeGNoYW5nZV9PblByZW1pc2VfTWFwaSI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDMwCiAgICB9LAogICAgIkhUVFBfMjAwX091dGxvb2tfV2ViX0FwcCI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDMwCiAgICB9LAogICAgIkhUVFBfMjAwX091dGxvb2tfUlBDIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJIVFRQXzIwMF9PdXRsb29rX05TUEkiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJIVFRQXzIwMF9PblByZW1pc2VfQXV0b0Rpc2NvdmVyX1JlZGlyZWN0X0FkZHJlc3NfRm91bmQiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJIVFRQXzIwMF9PblByZW1pc2VfQXV0b0Rpc2NvdmVyX0luY29ycmVjdFJlZGlyZWN0IjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblNldmVyaXR5IjogNjAKICAgIH0sCiAgICAiSFRUUF8yMDBfT25QcmVtaXNlX0F1dG9EaXNjb3Zlcl9SZWRpcmVjdF9BZGRyZXNzTm90Rm91bmQiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA2MAogICAgfSwKICAgICJIVFRQXzIwMF9FeGNoYW5nZV9PbmxpbmVfTWljcm9zb2Z0MzY1X0F1dG9EaXNjb3Zlcl9NU0lfTm9uX0NsaWNrVG9SdW4iOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJIVFRQXzIwMF9FeGNoYW5nZV9PbmxpbmVfTWljcm9zb2Z0MzY1X0F1dG9EaXNjb3Zlcl9NU0lfTm9uX0NsaWNrVG9SdW5fVW5leHBlY3RlZF9YTUxfUmVzcG9uc2UiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA2MAogICAgfSwKICAgICJIVFRQXzIwMF9FeGNoYW5nZV9PbmxpbmVfTWljcm9zb2Z0MzY1X0F1dG9EaXNjb3Zlcl9DbGlja1RvUnVuIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJIVFRQXzIwMF9FeGNoYW5nZV9PbmxpbmVfTWljcm9zb2Z0MzY1X0F1dG9EaXNjb3Zlcl9DbGlja1RvUnVuX1hNTF9SZXNwb25zZV9Ob3RfRm91bmQiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA2MAogICAgfSwKICAgICJIVFRQXzIwMF9VbmlmaWVkX0dyb3Vwc19TZXR0aW5ncyI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDMwCiAgICB9LAogICAgIkhUVFBfMjAwX1VuaWZpZWRfR3JvdXBzX1NldHRpbmdzX1VzZXJfQ2Fubm90X0NyZWF0ZV9Hcm91cHMiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA2MAogICAgfSwKICAgICJIVFRQXzIwMF9VbmlmaWVkX0dyb3Vwc19TZXR0aW5nc19TZXR0aW5nc19Ob3RfRm91bmQiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA0MAogICAgfSwKICAgICJIVFRQXzIwMF8zU19TdWdnZXN0aW9ucyI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblNldmVyaXR5IjogMzAKICAgIH0sCiAgICAiSFRUUF8yMDBfUkVTVF9QZW9wbGVfUmVxdWVzdCI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblNldmVyaXR5IjogMzAKICAgIH0sCiAgICAiSFRUUF8yMDBfTWljcm9zb2Z0MzY1X0FueV9PdGhlcl9FV1MiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDMwCiAgICB9LAogICAgIkhUVFBfMjAwX09uUHJlbWlzZV9BbnlfT3RoZXJfRVdTIjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJIVFRQXzIwMF9KYXZhc2NyaXB0IjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblNldmVyaXR5IjogNDAKICAgIH0sCiAgICAiSFRUUF8yMDBfTHVya2luZ19FcnJvcnMiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiA1MAogICAgfSwKICAgICJIVFRQXzIwMF9BY3R1YWxseV9PSyI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDMwCiAgICB9CiAgfSwKICAiSFRUUDIwMXMiOiB7CiAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAiU2Vzc2lvblNldmVyaXR5IjogMTAKICB9LAogICJIVFRQMjAycyI6IHsKICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAxMAogIH0sCiAgIkhUVFAyMDNzIjogewogICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgIlNlc3Npb25TZXZlcml0eSI6IDEwCiAgfSwKICAiSFRUUDIwNHMiOiB7CiAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAiU2Vzc2lvblNldmVyaXR5IjogMTAKICB9LAogICJIVFRQMjA1cyI6IHsKICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAxMAogIH0sCiAgIkhUVFAyMDZzIjogewogICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgIlNlc3Npb25TZXZlcml0eSI6IDEwCiAgfSwKICAiSFRUUDIwN3MiOiB7CiAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAiU2Vzc2lvblNldmVyaXR5IjogMTAKICB9LAogICJIVFRQMjA4cyI6IHsKICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAxMAogIH0sCiAgIkhUVFAyMThzIjogewogICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgIlNlc3Npb25TZXZlcml0eSI6IDEwCiAgfSwKICAiSFRUUDIyNnMiOiB7CiAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAiU2Vzc2lvblNldmVyaXR5IjogMTAKICB9LAogICJIVFRQMzAycyI6IHsKICAgICJIVFRQXzMwMl9SZWRpcmVjdF9BdXRvRGlzY292ZXIiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfSwKICAgICJIVFRQXzMwMl9SZWRpcmVjdF9BbGxPdGhlcnMiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAzMAogICAgfQogIH0sCiAgIkhUVFBfMzA3cyI6IHsKICAgICJIVFRQXzMwN19BdXRvRGlzY292ZXJfVGVtcG9yYXJ5X1JlZGlyZWN0IjogewogICAgICAiU2Vzc2lvbkF1dGhlbnRpY2F0aW9uQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25UeXBlQ29uZmlkZW5jZUxldmVsIjogMTAsCiAgICAgICJTZXNzaW9uUmVzcG9uc2VTZXJ2ZXJDb25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblNldmVyaXR5IjogNjAKICAgIH0sCiAgICAiSFRUUF8zMDdfT3RoZXJfQXV0b0Rpc2NvdmVyX1JlZGlyZWN0cyI6IHsKICAgICAgIlNlc3Npb25BdXRoZW50aWNhdGlvbkNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uVHlwZUNvbmZpZGVuY2VMZXZlbCI6IDEwLAogICAgICAiU2Vzc2lvblJlc3BvbnNlU2VydmVyQ29uZmlkZW5jZUxldmVsIjogNSwKICAgICAgIlNlc3Npb25TZXZlcml0eSI6IDQwCiAgICB9LAogICAgIkhUVFBfMzA3X0FsbF9PdGhlcl9SZWRpcmVjdHMiOiB7CiAgICAgICJTZXNzaW9uQXV0aGVudGljYXRpb25Db25maWRlbmNlTGV2ZWwiOiA1LAogICAgICAiU2Vzc2lvblR5cGVDb25maWRlbmNlTGV2ZWwiOiAxMCwKICAgICAgIlNlc3Npb25SZXNwb25zZVNlcnZlckNvbmZpZGVuY2VMZXZlbCI6IDUsCiAgICAgICJTZXNzaW9uU2V2ZXJpdHkiOiAxMAogICAgfQogIH0KfQo=";

            var base64EncodedBytes = Convert.FromBase64String(AssemblyShippedJsonData);
 
            Preferences.SessionClassification = Encoding.UTF8.GetString(base64EncodedBytes); ;

        }

        /*public string GetSessionClassificationJsonData(Session Session)
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
        }*/
    }

    public class SessionClassificationJsonSection
    {
        public int SessionAuthenticationConfidenceLevel { get; set; }

        public int SessionTypeConfidenceLevel { get; set; }

        public int SessionResponseServerConfidenceLevel { get; set; }

        public int SessionSeverity { get; set; }
    }
}
