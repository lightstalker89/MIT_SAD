using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace TCP_POST_GET_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 10000);

            serverSocket.Bind(ipep);
            serverSocket.Listen(0);

            Console.WriteLine("Waiting for connect ...");

            Socket clientSocket = serverSocket.Accept();
            IPEndPoint clientAddress = (IPEndPoint)clientSocket.RemoteEndPoint;

            Console.WriteLine("connected to client {0}", clientAddress.ToString());

            string welcomeMsg = "Useful connected to File-Server!";
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(welcomeMsg);

            // send welcome message
            clientSocket.Send(data);

            while (true)
            {
                int len = 0;
                data = new byte[1024];

                try
                {
                    //Server receive action
                    len = clientSocket.Receive(data);
                    if (len == 0)
                    {
                        //client has closed connection
                        Console.WriteLine("Client has closed connection");
                        break;
                    }
                    string[] packet = Encoding.ASCII.GetString(data, 0, len).Split(' ');
                    switch (packet[0])
                    {
                        case "PUT":
                            //Server receive length of file
                            int length;
                            int.TryParse(packet[1], out length);

                            //Server saveFile
                            saveFile(clientSocket, packet[2], length);

                            break;
                        case "GET":
                            sendFile(clientSocket, packet[1]);
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

            serverSocket.Close();
        }

        private static void sendFile(Socket clientSocket, string fileName)
        {
            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {
                    clientSocket.Send(
                        Encoding.ASCII.GetBytes(String.Join(" ", new String[] {"OK", fs.Length.ToString(), fileName})));
                        //Send Action

                    if (fs.Length > 104857600) //100 MB
                        Console.WriteLine("Sending big data, please wait!");
                    else
                        Console.WriteLine("Sending data!");
                    clientSocket.SendFile(fileName);
                    fs.Dispose();
                    fs.Close();
                    Console.WriteLine("Send data!");
                }
            }
            catch (FileNotFoundException)
            {
                clientSocket.Send(Encoding.ASCII.GetBytes(String.Join(" ", new String[] { "Error", "", "" })));
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot send file {0} ", e.Message);
            }
        }

        private static void saveFile(Socket clientSocket, string fileName, int length)
        {
            using (FileStream output = File.Create(fileName))
            {
                if (length < 104857600) //100 MB
                {
                    Console.WriteLine("Receive data!");
                    byte[] buffer = new byte[length];
                    clientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    output.Write(buffer, 0, length);
                    output.Flush();
                }
                else
                {
                    Console.WriteLine("Receive big data, please wait!");
                    int readBytes = 0;
                    byte[] buffer = new byte[5120]; //5KB chunks
                    while (readBytes != length)
                    {
                        int read = clientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                        readBytes += read;
                        output.Write(buffer, 0, read);
                        output.Flush();
                    }
                }
                output.Dispose();
                output.Close();
                Console.WriteLine("Saved data successful!");
            }
        }

        private static void saveFile(byte[] data)
        {
            File.WriteAllBytes("Foo.txt", data);
        }
    }
}
