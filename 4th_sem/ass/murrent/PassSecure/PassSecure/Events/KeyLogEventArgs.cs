#region File Header
// <copyright file="KeyLogEventArgs.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Events
{
    #region Usings

    using PassSecure.Models;

    #endregion

    /// <summary>
    /// </summary>
    public class KeyLogEventArgs
    {
        /// <summary>
        /// </summary>
        public KeyStroke KeyStroke { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="keyStroke">
        /// </param>
        public KeyLogEventArgs(KeyStroke keyStroke)
        {
            this.KeyStroke = keyStroke;
        }
    }
}
