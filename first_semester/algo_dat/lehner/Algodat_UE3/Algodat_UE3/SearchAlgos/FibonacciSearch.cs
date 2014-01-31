using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE3.SearchAlgos
{
    class FibonacciSearch
    {

        public int CompareCount {get; private set; }

        private static int[] FibonacciValues = new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 196418, 317811, 514229, 832040, 1346269, 2178309, 3524578, 5702887, 9227465, 14930352, 24157817, 39088169, 63245986, 102334155, 165580141, 267914296, 433494437, 701408733, 1134903170, 1836311903 };

        public FibonacciSearch()
        {
            this.CompareCount = 0;
        }

        public int Search(int number, int[] compareArray)
        {
            int FirstIndex = 0;
            int LastIndex = compareArray.Length;

            if (number > compareArray[compareArray.Length - 1])
            {
                this.CompareCount++;
                return -1;
            }

            if (number < compareArray[0])
            {
                this.CompareCount++;
                return -1;
            }

            while (FirstIndex <= LastIndex)
            {
                this.CompareCount++;

                int Mid = this.FindFibonacciNumber(FirstIndex, LastIndex) + FirstIndex;
                if (number > compareArray[Mid])
                {
                    FirstIndex = Mid + 1;
                }
                else if (number < compareArray[Mid])
                {
                    LastIndex = Mid - 1;
                }
                else
                {
                    return Mid;
                }
            }
            return -1;
        }

        private int FindFibonacciNumber(int firstIndex, int lastIndex)
        {
            int value = lastIndex - firstIndex;

            int number = 0;

            if (value == 0)
            {
                return value;
            }

            while (value > FibonacciValues[number])
            {
                number++;
            }

            return FibonacciValues[number - 1];
        }
    }
}
