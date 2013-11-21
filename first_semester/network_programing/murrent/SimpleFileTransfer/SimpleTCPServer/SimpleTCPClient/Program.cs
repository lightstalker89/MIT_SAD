// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace SimpleTCPClient
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
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
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

            byte[] dataBuffer = new byte[1024];
            int len = clientSocket.Receive(dataBuffer);

            Console.WriteLine(Encoding.ASCII.GetString(dataBuffer, 0, len));

            while (true)
            {
                string input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }

                if (input != null)
                {
                    clientSocket.Send(Encoding.ASCII.GetBytes(input));
                }

                dataBuffer = new byte[1024];

                len = clientSocket.Receive(dataBuffer);

                Console.WriteLine(Encoding.ASCII.GetString(dataBuffer, 0, len));

                Console.WriteLine("Terminating client");
                clientSocket.Close();
            }
        }
    }
}