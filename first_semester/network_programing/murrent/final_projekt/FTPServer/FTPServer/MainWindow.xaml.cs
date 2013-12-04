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
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    using FTPManager;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// </summary>
        private IFTPManager ftpManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Methods

        #region Event Methods

        /// <summary>
        /// Starts the FTP server.
        /// </summary>
        private void StartFTPServer()
        {
            this.ftpManager = FTPManagerFactory.CreateFTPManager();
            this.ftpManager.ServerStarted += FTPManagerServerStarted;
            this.ftpManager.ProgressUpdate += FTPManagerProgressUpdate;
            this.ftpManager.Start();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void FTPManagerProgressUpdate(object sender, ProgressUpdateEventArgs e)
        {
            this.WriteLogMessage(e.Message);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void FTPManagerServerStarted(object sender, EventArgs e)
        {
            this.SetStateToRunning();
            this.WriteLogMessage("FTP Server started");
        }

        /// <summary>
        /// Writes the log message.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void WriteLogMessage(string message)
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal, 
                new Action(
                    () =>
                    this.ListBoxMessages.Items.Add(
                        DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " - " + message)));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.StartFTPServer();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void MenuItemFileExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Sets the state to running.
        /// </summary>
        private void SetStateToRunning()
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal, 
                new Action(
                    () =>
                        {
                            this.StatusEllipse.Fill = Brushes.Green;
                            this.StatusText.Text = "Running";
                        }));
        }

        #endregion

        #endregion
    }
}