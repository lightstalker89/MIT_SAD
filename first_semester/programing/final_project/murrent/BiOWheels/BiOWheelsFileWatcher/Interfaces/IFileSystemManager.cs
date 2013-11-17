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
    /// </summary>
    public interface IFileSystemManager
    {
        /// <summary>
        /// </summary>
        /// <param name="item">
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
        /// </summary>
        /// <param name="item">
        /// </param>
        void CopyDirectory(SyncItem item);
    }
}