// *******************************************************
// * <copyright file="ClientConnection.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace FTPServer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// Represents the ClientConnection class which contains the entry point of the application.
    /// </summary>
    public class ClientConnection
    {
        /// <summary>
        /// The root directory on FTP server
        /// </summary>
        private readonly string rootDirOnSystem = Path.Combine("C:\\", "temp", "MainDirectory");

        /// <summary>
        /// The culture info for the date format
        /// </summary>
        private readonly CultureInfo ci = new CultureInfo("en-GB");

        /// <summary>
        /// The IPEndPoint which we received from the port command form the ftp server
        /// </summary>
        private IPEndPoint dataEndpoint; //PORT

        /// <summary>
        /// The passive listener which will be generated in the passive
        /// command form the ftp server
        /// </summary>
        private TcpListener passiveListener; //Passive

        #region control connection objects
        /// <summary>
        /// The TCP client instance which we received as parameter.
        /// Handles the control communication with the ftp client
        /// </summary>
        private TcpClient controlClient;

        /// <summary>
        /// The stream reader from the controlClient
        /// </summary>
        private StreamReader controlReader;

        /// <summary>
        /// The stream writer from the controlClient
        /// </summary>
        private StreamWriter controlWriter;
        #endregion

        #region data connection objects
        /// <summary>
        /// The TCP client instance which we get from the passive command.
        /// Handles the file communication with the ftp client
        /// </summary>
        private TcpClient dataClient;

        /// <summary>
        /// The stream reader from the dataClient
        /// </summary>
        private StreamReader dataReader;

        /// <summary>
        /// The stream writer from the dataClient
        /// </summary>
        private StreamWriter dataWriter;
        #endregion

        /// <summary>
        /// The name, which are logged in.
        /// </summary>
        private string username;

        /// <summary>
        /// The connection transfer type, standard value is ASCII
        /// </summary>
        private TransferType connectionTransferType = TransferType.Ascii;

        /// <summary>
        /// The data connection type, standard value is Active
        /// </summary>
        private DataConnectionType dataConnectionType = DataConnectionType.Active;

        /// <summary>
        /// The current directory on FTP server
        /// </summary>
        private string currentDirOnSystem = Path.Combine("C:\\", "temp", "MainDirectory");


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnection"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ClientConnection(TcpClient client)
        {
            this.controlClient = client;

            NetworkStream controlStream = this.controlClient.GetStream();

            this.controlReader = new StreamReader(controlStream);
            this.controlWriter = new StreamWriter(controlStream);
            Directory.CreateDirectory(this.rootDirOnSystem);
        }

        /// <summary>
        /// Event for any message.
        /// </summary>
        public event EventHandler<MessageEventArgs> Message;

        #region handle client

        /// <summary>
        /// Handles the incoming commands
        /// </summary>
        /// <param name="obj">The object.</param>
        public void HandleClient(object obj)
        {
            this.controlWriter.WriteLine("220 Service Ready.");
            this.controlWriter.Flush();

            try
            {
                string line;
                while (!string.IsNullOrEmpty(line = this.controlReader.ReadLine()))
                {
                    string[] command = line.Split(' ');

                    string cmd = command[0].ToUpperInvariant();
                    string arguments = command.Length > 1 ? line.Substring(command[0].Length + 1) : string.Empty;

                    string response = null;
                    switch (cmd)
                    {
                        case "USER":
                            this.Message(this, new MessageEventArgs("USER command received"));
                            response = this.User(arguments);
                            break;
                        case "PASS":
                            this.Message(this, new MessageEventArgs("PASS command received"));
                            response = this.Password(arguments);
                            break;
                        case "PWD":
                            this.Message(this, new MessageEventArgs("PWD command received"));
                            response = this.PrintWorkingDirectory();
                            break;
                        case "TYPE":
                            this.Message(this, new MessageEventArgs("TYPE command received"));
                            string[] splitArgs = arguments.Split(' ');
                            response = this.Type(splitArgs[0], splitArgs.Length > 1 ? splitArgs[1] : null);
                            break;
                        case "PASV":
                            this.Message(this, new MessageEventArgs("PASV command received"));
                            response = this.Passive();
                            break;
                        case "LIST":
                            this.Message(this, new MessageEventArgs("LIST command received"));
                            response = this.List(this.currentDirOnSystem);
                            break;
                        case "CWD":
                            this.Message(this, new MessageEventArgs("CWD command received"));
                            response = this.ChangeWorkingDirectory(arguments);
                            break;
                        case "PORT":
                            this.Message(this, new MessageEventArgs("PORT command received"));
                            response = this.Port(arguments);
                            break;
                        case "RETR":
                            this.Message(this, new MessageEventArgs("RETR command received"));
                            response = this.Retrieve(arguments);
                            break;
                        case "STOR":
                            this.Message(this, new MessageEventArgs("STOR command received"));
                            response = this.Store(arguments);
                            break;
                        case "MKD":
                            this.Message(this, new MessageEventArgs("MKD command received"));
                            response = this.CreateDir(arguments);
                            break;
                        case "DELE":
                            this.Message(this, new MessageEventArgs("DELE command received"));
                            response = this.Delete(arguments);
                            break;
                        case "RMD":
                            this.Message(this, new MessageEventArgs("RMD command received"));
                            response = this.RemoveDirectory(arguments);
                            break;
                        case "QUIT":
                            this.Message(this, new MessageEventArgs("QUIT command received"));
                            response = "221 Service closing control connection";
                            break;
                        default:
                            this.Message(this, new MessageEventArgs(string.Concat(cmd, " unknown command received")));
                            response = "502 Command not implemented";

                            // Not implemented commands
                            // SYST: Command is used to find out the operating system of the server.
                            // CDUP: Special case of CWD, not needed.
                            // FEAT: This command causes the FTP server to list all new FTP features that the server supports beyond those described in RFC 959. Not needed.
                            // RNFR: This command specifies the old pathname of the file which is to be renamed. This command must be immediately followed by a RNTO
                            // RNTO: This command specifies the new pathname of the file specified in the immediately preceding RNFR command. Together the two commands cause a file to be renamed.
                            break;
                    }

                    if (this.controlClient == null || !this.controlClient.Connected)
                    {
                        break;
                    }
                    else
                    {
                        this.controlWriter.WriteLine(response);
                        this.controlWriter.Flush();

                        if (response.StartsWith("221"))
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region FTP Commands

        #region user
        /// <summary>
        /// Accept all users which wants to log in.
        /// </summary>
        /// <param name="name">The user name which wants to log in.</param>
        /// <returns>The telnet message</returns>
        private string User(string name)
        {
            this.username = name;
            return "331 Username ok, need password";
        }
        #endregion

        #region password
        /// <summary>
        /// Accept every password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The telnet message</returns>
        private string Password(string password)
        {
            return "230 User logged in";
        }
        #endregion

        #region print working directory
        /// <summary>
        /// Prints the working directory.
        /// </summary>
        /// <returns>The telnet message</returns>
        private string PrintWorkingDirectory()
        {
            string current = this.currentDirOnSystem.Replace(this.rootDirOnSystem, string.Empty).Replace('\\', '/');

            if (current.Length == 0)
            {
                current = "/";
            }

            return string.Format("257 \"{0}\" is current directory.", current);
        }
        #endregion

        #region type
        /// <summary>
        /// Set the encoding which is received from the client. ( The way we will transfer data )
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <param name="formatControl">The format control.</param>
        /// <returns>The telnet message</returns>
        private string Type(string typeCode, string formatControl)
        {
            switch (typeCode)
            {
                case "A":
                    this.connectionTransferType = TransferType.Ascii;
                    break;
                case "I":
                    this.connectionTransferType = TransferType.Image;
                    break;
                default:
                    return "504 Command not implemented for that parameter.";
            }

            return string.Format("200 Type set to {0}", this.connectionTransferType);
        }
        #endregion

        #region passive
        /// <summary>
        /// Tells the server to open a port to listen on and send it back to the client.
        /// </summary>
        /// <returns>The telnet message</returns>
        private string Passive()
        {
            this.dataConnectionType = DataConnectionType.Passive;

            IPAddress localIp = ((IPEndPoint)this.controlClient.Client.LocalEndPoint).Address;

            // Choose a port which is free.
            // It will return an available port between 1024 and 5000.
            this.passiveListener = new TcpListener(localIp, 0);
            this.passiveListener.Start();

            // Send client back what ip address and port we are listening
            IPEndPoint passiveListenerEndpoint = (IPEndPoint)this.passiveListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short port = (short)passiveListenerEndpoint.Port;

            byte[] portArray = BitConverter.GetBytes(port);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(portArray);
            }

            return string.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", address[0], address[1], address[2], address[3], portArray[0], portArray[1]);
        }
        #endregion

        #region list
        /// <summary>
        /// Sends back a list of file system entries for the specified directory.
        /// </summary>
        /// <param name="currentDirectory">The current directory.</param>
        /// <returns>The telnet message</returns>
        private string List(string currentDirectory)
        {
            if (Directory.Exists(currentDirectory))
            {
                if (this.dataConnectionType == DataConnectionType.Active)
                {
                    // We are only using passive type
                }
                else
                {
                    this.passiveListener.BeginAcceptTcpClient(this.DoList, currentDirectory);
                }

                return string.Format("150 Opening {0} mode data transfer for LIST", this.dataConnectionType);
            }

            return "450 Requested file action not taken";
        }
        #endregion

        #region change working directory
        /// <summary>
        /// Changes the working directory.
        /// </summary>
        /// <param name="pathName">Name of the path.</param>
        /// <returns>The telnet message</returns>
        private string ChangeWorkingDirectory(string pathName)
        {
            if (pathName == "/")
            {
                this.currentDirOnSystem = this.rootDirOnSystem;
            }
            else
            {
                string newDir;

                if (pathName.StartsWith("/"))
                {
                    pathName = pathName.Substring(1).Replace('/', '\\');
                    newDir = Path.Combine(this.rootDirOnSystem, pathName);
                }
                else
                {
                    pathName = pathName.Replace('/', '\\');
                    newDir = Path.Combine(this.currentDirOnSystem, pathName);
                }

                if (Directory.Exists(newDir))
                {
                    this.currentDirOnSystem = new DirectoryInfo(newDir).FullName;

                    if (!this.IsPathValid(this.currentDirOnSystem))
                    {
                        this.currentDirOnSystem = this.rootDirOnSystem;
                    }
                }
                else
                {
                    this.currentDirOnSystem = this.rootDirOnSystem;
                }
            }

            return "250 Changed to new directory";
        }
        #endregion

        #region port
        /// <summary>
        /// This command tells the server to connect to a given IP address and port.
        /// In this method we will get our data endpoint.
        /// </summary>
        /// <param name="hostPort">The host port.</param>
        /// <returns>The telnet message</returns>
        private string Port(string hostPort)
        {
            this.dataConnectionType = DataConnectionType.Active;

            string[] portAndIp = hostPort.Split(',');

            byte[] address = new byte[4];
            byte[] port = new byte[2];

            for (int i = 0; i < 4; i++)
            {
                address[i] = Convert.ToByte(portAndIp[i]);
            }

            for (int i = 4; i < 6; i++)
            {
                port[i - 4] = Convert.ToByte(portAndIp[i]);
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(port);
            }

            this.dataEndpoint = new IPEndPoint(new IPAddress(address), BitConverter.ToInt16(port, 0));

            return "200 Data Connection Established";
        }
        #endregion

        #region retrieve
        /// <summary>
        /// Retrieves the specified file from the server.
        /// </summary>
        /// <param name="pathname">The pathname.</param>
        /// <returns>The telnet message</returns>
        private string Retrieve(string pathname)
        {
            pathname = Path.Combine(this.currentDirOnSystem, pathname);
            if (File.Exists(pathname))
            {
                if (this.dataConnectionType == DataConnectionType.Active)
                {
                    // We are only using passive type
                }
                else
                {
                    this.passiveListener.BeginAcceptTcpClient(this.DoRetrieve, pathname);
                }

                return string.Format("150 Opening {0} mode data transfer for RETR", this.dataConnectionType);
            }

            return "550 File Not Found";
        }
        #endregion

        #region store
        /// <summary>
        /// Save a file to the server
        /// </summary>
        /// <param name="pathname">The pathname.</param>
        /// <returns>The telnet message</returns>
        private string Store(string pathname)
        {
            pathname = Path.Combine(this.currentDirOnSystem, pathname);

            if (pathname != string.Empty)
            {
                if (this.dataConnectionType == DataConnectionType.Active)
                {
                    // We are only using passive type
                }
                else
                {
                    this.passiveListener.BeginAcceptTcpClient(this.DoStore, pathname);
                }

                return string.Format("150 Opening {0} mode data transfer for STOR", this.dataConnectionType);
            }

            return "450 Requested file action not taken";
        }
        #endregion

        #region create directory
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="pathName">Name of the path.</param>
        /// <returns>The telnet message</returns>
        private string CreateDir(string pathName)
        {
            pathName = this.NormalizeFilename(pathName);

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
        #endregion

        #region delete
        /// <summary>
        /// Deletes the specific file.
        /// </summary>
        /// <param name="pathName">Name of the path.</param>
        /// <returns>The telnet message</returns>
        private string Delete(string pathName)
        {
            pathName = this.NormalizeFilename(pathName);

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
        #endregion

        #region remove directory
        /// <summary>
        /// Removes the specific directory.
        /// </summary>
        /// <param name="pathName">Name of the path.</param>
        /// <returns>The telnet message</returns>
        private string RemoveDirectory(string pathName)
        {
            pathName = this.NormalizeFilename(pathName);

            if (pathName != null)
            {
                if (Directory.Exists(pathName))
                {
                    Directory.Delete(pathName);
                }
                else
                {
                    return "550 File Not Found";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 File Not Found";
        }
        #endregion

        #endregion

        #region ftp command helpers

        #region directory list
        /// <summary>
        /// Does the list.
        /// </summary>
        /// <param name="result">The result.</param>
        private void DoList(IAsyncResult result)
        {
            if (this.dataConnectionType == DataConnectionType.Active)
            {
                // We are only using passive type
            }
            else
            {
                this.dataClient = this.passiveListener.EndAcceptTcpClient(result);
            }

            string pathname = (string)result.AsyncState;

            using (NetworkStream dataStream = this.dataClient.GetStream())
            {
                this.dataReader = new StreamReader(dataStream, Encoding.ASCII);
                this.dataWriter = new StreamWriter(dataStream, Encoding.ASCII);

                IEnumerable<string> directories = Directory.GetDirectories(pathname);

                foreach (string dir in directories)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);

                    string date = d.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                        d.LastWriteTime.ToString("MMM dd  yyyy", this.ci) :
                        d.LastWriteTime.ToString("MMM dd HH:mm", this.ci);

                    string line = string.Format("drwxr-xr-x    2 2003     2003     {0} {1} {2}", "0", date, d.Name);

                    this.dataWriter.WriteLine(line);
                    this.dataWriter.Flush();
                }

                IEnumerable<string> files = Directory.GetFiles(pathname);

                foreach (string file in files)
                {
                    FileInfo f = new FileInfo(file);

                    string date = f.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                        f.LastWriteTime.ToString("MMM dd  yyyy", this.ci) :
                        f.LastWriteTime.ToString("MMM dd HH:mm", this.ci);

                    string line = string.Format("-rw-r--r--    2 2003     2003     {0} {1} {2}", f.Length, date, f.Name);

                    this.dataWriter.WriteLine(line);
                    this.dataWriter.Flush();
                }

                this.dataClient.Close();
                this.dataClient = null;

                this.controlWriter.WriteLine("226 Transfer complete");
                this.controlWriter.Flush();
            }
        }
        #endregion

        #region store data
        /// <summary>
        /// Does the store.
        /// </summary>
        /// <param name="result">The result.</param>
        private void DoStore(IAsyncResult result)
        {
            if (this.dataConnectionType == DataConnectionType.Active)
            {
                // We are only using passive type
            }
            else
            {
                this.dataClient = this.passiveListener.EndAcceptTcpClient(result);
            }

            string pathname = (string)result.AsyncState;

            using (NetworkStream dataStream = this.dataClient.GetStream())
            {
                using (FileStream fs = new FileStream(pathname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    this.StoreStream(dataStream, fs, 4096);

                    this.dataClient.Close();
                    this.dataClient = null;

                    this.controlWriter.WriteLine("226 Closing data connection, file transfer successful");
                    this.controlWriter.Flush();
                }
            }
        }

        /// <summary>
        /// Stores the stream.
        /// </summary>
        /// <param name="dataStream">The data stream.</param>
        /// <param name="fs">The file stream.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        private void StoreStream(NetworkStream dataStream, FileStream fs, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int count = 0;

            while ((count = dataStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, count);
            }
        }
        #endregion

        #region retrieve Data
        /// <summary>
        /// Does the retrieve.
        /// </summary>
        /// <param name="result">The result.</param>
        private void DoRetrieve(IAsyncResult result)
        {
            if (this.dataConnectionType == DataConnectionType.Active)
            {
                // We are only using passive type
            }
            else
            {
                this.dataClient = this.passiveListener.EndAcceptTcpClient(result);
            }

            string pathname = (string)result.AsyncState;

            using (NetworkStream dataStream = this.dataClient.GetStream())
            {
                using (FileStream fs = new FileStream(pathname, FileMode.Open, FileAccess.Read))
                {
                    CopyStream(fs, dataStream);

                    this.dataClient.Close();
                    this.dataClient = null;

                    this.controlWriter.WriteLine("226 Closing data connection, file transfer successful");
                    this.controlWriter.Flush();
                }
            }
        }

        /// <summary>
        /// Copies the stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <param name="output">The output stream.</param>
        /// <returns>The index of the written bytes</returns>
        private long CopyStream(Stream input, Stream output)
        {
            Stream limitedStream = output;

            if (this.connectionTransferType == TransferType.Image)
            {
                return this.CopyStream(input, limitedStream, 4096);
            }
            else
            {
                return this.CopyStreamAscii(input, limitedStream, 4096);
            }
        }

        /// <summary>
        /// Copies the stream.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>The index of the written bytes</returns>
        private long CopyStream(Stream input, Stream output, int bufferSize)
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
        /// Copies the stream ASCII.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>The index of the written bytes</returns>
        private long CopyStreamAscii(Stream input, Stream output, int bufferSize)
        {
            char[] buffer = new char[bufferSize];
            int count = 0;
            long total = 0;

            using (StreamReader rdr = new StreamReader(input))
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
        #endregion

        #region normalize filename
        /// <summary>
        /// Normalizes the filename.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The normalized filename</returns>
        private string NormalizeFilename(string path)
        {
            if (path == null)
            {
                path = string.Empty;
            }

            if (path == "/")
            {
                return this.rootDirOnSystem;
            }

            path = path.StartsWith("/")
                       ? new FileInfo(Path.Combine(this.currentDirOnSystem, path.Substring(1))).FullName
                       : new FileInfo(Path.Combine(this.currentDirOnSystem, path)).FullName;

            return this.IsPathValid(path) ? path : null;
        }
        #endregion

        #region is path valide
        /// <summary>
        /// Determines if path is valid.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True if the path is valid</returns>
        private bool IsPathValid(string path)
        {
            return path.StartsWith(this.rootDirOnSystem);
        }
        #endregion

        #endregion

        #region enums

        /// <summary>
        /// Enumeration for data connection type
        /// </summary>
        private enum DataConnectionType
        {
            Passive,
            Active,
        }

        /// <summary>
        /// Enumeration for transfer type
        /// </summary>
        private enum TransferType
        {
            Ascii,
            Ebcdic, //Not supported
            Image,
            Local, //Not supported
        }
        #endregion
    }
}
