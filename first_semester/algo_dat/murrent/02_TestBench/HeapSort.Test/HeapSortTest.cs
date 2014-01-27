// *******************************************************
// * <copyright file="HeapSortTest.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace HeapSort.Test
{
    using System;

    using NUnit.Framework;

    using SortHelper;

    /// <summary>
    /// </summary>
    public class HeapSortTest
    {
        /// <summary>
        /// </summary>
        private static readonly int[] heapSortValueCount = { 100, 1000, 10000, 100000 };

        /// <summary>
        /// </summary>
        private HeapSorter heapSorter;

        /// <summary>
        /// </summary>
        private static CArray numbers;

        /// <summary>
        /// </summary>
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