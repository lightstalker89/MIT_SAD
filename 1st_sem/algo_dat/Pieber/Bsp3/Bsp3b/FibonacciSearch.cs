using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp3b
{
    class FibonacciSearch : Bsp3a.ISearch
    {
        private static int[] FibonacciValues = new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 196418, 317811, 514229, 832040, 1346269, 2178309, 3524578, 5702887, 9227465, 14930352, 24157817, 39088169, 63245986, 102334155, 165580141, 267914296, 433494437, 701408733, 1134903170, 1836311903 };

        public int CompareCount { get; private set; }

        /// <summary>
        /// Searches the specified array.
        /// </summary>
        /// <param name="array">The array, which needs to be sorted descending.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>Index of the found value to search for, or -1 if the value couldn't be found.</returns>
        /// <exception cref="System.ArgumentNullException">array</exception>
        public int Search(int[] array, int value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            int lBound = 0, uBound = array.Length - 1;
            int index, fiboIndex;
            this.CompareCount = 0;

            while (uBound >= lBound)
            {
                fiboIndex = -1;
                // search highest possible Fibonacci number which is smaller than the boundary
                // and take that number as index for the search array to compare with
                while (FibonacciValues[++fiboIndex] < uBound - lBound) ;
                index = lBound + FibonacciValues[fiboIndex > 1 ? --fiboIndex : 0];

                ++this.CompareCount;

                if (array[index] == value)
                {
                    return index;
                }
                else if (array[index] < value)
                {
                    uBound = index - 1;
                }
                else
                {
                    lBound = index + 1;
                }
            }

            return -1;
        }

        public int GetMinValue(int[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Length == 0)
            {
                throw new ArgumentException("Given array has no elements.", "array");
            }

            return array.Length - 1;
        }

        public int GetMaxValue(int[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Length == 0)
            {
                throw new ArgumentException("Given array has no elements.", "array");
            }

            return 0;
        }
    }
}
