// *******************************************************
// * <copyright file="PerformanceMonitor.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsPerformanceMonitor
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// </summary>
    internal class PerformanceMonitor : IPerformanceMonitor
    {
        /// <summary>
        /// </summary>
        private readonly PerformanceCounter cpuCounter;

        /// <summary>
        /// </summary>
        private readonly PerformanceCounter ramCounter;

        /// <summary>
        /// </summary>
        internal PerformanceMonitor()
        {
            cpuCounter = new PerformanceCounter
                {
                   CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total" 
                };

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetCPUUsage()
        {
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            return Math.Round(cpuCounter.NextValue(), 1) + "% CPU usage";
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetRAMUsage()
        {
            return Math.Round(ramCounter.NextValue(), 1) + "MB free RAM";
        }
    }
}