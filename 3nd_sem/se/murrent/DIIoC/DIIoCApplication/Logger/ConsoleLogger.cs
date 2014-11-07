using System;
using DIIoCApplication.ExtensionMethods;
using DIIoCApplication.Interfaces;

namespace DIIoCApplication.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, Models.Enums.LogType logType)
        {
            Console.WriteLine(message.ToLogFileFormat(logType));
        }
    }
}
