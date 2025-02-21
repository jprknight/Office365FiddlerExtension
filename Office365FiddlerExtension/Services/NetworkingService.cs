using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Fiddler;

namespace Office365FiddlerExtension.Services
{
    public class NetworkingService
    {
        private static NetworkingService _instance;
        private Session session;

        public static NetworkingService Instance => _instance ?? (_instance = new NetworkingService());

        /// <summary>
        /// Test if the provided IP address is a valid IP address.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>bool</returns>
        public bool IsValidIPAddress(string ipAddress)
        {
            try
            {
                var tryAddress = IPAddress.Parse(ipAddress);
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"(NetworkingService) IsInSubnetMask: Issue with IP address format: {ipAddress}");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
                return false;
            }
            return true;
        }

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
            try
            {
                var tryAddress = IPAddress.Parse(ipAddress);
                var trySlashIdx = subnetMask.IndexOf("/");
                if (trySlashIdx == -1)
                {
                    // We only handle netmasks in format "IP/PrefixLength".
                    //throw new NotSupportedException("Only SubNetMasks with a given prefix length are supported.");
                    //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    //    $"(NetworkingService) IsInSubnetMask: Only SubNetMasks with a given prefix length are supported.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    $"(NetworkingService) IsInSubnetMask: Issue with IP address format: {ipAddress}");
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} {ex}");
                return false;
            }

            if (ipAddress.Contains("Not Present"))
            {
                return false;
            }

            var address = IPAddress.Parse(ipAddress);
            var slashIdx = subnetMask.IndexOf("/");
            if (slashIdx == -1)
            {
                // We only handle netmasks in format "IP/PrefixLength".
                //throw new NotSupportedException("Only SubNetMasks with a given prefix length are supported.");
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                //    $"(NetworkingService) IsInSubnetMask: Only SubNetMasks with a given prefix length are supported.");
                return false;
            }

            // First parse the address of the netmask before the prefix length.
            var maskAddress = IPAddress.Parse(subnetMask.Substring(0, slashIdx));

            if (maskAddress.AddressFamily != address.AddressFamily)
            {
                // We got something like an IPV4-Address for an IPv6-Mask. This is not valid.
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                //    $"(NetworkingService) IsInSubnetMask: Received something like an IPV4-Address for an IPv6-Mask. This is not valid.");
                return false;
            }

            // Now find out how long the prefix is.
            int maskLength = int.Parse(subnetMask.Substring(slashIdx + 1));

            if (maskLength == 0)
                return true;

            if (maskLength < 0)
            {
                // throw new NotSupportedException("A Subnetmask should not be less than 0.");
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                //    $"(NetworkingService) IsInSubnetMask: A Subnetmask should not be less than 0.");
                return false;
            }

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
                {
                    // throw new ArgumentException("Length of IP Address and Subnet Mask do not match.");
                    //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} " +
                    //    $"(NetworkingService) IsInSubnetMask: Length of IP Address and Subnet Mask do not match.");
                    return false;
                }
                    

                // Compare the prefix bits.
                for (var i = ipAddressLength - 1; i >= ipAddressLength - maskLength; i--)
                    if (ipAddressBits[i] != maskAddressBits[i])
                        return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tuple which takes in session, and returns if the HostIP associated with the session is within
        /// a private network or not.
        /// </summary>
        /// <param name="session"></param>
        /// <returns>bool isPrivateIPAddress, string classType</returns>
        public Tuple<bool,string> IsPrivateIPAddress(Session session)
        {
            this.session = session;

            bool isPrivateIPAddress = false;
            string classType = "";

            if (this.session["X-HostIP"] == null)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Session X-HostIP is null.");
                return Tuple.Create(false, "");
            }

            if (!NetworkingService.Instance.IsValidIPAddress(this.session["X-HostIP"]))
            {
                isPrivateIPAddress = false;
                classType = "invalid IP address";
            }

            if (IsInSubnetMask(this.session["X-HostIP"], "10.0.0.0/8"))
            {
                isPrivateIPAddress = true;
                classType = "class A";
            }
            else if (IsInSubnetMask(this.session["X-HostIP"], "172.16.0.0/12"))
            {
                isPrivateIPAddress = true;
                classType = "class B";
            }
            else if (IsInSubnetMask(this.session["X-HostIP"], "192.168.0.0/16"))
            {
                isPrivateIPAddress = true;
                classType = "class C";
            }

