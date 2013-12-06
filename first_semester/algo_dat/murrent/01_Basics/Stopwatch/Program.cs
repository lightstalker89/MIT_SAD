// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace StopwatchSample
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            int[] nums = new int[100000];
            BuildArray(nums);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            DisplayNums(nums);
            sw.Stop();

            Timing timing = new Timing();
            timing.StartTime();
            DisplayNums(nums);
            timing.StopTime();

            Console.WriteLine(string.Empty);
            Console.WriteLine(
                "Time Stopwatch: " + sw.Elapsed + " msecs " + sw.ElapsedMilliseconds + " ticks " + sw.ElapsedTicks);
            Console.WriteLine("Time Stopwatch: " + timing.Result().ToString());

            Console.ReadKey();
        }

        /// <summary>
        /// </summary>
        /// <param name="arr">
        /// </param>
        private static void BuildArray(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="arr">
        /// </param>
        private static void DisplayNums(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
        }
    }
}