// *******************************************************
// * <copyright file="FileHandlesErrorEventArgs.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileHandleWrapper.CustomEventArgs
{
    using System;

    /// <summary>
    /// The <see ref="FileHandlesErrorEventArgs"/> class and its interaction logic 
    /// </summary>
    public class FileHandlesErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileHandlesErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public FileHandlesErrorEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
    }
}