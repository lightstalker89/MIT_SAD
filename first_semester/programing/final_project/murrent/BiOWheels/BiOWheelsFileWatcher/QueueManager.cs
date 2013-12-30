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

[assembly: InternalsVisibleTo("BiOWheelsFileWatcher.Test")]

namespace BiOWheelsFileWatcher
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;

    using BiOWheelsFileHandleWrapper;
    using BiOWheelsFileHandleWrapper.CustomEventArgs;

    using BiOWheelsFileWatcher.CustomEventArgs;
    using BiOWheelsFileWatcher.Enums;
    using BiOWheelsFileWatcher.Helper;
    using BiOWheelsFileWatcher.Interfaces;

    using Timer = System.Timers.Timer;

    /// <summary>
    /// Class representing the <see cref="QueueManager"/> and its interaction logic
    /// </summary>
    public class QueueManager : IQueueManager
    {
        #region Private Fields

        /// <summary>
        /// Object used for locking
        /// </summary>
        private readonly object enqueueLockOject = new object();

        /// <summary>
        /// Object used for locking
        /// </summary>
        private readonly object queueItemLockObject = new object();

        /// <summary>
        /// Represents an instance of the queue holding <see cref="SyncItem"/> objects
        /// </summary>
        private ConcurrentQueue<SyncItem> syncItemQueue;

        /// <summary>
        /// Represents an instance of the queue holding <see cref="SyncItem"/> objects
        /// </summary>
        private ConcurrentQueue<SyncItem> syncItemWaitQueue;

        /// <summary>
        /// A value indicating whether the worker is in progress or not
        /// </summary>
        private bool isWorkerInProgress;

        /// <summary>
        ///  Represents an instance of the <see cref="FileSystemManager"/> class
        /// </summary>
        private IFileSystemManager fileSystemManager;

        /// <summary>
        /// The file handle wrapper
        /// </summary>
        private IFileHandleWrapper fileHandleWrapper;

        /// <summary>
        /// A value indicating if an item can be added to the queue or not
        /// </summary>
        private bool canDequeueItems;

        /// <summary>
        /// A value indicating whether an item can be taken from the wait queue or not
        /// </summary>
        private bool canDequeueWaitQueue;

        /// <summary>
        /// The maximum sync item retries
        /// </summary>
        private int maxSyncItemRetries;

        /// <summary>
        /// The actual <see cref="SyncItem"/> object
        /// </summary>
        private SyncItem actualSyncItem;

        /// <summary>
        /// The sync item wait queue timer
        /// </summary>
        private Timer syncItemWaitQueueTimer;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueManager"/> class
        /// </summary>
        /// <param name="fileSystemManager">
        /// The file system manager
        /// </param>
        /// <param name="fileHandleWrapper">
        /// The file handle wrapper.
        /// </param>
        internal QueueManager(IFileSystemManager fileSystemManager, IFileHandleWrapper fileHandleWrapper)
        {
            this.syncItemWaitQueueTimer = new System.Timers.Timer(120000);
            this.SyncItemWaitQueueTimer.Elapsed += this.SyncItemWaitQueueTimerElapsed;
            this.SyncItemWaitQueueTimer.Start();
            this.SyncItemWaitQueue = new ConcurrentQueue<SyncItem>();
            this.SyncItemQueue = new ConcurrentQueue<SyncItem>();
            this.FileSystemManager = fileSystemManager;
            this.CanDequeueItems = true;
            this.CanDequeueWaitQueue = false;
            this.MaxSyncItemRetries = 15;
            this.FileHandleWrapper = fileHandleWrapper;
            this.FileHandleWrapper.FileHandlesFound += this.FileHandleWrapperFileHandlesFound;
            this.FileHandleWrapper.FileHandlesError += this.FileHandleWrapperFileHandlesError;
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

        /// <summary>
        /// Delegate for the <see cref="ItemAddedToWaitQueueHandler"/> event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="data">The <see cref="UpdateProgressEventArgs"/> instance containing the event data.</param>
        public delegate void ItemAddedToWaitQueueHandler(object sender, UpdateProgressEventArgs data);

        #endregion

        #region Event Handler

        /// <inheritdoc/>
        public event CaughtExceptionHandler CaughtException;

        /// <inheritdoc/>
        public event ItemFinalizedHandler ItemFinalized;

        /// <inheritdoc/>
        public event ItemAddedToWaitQueueHandler ItemAddedToWaitQueue;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public bool CanDequeueItems
        {
            get
            {
                return this.canDequeueItems;
            }

            set
            {
                this.canDequeueItems = value;
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
        /// Gets or sets the sync item wait queue.
        /// </summary>
        /// <value>
        /// The synchronize item wait queue.
        /// </value>
        public ConcurrentQueue<SyncItem> SyncItemWaitQueue
        {
            get
            {
                return this.syncItemWaitQueue;
            }

            set
            {
                this.syncItemWaitQueue = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum sync item retries.
        /// </summary>
        /// <value>
        /// The maximum synchronize item retries.
        /// </value>
        public int MaxSyncItemRetries
        {
            get
            {
                return this.maxSyncItemRetries;
            }

            set
            {
                this.maxSyncItemRetries = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an item can be taken from the wait queue
        /// </summary>
        /// <value>
        /// <c>true</c> if items can be taken from the queue otherwise, <c>false</c>.
        /// </value>
        internal bool CanDequeueWaitQueue
        {
            get
            {
                return this.canDequeueWaitQueue;
            }

            set
            {
                this.canDequeueWaitQueue = value;
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

        /// <summary>
        /// Gets or sets the file handle wrapper.
        /// </summary>
        /// <value>
        /// The file handle wrapper.
        /// </value>
        internal IFileHandleWrapper FileHandleWrapper
        {
            get
            {
                return this.fileHandleWrapper;
            }

            set
            {
                this.fileHandleWrapper = value;
            }
        }

        /// <summary>
        /// Gets or sets the actual synchronize item.
        /// </summary>
        /// <value>
        /// The actual synchronize item.
        /// </value>
        internal SyncItem ActualSyncItem
        {
            get
            {
                return this.actualSyncItem;
            }

            set
            {
                this.actualSyncItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the sync item wait queue timer.
        /// </summary>
        /// <value>
        /// The synchronize item wait queue timer.
        /// </value>
        internal Timer SyncItemWaitQueueTimer
        {
            get
            {
                return this.syncItemWaitQueueTimer;
            }

            set
            {
                this.syncItemWaitQueueTimer = value;
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
            lock (this.enqueueLockOject)
            {
                this.CanDequeueItems = false;

                Task enqueueTask = Task.Factory.StartNew(() => this.SyncItemQueue.Enqueue(item));
                enqueueTask.Wait();

                this.CanDequeueItems = true;
            }
        }

        /// <summary>
        /// Enqueues an item to the wait queue.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        internal void EnqueueWaitQueue(SyncItem item)
        {
            lock (this.enqueueLockOject)
            {
                Task enqueueTask = Task.Factory.StartNew(() => this.SyncItemWaitQueue.Enqueue(item));
                enqueueTask.Wait();
            }
        }

        #region Event Methods

        /// <summary>
        /// Occurs when the sync item wait queue timer elapsed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.
        /// </param>
        protected void SyncItemWaitQueueTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.CanDequeueWaitQueue = true;
        }

        /// <summary>
        /// Called when an exception is caught.
        /// </summary>
        /// <param name="sender">
        /// The sender.
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
        /// Called when an item is finalized.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The <see cref="ItemFinalizedEventArgs"/> instance containing the event data.
        /// </param>
        protected void OnItemFinalized(object sender, ItemFinalizedEventArgs data)
        {
            if (this.ItemFinalized != null)
            {
                this.ItemFinalized(this, data);
            }
        }

        /// <summary>
        /// Called when an item is added to the wait queue
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The <see cref="UpdateProgressEventArgs"/> instance containing the event data.
        /// </param>
        protected void OnItemAddedToWaitQueueHandler(object sender, UpdateProgressEventArgs data)
        {
            if (this.ItemAddedToWaitQueue != null)
            {
                this.ItemAddedToWaitQueue(this, data);
            }
        }

        /// <summary>
        /// Occurs when an error occurred
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The <see cref="FileHandlesErrorEventArgs"/> instance containing the event data.
        /// </param>
        protected void FileHandleWrapperFileHandlesError(object sender, FileHandlesErrorEventArgs data)
        {
            // TODO: Error handling
        }

        /// <summary>
        /// Occurs when the file handle wrapper has finished
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="data">
        /// The <see cref="FileHandlesEventArgs"/> instance containing the event data.
        /// </param>
        protected void FileHandleWrapperFileHandlesFound(object sender, FileHandlesEventArgs data)
        {
            bool hasHandlesOpen = data.HasFileHandles;

            if (!hasHandlesOpen)
            {
                this.FinalizeQueueItem(this.ActualSyncItem);
            }
        }

        #endregion

        /// <summary>
        /// Checks the open handles for a file.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        private void CheckOpenHandles(SyncItem item)
        {
            this.ActualSyncItem = item;

            if (item.FullQualifiedSourceFileName.IsDirectory())
            {
                this.FinalizeQueueItem(this.ActualSyncItem);
            }
            else
            {
                this.FileHandleWrapper.FindHandlesForFile(item.FullQualifiedSourceFileName);
            }
        }

        /// <summary>
        /// Finalizes the queue item.
        /// </summary>
        /// <param name="syncItem">
        /// The synchronize item.
        /// </param>
        private void FinalizeQueueItem(SyncItem syncItem)
        {
            try
            {
                switch (syncItem.FileAction)
                {
                    case FileAction.DELETE:
                        this.fileSystemManager.Delete(syncItem);
                        break;

                    case FileAction.COPY:
                        this.FileSystemManager.Copy(syncItem);
                        break;

                    case FileAction.RENAME:
                        this.FileSystemManager.Rename(syncItem);
                        break;
                }

                this.OnItemFinalized(
                    this, 
                    new ItemFinalizedEventArgs(syncItem.SourceFile, syncItem.FileAction)
                        {
                           ItemsLeftInQueue = this.SyncItemQueue.Count 
                        });
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                syncItem.Retries++;

                if (syncItem.Retries > this.MaxSyncItemRetries)
                {
                    this.OnItemAddedToWaitQueueHandler(
                        this, 
                        new UpdateProgressEventArgs(
                            syncItem.SourceFile + " has been added to the wait queue because it cannot be accessed"));
                    this.EnqueueWaitQueue(syncItem);
                }
                else
                {
                    this.Enqueue(syncItem);
                }

                this.OnCaughtException(
                    this, 
                    new CaughtExceptionEventArgs(
                        unauthorizedAccessException.GetType(), unauthorizedAccessException.Message));
            }
            catch (ArgumentException argumentException)
            {
                this.OnCaughtException(
                    this, new CaughtExceptionEventArgs(argumentException.GetType(), argumentException.Message));
            }
            catch (PathTooLongException pathTooLongException)
            {
                this.OnCaughtException(
                    this, new CaughtExceptionEventArgs(pathTooLongException.GetType(), pathTooLongException.Message));
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
                    this, new CaughtExceptionEventArgs(fileNotFoundException.GetType(), fileNotFoundException.Message));
            }
            catch (IOException systemIOException)
            {
                syncItem.Retries++;

                if (syncItem.Retries > this.MaxSyncItemRetries)
                {
                    this.EnqueueWaitQueue(syncItem);
                    this.OnItemAddedToWaitQueueHandler(
                        this, 
                        new UpdateProgressEventArgs(
                            syncItem.SourceFile + " has been added to the wait queue because it cannot be accessed"));
                }
                else
                {
                    this.Enqueue(syncItem);
                }

                this.OnCaughtException(
                    this, new CaughtExceptionEventArgs(systemIOException.GetType(), systemIOException.Message));
            }
            catch (NotSupportedException notSupportedException)
            {
                this.OnCaughtException(
                    this, new CaughtExceptionEventArgs(notSupportedException.GetType(), notSupportedException.Message));
            }
            catch (AggregateException aggregateException)
            {
                this.OnCaughtException(
                    this, new CaughtExceptionEventArgs(aggregateException.GetType(), aggregateException.Message));
            }
        }

        /// <summary>
        /// Finalize Queue
        /// </summary>
        private void FinalizeQueue()
        {
            while (this.IsWorkerInProgress)
            {
                Thread.Sleep(100);

                if (this.CanDequeueItems)
                {
                    SyncItem item;

                    if (this.CanDequeueWaitQueue && this.SyncItemWaitQueue.Count > 0)
                    {
                        if (this.SyncItemWaitQueue.TryDequeue(out item))
                        {
                            lock (this.queueItemLockObject)
                            {
                                this.FinalizeQueueItem(item);
                                this.CanDequeueWaitQueue = false;
                            }
                        }
                    }
                    else
                    {
                        if (this.SyncItemQueue.TryDequeue(out item))
                        {
                            lock (this.queueItemLockObject)
                            {
                                this.FinalizeQueueItem(item);

                                // TODO: IMPROVE NOT WORKING GOOD ENOUGH
                                // this.CheckOpenHandles(item);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}