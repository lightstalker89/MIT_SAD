using System;
using System.IO;

namespace BiOWheelsLogger
{
    public class FileLogger : ILogger
    {
        #region Properties
        private bool isEnabled;
        internal bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        private long fileSize;
        internal long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            this.isEnabled = isLoggerEnabled;
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(long logFileSize)
        {
            this.fileSize = logFileSize;
        }

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            if (!String.IsNullOrEmpty(message) && this.isEnabled)
            {
                using (StreamWriter streamWriter = new StreamWriter("", true))
                {
                    streamWriter.WriteLine(message.ToLogFileString(messageType));
                }
            }
        }

        private void CheckFileSize()
        {
            FileInfo fi = new FileInfo("");
            double filesSize = fi.Length * Math.Sqrt(1024);

            if(fi.Exists)
            {
                
            }
            else
            {
                
            }

        }

        private void GetFileName()
        {

        }
        #endregion
    }
}
