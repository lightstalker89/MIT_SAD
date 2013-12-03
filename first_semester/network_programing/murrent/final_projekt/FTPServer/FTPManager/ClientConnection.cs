using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FTPManager
{
    public class ClientConnection
    {
        /// <summary>
        /// </summary>
        private sealed class DataConnectionOperation
        {
            /// <summary>
            /// </summary>
            public Func<NetworkStream, string, string> Operation { get; set; }

            /// <summary>
            /// </summary>
            public string Arguments { get; set; }
        }

        #region Delegates

        public delegate void ProgressUpdateHandler(object sender, ProgressUpdateEventArgs e);

        #endregion

        #region Events

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
            else
            {
                return CopyStreamAscii(input, limitedStream, 4096);
            }
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
            Ebcdic,

            /// <summary>
            /// </summary>
            Image,

            /// <summary>
            /// </summary>
            Local,
        }

        /// <summary>
        /// </summary>
        private enum FormatControlType
        {
            /// <summary>
            /// </summary>
            NonPrint,

            /// <summary>
            /// </summary>
            Telnet,

            /// <summary>
            /// </summary>
            CarriageControl,
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
        private StreamWriter dataWriter;

        /// <summary>
        /// </summary>
        private TransferType connectionType = TransferType.Ascii;

        /// <summary>
        /// </summary>
        private FormatControlType formatControlType = FormatControlType.NonPrint;

        /// <summary>
        /// </summary>
        private DataConnectionType dataConnectionType = DataConnectionType.Active;

        /// <summary>
        /// </summary>
        private const string RootDirectory = "C:\\temp";

        /// <summary>
        /// </summary>
        private string currentDirectory = "C:\\temp";

        /// <summary>
        /// </summary>
        private IPEndPoint dataEndpoint;

        /// <summary>
        /// </summary>
        private IPEndPoint remoteEndPoint;

        /// <summary>
        /// </summary>
        private string clientIp;
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

            clientIp = remoteEndPoint.Address.ToString();

            this.networkStream = this.tcpClient.GetStream();

            this.dataReader = new StreamReader(this.networkStream);
            this.dataWriter = new StreamWriter(this.networkStream);

            this.dataWriter.WriteLine("220 Welcome to MDM's FTP server");
            this.dataWriter.Flush();

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
                            break;

                        case "PASS":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("PASS command received"));

                            response = "230 User logged in";
                            break;

                        case "CWD":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("CWD command received"));

                            response = ChangeWorkingDirectory(arguments);
                            break;

                        case "QUIT":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("QUIT command received"));

                            response = "221 Service closing control connection";
                            break;

                        case "PORT":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("PORT command received"));

                            response = Port(arguments);
                            break;

                        case "PASV":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("PASV command received"));

                            response = Passive();
                            break;

                        case "TYPE":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("TYPE command received"));

                            response = Type(command[1], command.Length == 3 ? command[2] : null);
                            break;

                        case "DELE":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("DELETE command received"));

                            response = Delete(arguments);
                            break;

                        case "RMD":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("RMD command received"));

                            response = RemoveDir(arguments);
                            break;

                        case "MKD":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("MKD command received"));

                            response = CreateDir(arguments);
                            break;

                        case "PWD":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("PWD command received"));

                            response = PrintWorkingDirectory();
                            break;

                        case "RETR":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("RETR command received"));

                            response = Retrieve(arguments);
                            break;

                        case "STOR":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("STOR command received"));

                            response = Store(arguments);
                            break;

                        case "STOU":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("STOU command received"));

                            response = StoreUnique();
                            break;

                        case "LIST":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("LIST command received"));

                            response = List(arguments ?? currentDirectory);
                            break;

                        case "SYST":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("SYST command received"));

                            response = "215 WINDOWS";
                            break;

                        case "FEAT":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("FEAT command received"));

                            response = FeatureList();
                            break;

                        case "EPRT":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("EPRT command received"));

                            response = EPort(arguments);
                            break;

                        case "EPSV":
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("EPSV command received"));

                            response = EPassive();
                            break;

                        default:
                            this.OnProgressUpdate(new ProgressUpdateEventArgs("Command not implemented"));

                            response = "502 Command not implemented";
                            break;
                    }

                    if (this.tcpClient == null || !this.tcpClient.Connected)
                    {
                        break;
                    }

                    this.dataWriter.WriteLine(response);
                    this.dataWriter.Flush();

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

            path = path.StartsWith("/") ? new FileInfo(Path.Combine(RootDirectory, path.Substring(1))).FullName : new FileInfo(Path.Combine(currentDirectory, path)).FullName;

            return IsPathValid(path) ? path : null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string FeatureList()
        {
            this.dataWriter.WriteLine("211- Extensions supported:");
            this.dataWriter.WriteLine(" MDTM");
            this.dataWriter.WriteLine(" SIZE");
            return "211 End";
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
                string newDir;

                if (pathName.StartsWith("/"))
                {
                    pathName = pathName.Substring(1).Replace('/', '\\');
                    newDir = Path.Combine(RootDirectory, pathName);
                }
                else
                {
                    pathName = pathName.Replace('/', '\\');
                    newDir = Path.Combine(currentDirectory, pathName);
                }

                if (Directory.Exists(newDir))
                {
                    currentDirectory = new DirectoryInfo(newDir).FullName;

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
        /// <returns>
        /// </returns>
        private string EPassive()
        {
            this.dataConnectionType = DataConnectionType.Passive;

            IPAddress localIp = ((IPEndPoint)this.tcpClient.Client.LocalEndPoint).Address;

            this.passiveListener = new TcpListener(localIp, 0);
            this.passiveListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)this.passiveListener.LocalEndpoint;

            return string.Format("229 Entering Extended Passive Mode (|||{0}|)", passiveListenerEndpoint.Port);
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

            if (!string.IsNullOrWhiteSpace(formatControl))
            {
                switch (formatControl.ToUpperInvariant())
                {
                    case "N":
                        this.formatControlType = FormatControlType.NonPrint;
                        break;
                    default:
                        return "504 Command not implemented for that parameter";
                }
            }

            return string.Format("200 Type set to {0}", this.connectionType);
        }

        /// <summary>
        /// </summary>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string Delete(string pathName)
        {
            pathName = NormalizeFilename(pathName);

            if (pathName != null)
            {
                if (File.Exists(pathName))
                {
                    File.Delete(pathName);
                }
                else
                {
                    return "550 File Not Found";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 File Not Found";
        }

        /// <summary>
        /// </summary>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string RemoveDir(string pathName)
        {
            pathName = NormalizeFilename(pathName);

            if (pathName != null)
            {
                if (Directory.Exists(pathName))
                {
                    Directory.Delete(pathName);
                }
                else
                {
                    return "550 Directory Not Found";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 Directory Not Found";
        }

        /// <summary>
        /// </summary>
        /// <param name="pathName">
        /// </param>
        /// <returns>
        /// </returns>
        private string CreateDir(string pathName)
        {
            pathName = NormalizeFilename(pathName);

            if (pathName != null)
            {
                if (!Directory.Exists(pathName))
                {
                    Directory.CreateDirectory(pathName);
                }
                else
                {
                    return "550 Directory already exists";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 Directory Not Found";
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

                return string.Format("150 Opening {0} mode data transfer for STOR", this.dataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string StoreUnique()
        {
            string pathName = NormalizeFilename(new Guid().ToString());

            var state = new DataConnectionOperation { Arguments = pathName, Operation = StoreOperation };

            SetupDataConnectionOperation(state);

            return string.Format("150 Opening {0} mode data transfer for STOU", this.dataConnectionType);
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
                this.dataClient.BeginConnect(dataEndpoint.Address, dataEndpoint.Port, DoDataConnectionOperation, state);
            }
            else
            {
                this.passiveListener.BeginAcceptTcpClient(DoDataConnectionOperation, state);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="result">
        /// </param>
        private void DoDataConnectionOperation(IAsyncResult result)
        {
            HandleAsyncResult(result);

            DataConnectionOperation op = result.AsyncState as DataConnectionOperation;

            string response = null;

            using (NetworkStream dataStream = this.dataClient.GetStream())
            {
                if (op != null) response = op.Operation(dataStream, op.Arguments);
            }

            this.dataClient.Close();
            this.dataClient = null;

            this.dataWriter.WriteLine(response);
            this.dataWriter.Flush();
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

        #endregion
    }
}
