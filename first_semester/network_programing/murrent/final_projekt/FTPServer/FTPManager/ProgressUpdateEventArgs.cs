// *******************************************************
// * <copyright file="ProgressUpdateEventArgs.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace FTPManager
{
    using System;

    /// <summary>
    /// </summary>
    public class ProgressUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        public ProgressUpdateEventArgs(string message)
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