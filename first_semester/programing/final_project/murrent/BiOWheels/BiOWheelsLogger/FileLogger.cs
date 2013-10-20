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
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// </summary>
    public class FileLogger : ILogger
    {
        #region Events

        ///// <summary>
        ///// </summary>
        ///// <param name="sender">
        ///// </param>
        ///// <param name="e">
        ///// </param>
        //private async void LogBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        //{
        //    while (this.logQueue.Count > 0)
        //    {
        //        LogQueueItem item = logQueue.Dequeue();

        //        string actualFileName = await this.GetLogFileName();

        //        if (!string.IsNullOrEmpty(item.Message) && this.isEnabled)
        //        {
        //            try
        //            {
        //                await Task.Run(
        //                    () =>
        //                    {
        //                        using (StreamWriter streamWriter = new StreamWriter(actualFileName, true, Encoding.UTF8))
        //                        {
        //                            streamWriter.WriteLine(item.Message.ToLogFileString(item.MessageType));
        //                        }
        //                    });
        //            }
        //            catch (ObjectDisposedException odex)
        //            {
        //                this.logQueue.Enqueue(new LogQueueItem(odex.Message, MessageType.ERROR));
        //                this.RestartLogBackgroundWorker();
        //            }
        //            catch (IOException ioex)
        //            {
        //                this.logQueue.Enqueue(new LogQueueItem(ioex.Message, MessageType.ERROR));
        //                this.RestartLogBackgroundWorker();
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// </summary>
        ///// <param name="sender">
        ///// </param>
        ///// <param name="e">
        ///// </param>
        //private void LogBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        //{
        //    while (IsWorkerInProgress)
        //    {
        //        if (this.logQueue.Count != 0)
        //        {
        //            LogQueueItem item = logQueue.Dequeue();

        //            if (String.IsNullOrEmpty(this.fileName))
        //            {
        //                IEnumerable<string> files =
        //                    Directory.GetFiles(LogFileFolderName).OrderByDescending(File.GetLastWriteTime);

        //                if (files.Any())
        //                {
        //                    this.fileName = files.First();
        //                }
        //                else
        //                {
        //                    this.GenerateNewFileName();
        //                }
        //            }

        //            //if (!File.Exists(this.fileName))
        //            //{
        //            //    this.GenerateNewFileName();
        //            //}


        //            Stream actualFileStream = new FileStream(this.FullQualifiedFileName, FileMode.OpenOrCreate);
        //            double length = Math.Round(
        //                (actualFileStream.Length / 1024f) / 1024f, 5, MidpointRounding.AwayFromZero);


        //            if (length > this.maxFileSizeInMB)
        //            {
        //                this.GenerateNewFileName();
        //            }

        //            //if (!string.IsNullOrEmpty(item.Message) && this.isEnabled)
        //            //{
        //            //    try
        //            //    {
        //                    using (StreamWriter streamWriter = new StreamWriter(actualFileStream, Encoding.UTF8))
        //                    {
        //                        streamWriter.WriteLine(item.Message.ToLogFileString(item.MessageType));
        //                    }

        //                //}
        //                //catch (ObjectDisposedException odex)
        //                //{
        //                //    this.logQueue.Enqueue(new LogQueueItem(odex.Message, MessageType.ERROR));
        //                //    this.RestartLogBackgroundWorker();
        //                //}
        //                //catch (IOException ioex)
        //                //{
        //                //    this.logQueue.Enqueue(new LogQueueItem(ioex.Message, MessageType.ERROR));
        //                //    this.RestartLogBackgroundWorker();
        //                //}
        //            //}
        //        }
        //        System.Threading.Thread.Sleep(1000);
        //    }

        //    this.IsWorkerInProgress = false;
        //}

        #endregion

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

            this.FlushLog();

        }

        private void FlushLog()
        {
            while (logQueue.Count > 0)
            {
                LogQueueItem entry = logQueue.Dequeue();

                if (String.IsNullOrEmpty(this.fileName))
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


                Stream actualFileStream = new FileStream(this.FullQualifiedFileName, FileMode.Append);
                double length = Math.Round(
                    (actualFileStream.Length / 1024f) / 1024f, 5, MidpointRounding.AwayFromZero);


                if (length > this.maxFileSizeInMB)
                {
                    this.GenerateNewFileName();
                }

                if (!string.IsNullOrEmpty(entry.Message) && this.isEnabled)
                {
                    try
                    {
                        using (StreamWriter log = new StreamWriter(actualFileStream, Encoding.UTF8))
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
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateNewLogFileDirectoryIfNotExists()
        {
            if (!Directory.Exists(LogFileFolderName))
            {
                Directory.CreateDirectory(LogFileFolderName);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //private void RestartLogBackgroundWorker()
        //{
        //    this.logBackgroundWorker.CancelAsync();

        //    if (!this.logBackgroundWorker.IsBusy)
        //    {
        //        this.logBackgroundWorker.RunWorkerAsync();
        //    }
        //}

        ///// <summary>
        ///// </summary>
        ///// <returns>
        ///// </returns>
        //private async Task<string> GetLogFileName()
        //{
        //    // GET RID OF FILEINFO - NOT GOOD
        //    // FileInfo fi = null;
        //    // FileStream fs = null;

        //    // string result = await Task.Run(() =>
        //    // {
        //    // CreateNewLogFileDirectoryIfNotExists();

        //    // if (String.IsNullOrEmpty(this.fileName))
        //    // {
        //    // DirectoryInfo di = new DirectoryInfo(LogFileFolderName);

        //    // fi = di.GetFiles("*.txt").OrderByDescending(p => p.LastWriteTime).FirstOrDefault();

        //    // if (fi != null)
        //    // {
        //    // if (fi.Exists && (fi.Length / 1024 / 1024 < this.MaxFileSizeInMB))
        //    // {
        //    // this.fileName = fi.FullName;
        //    // fi = new FileInfo(this.fileName);
        //    // return this.fileName;
        //    // }
        //    // }
        //    // else
        //    // {
        //    // GenerateNewFileName();
        //    // //File.Create(this.fileName);

        //    // //fi = new FileInfo(this.fileName);
        //    // //fi.Create();

        //    // File.Create(this.fileName);

        //    // return this.fileName;
        //    // }
        //    // }
        //    // else
        //    // {
        //    // GenerateNewFileName();
        //    // }

        //    // //fi = new FileInfo(this.fileName);
        //    // fs = new FileStream(this.fileName,FileMode.Append);
        //    // return this.fileName;
        //    // });
        //    await Task.Run(
        //        () =>
        //        {
        //            if (String.IsNullOrEmpty(this.fileName))
        //            {
        //                IEnumerable<string> files = Directory.GetFiles(LogFileFolderName).OrderByDescending(File.GetLastWriteTime);

        //                if (files.Any())
        //                {
        //                    this.fileName = files.First();
        //                }
        //                else
        //                {
        //                    this.GenerateNewFileName();
        //                }
        //            }
        //            else
        //            {
        //                if (!File.Exists(this.fileName))
        //                {
        //                    this.GenerateNewFileName();
        //                }
        //                else
        //                {
        //                    double length;

        //                    using (Stream stream = new FileStream(this.fileName, FileMode.Open))
        //                    {
        //                        length = Math.Round(
        //                            (stream.Length / 1024f) / 1024f, 5, MidpointRounding.AwayFromZero);
        //                    }


        //                    if (length > this.maxFileSizeInMB)
        //                    {
        //                        this.GenerateNewFileName();
        //                    }
        //                }
        //            }
        //        });

        //    return this.FullQualifiedFileName;
        //}

        /// <summary>
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