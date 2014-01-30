// *******************************************************
// * <copyright file="BinaryTreeTest.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BinaryTree.Test
{
    #region Usings

    using System;
    using System.Diagnostics;

    using NUnit.Framework;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    public class BinaryTreeTest
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly CustomBinaryTree customBinaryTree = new CustomBinaryTree();

        /// <summary>
        /// </summary>
        private CArray numbers;

        /// <summary>
        /// </summary>
        private readonly Stopwatch stopwatch = new Stopwatch();

        #endregion

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
                this.customBinaryTree.Insert(number);
            }

            this.stopwatch.Stop();

            Console.WriteLine("Time needed for building and inserting: " + this.stopwatch.ElapsedMilliseconds + "ms - Ticks: " + this.stopwatch.ElapsedTicks);

            this.stopwatch.Reset();
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
                if (this.customBinaryTree.Search(customBinaryTree.Root, number))
                {
                    Console.WriteLine("Found: " + number);
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
                if (this.customBinaryTree.Search(customBinaryTree.Root, number))
                {
                    Console.WriteLine("Found: " + number);
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
}
