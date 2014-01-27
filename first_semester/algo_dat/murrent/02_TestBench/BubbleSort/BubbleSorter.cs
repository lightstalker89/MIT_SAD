// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using System.Diagnostics;

namespace BubbleSort
{
    public class BubbleSorter
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly int[] arrayToSort;
        private readonly int arrayLength;

        public BubbleSorter(int[] toSort)
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

            for (int i = 0; i < arrayLength - 1; ++i)
            {
                for (int j = 0; j < arrayLength - i - 1; ++j)
                {
                    if (arrayToSort[j] > arrayToSort[j + 1])
                    {
                        int tmp = arrayToSort[j];
                        arrayToSort[j] = arrayToSort[j + 1];
                        arrayToSort[j + 1] = tmp;
                    }
                }
            }

            stopwatch.Stop();
            this.ElapsedTime = stopwatch.ElapsedMilliseconds;

            this.SortedArray = arrayToSort;

            Console.WriteLine("Sort completed with: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
        }
    }
}
