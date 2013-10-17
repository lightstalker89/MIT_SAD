namespace BiOWheelsLogger
{
    internal class LogQueueItem
    {
        public LogQueueItem(string message, MessageType messageType)
        {
            this.Message = message;
            this.MessageType = messageType;
        }
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
    }
}
