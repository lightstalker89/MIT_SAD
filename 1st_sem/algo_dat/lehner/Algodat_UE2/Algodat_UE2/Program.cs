using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ALTestBench bench = new ALTestBench(100, 10000);
            bench.StartBubbleSort();
            bench.StartInsertionSort();
            bench.StartSelectionSort();
            bench.StartHeapSort();
        }
    }
}
