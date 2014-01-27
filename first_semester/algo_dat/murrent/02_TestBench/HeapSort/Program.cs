// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using System.Collections.Generic;
using System.Linq;
using SortHelper;

namespace HeapSort
{
    class Program
    {
        private static readonly List<double> times = new List<double>();
        private static HeapSorter heapSorter;
        private static CArray numbers;
        private static readonly int[] heapSortValueCount = { 100, 1000, 10000, 100000 };

        static void Main(string[] args)
        {
            numbers = new CArray(100, 589);
            times.Clear();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Testing bubble sort with 100 values:");
            Console.WriteLine("___________________________________________");
            Console.WriteLine();
            Console.ResetColor();

            for (int x = 0; x < 10; ++x)
            {
                heapSorter = new HeapSorter(numbers.NumberArray);
                heapSorter.Sort();
                times.Add(heapSorter.ElapsedTime);
            }

            Console.WriteLine("Average time: " + times.Average() + "ms");
        }
    }
}
