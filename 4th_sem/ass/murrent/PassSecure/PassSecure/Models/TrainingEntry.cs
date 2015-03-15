#region File Header
// <copyright file="TrainingEntry.cs" company="">
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
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    /// <summary>
    /// </summary>
    public class TrainingEntry
    {
        /// <summary>
        /// </summary>
        public TrainingEntry()
        {
            KeyStrokes = new List<KeyStroke>();
        }

        /// <summary>
        /// </summary>
        public int TrainingId { get; set; }

        /// <summary>
        /// </summary>
        public int Errors { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTimeBetweenKeyUp { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTimeBetweenKeyDown { get; set; }

        /// <summary>
        /// </summary>
        public double TotalFirstUpLastUpTime { get; set; }

        /// <summary>
        /// </summary>
        public double TotalFirstDownLastDownTime { get; set; }

        /// <summary>
        /// </summary>
        public List<KeyStroke> KeyStrokes { get; set; }

        /// <summary>
        /// </summary>
        public void Analyze()
        {
            if (KeyStrokes.Count > 1)
            {
                TotalFirstDownLastDownTime = KeyStrokes.Last().KeyDownTime.Ticks - KeyStrokes.First().KeyDownTime.Ticks;
                TotalFirstUpLastUpTime = KeyStrokes.Last().KeyUpTime.Ticks - KeyStrokes.First().KeyUpTime.Ticks;
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
