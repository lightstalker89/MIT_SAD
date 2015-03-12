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
    using System.Threading.Tasks;

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
        public TimeSpan AveragePasswordTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan AverageKeyUpTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan AverageKeyDownTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan AverageTotalKeyDownTime { get; set; }

        /// <summary>
        /// </summary>
        public TimeSpan AverageTotalKeyUpTime { get; set; }

        /// <summary>
        /// </summary>
        public void Analyze()
        {
            Parallel.ForEach(Trainings, (entry, state) => entry.Analyze());
        }
    }
}
