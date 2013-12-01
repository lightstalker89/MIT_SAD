// *******************************************************
// * <copyright file="FileWatcher.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
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
    using System.Linq;
    using System.Threading;

    using BiOWheelsFileWatcher.CustomEventArgs;
    using BiOWheelsFileWatcher.CustomExceptions;
    using BiOWheelsFileWatcher.Interfaces;

    /// <summary>
    /// Class representing the <see cref="FileWatcher"/> and its interaction logic
    /// </summary>
    public class FileWatcher : IFileWatcher
    {
        #region Private Fields

        /// <summary>
        /// Manager for the queue
        /// </summary>
        private readonly IQueueManager queueManager;

        /// <summary>
        /// List of mappings representing <see cref="DirectoryMapping"/>
        /// </summary>
        private IEnumerable<DirectoryMapping> mappings;

        /// <summary>
        /// Field representing the status of the <see cref="FileWatcher"/>
        /// </summary>
        private bool isWorkerInProgress;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWatcher"/> class
        /// </summary>
        /// <param name="queueManager">
        /// The queue manager.
        /// </param>
        internal FileWatcher(IQueueManager queueManager)
        {
            this.queueManager = queueManager;
            this.QueueManager.CaughtException += this.QueueManagerCaughtException;
            this.QueueManager.ItemFinalized += this.QueueManagerItemFinalized;
        }

        #region Delegates

        /// <summary>
        /// Delegate for the <see cref="CaughtExceptionHandler"/> event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="data">Data from the event</param>
        public delegate void CaughtExceptionHandler(object sender, CaughtExceptionEventArgs data);

        /// <summary>
        /// Delegate for the <see cref="ProgressUpdateHandler"/> event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="data">Data from the event</param>
        public delegate void ProgressUpdateHandler(object sender, UpdateProgressEventArgs data);

        #endregion

        #region Event Handler

        /// <summary>
        /// Event handler for catching an exception
        /// </summary>
        public event CaughtExceptionHandler CaughtException;

        /// <summary>
        /// Event handler for updating the progress
        /// </summary>
        public event ProgressUpdateHandler ProgressUpdate;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="QueueManager"/>
        /// </summary>
        internal QueueManager QueueManager
        {
            get
            {
                return this.queueManager as QueueManager;
            }
        }

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

        #endregion

        #region Methods

        /// <inheritdoc/>
        public void Init()
        {
            this.IsWorkerInProgress = true;

            foreach (DirectoryMapping mapping in this.Mappings)
            {
                Thread backgroundWatcherThread = new Thread(this.WatchDirectory);
                backgroundWatcherThread.Start(mapping);
            }

            this.QueueManager.DoWork();
        }

        /// <inheritdoc/>
        public void InitialScan()
        {
            // TODO: check directories and add them to the queue
            foreach (DirectoryMapping mapping in this.Mappings)
            {
            }

            this.Init();
        }

        /// <inheritdoc/>
        public void SetSourceDirectories(IEnumerable<DirectoryMapping> directoryMappings)
        {
            this.Mappings = directoryMappings;
        }

        /// <summary>
        /// Enumerate all files in the given directory including subdirectories
        /// </summary>
        /// <param name="directoryName">
        /// Directory name
        /// </param>
        /// <returns>
        /// A list of all file names
        /// </returns>
        internal IEnumerable<string> GetFilesForDirectory(string directoryName)
        {
            try
            {
                return Directory.GetFiles(directoryName, "*.*", SearchOption.AllDirectories).ToList();
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                this.CaughtException(
                    this, 
                    new CaughtExceptionEventArgs(
                        unauthorizedAccessException.GetType(), unauthorizedAccessException.Message)
                        {
                           CustomExceptionText = "Error while enumerating all files in the given directory" 
                        });

                return new List<string>();
            }
        }

        #region Event Methods

        /// <summary>
        /// Occurs when the instance of FileSystemWatcher is unable to continue monitoring changes or when the internal buffer overflows.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="ErrorEventArgs"/> instance containing the event data.
        /// </param>
        protected void FileSystemWatcherError(object sender, ErrorEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {
                this.OnCaughtException(
                    this, new CaughtExceptionEventArgs(e.GetException().GetType(), e.GetException().Message));
            }
        }

        /// <summary>
        /// Occurs when the file system watcher is disposed
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        protected void FileSystemWatcherDisposed(object sender, EventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {
            }
        }

        /// <summary>
        /// Occurs when the component is disposed by a call to the Dispose method. (Inherited from Component.)
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="data">
        /// The <see cref="UpdateProgressEventArgs"/> instance containing the event data.
        /// </param>
        protected void OnProgressUpdate(object sender, UpdateProgressEventArgs data)
        {
            if (this.ProgressUpdate != null)
            {
                this.ProgressUpdate(this, data);
            }
        }

        /// <summary>
        /// Occurs when an exception is caught
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="data">
        /// The <see cref="CaughtExceptionEventArgs"/> instance containing the event data.
        /// </param>
        protected void OnCaughtException(object sender, CaughtExceptionEventArgs data)
        {
            if (this.CaughtException != null)
            {
                this.CaughtException(this, data);
            }
        }

        /// <summary>
        /// Call when an exception is caught.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The <see cref="CaughtExceptionEventArgs"/> instance containing the event data.
        /// </param>
        protected void QueueManagerCaughtException(object sender, CaughtExceptionEventArgs data)
        {
            this.CaughtException(this, data);
        }

        /// <summary>
        /// Call when an item is finalized.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The <see cref="ItemFinalizedEventArgs"/> instance containing the event data.
        /// </param>
        protected void QueueManagerItemFinalized(object sender, ItemFinalizedEventArgs data)
        {
        }

        /// <summary>
        /// Occurs when a file or directory in the specified Path is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="CustomFileSystemEventArgs"/> instance containing the event data.
        /// </param>
        protected void FileSystemWatcherObjectChanged(object sender, CustomFileSystemEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {
                if (this.IsAllowedToAddItemToQueue(e.FullQualifiedFileName, watcher.ExcludedDirectories))
                {
                    this.AddQueueItem(
                        watcher.Destinations, e.FileName, e.FullQualifiedFileName, string.Empty, FileAction.COPY);
                    this.OnProgressUpdate(this, new UpdateProgressEventArgs("File --" + e.FileName + "-- has changed."));
                    this.OnProgressUpdate(
                        this, new UpdateProgressEventArgs("Added job to queue for comparing --" + e.FileName + "--"));
                }
            }
        }

        /// <summary>
        /// Event that occurs when a file or directory has been renamed
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="CustomFileRenamedEventArgs"/> instance containing the event data.
        /// </param>
        protected void FileSystemWatcherObjectRenamed(object sender, CustomFileRenamedEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {
                if (this.IsAllowedToAddItemToQueue(e.FullQualifiedFileName, watcher.ExcludedDirectories))
                {
                    this.AddQueueItem(
                        watcher.Destinations, e.FileName, e.FullQualifiedFileName, e.OldFileName, FileAction.COPY);
                    this.OnProgressUpdate(
                        this, 
                        new UpdateProgressEventArgs("File --" + e.OldFileName + " has been renamed to --" + e.FileName));
                    this.OnProgressUpdate(
                        this, 
                        new UpdateProgressEventArgs(
                            "Added job to queue for renaming --" + e.OldFileName + "-- to --" + e.FileName + "--"));
                }
            }
        }

        /// <summary>
        /// Event that occurs when a file or directory has been deleted
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="CustomFileSystemEventArgs"/> instance containing the event data.
        /// </param>
        protected void FileSystemWatcherObjectDeleted(object sender, CustomFileSystemEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {
                if (this.IsAllowedToAddItemToQueue(e.FullQualifiedFileName, watcher.ExcludedDirectories))
                {
                    this.AddQueueItem(
                        watcher.Destinations, e.FileName, e.FullQualifiedFileName, string.Empty, FileAction.DELETE);
                    this.OnProgressUpdate(
                        this, new UpdateProgressEventArgs("File --" + e.FileName + "-- has been deleted."));
                    this.OnProgressUpdate(
                        this, new UpdateProgressEventArgs("Added job to queue for deleting --" + e.FileName + "--"));
                }
            }
        }

        /// <summary>
        /// Event that occurs when a file or directory has been created
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="CustomFileSystemEventArgs"/> instance containing the event data.
        /// </param>
        protected void FileSystemWatcherObjectCreated(object sender, CustomFileSystemEventArgs e)
        {
            BiOWheelsFileSystemWatcher watcher = this.GetFileSystemWatcher(sender);

            if (watcher != null)
            {
                if (this.IsAllowedToAddItemToQueue(e.FullQualifiedFileName, watcher.ExcludedDirectories))
                {
                    this.AddQueueItem(
                        watcher.Destinations, e.FileName, e.FullQualifiedFileName, string.Empty, FileAction.COPY);
                    this.OnProgressUpdate(
                        this, new UpdateProgressEventArgs("File --" + e.FileName + "-- has been created."));
                    this.OnProgressUpdate(
                        this, new UpdateProgressEventArgs("Added job to queue for copying --" + e.FileName + "--"));
                }
            }
        }

        #endregion

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
                    try
                    {
                        BiOWheelsFileSystemWatcher fileSystemWatcher =
                            FileWatcherFactory.CreateFileSystemWatcher(
                                ((DirectoryMapping)mappingInfo).SourceDirectory, 
                                ((DirectoryMapping)mappingInfo).Recursive, 
                                ((DirectoryMapping)mappingInfo).DestinationDirectories, 
                                ((DirectoryMapping)mappingInfo).ExcludedDirectories);

                        fileSystemWatcher.Error += this.FileSystemWatcherError;
                        fileSystemWatcher.Disposed += this.FileSystemWatcherDisposed;
                        fileSystemWatcher.ObjectCreated += this.FileSystemWatcherObjectCreated;
                        fileSystemWatcher.ObjectDeleted += this.FileSystemWatcherObjectDeleted;
                        fileSystemWatcher.ObjectRenamed += this.FileSystemWatcherObjectRenamed;
                        fileSystemWatcher.ObjectChanged += this.FileSystemWatcherObjectChanged;
                        fileSystemWatcher.Filter = string.Empty;
                        fileSystemWatcher.InternalBufferSize = 64000;
                        fileSystemWatcher.EnableRaisingEvents = true;
                    }
                    catch (PathTooLongException pathTooLongException)
                    {
                        this.OnCaughtException(
                            this, 
                            new CaughtExceptionEventArgs(pathTooLongException.GetType(), pathTooLongException.Message));
                    }
                    catch (ArgumentException argumentException)
                    {
                        this.OnCaughtException(
                            this, new CaughtExceptionEventArgs(argumentException.GetType(), argumentException.Message));
                    }
                }
                else
                {
                    this.OnCaughtException(
                        this, 
                        new CaughtExceptionEventArgs(typeof(MappingInvalidException), "Mapping information is invalid"));
                }
            }
            else
            {
                this.OnCaughtException(
                    this, new CaughtExceptionEventArgs(typeof(MappingNullException), "Mapping information is null"));
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

        /// <summary>
        /// Adds an item to the queue.
        /// </summary>
        /// <param name="destinations">
        /// The destinations.
        /// </param>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="fullQualifiedFileName">
        /// Full name of the qualified file.
        /// </param>
        /// <param name="oldFileName">
        /// </param>
        /// <param name="fileAction">
        /// The file action.
        /// </param>
        private void AddQueueItem(
            IEnumerable<string> destinations, 
            string filename, 
            string fullQualifiedFileName, 
            string oldFileName, 
            FileAction fileAction)
        {
            SyncItem item = new SyncItem(destinations, filename, fullQualifiedFileName, oldFileName, fileAction);

            this.QueueManager.Enqueue(item);
        }

        /// <summary>
        /// Check if a file or directory can be added to the queue
        /// </summary>
        /// <param name="objectName">
        /// Name of the object.
        /// </param>
        /// <param name="excludedDirectories">
        /// The excluded directories.
        /// </param>
        /// <returns>
        /// A value indicating whether the item is allowed to be added to the queue or not
        /// </returns>
        private bool IsAllowedToAddItemToQueue(string objectName, IEnumerable<string> excludedDirectories)
        {
            bool isAllowedToAdd = true;

            if (excludedDirectories != null)
            {
                if (objectName.IsDirectory())
                {
                    string directoryName = objectName.Split(Path.DirectorySeparatorChar).Last();

                    if (excludedDirectories.Any(name => name.Equals(directoryName)))
                    {
                        isAllowedToAdd = false;
                    }
                }
                else
                {
                    string directoryName = Path.GetDirectoryName(objectName);

                    if (directoryName != null)
                    {
                        IEnumerable<string> directories = directoryName.Split(Path.DirectorySeparatorChar).ToList();

                        foreach (string directory in
                            directories.Where(directory => excludedDirectories.Any(directory.Equals)))
                        {
                            isAllowedToAdd = false;
                        }
                    }
                }
            }

            return isAllowedToAdd;
        }

        #endregion
    }
}