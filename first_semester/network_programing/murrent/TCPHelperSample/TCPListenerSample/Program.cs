// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace TCPListenerSample
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// </summary>
        private static readonly string WelcomeMessage = "Welcome to MDM's TCP Server Example";

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 10000);

            try
            {
                server.Start();
            }
            catch (IOException e)
            {
                Console.WriteLine("Error because another server is already running on this port");
            }

            Console.WriteLine("Waiting for client connection...");

            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine(client.Client.RemoteEndPoint + " connected successfully");

            NetworkStream ns = client.GetStream();

            ns.Write(Encoding.ASCII.GetBytes(WelcomeMessage), 0, WelcomeMessage.Length);
            ns.Flush();

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];

                    int len;

                    while ((len = ns.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        Console.Write(Encoding.ASCII.GetString(buffer, 0, len));

                        ns.Write(buffer, 0, buffer.Length);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Lost connection to client...");
                    client.Close();
                }
            }
        }
    }
}