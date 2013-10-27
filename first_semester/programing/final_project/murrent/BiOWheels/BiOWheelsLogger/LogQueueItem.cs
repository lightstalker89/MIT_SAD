// *******************************************************
// * <copyright file="LogQueueItem.cs" company="MDMCoWorks">
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
    /// Class representing a log queue item
    /// </summary>
    internal class LogQueueItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogQueueItem"/> class
        /// </summary>
        /// <param name="message">
        /// The message for the <see cref="LogQueueItem"/>
        /// </param>
        /// <param name="messageType">
        /// The message type for the <see cref="LogQueueItem"/>
        /// </param>
        public LogQueueItem(string message, MessageType messageType)
        {
            this.Message = message;
            this.MessageType = messageType;
        }

        /// <summary>
        /// Gets or sets the message which should be logged
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MessageType"/> which should be used for the logging
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// ToString method overriding default ToString method
        /// </summary>
        /// <returns>
        /// The custom format for the <see cref="LogQueueItem"/>
        /// </returns>
        public override string ToString()
        {
            return string.Concat(
                string.Format("{0:dd.MM.yyyy H:mm:ss:ffff}", DateTime.Now), 
                " [", 
                this.MessageType, 
                "] - " + this.Message);
        }
    }
}