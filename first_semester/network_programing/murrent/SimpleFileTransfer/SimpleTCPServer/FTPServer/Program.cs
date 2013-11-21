using System;

namespace FTPServer
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

            serverSocket.Bind(ipep);
            serverSocket.Listen(5);

            Console.WriteLine("Waiting for connect ...");

            Socket clientSocket = serverSocket.Accept();

            IPEndPoint clientAddress = clientSocket.RemoteEndPoint as IPEndPoint;


            if (clientAddress != null)
            {
                IPHostEntry entry = Dns.GetHostEntry(clientAddress.Address);

                Console.WriteLine("Connected to client {0} aka {1}", clientAddress, entry.HostName);
            }

            while (true)
            {
                int len;

                byte[] dataBuffer = new byte[2084];

                try
                {
                    len = clientSocket.Receive(dataBuffer);
                    string inFromClient = Encoding.ASCII.GetString(dataBuffer, 0, len);

                    Console.WriteLine("Received from client: " + inFromClient);
                }
                catch (Exception)
                {
                    Console.WriteLine("Lost connection to client");
                    clientSocket.Close();
                    break;
                }

                // SEND FILE BACK

            }

            clientSocket.Close();
            serverSocket.Close();
        }
    }
}
