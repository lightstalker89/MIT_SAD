// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace FTPServer
{
    using System;
    using System.Globalization;
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
        private static readonly Socket ServerSocket = new Socket(
            AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// </summary>
        private static readonly IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 10000);

        /// <summary>
        /// </summary>
        private static Socket clientSocket;

        /// <summary>
        /// </summary>
        private const string WelcomeMessage = "Welcome to MDM's FTP Server";

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            ServerSocket.Bind(IpEndPoint);
            ServerSocket.Listen(0);

            Console.WriteLine("Waiting for client connection ...");

            clientSocket = ServerSocket.Accept();
            IPEndPoint clientAddress = (IPEndPoint)clientSocket.RemoteEndPoint;

            Console.WriteLine("connected to client {0}", clientAddress);

            byte[] data = Encoding.ASCII.GetBytes(WelcomeMessage);

            clientSocket.Send(data);

            while (true)
            {
                data = new byte[1024];

                try
                {
                    int len = clientSocket.Receive(data);

                    if (len == 0)
                    {
                        Console.WriteLine("Client has closed connection");
                        break;
                    }

                    string[] parts = Encoding.ASCII.GetString(data, 0, len).Split(' ');

                    switch (parts[0].ToLower(CultureInfo.CurrentCulture))
                    {
                        case "put":
                            int length;
                            int.TryParse(parts[1], out length);

                            bool result = SaveFile(parts[2], length);

                            clientSocket.Send(
                                result
                                    ? Encoding.ASCII.GetBytes("File received")
                                    : Encoding.ASCII.GetBytes("Error while receiving the file"));
                            break;

                        case "get":
                            SendFileToClient(parts[1]);
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Lost connection to client");
                    break;
                }
            }

            clientSocket.Close();
            ServerSocket.Close();
        }

        /// <summary>
        /// Sends the file to client.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private static void SendFileToClient(string fileName)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);

                clientSocket.Send(
                    Encoding.ASCII.GetBytes(
                        string.Join(
                            " ", new[] { "OK", fileBytes.Length.ToString(CultureInfo.CurrentCulture), fileName })));

                clientSocket.SendFile(fileName);

                Console.WriteLine("Sending data ...");
            }
            catch (FileNotFoundException)
            {
                clientSocket.Send(Encoding.ASCII.GetBytes(string.Join(" ", new[] { "Error", string.Empty, string.Empty })));
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot send file {0} ", e.Message);
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        private static bool SaveFile(string fileName, int length)
        {
            try
            {
                byte[] buffer;

                if (length < 104857600)
                {
                    Console.WriteLine("Receiving data ...");
                    buffer = new byte[length];
                    clientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                }
                else
                {
                    Console.WriteLine("Receiving bigger data than expected ...");
                    int readBytes = 0;
                    buffer = new byte[5120];
                    while (readBytes != length)
                    {
                        int read = clientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                        readBytes += read;
                    }
                }

                File.WriteAllBytes(fileName, buffer);

                Console.WriteLine("File successfully saved");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}