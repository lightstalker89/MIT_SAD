// *******************************************************
// * <copyright file="StringHelper.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
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
    /// Class representing the string helper methods for the <see cref="FileLogger"/>
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Converts the incoming message to an human readable log file string
        /// </summary>
        /// <param name="input">
        /// error message
        /// </param>
        /// <param name="messageType">
        /// type of the message
        /// </param>
        /// <returns>
        /// The formatted string for a log file entry
        /// </returns>
        public static string ToLogFileString(this string input, MessageType messageType)
        {
            return string.Concat(
                string.Format("{0:dd.MM.yyyy H:mm:ss:ffff}", DateTime.Now), " [", messageType, "] - " + input);
        }
    }
}