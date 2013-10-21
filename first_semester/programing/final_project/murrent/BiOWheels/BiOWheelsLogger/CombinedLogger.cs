// *******************************************************
// * <copyright file="CombinedLogger.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsLogger
{
    /// <summary>
    /// </summary>
    public class CombinedLogger : ILogger
    {
        /// <summary>
        /// </summary>
        private readonly ILogger consoleLogger = new ConsoleLogger();

        /// <summary>
        /// </summary>
        private readonly ILogger fileLogger = new FileLogger();

        /// <summary>
        /// </summary>
        public CombinedLogger()
        {
            ((FileLogger)fileLogger).Init();
        }

        #region Methods

        /// <inheritdoc/>
        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            if (typeof(T) == typeof(ConsoleLogger))
            {
                ((ConsoleLogger)consoleLogger).IsEnabled = isLoggerEnabled;
            }
            else if (typeof(T) == typeof(FileLogger))
            {
                ((FileLogger)fileLogger).IsEnabled = isLoggerEnabled;
            }
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(double logFileSize)
        {
            if (typeof(T) == typeof(FileLogger))
            {
                ((FileLogger)fileLogger).MaxFileSizeInMB = logFileSize;
            }
        }

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            consoleLogger.Log(message, messageType);
            fileLogger.Log(message, messageType);
        }

        #endregion
    }
}