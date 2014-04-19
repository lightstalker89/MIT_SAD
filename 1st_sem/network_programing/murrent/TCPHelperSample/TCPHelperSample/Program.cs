// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace TCPClientSample
{
    using System;
    using System.Linq;
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
            TcpClient serverConnection = Connect();

            if (serverConnection != null)
            {
                NetworkStream ns = serverConnection.GetStream();

                byte[] buffer = new byte[1024];

                int len = ns.Read(buffer, 0, buffer.Length);

                Console.Write(Encoding.ASCII.GetString(buffer, 0, len));

                while (true)
                {
                    Console.WriteLine(string.Empty);
                    string input = Console.ReadLine();

                    if (input != null && input.Equals("exit"))
                    {
                        break;
                    }

                    if (input != null)
                    {
                        ns.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
                        ns.Flush();
                    }

                    Array.Clear(buffer, 0, buffer.Length);

                    len = ns.Read(buffer, 0, buffer.Length);

                    Console.Write(Encoding.ASCII.GetString(buffer, 0, buffer.ToList().Count(p => p != 0)));
                }

                Console.WriteLine("Client is terminating");
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static TcpClient Connect()
        {
            TcpClient serverConnection = null;
            try
            {
                serverConnection = new TcpClient("localhost", 10000);
            }
            catch (Exception)
            {
                Console.WriteLine("Server unreachable");
            }

            return serverConnection;
        }
    }
}