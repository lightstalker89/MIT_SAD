using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            List<int> unsortedList = new List<int>();
            int i = 10;
            while (i >= 0)
            {
                unsortedList.Add(random.Next(20));
                --i;
            }


            HeapSort sortAlgorithm = new HeapSort();
            Console.WriteLine("UnsortedList");
            sortAlgorithm.Output(unsortedList);
            unsortedList = sortAlgorithm.Sort(unsortedList);
            Console.WriteLine("-HeapSort- SortedList");
            sortAlgorithm.Output(unsortedList);

            Console.ReadKey();
        }
    }
}
