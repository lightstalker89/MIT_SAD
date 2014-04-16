using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beispiel3A
{
    public class BinarySearch : SearchAlogrithm
    {
        /// <summary>
        /// Search a value in a sorted array of integers 
        /// </summary>
        /// <param name="numbers">Sorted array of integers</param>
        /// <param name="value">Searched value</param>
        /// <param name="compareCount">Count of comparing</param>
        /// <returns>Index of searched value</returns>
        public override int Search(int[] numbers, int value, out int compareCount)
        {
            if (numbers == null)
            {
                throw new ArgumentNullException();
            }

            if (numbers.Length == 0)
            {
                throw new ArgumentException();
            }
            compareCount = 0;

            if (numbers.Length == 1 && numbers[0] != value)
            {
                // Just one item in the array and this item has not the searched value
                return -1;
            }

            int startIndex = 0;
            int endIndex = numbers.Length - 1;
            int middleIndex;

            while (startIndex <= endIndex)
            {
                middleIndex = (startIndex + endIndex) / 2;
                ++compareCount;
       
                if (numbers[middleIndex] < value)
                {
                    // Searched value exists on the right side 
                    startIndex = middleIndex + 1;
                }
                else if (numbers[middleIndex] > value)
                {
                    // Searched value exists on the left side
                    endIndex = middleIndex - 1;
                }
                else
                {
                    // numbers[middleIndex] == value
                    return middleIndex;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get the index for the minimum value of the integer array
        /// </summary>
        /// <param name="numbers">Sorted array of integers</param>
        /// <param name="compareCount">Count of comparing</param>
        /// <returns>Index of the minimum value</returns>
        public override int Min(int[] numbers, out int compareCount)
        {
            if (numbers == null)
            {
                throw new ArgumentNullException();
            }

            if (numbers.Length == 0)
            {
                throw new ArgumentException();
            }

            compareCount = 0;
            return 0;
        }

        /// <summary>
        /// Get the index for the maximum value of the integer array
        /// </summary>
        /// <param name="numbers">Sorted array of integers</param>
        /// <param name="compareCount">Count of comparing</param>
        /// <returns>Index of the maximum value</returns>
        public override int Max(int[] numbers, out int compareCount)
        {
            if (numbers == null)
            {
                throw new ArgumentNullException();
            }

            if (numbers.Length == 0)
            {
                throw new ArgumentException();
            }

            compareCount = 0;
            return numbers.Length - 1;
        }
    }
}
