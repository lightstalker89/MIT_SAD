// *******************************************************
// * <copyright file="QueueManager.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BiOWheelsFileWatcher.CustomEventArgs;

    /// <summary>
    /// </summary>
    internal class QueueManager : IQueueManager
    {
        #region Private Fields

        /// <summary>
        /// 
        /// </summary>
        private ConcurrentQueue<SyncItem> syncItemQueue;

        /// <summary>
        /// 
        /// </summary>
        private bool isWorkerInProgress;

        /// <summary>
        /// 
        /// </summary>
        private FileComparator fileComparator;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueManager"/> class
        /// </summary>
        /// <param name="fileComparator">
        /// </param>
        internal QueueManager(FileComparator fileComparator)
        {
            this.syncItemQueue = new ConcurrentQueue<SyncItem>();
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

        /// <summary>
        /// Gets or sets a value indicating whether the worker is in progress or not
        /// </summary>
        public bool IsWorkerInProgress
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
        private async void CopyFile(SyncItem item)
        {
            // TODO: Parallel Sync implement

            foreach (string destinationFile in
                item.Destinations.Select(
                    destination => destination + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile)))
            {
                string fileToCopy = destinationFile;

                await Task.Run(() => File.Copy(item.SourceFile, fileToCopy, true));
            }
        }

        /// <summary>
        /// Deletes a file in all given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        private async void DeleteFile(SyncItem item)
        {
            foreach (string destination in item.Destinations)
            {
                string fileDestination = destination;

                await
                    Task.Run(
                        () =>
                        File.Delete(fileDestination + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile)));
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
        private async void FinalizeQueue()
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
                                await Task.Run(() => this.DeleteFile(item));
                            }
                            else
                            {
                                await Task.Run(() => this.CopyFile(item));
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

                        break;
                    }
                    else if (item.FileAction == FileAction.DIFF)
                    {
                        await Task.Run(() => this.DiffFile(item));
                    }
                }
            }
        }

        #endregion
    }
}