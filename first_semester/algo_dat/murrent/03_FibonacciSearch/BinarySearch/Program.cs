using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SortHelper;

namespace BinarySearch
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

            for (int i = 0; i < randomNumbers.Length; i++)
            {
                int result = Search(array.ArraySorted, randomNumbers[i]);

                if (result == -1)
                {
                    Console.WriteLine("Search finished");
                }
                else
                {
                    Console.WriteLine("Found: " + randomNumbers[i]);
                }
            }

            stopwatch.Stop();

            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
            Console.WriteLine("Compare count: " + array.CompareCount);
            Console.ReadKey();

            stopwatch.Stop();
        }

        public static int Search(int[] arrayToSort, int value)
        {
            int lowerBound = 0;
            int higherBound = arrayToSort.Length;

            int mid;
            while (lowerBound <= higherBound)
            {
                mid = (lowerBound + higherBound) / 2;
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
    }
}
