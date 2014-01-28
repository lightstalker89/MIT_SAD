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
    using System.Runtime.CompilerServices;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// </summary>
        private static List<int> FibonacciNumbers = new List<int> { 0, 1 };

        /// <summary>
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// </summary>
        private static CArray array;

        /// <summary>
        /// </summary>
        private static readonly int[] RandomNumbers = new int[20];

        /// <summary>
        /// </summary>
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            for (int i = 0; i < RandomNumbers.Length; i++)
            {
                RandomNumbers[i] = Random.Next(100000);
            }

            array = new CArray(1000, 100000);

            Stopwatch.Start();

            foreach (int t in RandomNumbers)
            {
                int result = Search(array.ArraySortedDescending, t);

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
            int index = -1;
            int lowerBound = 0;
            int higherBound = arrayToSort.Length - 1;
            bool found = false;

            while (!found && (higherBound - lowerBound) > 0)
            {
                int mid = GetFibonacci(higherBound - lowerBound) + lowerBound;

                if (arrayToSort[mid] < value)
                {
                    higherBound = mid;
                }
                else if (arrayToSort[mid] > value)
                {
                    lowerBound = (higherBound >= mid + 1) ? mid + 1 : mid;
                }
                else
                {
                    found = true;
                    index = mid;
                }

                array.CompareCount++;
            }

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetFibonacci(int value)
        {
            int number = 0;

            if (value == 0)
            {
                return value;
            }

            while (value > FibonacciNumbers[number])
            {
                number++;

                if (FibonacciNumbers.Count <= number)
                {
                    int fibonacciNumber = FibonacciNumbers[number - 1] + FibonacciNumbers[number - 2];
                    FibonacciNumbers.Add(fibonacciNumber);
                }
            }

            return FibonacciNumbers[number - 1];
        }

        public static int FibonacciRecursive(int x)
        {

            if (x == 0 || x == 1)

                return x;

            return FibonacciRecursive(x - 1) +

                   FibonacciRecursive(x - 2);

        }
    }
}