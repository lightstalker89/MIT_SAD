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
            KeyStrokeDownTimes = new List<double>();
            KeyStrokeUpTimes = new List<double>();
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
        public double AverageHoldTime { get; set; }

        /// <summary>
        /// </summary>
        public List<KeyStroke> KeyStrokes { get; set; }

        /// <summary>
        /// </summary>
        public List<double> KeyStrokeDownTimes { get; set; }

        /// <summary>
        /// </summary>
        public List<double> KeyStrokeUpTimes { get; set; } 

        /// <summary>
        /// </summary>
        public void Analyze()
        {
            AverageTimeBetweenKeyUp = 0;
            AverageTimeBetweenKeyDown = 0;
            AverageHoldTime = 0;
            //KeyStrokeDownTimes = new List<double>();
            //KeyStrokeUpTimes = new List<double>();
            if (KeyStrokes.Count > 1)
            {
                TotalFirstDownLastDownTime = KeyStrokes.Last().KeyDownTime.TotalMilliseconds - KeyStrokes.First().KeyDownTime.TotalMilliseconds;
                TotalFirstUpLastUpTime = KeyStrokes.Last().KeyUpTime.TotalMilliseconds - KeyStrokes.First().KeyUpTime.TotalMilliseconds;
            }
            //double valueZeroUp = KeyStrokeUpTimes.ElementAtOrDefault(0);
            //if (valueZeroUp == 0.0)
            //{
            //    KeyStrokeUpTimes.Insert(0, 0);
            //}
            //double valueZeroDown = KeyStrokeDownTimes.ElementAtOrDefault(0);
            //if (valueZeroDown == 0.0)
            //{
            //    KeyStrokeDownTimes.Insert(0, 0);
            //}
            for (int i = 1; i < KeyStrokes.Count; i++)
            {
                int lastIndex = i - 1;
                KeyStrokes[i].TimeToLastKeyDown = KeyStrokes[i].KeyDownTime - KeyStrokes[lastIndex].KeyDownTime;
                KeyStrokes[i].TimeToLastKeyUp = KeyStrokes[i].KeyUpTime - KeyStrokes[lastIndex].KeyUpTime;
                //double valueDown = KeyStrokeDownTimes.ElementAtOrDefault(i);
                //double valueUp = KeyStrokeUpTimes.ElementAtOrDefault(i);
                //if (valueDown == 0.0)
                //{
                //    KeyStrokeDownTimes.Insert(i, 0);
                //}
                //if (valueUp == 0.0)
                //{
                //    KeyStrokeUpTimes.Insert(i, 0);
                //}
                //KeyStrokeDownTimes.Insert(i, KeyStrokes[i].TimeToLastKeyDown.TotalMilliseconds);
                //KeyStrokeUpTimes.Insert(i, KeyStrokes[i].TimeToLastKeyUp.TotalMilliseconds);
                AverageTimeBetweenKeyDown += KeyStrokes[i].TimeToLastKeyDown.TotalMilliseconds;
                AverageTimeBetweenKeyUp += KeyStrokes[i].TimeToLastKeyUp.TotalMilliseconds;
                AverageHoldTime += KeyStrokes[i].TimeBetweenDownAndUp.TotalMilliseconds;
            }
            AverageTimeBetweenKeyDown = Math.Round((AverageTimeBetweenKeyDown / KeyStrokes.Count), 5);
            AverageTimeBetweenKeyUp = Math.Round(AverageTimeBetweenKeyUp / KeyStrokes.Count, 5);
            AverageHoldTime = Math.Round(AverageHoldTime / KeyStrokes.Count, 5);
        }
    }
}
