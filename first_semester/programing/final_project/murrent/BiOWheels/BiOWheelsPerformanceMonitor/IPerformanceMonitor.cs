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
    /// </summary>
    public interface IPerformanceMonitor
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        string GetCPUUsage();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        string GetRAMUsage();
    }
}