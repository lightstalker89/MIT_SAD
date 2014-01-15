using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Globalization;

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
            // is needed, otherwise application would close.
            Console.ReadKey();
        }

        public static void StartServer()
        {
            // IPAddress Loopback is used for Local host.
            FTPListener = new TcpListener(IPAddress.Loopback, 21);
            FTPListener.Start();

            Logger.Log("Server started!");

            FTPListener.BeginAcceptTcpClient(HandleClient, FTPListener);

        }

        private static void HandleClient(IAsyncResult result)
        {
            FTPClient = FTPListener.EndAcceptTcpClient(result);
            //for new connection activate Listener!
            FTPListener.BeginAcceptTcpClient(HandleClient, FTPListener);

            IPEndPoint ipEndPoint = FTPClient.Client.RemoteEndPoint as IPEndPoint;

            if (ipEndPoint != null)
            {
                FTPClientIP = ipEndPoint.Address;
            }

            Thread clientThread = new Thread(ListenToFTPClient);
            clientThread.Start();
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

                        case "QUIT":
                            SendMessage("221 Service closing control connection\r\n");
                            break;

                        case "DELE":
                            SendMessage(DeleteFileResponse(ftpArgument));
                            break;

                        case "RMD":
                            SendMessage(RemoveDirectoryResponse(ftpArgument));
                            break;

                        case "PORT":
                            break;

                        case "MKD":
                            SendMessage(MakeDirectoryResponse(ftpArgument));
                            break;

                        case "STOR":
                            StoreResponse(ftpArgument);
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
                            SendMessage("211 END\r\n");
                            break;

                        case "PWD":
                            SendMessage(string.Format("257 \"{0}\" is current directory.\r\n", GetCurrentWorkingDirectory()));
                            break;

                        case "PASV":
                            SendMessage(PassiveModeResponse());
                            break;

                        case "CWD":
                            SendMessage(ChangeWorkingDirectoryResponse(ftpArgument));
                            break;

                        case "RETR":
                            RetrieveResponse(ftpArgument);
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
                return "";
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
                    d.LastWriteTime.ToString("MMM dd  yyyy", CultureInfo.CreateSpecificCulture("en-US")) :
                    d.LastWriteTime.ToString("MMM dd HH:mm", CultureInfo.CreateSpecificCulture("en-US"));

                string line = string.Format("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, d.Name);
                Console.WriteLine(line);
                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            foreach (string file in files)
            {
                FileInfo f = new FileInfo(file);

                string date = f.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                    f.LastWriteTime.ToString("MMM dd  yyyy", CultureInfo.CreateSpecificCulture("en-US")) :
                    f.LastWriteTime.ToString("MMM dd HH:mm", CultureInfo.CreateSpecificCulture("en-US"));

                string line = string.Format("-rw-r--r--    2 2003     2003     {0,8} {1} {2}", f.Length, date, f.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            netStream.Close();
            netStream.Dispose();

            dataWriter.Close();
            dataWriter.Dispose();

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

        private static void StoreResponse(string filename)
        {
            string path = string.Format("{0}\\{1}", CurrentDir, filename);

            if (path != null)
            {
                SendMessage("150 Opening Passive mode data transfer for STOR/STOU\r\n");
                FTPPassiveListener.BeginAcceptTcpClient(HandleStore, path);
            }
            else
            {
                SendMessage("450 Requested file action not taken\r\n");
            }
        }

        private static void HandleStore(IAsyncResult result)
        {
            TcpClient ftpDataClient = FTPPassiveListener.EndAcceptTcpClient(result);

            string path = (string)result.AsyncState;

            using (NetworkStream dataStream = ftpDataClient.GetStream())
            {
                using (FileStream fStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    byte[] buffer = new byte[4096];
                    int currentLength = 0;

                    while ((currentLength = dataStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fStream.Write(buffer, 0, currentLength);
                    }
                }
            }

            ftpDataClient.Close();
            ftpDataClient = null;

            SendMessage("226 Closing data connection, file transfer successful\r\n");
        }

        private static string DeleteFileResponse(string filePath)
        {
            string fullPath = string.Format("{0}\\{1}", CurrentDir, filePath);

            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return "250 Requested file action okay, completed\r\n";
                }
            }
            catch (Exception)
            {
                return "550 Error deleting File\r\n";
            }


            return "550 File Not Found\r\n";
        }

        private static string RemoveDirectoryResponse(string dirPath)
        {
            string fullPath = string.Format("{0}\\{1}", CurrentDir, dirPath);

            try
            {
                if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath);
                    return "250 Requested directory action okay, completed\r\n";
                }
            }
            catch (Exception)
            {
                return "550 Error deleting Directory\r\n";
            }


            return "550 directory Not Found\r\n";
        }

        private static string MakeDirectoryResponse(string dirName)
        {
            string fullPath = string.Format("{0}\\{1}", CurrentDir, dirName);

            try
            {
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                    return "250 Requested directory action okay, completed\r\n";
                }
            }
            catch (Exception)
            {
                return "550 Error creating Directory\r\n";
            }


            return "550 Directory already exists\r\n";
        }

        private static void RetrieveResponse(string fileName)
        {
            string fullPath = string.Format("{0}\\{1}", CurrentDir, fileName);

            if (File.Exists(fullPath))
            {
                SendMessage("150 Opening Passive mode data transfer for RETR\r\n");
                FTPPassiveListener.BeginAcceptTcpClient(HandleRetrieve, fullPath);
            }

            SendMessage("550 File Not Found\r\n");
        }

        private static void HandleRetrieve(IAsyncResult result)
        {
            TcpClient ftpDataClient = FTPPassiveListener.EndAcceptTcpClient(result);

            string path = (string)result.AsyncState;

            using (NetworkStream dataStream = ftpDataClient.GetStream())
            {
                using (FileStream fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    
                    int currentLength = 0;

                    if (Type == SendType.Image)
                    {
                        byte[] buffer = new byte[4096];
                        while ((currentLength = dataStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fStream.Write(buffer, 0, currentLength);
                        }
                    }
                    else
                    {
                        char[] buffer = new char[4096];
                        using (StreamReader strReader = new StreamReader(fStream))
                        {
                            using (StreamWriter strWriter = new StreamWriter(dataStream, Encoding.ASCII))
                            {
                                while ((currentLength = strReader.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    strWriter.Write(buffer, 0, currentLength);
                                }
                            }
                        }
                    }
                }
            }

            ftpDataClient.Close();
            ftpDataClient = null;

            SendMessage("226 Closing data connection, file transfer successful\r\n");
        }
    }
}
