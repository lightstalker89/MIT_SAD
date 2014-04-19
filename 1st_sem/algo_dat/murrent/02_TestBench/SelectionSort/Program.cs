// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace SelectionSort
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// </summary>
        private static readonly List<double> times = new List<double>();

        /// <summary>
        /// </summary>
        private static SelectionSorter selectionSorter;

        /// <summary>
        /// </summary>
        private static CArray numbers;

        /// <summary>
        /// </summary>
        private static readonly int[] selectionSortValueCount = { 100, 1000, 10000, 100000 };

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            foreach (int valueCount in selectionSortValueCount)
            {
                numbers = new CArray(valueCount, 589);
                times.Clear();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Testing selection sort with " + valueCount + " values:");
                Console.WriteLine("___________________________________________");
                Console.WriteLine();
                Console.ResetColor();

                for (int x = 0; x < 10; ++x)
                {
                    selectionSorter = new SelectionSorter(numbers.NumberArray);
                    selectionSorter.Sort();
                    times.Add(selectionSorter.ElapsedTime);
                }

                Console.WriteLine("Average time: " + times.Average() + "ms");
            }
        }
    }
}