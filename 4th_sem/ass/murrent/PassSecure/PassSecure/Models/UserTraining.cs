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
    using System.Linq;

    using PassSecure.Service;

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
        public double[] AverageKeyStrokeDownTimes { get; set; }

        /// <summary>
        /// </summary>
        public double[] AverageKeyStrokeUpTimes { get; set; }

        /// <summary>
        /// </summary>
        public double AverageDistance { get; set; }

        /// <summary>
        /// </summary>
        public void Analyze()
        {
            AverageKeyStrokeDownTimes = new double[Password.Length];
            AverageKeyStrokeUpTimes = new double[Password.Length];
            AverageTotalFirstDownLastDownTime = 0;
            AverageTotalFirstUpLastUpTime = 0;
            AverageTimeBetweenKeyUp = 0;
            AverageTimeBetweenKeyDown = 0;
            AverageKeyHoldTime = 0;
            AverageDistance = 0;
            for (int i = 0; i < Trainings.Count; i++)
            {
                Trainings[i].PasswordLength = Password.Length;
                Trainings[i].KeyStrokeUpTimes = new double[Password.Length - 1];
                Trainings[i].KeyStrokeDownTimes = new double[Password.Length - 1];
                Trainings[i].Analyze();
                AverageDistance += Trainings[i].Distance;
                AverageTotalFirstDownLastDownTime += Trainings[i].TotalFirstDownLastDownTime;
                AverageTotalFirstUpLastUpTime += Trainings[i].TotalFirstUpLastUpTime;
                AverageTimeBetweenKeyUp += Trainings[i].AverageTimeBetweenKeyUp;
                AverageTimeBetweenKeyDown += Trainings[i].AverageTimeBetweenKeyDown;
                AverageKeyHoldTime += Trainings[i].AverageHoldTime;
                for (int x = 0; x < Password.Length - 1; x++)
                {
                    AverageKeyStrokeDownTimes[x] += Trainings[i].KeyStrokeDownTimes[x];
                    AverageKeyStrokeUpTimes[x] += Trainings[i].KeyStrokeUpTimes[x];
                }

            }
            if (Trainings.Count > 0)
            {
                AverageTotalFirstUpLastUpTime = Math.Round(AverageTotalFirstUpLastUpTime / Trainings.Count, 5);
                AverageTotalFirstDownLastDownTime = Math.Round(AverageTotalFirstDownLastDownTime / Trainings.Count, 5);
                AverageTimeBetweenKeyUp = Math.Round(AverageTimeBetweenKeyUp / Trainings.Count, 5);
                AverageTimeBetweenKeyDown = Math.Round(AverageTimeBetweenKeyDown / Trainings.Count, 5);
                AverageKeyHoldTime = Math.Round(AverageKeyHoldTime / Trainings.Count, 5);
                AverageDistance = AverageDistance / Trainings.Count;
                for (int y = 0; y < Password.Length - 1; y++)
                {
                    AverageKeyStrokeDownTimes[y] = AverageKeyStrokeDownTimes[y] / Trainings.Count;
                    AverageKeyStrokeUpTimes[y] = AverageKeyStrokeUpTimes[y] / Trainings.Count;
                }
            }
        }
    }
}
