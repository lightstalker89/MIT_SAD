using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE2.Algos
{
    public class BubbleSortAlgo
    {
        public void Sort(int[] numberArray)
        {
            int temp = 0;

            //this.PrintArray(numberArray);

            for (int i = 0; i < numberArray.Length; i++)
            {
                for (int j = 0; j < numberArray.Length - 1 - i; j++)
                {
                    if (numberArray[j] > numberArray[j + 1])
                    {
                        temp = numberArray[j + 1];
                        numberArray[j + 1] = numberArray[j];
                        numberArray[j] = temp;
                    }
                }
            }
            //this.PrintArray(numberArray);
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
