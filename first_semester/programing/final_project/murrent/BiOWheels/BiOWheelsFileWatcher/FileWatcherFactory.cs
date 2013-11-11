// *******************************************************
// * <copyright file="FileWatcherFactory.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System.Collections.Generic;

    using BiOWheelsFileWatcher.Interfaces;

    /// <summary>
    /// </summary>
    public class FileWatcherFactory
    {
        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <returns>
        /// </returns>
        public static BiOWheelsFileSystemWatcher CreateFileSystemWatcher(string path)
        {
            return new BiOWheelsFileSystemWatcher(path);
        }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        /// <param name="recursive">
        /// </param>
        /// <param name="destinationDirectories">
        /// </param>
        /// <param name="blockCompareFileSizeInMB">
        /// </param>
        /// <param name="excludedDirectories">
        /// </param>
        /// <returns>
        /// </returns>
        public static BiOWheelsFileSystemWatcher CreateFileSystemWatcher(
            string path, 
            bool recursive, 
            List<string> destinationDirectories, 
            long blockCompareFileSizeInMB, 
            List<string> excludedDirectories)
        {
            return new BiOWheelsFileSystemWatcher(path)
                {
                    IncludeSubdirectories = recursive, 
                    Destinations = destinationDirectories, 
                    BlockCompareFileSizeInMB = blockCompareFileSizeInMB, 
                    ExcludedDirectories = excludedDirectories
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="queueManager">
        /// </param>
        /// <returns>
        /// </returns>
        public static IFileWatcher CreateFileWatcher(IQueueManager queueManager)
        {
            return new FileWatcher(queueManager);
        }

        /// <summary>
        /// </summary>
        /// <param name="blockSize">
        /// </param>
        /// <returns>
        /// </returns>
        public static IFileComparator CreateFileComparator(long blockSize)
        {
            return new FileComparator(blockSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="fileComparator">
        /// </param>
        /// <returns>
        /// </returns>
        public static IFileSystemManager CreateFileSystemManager(IFileComparator fileComparator)
        {
            return new FileSystemManager(fileComparator);
        }

        /// <summary>
        /// </summary>
        /// <param name="fileSystemManager">
        /// </param>
        /// <returns>
        /// </returns>
        public static IQueueManager CreateQueueManager(IFileSystemManager fileSystemManager)
        {
            return new QueueManager(fileSystemManager);
        }
    }
}