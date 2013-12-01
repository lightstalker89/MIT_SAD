// *******************************************************
// * <copyright file="PerformanceMonitorTest.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsPerformanceMonitor.Test
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    using NUnit.Framework;

    /// <summary>
    /// Class representing the test for the <see cref="BiOWheelsPerformanceMonitor.PerformanceMonitor"/>
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", 
        Justification = "Reviewed.")]
    [TestFixture]
    public class PerformanceMonitorTest
    {
        /// <summary>
        /// </summary>
        private IPerformanceMonitor performanceMonitor;

        /// <summary>
        /// </summary>
        [SetUp]
        public void Init()
        {
            performanceMonitor = PerformanceMonitorFactory.CreatePerformanceMonitor();
        }

        /// <summary>
        /// </summary>
        [TestCase]
        public void MonitorTest()
        {
            while (true)
            {
                Debug.WriteLine("CPU Usage: " + performanceMonitor.GetCPUUsage());
                Debug.WriteLine("Free RAM: " + performanceMonitor.GetRAMUsage());
                Thread.Sleep(2000);
            }
        }
    }
}