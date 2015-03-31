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

        ///// <summary>
        ///// </summary>
        //private void CheckLists()
        //{
        //    if (AverageKeyStrokeDownTimes == null) { AverageKeyStrokeDownTimes = new List<double>();}
        //    if (AverageKeyStrokeUpTimes == null) { AverageKeyStrokeUpTimes = new List<double>();}
        //}

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
            //AverageKeyStrokeDownTimes = new List<double>();
            //AverageKeyStrokeUpTimes = new List<double>();
            //CheckLists();
            for (int i = 0; i < Trainings.Count; i++)
            {
                Trainings[i].PasswordLength = Password.Length;
                Trainings[i].KeyStrokeUpTimes = new double[Password.Length];
                Trainings[i].KeyStrokeDownTimes = new double[Password.Length];
                Trainings[i].Analyze();
                //double valueDown = AverageKeyStrokeDownTimes.ElementAtOrDefault(i);
                //double valueUp = AverageKeyStrokeUpTimes.ElementAtOrDefault(i);
                //if (valueDown == 0.0)
                //{
                //    AverageKeyStrokeDownTimes.Insert(i, 0);
                //}
                //if (valueUp == 0.0)
                //{
                //    AverageKeyStrokeUpTimes.Insert(i, 0);
                //}
                //for (int x = 0; x < Trainings[i].KeyStrokeDownTimes.Count; x++)
                //{
                //    AverageKeyStrokeDownTimes[i] += Trainings[i].KeyStrokeDownTimes[x];
                //}
                //for (int y = 0; y < Trainings[i].KeyStrokeUpTimes.Count; y++)
                //{
                //    AverageKeyStrokeUpTimes[i] += Trainings[i].KeyStrokeUpTimes[y];                    
                //}
                AverageDistance += Trainings[i].Distance;
                AverageTotalFirstDownLastDownTime += Trainings[i].TotalFirstDownLastDownTime;
                AverageTotalFirstUpLastUpTime += Trainings[i].TotalFirstUpLastUpTime;
                AverageTimeBetweenKeyUp += Trainings[i].AverageTimeBetweenKeyUp;
                AverageTimeBetweenKeyDown += Trainings[i].AverageTimeBetweenKeyDown;
                AverageKeyHoldTime += Trainings[i].AverageHoldTime;
                //if (i == Trainings.Count - 1)
                //{
                //    AverageKeyStrokeDownTimes[i] = AverageKeyStrokeDownTimes[i] / Trainings[i].KeyStrokeDownTimes.Count;
                //    AverageKeyStrokeUpTimes[i] = AverageKeyStrokeUpTimes[i] / Trainings[i].KeyStrokeUpTimes.Count;
                //}
            }
            AverageTotalFirstUpLastUpTime = Math.Round(AverageTotalFirstUpLastUpTime / Trainings.Count, 5);
            AverageTotalFirstDownLastDownTime = Math.Round(AverageTotalFirstDownLastDownTime / Trainings.Count, 5);
            AverageTimeBetweenKeyUp = Math.Round(AverageTimeBetweenKeyUp / Trainings.Count, 5);
            AverageTimeBetweenKeyDown = Math.Round(AverageTimeBetweenKeyDown / Trainings.Count, 5);
            AverageKeyHoldTime = Math.Round(AverageKeyHoldTime / Trainings.Count, 5);
            AverageDistance = AverageDistance / Trainings.Count;
        }
    }
}
