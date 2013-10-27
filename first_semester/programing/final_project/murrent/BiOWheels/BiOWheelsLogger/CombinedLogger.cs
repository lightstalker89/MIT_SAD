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
        /// Initializes a new instance of the <see cref="CombinedLogger"/> class
        /// </summary>
        public CombinedLogger()
        {
            ((FileLogger)this.fileLogger).Init();
        }

        #region Methods

        /// <inheritdoc/>
        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            if (typeof(T) == typeof(ConsoleLogger))
            {
                ((ConsoleLogger)this.consoleLogger).IsEnabled = isLoggerEnabled;
            }
            else if (typeof(T) == typeof(FileLogger))
            {
                ((FileLogger)this.fileLogger).IsEnabled = isLoggerEnabled;
            }
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(double logFileSize)
        {
            if (typeof(T) == typeof(FileLogger))
            {
                ((FileLogger)this.fileLogger).MaxFileSizeInMB = logFileSize;
            }
        }

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            this.consoleLogger.Log(message, messageType);
            this.fileLogger.Log(message, messageType);
        }

        #endregion
    }
}