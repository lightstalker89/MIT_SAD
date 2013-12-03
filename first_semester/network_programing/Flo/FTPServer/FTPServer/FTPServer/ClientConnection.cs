using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace FTPServer
{
    class ClientConnection
    {
        //IPEndPoint which we received from the port command form the ftp server
        private IPEndPoint dataEndpoint; //PORT
        //TcpListener which we generate in the pasv command form the ftp server
        private TcpListener passiveListener; //Passive

        //The TcpClient instance which we received as parameter.
        // Handles the normal communication with the ftp client
        private TcpClient controlClient;
        private NetworkStream controlStream;
        private StreamReader controlReader;
        private StreamWriter controlWriter;

        //The TcpClient instance which we get from the pasv "passiveListener".
        // Handles the file communication with the ftp client
        private TcpClient dataClient;
        private StreamReader dataReader;
        private StreamWriter dataWriter;

        private string username;
        private TransferType connectionType = TransferType.Ascii;
        private DataConnectionType dataConnectionType = DataConnectionType.Active;

        private readonly string PresentDirOnFTP = "/";
        private readonly string RootDirOnSystem = Path.Combine("C:\\", "temp", "tempFTPDir");


        public ClientConnection(TcpClient client)
        {
            controlClient = client;

            controlStream = controlClient.GetStream();

            controlReader = new StreamReader(controlStream);
            controlWriter = new StreamWriter(controlStream);
            Directory.CreateDirectory(RootDirOnSystem);
        }

        # region handle client
        public void HandleClient(object obj)
        {
            controlWriter.WriteLine("220 Service Ready.");
            controlWriter.Flush();

            string line;

            try
            {
                while (!string.IsNullOrEmpty(line = controlReader.ReadLine()))
                {
                    string response = null;

                    string[] command = line.Split(' ');

                    string cmd = command[0].ToUpperInvariant();
                    string arguments = command.Length > 1 ? line.Substring(command[0].Length + 1) : null;

                    if (string.IsNullOrWhiteSpace(arguments))
                        arguments = null;

                    if (response == null)
                    {
                        switch (cmd)
                        {
                            case "USER":
                                response = User(arguments);
                                break;
                            case "PASS":
                                response = Password(arguments);
                                break;
                            case "CWD":
                                response = ChangeWorkingDirectory(arguments);
                                break;
                            case "CDUP":
                                response = ChangeWorkingDirectory("..");
                                break;
                            case "PWD":
                                response = "257 " + "\"" + PresentDirOnFTP + "\"" + " is current directory";
                                break;
                            case "QUIT":
                                response = "221 Service closing control connection";
                                break;
                            case "TYPE":
                                string[] splitArgs = arguments.Split(' ');
                                response = Type(splitArgs[0], splitArgs.Length > 1 ? splitArgs[1] : null);
                                break;
                            case "PORT":
                                response = Port(arguments);
                                break;
                            case "PASV":
                                response = Passive();
                                break;
                            case "LIST":
                                response = List();
                                break;
                            case "RETR":
                                response = Retrieve(arguments);
                                break;
                            default:
                                response = "502 Command not implemented";
                                break;
                            case "STOR":
                                response = Store(arguments);
                                break;
                        }
                    }

                    if (controlClient == null || !controlClient.Connected)
                    {
                        break;
                    }
                    else
                    {
                        controlWriter.WriteLine(response);
                        controlWriter.Flush();

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

        /// <summary>
        /// Accept all users
        /// </summary>
        # region user
        private string User(string username)
        {
            this.username = username;
            return "331 Username ok, need password";
        }
        # endregion

        /// <summary>
        /// Accept every password
        /// </summary>
        # region pwd
        private string Password(string password)
        {
            //Return always true
            return "230 User logged in";
        }
        #endregion

        /// <summary>
        /// Set the encoding which is received from the client. ( The way we will transfer data )
        /// </summary>
        #region type
        private string Type(string typeCode, string formatControl)
        {
            string response = "";

            switch (typeCode)
            {
                case "A":
                    connectionType = TransferType.Ascii;
                    response = "200 OK";
                    break;
                case "I":
                    connectionType = TransferType.Image;
                    response = "200 OK";
                    break;
                case "E":
                    //not implemented
                case "L":
                    //not implemented
                default:
                    response = "504 Command not implemented for that parameter.";
                    break;
            }

            if (formatControl != null)
            {
                switch (formatControl)
                {
                    case "N":
                        response = "200 OK";
                        break;
                    case "T":
                    case "C":
                    default:
                        response = "504 Command not implemented for that parameter.";
                        break;
                }
            }

            return response;
        }
        #endregion

        /// <summary>
        /// !!! Not yet implemented
        /// </summary>
        # region change directory
        private string ChangeWorkingDirectory(string pathname)
        {
            return "250 Changed to new directory";
        }
        # endregion

        /// <summary>
        /// This command tells the server to connect to a given IP address and port.
        /// In this method we will get our data endpoint.
        /// </summary>
        # region port
        private string Port(string hostPort)
        {
            dataConnectionType = DataConnectionType.Active;

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
                Array.Reverse(port);

            dataEndpoint = new IPEndPoint(new IPAddress(ipAddress), BitConverter.ToInt16(port, 0));

            return "200 Data Connection Established";
        }
        #endregion

        /// <summary>
        /// The PASV (passive) command tells the server to open a port to listen on.
        /// </summary>
        # region pasv
        private string Passive()
        {
            dataConnectionType = DataConnectionType.Passive;

            IPAddress localIp = ((IPEndPoint)controlClient.Client.LocalEndPoint).Address;

            //Choose a port which is free.
            //It will return an available port between 1024 and 5000.
            passiveListener = new TcpListener(localIp, 0);
            passiveListener.Start();

            //Send client back what ip address and port we ate listening
            IPEndPoint passiveListenerEndpoint = (IPEndPoint)passiveListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short port = (short)passiveListenerEndpoint.Port;

            byte[] portArray = BitConverter.GetBytes(port);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(portArray);

            return string.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", address[0], address[1], address[2], address[3], portArray[0], portArray[1]);
        }
        #endregion

        /// <summary>
        /// The LIST command sends back a list of file system entries for the specified directory.
        /// </summary>
        # region list
        private string List()
        {
            if (Directory.Exists(RootDirOnSystem))
            {
                if (dataConnectionType == DataConnectionType.Active)
                {
                    // We are only using passive type
                }
                else
                {
                    passiveListener.BeginAcceptTcpClient(DoList, RootDirOnSystem);
                }

                return string.Format("150 Opening {0} mode data transfer for LIST", dataConnectionType);
            }

            return "450 Requested file action not taken";
        }
        #endregion

        /// <summary>
        /// Send an requested file to the client
        /// </summary>
        # region retrieve
        private string Retrieve(string pathname)
        {
            pathname = Path.Combine(RootDirOnSystem, pathname);
            if (File.Exists(pathname))
            {
                if (dataConnectionType == DataConnectionType.Active)
                {
                    // We are only using passive type
                }
                else
                {
                    passiveListener.BeginAcceptTcpClient(DoRetrieve, pathname);
                }

                return string.Format("150 Opening {0} mode data transfer for RETR", dataConnectionType);
            }

            return "550 File Not Found";
        }
        #endregion

        /// <summary>
        /// Save and requested file to the server
        /// </summary>
        # region store
        private string Store(string pathname)
        {
            pathname = Path.Combine(RootDirOnSystem, pathname);

            if (pathname != null)
            {
                if (dataConnectionType == DataConnectionType.Active)
                {
                    // We are only using passive type
                }
                else
                {
                    passiveListener.BeginAcceptTcpClient(DoStore, pathname);
                }

                return string.Format("150 Opening {0} mode data transfer for STOR", dataConnectionType);
            }

            return "450 Requested file action not taken";
        }
        #endregion

        #endregion

        #region ftp command helpers

        # region directory list
        private void DoList(IAsyncResult result)
        {
            if (dataConnectionType == DataConnectionType.Active)
            {
                // We are only using passive type
            }
            else
            {
                dataClient = passiveListener.EndAcceptTcpClient(result);
            }

            string pathname = (string)result.AsyncState;

            using (NetworkStream dataStream = dataClient.GetStream())
            {
                dataReader = new StreamReader(dataStream, Encoding.ASCII);
                dataWriter = new StreamWriter(dataStream, Encoding.ASCII);

                IEnumerable<string> directories = Directory.EnumerateDirectories(pathname);

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

                IEnumerable<string> files = Directory.EnumerateFiles(pathname);

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

                dataClient.Close();
                dataClient = null;

                controlWriter.WriteLine("226 Transfer complete");
                controlWriter.Flush();
            }
        }
        # endregion

        # region store data
        private void DoStore(IAsyncResult result)
        {
            if (dataConnectionType == DataConnectionType.Active)
            {
                // We are only using passive type
            }
            else
            {
                dataClient = passiveListener.EndAcceptTcpClient(result);
            }

            string pathname = (string)result.AsyncState;

            using (NetworkStream dataStream = dataClient.GetStream())
            {
                using (FileStream fs = new FileStream(pathname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    StoreStream(dataStream, fs, 4096);

                    dataClient.Close();
                    dataClient = null;

                    controlWriter.WriteLine("226 Closing data connection, file transfer successful");
                    controlWriter.Flush();
                }
            }
        }

        private void StoreStream(NetworkStream dataStream, FileStream fs, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            long total = 0;

            while ((count = dataStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, count);
                total += count;
            }
        }
        # endregion

        #region retrieve Data
        private void DoRetrieve(IAsyncResult result)
        {
            if (dataConnectionType == DataConnectionType.Active)
            {
                // We are only using passive type
            }
            else
            {
                dataClient = passiveListener.EndAcceptTcpClient(result);
            }

            string pathname = (string) result.AsyncState;

            using (NetworkStream dataStream = dataClient.GetStream())
            {
                using (FileStream fs = new FileStream(pathname, FileMode.Open, FileAccess.Read))
                {
                    CopyStream(fs, dataStream);

                    dataClient.Close();
                    dataClient = null;

                    controlWriter.WriteLine("226 Closing data connection, file transfer successful");
                    controlWriter.Flush();
                }
            }
        }

        private long CopyStream(Stream input, Stream output)
        {
            Stream limitedStream = output; // new RateLimitingStream(output, 131072, 0.5);

            if (connectionType == TransferType.Image)
            {
                return CopyStream(input, limitedStream, 4096);
            }
            else
            {
                return CopyStreamAscii(input, limitedStream, 4096);
            }
        }

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

        private static long CopyStreamAscii(Stream input, Stream output, int bufferSize)
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

        #endregion

        # region enums
        private enum DataConnectionType
        {
            Passive,
            Active,
        }

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
