using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp3a
{
    class Program
    {
        static void Main(string[] args)
        {
            int maxValue = 1000;
            int[] array = CreateAndInitArray(1000, maxValue);
            ISearch search;

            search = new LinearSearch();

            int index;
            int[] searchValues = new int[10];

            Console.WriteLine("Linear search:");

            for (int i = 0; i < searchValues.Length; ++i)
            {
                searchValues[i] = new Random(i).Next(maxValue);
                index = search.Search(array, searchValues[i]);
                Output(search, index, searchValues[i]);
            }

            Bsp2a.ISort sort = new Bsp2a.InsertionSort();

            sort.Sort(ref array);

            search = new BinarySearch();

            Console.WriteLine();
            Console.WriteLine("Binary search:");

            for (int i = 0; i < searchValues.Length; ++i)
            {
                index = search.Search(array, searchValues[i]);
                Output(search, index, searchValues[i]);
            }

            Console.ReadLine();
        }

        private static void Output(ISearch search, int index, int searchValue)
        {
            if (index == -1)
            {
                Console.WriteLine("Value {0} not present in array; needed comparisons: {1}", searchValue, search.CompareCount);
            }
            else
            {
                Console.WriteLine("Found {0} at position {2} in array; needed comparisons: {1}", searchValue, search.CompareCount, index);
            }
        }

        private static int[] CreateAndInitArray(int maxCount, int maxValue)
        {
            if (maxCount < 0)
            {
                throw new ArgumentException("Value need to be greater or equal to 0", "maxCount");
            }

            if (maxValue < 0)
            {
                throw new ArgumentException("Value need to be greater or equal to 0", "maxValue");
            }

            int[] array = new int[maxCount];

            Random r = new Random();

            for (int i = 0; i < maxCount; ++i)
            {
                array[i] = r.Next(maxValue);
            }

            return array;
        }
    }
}
