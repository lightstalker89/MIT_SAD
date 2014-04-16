// *******************************************************
// * <copyright file="FileHandleWrapper.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BiOWheelsFileWatcher.Test")]

namespace BiOWheelsFileHandleWrapper
{
    using System;
    using System.Diagnostics;

    using BiOWheelsFileHandleWrapper.CustomEventArgs;

    using Microsoft.Win32;

    /// <summary>
    /// The <see ref="FileHandleWrapper"/> class and its interaction logic 
    /// </summary>
    public class FileHandleWrapper : IFileHandleWrapper
    {
        /// <summary>
        /// The handles for file
        /// </summary>
        private int handlesForFile;

        /// <summary>
        /// The result
        /// </summary>
        private string result;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHandleWrapper"/> class.
        /// </summary>
        internal FileHandleWrapper()
        {
            this.CreateRegistryKey();
        }

        #region Delegates

        /// <summary>
        /// Delegate for the <see cref="FileHandlesFoundHandler"/> event 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="data">The <see cref="EventArgs"/> instance containing the event data.</param>
        public delegate void FileHandlesFoundHandler(object sender, FileHandlesEventArgs data);

        /// <summary>
        /// Delegate for the <see cref="FileHandlesErrorHandler" /> event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="data">The <see cref="FileHandlesErrorEventArgs" /> instance containing the event data.</param>
        public delegate void FileHandlesErrorHandler(object sender, FileHandlesErrorEventArgs data);

        #endregion

        #region Event Handler

        /// <inheritdoc />
        public event FileHandlesFoundHandler FileHandlesFound;

        /// <inheritdoc />
        public event FileHandlesErrorHandler FileHandlesError;

        #endregion

        /// <summary>
        /// Gets or sets the handles for file.
        /// </summary>
        /// <value>
        /// The handles for file.
        /// </value>
        internal int HandlesForFile
        {
            get
            {
                return this.handlesForFile;
            }

            set
            {
                this.handlesForFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        internal string Result
        {
            get
            {
                return this.result;
            }

            set
            {
                this.result = value;
            }
        }

        #region Methods

        /// <inheritdoc/>
        public void FindHandlesForFile(string searchPattern)
        {
            try
            {
                Process process = new Process
                    {
                        StartInfo =
                            new ProcessStartInfo
                                {
                                    FileName = "handle.exe", 
                                    Arguments = "\"" + searchPattern + "\"", 
                                    CreateNoWindow = false, 
                                    UseShellExecute = false, 
                                    RedirectStandardError = true, 
                                    RedirectStandardOutput = true, 
                                    Verb = "runas"
                                }, 
                        EnableRaisingEvents = true
                    };

                process.Exited += this.ProcessExited;
                process.OutputDataReceived += this.ProcessOutputDataReceived;
                process.ErrorDataReceived += this.ProcessErrorDataReceived;
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
            catch (InvalidOperationException invalidOperationException)
            {
                this.OnFileHandlesError(this, new FileHandlesErrorEventArgs(invalidOperationException.Message));
            }
        }

        /// <summary>
        /// Creates the registry key.
        /// </summary>
        internal void CreateRegistryKey()
        {
            RegistryKey softwareKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);

            if (softwareKey != null)
            {
                RegistryKey sysinternalsKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Sysinternals", true);

                if (sysinternalsKey == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Sysinternals");
                }

                RegistryKey myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Sysinternals\\Handle", true);

                if (myKey == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Sysinternals\\Handle");
                    myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Sysinternals\\Handle", true);
                }

                myKey.SetValue("EulaAccepted", "1", RegistryValueKind.DWord);
            }
            else
            {
                this.OnFileHandlesError(this, new FileHandlesErrorEventArgs("Registry error. No SOFTWARE folder found"));
            }
        }

        #region Event Methods

        /// <summary>
        /// Handles the ProcessOutputDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="DataReceivedEventArgs"/> instance containing the event data.
        /// </param>
        internal void ProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Result += e.Data;
        }

        /// <summary>
        /// Handles the ErrorDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="DataReceivedEventArgs"/> instance containing the event data.
        /// </param>
        internal void ProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.OnFileHandlesError(sender, new FileHandlesErrorEventArgs(e.Data));
        }

        /// <summary>
        /// Occurs when the process has exited
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        internal void ProcessExited(object sender, EventArgs e)
        {
            this.Result =
                this.Result.Replace(
                    "Handle v3.51Copyright (C) 1997-2013 Mark RussinovichSysinternals - www.sysinternals.com", 
                    string.Empty);

            FileHandlesEventArgs eventArgs = new FileHandlesEventArgs(true);

            if (this.Result.ToLower().Contains("no matching handles found"))
            {
                eventArgs = new FileHandlesEventArgs(false);
            }

            this.OnFileHandlesFound(this, eventArgs);
        }

        /// <summary>
        /// Called when all file handles have been found
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        protected void OnFileHandlesFound(object sender, FileHandlesEventArgs data)
        {
            if (this.FileHandlesFound != null)
            {
                this.FileHandlesFound(this, data);
            }
        }

        /// <summary>
        /// Called when an error occurred
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The <see cref="FileHandlesErrorEventArgs"/> instance containing the event data.
        /// </param>
        protected void OnFileHandlesError(object sender, FileHandlesErrorEventArgs data)
        {
            if (this.FileHandlesError != null)
            {
                this.FileHandlesError(this, data);
            }
        }

        #endregion

        #endregion
    }
}