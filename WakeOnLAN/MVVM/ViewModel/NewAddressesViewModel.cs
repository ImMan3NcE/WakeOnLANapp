
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
//using ThreadNetwork;
using WakeOnLAN.MVVM.Models;

namespace WakeOnLAN.MVVM.ViewModel
{
    public  class NewAddressesViewModel
    {

        public List<TempAddress> TempAddresses { get; set; }
        public TempAddress CurrentTempAddress { get; set; }

        public ICommand RefreshCommand { get; set; }

        public NewAddressesViewModel()
        {
            ScanNetwork();


            RefreshCommand = new Command(async () =>
            {
                
                ScanNetwork();
                TempAddresses = App.BaseRepo.GetAllTempAdresses();

            });

        }

        public async void ScanNetwork()
        {


            string subnet = GetIpAddress();

            if (subnet == "1")
            {
                Debug.WriteLine("Wi-Fi IP address not found.");
                return;
            }
            else if (subnet.Contains("Error:")) 
            {
                Debug.WriteLine(subnet);
                return;
            }
            else
            {
                if (IPAddress.TryParse(subnet, out IPAddress ipAddress))
                {
                    var partsIP = subnet.Split('.');
                    subnet = string.Join(".", partsIP.Take(partsIP.Length - 1))+".";
                }
                else
                {
                    Console.WriteLine("IP Address IP isnt correct.");
                }
                
            }

            var activeHosts = new List<string>();
            var pingTasks = new List<Task>();

            for (int i = 1; i < 255; i++)
            {
                string ip = $"{subnet}{i}";
                pingTasks.Add(Task.Run(async () =>
                {
                    var ping = new Ping();
                    var reply = await ping.SendPingAsync(ip, 100);
                    

                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine("-------------------------x");
                        try
                        {
                            IPHostEntry host = Dns.GetHostEntry(ip);
                            Console.WriteLine(host.HostName);
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine($"HostNameError: {ex.Message}"); 
                        }
                        

                        lock (activeHosts)
                        {
                            Console.WriteLine(ip);
                            activeHosts.Add(ip);
                        }
                    }
                }));
            }

            await Task.WhenAll(pingTasks);
            foreach (var host in activeHosts) 
                {
                 Debug.WriteLine(host);
                }
            
        }
        
        public  string GetIpAddress()
        {
            
            try
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface intf in interfaces)
                {
                    Console.WriteLine($"Name: {intf.Name}");
                    Console.WriteLine($"Description: {intf.Description}");
                    Console.WriteLine($"Type: {intf.NetworkInterfaceType}");
                    Console.WriteLine($"Status: {intf.OperationalStatus}");
                    Console.WriteLine($"Speed: {intf.Speed} bits per second");
                    Console.WriteLine($"MAC Address: {intf.GetPhysicalAddress()}");

                    IPInterfaceProperties properties = intf.GetIPProperties();
                    

                    foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
                    {
                        Console.WriteLine($"IP Address: {ip.Address}");
                    }
                    Console.WriteLine("--------------------------------------------------");

                    if (intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        intf.OperationalStatus == OperationalStatus.Up)
                    {
                        var ipProperties = intf.GetIPProperties();

                        var ipAddress = ipProperties.UnicastAddresses
                            .Where(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            .Select(ip => ip.Address.ToString())
                            .FirstOrDefault();

                        if (!string.IsNullOrEmpty(ipAddress))
                        {
                            return ipAddress;
                        }

                    }

                   

                }
                return "1";
            }
            catch (Exception ex)
            {

                return $"Error: {ex.Message}"; 
            }

            //return $"Error: {ex}";
        }

        


        #region MacAddressTest
        //public async Task<Dictionary<string, string>> ScanNetworkWithMac(string subnet)
        //{
        //    var activeHosts = new List<string>();
        //    var pingTasks = new List<Task>();
        //    var macAddresses = new Dictionary<string, string>();

        //    for (int i = 1; i < 255; i++)
        //    {
        //        string ip = $"{subnet}.{i}";
        //        pingTasks.Add(Task.Run(async () =>
        //        {
        //            var ping = new Ping();
        //            var reply = await ping.SendPingAsync(ip, 100);
        //            if (reply.Status == IPStatus.Success)
        //            {
        //                lock (activeHosts)
        //                {
        //                    activeHosts.Add(ip);
        //                }
        //            }
        //        }));
        //    }

        //    await Task.WhenAll(pingTasks);

        //    //foreach (var host in activeHosts)
        //    //{
        //    //    string mac = GetMacAddress(host);
        //    //    if (!string.IsNullOrEmpty(mac))
        //    //    {
        //    //        macAddresses[host] = mac;
        //    //    }
        //    //    Debug.WriteLine($"Host: {host}, MAC: {mac}");
        //    //}

        //    //return macAddresses;
        //}


        //private string GetMacAddress(string ipAddress)
        //{
        //    try
        //    {




        //        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        //        string macAddress = "";
        //        foreach (NetworkInterface ni in interfaces)
        //        {
        //            Console.WriteLine($"Name: {ni.Name}");
        //            Console.WriteLine($"Description: {ni.Description}");
        //            Console.WriteLine($"Type: {ni.NetworkInterfaceType}");
        //            Console.WriteLine($"Status: {ni.OperationalStatus}");
        //            Console.WriteLine($"Speed: {ni.Speed} bits per second");
        //            Console.WriteLine($"MAC Address: {ni.GetPhysicalAddress()}");

        //            macAddress = ni.GetPhysicalAddress().ToString();

        //            IPInterfaceProperties properties = ni.GetIPProperties();

        //            // Iteracja przez adresy unicast (IPv4/IPv6)
        //            foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
        //            {
        //                Console.WriteLine($"IP Address: {ip.Address}");

        //            }
        //            Console.WriteLine("--------------------------------------------------");
        //        }

        //        //string macAddress = (from nic in NetworkInterface.GetAllNetworkInterfaces()
        //        //                     where nic.OperationalStatus == OperationalStatus.Up
        //        //                     select nic.GetPhysicalAddress().ToString()
        //        //).FirstOrDefault();
        //        return macAddress;



        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Error fetching MAC for {ipAddress}: {ex.Message}");
        //    }

        //    return null;
        //}


        //public static string GetMACAddressViaIP(string ipAddr)
        //{
        //    string result = "";

        //    // Ścieżka do pliku /proc/net/arp
        //    string loc = "/proc/net/arp";

        //    // Sprawdzamy, czy plik istnieje
        //    if (!File.Exists(loc))
        //    {
        //        Console.WriteLine("Open " + loc + " failed");
        //        return result;
        //    }

        //    try
        //    {
        //        // Otwieramy plik
        //        using (StreamReader reader = new StreamReader(loc))
        //        {
        //            string line;
        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                // Sprawdzamy, czy linia zawiera poszukiwany adres IP
        //                if (line.Contains(ipAddr))
        //                {
        //                    // Szukamy miejsca w linii, gdzie zaczyna się adres MAC (po dwóch pierwszych znakach)
        //                    int index = line.IndexOf(":") - 2;
        //                    if (index >= 0)
        //                    {
        //                        // Przechodzimy po znakach adresu MAC i dodajemy do wyniku
        //                        result = line.Substring(index, 17); // 17 to długość adresu MAC w formacie XX:XX:XX:XX:XX:XX
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error reading ARP file: " + ex.Message);
        //    }

        //    return result;
        //}

        #endregion



    }
}
