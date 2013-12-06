// *******************************************************
// * <copyright file="QueueSample.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("QueueSample.Test")]

namespace QueueSample
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see ref="QueueSample"/> class and its interaction logic
    /// </summary>
    /// <typeparam name="T">
    /// Type: T
    /// </typeparam>
    public class QueueSample<T> : IQueueSample<T>
    {
        /// <summary>
        /// The maximum queue items
        /// </summary>
        private int maxQueueItems;

        /// <summary>
        /// The queue
        /// </summary>
        private readonly List<T> queue = new List<T>();

        /// <summary>
        /// Adds an object to the end of the Queue
        /// </summary>
        /// <param name="queueItem">
        /// Type: T
        /// The object to add to the end of the Queue
        /// </param>
        /// <exception cref="QueueSample.QueueOutOfSpaceException">
        /// </exception>
        public void Enqueue(T queueItem)
        {
            if (this.QueueCount < this.maxQueueItems)
            {
                this.queue.Add(queueItem);
            }
            else
            {
                throw new QueueOutOfSpaceException();
            }
        }

        /// <summary>
        /// Remove and return the object at the beginning of the concurrent queue
        /// </summary>
        /// <returns>Type: T
        /// The first item of the queue</returns>
        /// <exception cref="QueueSample.QueueIsEmptyException"></exception>
        public T Dequeue()
        {
            if (this.queue.Count > 0)
            {
                T queueItem = this.queue.First();

                this.queue.Remove(queueItem);

                return queueItem;
            }

            throw new QueueIsEmptyException();
        }

        /// <summary>
        /// Pick the first item out of the queue without deleting it
        /// </summary>
        /// <returns>The first item of the queue</returns>
        public T Peek()
        {
            return this.queue.First();
        }

        /// <summary>
        /// Gets or sets the maximum queue items.
        /// </summary>
        /// <value>
        /// The maximum queue items.
        /// </value>
        public int MaxQueueItems
        {
            get
            {
                return this.maxQueueItems;
            }

            set
            {
                this.maxQueueItems = value;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the Queue
        /// </summary>
        /// <value>
        /// The queue count.
        /// </value>
        internal int QueueCount
        {
            get
            {
                return this.queue.Count;
            }
        }

        /// <summary>
        /// Gets the queue.
        /// </summary>
        /// <value>
        /// The queue.
        /// </value>
        internal IList<T> Queue
        {
            get
            {
                return this.queue;
            }
        }
    }
}