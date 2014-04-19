using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace TCP_POST_GET_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket clieSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

            Console.WriteLine("Trying to connect ...");
            try
            {
                clieSocket.Connect(ipep);
            }
            catch (SocketException)
            {
                Console.WriteLine("Cannot connect to server {0}", ipep.ToString());
                Console.ReadKey();
                return;
            }

            byte[] data = new byte[1024];
            int len = clieSocket.Receive(data);
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, len));
            bool breakLoop = true;

            while (breakLoop)
            {
                Console.WriteLine();
                Console.WriteLine("Please enter GET or PUT command with an file name!");
                string[] action = Console.ReadLine().Split(' ');

                if (action.Length > 0)
                {
                    switch (action[0])
                    {
                        case "PUT":
                            sendFile(clieSocket, action[1]);
                            break;
                        case "GET":
                            requestFile(clieSocket, action[1]);
                            break;
                        case "EXIT":
                            breakLoop = false;
                            break;
                        default:
                            showHelp();
                            break;
                    }
                }
                else
                {
                    showHelp();
                }
                
            }

            Console.WriteLine("Terminating client");
            clieSocket.Close();
        }

        private static void sendFile(Socket clieSocket, string fileName)
        {
            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {
                    clieSocket.Send(Encoding.ASCII.GetBytes(String.Join(" ", new String[] { "PUT", fs.Length.ToString(), fileName}))); //Send Action
                    
                    if (fs.Length > 104857600) //100 MB
                        Console.WriteLine("Sending big data, please wait!");
                    else
                        Console.WriteLine("Sending data!");
                    clieSocket.SendFile(fileName);
                    fs.Dispose();
                    fs.Close();
                    Console.WriteLine("Send data!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot send file {0} ", e.Message);
            }

        }

        private static void requestFile(Socket clientSocket, string fileName)
        {
            try
            {
                clientSocket.Send(Encoding.ASCII.GetBytes(String.Join(" ", new String[] { "GET", fileName }))); //Send Action
                
                //get File info
                byte[] data = new byte[1024];
                int len = clientSocket.Receive(data);
                string[] packet = Encoding.ASCII.GetString(data, 0, len).Split(' ');

                switch (packet[0])
                {
                    case "OK":
                        int packetSize;
                        int.TryParse(packet[1], out packetSize);
                        saveFile(clientSocket, packet[2], packetSize);
                        break;
                    default:
                        Console.WriteLine("File on Server not found!");
                        break;
                }
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

        private static void showHelp()
        {
            Console.WriteLine("Example: GET test.exe");
        }
    }
}
