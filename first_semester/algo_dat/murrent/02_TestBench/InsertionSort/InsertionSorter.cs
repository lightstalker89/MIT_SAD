// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using System.Diagnostics;

namespace InsertionSort
{
    public class InsertionSorter
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly int[] arrayToSort;
        private readonly int arrayLength;

        public InsertionSorter(int[] toSort)
        {
            this.arrayToSort = toSort;
            this.arrayLength = this.arrayToSort.Length;
        }

        public double ElapsedTime { get; set; }

        public int[] SortedArray { get; set; }

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

            Console.WriteLine("Sort completed with: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
        }
    }
}
