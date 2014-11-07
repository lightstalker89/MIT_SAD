using System.Diagnostics;
using DIIoCApplication.ExtensionMethods;
using DIIoCApplication.Interfaces;

namespace DIIoCApplication.Logger
{
    public class EventLogger : ILogger
    {
        public void Log(string message, Models.Enums.LogType logType)
        {
            EventLog objEventLog = new EventLog(); 
            objEventLog.WriteEntry(message.ToLogFileFormat(logType));
        }
    }
}
