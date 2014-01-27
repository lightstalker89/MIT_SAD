// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace _03_FibonacciSearch
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// </summary>
        private static List<int> fibonacciNumbers = new List<int>();

        /// <summary>
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// </summary>
        private static CArray array;

        /// <summary>
        /// </summary>
        private static readonly int[] RandomNumbers = new int[10];

        /// <summary>
        /// </summary>
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                RandomNumbers[i] = Random.Next(100);
            }

            array = new CArray(100, 100000);

            Stopwatch.Start();

            foreach (int t in RandomNumbers)
            {
                int result = Search(array.ArraySorted, t);

                if (result == -1)
                {
                    Console.WriteLine("Search finished");
                }
                else
                {
                    Console.WriteLine("Found: " + t);
                }
            }

            Stopwatch.Stop();

            Console.WriteLine("Elapsed: " + Stopwatch.ElapsedMilliseconds + "ms - " + Stopwatch.ElapsedTicks + " ticks");
            Console.WriteLine("Compare count: " + array.CompareCount);
            Console.ReadKey();

            Stopwatch.Stop();
        }

        /// <summary>
        /// </summary>
        /// <param name="arrayToSort">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public static int Search(int[] arrayToSort, int value)
        {
            int lowerBound = 0;
            int higherBound = arrayToSort.Length;

            while (lowerBound <= higherBound)
            {
                int mid = GetFibonacci((higherBound + lowerBound));

                if (arrayToSort[mid] < value)
                {
                    lowerBound = mid + 1;
                }
                else if (arrayToSort[mid] > value)
                {
                    higherBound = mid - 1;
                }
                else
                {
                    return mid;
                }
            }

            array.CompareCount++;

            return -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetFibonacci(int value)
        {
            int a = 0;
            int b = 1;
            int lastNumber = 0;

            for (int i = 0; i <= value; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;

                if (b > value)
                {
                    return lastNumber;
                }

                lastNumber = b;
            }

            return -1;
        }
    }
}