// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace UPDClientSample
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
        private const string WelcomeMessage = "Hello Master";

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
            udpClient = new UdpClient("192.168.45.74", 10000);

            ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // byte[] data = udpClient.Receive(ref ipEndPoint);

            // Console.WriteLine("Received from server: " + Encoding.ASCII.GetString(data));
            Thread sendThread = new Thread(Send);
            sendThread.Start();

            Thread receiveThread = new Thread(Receive);
            receiveThread.Start();
        }

        /// <summary>
        /// </summary>
        private static void Send()
        {
            udpClient.Send(Encoding.ASCII.GetBytes(WelcomeMessage), WelcomeMessage.Length);

            while (true)
            {
                string input = Console.ReadLine();

                if (input != null && input.Equals("exit"))
                {
                    break;
                }

                udpClient.Send(Encoding.ASCII.GetBytes(input), input.Length);

                // Console.WriteLine(Encoding.ASCII.GetString(data));
            }

            Console.WriteLine("Terminating client...");
        }

        /// <summary>
        /// </summary>
        private static void Receive()
        {
            while (true)
            {
                byte[] data = udpClient.Receive(ref ipEndPoint);

                Console.WriteLine("Received from server: " + Encoding.ASCII.GetString(data));
            }
        }
    }
}