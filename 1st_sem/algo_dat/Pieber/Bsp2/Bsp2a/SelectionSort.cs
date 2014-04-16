using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp2a
{
    public class SelectionSort : ISort
    {
        public string Name()
        {
            return "SelectionSort";
        }

        public void Sort(ref int[] array)
        {
            int indexWithSmallestValue;

            for (int i = 0; i < array.Length - 1; ++i)
            {
                indexWithSmallestValue = i;

                for (int j = i + 1; j < array.Length; ++j)
                {
                    if (array[indexWithSmallestValue] > array[j])
                    {
                        indexWithSmallestValue = j;
                    }
                }

                if (indexWithSmallestValue != i)
                {
                    int tmp = array[indexWithSmallestValue];
                    array[indexWithSmallestValue] = array[i];
                    array[i] = tmp;
                }
            }
        }
    }
}
