using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp3a
{
    class BinarySearch : ISearch
    {
        public int CompareCount { get; private set; }

        public int Search(int[] array, int value)
        {
            int lBound = 0, uBound = array.Length - 1;
            int index = Math.Max(0, ((uBound - lBound) >> 1) - 1);

            this.CompareCount = 0;

            while (uBound >= lBound)
            {
                ++this.CompareCount;

                if (array[index] == value)
                {
                    return index;
                }
                else if (array[index] > value)
                {
                    uBound = index - 1;
                    index -= ((uBound - lBound) >> 1) + 1;
                }
                else
                {
                    lBound = index + 1;
                    index += ((uBound - lBound) >> 1) + 1;
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

            return 0;
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

            return array.Length - 1;
        }
    }
}
