// *******************************************************
// * <copyright file="BubbleSorter.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BubbleSort
{
    #region Usings

    using System;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// </summary>
    public class BubbleSorter
    {
        /// <summary>
        /// </summary>
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// </summary>
        private readonly int[] arrayToSort;

        /// <summary>
        /// </summary>
        private readonly int arrayLength;

        /// <summary>
        /// </summary>
        /// <param name="toSort">
        /// </param>
        public BubbleSorter(int[] toSort)
        {
            this.arrayToSort = toSort;
            this.arrayLength = this.arrayToSort.Length;
        }

        /// <summary>
        /// </summary>
        public double ElapsedTime { get; set; }

        /// <summary>
        /// </summary>
        public int[] SortedArray { get; set; }

        /// <summary>
        /// </summary>
        public void Sort()
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();

            for (int i = 0; i < this.arrayLength - 1; ++i)
            {
                for (int j = 0; j < this.arrayLength - i - 1; ++j)
                {
                    if (this.arrayToSort[j] > this.arrayToSort[j + 1])
                    {
                        int tmp = this.arrayToSort[j];
                        this.arrayToSort[j] = this.arrayToSort[j + 1];
                        this.arrayToSort[j + 1] = tmp;
                    }
                }
            }

            this.stopwatch.Stop();
            this.ElapsedTime = this.stopwatch.ElapsedMilliseconds;

            this.SortedArray = this.arrayToSort;

            Console.WriteLine(
                "Sort completed with: " + this.stopwatch.ElapsedMilliseconds + "ms - " + this.stopwatch.ElapsedTicks + " ticks");
        }
    }
}