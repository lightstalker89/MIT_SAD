// *******************************************************
// * <copyright file="PerformanceMonitorFactory.cs" company="MDMCoWorks">
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
    /// The <see ref="PerformanceMonitorFactory"/> class and its interaction logic 
    /// </summary>
    public class PerformanceMonitorFactory
    {
        /// <summary>
        /// Creates the performance monitor.
        /// </summary>
        /// <returns>An instance of the <see cref="PerformanceMonitor"/> class</returns>
        public static IPerformanceMonitor CreatePerformanceMonitor()
        {
            return new PerformanceMonitor();
        }
    }
}