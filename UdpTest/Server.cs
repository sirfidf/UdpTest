using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace UdpTest
{
    public class Server
    {
        public static void Start()
        {
            using UdpClient client = new();
            var ipAddressAndBroadcastAddressDisctionary = GetLocalIpAddressAndBroadcastAddressDictionary();

            while (true)
            {
                foreach (var ipAddressAndBroadcastAddress in ipAddressAndBroadcastAddressDisctionary)
                {
                    var data = Encoding.UTF8.GetBytes("agent endpoint: " + ipAddressAndBroadcastAddress.Key + ":32400");
                    client.Send(data, data.Length, new IPEndPoint(ipAddressAndBroadcastAddress.Value, 12345));
                }
                Thread.Sleep(5000);
            }
        }

        public static Dictionary<IPAddress, IPAddress> GetLocalIpAddressAndBroadcastAddressDictionary()
        {
            Dictionary<IPAddress, IPAddress> ipAddressAndBroadcastAddressDisctionary = [];
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var address in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        var ipBytes = address.Address.GetAddressBytes();
                        if (ipBytes[0] == 127 || ipBytes[0] == 169)
                        {
                            continue;
                        }
                        var subnetMaskBytes = address.IPv4Mask.GetAddressBytes();
                        var broadcastAddressBytes = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            broadcastAddressBytes[i] = (byte)(ipBytes[i] | ~subnetMaskBytes[i]);
                        }
                        var broadcastAddress = new IPAddress(broadcastAddressBytes);
                        ipAddressAndBroadcastAddressDisctionary[address.Address] = broadcastAddress;
                    }
                }
            }
            return ipAddressAndBroadcastAddressDisctionary;
        }
    }
}
