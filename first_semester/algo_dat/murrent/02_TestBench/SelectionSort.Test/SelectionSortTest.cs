// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using NUnit.Framework;
using SortHelper;

namespace SelectionSort.Test
{
    public class SelectionSortTest
    {
        private static readonly int[] selectionSortValueCount = { 100, 1000, 10000, 100000 };
        private SelectionSorter insertionSorter;
        private static CArray numbers;

        [TestCase]
        public void BubbleSorterTest()
        {
            foreach (int valueCount in selectionSortValueCount)
            {
                numbers = new CArray(valueCount, 589);

                for (int x = 0; x < 10; ++x)
                {
                    insertionSorter = new SelectionSorter(numbers.NumberArray);
                    insertionSorter.Sort();
                }

                Array.Sort(numbers.NumberArray);

                Assert.AreEqual(numbers.NumberArray, insertionSorter.SortedArray);
            }
        }
    }
}
