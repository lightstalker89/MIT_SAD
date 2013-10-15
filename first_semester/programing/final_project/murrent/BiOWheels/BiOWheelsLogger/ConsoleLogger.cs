using System;

namespace BiOWheelsLogger
{
    public class ConsoleLogger : ILogger
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
            if (!String.IsNullOrEmpty(message) && this.IsEnabled)
            {
                Console.WriteLine(message.ToLogFileString(messageType));
            }
        }
        #endregion
    }
}
