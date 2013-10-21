namespace BiOWheelsLogger
{
    public interface ILogger
    {
        /// <summary>
        /// Write log entry
        /// </summary>
        /// <param name="message">message to write</param>
        /// <param name="messageType">messageType for the message</param>
        void Log(string message, MessageType messageType);

        /// <summary>
        /// Sets the state of the logger
        /// </summary>
        /// <typeparam name="T">Type of the logger</typeparam>
        /// <param name="isLoggerEnabled">Status</param>
        void SetIsEnabled<T>(bool isLoggerEnabled);

        /// <summary>
        /// Sets the filesize of the logfile
        /// </summary>
        /// <typeparam name="T">Type of the logger</typeparam>
        /// <param name="logFileSize">Logfile size</param>
        void SetFileSize<T>(double logFileSize);
    }
}
