// *******************************************************
// * <copyright file="QueueMessage.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxLogger
{
    /// <summary>
    /// Class representing the <see cref="QueueMessage"/>
    /// </summary>
    public class QueueMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueMessage"/> class.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="messageType">Type of the log message.</param>
        public QueueMessage(string message, MessageType messageType)
        {
            this.MessageType = messageType;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the log message.
        /// </summary>
        /// <value>
        /// The log message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type of the log message.
        /// </summary>
        /// <value>
        /// The type of the log message.
        /// </value>
        public MessageType MessageType { get; set; }
    }
}
