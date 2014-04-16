// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace ThreadingExample
{
    using System;
    using System.Threading;

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
            Thread myThread = new Thread(StartThread);
            myThread.Start();

            Thread mySecondThread = new Thread(StartSecondThread);
            mySecondThread.Start();
        }

        /// <summary>
        /// </summary>
        private static void StartSecondThread()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(i + " B");
            }
        }

        /// <summary>
        /// </summary>
        private static void StartThread()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(i + " A");
            }
        }
    }
}