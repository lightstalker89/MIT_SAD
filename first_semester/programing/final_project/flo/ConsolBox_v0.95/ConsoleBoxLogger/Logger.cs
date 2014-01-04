// *******************************************************
// * <copyright file="Logger.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxLogger
{
    using System;

    /// <summary>
    /// Class representing the <see cref="Logger"/>
    /// </summary>
    public class Logger : ILogger
    {

        /// <summary>
        /// The console logger
        /// </summary>
        private readonly ILogger consoleLogger = new ConsoleLogger();

        /// <summary>
        /// The file logger
        /// </summary>
        private readonly ILogger fileLogger = new FileLogger();

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            try
            {
                this.consoleLogger.Log(message, messageType);
                this.fileLogger.Log(message, messageType);
            }
            catch (Exception e)
            {
                this.consoleLogger.Log(e.Message, messageType);
            }
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(long logFileSize)
        {
            if (typeof(T) == typeof(FileLogger))
            {
                this.fileLogger.SetFileSize<T>(logFileSize);
            }
        }
    }
}
