using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE2.Algos
{
    public class SelectionSortAlgo
    {
        public void Sort(int[] numberArray)
        {
            int min, temp;

            for (int i = 0; i < numberArray.Length - 1; i++)
            {
                min = i;

                for (int j = i + 1; j < numberArray.Length; j++)
                {
                    if (numberArray[j] < numberArray[min])
                    {
                        min = j;
                    }
                }

                temp = numberArray[i];
                numberArray[i] = numberArray[min];
                numberArray[min] = temp;
            }
        }
    }
}
