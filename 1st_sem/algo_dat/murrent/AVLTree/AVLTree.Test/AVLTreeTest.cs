// *******************************************************
// * <copyright file="AVLTreeTest.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace AVLTree.Test
{
    #region Usings

    using System;
    using System.Configuration;
    using System.Diagnostics;

    using NUnit.Framework;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    public class AVLTreeTest
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly CustomAVLTree<int, int> customAVLTree = new CustomAVLTree<int, int>();

        /// <summary>
        /// </summary>
        private CArray numbers;

        /// <summary>
        /// </summary>
        private readonly Stopwatch stopwatch = new Stopwatch();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.numbers = new CArray(100, 100000);
        }

        /// <summary>
        /// </summary>
        [TestCase]
        public void TestInsert()
        {
            Console.WriteLine("Building tree with unsorted array");

            this.stopwatch.Start();

            foreach (int number in this.numbers.NumberArray)
            {
                this.customAVLTree.Insert(number, number);
            }

            this.stopwatch.Stop();

            Console.WriteLine("Time needed for building and inserting: " + this.stopwatch.ElapsedMilliseconds + "ms - Ticks: " + this.stopwatch.ElapsedTicks);
            Console.WriteLine();
            this.customAVLTree.DisplayTree(this.customAVLTree.Root, string.Empty);
            this.stopwatch.Reset();

            foreach (int number in this.numbers.NumberArray)
            {
                int foundNumber = 0;

                Assert.IsTrue(this.customAVLTree.Search(number, out foundNumber));
            }
        }

        /// <summary>
        /// </summary>
        [TestCase]
        public void TestSearchUnsorted()
        {
            int compareCount = 0;

            int[] numbersToSearch = new int[100];
            Random rnd = new Random();

            for (int i = 0; i < numbersToSearch.Length; i++)
            {
                numbersToSearch[i] = rnd.Next(100000);
            }

            this.stopwatch.Start();

            foreach (int number in numbersToSearch)
            {
                int foundNumber;

                if (this.customAVLTree.Search(number, out foundNumber))
                {
                    Console.WriteLine("Found: " + foundNumber);
                }
                else
                {
                    compareCount++;
                }

            }

            this.stopwatch.Stop();

            Console.WriteLine("Time needed for building and inserting: " + this.stopwatch.ElapsedMilliseconds + "ms - Ticks: " + this.stopwatch.ElapsedTicks);

            this.stopwatch.Reset();

            Console.WriteLine("Compared: " + compareCount + "time(s)");
        }

        /// <summary>
        /// </summary>
        [TestCase]
        public void TestSearchSorted()
        {
            int compareCount = 0;

            int[] numbersToSearch = new int[100];
            Random rnd = new Random();

            for (int i = 0; i < numbersToSearch.Length; i++)
            {
                numbersToSearch[i] = rnd.Next(100000);
            }

            Array.Sort(numbersToSearch);

            this.stopwatch.Start();

            foreach (int number in numbersToSearch)
            {
                int foundNumber;

                if (this.customAVLTree.Search(number, out foundNumber))
                {
                    Console.WriteLine("Found: " + foundNumber);
                }
                else
                {
                    compareCount++;
                }

            }

            this.stopwatch.Stop();

            Console.WriteLine("Time needed for building and inserting: " + this.stopwatch.ElapsedMilliseconds + "ms - Ticks: " + this.stopwatch.ElapsedTicks);

            this.stopwatch.Reset();

            Console.WriteLine("Compared: " + compareCount + "time(s)");
        }
    }
        #endregion
}