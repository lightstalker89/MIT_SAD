namespace BiOWheelsLogger
{
    public class CombinedLogger : ILogger
    {
        private readonly ILogger consoleLogger = new ConsoleLogger();
        private readonly ILogger fileLogger = new FileLogger();

        public CombinedLogger()
        {
            ((FileLogger) fileLogger).Init();
        }

        #region Methods
        /// <inheritdoc/>
        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            if (typeof(T) == typeof(ConsoleLogger))
            {
                ((ConsoleLogger)consoleLogger).IsEnabled = isLoggerEnabled;
            }
            else if (typeof(T) == typeof(FileLogger))
            {
                ((FileLogger)fileLogger).IsEnabled = isLoggerEnabled;
            }
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(long logFileSize)
        {
            if (typeof(T) == typeof(FileLogger))
            {
                ((FileLogger)fileLogger).MaxFileSizeInMB = logFileSize;
            }
        }

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            consoleLogger.Log(message, messageType);
            fileLogger.Log(message, messageType);
        }
        #endregion
    }
}
