// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using NUnit.Framework;
using SortHelper;

namespace BubbleSort.Test
{
    public class BubbleSortTest
    {
        private static readonly int[] bubbleSortValueCount = { 100, 1000, 10000, 100000 };
        private BubbleSorter bubbleSorter;
        private static CArray numbers;

        [TestCase]
        public void BubbleSorterTest()
        {
            foreach (int valueCount in bubbleSortValueCount)
            {
                numbers = new CArray(valueCount, 589);

                for (int x = 0; x < 10; ++x)
                {
                    bubbleSorter = new BubbleSorter(numbers.NumberArray);
                    bubbleSorter.Sort();
                }

                Array.Sort(numbers.NumberArray);

                Assert.AreEqual(numbers.NumberArray, bubbleSorter.SortedArray);
            }
        }
    }
}
