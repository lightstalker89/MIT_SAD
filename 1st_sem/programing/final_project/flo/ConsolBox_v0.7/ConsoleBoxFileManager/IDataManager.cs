// *******************************************************
// * <copyright file="IDataManager.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxDataManager
{
    using System;
    using System.Collections.Concurrent;
    using ConsoleBoxDataManager.Events;

    /// <summary>
    /// Interface representing methods and properties of the DataManager
    /// </summary>
    public interface IDataManager
    {
        /// <summary>
        /// Event for any exception.
        /// </summary>
        event EventHandler<ExceptionDataManagerEventArgs> ExceptionMessage;

        /// <summary>
        /// Gets or sets the queue items.
        /// </summary>
        ConcurrentQueue<QueueItem> QueueItems { get; set; }

        /// <summary>
        /// Initializes the data manager.
        /// </summary>
        /// <param name="blockCompareSize">Size of the block compare.</param>
        /// <param name="blockSize">Size of the block.</param>
        /// <param name="parallelSync">ParallelSync on or off.</param>
        void InitializeDataManager(long blockCompareSize, long blockSize, bool parallelSync);

        /// <summary>
        /// Adds the file manager job.
        /// </summary>
        /// <param name="item">The item.</param>
        void AddFileManagerJob(QueueItem item);
    }
}
