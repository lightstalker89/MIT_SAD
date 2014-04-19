// *******************************************************
// * <copyright file="ILogger.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsLogger
{
    /// <summary>
    /// Interface representing the must implement methods
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Write log entry
        /// </summary>
        /// <param name="message">
        /// message to write
        /// </param>
        /// <param name="messageType">
        /// messageType for the message
        /// </param>
        void Log(string message, MessageType messageType);

        /// <summary>
        /// Sets the state of the logger
        /// </summary>
        /// <typeparam name="T">
        /// Type of the logger
        /// </typeparam>
        /// <param name="isLoggerEnabled">
        /// Status of the logger
        /// </param>
        void SetIsEnabled<T>(bool isLoggerEnabled);

        /// <summary>
        /// Sets the file size of the log file
        /// </summary>
        /// <typeparam name="T">
        /// Type of the logger
        /// </typeparam>
        /// <param name="logFileSize">
        /// Log file size
        /// </param>
        void SetFileSize<T>(double logFileSize);
    }
}