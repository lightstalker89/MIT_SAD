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
        /// Gets a value indicating whether [is syncing].
        /// </summary>
        /// <value>
        ///   <c>true</c> if it is syncing; otherwise, <c>false</c>.
        /// </value>
        bool IsSyncing { get; }

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

        /// <summary>
        /// Sets the parallel synchronize.
        /// </summary>
        /// <param name="parallelSync">if set to <c>true</c> parallel synchronize is activated.</param>
        void SetParallelSync(bool parallelSync);

        /// <summary>
        /// Sets the block compare size in mb.
        /// </summary>
        /// <param name="blockCompareSizeInMb">The block compare size in mb.</param>
        void SetBlockCompareSizeInMb(long blockCompareSizeInMb);

        /// <summary>
        /// Sets the block size in mb.
        /// </summary>
        /// <param name="blockSizeInMb">The block size in mb.</param>
        void SetBlockSizeInMb(long blockSizeInMb);

        /// <summary>
        /// Gets the the value if parallel synchronization is activated.
        /// </summary>
        /// <returns>The state if parallel synchronization is activated or not</returns>
        bool GetParallelSync();
    }
}
