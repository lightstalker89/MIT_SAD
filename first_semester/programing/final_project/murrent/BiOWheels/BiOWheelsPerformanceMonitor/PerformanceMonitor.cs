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
    /// The <see ref="PerformanceMonitor"/> class and its interaction logic 
    /// </summary>
    internal class PerformanceMonitor : IPerformanceMonitor
    {
        /// <summary>
        /// Holds the instance of the CPU monitor
        /// </summary>
        private readonly PerformanceCounter cpuCounter;

        /// <summary>
        /// Holds the instance of the Ram monitor
        /// </summary>
        private readonly PerformanceCounter ramCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceMonitor"/> class.
        /// </summary>
        internal PerformanceMonitor()
        {
            this.cpuCounter = new PerformanceCounter
                {
                   CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total" 
                };

            this.ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        /// <inheritdoc/>
        public string GetCPUUsage()
        {
            this.cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            return Math.Round(this.cpuCounter.NextValue(), 1) + "% CPU usage";
        }

        /// <inheritdoc/>
        public string GetRAMUsage()
        {
            return Math.Round(this.ramCounter.NextValue(), 1) + "MB free RAM";
        }
    }
}