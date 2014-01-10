// *******************************************************
// * <copyright file="ClientConnection.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace FTPManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// </summary>
    public class ClientConnection
    {
        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public delegate void ProgressUpdateHandler(object sender, ProgressUpdateEventArgs e);

        #endregion

        #region Events

        /// <summary>
        /// </summary>
        public event ProgressUpdateHandler ProgressUpdate;

        #endregion

        #region Copy Stream Implementations

        /// <summary>
        /// </summary>
        /// <param name="input">
        /// </param>
        /// <param name="output">
        /// </param>
        /// <param name="bufferSize">
        /// </param>
        /// <returns>
        /// </returns>
        private static long CopyStream(Stream input, Stream output, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            long total = 0;

            while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, count);
                total += count;
            }

            return total;
        }

        /// <summary>
        /// </summary>
        /// <param name="input">
        /// </param>
        /// <param name="output">
        /// </param>
        /// <param name="bufferSize">
        /// </param>
        /// <returns>
        /// </returns>
        private static long CopyStreamAscii(Stream input, Stream output, int bufferSize)
        {
            char[] buffer = new char[bufferSize];
            int count = 0;
            long total = 0;

            using (StreamReader rdr = new StreamReader(input, Encoding.ASCII))
            {
                using (StreamWriter wtr = new StreamWriter(output, Encoding.ASCII))
                {
                    while ((count = rdr.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        wtr.Write(buffer, 0, count);
                        total += count;
                    }
                }
            }

            return total;
        }

        /// <summary>
        /// </summary>
        /// <param name="input">
        /// </param>
        /// <param name="output">
        /// </param>
        /// <returns>
        /// </returns>
        private long CopyStream(Stream input, Stream output)
        {
            Stream limitedStream = output;

            if (this.connectionType == TransferType.Image)
            {
                return CopyStream(input, limitedStream, 4096);
            }

            return CopyStreamAscii(input, limitedStream, 4096);
        }

        #endregion

        #region Enums

        /// <summary>
        /// </summary>
        private enum TransferType
        {
            /// <summary>
            /// </summary>
            Ascii, 

            /// <summary>
            /// </summary>
            Image
        }

        /// <summary>
        /// </summary>
        private enum FormatControlType
        {
            /// <summary>
            /// </summary>
            NonPrint
        }

        /// <summary>
        /// </summary>
        private enum DataConnectionType
        {
            /// <summary>
            /// </summary>
            Passive, 

            /// <summary>
            /// </summary>
            Active, 
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// </summary>
        private TcpListener passiveListener;

        /// <summary>
        /// </summary>
        private readonly TcpClient tcpClient;

        /// <summary>
        /// </summary>
        private TcpClient dataClient;

        /// <summary>
        /// </summary>
        private NetworkStream networkStream;

        /// <summary>
        /// </summary>
        private StreamReader dataReader;

        /// <summary>
        /// </summary>
        private TransferType connectionType = TransferType.Ascii;

        /// <summary>
        /// </summary>
        private DataConnectionType dataConnectionType = DataConnectionType.Active;

        /// <summary>
        /// </summary>
        private const string RootDirectory = "C:\\temp\\MainDirectory";

        /// <summary>
        /// </summary>
        private string currentDirectory = "C:\\temp\\MainDirectory";

        /// <summary>
        /// </summary>
        private IPEndPoint dataEndpoint;

        /// <summary>
        /// </summary>
        private IPEndPoint remoteEndPoint;

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        public ClientConnection(TcpClient client)
        {
            this.tcpClient = client;
        }

        #region Methods

        #region Event Methods

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        protected void OnProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (ProgressUpdate != null)
            {
                ProgressUpdate(this, e);
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        public void HandleClient(object obj)
        {
            remoteEndPoint = (IPEndPoint)this.tcpClient.Client.RemoteEndPoint;

            this.networkStream = this.tcpClient.GetStream();

            this.dataReader = new StreamReader(this.networkStream);

            this.SendMessage("220 Welcome to MDM's FTP server\r\n");

            this.dataClient = new TcpClient();

            try
            {
                string line;
                while ((line = this.dataReader.ReadLine()) != null)
                {
                    string response;

                    string[] command = line.Split(' ');

                    string cmd = command[0].ToUpperInvariant();
                    string arguments = command.Length > 1 ? line.Substring(command[0].Length + 1) : null;

                    if (arguments != null && arguments.Trim().Length == 0)
                    {
                        arguments = null;
                    }

                    switch (cmd)
                    {
                        case "USER":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("USER command received"));

                            response = "331 Username ok, need password";

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "PASS":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("PASS command received"));

                            response = "230 User logged in";

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "CWD":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("CWD command received"));

                            response = ChangeWorkingDirectory(arguments);

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "QUIT":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("QUIT command received"));

                            response = "221 Service closing control connection";

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "PORT":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("PORT command received"));

                            response = Port(arguments);

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "PASV":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("PASV command received"));

                            response = Passive();

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "TYPE":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("TYPE command received"));

                            response = Type(command[1], command.Length == 3 ? command[2] : null);

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "PWD":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("PWD command received"));

                            response = PrintWorkingDirectory();

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "RETR":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("RETR command received"));

                            response = Retrieve(arguments);

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "STOR":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("STOR command received"));

                            response = Store(arguments);

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "LIST":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("LIST command received"));

                            response = List(currentDirectory);

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "SYST":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("SYST command received"));

                            response = "215 WINDOWS";

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "FEAT":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("FEAT command received"));

                            response = "211 END";

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        case "EPRT":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("EPRT command received"));

                            response = EPort(arguments);

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;

                        default:
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Command not implemented"));

                            response = "502 Command not implemented";

                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Sending response: " + response));
                            break;
                    }

                    if (this.tcpClient == null || !this.tcpClient.Connected)
                    {
                        break;
                    }

                    this.SendMessage(response + "\r\n");

                    if (response.StartsWith("221"))
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnProgressUpdate(new ProgressUpdateEventArgs("Error occurred" + ex.Message));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsPathValid(string path)
        {
            return path.StartsWith(RootDirectory);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        private string NormalizeFilename(string path)
        {
            if (path == null)
            {
                path = string.Empty;
            }

            if (path == "/")
            {
                return RootDirectory;
            }

            path = path.StartsWith("/")
                       ? new FileInfo(Path.Combine(RootDirectory, path.Substring(1))).FullName
                       : new FileInfo(Path.Combine(currentDirectory, path)).FullName;

            return path;
        }

        /// <summary>
        /// </summary>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string ChangeWorkingDirectory(string pathName)
        {
            if (pathName == "/")
            {
                currentDirectory = RootDirectory;
            }
            else
            {
                string directory;

                if (pathName.StartsWith("/"))
                {
                    pathName = pathName.Substring(1).Replace('/', '\\');
                    directory = Path.Combine(RootDirectory, pathName);
                }
                else
                {
                    pathName = pathName.Replace('/', '\\');
                    directory = Path.Combine(currentDirectory, pathName);
                }

                if (Directory.Exists(directory))
                {
                    currentDirectory = new DirectoryInfo(directory).FullName;

                    if (!IsPathValid(currentDirectory))
                    {
                        currentDirectory = RootDirectory;
                    }
                }
                else
                {
                    currentDirectory = RootDirectory;
                }
            }

            return "250 Changed to new directory";
        }

        /// <summary>
        /// </summary>
        /// <param name="hostPort">
        /// </param>
        /// <returns>
        /// </returns>
        private string Port(string hostPort)
        {
            this.dataConnectionType = DataConnectionType.Active;

            string[] ipAndPort = hostPort.Split(',');

            byte[] ipAddress = new byte[4];
            byte[] port = new byte[2];

            for (int i = 0; i < 4; i++)
            {
                ipAddress[i] = Convert.ToByte(ipAndPort[i]);
            }

            for (int i = 4; i < 6; i++)
            {
                port[i - 4] = Convert.ToByte(ipAndPort[i]);
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(port);
            }

            dataEndpoint = new IPEndPoint(new IPAddress(ipAddress), BitConverter.ToInt16(port, 0));

            return "200 Data Connection Established";
        }

        /// <summary>
        /// </summary>
        /// <param name="hostPort">
        /// </param>
        /// <returns>
        /// </returns>
        private string EPort(string hostPort)
        {
            this.dataConnectionType = DataConnectionType.Active;

            char delimiter = hostPort[0];

            string[] rawSplit = hostPort.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            string ipAddress = rawSplit[1];
            string port = rawSplit[2];

            dataEndpoint = new IPEndPoint(IPAddress.Parse(ipAddress), int.Parse(port));

            return "200 Data Connection Established";
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string Passive()
        {
            this.dataConnectionType = DataConnectionType.Passive;

            IPAddress localIp = ((IPEndPoint)this.tcpClient.Client.LocalEndPoint).Address;

            this.passiveListener = new TcpListener(localIp, 0);
            this.passiveListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)this.passiveListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short port = (short)passiveListenerEndpoint.Port;

            byte[] portArray = BitConverter.GetBytes(port);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(portArray);
            }

            return string.Format(
                "227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", 
                address[0], 
                address[1], 
                address[2], 
                address[3], 
                portArray[0], 
                portArray[1]);
        }

        /// <summary>
        /// </summary>
        /// <param name="typeCode">
        /// </param>
        /// <param name="formatControl">
        /// </param>
        /// <returns>
        /// </returns>
        private string Type(string typeCode, string formatControl)
        {
            switch (typeCode.ToUpperInvariant())
            {
                case "A":
                    this.connectionType = TransferType.Ascii;
                    break;
                case "I":
                    this.connectionType = TransferType.Image;
                    break;
                default:
                    return "504 Command not implemented for that parameter";
            }
            
            return string.Format("200 Type set to {0}", this.connectionType);
        }

        /// <summary>
        /// </summary>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string Retrieve(string pathName)
        {
            pathName = NormalizeFilename(pathName);

            if (pathName != null)
            {
                if (File.Exists(pathName))
                {
                    var state = new DataConnectionOperation { Arguments = pathName, Operation = RetrieveOperation };

                    SetupDataConnectionOperation(state);

                    return string.Format("150 Opening {0} mode data transfer for RETR", this.dataConnectionType);
                }
            }

            return "550 File Not Found";
        }

        /// <summary>
        /// </summary>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string Store(string pathName)
        {
            pathName = NormalizeFilename(pathName);

            if (pathName != null)
            {
                var state = new DataConnectionOperation { Arguments = pathName, Operation = StoreOperation };

                SetupDataConnectionOperation(state);

                return string.Format("150 Opening {0} mode data transfer for STOR/STOU", this.dataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string PrintWorkingDirectory()
        {
            string current = currentDirectory.Replace(RootDirectory, string.Empty).Replace('\\', '/');

            if (current.Length == 0)
            {
                current = "/";
            }

            return string.Format("257 \"{0}\" is current directory.", current);
        }

        /// <summary>
        /// </summary>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string List(string pathName)
        {
            pathName = NormalizeFilename(pathName);

            if (pathName != null)
            {
                var state = new DataConnectionOperation { Arguments = pathName, Operation = ListOperation };

                SetupDataConnectionOperation(state);

                return string.Format("150 Opening {0} mode data transfer for LIST", this.dataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        #endregion

        #region DataConnection Operations

        /// <summary>
        /// </summary>
        /// <param name="result">
        /// </param>
        private void HandleAsyncResult(IAsyncResult result)
        {
            if (this.dataConnectionType == DataConnectionType.Active)
            {
                this.dataClient.EndConnect(result);
            }
            else
            {
                this.dataClient = this.passiveListener.EndAcceptTcpClient(result);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="state">
        /// </param>
        private void SetupDataConnectionOperation(DataConnectionOperation state)
        {
            if (this.dataConnectionType == DataConnectionType.Active)
            {
                this.dataClient = new TcpClient(dataEndpoint.AddressFamily);
                this.dataClient.BeginConnect(
                    dataEndpoint.Address, dataEndpoint.Port, this.DoDataConnectionOperationCallback, state);
            }
            else
            {
                this.passiveListener.BeginAcceptTcpClient(this.DoDataConnectionOperationCallback, state);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="result">
        /// </param>
        private void DoDataConnectionOperationCallback(IAsyncResult result)
        {
            this.HandleAsyncResult(result);

            DataConnectionOperation op = result.AsyncState as DataConnectionOperation;

            string response = null;

            using (NetworkStream dataStream = this.dataClient.GetStream())
            {
                if (op != null)
                {
                    response = op.Operation(dataStream, op.Arguments);
                }
            }

            this.dataClient.Close();
            this.dataClient = null;

            this.SendMessage(response + "\r\n");
        }

        /// <summary>
        /// </summary>
        /// <param name="dataStream">
        /// </param>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string RetrieveOperation(NetworkStream dataStream, string pathName)
        {
            long bytes = 0;

            using (FileStream fs = new FileStream(pathName, FileMode.Open, FileAccess.Read))
            {
                bytes = CopyStream(fs, dataStream);
            }

            return "226 Closing data connection, file transfer successful";
        }

        /// <summary>
        /// </summary>
        /// <param name="dataStream">
        /// </param>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string StoreOperation(NetworkStream dataStream, string pathName)
        {
            long bytes = 0;

            using (
                FileStream fs = new FileStream(
                    pathName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan)
                )
            {
                bytes = CopyStream(dataStream, fs);
            }

            return "226 Closing data connection, file transfer successful";
        }

        /// <summary>
        /// </summary>
        /// <param name="dataStream">
        /// </param>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string ListOperation(NetworkStream dataStream, string pathName)
        {
            StreamWriter dataWriter = new StreamWriter(dataStream, Encoding.ASCII);

            IEnumerable<string> directories = Directory.EnumerateDirectories(pathName);

            foreach (string dir in directories)
            {
                DirectoryInfo d = new DirectoryInfo(dir);

                string date = d.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180)
                                  ? d.LastWriteTime.ToString("MMM dd  yyyy")
                                  : d.LastWriteTime.ToString("MMM dd HH:mm");

                string line = string.Format("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, d.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            IEnumerable<string> files = Directory.EnumerateFiles(pathName);

            foreach (string file in files)
            {
                FileInfo f = new FileInfo(file);

                string date = f.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180)
                                  ? f.LastWriteTime.ToString("MMM dd  yyyy")
                                  : f.LastWriteTime.ToString("MMM dd HH:mm");

                string line = string.Format("-rw-r--r--    2 2003     2003     {0,8} {1} {2}", f.Length, date, f.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            return "226 Transfer complete";
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        internal void SendMessage(string message)
        {
            Thread currentThread = Thread.CurrentThread;

            lock (currentThread)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                networkStream.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion
    }
}