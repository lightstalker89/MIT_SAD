// *******************************************************
// * <copyright file="ExceptionDataManagerEventArgs.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxDataManager.Events
{
    using System;

    /// <summary>
    /// Class representing the <see cref="ExceptionDataManagerEventArgs"/>
    /// </summary>
    public class ExceptionDataManagerEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDataManagerEventArgs"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of the exception.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        public ExceptionDataManagerEventArgs(Type exceptionType, string exceptionMessage)
        {
            this.ExceptionType = exceptionType;
            this.ExceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Gets or sets the type of the exception
        /// </summary>
        public Type ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the message of the exception
        /// </summary>
        public string ExceptionMessage { get; set; }
    }
}
