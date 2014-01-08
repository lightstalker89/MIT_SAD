// *******************************************************
// * <copyright file="ExceptionFileWatcherEventArgs.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxFileWatcher.Events
{
    using System;

    /// <summary>
    /// Class representing the <see cref="ExceptionFileWatcherEventArgs"/>
    /// </summary>
    public class ExceptionFileWatcherEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFileWatcherEventArgs"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of the exception.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        public ExceptionFileWatcherEventArgs(Type exceptionType, string exceptionMessage)
        {
            this.ExceptionType = exceptionType;
            this.ExceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Gets the type of the exception
        /// </summary>
        public Type ExceptionType { get; private set; }

        /// <summary>
        /// Gets the message of the exception
        /// </summary>
        public string ExceptionMessage { get; private set; }
    }
}
