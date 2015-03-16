#region File Header
// <copyright file="KeyStroke.cs" company="">
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
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// </summary>
    public class KeyStroke
    {
        /// <summary>
        /// </summary>
        public KeyStroke()
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="key">
        /// </param>
        public KeyStroke(Keys key)
        {
            this.Key = key;
        }

        /// <summary>
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan KeyDownTime { get; set; }

        /// <summary>
        /// </summary>
        private TimeSpan keyUpTime;

        /// <summary>
        /// </summary>
        public TimeSpan KeyUpTime
        {
            get
            {
                return keyUpTime;
            }

            set
            {
                keyUpTime = value;
                this.TimeBetweenDownAndUp = KeyUpTime - KeyDownTime;
            }
        }

        /// <summary>
        /// </summary>
        public TimeSpan TimeBetweenDownAndUp { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan TimeToLastKeyUp { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan TimeToLastKeyDown { get; set; }
    }
}
