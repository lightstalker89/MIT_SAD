// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace UDPServerSample
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// </summary>
        private static Thread receiveThread;

        /// <summary>
        /// </summary>
        private static readonly string WelcomeMessage = "Hello Master";

        /// <summary>
        /// </summary>
        private static UdpClient udpClient;

        /// <summary>
        /// </summary>
        private static IPEndPoint ipEndPoint;

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 10000));

            ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // udpClient.Send(Encoding.ASCII.GetBytes(WelcomeMessage), WelcomeMessage.Length);
            receiveThread = new Thread(Receive);
            receiveThread.Start();
        }

        /// <summary>
        /// </summary>
        private static void Receive()
        {
            byte[] data = new byte[1024];

            while (true)
            {
                udpClient.Receive(ref ipEndPoint);

                data = udpClient.Receive(ref ipEndPoint);

                Console.WriteLine(Encoding.ASCII.GetString(data));

                udpClient.Send(data, data.Length, ipEndPoint);
            }
        }
    }
}