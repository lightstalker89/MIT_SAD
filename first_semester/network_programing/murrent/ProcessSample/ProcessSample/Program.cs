// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace ProcessSample
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
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                Console.WriteLine("**************************");

                if (process.Id == 0)
                {
                    continue;
                }

                // DateTime startTime = process.StartTime;
                int id = process.Id;
                string processName = process.ProcessName;

                Console.WriteLine("Found process:");
                Console.WriteLine(processName + " started with id " + id);

                ProcessThreadCollection processThreadCollection = process.Threads;

                foreach (ProcessThread processThread in processThreadCollection)
                {
                    ThreadState threadState = processThread.ThreadState;

                    // TimeSpan totalProcessorTime = processThread.TotalProcessorTime;
                    int basePriority = processThread.BasePriority;

                    Console.WriteLine("Found Thread:");
                    Console.WriteLine("Thread state " + threadState + " with priority " + basePriority);
                }

                Process myProcess = Process.GetCurrentProcess();
                DateTime startedTime = myProcess.StartTime;

                Console.WriteLine("Started: " + startedTime.ToShortTimeString());

                Console.WriteLine("**************************");
            }
        }
    }
}