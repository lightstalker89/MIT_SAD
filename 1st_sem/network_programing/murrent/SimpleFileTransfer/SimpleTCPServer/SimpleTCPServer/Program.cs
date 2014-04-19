// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace SimpleTCPServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// </summary>
        private const string WelcomeMessage = "Welcome to MDM's Server";

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

            serverSocket.Bind(ipep);
            serverSocket.Listen(5);

            Console.WriteLine("Waiting for connect ...");

            Socket clientSocket = serverSocket.Accept();

            IPEndPoint clientAddress = clientSocket.RemoteEndPoint as IPEndPoint;

            Console.WriteLine("Connected to client {0}", clientAddress);

            /*
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(WelcomeMessage);
            clientSocket.Send(Encoding.ASCII.GetBytes(data);
            */
            byte[] dataBuffer = new byte[1024];

            IPHostEntry entry = Dns.GetHostEntry(clientAddress.Address);

            Console.WriteLine(entry.HostName);

            clientSocket.Send(Encoding.ASCII.GetBytes(WelcomeMessage));

            while (true)
            {
                int len = 0;

                dataBuffer = new byte[24];

                try
                {
                    len = clientSocket.Receive(dataBuffer);
                }
                catch (Exception)
                {
                    Console.WriteLine("Lost connection to client");
                    clientSocket.Close();
                    break;
                }

                if (len == 0)
                {
                    Console.WriteLine("Client has closed connection");
                    break;
                }

                string message = Encoding.ASCII.GetString(dataBuffer, 0, len);
                Console.WriteLine(message);

                dataBuffer = Encoding.ASCII.GetBytes(message);
                clientSocket.Send(dataBuffer);
            }

            clientSocket.Close();
            serverSocket.Close();
        }
    }
}