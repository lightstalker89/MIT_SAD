using System;

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

        public override string ToString()
        {
            return String.Concat(String.Format("{0:dd.MM.yyyy H:mm:ss:ffff}", DateTime.Now), " [", this.MessageType, "] - " + this.Message);
        }
    }
}
