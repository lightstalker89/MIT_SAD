// *******************************************************
// * <copyright file="QueueManager.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Runtime.CompilerServices;
using BiOWheelsFileWatcher.Interfaces;

[assembly: InternalsVisibleTo("BiOWheelsFileWatcher.Test")]

namespace BiOWheelsFileWatcher
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
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
        ///  Represents an instance of the <see cref="FileSystemManager"/> class
        /// </summary>
        private IFileSystemManager fileSystemManager;

        /// <summary>
        /// A value indicating if an item can be added to the queue or not
        /// </summary>
        private bool canAddItemsToQueue;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueManager"/> class
        /// </summary>
        /// <param name="fileSystemManager">The file system manager</param>
        internal QueueManager(IFileSystemManager fileSystemManager)
        {
            this.SyncItemQueue = new ConcurrentQueue<SyncItem>();
            this.FileSystemManager = fileSystemManager;
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
        /// Gets or sets the <see cref="FileSystemManager"/> instance
        /// </summary>
        internal IFileSystemManager FileSystemManager
        {
            get
            {
                return this.fileSystemManager;
            }

            set
            {
                this.fileSystemManager = value;
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
        /// Called when an exception is caught.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="data">The <see cref="CaughtExceptionEventArgs"/> instance containing the event data.</param>
        protected void OnCaughtException(object sender, CaughtExceptionEventArgs data)
        {
            if (this.CaughtException != null)
            {
                this.CaughtException(this, data);
            }
        }

        /// <summary>
        /// Called when an item is finalized.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="data">The <see cref="ItemFinalizedEventArgs"/> instance containing the event data.</param>
        protected void OnItemFinalized(object sender, ItemFinalizedEventArgs data)
        {
            if (this.ItemFinalized != null)
            {
                this.ItemFinalized(this, data);
            }
        }

        #endregion

        /// <summary>
        /// Finalize Queue
        /// </summary>
        private void FinalizeQueue()
        {
            while (this.IsWorkerInProgress)
            {
                SyncItem item;

                if (this.SyncItemQueue.TryDequeue(out item) && this.CanAddItemsToQueue)
                {
                    try
                    {
                        switch (item.FileAction)
                        {
                            case FileAction.DELETE:
                                this.fileSystemManager.Delete(item);
                                break;

                            case FileAction.COPY:
                                this.FileSystemManager.Copy(item);
                                break;

                            case FileAction.DIFF:
                                this.FileSystemManager.DiffFile(item);
                                break;
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
            }
        }

        #endregion
    }
}