// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace FTPSender
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
        private static readonly Socket ClientSocket = new Socket(
            AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// </summary>
        private static readonly IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Trying to connect ...");

            try
            {
                ClientSocket.Connect(IpEndPoint);
                Console.WriteLine("Successfully connected");
            }
            catch (SocketException)
            {
                Console.WriteLine("Cannot connect to server {0}", IpEndPoint);
                Console.ReadKey();
                return;
            }

            byte[] data = new byte[1024];

            int len = ClientSocket.Receive(data);

            Console.WriteLine(Encoding.ASCII.GetString(data, 0, len));

            while (true)
            {
                string consoleInput = Console.ReadLine();

                if (consoleInput != null)
                {
                    string[] parts = consoleInput.Split(' ');

                    switch (parts[0].ToLower(CultureInfo.CurrentCulture))
                    {
                        case "put":
                            PutFile(parts[1]);
                            break;
                        case "get":
                            GetFile(parts[1]);
                            break;

                        case "exit":
                            ClientSocket.Close();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Puts the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private static void PutFile(string fileName)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);

                ClientSocket.Send(
                    Encoding.ASCII.GetBytes(
                        string.Join(
                            " ", new[] { "PUT", fileBytes.Length.ToString(CultureInfo.InvariantCulture), fileName })));

                ClientSocket.SendFile(fileName);

                Console.WriteLine("Sending data ...");
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot send file {0} ", e.Message);
            }
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private static void GetFile(string fileName)
        {
            try
            {
                ClientSocket.Send(Encoding.ASCII.GetBytes(string.Join(" ", new[] { "GET", fileName })));

                byte[] data = new byte[1024];
                int len = ClientSocket.Receive(data);
                string[] packet = Encoding.ASCII.GetString(data, 0, len).Split(' ');

                switch (packet[0])
                {
                    case "OK":
                        int packetSize;
                        int.TryParse(packet[1], out packetSize);
                        SaveFile(packet[2], packetSize);
                        break;
                    default:
                        Console.WriteLine("Error while receiving file");
                        break;
                }
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
        private static void SaveFile(string fileName, int length)
        {
            byte[] buffer;

            if (length < 104857600)
            {
                Console.WriteLine("Receiving data ...");
                buffer = new byte[length];
                ClientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
            }
            else
            {
                Console.WriteLine("Receiving bigger data than expected ...");
                int readBytes = 0;
                buffer = new byte[5120];
                while (readBytes != length)
                {
                    int read = ClientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    readBytes += read;
                }
            }

            File.WriteAllBytes(fileName, buffer);

            Console.WriteLine("File successfully saved");
        }
    }
}