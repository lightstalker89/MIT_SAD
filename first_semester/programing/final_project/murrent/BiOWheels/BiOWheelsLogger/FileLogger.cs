// *******************************************************
// * <copyright file="FileLogger.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsLogger
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// </summary>
    public class FileLogger : ILogger
    {
        #region Private Fields

        /// <summary>
        /// </summary>
        private Queue<LogQueueItem> logQueue;

        /// <summary>
        /// </summary>
        private const string LogFileFolderName = "log";

        /// <summary>
        /// </summary>
        private string fileName;
        #endregion

        #region Properties
        /// <summary>
        /// </summary>
        private bool isEnabled;

        /// <summary>
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
        /// </summary>
        private long maxFileSizeInMB;

        /// <summary>
        /// </summary>
        internal long MaxFileSizeInMB
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
        /// </summary>
        internal string FullQualifiedFileName
        {
            get
            {
                return LogFileFolderName + Path.DirectorySeparatorChar + this.fileName;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// </summary>
        public void Init()
        {
            CreateNewLogFileDirectoryIfNotExists();

            logQueue = new Queue<LogQueueItem>();
        }

        /// <inheritdoc/>
        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            this.isEnabled = isLoggerEnabled;
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(long logFileSize)
        {
            this.maxFileSizeInMB = logFileSize;
        }

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            this.logQueue.Enqueue(new LogQueueItem(message, messageType));

            this.FinalizeQueue();
        }

        /// <summary>
        /// Iterate through the queue and finalize the jobs
        /// </summary>
        private void FinalizeQueue()
        {
            while (logQueue.Count > 0)
            {
                LogQueueItem entry = logQueue.Dequeue();

                if (String.IsNullOrEmpty(this.fileName))
                {
                    this.CheckIfLastFileExists();
                }

                Stream actualFileStream = null;
                double length = 0.0;

                try
                {
                    actualFileStream = new FileStream(this.FullQualifiedFileName, FileMode.Append);
                    length = Math.Round(
                        (actualFileStream.Length / 1024f) / 1024f, 5, MidpointRounding.AwayFromZero);

                }
                catch (IOException ioex)
                {
                    this.logQueue.Enqueue(new LogQueueItem(ioex.Message, MessageType.ERROR));
                }

                if (length > this.maxFileSizeInMB)
                {
                    this.GenerateNewFileName();
                }

                this.WriteToLogFile(entry, actualFileStream);
            }
        }

        /// <summary>
        /// Writes the message to the logfile
        /// </summary>
        /// <param name="entry">The LogQueueItem from the queue</param>
        /// <param name="streamToWrite">File as stream</param>
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
                catch (ObjectDisposedException odex)
                {
                    this.logQueue.Enqueue(new LogQueueItem(odex.Message, MessageType.ERROR));
                }
                catch (IOException ioex)
                {
                    this.logQueue.Enqueue(new LogQueueItem(ioex.Message, MessageType.ERROR));
                }
            }
        }

        /// <summary>
        /// Checks if there is the last file and if there is free space to write to that file
        /// </summary>
        private void CheckIfLastFileExists()
        {
            IEnumerable<string> files =
                     Directory.GetFiles(LogFileFolderName).OrderByDescending(File.GetLastWriteTime);

            if (files.Any())
            {
                this.fileName = files.First().Replace(LogFileFolderName + "\\", String.Empty);
            }
            else
            {
                this.GenerateNewFileName();
            }
        }

        /// <summary>
        /// Creates the log file directory if it does not exsit
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
            this.fileName = String.Format(
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