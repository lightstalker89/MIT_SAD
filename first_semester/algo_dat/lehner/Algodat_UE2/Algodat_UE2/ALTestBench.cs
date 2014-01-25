using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Algodat_UE2.Algos;

namespace Algodat_UE2
{
    public class ALTestBench
    {

        private int MaxValue { get; set; }
        private int MaxSize { get; set; }
        private int[] NumberArray { get; set; }

        public ALTestBench(int maxValue, int maxSize)
        {
            this.MaxSize = maxSize;
            this.MaxValue = maxValue;
            this.NumberArray = this.GenerateRandomArray();
        }

        public int[] GenerateRandomArray()
        {
            int[] numArray = new int[this.MaxSize];
            Random randNum = new Random();
   
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = randNum.Next(MaxValue);
            }

            return numArray;
        }

        public void StartBubbleSort()
        {
            int count = 1;
            List<long> times = new List<long>();

            BubbleSortAlgo bubble = new BubbleSortAlgo();
            Stopwatch watch = new Stopwatch();

            while (count != 10)
            {
                int[] array = GenerateRandomArray();
                watch.Start();
                bubble.Sort(array);
                watch.Stop();
                times.Add(watch.ElapsedMilliseconds);
                watch.Reset();
                count++;
            }

            long average = GetAverageValue(times);

            Console.WriteLine("Bubble Sort average Time: {0}ms", average);
        }

        public void StartInsertionSort()
        {
            int count = 1;
            List<long> times = new List<long>();

            InsertionSortAlgo insertionAlgo = new InsertionSortAlgo();
            Stopwatch watch = new Stopwatch();

            while (count != 10)
            {
                int[] array = this.GenerateRandomArray();
                watch.Start();
                insertionAlgo.Sort(array);
                watch.Stop();
                times.Add(watch.ElapsedMilliseconds);
                watch.Reset();
                count++;
            }

            long average = GetAverageValue(times);

            Console.WriteLine("Insertion Sort average Time: {0}ms", average);
        }

        public void StartSelectionSort()
        {
            int count = 1;
            List<long> times = new List<long>();

            SelectionSortAlgo selectionAlgo = new SelectionSortAlgo();
            Stopwatch watch = new Stopwatch();

            while (count != 10)
            {
                int[] array = this.GenerateRandomArray();
                watch.Start();
                selectionAlgo.Sort(array);
                watch.Stop();
                times.Add(watch.ElapsedMilliseconds);
                watch.Reset();
                count++;
            }

            long average = GetAverageValue(times);

            Console.WriteLine("Selection Sort average Time: {0}ms", average);
        }

        public void StartHeapSort()
        {
            int count = 1;
            List<long> times = new List<long>();

            HeapSortAlgo heapAlgo = new HeapSortAlgo();
            Stopwatch watch = new Stopwatch();

            while (count != 10)
            {
                int[] array = this.GenerateRandomArray();
                watch.Start();
                heapAlgo.Sort(array);
                watch.Stop();
                times.Add(watch.ElapsedMilliseconds);
                watch.Reset();
                count++;
            }

            long average = GetAverageValue(times);

            Console.WriteLine("Heap Sort average Time: {0}ms", average);
        }

        public long GetAverageValue(List<long> times)
        {
            long allTimes = 0;
            foreach (long item in times)
            {
                allTimes += item;
            }

            return (allTimes / times.Count);
        }

    }
}
