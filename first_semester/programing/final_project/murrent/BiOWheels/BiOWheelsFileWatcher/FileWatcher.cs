// *******************************************************
// * <copyright file="FileWatcher.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BiOWheelsFileWatcher.Test")]

namespace BiOWheelsFileWatcher
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// Class representing the <see cref="FileWatcher"/> and its interaction logic
    /// </summary>
    public class FileWatcher : IFileWatcher
    {
        /// <summary>
        /// List of mappings representing <see cref="DirectoryMapping"/>
        /// </summary>
        private IEnumerable<DirectoryMapping> mappings;

        /// <summary>
        /// Field representing the status of the <see cref="FileWatcher"/>
        /// </summary>
        private bool isWorkerInProgress;

        /// <summary>
        /// Gets or sets the list of mappings
        /// </summary>
        internal IEnumerable<DirectoryMapping> Mappings
        {
            get
            {
                return this.mappings;
            }

            set
            {
                this.mappings = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="FileWatcher"/> is in progress or not
        /// </summary>
        internal bool IsWorkerInProgress
        {
            get
            {
                return this.isWorkerInProgress;
            }

            private set
            {
                this.isWorkerInProgress = value;
            }
        }

        #region Methods

        /// <inheritdoc/>
        public void Init()
        {
            this.Mappings = new List<DirectoryMapping>();
            this.IsWorkerInProgress = true;

            foreach (DirectoryMapping mapping in this.Mappings)
            {
                Thread backgroundWatcherThread = new Thread(this.WatchDirectory);
                backgroundWatcherThread.Start(mapping);
            }
        }

        /// <inheritdoc/>
        public void SetSourceDirectories(IEnumerable<DirectoryMapping> directoryMappings)
        {
            this.mappings = directoryMappings;
        }

        /// <summary>
        /// Method for watching a specific directory - will be executed in a new thread
        /// </summary>
        /// <param name="mappingInfo">
        /// Object containing the <see cref="DirectoryMapping"/> information
        /// </param>
        private void WatchDirectory(object mappingInfo)
        {
            if (mappingInfo != null)
            {
                if (mappingInfo.GetType() == typeof(DirectoryMapping))
                {
                    BiOWheelsFileSystemWatcher fileSystemWatcher =
                        new BiOWheelsFileSystemWatcher(((DirectoryMapping)mappingInfo).SorceDirectory)
                            {
                               IncludeSubdirectories = ((DirectoryMapping)mappingInfo).Recursive, 
                            };
                    fileSystemWatcher.Changed += this.FileSystemWatcherChanged;
                    fileSystemWatcher.Created += this.FileSystemWatcherCreated;
                    fileSystemWatcher.Deleted += this.FileSystemWatcherDeleted;
                    fileSystemWatcher.Error += this.FileSystemWatcherError;
                    fileSystemWatcher.Renamed += this.FileSystemWatcherRenamed;
                    fileSystemWatcher.Disposed += this.FileSystemWatcherDisposed;
                    fileSystemWatcher.EnableRaisingEvents = true;
                }
                else
                {
                    // TODO: Custom error event implementation
                }
            }
            else
            {
                // TODO: Custom error event implementation
            }
        }

        /// <summary>
        /// Get the <see cref="BiOWheelsFileSystemWatcher"/> from the object
        /// </summary>
        /// <param name="sender">
        /// Object from the event
        /// </param>
        /// <returns>
        /// The instance of the <see cref="BiOWheelsFileSystemWatcher"/>
        /// </returns>
        private BiOWheelsFileSystemWatcher GetFileSystemWatcher(object sender)
        {
            BiOWheelsFileSystemWatcher watcher = sender as BiOWheelsFileSystemWatcher;

            if (watcher != null)
            {
                return watcher;
            }

            return null;
        }

        #endregion

        #region Events

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void FileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if(watcher != null)
            {
                
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void FileSystemWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void FileSystemWatcherCreated(object sender, FileSystemEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void FileSystemWatcherDisposed(object sender, EventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void FileSystemWatcherRenamed(object sender, RenamedEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void FileSystemWatcherError(object sender, ErrorEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {

            }
        }

        #endregion
    }
}