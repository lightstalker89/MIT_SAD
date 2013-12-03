using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPServer
{
    public partial class Form1 : Form
    {
        private TcpListener listener;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialise a TCP listener which listen for incoming connection.
        /// If a client is is connected the asynchronous method "HandleAcceptTcpClient"
        /// is called.
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            // If you want to accept all incoming connections change to IPAddress.Any
            listener = new TcpListener(IPAddress.Loopback, 21);
            listener.Start();
            // Handles the incoming connection
            listener.BeginAcceptTcpClient(HandleAcceptTcpClient, listener);
        }

        /// <summary>
        /// This asynchronous method handles the incoming connection.
        /// 
        /// The "listener.EndAcceptTcpClient" command accepts an incoming
        /// connection and creates a new TcpClient to handle communictaion.
        /// 
        /// The ftp communication is implemented in the "ClientConnection" class.
        /// 
        /// The "ThreadPool.QueueUserWorkItem" creates a new background Thread
        /// in which the server is handle the connection.
        /// </summary>
        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            TcpClient client = listener.EndAcceptTcpClient(result);
            // For other incoming connections
            listener.BeginAcceptTcpClient(HandleAcceptTcpClient, listener);

            ClientConnection connection = new ClientConnection(client);

            ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
        }
    }
}
