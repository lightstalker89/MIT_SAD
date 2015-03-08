// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyStroke.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// <author>Mario Murrent</author>
// --------------------------------------------------------------------------------------------------------------------

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
    }
}
