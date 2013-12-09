// *******************************************************
// * <copyright file="FTPManager.cs" company="MDMCoWorks">
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
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// </summary>
    public class FTPManager : IFTPManager
    {
        /// <summary>
        /// </summary>
        private bool listening;

        /// <summary>
        /// The listener
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The active connections
        /// </summary>
        private List<ClientConnection> activeConnections;

        /// <summary>
        /// The local end point
        /// </summary>
        private readonly IPEndPoint localEndPoint;

        /// <summary>
        /// </summary>
        public FTPManager()
            : this(IPAddress.Any, 21)
        {
        }

        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public delegate void ServerStartedEventHandler(object sender, EventArgs e);

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
        public event ServerStartedEventHandler ServerStarted;

        /// <summary>
        /// </summary>
        public event ProgressUpdateHandler ProgressUpdate;

        #endregion

        #region Methods

        #region Event Methods

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        protected void OnChanged(EventArgs e)
        {
            if (ServerStarted != null)
            {
                ServerStarted(this, e);
            }
        }

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

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void ConnectionProgressUpdate(object sender, ProgressUpdateEventArgs e)
        {
            this.OnProgressUpdate(e);
        }

        #endregion
    }
}