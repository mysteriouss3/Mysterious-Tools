using System;
using System.Collections.Generic;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousTools.Hwid
{
    public class Finger
    {
        private static string _systemInformation = string.Empty;

        public static async Task<string> GetSystemInformationAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_systemInformation))
                {
                    var pcInfoTask = GetIdentifierAsync("Win32_ComputerSystemProduct", new List<string> { "UUID" });
                    var cpuIdTask = GetIdentifierAsync("Win32_Processor", new List<string> { "UniqueId", "ProcessorId", "Name", "Manufacturer", "NumberOfCores", "Revision" });
                    var biosIdTask = GetIdentifierAsync("Win32_BIOS", new List<string> { "Manufacturer", "SMBIOSBIOSVersion", "IdentificationCode", "SerialNumber", "ReleaseDate", "Version" });
                    var mainboardIdTask = GetIdentifierAsync("Win32_BaseBoard", new List<string> { "Model", "Manufacturer", "Name", "SerialNumber", "Product" });
                    var gpuIdTask = GetIdentifierAsync("Win32_VideoController", new List<string> { "Name" });
                    var ramInfoTask = GetIdentifierAsync("Win32_PhysicalMemory", new List<string> { "Name", "Manufacturer", "PartNumber", "Capacity", "SerialNumber" });
                    var diskIdTask = GetIdentifierAsync("Win32_LogicalDisk", new List<string> { "VolumeSerialNumber", "Size" }, "DeviceID='C:'");
                    //var networkIdTask = GetIdentifierAsync("Win32_NetworkAdapterConfiguration", new List<string> { "MACAddress" }, "NOT Description LIKE '%virtual%' AND (Description LIKE '%Ethernet%' OR Description LIKE '%Wireless%')");

                    await Task.WhenAll(pcInfoTask, cpuIdTask, biosIdTask, mainboardIdTask, gpuIdTask, ramInfoTask, diskIdTask);

                    var pcInfoResult = await pcInfoTask;
                    var cpuIdResult = await cpuIdTask;
                    var biosIdResult = await biosIdTask;
                    var mainboardIdResult = await mainboardIdTask;
                    var gpuIdResult = await gpuIdTask;
                    var ramInfoResult = await ramInfoTask;
                    var diskIdResult = await diskIdTask;
                    var networkIdResult = await GetLANAndWiFiMACAddressesAsync();
                    var networkIdResultString = string.Join("-", networkIdResult);

                    var concatStr = $"PC: {pcInfoResult}\nCPU: {cpuIdResult}\nBIOS: {biosIdResult}\nMainboard: {mainboardIdResult}\n" +
                        $"GPU: {gpuIdResult}\nRAM: {ramInfoResult}\nDisk: {diskIdResult}\nNetwork: {networkIdResultString}";

                    _systemInformation = concatStr;
                }
                return await GetHashAsync(_systemInformation);
            }
            catch (Exception ex)
            {
                return await GetHashAsync($"{Environment.MachineName}-{Environment.UserName}-{Environment.ProcessorCount}-{ex.Message}");
            }
        }


        private static Task<string> GetHashAsync(string s)
        {
            using (var provider = new SHA256CryptoServiceProvider())
            {
                var utf8 = Encoding.UTF8.GetBytes(s);
                var hashBytes = provider.ComputeHash(utf8);
                var guidBytes = new byte[16];
                Array.Copy(hashBytes, guidBytes, 16);
                return Task.FromResult(new Guid(guidBytes).ToString().ToUpper());
            }
        }

        public static Task<List<string>> GetLANAndWiFiMACAddressesAsync()
        {
            List<string> macAddresses = new List<string>();

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in interfaces)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet || adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    macAddresses.Add(adapter.GetPhysicalAddress().ToString());
                }
            }

            return Task.FromResult(macAddresses);
        }

        private static async Task<string> GetIdentifierAsync(string wmiClass, List<string> properties, string condition = null)
        {
            var result = new StringBuilder();
            try
            {
                var query = $"SELECT {string.Join(",", properties)} FROM {wmiClass}";
                if (!string.IsNullOrEmpty(condition))
                {
                    query += $" WHERE {condition}";
                }

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    var items = await Task.Run(() => searcher.Get());

                    foreach (var item in items)
                    {
                        foreach (var property in properties)
                        {
                            var value = item.Properties[property]?.Value?.ToString();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                result.Append($"{value};");
                            }
                        }
                    }
                }
            }
            catch
            {
                return $"{Environment.MachineName}-{Environment.UserName}";
            }
            return result.ToString().Trim().Replace(" ", string.Empty);
        }
    }
}
