// *******************************************************
// * <copyright file="UpdateProgressEventArgs.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.CustomEventArgs
{
    using System;

    /// <summary>
    /// Class representing the event args for the progress update event
    /// </summary>
    public class UpdateProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProgressEventArgs"/> class
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public UpdateProgressEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }
    }
}