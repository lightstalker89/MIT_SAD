#region File Header
// <copyright file="PasswordAnalyzer.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Service
{
    #region Usings

    using System;
    using System.Linq;

    using PassSecure.Data;
    using PassSecure.ExtensionMethods;
    using PassSecure.Models;
    using PassSecure.Utils;

    #endregion

    /// <summary>
    /// </summary>
    public class PasswordAnalyzer
    {
        /// <summary>
        /// </summary>
        private readonly DataStore dataStore;

        /// <summary>
        /// </summary>
        public PasswordAnalyzer()
        {
            dataStore = SimpleContainer.Resolve<DataStore>();
        }

        /// <summary>
        /// </summary>
        /// <param name="username">
        /// </param>
        /// <param name="passwordEntry">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsAccepted(string username, PasswordEntry passwordEntry)
        {
            bool accepted = false;
            UserTraining userTraining = dataStore.GetUserTraining(username);
            if (userTraining != null)
            {
                byte[] averageTrainingCriterias =
                    ArrayUtils.ConcatArrays(
                        userTraining.AverageTimeBetweenKeyUp.ToByteArray(),
                        userTraining.AverageTimeBetweenKeyDown.ToByteArray(),
                        userTraining.AverageTotalKeyDownTime.ToByteArray());
                byte[] currentTrainingCriterias =
                    ArrayUtils.ConcatArrays(
                        passwordEntry.AverageTimeBetweenKeyUp.ToByteArray(),
                        passwordEntry.AverageTimeBetweenKeyDown.ToByteArray(),
                        passwordEntry.TotalKeyDownTime.ToByteArray());
                double difference = CheckCriteria(currentTrainingCriterias, averageTrainingCriterias);
                if (difference < 0.3)
                {
                    accepted = true;
                }
            }

            return accepted;
        }

        /// <summary>
        /// </summary>
        /// <param name="entryToMatch">
        /// </param>
        /// <param name="averageTrainingCriterias">
        /// </param>
        /// <returns>
        /// </returns>
        private double CheckCriteria(byte[] entryToMatch, byte[] averageTrainingCriterias)
        {
            double spec1 = 0;
            double spec2 = 0;

            // Accumulate the values in the oldFrame array
            double spec1Sum = AccumulateArray(averageTrainingCriterias);

            // Accumulate the values in the newFrame array
            double spec2Sum = AccumulateArray(entryToMatch);
            double kullbackD1 = 0;
            double kullbackD2 = 0;

            // Calculate the Kullback-Leibler-Divergenz
            for (int i = 0; i < entryToMatch.Length; i++)
            {
                spec1 = averageTrainingCriterias[i] / spec1Sum;
                spec2 = entryToMatch[i] / spec2Sum;
                kullbackD1 += spec1 * Math.Log(spec1 / spec2, 2);
                kullbackD2 += spec2 * Math.Log(spec2 / spec1, 2);
            }

            // Return the Kullback-Leibler-Divergenz
            return (kullbackD1 + kullbackD2) / 2;
        }

        /// <summary>
        /// </summary>
        /// <param name="array">
        /// </param>
        /// <returns>
        /// </returns>
        public double AccumulateArray(byte[] array)
        {
            return array.Aggregate<byte, double>(0, (current, t) => current + t);
        }
    }
}
