using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPDClientSample
{
    using System.Net;
    using System.Net.Sockets;

    class Program
    {
        private static readonly string WelcomeMessage = "Hello Master";

        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient("localhost", 10000);

            udpClient.Send(Encoding.ASCII.GetBytes(WelcomeMessage), WelcomeMessage.Length);

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] data = udpClient.Receive(ref ipEndPoint);

            Console.WriteLine("Received from server: " + Encoding.ASCII.GetString(data));

            while (true)
            {
                Console.WriteLine(string.Empty);
                string input = Console.ReadLine();

                if (input != null && input.Equals("exit"))
                {
                    break;
                }

                udpClient.Send(Encoding.ASCII.GetBytes(input), input.Length);

                data = udpClient.Receive(ref ipEndPoint);

                Console.WriteLine(Encoding.ASCII.GetString(data));
            }

            Console.WriteLine("Terminating client...");
        }
    }
}