            return Tuple.Create(isPrivateIPAddress, classType);
        }

        /// <summary>
        /// Tuple which takes in session, and returns if the HostIP associated with the session is within
        /// a private network or not.
        /// </summary>
        /// <param name="session"></param>
        /// <returns>bool isPrivateIPAddress, string classType</returns>
        public Tuple<bool, string> IsPrivateIPAddress(string ipAddress)
        {
            if (ipAddress == null || ipAddress == "")
            {
                return Tuple.Create(false,"Null or empty IP address input.");
            } 

            bool isPrivateIPAddress = false;
            string classType = "";

            if (!NetworkingService.Instance.IsValidIPAddress(ipAddress))
            {
                isPrivateIPAddress = false;
                classType = "invalid IP address";
            }

            if (IsInSubnetMask(ipAddress, "10.0.0.0/8"))
            {
                isPrivateIPAddress = true;
                classType = "class A";
            }
            else if (IsInSubnetMask(ipAddress, "172.16.0.0/12"))
            {
                isPrivateIPAddress = true;
                classType = "class B";
            }
            else if (IsInSubnetMask(ipAddress, "192.168.0.0/16"))
            {
                isPrivateIPAddress = true;
                classType = "class C";
            }

            return Tuple.Create(isPrivateIPAddress, classType);
        }

        /// <summary>
        /// Tuple which takes in session, and returns if the HostIP associated with the session is within
        /// a Microsoft365 subnet or not.
        /// </summary>
        /// <param name="session"></param>
        /// <returns>bool isMicrosoft365IP, string matchingSubnet</returns>
        public Tuple<bool,string> IsMicrosoft365IPAddress(Session session)
        {
            if (SettingsJsonService.Instance.GetDeserializedExtensionSettings().NeverWebCall)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} NeverWebCall true, returning.");
                return Tuple.Create(false, "NeverWebCall true.");
            }

            this.session = session;

            bool isMicrosoft365IP = false;
            string matchingSubnet = "";

            if (this.session["X-HostIP"] == null)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} Session X-HostIP is null.");
                return Tuple.Create(false, "Application preference MicrosoftURLsIPsWebService is null");
            }

            if (Preferences.MicrosoftURLsIPsWebService.Length == 0)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} MicrosoftURLsIPsWebService is null.");
                return Tuple.Create(false, "");
            }
            
            JArray jArray = JArray.Parse(Preferences.MicrosoftURLsIPsWebService);

            var children = jArray.Children();

            foreach (JObject child in children.Cast<JObject>())
            {
                try
                {
                    // Attempting to deserialize the Json object within child can and will fail here.
                    // Multiple Json sections in the source data do not include IPs, the only include URLs.
                    // For this reason this entire section needs to be within a try, catch statement to handle the failures in code.
                    var childJson = JsonConvert.DeserializeObject<NetworkingService.EndPointJson>(child.ToString());

                    // Iterate through the subnets in each child.
                    foreach (string subnet in childJson.IPs)
                    {
                        if (!NetworkingService.Instance.IsValidIPAddress(this.session["X-HostIP"]))
                        {
                            isMicrosoft365IP = false;
                            matchingSubnet = "invalid IP address";
                        }

                        if (IsInSubnetMask(this.session["X-HostIP"], subnet))
                        {
                            isMicrosoft365IP = true;
                            matchingSubnet = subnet;
                            break;
                        }
                        //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        //        $"{this.session["X-HostIP"]} in subnet {subnet}. isMicrosoft365IP = {isMicrosoft365IP}.");
                    }
                }
                catch //(Exception ex)
                {
                    // Do nothing here. We're expecting to have some children which do not include ips, which will throw an exception.
                    // Just want to ignore / handle these failures.

                    //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    //                $"{this.session["X-HostIP"]} Exception {ex}");
                }
            }

            return Tuple.Create(isMicrosoft365IP,matchingSubnet);
        }

        /// <summary>
        /// Tuple which takes in a string for ipAddress, and returns if the HostIP associated with the session is within
        /// a Microsoft365 subnet or not.
        /// </summary>
        /// <param name="string"></param>
        /// <returns>bool isMicrosoft365IP, string matchingSubnet</returns>
        public Tuple<bool, string> IsMicrosoft365IPAddress(string ipAddress)
        {
            if (ipAddress == null || ipAddress == "")
            {
                return Tuple.Create(false, "Null or empty IP address input.");
            }

            bool isMicrosoft365IP = false;
            string matchingSubnet = "";

            if (Preferences.MicrosoftURLsIPsWebService.Length == 0)
            {
                FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): MicrosoftURLsIPsWebService is null.");
                return Tuple.Create(false, "Application preference MicrosoftURLsIPsWebService is null");
            }

            JArray jArray = JArray.Parse(Preferences.MicrosoftURLsIPsWebService);

            var children = jArray.Children();

            foreach (JObject child in children.Cast<JObject>())
            {
                try
                {
                    // Attempting to deserialize the Json object within child can and will fail here.
                    // Multiple Json sections in the source data do not include IPs, the only include URLs.
                    // For this reason this entire section needs to be within a try, catch statement to handle the failures in code.
                    var childJson = JsonConvert.DeserializeObject<NetworkingService.EndPointJson>(child.ToString());

                    // Iterate through the subnets in each child.
                    foreach (string subnet in childJson.IPs)
                    {
                        if (!NetworkingService.Instance.IsValidIPAddress(ipAddress))
                        {
                            isMicrosoft365IP = false;
                            matchingSubnet = "invalid IP address";
                        }

                        if (IsInSubnetMask(ipAddress, subnet))
                        {
                            isMicrosoft365IP = true;
                            matchingSubnet = subnet;
                            break;
                        }
                        //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                        //        $"{this.session["X-HostIP"]} in subnet {subnet}. isMicrosoft365IP = {isMicrosoft365IP}.");
                    }
                }
                catch //(Exception ex)
                {
                    // Do nothing here. We're expecting to have some children which do not include ips, which will throw an exception.
                    // Just want to ignore / handle these failures.

                    //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): " +
                    //                $"{this.session["X-HostIP"]} Exception {ex}");
                }
            }

            return Tuple.Create(isMicrosoft365IP, matchingSubnet);
        }

        public class EndPointJson
        {
            public int Id { get; set; }

            public string ServiceArea { get; set; }

            public string ServiceAreaDisplayName { get; set; }

            public ArrayList Urls { get; set; }

            public ArrayList IPs { get; set; }

            public string TcpPorts { get; set; }

            public string UDPPorts { get; set; }

            public bool ExpressRoute { get; set; }

            public string Category { get; set; }

            public bool Required { get; set; }

            public string Notes { get; set; }
        }
    }
}
