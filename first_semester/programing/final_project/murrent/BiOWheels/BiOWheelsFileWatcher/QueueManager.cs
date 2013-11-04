// *******************************************************
// * <copyright file="QueueManager.cs" company="MDMCoWorks">
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
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using BiOWheelsFileWatcher.CustomEventArgs;

    /// <summary>
    /// Class representing the <see cref="QueueManager"/> and its interaction logic
    /// </summary>
    internal class QueueManager : IQueueManager
    {
        #region Private Fields

        /// <summary>
        /// Represents an instance of the queue holding <see cref="SyncItem"/> objects
        /// </summary>
        private ConcurrentQueue<SyncItem> syncItemQueue;

        /// <summary>
        /// A value indicating whether the worker is in progress or not
        /// </summary>
        private bool isWorkerInProgress;

        /// <summary>
        /// Represents an instance of the <see cref="FileComparator"/> class
        /// </summary>
        private FileComparator fileComparator;

        /// <summary>
        /// A value indicating if an item can be added to the queue or not
        /// </summary>
        private bool canAddItemsToQueue;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueManager"/> class
        /// </summary>
        /// <param name="fileComparator">
        /// </param>
        internal QueueManager(FileComparator fileComparator)
        {
            this.SyncItemQueue = new ConcurrentQueue<SyncItem>();
            this.FileComparator = fileComparator;
        }

        #region Delegates

        /// <summary>
        /// Delegate for the <see cref="CaughtExceptionHandler"/> event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="data">Data from the event</param>
        public delegate void CaughtExceptionHandler(object sender, CaughtExceptionEventArgs data);

        /// <summary>
        /// Delegate for the <see cref="ItemFinalizedHandler"/> event
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="data">Data from the event</param>
        public delegate void ItemFinalizedHandler(object sender, ItemFinalizedEventArgs data);

        #endregion

        #region Event Handler

        /// <summary>
        /// Event handler for catching an exception
        /// </summary>
        public event CaughtExceptionHandler CaughtException;

        /// <summary>
        /// Event handler for catching an exception
        /// </summary>
        public event ItemFinalizedHandler ItemFinalized;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public bool CanAddItemsToQueue
        {
            get
            {
                return this.canAddItemsToQueue;
            }

            set
            {
                this.canAddItemsToQueue = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the worker is in progress or not
        /// </summary>
        internal bool IsWorkerInProgress
        {
            get
            {
                return this.isWorkerInProgress;
            }

            set
            {
                this.isWorkerInProgress = value;
            }
        }

        /// <summary>
        /// Gets or sets the queue
        /// </summary>
        public ConcurrentQueue<SyncItem> SyncItemQueue
        {
            get
            {
                return this.syncItemQueue;
            }

            set
            {
                this.syncItemQueue = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="FileComparator"/> instance
        /// </summary>
        internal FileComparator FileComparator
        {
            get
            {
                return this.fileComparator;
            }

            set
            {
                this.fileComparator = value;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public void DoWork()
        {
            this.IsWorkerInProgress = true;

            Thread workerThread = new Thread(this.FinalizeQueue) { IsBackground = true };
            workerThread.Start();
        }

        /// <inheritdoc/>
        public void Enqueue(SyncItem item)
        {
            this.SyncItemQueue.Enqueue(item);
        }

        #region Event Methods

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="data">
        /// </param>
        protected void OnCaughtException(object sender, CaughtExceptionEventArgs data)
        {
            if (this.CaughtException != null)
            {
                this.CaughtException(this, data);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="data">
        /// </param>
        protected void OnItemFinalized(object sender, ItemFinalizedEventArgs data)
        {
            if (this.ItemFinalized != null)
            {
                this.ItemFinalized(this, data);
            }
        }

        #endregion

        /// <summary>
        /// Copies a file to the given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        private void CopyFile(SyncItem item)
        {
            // TODO: Parallel Sync implement

            foreach (string destination in item.Destinations)
            {
                this.CreateDirectoryIfNotExists(destination);

                string pathToCopy = Path.GetDirectoryName(destination + Path.DirectorySeparatorChar + item.SourceFile);

                this.CreateDirectoryIfNotExists(pathToCopy);

                string fileToCopy = pathToCopy + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile);

                File.Copy(item.FullQualifiedSourceFileName, fileToCopy, true);
            }
        }

        /// <summary>
        /// Deletes a file in all given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        private void DeleteFile(SyncItem item)
        {
            foreach (string destination in item.Destinations)
            {
                string pathToDelete = Path.GetDirectoryName(destination + Path.DirectorySeparatorChar + item.SourceFile);

                if (pathToDelete != null && Directory.Exists(pathToDelete))
                {
                    File.Delete(pathToDelete + Path.DirectorySeparatorChar +
                                            Path.GetFileName(item.SourceFile));
                }
            }
        }

        /// <summary>
        /// Compare files from a destination with files in all given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        private void DiffFile(SyncItem item)
        {
            foreach (string destinationFile in
                item.Destinations.Select(
                    destination => destination + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile)))
            {
            }
        }

        /// <summary>
        /// Finalize Queue
        /// </summary>
        private void FinalizeQueue()
        {
            while (this.IsWorkerInProgress)
            {
                SyncItem item;

                if (this.SyncItemQueue.TryDequeue(out item))
                {
                    if (item.FileAction == FileAction.COPY || item.FileAction == FileAction.DELETE)
                    {
                        try
                        {
                            if (item.FileAction == FileAction.DELETE)
                            {
                                this.DeleteFile(item);
                            }
                            else
                            {
                                this.CopyFile(item);
                            }
                        }
                        catch (UnauthorizedAccessException unauthorizedAccessException)
                        {
                            this.OnCaughtException(
                                this,
                                new CaughtExceptionEventArgs(
                                    unauthorizedAccessException.GetType(), unauthorizedAccessException.Message));
                        }
                        catch (ArgumentException argumentException)
                        {
                            this.OnCaughtException(
                                this,
                                new CaughtExceptionEventArgs(argumentException.GetType(), argumentException.Message));
                        }
                        catch (PathTooLongException pathTooLongException)
                        {
                            this.OnCaughtException(
                                this,
                                new CaughtExceptionEventArgs(
                                    pathTooLongException.GetType(), pathTooLongException.Message));
                        }
                        catch (DirectoryNotFoundException directoryNotFoundException)
                        {
                            this.OnCaughtException(
                                this,
                                new CaughtExceptionEventArgs(
                                    directoryNotFoundException.GetType(), directoryNotFoundException.Message));
                        }
                        catch (FileNotFoundException fileNotFoundException)
                        {
                            this.OnCaughtException(
                                this,
                                new CaughtExceptionEventArgs(
                                    fileNotFoundException.GetType(), fileNotFoundException.Message));
                        }
                        catch (IOException systemIOException)
                        {
                            this.OnCaughtException(
                                this,
                                new CaughtExceptionEventArgs(systemIOException.GetType(), systemIOException.Message));
                        }
                        catch (NotSupportedException notSupportedException)
                        {
                            this.OnCaughtException(
                                this,
                                new CaughtExceptionEventArgs(
                                    notSupportedException.GetType(), notSupportedException.Message));
                        }
                    }
                    else if (item.FileAction == FileAction.DIFF)
                    {
                        this.DiffFile(item);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a directory if it does not exist
        /// </summary>
        /// <param name="directory"></param>
        private void CreateDirectoryIfNotExists(string directory)
        {
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        #endregion
    }
}