//-----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.Log
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prototyp.View;

    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets the queue of log entries for the logger
        /// </summary>
        ConcurrentQueue<LogEntry> Logs 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Write log entries to the file
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="logType">Log type</param>
        void WriteToFile(string message, LoggingType logType);

        /// <summary>
        /// Write log entries to the console
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="logType">Log type</param>
        /// <param name="window">Window to log</param>
        void WriteToConsole(string message, LoggingType logType, IWindow window);
    }
}
