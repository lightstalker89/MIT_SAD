//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Beispiel3A
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ListRandomNumbers;

    /// <summary>
    /// Entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point function
        /// </summary>
        /// <param name="args">Passed arguments</param>
        static void Main(string[] args)
        {
            Console.SetBufferSize(999, 999);
            Console.SetWindowSize(150, 69);

            // Random numbers (Range: 1 - 4);
            CArray randomNumbers = new CArray(2, 4);

            // LINEAR SEARCH START
            SearchAlogrithm linearSearch = new LinearSearch();
            int compcount = 0;
            MyToString(randomNumbers.UnsortedArray);
            int index = linearSearch.Search(randomNumbers.UnsortedArray, 2, out compcount);
            Console.WriteLine("Searched value: {0}, Found index: {1}, compare count: {2}", 2, index, compcount);

            Console.ReadKey();

            compcount = 0;
            MyToString(randomNumbers.UnsortedArray);
            int minValue = linearSearch.Min(randomNumbers.UnsortedArray, out compcount);
            Console.WriteLine("Index of min value from unsorted list: {0}, compare count: {1}", minValue, compcount);

            Console.ReadKey();

            compcount = 0;
            MyToString(randomNumbers.UnsortedArray);
            int maxValue = linearSearch.Max(randomNumbers.UnsortedArray, out compcount);
            Console.WriteLine("Index of max value from unsorted list: {0}, compare count: {1}", maxValue, compcount);

            Console.ReadKey();
            // LINEAR SEARCH END

            // BINARY SEARCH START
            compcount = 0;
            SearchAlogrithm binarySearch = new BinarySearch();
            MyToString(randomNumbers.SortedArray);
            index = binarySearch.Search(randomNumbers.SortedArray, 2, out compcount);
            Console.WriteLine("Searched value: {0}, Found index: {1}, compare count: {2}", 2, index, compcount);
            Console.WriteLine("Index of min value from sorted list: {0}", binarySearch.Min(randomNumbers.SortedArray, out compcount));
            Console.WriteLine("Index of max value from sorted list: {0}", binarySearch.Max(randomNumbers.SortedArray, out compcount));
            // BINARY SEARCH END

            Console.ReadKey();
        }

        /// <summary>
        /// Output an array of integers to the console
        /// </summary>
        /// <param name="array">Array of integers</param>
        static void MyToString(int[] array)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                Console.WriteLine("[{0}]: {1}", i, array[i]);
            }

            Console.WriteLine();
        }
    }
}
