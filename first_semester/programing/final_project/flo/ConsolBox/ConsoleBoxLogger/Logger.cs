using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBoxLogger
{
    public class Logger : ILogger
    {

        private readonly ILogger consoleLogger = new ConsoleLogger();
        private readonly ILogger fileLogger = new FileLogger();

        private long fileSize;
        public long FileSize
        {
            get { return fileSize; }
            set { 
                fileSize = value;
                fileLogger.FileSize = value;
            }
        }

        public void Log(string message, MessageType messageType)
        {
            consoleLogger.Log(message, messageType);
            fileLogger.Log(message, messageType);
        }
    }
}
