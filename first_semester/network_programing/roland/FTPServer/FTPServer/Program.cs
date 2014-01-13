using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FTPServer
{

    enum SendType
    {
        Ascii,
        Image
    }

    public class Program
    {
        private static TcpClient FTPClient { get; set; }
        private static NetworkStream FTPClientStream { get; set; }
        //private static TcpClient FTPDataClient { get; set; }
        private static TcpListener FTPListener { get; set; }
        private static TcpListener FTPPassiveListener { get; set; }
        private static Logger Logger { get; set; }
        private static IPAddress FTPClientIP { get; set; }
        private static string MyRootDir = "C:\\temp\\FTPPath";
        private static string CurrentDir { get; set; }

        private static SendType Type = SendType.Ascii;


        public static void Main(string[] args)
        {
            Console.BufferHeight = 5000;
            Console.BufferWidth = 5000;

            Logger = new Logger();
            Thread serverThread = new Thread(StartServer);

            if (!Directory.Exists(MyRootDir))
            {
                Directory.CreateDirectory(MyRootDir);
            }

            CurrentDir = MyRootDir;
            serverThread.Start();
        }

        public static void StartServer()
        {
            FTPListener = new TcpListener(IPAddress.Loopback, 21);
            FTPListener.Start();

            Logger.Log("Server started!");

            try
            {
                FTPClient = FTPListener.AcceptTcpClient();

                IPEndPoint ipEndPoint = FTPClient.Client.RemoteEndPoint as IPEndPoint;

                if (ipEndPoint != null)
                {
                    FTPClientIP = ipEndPoint.Address;
                }

                Thread clientThread = new Thread(ListenToFTPClient);
                clientThread.Start();
            }
            catch (Exception)
            {
            }
        }

        private static void ListenToFTPClient()
        {
            Logger.Log(string.Format("Client {0} connected", FTPClientIP));

            //try
            //{
                FTPClientStream = FTPClient.GetStream();

                SendMessage("220 Server ready\r\n");

                string clientMessage;

                while ((clientMessage = ReadMessage()) != null)
                {
                    Thread.Sleep(100);

                    Logger.Log(clientMessage);
                    string[] splitResponse = clientMessage.Split(' ');
                    string ftpCommand = splitResponse[0].Length == 0 ? string.Empty : splitResponse[0].ToUpperInvariant();
                    
                    string ftpArgument = GetFTPArgumentFromResponse(splitResponse);

                    switch (ftpCommand)
                    {
                        case "USER":
                            SendMessage("331 Username ok, need password\r\n");
                            break;

                        case "PASS":
                            SendMessage("230 User logged in\r\n");
                            break;

                        case "BYE":
                            break;

                        case "BYTE":
                            break;

                        case "RETR":
                            break;
                        case "PORT":
                            //SendMessage(PortResponse(ftpArgument));
                            break;
                        case "STORE":
                            break;

                        case "LIST":
                            SendMessage(ListResponse(CurrentDir));
                            break;

                        case "SYST":
                            SendMessage("215 WINDOWS-NT-6\r\n");
                            break;

                        case "TYPE":
                            SendMessage(TypeResponse(ftpArgument));
                            break;

                        case "FEAT":
                            SendMessage("no-features\r\n");
                            break;

                        case "PWD":
                            SendMessage(string.Format("257 \"{0}\" is current directory.\r\n", GetCurrentWorkingDirectory()));
                            break;

                        case "PASV":
                            SendMessage(PassiveModeResponse());
                            break;
                        case "CWD":
                            SendMessage(ChangeWorkingDirectoryResponse(ftpArgument));
                            //SendMessage(string.Format("257 \"{0}\" is current directory.\r\n", GetCurrentWorkingDirectory()));
                            break;
                        case "":
                            break;

                        default:
                            SendMessage("502 Command not implemented\r\n");
                            break;
                    }
                }
            //}
            //catch (Exception)
            //{

            //}
        }

        private static void SendMessage(string message)
        {
            Thread currentThread = Thread.CurrentThread;

            lock (currentThread)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                FTPClientStream.Write(buffer, 0, buffer.Length);
            }
        }

        private static string ReadMessage()
        {
            StreamReader reader = new StreamReader(FTPClientStream);
            return reader.ReadLine();
        }

        private static string GetFTPArgumentFromResponse(string[] response)
        {
            if (response.Length <= 1)
            {
                return null;
            }
            StringBuilder result = new StringBuilder();
            for (int i = 1; i < response.Length; i++)
            {
                result.Append(response[i]);
            }

            return result.ToString();
        }

        private static string GetCurrentWorkingDirectory()
        {
            string workDir = CurrentDir.Replace(MyRootDir, string.Empty);

            if (workDir.Length == 0)
            {
                workDir = "/";
            }

            return workDir.Replace('\\', '/');
        }

        private static string TypeResponse(string attribute)
        {
            switch (attribute)
            {
                case "A":
                    Type = SendType.Ascii;
                    break;
                case "I":
                    Type = SendType.Image;
                    break;
                default:
                    return "502 Command not implemented\r\n";
            }

            return string.Format("200 Type set to {0}\r\n", Type);
        }

        private static string PassiveModeResponse()
        {
            IPAddress localIp = ((IPEndPoint)FTPClient.Client.LocalEndPoint).Address;

            FTPPassiveListener = new TcpListener(localIp, 0);
            FTPPassiveListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)FTPPassiveListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short port = (short)passiveListenerEndpoint.Port;

            byte[] portArray = BitConverter.GetBytes(port);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(portArray);
            }
            return string.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})\r\n", address[0], address[1], address[2], address[3], portArray[0], portArray[1]);
        }

        private static string ListResponse(string dir)
        {
            if (Directory.Exists(dir))
            {
                SendMessage("150 Opening Passive mode data transfer for LIST\r\n");
                FTPPassiveListener.BeginAcceptTcpClient(HandleList, CurrentDir);
            }

            return "450 Requested file action not taken\r\n";
        }

        private static void HandleList(IAsyncResult result)
        {
            TcpClient ftpDataClient = FTPPassiveListener.EndAcceptTcpClient(result);

            string pathName = (string)result.AsyncState;

            NetworkStream netStream = ftpDataClient.GetStream();
            StreamWriter dataWriter = new StreamWriter(netStream, Encoding.ASCII);

            string[] directories = Directory.GetDirectories(pathName);
            string[] files = Directory.GetFiles(pathName);

            foreach (string dir in directories)
            {
                DirectoryInfo d = new DirectoryInfo(dir);

                string date = d.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                    d.LastWriteTime.ToString("MMM dd  yyyy") :
                    d.LastWriteTime.ToString("MMM dd HH:mm");

                string line = string.Format("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, d.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            foreach (string file in files)
            {
                FileInfo f = new FileInfo(file);

                string date = f.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                    f.LastWriteTime.ToString("MMM dd  yyyy") :
                    f.LastWriteTime.ToString("MMM dd HH:mm");

                string line = string.Format("-rw-r--r--    2 2003     2003     {0,8} {1} {2}", f.Length, date, f.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            netStream.Close();
            netStream.Dispose();

            ftpDataClient.Close();
            ftpDataClient = null;

            SendMessage("226 Transfer complete\r\n");
        }

        private static string ChangeWorkingDirectoryResponse(string path)
        {
            if (path.Equals("/"))
            {
                CurrentDir = MyRootDir;
            }
            else
            {
                string directory;

                if (path.StartsWith("/"))
                {
                    path = path.Substring(1).Replace('/', '\\');
                    directory = Path.Combine(MyRootDir, path);
                }
                else
                {
                    path = path.Replace('/', '\\');
                    directory = Path.Combine(CurrentDir, path);
                }

                if (Directory.Exists(directory))
                {
                    CurrentDir = new DirectoryInfo(directory).FullName;

                    if (!CurrentDir.StartsWith(MyRootDir))
                    {
                        CurrentDir = MyRootDir;
                    }
                }
                else
                {
                    CurrentDir = MyRootDir;
                }
            }

            return "250 Changed to new directory\r\n";
        }
    }
}
