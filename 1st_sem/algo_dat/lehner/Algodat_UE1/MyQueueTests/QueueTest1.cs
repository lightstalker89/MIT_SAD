using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyQueueProgram;

namespace MyQueueTests
{
    [TestClass]
    public class QueueTest1
    {
        [TestMethod]
        public void Dequeue_items()
        {
            MyQueue s = new MyQueue(10);
            s.Enqueue(1);
            s.Enqueue(2);
            s.Enqueue(3);
            s.Dequeue();
            Assert.AreEqual(0, s.GetElementAtIndex(2));
        }

        [TestMethod]
        public void Enqueue_items()
        {
            MyQueue s = new MyQueue(10);
            s.Enqueue(1);
            Assert.AreEqual(1, s.GetElementAtIndex(0));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Queue_OutOfBound_Check()
        {
            MyQueue s = new MyQueue(10);
            s.Enqueue(1);
            s.Enqueue(2);
            s.Enqueue(3);
            s.Enqueue(4);
            s.Enqueue(5);
            s.Enqueue(6);
            s.Enqueue(7);
            s.Enqueue(8);
            s.Enqueue(9);
            s.Enqueue(10);
            s.Enqueue(11);
        }
    }
}
