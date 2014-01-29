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
        private BinaryTree binaryTreeClass;
        List<long> times = new List<long>();

        [SetUp]
        public void Init()
        {
            sortAlgoClass = new SortAlgo();
            sortHeapClass = new SortHeap();
            searchAlgoClass = new SearchAlgo();
            binaryTreeClass = new BinaryTree();

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

        // -------------------- Search --------------------
        [TestCase]
        public void TestLinearSearch()
        {
            int maxValue = 100;
            Random randNum = new Random();
            for (int i = 0; i < 10; i++)
            {
                int[] array = GenerateArray(InitialiseArray(maxValue), 0, maxValue);
                int randNumber = randNum.Next(0, maxValue);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int index = searchAlgoClass.linearSearch(array, randNumber);
                sw.Stop();
                if (index != -1)
                {
                    Console.WriteLine(String.Concat("Number ", randNumber, " found in ", sw.ElapsedMilliseconds, " msc, and need ", index, " rounds"));
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
            int maxValue = 1000;
            Random randNum = new Random();
            for (int i = 0; i < 10; i++)
            {
                int[] array = sortAlgoClass.insertionSort(GenerateArray(InitialiseArray(maxValue), 0, maxValue)); // First sort array because binary search works only with sorted arrays
                int randNumber = randNum.Next(0, maxValue);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int index = searchAlgoClass.BinarySearch(array, randNumber);
                sw.Stop();
                if (index != -1)
                {
                    Console.WriteLine(String.Concat("Number ", randNumber, " found in ", sw.ElapsedMilliseconds, " msc, and need ", index, " rounds"));
                }
                else
                {
                    Console.WriteLine(String.Concat("Number ", randNumber, " not found, need", sw.ElapsedMilliseconds, " msc"));
                }
            }
        }

        // -------------------- Tree --------------------
        [TestCase]
        public void TestBinaryTree()
        {
            int maxValue = 1000;
            Random randNum = new Random();
            for (int i = 0; i < 10; i++)
            {
                int randNumber = randNum.Next(0, maxValue);
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                binaryTreeClass.insert(i);
                //binaryTreeClass.
                //sw.Stop();
            }
            binaryTreeClass.postOrderTraversal();
            binaryTreeClass.preOrderTraversal();
            binaryTreeClass.inOrderTraversal();
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
