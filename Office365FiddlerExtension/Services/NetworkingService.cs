using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Fiddler;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace Office365FiddlerExtension.Services
{
    public class NetworkingService
    {
        private static NetworkingService _instance;
        private Session session;

        public static NetworkingService Instance => _instance ?? (_instance = new NetworkingService());

        // Need to call a function in this class to:
        //   Start out with a bool IsMicrosoft365IP = false.
        //   Iterate through all the children json objects in the json data stored in an application preference.
        //   For each interation / child, iterate through each each IP found in the child, and run the IsInSubnetMask(this.session.hostip, each.child.subnet)
        //     If IsInSubnetMask returns true, set IsMicrosoft365IP to true.


        /// <summary>
        /// Returns TRUE if the given IP address is contained in the given subnetmask, FALSE otherwise.
        /// Examples:
        /// - IsInSubnet("192.168.5.1", "192.168.5.85/24") -> TRUE
        /// - IsInSubnet("192.168.5.1", "192.168.5.85/32") -> FALSE
        /// ref.: https://stackoverflow.com/a/56461160
        /// </summary>
        /// <param name="address">The IP Address to check</param>
        /// <param name="subnetMask">The SubnetMask</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsInSubnetMask(string ipAddress, string subnetMask)
        {
            var address = IPAddress.Parse(ipAddress);
            var slashIdx = subnetMask.IndexOf("/");
            if (slashIdx == -1)
                // We only handle netmasks in format "IP/PrefixLength".
                throw new NotSupportedException("Only SubNetMasks with a given prefix length are supported.");

            // First parse the address of the netmask before the prefix length.
            var maskAddress = IPAddress.Parse(subnetMask.Substring(0, slashIdx));

            if (maskAddress.AddressFamily != address.AddressFamily)
                // We got something like an IPV4-Address for an IPv6-Mask. This is not valid.
                return false;

            // Now find out how long the prefix is.
            int maskLength = int.Parse(subnetMask.Substring(slashIdx + 1));

            if (maskLength == 0)
                return true;

            if (maskLength < 0)
                throw new NotSupportedException("A Subnetmask should not be less than 0.");

            if (maskAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                var maskAddressBits = BitConverter.ToUInt32(maskAddress.GetAddressBytes().Reverse().ToArray(), 0);
                var ipAddressBits = BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray(), 0);
                uint mask = uint.MaxValue << (32 - maskLength);

                // https://stackoverflow.com/a/1499284/3085985
                // Bitwise AND mask and MaskAddress, this should be the same as mask and IpAddress
                // as the end of the mask is 0000 which leads to both addresses to end with 0000
                // and to start with the prefix.
                return (maskAddressBits & mask) == (ipAddressBits & mask);
            }

            if (maskAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                // Convert the mask address to a BitArray. Reverse the BitArray to compare the bits of each byte in the right order.
                var maskAddressBits = new BitArray(maskAddress.GetAddressBytes().Reverse().ToArray());

                // And convert the IpAddress to a BitArray. Reverse the BitArray to compare the bits of each byte in the right order.
                var ipAddressBits = new BitArray(address.GetAddressBytes().Reverse().ToArray());
                var ipAddressLength = ipAddressBits.Length;

                if (maskAddressBits.Length != ipAddressBits.Length)
                    throw new ArgumentException("Length of IP Address and Subnet Mask do not match.");

                // Compare the prefix bits.
                for (var i = ipAddressLength - 1; i >= ipAddressLength - maskLength; i--)
                    if (ipAddressBits[i] != maskAddressBits[i])
                        return false;

                return true;
            }

            return false;
        }

        // https://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_Linq_JToken_Children.htm

        /*
        These two functions are consistent with what has been created in other classes, but it looks as though these aren't needed here.      

        public EndPointJson GetDeserializedEndpointJsonChild(string json)
        {
            try
            {
                //return JsonConvert.DeserializeObject<SessionFlagService.ExtensionSessionFlags>(SessionFlagService.Instance.GetSessionJsonData(this.session));
                return JsonConvert.DeserializeObject<NetworkingService.EndPointJson>(NetworkingService.Instance.GetEndpointJsonData(json));
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error deserializing session flags.");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {ex}");
            }
            return null;
        }

        public string GetEndpointJsonData(string json)
        {
            // Make sure the extension session flag is created if it doesn't exist.
            CreateExtensionSessionFlag(this.session);

            return this.session["Microsoft365FiddlerExtensionJson"];
        }

        */

        /// <summary>
        /// Function to read Microsoft365 URls and IPs json data, iterate through children, to 
        /// confirm IP address is or is not a Microsoft365 IP address.
        /// </summary>
        /// <param name="json"></param>
        /// <returns>this.session["X-HostIP"] null</returns>
        public bool IsMicrosoft365IPAddress(Session session)
        {
            this.session = session;

            bool isMicrosoft365IP = false;

            if (this.session["X-HostIP"] == null)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Session X-HostIP is null.");
                return false;
            }

            // Serialize the string to Json.

            if (Preferences.MicrosoftURLsIPsWebService.Length == 0)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} MicrosoftURLsIPsWebService is null.");
                return false;
            }
            
            // https://stackoverflow.com/questions/34690581/error-reading-jobject-from-jsonreader-current-jsonreader-item-is-not-an-object
            //Preferences.MicrosoftURLsIPsWebService = Preferences.MicrosoftURLsIPsWebService.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });

            //JObject jObject = JObject.Parse(Preferences.MicrosoftURLsIPsWebService);

            JArray jArray = JArray.Parse(Preferences.MicrosoftURLsIPsWebService);

            var children = jArray.Children();//.Children();

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): CHILDREN COUNT: {children.Count()}");

            //var valuesList = new List<string>();
            foreach (JObject child in children)
            {
                //valuesList.AddRange(child["values"].ToObject<List<string>>());

                var childJson = JsonConvert.DeserializeObject<NetworkingService.EndPointJson>(child.ToString());

                foreach (string subnet in childJson.ips)
                {
                    if (IsInSubnetMask(this.session["X-HostIP"], subnet))
                    {
                        isMicrosoft365IP = true;

                        string message = $"CHILDREN COUNT: {this.session["X-HostIP"]} {subnet} / {isMicrosoft365IP}";
                        string caption = "Error Detected in Input";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;

                        // Displays the MessageBox.
                        result = MessageBox.Show(message, caption, buttons);
                    }
                }
            }

            string message1 = $"FINISHED: {isMicrosoft365IP}";
            string caption1 = "Error Detected in Input";
            MessageBoxButtons buttons1 = MessageBoxButtons.YesNo;
            DialogResult result1;

            // Displays the MessageBox.
            result1 = MessageBox.Show(message1, caption1, buttons1);

            //JObject endpointdata = JObject.Parse(Preferences.MicrosoftURLsIPsWebService);

            //IList<JToken> list = endpointdata.Children();

            //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): CHILDREN COUNT: {Microsoft365URLsIPsJson.Count}");
            /*
            for (int i = 0; i < Microsoft365URLsIPsJson.Count; i++)
            {
                var jsonChild = JsonConvert.DeserializeObject<NetworkingService.EndPointJson>((string)list[i]);

                foreach (string subnet in jsonChild.ips)
                {
                    if (IsInSubnetMask(this.session["X-HostIP"], subnet))
                    {
                        isMicrosoft365IP = true;
                    }
                }
            }
            */
            return isMicrosoft365IP;
        }

        /// <summary>
        /// Update the Microsoft 365 URLs and IP addresses data from the web. Store it in an application preference for use in session analysis.
        /// Function intended to only be run once per Fiddler session to avoid any 429 "Too Many Requests" from the data source.
        /// </summary>
        public async Task UpdateMicrosft365URLsIPsFromWebAsync()
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} (NetworkingService): NeverWebCall enabled, returning.");
                return;
            }

            var extensionURLs = URLsJsonService.Instance.GetDeserializedExtensionURLs();

            FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                $"({this.GetType().Name}): Update attempt on Microsoft365 URLs and IPs at: {extensionURLs.MicrosoftURLsIPsWebService}");

            using (var getSettings = new HttpClient())
            {
                try
                {
                    var response = await getSettings.GetAsync(extensionURLs.MicrosoftURLsIPsWebService);

                    response.EnsureSuccessStatusCode();

                    var jsonString = string.Empty;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        stream.Position = 0;
                        using (var reader = new StreamReader(stream))
                        {
                            jsonString = await reader.ReadToEndAsync();
                        }
                    }

                    // Save this new data into the SessionClassification Fiddler setting.
                    if (Preferences.MicrosoftURLsIPsWebService != jsonString)
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): MicrosoftURLsIPsWebService Fiddler setting updated.");
                        Preferences.MicrosoftURLsIPsWebService = jsonString;
                    }
                    else
                    {
                        FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): MicrosoftURLsIPsWebService Fiddler setting no update needed.");
                    }
                }
                catch (Exception ex)
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): Error retrieving MicrosoftURLsIPsWebService from Github {ex}");
                }
            }
        }


        public class EndPointJson
        {
            public int id { get; set; }

            public string serviceArea { get; set; }

            public string serviceAreaDisplayName { get; set; }

            public ArrayList urls { get; set; }

            public ArrayList ips { get; set; }

            public string tcpPorts { get; set; }

            public string udpPorts { get; set; }

            public bool expressRoute { get; set; }

            public string category { get; set; }

            public bool required { get; set; }

            public string notes { get; set; }
        }
    }


}
