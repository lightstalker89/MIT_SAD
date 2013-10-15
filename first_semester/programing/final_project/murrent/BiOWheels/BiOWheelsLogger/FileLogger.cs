using System;
using System.IO;

namespace BiOWheelsLogger
{
    public class FileLogger : ILogger
    {
        #region Properties
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        private long fileSize;
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            if (!String.IsNullOrEmpty(message))
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
        }

        private void GetFileName()
        {

        }
        #endregion
    }
}
