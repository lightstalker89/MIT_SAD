// *******************************************************
// * <copyright file="LogFileMessage.cs" company="FGrill">
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
    /// Class representing the <see cref="LogFileMessage"/>
    /// </summary>
    public static class LogFileMessage
    {
        /// <summary>
        /// Converts the incoming message to an human readable log file string
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="messageType">type of the message</param>
        /// <returns>Returns a formatted log message.</returns>
        public static string ToLogFileString(this string message, MessageType messageType)
        {
            return string.Concat(messageType, string.Format(" {0:dd.MM.yyyy H:mm:ss zzz}", DateTime.Now), " - " + message);
        }
    }
}
