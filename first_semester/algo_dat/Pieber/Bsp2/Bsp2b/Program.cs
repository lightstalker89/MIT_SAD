using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp2b
{
    class Program
    {
        static void Main(string[] args)
        {
            HeapSort sort = new HeapSort();

            int[] array = CreateAndInitArray(100, 50);

            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();

            Bsp2a.Timing t = new Bsp2a.Timing();

            t.StartTime();

            sort.CreateHeap(ref array);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = sort.GetBiggestValueAndReorganiseHeap();
                //Console.Write(sort.GetBiggestValueAndReorganiseHeap() + " ");
            }
            //Console.WriteLine();

            t.StopTime();
            Console.WriteLine(t.Result().ToString());

            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();

            Console.ReadLine();
        }

        private static int[] CreateAndInitArray(int maxCount, int maxValue)
        {
            if (maxCount < 0)
            {
                throw new ArgumentException("Value need to be greater or equal to 0", "maxCount");
            }

            if (maxValue < 0)
            {
                throw new ArgumentException("Value need to be greater or equal to 0", "maxValue");
            }

            int[] array = new int[maxCount];

            Random r = new Random();

            for (int i = 0; i < maxCount; ++i)
            {
                array[i] = r.Next(maxValue);
            }

            return array;
        }
    }
}
