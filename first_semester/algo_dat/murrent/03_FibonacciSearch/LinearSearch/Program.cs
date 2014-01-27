// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace LinearSearch
{
    #region Usings

    using System;
    using System.Diagnostics;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    internal class Program
    {
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

            array = new CArray(1000, 100);

            Stopwatch.Start();

            foreach (int t in RandomNumbers)
            {
                int result = Search(t);

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
        }

        /// <summary>
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        private static int Search(int key)
        {
            int i;

            for (i = 0; i < array.NumberArray.Length; i++)
            {
                if (array.NumberArray[i] == key)
                {
                    return i;
                }

                array.CompareCount++;
            }

            return -1;
        }
    }
}