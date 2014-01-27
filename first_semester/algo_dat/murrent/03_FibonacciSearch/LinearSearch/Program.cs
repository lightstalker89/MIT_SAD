// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using System.Diagnostics;
using SortHelper;

namespace LinearSearch
{
    class Program
    {
        private static readonly Random random = new Random();
        private static CArray array;
        private static readonly int[] randomNumbers = new int[10];
        private static readonly Stopwatch stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                randomNumbers[i] = random.Next(100);
            }

            array = new CArray(1000, 100);

            stopwatch.Start();

            foreach (int t in randomNumbers)
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

            stopwatch.Stop();

            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
            Console.WriteLine("Compare count: " + array.CompareCount);
            Console.ReadKey();
        }

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
