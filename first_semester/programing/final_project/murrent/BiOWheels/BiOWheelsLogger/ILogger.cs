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

        long FileSize { get; set; }

        void SetIsEnabled<T>(bool isLoggerEnabled);
    }
}
