#region File Header
// <copyright file="UserTraining.cs" company="">
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

    #endregion

    /// <summary>
    /// </summary>
    public class UserTraining
    {
        /// <summary>
        /// </summary>
        public UserTraining()
        {
            Trainings = new List<TrainingEntry>();
        }

        /// <summary>
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// </summary>
        public List<TrainingEntry> Trainings { get; set; }

        /// <summary>
        /// </summary>
        public double AverageKeyHoldTime { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTotalFirstUpLastUpTime { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTotalFirstDownLastDownTime { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTotalKeyDownTime { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTotalKeyUpTime { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTimeBetweenKeyUp { get; set; }

        /// <summary>
        /// </summary>
        public double AverageTimeBetweenKeyDown { get; set; }

        /// <summary>
        /// </summary>
        public bool AcceptedUserAttempt { get; set; }

        /// <summary>
        /// </summary>
        public List<double> AverageKeyStrokeDownTimes { get; set; }

        /// <summary>
        /// </summary>
        public List<double> AverageKeyStrokeUpTimes { get; set; } 

        /// <summary>
        /// </summary>
        public void Analyze()
        {
            foreach (TrainingEntry training in Trainings)
            {
                training.Analyze();
                AverageTotalFirstDownLastDownTime += training.TotalFirstDownLastDownTime;
                AverageTotalFirstUpLastUpTime += training.TotalFirstUpLastUpTime;
                AverageTimeBetweenKeyUp += training.AverageTimeBetweenKeyUp;
                AverageTimeBetweenKeyDown += training.AverageTimeBetweenKeyDown;
                AverageKeyHoldTime += training.AverageHoldTime;
            }
            AverageTotalFirstUpLastUpTime = Math.Round(AverageTotalFirstUpLastUpTime / Trainings.Count, 5);
            AverageTotalFirstDownLastDownTime = Math.Round(AverageTotalFirstDownLastDownTime / Trainings.Count, 5);
            AverageTimeBetweenKeyUp = Math.Round(AverageTimeBetweenKeyUp / Trainings.Count, 5);
            AverageTimeBetweenKeyDown = Math.Round(AverageTimeBetweenKeyDown / Trainings.Count, 5);
            AverageKeyHoldTime = Math.Round(AverageKeyHoldTime / Trainings.Count, 5);
        }
    }
}
