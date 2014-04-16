// *******************************************************
// * <copyright file="ILogger.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxLogger
{
    /// <summary>
    /// Interface representing the logger
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageType">Type of the message.</param>
        void Log(string message, MessageType messageType);

        /// <summary>
        /// Sets the size of the log file.
        /// </summary>
        /// <typeparam name="T">The instance of the logger.</typeparam>
        /// <param name="logFileSize">Size of the log file.</param>
        void SetFileSize<T>(long logFileSize);
    }
}
