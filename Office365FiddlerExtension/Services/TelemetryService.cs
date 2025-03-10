using Fiddler;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Office365FiddlerExtension.Services
{
    /// <summary>
    /// Class to initialize and run telemetry.
    /// </summary>

    public partial class TelemetryService
    {

        private static TelemetryService _instance;
        public static TelemetryService Instance => _instance ?? (_instance = new TelemetryService());

        /// <summary>
        /// Instrumentation Key used to communicate with Azure Application Insights.
        /// </summary>

        // Pull telemetry instrumentation key from Json.
        private static readonly string iKey = URLsJsonService.Instance.GetDeserializedExtensionURLs().TelemetryInstrumentationKey;

        /// <summary>
        /// Azure Application Insights Telemetry client.
        /// </summary>
        private static TelemetryClient Client { get; set; }

        /// <summary>
        /// Property to hold a static reference of the unique user ID.
        /// </summary>
        //private static string UUID { get; set; }

        private static string SessionID = Guid.NewGuid().ToString();

        private static string OS = Environment.OSVersion.ToString();

        /// <summary>
        /// Property to track whether or not the client has been initialized.
        /// </summary>
        public static bool IsInitialized { get; set; } = false;

        private static int ExceptionCounter = 0;
        /// <summary>
        /// Initialize the Azure Application Insights Telemetry Client.
        /// </summary>
        /// <returns>Bool</returns>
        public static async Task InitializeAsync()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (TelemetryService): NeverWebCall enabled, returning.");
                return;
            }

            var ExtensionSettings = SettingsJsonService.Instance.GetDeserializedExtensionSettings();

            if (!ExtensionSettings.ExtensionSessionProcessingEnabled)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (TelemetryService): Extension not enabled, exiting.");
                return;
            }

            if (!IsInitialized)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (TelemetryService): Using instrumentation key: {URLsJsonService.Instance.GetDeserializedExtensionURLs().TelemetryInstrumentationKey}");

                try
                {
                    ExceptionCounter = 0;

                    CustomTrackEvent("FiddlerApplicationStart");

                    IsInitialized = true;
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (TelemetryService): {ex}");
                }
            }
        }

        public async static void CustomTrackException(Exception _ex)
        {
            try
            {
                Client = new TelemetryClient
                {
                    InstrumentationKey = iKey
                };

                //UUID = await GetComputerUUID();

                //Client.Context.User.Id = UUID;

                Client.Context.Session.Id = SessionID;

                Client.Context.Device.OperatingSystem = OS;

                Client.Context.Component.Version = $"{VersionJsonService.Instance.GetExtensionVersion()} / " +
                    $"{VersionJsonService.Instance.GetRulesetVersion()}";

                Client.TrackException(_ex);

                //await FlushClientAsync();
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (TelemetryService): {ex}");
            }
        }

        public async static void CustomTrackMetric(string _string, long _value)
        {
            try
            {
                Client = new TelemetryClient
                {
                    InstrumentationKey = iKey
                };

                //UUID = await GetComputerUUID();

                //Client.Context.User.Id = UUID;

                Client.Context.Session.Id = SessionID;

                Client.Context.Device.OperatingSystem = OS;

                Client.Context.Component.Version = $"{VersionJsonService.Instance.GetExtensionVersion()} / " +
                    $"{VersionJsonService.Instance.GetRulesetVersion()}";

                Client.TrackMetric(_string, _value);

                //await FlushClientAsync();
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (TelemetryService): {ex}");
            }
        }

        public async static void CustomTrackEvent(string _value)
        {
            try
            {
                Client = new TelemetryClient
                {
                    InstrumentationKey = iKey
                };

                //UUID = await GetComputerUUID();

                //Client.Context.User.Id = UUID;

                Client.Context.Session.Id = SessionID;

                Client.Context.Device.OperatingSystem = OS;

                Client.Context.Component.Version = $"{VersionJsonService.Instance.GetExtensionVersion()} / " +
                    $"{VersionJsonService.Instance.GetRulesetVersion()}";

                Client.TrackEvent(_value);

                // Leaving this commented. It is run multiple times, so is detrimental to performance.
                // await FlushClientAsync();
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (TelemetryService): {ex}");
                ExceptionCounter++;
            }            
        }

        /// <summary>
        /// Method to call and track events from other portions of the application.
        /// </summary>
        /// <param name="EventName">Name of the event you want to track. This is required.</param>
        /// <returns>Task Completion Event.</returns>
        public async static void dfsTrackEvent(string _value)
        {
            try
            {
                if (await GetClientStatus())
                {
                    Client.TrackEvent(_value);

                    await FlushClientAsync();
                }
            }
            catch
            {
                // swallow exception and if 5 exceptions, disable telemetry.
                ExceptionCounter++;
            }
        }
        

        public async static Task FlushClientAsync()
        {
            try
            {
                Client.Flush();

                // Allow time for flushing:
                await Task.Delay(1500);
            }
            catch
            {
                // swallow exception and if 5 exceptions, disable telemetry.
                ExceptionCounter++;
            }
        }

        private async static Task<bool> GetClientStatus()
        {
            //bool results = false;
            bool results;
            if (ExceptionCounter <= 4)
            {
                results = true;
            }
            else
            {
                results = false;
            }

            return await Task.FromResult(results);
        }
        /// <summary>
        /// Generate unique ID based on the motherboards serial number and or UserName + MachineName.
        /// </summary>
        /// <returns>String</returns>
        private static Task<string> GetComputerUUID()
        {
            string userName = null;
            try
            {
                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");

                ManagementObjectCollection _managementObjects = query.Get();

                // foreach (ManagementObject mgmtObject in _managementObjects)
                foreach (ManagementObject mgmtObject in _managementObjects.Cast<ManagementObject>())
                {
                    userName = CreateMD5((string)mgmtObject["SerialNumber"]);
                }
            }
            catch
            {
                userName = CreateMD5(Environment.MachineName);
                //userName = CreateMD5(Environment.UserName + Environment.MachineName);
                // TODO add exception logic.
            }
            return Task.FromResult(userName);
        }

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private static string AppVersion
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fileVersionInfo.FileVersion;
            }
        }
    }
}
