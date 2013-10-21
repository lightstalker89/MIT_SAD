using System;

namespace BiOWheelsLogger
{
    public class ConsoleLogger : ILogger
    {
        #region Properties
        private bool isEnabled;
        internal bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            this.isEnabled = isLoggerEnabled;
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(double logFileSize) { }

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
