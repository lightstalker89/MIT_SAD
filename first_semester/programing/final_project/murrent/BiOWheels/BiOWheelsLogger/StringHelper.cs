// *******************************************************
// * <copyright file="StringHelper.cs" company="MDMCoWorks">
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
    ///
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Converts the incoming message to an human readable logfile string
        /// </summary>
        /// <param name="input">
        /// error message
        /// </param>
        /// <param name="messageType">
        /// type of the message
        /// </param>
        /// <returns>
        /// </returns>
        public static string ToLogFileString(this string input, MessageType messageType)
        {
            return string.Concat(
                string.Format("{0:dd.MM.yyyy H:mm:ss:ffff}", DateTime.Now), " [", messageType, "] - " + input);
        }
    }
}