// *******************************************************
// * <copyright file="ConsoleLogger.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsLogger
{
    using System;

    /// <summary>
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        #region Properties

        /// <summary>
        /// </summary>
        private bool isEnabled;

        /// <summary>
        /// </summary>
        internal bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public void SetIsEnabled<T>(bool isLoggerEnabled)
        {
            this.isEnabled = isLoggerEnabled;
        }

        /// <inheritdoc/>
        public void SetFileSize<T>(double logFileSize)
        {
        }

        /// <inheritdoc/>
        public void Log(string message, MessageType messageType)
        {
            if (!string.IsNullOrEmpty(message) && this.IsEnabled)
            {
                Console.WriteLine(message.ToLogFileString(messageType));
            }
        }

        #endregion
    }
}