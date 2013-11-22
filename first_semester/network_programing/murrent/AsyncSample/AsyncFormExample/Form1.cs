// *******************************************************
// * <copyright file="Form1.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace AsyncFormExample
{
    using System;
    using System.Drawing;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// </summary>
        private byte[] receivedData;

        /// <summary>
        /// </summary>
        private readonly Socket clientSocket;

        /// <summary>
        /// </summary>
        private readonly IPEndPoint ipep;

        /// <summary>
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void Button1Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(textBox1.Text);
            textBox1.Clear();

            clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, clientSocket);
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void Button2Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket.BeginConnect(ipep, ConnectCallBack, clientSocket);
            }
            catch (Exception)
            {
                listBox1.Items.Add("Error while connecting to the server");
            }
        }

        /// <summary>
        /// Disconnect from server
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void Button3Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket.Disconnect(false);
            }
            catch (Exception)
            {
                listBox1.Items.Add("Error while disconnecting from the server");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ar">
        /// </param>
        private void ConnectCallBack(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;

            try
            {
                if (socket != null)
                {
                    socket.EndConnect(ar);
                }
            }
            catch (Exception)
            {
                listBox1.Items.Add("Error while connecting to the server");
            }

            if (socket != null && socket.Connected)
            {
                this.BeginInvoke(
                    new Action(
                        () =>
                        {
                            listBox1.Items.Add("Connected");
                            toolStripStatusLabel1.Text = "Connected";
                            toolStripStatusLabel1.BackColor = Color.LightGreen;
                        }));
            }
            else
            {
                this.BeginInvoke(
                    new Action(
                        () =>
                        {
                            listBox1.Items.Add("Disconnected");
                            toolStripStatusLabel1.Text = "Disconnected";
                            toolStripStatusLabel1.BackColor = Color.LightCoral;
                        }));
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            receivedData = new byte[4069];
            Socket socket = ar.AsyncState as Socket;

            try
            {
                if (socket != null)
                {
                    socket.EndSend(ar);

                    socket.BeginReceive(
                        receivedData, 0, receivedData.Length, SocketFlags.None, ReceivedCallBack, socket);
                }
            }
            catch (Exception)
            {
                listBox1.Items.Add("Error while sending data to the server");
            }
        }

        private void ReceivedCallBack(IAsyncResult ar)
        {
            try
            {
                Socket socket = ar.AsyncState as Socket;

                if (socket != null)
                {
                    int len = socket.EndReceive(ar);

                    this.BeginInvoke(
                        new Action(
                            () =>
                                {
                                    string message = Encoding.ASCII.GetString(receivedData, 0, len);

                                    listBox1.Items.Add(message);
                                }));
                }
            }
            catch (Exception)
            {
                listBox1.Items.Add("Error while receiving data from the server");
            }
        }
    }
}