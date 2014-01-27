// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using NUnit.Framework;
using SortHelper;

namespace InsertionSort.Test
{
    public class InsertionSortTest
    {
        private static readonly int[] insertionSortValueCount = { 100, 1000, 10000, 100000 };
        private InsertionSorter insertionSorter;
        private static CArray numbers;

        [TestCase]
        public void BubbleSorterTest()
        {
            foreach (int valueCount in insertionSortValueCount)
            {
                numbers = new CArray(valueCount, 589);

                for (int x = 0; x < 10; ++x)
                {
                    insertionSorter = new InsertionSorter(numbers.NumberArray);
                    insertionSorter.Sort();
                }

                Array.Sort(numbers.NumberArray);

                Assert.AreEqual(numbers.NumberArray, insertionSorter.SortedArray);
            }
        }
    }
}
