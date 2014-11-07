using System.Diagnostics;
using DIIoCApplication.ExtensionMethods;
using DIIoCApplication.Interfaces;

namespace DIIoCApplication.Logger
{
    public class EventLogger : ILogger
    {
        private readonly EventLog objEventLog;

        public EventLogger()
        {
            objEventLog = new EventLog();
            EventLog.CreateEventSource("DIIoC", "DIIoC");
        }
        public void Log(string message, Models.Enums.LogType logType)
        {
            objEventLog.WriteEntry(message.ToLogFileFormat(logType));
        }
    }
}
