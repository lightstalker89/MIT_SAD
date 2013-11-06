// *******************************************************
// * <copyright file="ThreadTestHelper.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BiOWheelsFileWatcher.Test")]
[assembly: InternalsVisibleTo("BiOWheelsLogger.Test")]

namespace BiOWheelsTestHelper
{
    using System.Threading;

    /// <summary>
    /// Representing a ThreadTestHelper class which holds a helper method for testing threaded methods
    /// </summary>
    public static class ThreadTestHelper
    {
        /// <summary>
        /// Delegate for the boolean value
        /// </summary>
        /// <returns>The boolean value to wait for</returns>
        internal delegate bool WaitCondition();

        /// <summary>
        /// Helper method for testing the logger background thread
        /// </summary>
        /// <param name="condition">
        /// Delegate for delivering the boolean value
        /// </param>
        /// <param name="totalTimeout">
        /// Expected time needed for the total progress
        /// </param>
        /// <param name="step">
        /// Time the thread should wait till thread is continued
        /// </param>
        internal static void WaitForCondition(WaitCondition condition, int totalTimeout, int step)
        {
            int currentTimeout = 0;

            while (condition() == false && currentTimeout < totalTimeout)
            {
                Thread.Sleep(step);
                currentTimeout += step;
            }
        }
    }
}