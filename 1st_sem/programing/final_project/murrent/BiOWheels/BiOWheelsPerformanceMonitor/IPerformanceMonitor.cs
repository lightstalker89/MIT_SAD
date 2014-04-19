// *******************************************************
// * <copyright file="IPerformanceMonitor.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsPerformanceMonitor
{
    /// <summary>
    ///  Interface for the <see cref="PerformanceMonitor"/> class
    /// </summary>
    public interface IPerformanceMonitor
    {
        /// <summary>
        /// Gets the CPU usage.
        /// </summary>
        /// <returns>The CPU usage as string</returns>
        string GetCPUUsage();

        /// <summary>
        /// Gets the RAM usage.
        /// </summary>
        /// <returns>The RAM usage as string</returns>
        string GetRAMUsage();
    }
}