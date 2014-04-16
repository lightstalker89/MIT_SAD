using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp2a
{
    public class BubbleSort : ISort
    {
        public string Name()
        {
            return "BubbleSort";
        }

        public void Sort(ref int[] array)
        {
            bool exchanged = true;

            int length = array.Length;

            for (int i = 0; exchanged && i < length - 1; ++i)
            {
                exchanged = false;

                for (int j = 0; j < length - 1; ++j)
                {
                    if (array[j] > array[j + 1])
                    {
                        int tmp = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = tmp;
                        exchanged = true;
                    }
                }
            }
        }
    }
}
