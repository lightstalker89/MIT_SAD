using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FTPManager
{
    public class FTPManager : IFTPManager
    {
         /// <summary>
        /// </summary>
        private bool listening;

        /// <summary>
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// </summary>
        private List<ClientConnection> activeConnections;

        /// <summary>
        /// </summary>
        private readonly IPEndPoint localEndPoint;

        /// <summary>
        /// </summary>
        public FTPManager()
            : this(IPAddress.Any, 21)
        {

        }

        #region Delegates

        public delegate void ServerStartedEventHandler(object sender, EventArgs e);

        public delegate void ProgressUpdateHandler(object sender, ProgressUpdateEventArgs e);

        #endregion

        #region Events

        public event ServerStartedEventHandler ServerStarted;

        public event ProgressUpdateHandler ProgressUpdate;

        #endregion

        #region Methods

        #region Event Methods

        protected void OnChanged(EventArgs e)
        {
            if (ServerStarted != null)
            {
                ServerStarted(this, e);
            }
        }

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
        /// <param name="ipAddress">
        /// </param>
        /// <param name="port">
        /// </param>
        public FTPManager(IPAddress ipAddress, int port)
        {
            localEndPoint = new IPEndPoint(ipAddress, port);
        }

        /// <summary>
        /// </summary>
        public void Start()
        {
            listener = new TcpListener(localEndPoint);

            listening = true;
            listener.Start();

            this.OnChanged(new EventArgs());

            activeConnections = new List<ClientConnection>();

            listener.BeginAcceptTcpClient(HandleAcceptTcpClient, listener);
        }

        /// <summary>
        /// </summary>
        /// <param name="result">
        /// </param>
        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            if (listening)
            {
                listener.BeginAcceptTcpClient(HandleAcceptTcpClient, listener);

                TcpClient client = listener.EndAcceptTcpClient(result);

                ClientConnection connection = new ClientConnection(client);
                connection.ProgressUpdate += ConnectionProgressUpdate;

                activeConnections.Add(connection);

                ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
            }
        }

        private void ConnectionProgressUpdate(object sender, ProgressUpdateEventArgs e)
        {
            this.OnProgressUpdate(e);
        }

        #endregion
    }
}
