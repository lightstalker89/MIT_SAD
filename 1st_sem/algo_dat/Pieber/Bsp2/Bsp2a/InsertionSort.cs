using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp2a
{
    public class InsertionSort : ISort
    {
        public string Name()
        {
            return "InsertionSort";
        }

        public void Sort(ref int[] array)
        {
            int j, length = array.Length, cur;

            for (int i = 1; i < length; ++i)
            {
                cur = array[i];
                for (j = i - 1; j >= 0 && array[j] > cur; --j)
                {
                    array[j + 1] = array[j];
                }

                array[j + 1] = cur;
            }
        }
    }
}
