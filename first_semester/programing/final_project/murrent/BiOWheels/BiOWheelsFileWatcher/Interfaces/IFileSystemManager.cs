// *******************************************************
// * <copyright file="IFileSystemManager.cs" company="MDMCoWorks">
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
    ///  Interface representing the <see cref="IFileSystemManager"/>
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether parallel sync is activated or not
        /// </summary>
        bool IsParallelSyncActivated { get; set; }

        /// <summary>
        /// Gets or sets the block size in MB
        /// </summary>
        long BlockCompareFileSizeInMB { get; set; }

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        void Delete(SyncItem item);

        /// <summary>
        /// Copies a file to the given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        void Copy(SyncItem item);

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        void CopyDirectory(SyncItem item);
    }
}