// *******************************************************
// * <copyright file="MainWindow.xaml.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace FTPServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        private const string ServerMessage = "220 FTP Welcome to MDM's FTP server\r\n";

        private const string PresentDirectoryOnFTP = "/";

        private readonly string rootDirectoryOnSystem = Path.Combine("C:\\", "temp", "tempFTPDir");

        private Thread ftpserverThread;
        private TcpListener ftpCommandListner;
        private TcpClient tcpClient;
        private IPAddress clientIp;
        private int intPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            if (!Directory.Exists(this.rootDirectoryOnSystem))
            {
                Directory.CreateDirectory(this.rootDirectoryOnSystem);
            }
        }

        #region Properties
        /// <summary>
        /// Gets or sets the FTP command listner.
        /// </summary>
        /// <value>
        /// The FTP command listner.
        /// </value>
        public TcpListener FTPCommandListner
        {
            get
            {
                return this.ftpCommandListner;
            }
            set
            {
                this.ftpCommandListner = value;
            }
        }

        /// <summary>
        /// Gets or sets the client socket.
        /// </summary>
        /// <value>
        /// The client socket.
        /// </value>
        public TcpClient TcpClient
        {
            get
            {
                return this.tcpClient;
            }
            set
            {
                this.tcpClient = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the FTP server
        /// </summary>
        internal void Start()
        {
            this.FTPCommandListner = new TcpListener(IPAddress.Loopback, 21);
            this.FTPCommandListner.Start();

            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(
                    () =>
                    {
                        this.StatusEllipse.Fill = Brushes.Green;
                        this.StatusText.Text = "Running";

                    }));

            this.WriteLogMessage(
                         DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()
                         + " - FTP Server started");

            try
            {
                this.TcpClient = this.FTPCommandListner.AcceptTcpClient();

                IPEndPoint ipEndPoint = this.TcpClient.Client.RemoteEndPoint as IPEndPoint;

                if (ipEndPoint != null)
                {
                    this.clientIp = ipEndPoint.Address;
                }

                Thread clientThread = new Thread(this.HandleClient);
                clientThread.Start();
            }
            catch (Exception)
            {
            }
        }

        internal void HandleClient()
        {
            this.WriteLogMessage("Client from " + this.clientIp + " connected");

            try
            {
                NetworkStream tcpClientStream = tcpClient.GetStream();

                SendMessage(ServerMessage, ref tcpClientStream);

                bool done = false;

                while (!done)
                {
                    Thread.Sleep(100);

                    string clientMessage = ReadMessage(ref this.tcpClient, ref tcpClientStream);
                    string ftpCommandShortcut = clientMessage.Length == 0 ? "" : clientMessage.Substring(0, 4).Trim();

                    ftpCommandShortcut = ftpCommandShortcut.ToUpper();

                    switch (ftpCommandShortcut)
                    {
                        case "USER":
                            this.SendMessage("331 Username ok, need password\n", ref tcpClientStream);
                            break;

                        case "PASS":
                            this.SendMessage("230 User logged in\n", ref tcpClientStream);
                            this.WriteLogMessage("User logged in successfully");
                            break;

                        case "BYE":
                            break;

                        case "BYTE":
                            break;

                        case "RETR":
                            break;

                        case "STORE":
                            break;

                        case "LIST":
                            this.SendMessage("150 Opening ASCII mode data connection for /\r\n", ref tcpClientStream);
                            this.WriteLogMessage("LIST command received");
                            ListDirectory(ref tcpClientStream);
                            break;

                        case "SYST":
                            this.WriteLogMessage("SYST command received");
                            this.SendMessage("215 WINDOWS-NT-6\n", ref tcpClientStream);
                            break;

                        case "TYPE":
                            this.WriteLogMessage("TYPE command received");
                            this.SendMessage("200 type set\n", ref tcpClientStream);
                            break;

                        case "FEAT":
                            this.WriteLogMessage("FEAT command received");
                            this.SendMessage("no-features\n", ref tcpClientStream);
                            break;

                        case "PWD":
                            this.WriteLogMessage("PWD command received");
                            this.SendMessage("257 " + "\"" + PresentDirectoryOnFTP + "\"" + " is current directory \r\n", ref tcpClientStream);
                            break;

                        case "PASV":
                            this.WriteLogMessage("PASV command received");
                            this.SendMessage("227 Entering passive mode\n", ref tcpClientStream);
                            break;

                        case "PORT":
                            // TODO: Do somethings
                            this.WriteLogMessage("PORT command received");
                            this.SendMessage("200 PORT command successful\r\n", ref tcpClientStream);
                            break;

                        case "":
                            break;

                        default:
                            this.WriteLogMessage(ftpCommandShortcut + " command not supported");
                            this.SendMessage("502 Command not implemented", ref tcpClientStream);
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        internal void ListDirectory(ref NetworkStream tcpClientStream)
        {

            string currentDirectory = this.rootDirectoryOnSystem + PresentDirectoryOnFTP;
            string tmpFileName = "";

            string[] files = Directory.GetFiles(currentDirectory);
            string[] directories = Directory.GetDirectories(currentDirectory);

            try
            {
                string ownerGroupFile = "-rwxr--r-- 1 owner group ";

                //foreach (string fileName in files)
                //{
                //    try
                //    {
                //        FileStream tmpFile = File.Open(fileName, FileMode.Open);

                //        tmpFileName = tmpFile.Name.Replace("\\", "/");

                //        ownerGroupFile += tmpFile.Length + " " + File.GetLastAccessTime(fileName).ToShortDateString() + " " + tmpFileName.Trim() + "\n";
                //        byte[] buffer = Encoding.ASCII.GetBytes(ownerGroupFile);
                //        try
                //        {
                //            if (tcpClientStream.CanWrite)
                //            {
                //                tcpClientStream.Write(buffer, 0, buffer.Length);
                //            }
                //        }
                //        catch (Exception e)
                //        {
                //            this.WriteLogMessage("Error while sending file info");
                //        }

                //    }
                //    catch (Exception)
                //    {

                //    }
                //}

                //foreach (string directory in directories)
                //{
                //    string ownerGroupDirectory = "drwxr-xr-x 1 root root";

                //    string directoryName = Path.GetDirectoryName(directory);

                //    if (directoryName != null)
                //    {
                //        tmpFileName = directoryName.Replace("\\", "/");

                //        ownerGroupDirectory += "  0  " + "  " + "Dec 2 18:53"  + "  " + directoryName.Trim() + "\r";
                //        byte[] buffer = Encoding.ASCII.GetBytes(ownerGroupDirectory);

                //        try
                //        {
                //            if (tcpClientStream.CanWrite)
                //            {
                //                tcpClientStream.Write(buffer, 0, buffer.Length);
                //            }
                //        }
                //        catch (Exception e)
                //        {
                //            this.WriteLogMessage("Error while sending directory info");
                //        }
                //    }
                //}

                this.SendMessage("drwxr-xr-x 1 root root 1024 Sep 24 21:10 test\r\n", ref tcpClientStream);

            }
            catch (Exception)
            {

            }

            this.SendMessage("226 Transfer complete.\n", ref tcpClientStream);
        }

        internal string ReadMessage(ref TcpClient tcpClient, ref NetworkStream inBuffer)
        {
            string clientMessage;
            StringBuilder clientTmp = new StringBuilder();
            byte[] buffer = new Byte[1024];

            Thread currentThread = Thread.CurrentThread;

            lock (currentThread)
            {
                if (tcpClient.Available > 0)
                {
                    while (tcpClient.Available > 0)
                    {
                        int iBytes = inBuffer.Read(buffer, 0, buffer.Length);
                        string tmp = Encoding.ASCII.GetString(buffer, 0, iBytes);
                        clientTmp.Append(tmp);
                    }
                }
                clientMessage = clientTmp.ToString();
            }
            return clientMessage;
        }

        internal void SendMessage(string message, ref NetworkStream networkStream)
        {
            Thread currentThread = Thread.CurrentThread;

            lock (currentThread)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                networkStream.Write(buffer, 0, buffer.Length);
            }
        }

        internal bool SendFile(string filePath, ref NetworkStream networkStream)
        {

            Thread currentThread = Thread.CurrentThread;
            try
            {
                lock (currentThread)
                {
                    StreamReader outFile = new StreamReader(filePath);

                    char[] buff = new Char[1024];
                    int amount;
                    while ((amount = outFile.Read(buff, 0, 1024)) != 0)
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes(buff);
                        networkStream.Write(buffer, 0, amount);
                    }
                }
            }
            catch (IOException ioException)
            {
                networkStream.Close();
                return false;
            }
            networkStream.Close();
            return true;
        }

        internal void WriteLogMessage(string message)
        {
            this.Dispatcher.BeginInvoke(
           DispatcherPriority.Normal,
           new Action(
               () => this.ListBoxMessages.Items.Add(message)));
        }

        #region Event Methods

        /// <summary>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemFileExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Windows is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ftpserverThread = new Thread(this.Start);
            ftpserverThread.Start();
        }

        #endregion


        #endregion
    }
}