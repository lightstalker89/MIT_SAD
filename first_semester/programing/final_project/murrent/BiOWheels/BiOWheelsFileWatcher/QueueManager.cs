using System.Collections.Concurrent;
using System.Threading;

namespace BiOWheelsFileWatcher
{
    internal class QueueManager : IQueueManager
    {
        internal QueueManager()
        {
            this.syncItemQueue = new ConcurrentQueue<SyncItem>();
        }

        #region Private Fields
        /// <summary>
        /// 
        /// </summary>
        private ConcurrentQueue<SyncItem> syncItemQueue;

        /// <summary>
        /// 
        /// </summary>
        private bool isWorkerInProgress;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public bool IsWorkerInProgress
        {
            get { return this.isWorkerInProgress; }
            set
            {
                this.isWorkerInProgress = value;
            }
        }

        /// <summary>
        /// 
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
        #endregion

        #region Methods

        /// <inheritdoc/>
        public void DoWork()
        {
            Thread workerThread = new Thread(FinalizeQueue)
                                  {
                                      IsBackground = true
                                  };
            workerThread.Start();
        }

        /// <inheritdoc/>
        public void Enqueue(SyncItem item)
        {
            this.SyncItemQueue.Enqueue(item);
        }

        /// <summary>
        /// 
        /// </summary>
        private void FinalizeQueue()
        {
            while (this.IsWorkerInProgress)
            {
                SyncItem item;

                if (this.SyncItemQueue.TryDequeue(out item))
                {

                }
            }
        }

        #endregion
    }
}
