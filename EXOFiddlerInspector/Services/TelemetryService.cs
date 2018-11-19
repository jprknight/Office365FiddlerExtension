using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using System.Management;

namespace EXOFiddlerInspector.Services
{
    public partial class TelemetryService
    {
        /// <summary>
        /// Instrumentation Key used to communicate with Azure Application Insights.
        /// </summary>
        private static readonly string iKey = "f236d9b9-fc00-461d-a9c6-64f231416908";

        /// <summary>
        /// Azure Application Insights Telemetry client.
        /// </summary>
        private static TelemetryClient Client { get; set; }

        /// <summary>
        /// Property to hold a static reference of the unique user ID.
        /// </summary>
        private static string UUID { get; set; }

        /// <summary>
        /// Property to track whether or not the client has been initialized.
        /// </summary>
        public static bool IsInitialized { get; set; } = false;

        /// <summary>
        /// Initialize the Azure Application Insights Telemetry Client.
        /// </summary>
        /// <returns>Bool</returns>
        public static async Task<bool> InitializeAsync()
        {
            bool results = false;
            if (!IsInitialized)
            {               
                try
                {
                    Client = new TelemetryClient();
                    Client.InstrumentationKey = iKey;
                    UUID = await Task.Run(() => GetComputerUUID());
                    IsInitialized = true;
                    Client.TrackEvent("UserSession");
                    results = true;
                }
                catch
                {
                    results = false;
                }              
            }
            return results;
        }

        /// <summary>
        /// Method to call and track events from other portions of the application.
        /// </summary>
        /// <param name="EventName">Name of the event you want to track. This is required.</param>
        /// <param name="UserId">Custom user name, else a unique guid is generated. This is not required.</param>
        /// <returns>Task Completion Event.</returns>
        public static async Task TrackEvent(string EventName, string UserId = null)
        {
            try
            {
                Client.Context.User.Id = UUID;
                Client.Context.Session.Id = Guid.NewGuid().ToString();
                Client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
                Client.TrackEvent(EventName);

                // Force client to push data to azure.
                Client.Flush();

                // Allow time for flushing:
                await Task.Delay(1500);
            }
            catch
            {
                // TODO add exception logic.
            }
            return;
        }


        /// <summary>
        /// Generate unique ID based on the motherboards serial number.
        /// </summary>
        /// <returns>String</returns>
        private static string GetComputerUUID()
        {
           
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection _managementObjects = query.Get();

            foreach (ManagementObject mgmtObject in _managementObjects)
            {
                UUID = (string)mgmtObject["SerialNumber"];
            }
            return UUID;
        }

    }
}
