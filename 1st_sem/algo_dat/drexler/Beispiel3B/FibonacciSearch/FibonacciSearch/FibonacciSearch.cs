//-----------------------------------------------------------------------
// <copyright file="FibonacciSearch.cs" company="MD Development">
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

    public class FibonacciSearch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="elem"></param>
        /// <returns></returns>
        public int Search(int[] arr, int elem)
        {
            return fibonacciSearchRec(arr, elem, 0, arr.Length - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="elem"></param>
        /// <param name="lowerBoundery"></param>
        /// <param name="upperBoundery"></param>
        /// <returns></returns>
        public int fibonacciSearchRec(int[] arr, int elem, int lowerBoundery, int upperBoundery)
        {
            int k = 0;

            // Get the maximum fibonacci number which is smaller then the array lenght of integers
            while (GetFibonacci(k + 1) < (upperBoundery - lowerBoundery))
            {
                k++;
            }

            // Check if searched element exists on the fibonacci index plus the lower boundery
            if (elem == arr[lowerBoundery + GetFibonacci(k)]) // terminate condition
            {
                return lowerBoundery + GetFibonacci(k);
            }

            // Element not available/ element doesn´t exist
            if (lowerBoundery == upperBoundery) // terminate condition
            {
                return -1;
            }

            // if element exists on the left side, calculate new upper boundery and new fibonacci number (middle)
            if (elem < arr[lowerBoundery + GetFibonacci(k)])
            {
                return fibonacciSearchRec(arr, elem, lowerBoundery, lowerBoundery + GetFibonacci(k) - 1);
            }

            // Element exists on the right side, calculate new lower boundery and new fibonacci number (middle)
            return fibonacciSearchRec(arr, elem, lowerBoundery + GetFibonacci(k) + 1, upperBoundery);
        }

        /// <summary>
        /// The fibonacci series
        /// </summary>
        /// <param name="size">The size of the fibonacci series</param>
        /// <returns>Fibonacci series</returns>
        public int[] GetFibonacciSeries(int size)
        {
            int[] fibonacciSeries = new int[size];

            int f0 = 0;
            int f1 = 1;
            int f2 = 1;

            for (int i = 0; i < fibonacciSeries.Length; ++i)
            {
                if (i == 0)
                {
                    fibonacciSeries[i] = 0;
                }
                else if (i == 1)
                {
                    fibonacciSeries[i] = 1;
                }
                else
                {
                    fibonacciSeries[i] = fibonacciSeries[i - 2] + fibonacciSeries[i - 1];
                }
            }

            return fibonacciSeries;
        }


        /// <summary>
        /// Get fibonacci number at passed index
        /// </summary>
        /// <param name="index">Index of the fibonacci number</param>
        /// <returns>Fibonacci number</returns>
        public int GetFibonacci(int index)
        {
            int[] fibonacciSeries = new int[index + 1];

            int f0 = 0;
            int f1, f2 = 1;

            for (int i = 0; i < fibonacciSeries.Length; ++i)
            {
                if (i == 0)
                {
                    fibonacciSeries[i] = 0;
                }
                else if (i == 1)
                {
                    fibonacciSeries[i] = 1;
                }
                else
                {
                    fibonacciSeries[i] =  fibonacciSeries[i - 2] + fibonacciSeries[i - 1];
                }
            }

            return fibonacciSeries[index];
        }
    }
}
