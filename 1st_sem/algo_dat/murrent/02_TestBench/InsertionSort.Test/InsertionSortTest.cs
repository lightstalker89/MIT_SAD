// *******************************************************
// * <copyright file="InsertionSortTest.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace InsertionSort.Test
{
    #region Usings

    using System;

    using NUnit.Framework;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    public class InsertionSortTest
    {
        /// <summary>
        /// </summary>
        private static readonly int[] insertionSortValueCount = { 100, 1000, 10000, 100000 };

        /// <summary>
        /// </summary>
        private InsertionSorter insertionSorter;

        /// <summary>
        /// </summary>
        private static CArray numbers;

        /// <summary>
        /// </summary>
        [TestCase]
        public void BubbleSorterTest()
        {
            foreach (int valueCount in insertionSortValueCount)
            {
                numbers = new CArray(valueCount, 589);

                for (int x = 0; x < 10; ++x)
                {
                    this.insertionSorter = new InsertionSorter(numbers.NumberArray);
                    this.insertionSorter.Sort();
                }

                Array.Sort(numbers.NumberArray);

                Assert.AreEqual(numbers.NumberArray, this.insertionSorter.SortedArray);
            }
        }
    }
}