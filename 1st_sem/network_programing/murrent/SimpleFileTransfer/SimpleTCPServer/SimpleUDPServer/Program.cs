// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace SimpleUDPServer
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
            Socket updSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 10000);

            updSocket.Bind(ipep);

            Console.WriteLine("Waiting for client ...");

            IPEndPoint broadCastIPEP = new IPEndPoint(IPAddress.Broadcast, 0);
            IPEndPoint remoteIPEP = new IPEndPoint(IPAddress.Any, 0);

            EndPoint remoteEP = remoteIPEP;

            byte[] dataBuffer = new byte[1024];
            int len = 0;
            len = updSocket.ReceiveFrom(dataBuffer, ref remoteEP);

            string text = Encoding.ASCII.GetString(dataBuffer, 0, len);

            Console.WriteLine("Message is {0}", text);
            Console.WriteLine("From host {0}", remoteIPEP.Address);

            string welcomeMessage = "Welcome to MDM's server";
            dataBuffer = Encoding.ASCII.GetBytes(welcomeMessage);

            updSocket.SendTo(dataBuffer, remoteEP);

            while (true)
            {
                dataBuffer = new byte[1024];
                len = updSocket.ReceiveFrom(dataBuffer, ref remoteEP);

                text = Encoding.ASCII.GetString(dataBuffer, 0, len);

                Console.WriteLine("{0}: {1} - {2}", remoteEP, text, DateTime.Now);

                dataBuffer = Encoding.ASCII.GetBytes(text);
                updSocket.SendTo(dataBuffer, remoteEP);
            }
        }
    }
}