// *******************************************************
// * <copyright file="BubbleSortTest.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BubbleSort.Test
{
    using System;

    using NUnit.Framework;

    using SortHelper;

    /// <summary>
    /// </summary>
    public class BubbleSortTest
    {
        /// <summary>
        /// </summary>
        private static readonly int[] bubbleSortValueCount = { 100, 1000, 10000, 100000 };

        /// <summary>
        /// </summary>
        private BubbleSorter bubbleSorter;

        /// <summary>
        /// </summary>
        private static CArray numbers;

        /// <summary>
        /// </summary>
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