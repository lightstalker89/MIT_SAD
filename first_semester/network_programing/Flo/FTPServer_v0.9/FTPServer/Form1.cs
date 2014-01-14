// *******************************************************
// * <copyright file="Form1.cs" company="FGrill">
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
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Represents the Form1 class which contains the entry point of the application.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// The TCP listener
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The information items
        /// </summary>
        private List<string> infoItems = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.lbxReceived.DataSource = infoItems;
        }


        /// <summary>
        /// Initialize a TCP listener which listen for incoming connection.
        /// If a client is is connected the asynchronous method is called.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            this.UpdateListBox("Listening...");

            // If you want to accept all incoming connections change to IPAddress.Any
            this.listener = new TcpListener(IPAddress.Loopback, 21);
            this.listener.Start();

            // Handles the incoming connection
            this.listener.BeginAcceptTcpClient(this.HandleAcceptTcpClient, this.listener);
        }


        /// <summary>
        /// This asynchronous method handles the incoming connection.
        /// </summary>
        /// <param name="result">The result.</param>
        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            // Accepts an incoming connection and creates a new TcpClient to handle communictaion 
            TcpClient client = this.listener.EndAcceptTcpClient(result);

            // For other incoming connections
            this.listener.BeginAcceptTcpClient(HandleAcceptTcpClient, listener);
            ClientConnection connection = new ClientConnection(client);
            
            // Register event
            connection.Message += this.Connection_Message;

            // The "ThreadPool.QueueUserWorkItem" creates a new background Thread in which the server is handle the connection.
            ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
            this.UpdateListBox("Client conneced: " + client.Client.RemoteEndPoint);
        }

        /// <summary>
        /// Handles the Message event of the connection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MessageEventArgs"/> instance containing the event data.</param>
        private void Connection_Message(object sender, MessageEventArgs e)
        {
            this.UpdateListBox(e.Message);
        }

        /// <summary>
        /// Updates the ListBox.
        /// </summary>
        /// <param name="text">The text.</param>
        private void UpdateListBox(string text)
        {
            MethodInvoker inv = delegate
            {
                this.infoItems.Add(text);
                this.lbxReceived.DataSource = null;
                this.lbxReceived.DataSource = this.infoItems;
            };
            this.Invoke(inv);
        }
    }
}
