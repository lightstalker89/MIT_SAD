#region File Header
// <copyright file="PasswordEntry.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion

using System.Collections.Generic;
using System.Linq;

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
        public TimeSpan TotalFirstUpLastUpTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan TotalFirstDownLastDownTime { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTimeBetweenKeyUp { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTimeBetweenKeyDown { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan TotalKeyDownTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan TotalKeyUpTime { get; set; }

        /// <summary>
        /// </summary>
        public List<KeyStroke> KeyStrokes { get; set; }

        /// <summary>
        /// </summary>
        public void Analyze()
        {
            if (KeyStrokes.Count > 1)
            {
                TotalFirstDownLastDownTime = KeyStrokes.Last().KeyDownTime - KeyStrokes.First().KeyDownTime;
                TotalFirstUpLastUpTime = KeyStrokes.Last().KeyUpTime - KeyStrokes.First().KeyUpTime;
            }

            for (int i = 1; i < KeyStrokes.Count; i++)
            {
                int lastIndex = i - 1;
                KeyStrokes[i].TimeToLastKeyDown = KeyStrokes[i].KeyDownTime - KeyStrokes[lastIndex].KeyDownTime;
                KeyStrokes[i].TimeToLastKeyUp = KeyStrokes[i].KeyUpTime - KeyStrokes[lastIndex].KeyUpTime;
                AverageTimeBetweenKeyDown += KeyStrokes[i].TimeToLastKeyDown.Ticks;
                AverageTimeBetweenKeyUp += KeyStrokes[i].TimeToLastKeyUp.Ticks;
            }
            AverageTimeBetweenKeyDown = AverageTimeBetweenKeyDown / KeyStrokes.Count;
            AverageTimeBetweenKeyUp = AverageTimeBetweenKeyUp / KeyStrokes.Count;
        }
    }
}
