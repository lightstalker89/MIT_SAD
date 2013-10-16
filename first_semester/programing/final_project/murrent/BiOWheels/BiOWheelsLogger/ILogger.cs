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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLoggerEnabled"></param>
        void SetIsEnabled<T>(bool isLoggerEnabled);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logFileSize"></param>
        void SetFileSize<T>(long logFileSize);
    }
}
