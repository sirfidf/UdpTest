using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpTest
{
    public class Client
    {
        public static void Start()
        {
            using UdpClient client = new();
            client.Client.Bind(new IPEndPoint(IPAddress.Any, 12345));
            var from = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                var data = client.Receive(ref from);
                var message = Encoding.UTF8.GetString(data);
                Console.WriteLine(message);
            }
        }
    }
}
