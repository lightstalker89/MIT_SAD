// *******************************************************
// * <copyright file="HeapSorter.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace HeapSort
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// </summary>
    public class HeapSorter
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
        public HeapSorter(int[] toSort)
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

            this.GenerateMaxHeap(this.arrayToSort);

            for (int i = this.arrayLength - 1; i >= 0; i += -1)
            {
                this.Swap(this.arrayToSort, i, 0);
                this.Sink(this.arrayToSort, 0, i);
            }

            stopwatch.Stop();
            this.ElapsedTime = stopwatch.ElapsedMilliseconds;

            this.SortedArray = arrayToSort;

            Console.WriteLine(
                "Sort completed with: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
        }

        /// <summary>
        /// </summary>
        /// <param name="a">
        /// </param>
        private void GenerateMaxHeap(int[] a)
        {
            for (int i = a.Length / 2 - 1; i >= 1; i += -1)
            {
                this.Sink(a, i, a.Length);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="a">
        /// </param>
        /// <param name="i">
        /// </param>
        /// <param name="n">
        /// </param>
        private void Sink(int[] a, int i, int n)
        {
            while (i <= (n / 2 - 1))
            {
                int kindIndex = (i + 1) * 2 - 1;

                if (kindIndex + 1 <= n - 1)
                {
                    if (a[kindIndex] < a[kindIndex + 1])
                    {
                        kindIndex += 1;
                    }
                }

                if (a[i] < a[kindIndex])
                {
                    this.Swap(a, i, kindIndex);
                    i = kindIndex;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="a">
        /// </param>
        /// <param name="i">
        /// </param>
        /// <param name="kindIndex">
        /// </param>
        private void Swap(int[] a, int i, int kindIndex)
        {
            int z = a[i];
            a[i] = a[kindIndex];
            a[kindIndex] = z;
        }
    }
}