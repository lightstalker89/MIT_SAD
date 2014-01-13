using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectionSort
{
    public class SelectionSorter
    {
        Stopwatch stopwatch = new Stopwatch();

        private int[] arrayToSort;
        private readonly int arrayLength;

        public SelectionSorter(int[] toSort)
        {
            this.arrayToSort = toSort;
            this.arrayLength = this.arrayToSort.Length;
        }

        public double ElapsedTime { get; set; }

        public void Sort()
        {
            stopwatch.Reset();
            stopwatch.Start();

            for (int i = 0; i < arrayLength - 1; ++i)
            {
                int minPos = this.FindMinPosition(i);
                this.SwapPositions(minPos, i);
            }

            stopwatch.Stop();
            this.ElapsedTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("Sort completed with: " + stopwatch.ElapsedMilliseconds + "ms - " + stopwatch.ElapsedTicks + " ticks");
        }

        private int FindMinPosition(int positionFrom)
        {
            int minPos = positionFrom;

            for (int i = positionFrom + 1; i < arrayLength; ++i)
            {
                if (arrayToSort[i] < arrayToSort[minPos])
                {
                    minPos = i;
                }
            }

            return minPos;
        }

        private void SwapPositions(int positionA, int positionB)
        {
            int tempPositionA = this.arrayToSort[positionA];
            this.arrayToSort[positionA] = positionB;
            this.arrayToSort[positionB] = tempPositionA;
        }
    }
}
