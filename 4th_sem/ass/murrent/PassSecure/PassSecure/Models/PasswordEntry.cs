#region File Header
// <copyright file="PasswordEntry.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Models
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public class PasswordEntry
    {
        /// <summary>
        /// </summary>
        public TimeSpan TotalTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan KeyUpTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan KeyDownTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan TotalKeyDownTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan TotalKeyUpTime { get; set; }
    }
}
