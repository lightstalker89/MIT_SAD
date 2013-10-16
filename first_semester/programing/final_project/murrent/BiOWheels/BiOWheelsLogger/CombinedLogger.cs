namespace BiOWheelsLogger
{
    public class CombinedLogger : ILogger
    {
        private readonly ILogger consoleLogger = new ConsoleLogger();
        private readonly ILogger fileLogger = new FileLogger();

        #region Properties
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set
            {
                this.isEnabled = value;
                this.consoleLogger.IsEnabled = true;
                this.fileLogger.IsEnabled = true;
            }
        }

        private long fileSize;
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            if (typeof(T) == typeof(ConsoleLogger))
            {
                consoleLogger.IsEnabled = isLoggerEnabled;
            }
            else if (typeof(T) == typeof(FileLogger))
            {
                fileLogger.IsEnabled = isLoggerEnabled;
            }
        }

        #endregion

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            if (this.IsEnabled)
            {
                consoleLogger.Log(message, messageType);
                fileLogger.Log(message, messageType);
            }
        }
    }
}
