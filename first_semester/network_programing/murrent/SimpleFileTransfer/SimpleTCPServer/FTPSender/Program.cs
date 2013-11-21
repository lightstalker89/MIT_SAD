using System;
using System.Text;

namespace FTPSender
{
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    class Program
    {
        static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);

            Console.WriteLine("Trying to connect ...");

            try
            {
                clientSocket.Connect(ipep);

                Console.WriteLine("Successfully connected");
            }
            catch (SocketException socketException)
            {
                Console.WriteLine("Error while connecting to the server {0}", ipep);
                Console.WriteLine("Exception: {0}", socketException.Message);
            }

            while (true)
            {
                byte[] dataBuffer = null;

                string message = Console.ReadLine();
                string[] parts = null;

                if (message != null)
                {
                    parts = message.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                }

                if (message != null && message.ToLower().Contains("get"))
                {
                    clientSocket.Send(Encoding.ASCII.GetBytes(message));
                }

                else if (message != null && message.ToLower().Contains("put"))
                {
                    byte[] file = File.ReadAllBytes(parts[1]);
                    byte[] fileName = Encoding.ASCII.GetBytes(parts[1]);
                    byte[] whiteSpace = Encoding.ASCII.GetBytes(" ");

                    dataBuffer = new byte[file.Length + fileName.Length + whiteSpace.Length];

                    Array.Copy(file, dataBuffer, file.Length);
                    Array.Copy(whiteSpace,0, dataBuffer, file.Length, whiteSpace.Length);
                    Array.Copy(fileName,0, dataBuffer, whiteSpace.Length + file.Length, fileName.Length);

                    clientSocket.Send(dataBuffer);
                }

                dataBuffer = new byte[1024];

                int len = clientSocket.Receive(dataBuffer);

                Console.WriteLine(Encoding.ASCII.GetString(dataBuffer, 0, len).Trim());
            }
        }
    }
}
