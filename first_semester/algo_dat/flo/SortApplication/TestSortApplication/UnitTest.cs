using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SortApplication;

namespace TestSortApplication
{
    [TestFixture]
    public class UnitTest
    {
        private SortAlgo sortAlgoClass;
        private SortHeap sortHeapClass;
        private SearchAlgo searchAlgoClass;
        List<long> times = new List<long>();

        [SetUp]
        public void Init()
        {
            sortAlgoClass = new SortAlgo();
            sortHeapClass = new SortHeap();
            searchAlgoClass = new SearchAlgo();
        }

        [TestCase]
        public void TestSelectionSort()
        {
            int maxValue = 100;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int[] array = GenerateArray(InitialiseArray(maxValue), 0, maxValue);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int[] result = sortAlgoClass.selectionSort(array);
                    sw.Stop();
                    OutputTime(String.Concat("Array Length: ", maxValue), sw);
                }
                maxValue *= 10;
                OutputTimes();
            }
        }

        [TestCase]
        public void TestBubbleSort()
        {
            int maxValue = 100;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int[] array = GenerateArray(InitialiseArray(maxValue), 0, maxValue);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int[] result = sortAlgoClass.bubbleSort(array);
                    sw.Stop();
                    OutputTime(String.Concat("Array Length: ", maxValue), sw);
                }
                maxValue *= 10;
                OutputTimes();
            }
        }

        [TestCase]
        public void TestInsertionSort()
        {
            int maxValue = 100;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int[] array = GenerateArray(InitialiseArray(maxValue), 0, maxValue);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int[] result = sortAlgoClass.insertionSort(array);
                    sw.Stop();
                    OutputTime(String.Concat("Array Length: ", maxValue), sw);
                }
                maxValue *= 10;
                OutputTimes();
            }
        }

        [TestCase]
        public void TestHeapSort()
        {
            int maxValue = 100;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int[] array = GenerateArray(InitialiseArray(maxValue), 0, maxValue);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int[] result = sortHeapClass.heapSort(array);
                    sw.Stop();
                    OutputTime(String.Concat("Array Length: ", maxValue), sw);
                }
                maxValue *= 10;
                OutputTimes();
            }
        }

        [TestCase]
        public void TestLinearSearch()
        {
            int maxValue = 1000000;
            Random randNum = new Random();
            for (int i = 0; i < 10; i++)
            {
                int[] array = GenerateArray(InitialiseArray(maxValue), 0, maxValue);
                int randNumber = randNum.Next(0, maxValue);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int foundNumber = searchAlgoClass.linearSearch(array, randNumber);
                sw.Stop();
                if (foundNumber == randNumber)
                {
                    Console.WriteLine(String.Concat("Number ", foundNumber, " found in ", sw.ElapsedMilliseconds, " msc"));
                }
                else
                {
                    Console.WriteLine(String.Concat("Number not found! ", sw.ElapsedMilliseconds, " msc"));
                }
            }
        }

        [TestCase]
        public void TestBinarySearch()
        {
            int maxValue = 100000;
            Random randNum = new Random();
            for (int i = 0; i < 10; i++)
            {
                int[] array = sortAlgoClass.insertionSort(GenerateArray(InitialiseArray(maxValue), 0, maxValue)); // First sort array because binary search works only with sorted arrays
                int randNumber = randNum.Next(0, maxValue);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int foundNumber = searchAlgoClass.BinarySearch(array, 0, array.Length, randNumber);
                sw.Stop();
                if (foundNumber == randNumber)
                {
                    Console.WriteLine(String.Concat("Number ", foundNumber, " found in ", sw.ElapsedMilliseconds, " msc"));
                }
                else
                {
                    Console.WriteLine(String.Concat("Number ", randNumber, " not found, need", sw.ElapsedMilliseconds, " msc"));
                }
            }
        }

        private int[] InitialiseArray(int maxCount)
        {
            return new int[maxCount];
        }

        private int[] GenerateArray(int[] array, int min, int max)
        {
            Random randNum = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = randNum.Next(min, max);
            }
            return array;
        }

        private void OutputTime(string message, Stopwatch sw)
        {
            times.Add(sw.ElapsedMilliseconds);
            Console.WriteLine(message +
                ": Time StopWatch: " + sw.Elapsed.ToString()
                + " msecs " + sw.ElapsedMilliseconds.ToString()
                + " ticks " + sw.ElapsedTicks.ToString());
        }

        private void OutputTimes()
        {
            Console.WriteLine();
            Console.WriteLine("------------***********------------");
            Console.WriteLine("Min Time was: " + times.Min() + " msc");
            Console.WriteLine("Average Time was: " + times.Average() + " msc");
            Console.WriteLine("Max Time was: " + times.Max() + " msc");
            Console.WriteLine("------------***********------------");
            Console.WriteLine();
        }
    }
}
