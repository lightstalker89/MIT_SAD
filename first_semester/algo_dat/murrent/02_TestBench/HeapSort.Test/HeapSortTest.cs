// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using NUnit.Framework;
using SortHelper;

namespace HeapSort.Test
{
    public class HeapSortTest
    {
        private static readonly int[] heapSortValueCount = { 100, 1000, 10000, 100000 };
        private HeapSorter heapSorter;
        private static CArray numbers;

        [TestCase]
        public void BubbleSorterTest()
        {
            foreach (int valueCount in heapSortValueCount)
            {
                numbers = new CArray(valueCount, 589);

                for (int x = 0; x < 10; ++x)
                {
                    heapSorter = new HeapSorter(numbers.NumberArray);
                    heapSorter.Sort();
                }

                Array.Sort(numbers.NumberArray);

                Assert.AreEqual(numbers.NumberArray, heapSorter.SortedArray);
            }
        }
    }
}
