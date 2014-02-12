using System;
using System.Collections.Generic;
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
            List<int> sortedList = new List<int>();
            CArray unsortedArray = new CArray(1000, 1500);

            Console.WriteLine("UnsortedList");

            // Selection Sort
            SortAlgorithm selectionSort = new SelectionSort();
            selectionSort.Output(unsortedArray.UnsortedArray.ToList<int>());
            Console.WriteLine(("Selection-Sort"));
            Timing.Timing timing = new Timing.Timing();
            timing.StartTime();
            sortedList = selectionSort.Sort(unsortedArray.UnsortedArray.ToList<int>());
            timing.StopTime();
            selectionSort.Output(sortedList);
            Console.WriteLine("Time result: {0}", timing.Result().ToString());

            Console.ReadKey();

            // Insertion Sort
            SortAlgorithm insertionSort = new InsertionSort();
            Console.WriteLine("Insertion-Sort");
            timing = new Timing.Timing();
            timing.StartTime();
            sortedList = insertionSort.Sort(unsortedArray.UnsortedArray.ToList<int>());
            timing.StopTime();
            insertionSort.Output(sortedList);
            Console.WriteLine("Time result: {0}", timing.Result().ToString());

            Console.ReadKey();

            // Bubble Sort
            SortAlgorithm bubbleSort = new BubbleSort();
            Console.WriteLine("Bubble-Sort");
            timing = new Timing.Timing();
            timing.StartTime();
            sortedList = bubbleSort.Sort(unsortedArray.UnsortedArray.ToList<int>());
            timing.StopTime();
            bubbleSort.Output(sortedList);
            Console.WriteLine("Time result: {0}", timing.Result().ToString());

            Console.ReadKey();
        }
    }
}
