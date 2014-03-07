using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beispiel3A
{
    public class LinearSearch : SearchAlogrithm
    {
        /// <summary>
        /// Search a value in an unsorted array of integers
        /// </summary>
        /// <param name="numbers">Unsorted array of integers</param>
        /// <param name="value">Value to search for</param>
        /// <param name="compareCount">Count of comparing</param>
        /// <returns>Index of the searched value</returns>
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

            for (int i = 0; i < numbers.Length; ++i)
            {
                if (numbers[i] == value)
                {
                    compareCount = i + 1;
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the index for the minimum value of the array
        /// </summary>
        /// <param name="numbers">Unsorted array of integers</param>
        /// <param name="compareCount">Count of comparing</param>
        /// <returns>Index of the min value</returns>
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

            int min = int.MaxValue;
            compareCount = 0;

            for (int i = 0; i < numbers.Length; ++i)
            {
                if (numbers[i] < min && numbers[i] > 0)
                {
                    min = i;
                    compareCount = i + 1;
                }
            }

            return min;
        }

        /// <summary>
        /// Returns the index for the maximum value of the array
        /// </summary>
        /// <param name="numbers">Unsorted array of integers</param>
        /// <param name="compareCount">Count of comparing</param>
        /// <returns>Index of the max value</returns>
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

            int maxValue = int.MinValue;
            compareCount = 0;

            for (int i = 0; i < numbers.Length; ++i)
            {
                if (numbers[i] > 0 && numbers[i] > maxValue)
                {
                    maxValue = i;
                    compareCount = i + 1;
                }
            }

            return maxValue;
        }
    }
}
