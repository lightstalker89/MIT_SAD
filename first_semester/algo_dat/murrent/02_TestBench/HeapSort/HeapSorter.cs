using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeapSort
{
    public class HeapSorter
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly int[] arrayToSort;
        private readonly int arrayLength;

        public HeapSorter(int[] toSort)
        {
            this.arrayToSort = toSort;
            this.arrayLength = this.arrayToSort.Length;
        }

        public double ElapsedTime { get; set; }

        public void Sort()
        {
            stopwatch.Reset();
            stopwatch.Start();

            this.GenerateMaxHeap(this.arrayToSort);

            for (int i = this.arrayLength - 1; i >= 0; i += -1)
            {
                this.Swap(this.arrayToSort, i, 0);
                this.Sink(this.arrayToSort, 0, i);
            }

            stopwatch.Stop();
            this.ElapsedTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("Sort completed with: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
        }

        private void GenerateMaxHeap(int[] a)
        {
            for (int i = (a.Length / 2 - 1); i >= 1; i += -1)
            {
                this.Sink(a, i, a.Length);
            }
        }

        private void Sink(int[] a, int i, int n)
        {
            while (i <= (n / 2 - 1))
            {
                int kindIndex = (i + 1) * 2 - 1;

                if (kindIndex + 1 <= n - 1)
                {
                    if (a[kindIndex] < a[kindIndex + 1])
                        kindIndex += 1;

                }

                if (a[i] < a[kindIndex])
                {
                    this.Swap(a, i, kindIndex);
                    i = kindIndex;
                }
                else { break; }


            }
        }

        private void Swap(int[] a, int i, int kindIndex)
        {
            int z = a[i];
            a[i] = a[kindIndex];
            a[kindIndex] = z;
        }
    }
}
