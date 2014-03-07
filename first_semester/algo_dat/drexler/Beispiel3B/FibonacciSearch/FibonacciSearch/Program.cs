//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace FibonacciSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ListRandomNumbers;

    class Program
    {
        static void Main(string[] args)
        {
            CArray array = new CArray(10,15);
            Random random = new Random();
            int[] searchValues = new int[3];

            for (int i = 0; i < searchValues.Length; i++)
            {
                searchValues[i] = random.Next(15);
            }

            FibonacciSearch a = new FibonacciSearch();
            Output(array.SortedArray);

            foreach (int item in searchValues)
            {
                int index = a.Search(array.SortedArray, item);
                Console.WriteLine("Searched value: {0}; Index: {1}", item, index);
            }

            Console.ReadKey();
        }

        static void Output(int[] array)
        {
            foreach (var item in array)
            {
                Console.Write(item.ToString() + " ");
            }

            Console.WriteLine();
        }
    }
}
