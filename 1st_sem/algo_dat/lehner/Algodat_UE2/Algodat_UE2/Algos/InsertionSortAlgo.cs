using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE2.Algos
{
    class InsertionSortAlgo
    {
        public void Sort(int[] numberArray)
        {
           // this.PrintArray(numberArray);

            int temp1 = 0;
            int temp2 = 0;

            for (int i = 1; i < numberArray.Length; i++)
            {
                temp1 = numberArray[i];
                temp2 = i - 1;

                while (temp2 >= 0 && numberArray[temp2] > temp1)
                {
                    numberArray[temp2 + 1] = numberArray[temp2];
                    temp2--;
                }
                numberArray[temp2 + 1] = temp1;
            }

           // this.PrintArray(numberArray);
        }

        public void PrintArray(int[] numberArray)
        {
            StringBuilder strB = new StringBuilder();
            foreach (int item in numberArray)
            {
                strB.Append(string.Format("{0} ", item));
            }

            Console.WriteLine(strB.ToString());
        }
    }
}
