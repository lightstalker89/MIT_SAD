using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp3a
{
    class LinearSearch : ISearch
    {
        public int CompareCount { get; private set; }

        public int Search(int[] array, int value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            this.CompareCount = array.Length;

            for (int i = 0; i < array.Length; ++i)
            {
                if (value == array[i])
                {
                    this.CompareCount = i + 1;
                    return i;
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

            int minIndex = int.MaxValue;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < array[minIndex])
                {
                    minIndex = i;
                }
            }

            return minIndex;
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

            int maxIndex = int.MinValue;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > array[maxIndex])
                {
                    maxIndex = i;
                }
            }

            return maxIndex;
        }
    }
}
