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
    /// </summary>
    public class PerformanceMonitorFactory
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static IPerformanceMonitor CreatePerformanceMonitor()
        {
            return new PerformanceMonitor();
        }
    }
}