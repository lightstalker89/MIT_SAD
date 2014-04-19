// *******************************************************
// * <copyright file="SelectionSortTest.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace SelectionSort.Test
{
    #region Usings

    using System;

    using NUnit.Framework;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    public class SelectionSortTest
    {
        /// <summary>
        /// </summary>
        private static readonly int[] selectionSortValueCount = { 100, 1000, 10000, 100000 };

        /// <summary>
        /// </summary>
        private SelectionSorter insertionSorter;

        /// <summary>
        /// </summary>
        private static CArray numbers;

        /// <summary>
        /// </summary>
        [TestCase]
        public void BubbleSorterTest()
        {
            foreach (int valueCount in selectionSortValueCount)
            {
                numbers = new CArray(valueCount, 589);

                for (int x = 0; x < 10; ++x)
                {
                    this.insertionSorter = new SelectionSorter(numbers.NumberArray);
                    this.insertionSorter.Sort();
                }

                Array.Sort(numbers.NumberArray);

                Assert.AreEqual(numbers.NumberArray, this.insertionSorter.SortedArray);
            }
        }
    }
}