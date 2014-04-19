using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp2a
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = null;

            ISort sort;

            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        array = CreateAndInitArray(100, 50);
                        break;
                    case 1:
                        array = CreateAndInitArray(1000, 50);
                        break;
                    case 2:
                        array = CreateAndInitArray(10000, 50);
                        break;
                    case 3:
                        array = CreateAndInitArray(100000, 50);
                        break;
                }

                for (int j = 0; j < 3; j++)
                {
                    switch (j)
                    {
                        case 0:
                            sort = new InsertionSort();
                            break;
                        case 1:
                            sort = new SelectionSort();
                            break;
                        case 2:
                            sort = new BubbleSort();
                            break;
                        default:
                            sort = null;
                            break;
                    }

                    TimeSpan spanSort = new TimeSpan(0);

                    Console.Write("iteration ");
                    int k = 0;
                    for (; k < 10; ++k)
                    {
                        if (k > 0)
                        {
                            Console.Write(", ");
                        }

                        Console.Write(k + 1);
                        int[] testArray = (int[])array.Clone();
                        spanSort += Sort(sort, testArray);
                    }
                    Console.WriteLine();

                    Console.WriteLine("average time sorting " + array.Length.ToString().PadLeft(6) + " for " + sort.Name().PadRight(15) + ": " + new TimeSpan(spanSort.Ticks / k).ToString());
                }
            }

            Console.ReadLine();
        }

        private static TimeSpan Sort(ISort sort, int[] array)
        {
            if (array.Length <= 10)
            {
                PrintArray(array);
            }

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            Timing t = new Timing();
            t.StartTime();

            sort.Sort(ref array);

            //sw.Stop();
            t.StopTime();

            if (array.Length <= 10)
            {
                PrintArray(array);
                Console.WriteLine(t.Result().ToString());
            }

            return t.Result();
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

        private static void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                Console.Write("{0} ", array[i]);
            }

            Console.WriteLine();
        }
    }
}
