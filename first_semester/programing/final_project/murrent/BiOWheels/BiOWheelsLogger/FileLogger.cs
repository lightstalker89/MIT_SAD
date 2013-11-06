// *******************************************************
// * <copyright file="FileLogger.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BiOWheelsLogger.Test")]

namespace BiOWheelsLogger
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Class representing the FileLogger and interaction logic
    /// </summary>
    public class FileLogger : ILogger
    {
        #region Constants

        /// <summary>
        /// Constant for the log file folder name
        /// </summary>
        private const string LogFileFolderName = "log";

        #endregion

        #region Private Fields

        /// <summary>
        /// The list of <see cref="LogQueueItem"/>
        /// </summary>
        private ConcurrentQueue<LogQueueItem> logQueue;

        /// <summary>
        /// String representing the file name of the log file
        /// </summary>
        private string fileName;

        /// <summary>
        /// Boolean value representing the value if the worker thread is in progress
        /// </summary>
        private bool isWorkerInProgress;

        /// <summary>
        /// Double representing the maximum file size in MB
        /// </summary>
        private double maxFileSizeInMB;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value if the <see cref="FileLogger"/> is enabled or not
        /// </summary>
        private bool isEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether a status of the <see cref="FileLogger"/> is enabled or not
        /// </summary>
        internal bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.isEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum file size in MB
        /// </summary>
        internal double MaxFileSizeInMB
        {
            get
            {
                return this.maxFileSizeInMB;
            }

            set
            {
                this.maxFileSizeInMB = value;
            }
        }

        /// <summary>
        /// Gets the full qualified name for the log file including file path
        /// </summary>
        internal string FullQualifiedFileName
        {
            get
            {
                return LogFileFolderName + Path.DirectorySeparatorChar + this.fileName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the background thread is running
        /// </summary>
        internal bool IsWorkerInProgress
        {
            get
            {
                return this.isWorkerInProgress;
            }

            private set
            {
                this.isWorkerInProgress = value;
            }
        }

        /// <summary>
        /// Gets or sets the log queue
        /// </summary>
        internal ConcurrentQueue<LogQueueItem> LogQueue
        {
            get
            {
                return this.logQueue;
            }

            set
            {
                this.logQueue = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Start the thread which finalizes the queue
        /// </summary>
        public void StartBackgroundWorker()
        {
            Thread workerThread = new Thread(this.FinalizeQueue) { IsBackground = true };
            workerThread.Start();
        }

        /// <summary>
        /// Initialize the logger
        /// </summary>
        public void Init()
        {
            this.IsWorkerInProgress = true;

            this.CreateNewLogFileDirectoryIfNotExists();

            this.LogQueue = new ConcurrentQueue<LogQueueItem>();

            this.StartBackgroundWorker();
        }

        /// <inheritdoc/>
        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            this.isEnabled = isLoggerEnabled;
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(double logFileSize)
        {
            this.maxFileSizeInMB = logFileSize;
        }

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            this.LogQueue.Enqueue(new LogQueueItem(message, messageType));
        }

        /// <summary>
        /// Iterate through the queue and finalize the jobs
        /// </summary>
        private void FinalizeQueue()
        {
            while (this.IsWorkerInProgress)
            {
                LogQueueItem entry;

                if (this.LogQueue.TryDequeue(out entry))
                {
                    if (string.IsNullOrEmpty(this.fileName))
                    {
                        this.CheckIfLastFileExists();
                    }

                    Stream actualFileStream = null;
                    double length = 0.0;

                    try
                    {
                        actualFileStream = new FileStream(this.FullQualifiedFileName, FileMode.Append);
                        length = Math.Round((actualFileStream.Length / 1024f) / 1024f, 2, MidpointRounding.AwayFromZero);
                    }
                    catch (IOException systemIOException)
                    {
                        this.LogQueue.Enqueue(new LogQueueItem(systemIOException.Message, MessageType.ERROR));
                    }
                    catch (NotSupportedException notSupportedException)
                    {
                        this.LogQueue.Enqueue(new LogQueueItem(notSupportedException.Message, MessageType.ERROR));
                    }

                    if (length > this.maxFileSizeInMB)
                    {
                        if (actualFileStream != null)
                        {
                            actualFileStream.Close();
                        }

                        this.RenameFile();
                        this.GenerateNewFileName();

                        actualFileStream = new FileStream(this.FullQualifiedFileName, FileMode.Append);
                    }

                    this.WriteToLogFile(entry, actualFileStream);
                }
            }
        }

        /// <summary>
        /// Writes the message to the log file
        /// </summary>
        /// <param name="entry">
        /// The LogQueueItem from the queue
        /// </param>
        /// <param name="streamToWrite">
        /// File as stream
        /// </param>
        private void WriteToLogFile(LogQueueItem entry, Stream streamToWrite)
        {
            if (!string.IsNullOrEmpty(entry.Message) && this.isEnabled)
            {
                try
                {
                    using (StreamWriter log = new StreamWriter(streamToWrite, Encoding.UTF8))
                    {
                        log.WriteLine(entry.ToString());
                    }
                }
                catch (ObjectDisposedException objectDisposedException)
                {
                    this.LogQueue.Enqueue(new LogQueueItem(objectDisposedException.Message, MessageType.ERROR));
                }
                catch (IOException systemIOException)
                {
                    this.LogQueue.Enqueue(new LogQueueItem(systemIOException.Message, MessageType.ERROR));
                }
            }
        }

        /// <summary>
        /// Renames the actual log file
        /// </summary>
        private void RenameFile()
        {
            File.Move(this.FullQualifiedFileName, this.FullQualifiedFileName + ".bak");
        }

        /// <summary>
        /// Checks if there is the last file and if there is free space to write to that file
        /// </summary>
        private void CheckIfLastFileExists()
        {
            IEnumerable<string> files = Directory.GetFiles(LogFileFolderName).OrderByDescending(File.GetLastWriteTime);

            if (files.Any())
            {
                this.fileName = files.First().Replace(LogFileFolderName + "\\", string.Empty);
            }
            else
            {
                this.GenerateNewFileName();
            }
        }

        /// <summary>
        /// Creates the log file directory if it does not exist
        /// </summary>
        private void CreateNewLogFileDirectoryIfNotExists()
        {
            if (!Directory.Exists(LogFileFolderName))
            {
                Directory.CreateDirectory(LogFileFolderName);
            }
        }

        /// <summary>
        /// Generates a new filename
        /// </summary>
        private void GenerateNewFileName()
        {
            this.fileName = string.Format(
                "BiOWheels_Log-{0}-{1}-{2}T{3}-{4}-{5}-{6}.txt", 
                DateTime.Now.Year, 
                DateTime.Now.Month, 
                DateTime.Now.Day, 
                DateTime.Now.Hour, 
                DateTime.Now.Minute, 
                DateTime.Now.Second, 
                DateTime.Now.Millisecond);
        }

        #endregion
    }
}