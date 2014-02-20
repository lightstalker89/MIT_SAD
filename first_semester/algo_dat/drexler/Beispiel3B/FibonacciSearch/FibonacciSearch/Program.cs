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
            FibonacciSearch a = new FibonacciSearch();
            Output(array.SortedArray);
            int searchedValue = a.Search(array.SortedArray, 5);
            Console.WriteLine("Searched value: {0}", searchedValue);
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
