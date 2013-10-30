// *******************************************************
// * <copyright file="QueueManager.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System;
using System.IO;
using BiOWheelsFileWatcher.CustomEventArgs;

namespace BiOWheelsFileWatcher
{
    using System.Collections.Concurrent;
    using System.Threading;

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
        internal QueueManager()
        {
            this.syncItemQueue = new ConcurrentQueue<SyncItem>();
            this.fileComparator = new FileComparator();
        }

        #region Delegates

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public delegate void CaughtExceptionHandler(object sender, CaughtExceptionEventArgs data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
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
        /// Gets the <see cref="FileComparator"/> instance
        /// </summary>
        internal FileComparator FileComparator
        {
            get
            {
                return this.fileComparator;
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
                                File.Delete(item.DestinationFolder + Path.DirectorySeparatorChar +
                                            Path.GetFileName(item.SourceFile));
                            }
                            else
                            {
                                if (item.IsParallelSyncAllowed)
                                {

                                }
                                else
                                {
                                    File.Copy(item.SourceFile, item.DestinationFolder + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile), true);                                    
                                }
                            }
                        }
                        catch (UnauthorizedAccessException unauthorizedAccessException)
                        {
                            OnCaughtException(this,
                                new CaughtExceptionEventArgs(unauthorizedAccessException.GetType(),
                                    unauthorizedAccessException.Message));
                        }
                        catch (ArgumentException argumentException)
                        {
                            OnCaughtException(this,
                                new CaughtExceptionEventArgs(argumentException.GetType(), argumentException.Message));
                        }
                        catch (PathTooLongException pathTooLongException)
                        {
                            OnCaughtException(this,
                                new CaughtExceptionEventArgs(pathTooLongException.GetType(),
                                    pathTooLongException.Message));
                        }
                        catch (DirectoryNotFoundException directoryNotFoundException)
                        {
                            OnCaughtException(this,
                                new CaughtExceptionEventArgs(directoryNotFoundException.GetType(),
                                    directoryNotFoundException.Message));
                        }
                        catch (FileNotFoundException fileNotFoundException)
                        {
                            OnCaughtException(this,
                                new CaughtExceptionEventArgs(fileNotFoundException.GetType(),
                                    fileNotFoundException.Message));
                        }
                        catch (IOException systemIOException)
                        {
                            OnCaughtException(this,
                                new CaughtExceptionEventArgs(systemIOException.GetType(), systemIOException.Message));
                        }
                        catch (NotSupportedException notSupportedException)
                        {
                            OnCaughtException(this,
                                new CaughtExceptionEventArgs(notSupportedException.GetType(),
                                    notSupportedException.Message));
                        }
                        break;
                    }
                    else if (item.FileAction == FileAction.DIFF)
                    {

                    }
                }
            }
        }

        #endregion
    }
}