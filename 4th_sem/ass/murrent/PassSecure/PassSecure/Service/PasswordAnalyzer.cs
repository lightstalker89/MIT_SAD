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
    using System.Diagnostics;
    using System.Linq;

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
                //byte[] trainingKeyUpData =
                //    ArrayUtils.ConcatArrays(
                //        userTraining.AverageTimeBetweenKeyUp.ToByteArray(),
                //        userTraining.AverageTimeBetweenKeyDown.ToByteArray(),
                //        userTraining.AverageTotalFirstDownLastDownTime.ToByteArray(),
                //       userTraining.AverageTotalFirstUpLastUpTime.ToByteArray(),
                //       userTraining.AverageKeyHoldTime.ToByteArray());
                //byte[] currentKeyUpData =
                //    ArrayUtils.ConcatArrays(
                //        passwordEntry.AverageTimeBetweenKeyUp.ToByteArray(),
                //        passwordEntry.AverageTimeBetweenKeyDown.ToByteArray(),
                //         passwordEntry.AverageTotalFirstDownLastDownTime.ToByteArray(),
                //        passwordEntry.AverageTotalFirstUpLastUpTime.ToByteArray(),
                //        passwordEntry.AverageKeyHoldTime.ToByteArray());
                //double differenceKullback = CheckCriteria(currentKeyUpData, trainingKeyUpData);
                //double[] trainingDoubles = new[]
                //                               {
                //                                   userTraining.AverageTimeBetweenKeyUp,
                //                                   userTraining.AverageTimeBetweenKeyDown,
                //                                   userTraining.AverageTotalFirstDownLastDownTime,
                //                                   userTraining.AverageTotalFirstUpLastUpTime,
                //                                   userTraining.AverageKeyHoldTime
                //                               };
                //double[] currentDoubles = new[]
                //                              {
                //                                  passwordEntry.AverageTimeBetweenKeyUp,
                //                                  passwordEntry.AverageTimeBetweenKeyDown,
                //                                  passwordEntry.AverageTotalFirstDownLastDownTime,
                //                                  passwordEntry.AverageTotalFirstUpLastUpTime,
                //                                  passwordEntry.AverageKeyHoldTime
                //                              };

                //double distance = Accord.Math.Distance.Manhattan(trainingDoubles, currentDoubles);
                //double euclidDistance = Accord.Math.Distance.Euclidean(trainingDoubles, currentDoubles);
                //Debug.WriteLine("MANHATTAN DISTANCE: " + distance);
                //Debug.WriteLine("EUCLIDEAN DISTANCE: " + euclidDistance);
                //double differenceKeyUpDown = CheckCriteria(currentKeyUpData, trainingKeyUpData);
                //byte[] trainingFirstDownLastDownData =
                //   ArrayUtils.ConcatArrays(
                //       userTraining.AverageTotalFirstDownLastDownTime.ToByteArray(),
                //       userTraining.AverageTotalFirstUpLastUpTime.ToByteArray());
                //byte[] currentFirstUpLastUpDate =
                //    ArrayUtils.ConcatArrays(
                //        passwordEntry.AverageTotalFirstDownLastDownTime.ToByteArray(),
                //        passwordEntry.AverageTotalFirstUpLastUpTime.ToByteArray());
                //double differenceFirstKeyUpDown = CheckCriteria(currentFirstUpLastUpDate, trainingFirstDownLastDownData);
                //double difference = (differenceKeyUpDown + differenceFirstKeyUpDown) / 2;
                // double difference = (differenceFirstKeyUpDown + differenceKeyUpDown);
                //Debug.WriteLine("---------");
                //Debug.WriteLine("KullBack Leibler: " + difference);
                //Debug.WriteLine(difference / 2);
                //Debug.WriteLine(differenceKeyUpDown);
                // Debug.WriteLine(differenceFirstKeyUpDown);
                //Debug.WriteLine((differenceFirstKeyUpDown + differenceKeyUpDown) / 2);
                //Debug.WriteLine("****");
                //Debug.WriteLine(difference);
                //Debug.WriteLine(difference / 2);
                //double[] manhattanData = new[] {    
                //                                    userTraining.AverageKeyHoldTime,
                //                                    userTraining.AverageTimeBetweenKeyUp,
                //                                    userTraining.AverageTimeBetweenKeyDown,
                //                                    userTraining.AverageTotalFirstDownLastDownTime,
                //                                    userTraining.AverageTotalFirstUpLastUpTime,
                //                                    };
                ////manhattanData = ArrayUtils.ConcatArrays(manhattanData, userTraining.AverageKeyStrokeDownTimes.ToArray());
                ////manhattanData = ArrayUtils.ConcatArrays(manhattanData, userTraining.AverageKeyStrokeUpTimes.ToArray());
                //double[] manhattanCurrentData = new[] {
                //                                    passwordEntry.AverageKeyHoldTime, 
                //                                    passwordEntry.AverageTimeBetweenKeyUp,
                //                                  passwordEntry.AverageTimeBetweenKeyDown,
                //                                  passwordEntry.AverageTotalFirstDownLastDownTime,
                //                                  passwordEntry.AverageTotalFirstUpLastUpTime, };
                //manhattanCurrentData = ArrayUtils.ConcatArrays(
                //    manhattanCurrentData,
                //    passwordEntry.AverageKeyStrokeDownTimes.ToArray());
                //manhattanCurrentData = ArrayUtils.ConcatArrays(
                //    manhattanCurrentData,
                //    passwordEntry.AverageKeyStrokeUpTimes.ToArray());
                double difference = CalculateDistance(userTraining, passwordEntry); /// manhattanData.Length;
                //double diff = new double[] { userTraining.AverageDistance }.Manhattan(new[] { passwordEntry.AverageDistance });
                double diff = Math.Abs(difference - userTraining.AverageDistance);
                if (difference <= userTraining.AverageDistance)
                {
                    status = Enums.PasswordStatus.Accepted;
                }
                else if (difference > userTraining.AverageDistance && difference <= (userTraining.AverageDistance + userTraining.AverageDistance / 5))
                {
                    status = Enums.PasswordStatus.PartialAccepted;
                }
                Debug.WriteLine(status);

                //Debug.WriteLine("Kullback: " + differenceKullback);
                Debug.WriteLine("Manhattan distance: " + difference);
               // Debug.WriteLine("Calc Manhattan distance: " + diff);
                Debug.WriteLine("Average manhattan distance: " + userTraining.AverageDistance);
                Debug.WriteLine("Distance: " + diff);
                //double distanceKeyUp = Accord.Math.Distance.BitwiseHamming(trainingKeyUpData, currentKeyUpData);
                //double distanceKeyDown = Accord.Math.Distance.BitwiseHamming(userTraining.AverageTotalFirstDownLastDownTime.ToByteArray(), passwordEntry.TotalFirstDownLastDownTime.ToByteArray());
                //double distanceKeyUp = Accord.Math.Distance.BitwiseHamming(userTraining.AverageTotalFirstUpLastUpTime.ToByteArray(), passwordEntry.TotalFirstUpLastUpTime.ToByteArray());
                //double distanceKeyDownManhattan =
                //    Accord.Math.Distance.Manhattan(new[] { userTraining.AverageTotalFirstDownLastDownTime }, new[] { passwordEntry.TotalFirstDownLastDownTime });

                //double distanceKeyUpManhattan =
                //  Accord.Math.Distance.Manhattan(new[] { userTraining.AverageTotalFirstUpLastUpTime, userTraining.AverageTotalFirstDownLastDownTime }, new[] { passwordEntry.TotalFirstUpLastUpTime, passwordEntry.TotalFirstDownLastDownTime });
                //Debug.WriteLine("KeyDown distance: " + distanceKeyDownManhattan);
                //Debug.WriteLine("Manhattan Distance: " + distanceKeyUpManhattan);
                //Debug.WriteLine("Time distance KeyDown: " + (userTraining.AverageTotalFirstDownLastDownTime - passwordEntry.TotalFirstDownLastDownTime));
                //Debug.WriteLine("Time distance KeyUp: " + (userTraining.AverageTotalFirstUpLastUpTime - passwordEntry.TotalFirstUpLastUpTime));
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
                                                    userTraining.AverageTotalFirstUpLastUpTime,
                                     };
            double[] manhattanCurrentData =  {
                                                    passwordEntry.AverageKeyHoldTime, 
                                                    passwordEntry.AverageTimeBetweenKeyUp,
                                                  passwordEntry.AverageTimeBetweenKeyDown,
                                                  passwordEntry.AverageTotalFirstDownLastDownTime,
                                                  passwordEntry.AverageTotalFirstUpLastUpTime, };
            return manhattanData.Manhattan(manhattanCurrentData);
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
            double[] manhattanCurrentData =  {
                                                    passwordEntry.AverageHoldTime, 
                                                    passwordEntry.AverageTimeBetweenKeyUp,
                                                  passwordEntry.AverageTimeBetweenKeyDown,
                                                  passwordEntry.TotalFirstDownLastDownTime,
                                                  passwordEntry.TotalFirstUpLastUpTime, };
            return manhattanData.Manhattan(manhattanCurrentData);
        }

        ///// <summary>
        ///// </summary>
        ///// <param name="entryToMatch"></param>
        ///// <param name="averageTrainingCriteria"></param>
        ///// <returns></returns>
        //private double CheckCriteria(byte[] entryToMatch, byte[] averageTrainingCriteria)
        //{
        //    double spec1 = 0;
        //    double spec2 = 0;
        //    double spec1Sum = AccumulateArray(averageTrainingCriteria);
        //    double spec2Sum = AccumulateArray(entryToMatch);
        //    double kullbackD1 = 0;
        //    double kullbackD2 = 0;

        //    for (int i = 0; i < entryToMatch.Length; i++)
        //    {
        //        if (entryToMatch[i] != 0)
        //        {
        //            spec1 = averageTrainingCriteria[i] / spec1Sum;
        //            spec2 = entryToMatch[i] / spec2Sum;
        //            kullbackD1 += spec1 * Math.Log((spec1 / spec2), 2);
        //            kullbackD2 += spec2 * Math.Log((spec2 / spec1), 2);
        //        }
        //    }

        //    return (kullbackD1 + kullbackD2) / 2;
        //}

        ///// <summary>
        ///// </summary>
        ///// <param name="averageI"></param>
        ///// <param name="averageII"></param>
        ///// <param name="currentI"></param>
        ///// <param name="currentII"></param>
        ///// <returns></returns>
        //private double GetDifference(double averageI, double averageII, double currentI, double currentII)
        //{
        //    byte[] averageData =
        //            ArrayUtils.ConcatArrays(
        //                averageI.ToByteArray(),
        //               averageII.ToByteArray());
        //    byte[] currentData =
        //        ArrayUtils.ConcatArrays(
        //           currentI.ToByteArray(),
        //            currentII.ToByteArray());
        //    return CheckCriteria(currentData, averageData);

        //}

        ///// <summary>
        ///// </summary>
        ///// <param name="array">
        ///// </param>
        ///// <returns>
        ///// </returns>
        //private double AccumulateArray(byte[] array)
        //{
        //    return array.Aggregate<byte, double>(0, (current, t) => current + t);
        //}
    }
}
