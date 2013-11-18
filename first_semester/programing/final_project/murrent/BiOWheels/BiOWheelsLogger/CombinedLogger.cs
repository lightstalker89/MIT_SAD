// *******************************************************
// * <copyright file="CombinedLogger.cs" company="MDMCoWorks">
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
    /// Initializes a new instance of the <see cref="CombinedLogger"/> class
    /// </summary>
    public class CombinedLogger : ILogger
    {
        /// <summary>
        /// Represents the instance of the <see cref="ConsoleLogger"/> class
        /// </summary>
        private readonly ILogger consoleLogger;

        /// <summary>
        /// Represents the instance of the <see cref="FileLogger"/> class 
        /// </summary>
        private readonly ILogger fileLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinedLogger" /> class
        /// </summary>
        /// <param name="consoleLogger">The console logger.</param>
        /// <param name="fileLogger">The file logger.</param>
        internal CombinedLogger(ILogger consoleLogger, ILogger fileLogger)
        {
            this.consoleLogger = consoleLogger;
            this.fileLogger = fileLogger;

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