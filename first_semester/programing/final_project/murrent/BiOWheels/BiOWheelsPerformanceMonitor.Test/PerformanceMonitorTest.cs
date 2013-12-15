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
        /// The performance monitor
        /// </summary>
        private IPerformanceMonitor performanceMonitor;

        /// <summary>
        /// Sets up the test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.performanceMonitor = PerformanceMonitorFactory.CreatePerformanceMonitor();
        }

        /// <summary>
        /// Testing the <see cref="PerformanceMonitor"/> class methods
        /// </summary>
        [TestCase]
        public void MonitorTest()
        {
            while (true)
            {
                string cpuUsage = this.performanceMonitor.GetCPUUsage();
                string ramUsage = this.performanceMonitor.GetRAMUsage();
                Debug.WriteLine("CPU Usage: " + cpuUsage);
                Debug.WriteLine("Free RAM: " + ramUsage);
                Thread.Sleep(2000);

                Assert.IsNotNullOrEmpty(cpuUsage);
                Assert.IsNotNullOrEmpty(ramUsage);
            }
        }
    }
}