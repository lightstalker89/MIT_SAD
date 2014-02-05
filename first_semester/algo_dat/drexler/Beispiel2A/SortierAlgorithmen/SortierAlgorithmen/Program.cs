using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortierAlgorithmen
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

            SortAlgorithm sortAlgorithm = new SelectionSort();
            Console.WriteLine("UnsortedList");
            sortAlgorithm.Output(unsortedList);


            // Console.WriteLine(("Selection-Sort"));
            // sortAlgorithm.Output(sortAlgorithm.Sort(unsortedList));

            //Console.ReadKey();

            //sortAlgorithm = new InsertionSort();
            //Console.WriteLine("Insertion-Sort");
            //sortAlgorithm.Output(sortAlgorithm.Sort(unsortedList));

            Console.ReadKey();

            sortAlgorithm = new BubbleSort();
            Console.WriteLine("Bubble-Sort");
            sortAlgorithm.Output(sortAlgorithm.Sort(unsortedList));

            Console.ReadKey();
        }
    }
}
