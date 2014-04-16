// *******************************************************
// * <copyright file="ConsoleLogger.cs" company="FGrill">
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
    /// Class representing the <see cref="ConsoleLogger"/>
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            Console.WriteLine(message.ToLogFileString(messageType));
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(long logFileSize)
        {
        }
    }
}
