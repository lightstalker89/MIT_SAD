// *******************************************************
// * <copyright file="QueueSampleTest.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace QueueSample.Test
{
    using System.Globalization;
    using System.Linq;

    using NSubstitute;

    using NUnit.Framework;

    /// <summary>
    /// The <see ref="QueueSampleTest"/> class and its interaction logic 
    /// </summary>
    [TestFixture]
    public class QueueSampleTest
    {
        /// <summary>
        /// The queue
        /// </summary>
        private QueueSample<string> queue;

        /// <summary>
        /// Set up test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.queue = Substitute.For<QueueSample<string>>();
            this.queue.MaxQueueItems = 10;
        }

        /// <summary>
        /// Test properties of the class
        /// </summary>
        [TestCase]
        public void QueueSampleClassTest()
        {
            Assert.That(queue.MaxQueueItems.Equals(10));
        }

        /// <summary>
        /// Add max allowed items to the queue
        /// </summary>
        [TestCase]
        public void EnqueueToMaxItemCountTest()
        {
            for (int i = 0; i < 10; ++i)
            {
                this.queue.Enqueue("Item " + i.ToString(CultureInfo.InvariantCulture));
            }

            Assert.That(this.queue.QueueCount.Equals(10));
        }

        /// <summary>
        /// Add more items to the queue than allowed
        /// </summary>
        [TestCase]
        public void EnqueueOverMaxItemCountTest()
        {
            Assert.Throws<QueueOutOfSpaceException>(
                () =>
                    {
                        for (int i = 0; i <= 12; ++i)
                        {
                            this.queue.Enqueue("Item " + i.ToString(CultureInfo.InvariantCulture));
                        }
                    });

            Assert.That(this.queue.QueueCount.Equals(10));
        }

        /// <summary>
        /// Dequeue the queue with no items
        /// </summary>
        [TestCase]
        public void DequeueEmptyQueueTest()
        {
            EmptyQueue();

            Assert.Throws<QueueIsEmptyException>(() => this.queue.Dequeue());
        }

        /// <summary>
        /// Dequeue the queue filled with items
        /// </summary>
        [TestCase]
        public void DequeueTest()
        {
            FillQueue();

            for (int i = 9; i >= 0; --i)
            {
                this.queue.Dequeue();
            }

            Assert.That(this.queue.QueueCount.Equals(0));
        }

        /// <summary>
        /// Get an item without deleting from the queue
        /// </summary>
        [TestCase]
        public void PeekTest()
        {
            this.EmptyQueue();
            this.FillQueue();

            int countBefore = this.queue.QueueCount;

            string item = this.queue.Peek();

            int countAfter = this.queue.QueueCount;

            Assert.That(countBefore.Equals(countAfter));
            Assert.NotNull(item);
            Assert.Contains(item, this.queue.Queue.ToList());
        }

        /// <summary>
        /// Fills the queue.
        /// </summary>
        private void FillQueue()
        {
            for (int i = this.queue.QueueCount; i < this.queue.MaxQueueItems; ++i)
            {
                this.queue.Enqueue("Item " + i);
            }
        }

        /// <summary>
        /// Empties the queue.
        /// </summary>
        private void EmptyQueue()
        {
            if (this.queue.QueueCount > 0)
            {
                for (int i = this.queue.QueueCount; i >= 0; --i)
                {
                    this.queue.Dequeue();
                }
            }
        }
    }
}