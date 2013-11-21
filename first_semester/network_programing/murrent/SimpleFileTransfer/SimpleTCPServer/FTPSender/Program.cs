using System;
using System.Text;

namespace FTPSender
{
    using System.Net;
    using System.Net.Sockets;

    class Program
    {
        private const string GetExtension = " HTTP/1.1\r\n\r\n";

        private const string PutExtension = "";

        static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

            Console.WriteLine("Trying to connect ...");

            try
            {
                clientSocket.Connect(ipep);
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("Error while connecting to the server {0}", ipep);
                Console.WriteLine("Exception: {0}", socketException.Message);
            }

            while (true)
            {
                string message = Console.ReadLine();

                if (message != null && message.ToLower().Contains("get"))
                {
                    message += GetExtension;
                }
                else if (message != null && message.ToLower().Contains("put"))
                {
                    message += PutExtension;
                }

                clientSocket.Send(Encoding.ASCII.GetBytes(message));

                byte[] dataBuffer = new byte[1024];

                int len = clientSocket.Receive(dataBuffer);

                Console.WriteLine(Encoding.ASCII.GetString(dataBuffer, 0, len).Trim());
            }
        }
    }
}
