using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            stopwatch.Start();

            array = new CArray(1000, 100);

            for (int i = 0; i < randomNumbers.Length; i++)
            {
                int result = Search(randomNumbers[i]);

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
