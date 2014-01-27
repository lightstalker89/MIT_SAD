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
    using System;
    using System.Diagnostics;

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
            stopwatch.Reset();
            stopwatch.Start();

            for (int j = 0; j < arrayLength; j++)
            {
                int key = arrayToSort[j];
                int i = j - 1;

                while (i >= 0 && arrayToSort[i] > key)
                {
                    arrayToSort[i + 1] = arrayToSort[i];

                    i = i - 1;
                }

                arrayToSort[i + 1] = key;
            }

            stopwatch.Stop();
            this.ElapsedTime = stopwatch.ElapsedMilliseconds;

            this.SortedArray = arrayToSort;

            Console.WriteLine(
                "Sort completed with: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
        }
    }
}