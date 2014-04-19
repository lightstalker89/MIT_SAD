// *******************************************************
// * <copyright file="Timing.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace StopwatchSample
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The <see ref="Timing" /> class its interaction logic
    /// </summary>
    public class Timing
    {
        /// <summary>
        /// </summary>
        private TimeSpan startTime;

        /// <summary>
        /// </summary>
        private TimeSpan duration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timing"/> class.
        /// </summary>
        public Timing()
        {
            this.startTime = new TimeSpan(0);
            this.duration = new TimeSpan(0);
        }

        /// <summary>
        /// Stops the timeer.
        /// </summary>
        public void StopTime()
        {
            this.duration = Process.GetCurrentProcess().Threads[0].UserProcessorTime.Subtract(startTime);
        }

        /// <summary>
        /// Starts the timeer.
        /// </summary>
        public void StartTime()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            this.startTime = Process.GetCurrentProcess().Threads[0].UserProcessorTime;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public TimeSpan Result()
        {
            return this.duration;
        }
    }
}