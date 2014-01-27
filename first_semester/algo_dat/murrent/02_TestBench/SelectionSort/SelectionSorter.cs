// *******************************************************
// * <copyright file="SelectionSorter.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace SelectionSort
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// </summary>
    public class SelectionSorter
    {
        /// <summary>
        /// </summary>
        private Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// </summary>
        private int[] arrayToSort;

        /// <summary>
        /// </summary>
        private readonly int arrayLength;

        /// <summary>
        /// </summary>
        /// <param name="toSort">
        /// </param>
        public SelectionSorter(int[] toSort)
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

            for (int i = 0; i < arrayLength - 1; ++i)
            {
                int minPos = this.FindMinPosition(i);
                this.SwapPositions(minPos, i);
            }

            stopwatch.Stop();
            this.ElapsedTime = stopwatch.ElapsedMilliseconds;

            this.SortedArray = arrayToSort;

            Console.WriteLine(
                "Sort completed with: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
        }

        /// <summary>
        /// </summary>
        /// <param name="positionFrom">
        /// </param>
        /// <returns>
        /// </returns>
        private int FindMinPosition(int positionFrom)
        {
            int minPos = positionFrom;

            for (int i = positionFrom + 1; i < arrayLength; ++i)
            {
                if (arrayToSort[i] < arrayToSort[minPos])
                {
                    minPos = i;
                }
            }

            return minPos;
        }

        /// <summary>
        /// </summary>
        /// <param name="positionA">
        /// </param>
        /// <param name="positionB">
        /// </param>
        private void SwapPositions(int positionA, int positionB)
        {
            int tempPositionA = this.arrayToSort[positionA];
            this.arrayToSort[positionA] = positionB;
            this.arrayToSort[positionB] = tempPositionA;
        }
    }
}