namespace UDPServerSample
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    class Program
    {
        private static readonly string WelcomeMessage = "Hello Master";

        static void Main(string[] args)
        {
            UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 10000));

            udpClient.Send(Encoding.ASCII.GetBytes(WelcomeMessage), WelcomeMessage.Length);

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] data = udpClient.Receive(ref ipEndPoint);

            Console.WriteLine("Received from client: " + Encoding.ASCII.GetString(data));

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
