using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUDPClient
{
    using System.Net;
    using System.Net.Sockets;

    class Program
    {
        static void Main(string[] args)
        {
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

            udpSocket.Bind(ipep);
            udpSocket.EnableBroadcast = true;

            string ping = "Hello Server!";

            byte[] data = Encoding.ASCII.GetBytes(ping);

            IPEndPoint udpServer = new IPEndPoint(IPAddress.Broadcast, 10000);

            EndPoint serverEP = udpServer as EndPoint;

            udpSocket.SendTo(data, serverEP);

            IPEndPoint lastSender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderEP = lastSender as EndPoint;

            data = new byte[1024];
            int len = udpSocket.ReceiveFrom(data, ref senderEP);

            if (senderEP != serverEP)
            {
                // this is not our server ...
            }

            Console.WriteLine(Encoding.ASCII.GetString(data, 0, len));
            Console.WriteLine("Server is {0}", senderEP);

            while (true)
            {
                //string input = "**************************WUFF**WUFF**WUFF*******************************";

                string input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }

                udpSocket.SendTo(Encoding.ASCII.GetBytes(input), serverEP);

                data = new byte[1024];
                len = udpSocket.ReceiveFrom(data, ref senderEP);

                Console.WriteLine(Encoding.ASCII.GetString(data, 0, len));
            }

            udpSocket.Close();
        }
    }
}
