using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Algodat_UE3.SearchAlgos;

namespace Algodat_UE3
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            int[] searchNumbers = new int[10];

            for (int i = 0; i < 10; i++)
            {
                searchNumbers[i] = r.Next(100);
            }

            LinearSearch linSearch = new LinearSearch();
            BinarySearch binSearch = new BinarySearch();
            int[] randomNumbers = GenerateRandomArray(1000, 100);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < searchNumbers.Length; i++)
            {
                int result = linSearch.Search(searchNumbers[i], randomNumbers);

                if (result != -1)
                {
                    Console.WriteLine("Found: " + searchNumbers[i]);
                }
            }

            watch.Stop();
            Console.WriteLine("Linear Time: " + watch.ElapsedMilliseconds + "ms");
            Console.WriteLine("Linear Compare count: " + linSearch.CompareCount);
            watch.Reset();

            Array.Sort(randomNumbers);
            watch.Start();

            for (int i = 0; i < searchNumbers.Length; i++)
            {
                int result = binSearch.Search(searchNumbers[i], randomNumbers);

                if (result != -1)
                {
                    Console.WriteLine("Found: " + searchNumbers[i]);
                }
            }

            watch.Stop();
            Console.WriteLine("Binary Time: " + watch.ElapsedMilliseconds + "ms");
            Console.WriteLine("Binary Compare count: " + binSearch.CompareCount);
            watch.Reset();
        }

        public static int[] GenerateRandomArray(int size, int maxValue)
        {
            int[] numArray = new int[size];
            Random randNum = new Random();

            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = randNum.Next(maxValue);
            }

            return numArray;
        }
    }
}
