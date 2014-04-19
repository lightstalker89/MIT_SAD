using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bsp1b;

namespace Bsp1bTests
{
    [TestClass]
    public class QueueTests
    {
        [TestMethod]
        public void ctorTest()
        {
            Queue<double> s = new Queue<double>();

            PrivateObject poS = new PrivateObject(s);

            Assert.IsTrue(((int)poS.GetField("lastIndex")) < 0);
            Assert.AreEqual(0, ((double[])poS.GetField("queue")).Length);
        }

        [TestMethod]
        public void EnqueueTest()
        {
            Queue<double> s = new Queue<double>();

            s.Enqueue(23.3d);
            s.Enqueue(8437209.12d);
            s.Enqueue(7437209.12d);
            s.Enqueue(6437209.12d);
            s.Enqueue(5437209.12d);
            s.Enqueue(4437209.12d);
            s.Enqueue(3437209.12d);
            s.Enqueue(2437209.12d);
            s.Enqueue(1437209.12d);

            PrivateObject poS = new PrivateObject(s);

            double[] privateQueue = (double[])poS.GetField("queue");

            Assert.AreEqual(8, ((int)poS.GetField("lastIndex")));
            Assert.IsTrue(privateQueue.Length >= 9);
            Assert.AreEqual(23.3d, privateQueue[0]);
            Assert.AreEqual(8437209.12d, privateQueue[1]);
            Assert.AreEqual(7437209.12d, privateQueue[2]);
            Assert.AreEqual(6437209.12d, privateQueue[3]);
            Assert.AreEqual(5437209.12d, privateQueue[4]);
            Assert.AreEqual(4437209.12d, privateQueue[5]);
            Assert.AreEqual(3437209.12d, privateQueue[6]);
            Assert.AreEqual(2437209.12d, privateQueue[7]);
            Assert.AreEqual(1437209.12d, privateQueue[8]);
        }

        [TestMethod]
        [ExpectedException(typeof(QueueEmptyException))]
        public void DequeueTest()
        {
            Queue<double> s = new Queue<double>();

            s.Enqueue(23.3d);
            s.Enqueue(8437209.12d);
            s.Enqueue(7437209.12d);
            Assert.AreEqual(23.3d, s.Dequeue());
            s.Enqueue(6437209.12d);
            s.Enqueue(5437209.12d);
            s.Enqueue(4437209.12d);
            s.Enqueue(3437209.12d);
            s.Enqueue(2437209.12d);
            Assert.AreEqual(8437209.12d, s.Dequeue());
            s.Enqueue(1437209.12d);
            Assert.AreEqual(7437209.12d, s.Dequeue());
            Assert.AreEqual(6437209.12d, s.Dequeue());
            Assert.AreEqual(5437209.12d, s.Dequeue());
            Assert.AreEqual(4437209.12d, s.Dequeue());
            Assert.AreEqual(3437209.12d, s.Dequeue());
            Assert.AreEqual(2437209.12d, s.Dequeue());
            Assert.AreEqual(1437209.12d, s.Dequeue());

            PrivateObject poS = new PrivateObject(s);

            double[] privateQueue = (double[])poS.GetField("queue");

            Assert.IsTrue(((int)poS.GetField("lastIndex")) < 0);
            Assert.IsTrue(privateQueue.Length >= 0);
            s.Dequeue();
        }

        [TestMethod]
        public void PeekTest()
        {
            Queue<double> s = new Queue<double>();

            s.Enqueue(23.3d);
            s.Enqueue(8437209.12d);
            Assert.AreEqual(23.3d, s.Peek());
            s.Enqueue(7437209.12d);
            Assert.AreEqual(23.3d, s.Peek());
            s.Enqueue(6437209.12d);
            s.Enqueue(5437209.12d);
            Assert.AreEqual(23.3d, s.Peek());
            s.Enqueue(4437209.12d);
            s.Enqueue(3437209.12d);
            s.Enqueue(2437209.12d);
            s.Enqueue(1437209.12d);
            Assert.AreEqual(23.3d, s.Peek());
            s.Dequeue();
            Assert.AreEqual(8437209.12d, s.Peek());
        }
    }
}
