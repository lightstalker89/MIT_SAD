using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListRandomNumbers;
using System.Collections;

namespace Heap
{
    class Program
    {
        static void Main(string[] args)
        {
            RandomNumbers rnumbers = new RandomNumbers();
            ArrayList unsortedArray = rnumbers.GetNonRepeatingRandomNumbers(10);

            HeapSort sortAlgorithm = new HeapSort();
            Console.WriteLine("UnsortedList");
            sortAlgorithm.Output(unsortedArray);
            unsortedArray = sortAlgorithm.Sort(unsortedArray);
            Console.WriteLine("-HeapSort- SortedList");
            sortAlgorithm.Output(unsortedArray);

            Console.ReadKey();
        }
    }
}
