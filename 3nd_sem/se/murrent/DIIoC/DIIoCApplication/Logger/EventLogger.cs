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
        
        public void Log(string message, Enums.LogType logType)
        {
            if (canWriteToEventLog)
            {
                objEventLog.Source = Settings.Default["ApplicationName"].ToString();
                EventLogEntryType entryType = EventLogEntryType.Information;
                if (logType == Enums.LogType.WARN)
                {
                    entryType = EventLogEntryType.Warning;
                }
                else if (logType == Enums.LogType.ERROR)
                {
                    entryType = EventLogEntryType.Error;
                }
                objEventLog.WriteEntry(message, EventLogEntryType.Information);
            }
        }
    }
}
