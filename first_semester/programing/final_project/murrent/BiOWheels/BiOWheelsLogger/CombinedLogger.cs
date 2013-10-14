namespace BiOWheelsLogger
{
    public class CombinedLogger : ILogger
    {
        private readonly ILogger consoleLogger = new ConsoleLogger();
        private readonly ILogger fileLogger = new ConsoleLogger();

        #region Properties
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.isEnabled = value; }
        }

        private long fileSize;
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
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
