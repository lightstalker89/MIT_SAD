using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp2b
{
    class HeapSort
    {
        private int[] heap;
        private int heapSize;

        public string Name()
        {
            return "HeapSort";
        }

        public void CreateHeap(ref int[] array)
        {
            heap = new int[array.Length + 1];
            heapSize = array.Length;
            int current, currentHalf;

            for (int i = 0; i < array.Length; ++i)
            {
                current = i + 1;
                currentHalf = current >> 1;
                heap[current] = array[i];
                currentHalf = current >> 1;

                while (current > 1 && heap[current] > heap[currentHalf])
                {
                    int tmp = heap[currentHalf];
                    heap[currentHalf] = heap[current];
                    heap[current] = tmp;
                    current = currentHalf;
                    currentHalf = current >> 1;
                }
            }
        }

        public int GetBiggestValueAndReorganiseHeap()
        {
            int retValue = heap[1];
            int next;

            heap[1] = heap[heapSize];

            for (int i = 1; (i << 1) <= heapSize; i = next)
            {
                next = i << 1;

                // take the bigger value for changing position
                if (next < heapSize && heap[next] < heap[next + 1])
                {
                    ++next;
                }

                if (heap[i] < heap[next])
                {
                    int tmp = heap[i];
                    heap[i] = heap[next];
                    heap[next] = tmp;
                }
            }

            return retValue;
        }
    }
}
