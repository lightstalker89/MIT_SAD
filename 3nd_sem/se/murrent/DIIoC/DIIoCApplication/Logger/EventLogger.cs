// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using System.Diagnostics;
using DIIoCApplication.Interfaces;
using DIIoCApplication.Models;
using DIIoCApplication.Properties;

namespace DIIoCApplication.Logger
{
    public class EventLogger : ILogger
    {
        private string eventSource;
        private readonly EventLog objEventLog;
        private readonly bool canWriteToEventLog;

        public EventLogger()
        {
            eventSource = Settings.Default["ApplicationName"].ToString();
            try
            {
                objEventLog = new EventLog();
                if (!EventLog.SourceExists(eventSource))
                {
                    EventLog.CreateEventSource(eventSource, "DIIoC");
                }
                canWriteToEventLog = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("No Access");
                canWriteToEventLog = false;
            }
        }

        public void Log(string message, Enums.LogType logType)
        {
            if (canWriteToEventLog)
            {
                objEventLog.Source = eventSource;
                EventLogEntryType entryType = EventLogEntryType.Information;
                if (logType == Enums.LogType.WARN)
                {
                    entryType = EventLogEntryType.Warning;
                }
                else if (logType == Enums.LogType.ERROR)
                {
                    entryType = EventLogEntryType.Error;
                }
                objEventLog.WriteEntry(message, entryType);
            }
        }
    }
}
