//-----------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.Log
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Type of log entry
    /// </summary>
    public enum LoggingType
    {
        /// <summary>
        /// Info log entry
        /// </summary>
        Info,

        /// <summary>
        /// Warning log entry
        /// </summary>
        Warning,

        /// <summary>
        /// Success log entry
        /// </summary>
        Success,

        /// <summary>
        /// Error log entry
        /// </summary>
        Error,

        /// <summary>
        /// Command log entry
        /// </summary>
        Command
    }

    /// <summary>
    /// Defines a log entry 
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Log time
        /// </summary>
        private DateTime logTime;

        /// <summary>
        /// Log message
        /// </summary>
        private string message;

        /// <summary>
        /// Log type
        /// </summary>
        private LoggingType logType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class
        /// </summary>
        /// <param name="msg">Log message</param>
        /// <param name="type">Log type</param>
        public LogEntry(string msg, LoggingType type)
        {
            this.message = msg;
            this.logType = type;
            this.logTime = DateTime.Now;
        }

        /// <summary>
        /// Gets the specified log message
        /// </summary>
        public string Message
        {
            get { return this.message; }
        }

        /// <summary>
        /// Gets the specified log type
        /// </summary>
        public LoggingType LogType
        {
            get { return this.logType; }
        }
    }
}
