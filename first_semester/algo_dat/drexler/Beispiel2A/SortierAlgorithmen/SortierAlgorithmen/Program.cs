using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListRandomNumbers;
using Timing;
using System.Collections;

namespace SortierAlgorithmen
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            List<int> sortedList = new List<int>();
            // CArray unsortedArray = new CArray(100, 150);
            // CArray unsortedArray = new CArray(1000, 1200);
            CArray unsortedArray = new CArray(100000, 120000);

            Console.WriteLine("UnsortedList");

            // Selection Sort
            SortAlgorithm selectionSort = new SelectionSort();
            // selectionSort.Output(unsortedArray.UnsortedArray.ToList<int>());
            Console.WriteLine(("Selection-Sort"));

            stopwatch.Reset();
            stopwatch.Start();
            sortedList = selectionSort.Sort(unsortedArray.UnsortedArray.ToList<int>());
            stopwatch.Stop();
            // selectionSort.Output(sortedList);
            Console.WriteLine("Time result: {0}ms", stopwatch.ElapsedMilliseconds.ToString());

            Console.ReadKey();

            // Insertion Sort
            SortAlgorithm insertionSort = new InsertionSort();
            Console.WriteLine("Insertion-Sort");
            stopwatch.Reset();
            stopwatch.Start();
            sortedList = insertionSort.Sort(unsortedArray.UnsortedArray.ToList<int>());
            stopwatch.Stop();
            // insertionSort.Output(sortedList);
            Console.WriteLine("Time result: {0}ms", stopwatch.ElapsedMilliseconds.ToString());

            Console.ReadKey();

            // Bubble Sort
            SortAlgorithm bubbleSort = new BubbleSort();
            Console.WriteLine("Bubble-Sort");
            stopwatch.Reset();
            stopwatch.Start();
            sortedList = bubbleSort.Sort(unsortedArray.UnsortedArray.ToList<int>());
            stopwatch.Stop();
            // bubbleSort.Output(sortedList);
            Console.WriteLine("Time result: {0}ms", stopwatch.ElapsedMilliseconds.ToString());

            Console.ReadKey();
        }
    }
}
