// *******************************************************
// * <copyright file="IQueueManager.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    /// <summary>
    /// Interface representing must implement methods
    /// </summary>
    internal interface IQueueManager
    {
        /// <summary>
        /// Start the <see cref="QueueManager"/>
        /// </summary>
        void DoWork();

        /// <summary>
        /// Add an item to the queue
        /// </summary>
        /// <param name="item">
        /// Item which will be added to the queue
        /// </param>
        void Enqueue(SyncItem item);
    }
}