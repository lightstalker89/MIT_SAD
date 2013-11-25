// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace TCPSelectSample
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// </summary>
        private static readonly ArrayList SocketList = new ArrayList(2);

        /// <summary>
        /// </summary>
        private static ArrayList copyList = new ArrayList(2);

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 10000);

            serverSocket.Bind(ipep);
            serverSocket.Listen(5);

            Console.WriteLine("Waiting for client ...");

            Socket clientSocket = serverSocket.Accept();
            IPEndPoint clientEndpoint = clientSocket.RemoteEndPoint as IPEndPoint;

            if (clientEndpoint != null)
            {
                Console.WriteLine("Client connected from " + clientEndpoint.Address);
            }

            Console.WriteLine("Waiting for client ...");

            Socket clientSocketSecond = serverSocket.Accept();
            IPEndPoint clientEndpointSecond = clientSocketSecond.RemoteEndPoint as IPEndPoint;

            if (clientEndpointSecond != null)
            {
                Console.WriteLine("Client connected from " + clientEndpointSecond.Address);
            }

            if (clientEndpoint != null)
            {
                SocketList.Add(clientSocket);
            }

            if (clientEndpointSecond != null)
            {
                SocketList.Add(clientSocketSecond);
            }

            serverSocket.Close();

            while (true)
            {
                copyList = new ArrayList(SocketList);

                Console.WriteLine("monitoring {0}", SocketList.Count);

                Socket.Select(copyList, null, null, 1000000);

                foreach (Socket socket in copyList)
                {
                    byte[] data = new byte[1024];

                    //int len = socket.Receive(data);

                    //if (len == 0)
                    //{
                    //    Console.WriteLine("Socket {0} closed", socket.RemoteEndPoint);

                    //    SocketList.Remove(socket);

                    //    if (SocketList.Count == 0)
                    //    {
                    //        return;
                    //    }
                    //}

                    SendMessage(Encoding.ASCII.GetBytes("teste"));
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="data">
        /// </param>
        private static void SendMessage(byte[] data)
        {
            foreach (Socket socket in SocketList)
            {
                socket.Send(data);
            }
        }
    }
}