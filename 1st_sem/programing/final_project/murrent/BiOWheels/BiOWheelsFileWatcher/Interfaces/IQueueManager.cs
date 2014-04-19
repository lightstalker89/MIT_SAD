// *******************************************************
// * <copyright file="IQueueManager.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.Interfaces
{
    /// <summary>
    /// Interface representing must implement methods
    /// </summary>
    public interface IQueueManager
    {
        /// <summary>
        /// Event handler for catching an exception
        /// </summary>
        event QueueManager.CaughtExceptionHandler CaughtException;

        /// <summary>
        /// Event handler for catching an exception
        /// </summary>
        event QueueManager.ItemFinalizedHandler ItemFinalized;

        /// <summary>
        /// Event handler for an item that has been added to the wait queue
        /// </summary>
        event QueueManager.ItemAddedToWaitQueueHandler ItemAddedToWaitQueue;

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