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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Documents;
    using System.Windows.Forms;

    using Accord.Math;

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
        public Enums.PasswordStatus IsAccepted(string username, UserTraining passwordEntry)
        {
            Enums.PasswordStatus status = Enums.PasswordStatus.NotAccepted;
            UserTraining userTraining = dataStore.GetUserTraining(username);
            if (userTraining != null)
            {
                double difference = CalculateDistance(userTraining, passwordEntry);
                //double difference = CalculateEuclideanDistance(userTraining, passwordEntry);
                double diff = Math.Abs(difference - userTraining.AverageDistance);
                if (difference <= userTraining.AverageDistance)
                {
                    status = Enums.PasswordStatus.Accepted;
                }
                //else if (difference > userTraining.AverageDistance && difference <= (userTraining.AverageDistance + userTraining.AverageDistance / 5))
                //{
                //    status = Enums.PasswordStatus.PartialAccepted;
                //}
                Debug.WriteLine(status);
                Debug.WriteLine("Manhattan distance: " + difference);
                Debug.WriteLine("Average manhattan distance: " + userTraining.AverageDistance);
                //Debug.WriteLine("Euclidean distance: " + eucDifference);
            }

            return status;
        }

        public double CalculateDistance(UserTraining userTraining, UserTraining passwordEntry)
        {
            double[] manhattanData = {    
                                                    userTraining.AverageKeyHoldTime,
                                                    userTraining.AverageTimeBetweenKeyUp,
                                                    userTraining.AverageTimeBetweenKeyDown,
                                                    userTraining.AverageTotalFirstDownLastDownTime,
                                                    userTraining.AverageTotalFirstUpLastUpTime
                                     };
            if (userTraining.AverageKeyStrokeDownTimes != null && userTraining.AverageKeyStrokeUpTimes != null)
            {
                ArrayUtils.ConcatArrays(manhattanData, userTraining.AverageKeyStrokeDownTimes);
                ArrayUtils.ConcatArrays(manhattanData, userTraining.AverageKeyStrokeUpTimes);
            }
            double[] manhattanCurrentData =  {
                                                    passwordEntry.AverageKeyHoldTime, 
                                                    passwordEntry.AverageTimeBetweenKeyUp,
                                                  passwordEntry.AverageTimeBetweenKeyDown,
                                                  passwordEntry.AverageTotalFirstDownLastDownTime,
                                                  passwordEntry.AverageTotalFirstUpLastUpTime };
            if (passwordEntry.AverageKeyStrokeDownTimes != null && passwordEntry.AverageKeyStrokeUpTimes != null)
            {
                ArrayUtils.ConcatArrays(manhattanData, passwordEntry.AverageKeyStrokeDownTimes);
                ArrayUtils.ConcatArrays(manhattanData, passwordEntry.AverageKeyStrokeUpTimes);
            }
            return manhattanData.Manhattan(manhattanCurrentData);
        }

        public double CalculateEuclideanDistance(UserTraining userTraining, UserTraining passwordEntry)
        {
            double[] euclideanData = {    
                                                    userTraining.AverageKeyHoldTime,
                                                    userTraining.AverageTimeBetweenKeyUp,
                                                    userTraining.AverageTimeBetweenKeyDown,
                                                    userTraining.AverageTotalFirstDownLastDownTime,
                                                    userTraining.AverageTotalFirstUpLastUpTime
                                     };
            if (userTraining.AverageKeyStrokeDownTimes != null && userTraining.AverageKeyStrokeUpTimes != null)
            {
                ArrayUtils.ConcatArrays(euclideanData, userTraining.AverageKeyStrokeDownTimes);
                ArrayUtils.ConcatArrays(euclideanData, userTraining.AverageKeyStrokeUpTimes);
            }
            double[] euclideanCurrentData =  {
                                                    passwordEntry.AverageKeyHoldTime, 
                                                    passwordEntry.AverageTimeBetweenKeyUp,
                                                  passwordEntry.AverageTimeBetweenKeyDown,
                                                  passwordEntry.AverageTotalFirstDownLastDownTime,
                                                  passwordEntry.AverageTotalFirstUpLastUpTime };
            if (passwordEntry.AverageKeyStrokeDownTimes != null && passwordEntry.AverageKeyStrokeUpTimes != null)
            {
                ArrayUtils.ConcatArrays(euclideanData, passwordEntry.AverageKeyStrokeDownTimes);
                ArrayUtils.ConcatArrays(euclideanData, passwordEntry.AverageKeyStrokeUpTimes);
            }
            return euclideanData.Euclidean(euclideanCurrentData);
        }

        public double CalculateDistance(UserTraining userTraining, TrainingEntry passwordEntry)
        {
            double[] manhattanData = {    
                                                    userTraining.AverageKeyHoldTime,
                                                    userTraining.AverageTimeBetweenKeyUp,
                                                    userTraining.AverageTimeBetweenKeyDown,
                                                    userTraining.AverageTotalFirstDownLastDownTime,
                                                    userTraining.AverageTotalFirstUpLastUpTime,
                                     };
            if (userTraining.AverageKeyStrokeDownTimes != null && userTraining.AverageKeyStrokeUpTimes != null)
            {
                ArrayUtils.ConcatArrays(manhattanData, userTraining.AverageKeyStrokeDownTimes);
                ArrayUtils.ConcatArrays(manhattanData, userTraining.AverageKeyStrokeUpTimes);
            }
            double[] manhattanCurrentData =  {
                                                    passwordEntry.AverageHoldTime, 
                                                    passwordEntry.AverageTimeBetweenKeyUp,
                                                  passwordEntry.AverageTimeBetweenKeyDown,
                                                  passwordEntry.TotalFirstDownLastDownTime,
                                                  passwordEntry.TotalFirstUpLastUpTime, };
            if (passwordEntry.KeyStrokeDownTimes != null && passwordEntry.KeyStrokeUpTimes != null)
            {
                ArrayUtils.ConcatArrays(manhattanData, passwordEntry.KeyStrokeDownTimes);
                ArrayUtils.ConcatArrays(manhattanData, passwordEntry.KeyStrokeUpTimes);
            }
            return manhattanData.Manhattan(manhattanCurrentData);
        }

        public double CalculateEuclideanDistance(UserTraining userTraining, TrainingEntry passwordEntry)
        {
            double[] euclideanData = {    
                                                    userTraining.AverageKeyHoldTime,
                                                    userTraining.AverageTimeBetweenKeyUp,
                                                    userTraining.AverageTimeBetweenKeyDown,
                                                    userTraining.AverageTotalFirstDownLastDownTime,
                                                    userTraining.AverageTotalFirstUpLastUpTime,
                                     };
            if (userTraining.AverageKeyStrokeDownTimes != null && userTraining.AverageKeyStrokeUpTimes != null)
            {
                ArrayUtils.ConcatArrays(euclideanData, userTraining.AverageKeyStrokeDownTimes);
                ArrayUtils.ConcatArrays(euclideanData, userTraining.AverageKeyStrokeUpTimes);
            }
            double[] euclideanCurrentData =  {
                                                    passwordEntry.AverageHoldTime, 
                                                    passwordEntry.AverageTimeBetweenKeyUp,
                                                  passwordEntry.AverageTimeBetweenKeyDown,
                                                  passwordEntry.TotalFirstDownLastDownTime,
                                                  passwordEntry.TotalFirstUpLastUpTime, };
            if (passwordEntry.KeyStrokeDownTimes != null && passwordEntry.KeyStrokeUpTimes != null)
            {
                ArrayUtils.ConcatArrays(euclideanData, passwordEntry.KeyStrokeDownTimes);
                ArrayUtils.ConcatArrays(euclideanData, passwordEntry.KeyStrokeUpTimes);
            }
            return euclideanData.Euclidean(euclideanCurrentData);
        }
    }
}
