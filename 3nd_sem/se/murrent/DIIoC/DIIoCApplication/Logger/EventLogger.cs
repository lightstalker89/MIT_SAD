using System;
using System.Diagnostics;
using DIIoCApplication.ExtensionMethods;
using DIIoCApplication.Interfaces;

namespace DIIoCApplication.Logger
{
    public class EventLogger : ILogger
    {
        private readonly EventLog objEventLog;
        private readonly bool canWriteToEventLog;

        public EventLogger()
        {
            try
            {
                objEventLog = new EventLog();
                EventLog.CreateEventSource("DIIoC", "DIIoC");
                canWriteToEventLog = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("No Access");
                canWriteToEventLog = false;
            }
        }
        public void Log(string message, Models.Enums.LogType logType)
        {
            if (canWriteToEventLog)
            {
                objEventLog.WriteEntry(message.ToLogFileFormat(logType));
            }
        }
    }
}
