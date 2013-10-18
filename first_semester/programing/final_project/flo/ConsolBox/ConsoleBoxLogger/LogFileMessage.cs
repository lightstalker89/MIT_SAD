using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBoxLogger
{
    public static class LogFileMessage
    {
        /// <summary>
        /// Converts the incoming message to an human readable logfile string
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="messageType">type of the message</param>
        /// <returns></returns>
        public static string ToLogFileString(this string message, MessageType messageType)
        {
            return String.Concat(messageType + " - " + message, string.Format("{0:dd.MM.yyyy H:mm:ss zzz}", DateTime.Now));
        }
    }
}
