using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBoxLogger
{
    public class ConsoleLogger : ILogger
    {
        public long FileSize { get; set; }

        public void Log(string message, MessageType messageType)
        {
            Console.WriteLine(message.ToLogFileString(messageType));
        }
    }
}
