using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SortHelper;

namespace BubbleSort
{
    class Program
    {
        private static readonly List<double> times = new List<double>();
        private static BubbleSorter bubbleSorter;
        private static CArray numbers;
        private static readonly int[] selectionSortValueCount = { 100, 1000, 10000, 100000 };

        static void Main(string[] args)
        {
            foreach (int valueCount in selectionSortValueCount)
            {
                numbers = new CArray(valueCount, 589);
                times.Clear();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Testing bubble sort with " + valueCount + " values:");
                Console.WriteLine("___________________________________________");
                Console.WriteLine();
                Console.ResetColor();

                for (int x = 0; x < 10; ++x)
                {
                    bubbleSorter = new BubbleSorter(numbers.Array);
                    bubbleSorter.Sort();
                    times.Add(bubbleSorter.ElapsedTime);
                }

                Console.WriteLine("Average time: " + times.Average() + "ms");
            }
        }
    }
}
