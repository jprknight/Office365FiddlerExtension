using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace O365FiddlerInspector.Services
{
    public partial class TelemetryService
    {
        /// <summary>
        /// Instrumentation Key used to communicate with Azure Application Insights.
        /// </summary>
        // Old iKey.
        // private static readonly string iKey = "f236d9b9-fc00-461d-a9c6-64f231416908";
        private static readonly string iKey = "87fb55ab-0052-4970-9318-7c740220e3c0";
        
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

        private static int ExceptionCounter = 0;
        /// <summary>
        /// Initialize the Azure Application Insights Telemetry Client.
        /// </summary>
        /// <returns>Bool</returns>
        public static async Task InitializeAsync()
        {
            if (!IsInitialized)
            {
                try
                {
                    ExceptionCounter = 0;

                    Client = new TelemetryClient();

                    Client.InstrumentationKey = iKey;

                    UUID = await GetComputerUUID();

                    Client.Context.User.Id = UUID;

                    Client.Context.Session.Id = Guid.NewGuid().ToString();

                    Client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

                    Client.Context.Component.Version = Preferences.AppVersion;

                    TrackEvent("UserSession");

                    IsInitialized = true;
                }
                catch
                {
                    // TODO add exception logic
                }
            }
        }

        /// <summary>
        /// Method to call and track events from other portions of the application.
        /// </summary>
        /// <param name="EventName">Name of the event you want to track. This is required.</param>
        /// <returns>Task Completion Event.</returns>
        public async static void TrackEvent(string _value)
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
            bool results = false;
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

                foreach (ManagementObject mgmtObject in _managementObjects)
                {
                    userName = CreateMD5((string)mgmtObject["SerialNumber"]);
                }
            }
            catch
            {
                userName = CreateMD5(Environment.UserName + Environment.MachineName);
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

    }
}
