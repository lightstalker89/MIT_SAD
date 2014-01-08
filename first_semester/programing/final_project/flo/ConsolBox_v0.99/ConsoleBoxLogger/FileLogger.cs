// *******************************************************
// * <copyright file="FileLogger.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxLogger
{
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// Class representing the <see cref="FileLogger"/>
    /// </summary>
    public class FileLogger : ILogger
    {
        /// <summary>
        /// The log file name
        /// </summary>
        private const string LogName = "Log.txt";

        /// <summary>
        /// The log backup file name
        /// </summary>
        private const string LogBackupName = "Log.bak";

        /// <summary>
        /// The log thread
        /// </summary>
        private Thread logThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        public FileLogger()
        {
            this.MessageQueue = new ConcurrentQueue<QueueMessage>();
            this.logThread = new Thread(this.LogInFile) { IsBackground = true };
            this.logThread.Start();
        }

        /// <summary>
        /// Gets or sets the message queue.
        /// </summary>
        public ConcurrentQueue<QueueMessage> MessageQueue { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        public double FileSize { get; set; }

        /// <inheritdoc/>
        public void SetFileSize<T>(long logFileSize)
        {
            this.FileSize = logFileSize;
        }

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            this.MessageQueue.Enqueue(new QueueMessage(message, messageType));
        }

        /// <summary>
        /// Logs the information in file.
        /// </summary>
        private void LogInFile()
        {
            do
            {
                if (this.MessageQueue.Count > 0)
                {
                    double size = 0;
                    using (FileStream file = new FileStream(LogName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(file))
                        {
                            QueueMessage currentElement;
                            if (this.MessageQueue.TryDequeue(out currentElement))
                            {
                                streamWriter.WriteLine(currentElement.Message.ToLogFileString(currentElement.MessageType));
                                streamWriter.Flush();
                                size = file.Length;
                            }
                        }
                    }

                    if (size > ((this.FileSize * 1024) * 1024))
                    {
                        this.ChangeLogFile();
                    }
                }
            }
            while (true);
        }

        /// <summary>
        /// Creates a new log file and renames the old one.
        /// </summary>
        private void ChangeLogFile()
        {
            System.IO.File.Delete(LogBackupName);
            System.IO.File.Move(LogName, LogBackupName);
        }
    }
}
