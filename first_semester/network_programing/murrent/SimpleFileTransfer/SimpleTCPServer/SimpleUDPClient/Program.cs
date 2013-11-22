// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace SimpleUDPClient
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
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

            udpSocket.Bind(ipep);
            udpSocket.EnableBroadcast = true;

            string ping = "Hello Server!";

            byte[] data = Encoding.ASCII.GetBytes(ping);

            IPEndPoint udpServer = new IPEndPoint(IPAddress.Broadcast, 10000);

            EndPoint serverEP = udpServer;

            udpSocket.SendTo(data, serverEP);

            IPEndPoint lastSender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderEP = lastSender;

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
                // string input = "**************************WUFF**WUFF**WUFF*******************************";
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