// *******************************************************
// * <copyright file="InsertionSorter.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace InsertionSort
{
    #region Usings

    using System;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// </summary>
    public class InsertionSorter
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
        public InsertionSorter(int[] toSort)
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

            for (int j = 0; j < this.arrayLength; j++)
            {
                int key = this.arrayToSort[j];
                int i = j - 1;

                while (i >= 0 && this.arrayToSort[i] > key)
                {
                    this.arrayToSort[i + 1] = this.arrayToSort[i];

                    i = i - 1;
                }

                this.arrayToSort[i + 1] = key;
            }

            this.stopwatch.Stop();
            this.ElapsedTime = this.stopwatch.ElapsedMilliseconds;

            this.SortedArray = this.arrayToSort;

            Console.WriteLine(
                "Sort completed with: " + this.stopwatch.ElapsedMilliseconds + "ms - " + this.stopwatch.ElapsedTicks + " ticks");
        }
    }
}