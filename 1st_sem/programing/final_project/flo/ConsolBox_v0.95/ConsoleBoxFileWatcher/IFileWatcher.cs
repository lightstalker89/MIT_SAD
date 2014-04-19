// *******************************************************
// * <copyright file="IFileWatcher.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxFileWatcher
{
    using System;
    using System.Collections.Generic;
    using ConsoleBoxFileWatcher.Events;

    /// <summary>
    /// Interface representing methods and properties of the FileWatcher
    /// </summary>
    public interface IFileWatcher
    {
        /// <summary>
        /// Event for any exception which occurs in the FileWatcher.
        /// </summary>
        event EventHandler<ExceptionFileWatcherEventArgs> ExceptionMessage;

        /// <summary>
        /// Event for the jobs which has been detected in the FileWatcher.
        /// </summary>
        event EventHandler<FileWatcherJobEventArgs> FileWatcherJob;

        /// <summary>
        /// Initializes the file watcher.
        /// </summary>
        /// <param name="sourceFolderInfo">The source folder information.</param>
        void InitializeFileWatcher(IList<SourceFolderInfo> sourceFolderInfo);

        /// <summary>
        /// Sets the file system watcher.
        /// </summary>
        void SetFileSystemWatcher();

        /// <summary>
        /// Scans the state of the folders for synchronization.
        /// </summary>
        void ScanFoldersForSyncState();
    }
}
