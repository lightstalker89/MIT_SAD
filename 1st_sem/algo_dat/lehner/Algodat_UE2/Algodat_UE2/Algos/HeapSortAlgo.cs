using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE2.Algos
{
    class HeapSortAlgo
    {
        public void Sort(int[] numberArray)
        {
            //this.PrintArray(numberArray);
            generateMaxHeap(numberArray);

            for (int i = numberArray.Length - 1; i >= 0; i += -1)
            {
                this.change(numberArray, i, 0);
                this.shift(numberArray, 0, i);
            }
            //this.PrintArray(numberArray);
        }
        // generate max heap tree
        private void generateMaxHeap(int[] a)
        {
            for (int i = (int)(a.Length / 2 - 1); i >= 1; i += -1)
            {
                this.shift(a, i, a.Length);
            }
        }

        private void shift(int[] a, int i, int n)
        {
            while (i <= (n / 2 - 1))
            {
                // index of left child
                int childIndex = (i + 1) * 2 - 1;

                // check for if right child exists
                if (childIndex + 1 <= n - 1)
                {
                    // if right child is bigger take it
                    if (a[childIndex] < a[childIndex + 1])
                        childIndex += 1;
                }

                // check if element has to sink
                if (a[i] < a[childIndex])
                {
                    this.change(a, i, childIndex);
                    i = childIndex;
                }
                else { break; }


            }
        }
        // switch elements
        private void change(int[] a, int i, int childIndex)
        {
            int z = a[i];
            a[i] = a[childIndex];
            a[childIndex] = z;
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
