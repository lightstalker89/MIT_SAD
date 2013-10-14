namespace BiOWheelsLogger
{
    public class LoggingEngine
    {
        private readonly ILogger logger;

        #region Properties
        private long fileSize;
        public long FileSize
        {
            get { return fileSize; }
            set
            {
                fileSize = value;
                this.logger.FileSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the filename for the log file. Overrides the default filename
        /// </summary>
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                this.logger.IsEnabled = value;
            }
        }
        #endregion

        /// <summary>
        /// Constructor for a default logger
        /// </summary>
        public LoggingEngine()
        {
            this.logger = new ConsoleLogger();
        }

        /// <summary>
        /// Constructor for injecting a different ILogger
        /// </summary>
        /// <param name="logger">a logger</param>
        public LoggingEngine(ILogger logger)
        {
            this.logger = logger;
        }

        #region Methods
        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="messageType">messageType</param>
        public void Log(string message, MessageType messageType)
        {
            this.logger.Log(message, messageType);
        }
        #endregion
    }
}
