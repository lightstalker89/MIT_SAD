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
        public int PasswordLength { get; set; }

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
        public double AverageHoldTime { get; set; }

        /// <summary>
        /// </summary>
        public List<KeyStroke> KeyStrokes { get; set; }

        /// <summary>
        /// </summary>
        public double[] KeyStrokeDownTimes { get; set; }

        /// <summary>
        /// </summary>
        public double[] KeyStrokeUpTimes { get; set; }

        /// <summary>
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// </summary>
        public void Analyze()
        {
            AverageTimeBetweenKeyUp = 0;
            AverageTimeBetweenKeyDown = 0;
            AverageHoldTime = 0;
            KeyStrokeDownTimes = new double[PasswordLength - 1];
            KeyStrokeUpTimes = new double[PasswordLength - 1];
            if (KeyStrokes.Count > 1)
            {
                TotalFirstDownLastDownTime = KeyStrokes.Last().KeyDownTime.TotalMilliseconds - KeyStrokes.First().KeyDownTime.TotalMilliseconds;
                TotalFirstUpLastUpTime = KeyStrokes.Last().KeyUpTime.TotalMilliseconds - KeyStrokes.First().KeyUpTime.TotalMilliseconds;
            }
            for (int i = 1; i < KeyStrokes.Count; i++)
            {
                int lastIndex = i - 1;
                KeyStrokes[i].TimeToLastKeyDown = KeyStrokes[i].KeyDownTime - KeyStrokes[lastIndex].KeyDownTime;
                KeyStrokes[i].TimeToLastKeyUp = KeyStrokes[i].KeyUpTime - KeyStrokes[lastIndex].KeyUpTime;


                KeyStrokeDownTimes[i - 1] += KeyStrokes[i].KeyDownTime.TotalMilliseconds
                                            - KeyStrokes[i - 1].KeyDownTime.TotalMilliseconds;
                KeyStrokeUpTimes[i - 1] += KeyStrokes[i].KeyUpTime.TotalMilliseconds
                                          - KeyStrokes[i - 1].KeyUpTime.TotalMilliseconds;
                AverageTimeBetweenKeyDown += KeyStrokes[i].TimeToLastKeyDown.TotalMilliseconds;
                AverageTimeBetweenKeyUp += KeyStrokes[i].TimeToLastKeyUp.TotalMilliseconds;
                AverageHoldTime += KeyStrokes[i].TimeBetweenDownAndUp.TotalMilliseconds;
            }
            if (KeyStrokes.Count > 0)
            {
                AverageTimeBetweenKeyDown = Math.Round((AverageTimeBetweenKeyDown / KeyStrokes.Count), 5);
                AverageTimeBetweenKeyUp = Math.Round(AverageTimeBetweenKeyUp / KeyStrokes.Count, 5);
                AverageHoldTime = Math.Round(AverageHoldTime / KeyStrokes.Count, 5);
            }
        }
    }
}
