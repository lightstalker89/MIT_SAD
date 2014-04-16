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

    /// <summary>
    /// Data transfer type.
    /// </summary>
    enum SendType
    {
        Ascii,
        Image
    }

    public class Program
    {
        /// <summary>
        /// Client Connection
        /// </summary>
        private static TcpClient FTPClient { get; set; }

        /// <summary>
        /// Stream for sending commands to the client.
        /// </summary>
        private static NetworkStream FTPClientStream { get; set; }

        /// <summary>
        /// Listener which initializes the client connection.
        /// </summary>
        private static TcpListener FTPListener { get; set; }

        /// <summary>
        /// Listener which initializes the Data connection.
        /// </summary>
        private static TcpListener FTPPassiveListener { get; set; }

        /// <summary>
        /// Message Logger
        /// </summary>
        private static Logger Logger { get; set; }

        /// <summary>
        /// IPAdress of the client.
        /// </summary>
        private static IPAddress FTPClientIP { get; set; }

        /// <summary>
        /// My root directory.
        /// </summary>
        private static string MyRootDir = "C:\\temp\\FTPPath";

        /// <summary>
        /// The current directory.
        /// </summary>
        private static string CurrentDir { get; set; }

        /// <summary>
        /// The Data transfer type.
        /// </summary>
        private static SendType Type = SendType.Ascii;

        /// <summary>
        /// List of client threads
        /// </summary>
        private static List<Thread> ClientThreads { get; set; }

        /// <summary>
        /// main entry point method
        /// </summary>
        /// <param name="args">console arguments</param>
        public static void Main(string[] args)
        {
            // increase console window.
            Console.BufferHeight = 5000;
            Console.BufferWidth = 5000;

            //initialize client thread list
            ClientThreads = new List<Thread>();

            Logger = new Logger();

            //start server thread.
            Thread serverThread = new Thread(StartServer);

            //create Rootdir if not existing.
            if (!Directory.Exists(MyRootDir))
            {
                Directory.CreateDirectory(MyRootDir);
            }

            CurrentDir = MyRootDir;
            serverThread.Start();

            // is needed, otherwise application would close.
            Console.ReadKey();
        }

        /// <summary>
        /// start the server
        /// </summary>
        public static void StartServer()
        {
            // IPAddress Loopback is used for Local host. Use IPAdress.Any for other IP.
            FTPListener = new TcpListener(IPAddress.Loopback, 21);

            //start Client Command Listener
            FTPListener.Start();

            Logger.Log("Server started!");

            FTPListener.BeginAcceptTcpClient(HandleClient, FTPListener);

        }

        /// <summary>
        /// Asynchron handle method for client.
        /// </summary>
        /// <param name="result">Parameters from the asynchronous caller.</param>
        private static void HandleClient(IAsyncResult result)
        {
            FTPClient = FTPListener.EndAcceptTcpClient(result);
            //for new connection activate Listener!
            FTPListener.BeginAcceptTcpClient(HandleClient, FTPListener);

            // Get ip from the client.
            IPEndPoint ipEndPoint = FTPClient.Client.RemoteEndPoint as IPEndPoint;

            if (ipEndPoint != null)
            {
                FTPClientIP = ipEndPoint.Address;
            }

            //start client thread.
            Thread clientThread = new Thread(ListenToFTPClient);
            clientThread.Start();
            
        }

        /// <summary>
        /// Listening to ftp commands
        /// </summary>
        private static void ListenToFTPClient()
        {
            Logger.Log(string.Format("Client {0} connected", FTPClientIP));

            try
            {
                // get response stream from client.
                FTPClientStream = FTPClient.GetStream();

                SendMessage("220 Server ready\r\n");

                string clientMessage;

                while ((clientMessage = ReadMessage()) != null)
                {
                    Thread.Sleep(100);

                    // split response string. Divided into FTP Command and Arguments.
                    string[] splitResponse = clientMessage.Split(' ');
                    string ftpCommand = splitResponse[0].Length == 0 ? string.Empty : splitResponse[0].ToUpperInvariant();
                    
                    string ftpArgument = GetFTPArgumentFromResponse(splitResponse);

                    switch (ftpCommand)
                    {
                        case "USER":
                            Logger.Log(clientMessage);
                            SendMessage("331 Username ok, need password\r\n");
                            break;

                        case "PASS":
                            Logger.Log(clientMessage);
                            SendMessage("230 User logged in\r\n");
                            break;

                        case "QUIT":
                            Logger.Log(clientMessage);
                            QuitResponse();
                            SendMessage("221 Service closing control connection\r\n");
                            break;

                        case "DELE":
                            Logger.Log(clientMessage);
                            SendMessage(DeleteFileResponse(ftpArgument));
                            break;

                        case "RMD":
                            Logger.Log(clientMessage);
                            SendMessage(RemoveDirectoryResponse(ftpArgument));
                            break;

                        case "MKD":
                            Logger.Log(clientMessage);
                            SendMessage(MakeDirectoryResponse(ftpArgument));
                            break;

                        case "STOR":
                            Logger.Log(clientMessage);
                            StoreResponse(ftpArgument);
                            break;

                        case "LIST":
                            Logger.Log(clientMessage);
                            ListResponse(CurrentDir);
                            break;

                        case "SYST":
                            Logger.Log(clientMessage);
                            SendMessage("215 WINDOWS-NT-6\r\n");
                            break;

                        case "TYPE":
                            Logger.Log(clientMessage);
                            SendMessage(TypeResponse(ftpArgument));
                            break;

                        case "FEAT":
                            Logger.Log(clientMessage);
                            SendMessage("211 END\r\n");
                            break;

                        case "PWD":
                            Logger.Log(clientMessage);
                            SendMessage(string.Format("257 \"{0}\" is current directory.\r\n", GetCurrentWorkingDirectory()));
                            break;

                        case "PASV":
                            Logger.Log(clientMessage);
                            SendMessage(PassiveModeResponse());
                            break;

                        case "CWD":
                            Logger.Log(clientMessage);
                            SendMessage(ChangeWorkingDirectoryResponse(ftpArgument));
                            break;

                        case "RETR":
                            Logger.Log(clientMessage);
                            RetrieveResponse(ftpArgument);
                            break;
                        default:
                            SendMessage("502 Command not implemented\r\n");
                            break;
                    }
                }
            }
            catch (Exception)
            {
                Logger.Log("Something went wrong!!");
            }
        }

        /// <summary>
        /// Send response message to Client.
        /// </summary>
        /// <param name="message">the message which should be sent.</param>
        private static void SendMessage(string message)
        {
            // get current thread.
            Thread currentThread = Thread.CurrentThread;

            //lock thread for sending message. 
            lock (currentThread)
            {
                // write server response to the client stream.
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                FTPClientStream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// Read Message from client stream.
        /// </summary>
        /// <returns></returns>
        private static string ReadMessage()
        {
            StreamReader reader = new StreamReader(FTPClientStream);
            return reader.ReadLine();
        }

        /// <summary>
        /// Get the correct FTP Argument from client command.
        /// </summary>
        /// <param name="response">array of splitet response.</param>
        /// <returns>argument string</returns>
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

        /// <summary>
        /// Gets current working directory.
        /// </summary>
        /// <returns>path of current working directory.</returns>
        private static string GetCurrentWorkingDirectory()
        {
            // replace path with defined myRootDir.
            string workDir = CurrentDir.Replace(MyRootDir, string.Empty);

            if (workDir.Length == 0)
            {
                workDir = "/";
            }

            return workDir.Replace('\\', '/');
        }

        /// <summary>
        /// Handle the Type command.
        /// </summary>
        /// <param name="attribute">the type</param>
        /// <returns>Response message for client.</returns>
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

        /// <summary>
        /// Handle Passive mode client command.
        /// </summary>
        /// <returns></returns>
        private static string PassiveModeResponse()
        {
            //get client IP Adress.
            IPAddress localIp = ((IPEndPoint)FTPClient.Client.LocalEndPoint).Address;

            //initialize Data Connection Listener
            FTPPassiveListener = new TcpListener(localIp, 0);
            FTPPassiveListener.Start();

            //Get passive server IPAdress.
            IPEndPoint passiveListenerEndpoint = (IPEndPoint)FTPPassiveListener.LocalEndpoint;

            //get IPAdress in bytes.
            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();

            //Get free port.
            short port = (short)passiveListenerEndpoint.Port;

            //convert into port array.
            byte[] portArray = BitConverter.GetBytes(port);

            //Indicates the byte order for the computer architecture.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(portArray);
            }
            //port will be calculated with port1 * 256 + port2 = port.
            return string.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})\r\n", address[0], address[1], address[2], address[3], portArray[0], portArray[1]);
        }

        /// <summary>
        /// Handle for List command.
        /// </summary>
        /// <param name="dir">the directory path, which is needed for file list.</param>
        private static void ListResponse(string dir)
        {
            if (Directory.Exists(dir))
            {
                //start passive mode data transfer handle.
                SendMessage("150 Opening Passive mode data transfer for LIST\r\n");
                FTPPassiveListener.BeginAcceptTcpClient(HandleList, CurrentDir);
            }
            else
            {
                SendMessage("450 Requested file action not taken\r\n");
            }
        }

        /// <summary>
        /// Asynchronous handle method for List command.
        /// </summary>
        /// <param name="result"></param>
        private static void HandleList(IAsyncResult result)
        {
            //open passive listener
            TcpClient ftpDataClient = FTPPassiveListener.EndAcceptTcpClient(result);

            string pathName = (string)result.AsyncState;

            using (NetworkStream netStream = ftpDataClient.GetStream())
            {
                using (StreamWriter dataWriter = new StreamWriter(netStream, Encoding.ASCII))
                {
                    //get files and directories.
                    string[] directories = Directory.GetDirectories(pathName);
                    string[] files = Directory.GetFiles(pathName);

                    foreach (string dir in directories)
                    {
                        DirectoryInfo d = new DirectoryInfo(dir);

                        //Set data format to US. Otherwise problems with encoding.
                        string date = d.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                            d.LastWriteTime.ToString("MMM dd  yyyy", CultureInfo.CreateSpecificCulture("en-US")) :
                            d.LastWriteTime.ToString("MMM dd HH:mm", CultureInfo.CreateSpecificCulture("en-US"));

                        //format for line is important. Otherwise data cannot be transfered.
                        string line = string.Format("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, d.Name);
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
                }
            }

            //close data connection.
            ftpDataClient.Close();
            ftpDataClient = null;

            SendMessage("226 Transfer complete\r\n");
        }

        /// <summary>
        /// Change Working Directory method
        /// </summary>
        /// <param name="path">the directory path</param>
        /// <returns>response for client.</returns>
        private static string ChangeWorkingDirectoryResponse(string path)
        {
            //check if root dir.
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
                    // set new current dir.
                    CurrentDir = new DirectoryInfo(directory).FullName;

                    //fallback to root dir.
                    if (!CurrentDir.StartsWith(MyRootDir))
                    {
                        CurrentDir = MyRootDir;
                    }
                }
                else
                {
                    //fallback to root dir.
                    CurrentDir = MyRootDir;
                }
            }

            return "250 Changed to new directory\r\n";
        }

        /// <summary>
        /// Handle for store command.
        /// </summary>
        /// <param name="filename">the filename of the file which should be saved</param>
        private static void StoreResponse(string filename)
        {
            //get full path name.
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

        /// <summary>
        /// Handle asynchronous store method
        /// </summary>
        /// <param name="result">Asynchornous object with data</param>
        private static void HandleStore(IAsyncResult result)
        {
            //initialize passive conection
            TcpClient ftpDataClient = FTPPassiveListener.EndAcceptTcpClient(result);

            string path = (string)result.AsyncState;

            using (NetworkStream dataStream = ftpDataClient.GetStream())
            {
                //read data from client file with FileStream.
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

            //close data connection
            ftpDataClient.Close();
            ftpDataClient = null;

            SendMessage("226 Closing data connection, file transfer successful\r\n");
        }

        /// <summary>
        /// Response method for deleting file on server.
        /// </summary>
        /// <param name="filePath">file to delete</param>
        /// <returns>response message</returns>
        private static string DeleteFileResponse(string filePath)
        {
            //get full path.
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

        /// <summary>
        /// Delete directory on server.
        /// </summary>
        /// <param name="dirPath">directory path</param>
        /// <returns>response message</returns>
        private static string RemoveDirectoryResponse(string dirPath)
        {
            //get full path.
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

        /// <summary>
        /// Make Directory response method
        /// </summary>
        /// <param name="dirName">name of directory</param>
        /// <returns>response message</returns>
        private static string MakeDirectoryResponse(string dirName)
        {
            //get full path.
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

        /// <summary>
        /// Retrieve response method
        /// </summary>
        /// <param name="fileName">filename of file which should be send to client.</param>
        private static void RetrieveResponse(string fileName)
        {
            //get full path.
            string fullPath = string.Format("{0}\\{1}", CurrentDir, fileName);

            if (File.Exists(fullPath))
            {
                //send message for indicating, that passive mode data transfer connection will be activated.
                SendMessage("150 Opening Passive mode data transfer for RETR\r\n");
                FTPPassiveListener.BeginAcceptTcpClient(HandleRetrieve, fullPath);
            }
            else
            {
                SendMessage("550 File Not Found\r\n");
            }
        }

        /// <summary>
        /// Method for handling asynchronous retrieve method.
        /// </summary>
        /// <param name="result">asynchronous object with data</param>
        private static void HandleRetrieve(IAsyncResult result)
        {
            //activate data connection
            TcpClient ftpDataClient = FTPPassiveListener.EndAcceptTcpClient(result);

            //get file path.
            string path = (string)result.AsyncState;

            //open Networkstream
            using (NetworkStream dataStream = ftpDataClient.GetStream())
            {
                //Read file 
                using (FileStream fStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    
                    int currentLength = 0;

                    //check for send type.
                    if (Type == SendType.Image)
                    {
                        //buffer size for sending
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
                            //set writer to ascii encoding
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

            //close data connection.
            ftpDataClient.Close();
            ftpDataClient = null;

            SendMessage("226 Closing data connection, file transfer successfull\r\n");
        }

        /// <summary>
        /// 
        /// </summary>
        static private void QuitResponse()
        {
            foreach (Thread client in ClientThreads)
            {
                client.Abort();
            }
        }
    }
}
