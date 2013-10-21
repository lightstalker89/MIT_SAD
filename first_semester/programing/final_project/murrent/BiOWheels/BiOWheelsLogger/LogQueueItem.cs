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
    /// </summary>
    internal class LogQueueItem
    {
        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="messageType">
        /// </param>
        public LogQueueItem(string message, MessageType messageType)
        {
            this.Message = message;
            this.MessageType = messageType;
        }

        /// <summary>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// </summary>
        /// <returns>
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