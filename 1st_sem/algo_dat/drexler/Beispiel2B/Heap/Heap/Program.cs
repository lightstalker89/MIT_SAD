using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListRandomNumbers;
using System.Collections;
using Timing;

namespace Heap
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> sortedList = new List<int>();
            ListRandomNumbers.CArray cArray = new ListRandomNumbers.CArray(100, 150);

            Console.WriteLine("UnsortedList");
            Console.WriteLine("-------------------------------------------");
            HeapSort sortAlgorithm = new HeapSort();
            sortAlgorithm.Output(cArray.UnsortedArray.ToList());

            //Timing.Timing stopWatch = new Timing.Timing();
            Timing.Timing stopWatch = new Timing.Timing();
            stopWatch.StartTime();
            sortedList = sortAlgorithm.Sort(cArray.UnsortedArray.ToList());
            stopWatch.StopTime();

            Console.WriteLine("-HeapSort- SortedList");
            Console.WriteLine("-------------------------------------------");
            sortAlgorithm.Output(sortedList);
            Console.WriteLine("Time result: {0}", stopWatch.Result().ToString());

            Console.ReadKey();
        }
    }
}
