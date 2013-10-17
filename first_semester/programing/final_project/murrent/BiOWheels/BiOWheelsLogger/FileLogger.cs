using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiOWheelsLogger
{
    public class FileLogger : ILogger
    {
        public FileLogger() { }

        #region Events
        private async void LogBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            while (this.logQueue.Count > 0)
            {
                LogQueueItem item = logQueue.Dequeue();

                string actualFileName = await GetLogFile();
                
                if (!String.IsNullOrEmpty(item.Message) && this.isEnabled)
                {
                    try
                    {
                    await Task.Run(() =>
                                       {
                                           using (StreamWriter streamWriter = new StreamWriter(actualFileName,true,Encoding.UTF8))
                                           {
                                               streamWriter.WriteLine(item.Message.ToLogFileString(item.MessageType));
                                           }
                                       });
                    }
                    catch (ObjectDisposedException odex)
                    {
                        this.logQueue.Enqueue(new LogQueueItem(odex.Message, MessageType.ERROR));
                        this.RestartLogBackgroundWorker();
                    }
                    catch (IOException ioex)
                    {
                        this.logQueue.Enqueue(new LogQueueItem(ioex.Message, MessageType.ERROR));
                        this.RestartLogBackgroundWorker();
                    }
                }
            }
        }
        #endregion

        #region Private Fields
        private Queue<LogQueueItem> logQueue;
        private BackgroundWorker logBackgroundWorker;
        private const string LogFileFolderName = "log";
        private string fileName;
        #endregion

        #region Properties
        private bool isEnabled;
        internal bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
            }
        }

        private long maxFileSizeInMB;
        internal long MaxFileSizeInMB
        {
            get { return maxFileSizeInMB; }
            set { maxFileSizeInMB = value; }
        }

        internal string FullQualifiedFileName
        {
            get { return LogFileFolderName + Path.DirectorySeparatorChar + this.fileName; }
        }
        #endregion

        #region Methods
        public void Init()
        {
            logQueue = new Queue<LogQueueItem>();
            logBackgroundWorker = new BackgroundWorker
                                      {
                                          WorkerSupportsCancellation = true,
                                          WorkerReportsProgress = true
                                      };
            logBackgroundWorker.DoWork += LogBackgroundWorkerDoWork;
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

            if (!this.logBackgroundWorker.IsBusy)
            {
                this.logBackgroundWorker.RunWorkerAsync();
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

        /// <summary>
        /// 
        /// </summary>
        private void RestartLogBackgroundWorker()
        {
            this.logBackgroundWorker.CancelAsync();

            if (!this.logBackgroundWorker.IsBusy)
            {
                this.logBackgroundWorker.RunWorkerAsync();
            }
        }

        private async Task<string> GetLogFile()
        {
            // GET RID OF FILEINFO - NOT GOOD

            string result = await Task.Run(() =>
                   {
                       //CreateNewLogFileDirectoryIfNotExists();           

                       //if (String.IsNullOrEmpty(this.fileName))
                       //{
                       //    DirectoryInfo di = new DirectoryInfo(LogFileFolderName);

                       //    FileInfo fi = di.GetFiles("*.txt").OrderByDescending(p => p.LastWriteTime).FirstOrDefault();

                       //    if (fi != null)
                       //    {
                       //        if (fi.Exists && (fi.Length / 1024 / 1024 < this.MaxFileSizeInMB))
                       //        {
                       //            this.fileName = fi.FullName;

                       //            return this.fileName;
                       //        }
                       //    }
                       //    else
                       //    {
                       //        GenerateNewFileName();
                       //        File.Create(this.fileName);

                       //        return this.fileName;
                       //    }
                       //}

                       //return LogFileFolderName + Path.DirectorySeparatorChar + this.fileName;

                       return "test.txt";
                   });

            return result;
        }

        private void GenerateNewFileName()
        {
            this.fileName = String.Concat(LogFileFolderName, Path.DirectorySeparatorChar, String.Format("BiOWheels_Log-{0}-{1}-{2}T{3}-{4}-{5}-{6}.txt", DateTime.Now.Year, DateTime.Now.Month,
                          DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,
                          DateTime.Now.Millisecond));
        }
        #endregion
    }
}
