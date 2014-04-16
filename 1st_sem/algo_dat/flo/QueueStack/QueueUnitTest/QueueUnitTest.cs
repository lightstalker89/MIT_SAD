using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyQueueClass;
using NUnit.Framework;

namespace QueueUnitTest
{
    [TestFixture]
    public class QueueUnitTest
    {
        private QueueClass queue;

        [SetUp]
        public void Init()
        {
            queue = new QueueClass();
        }

        [TestCase]
        public void TestQueue()
        {
            string[] testSentence = new[] { "This", "is", "a", "random", "sentence", " filling", "in", "in", "the", "stack" };

            queue.EnqueueQueue(testSentence);
            string element = queue.PeekFirstElementInQueue(); //Get first element without removing it
            string[] returnedArray = queue.DequeueQueue(10); // Get elements in queue

            Assert.AreEqual(testSentence, returnedArray); // check if arrays are equal
            Assert.AreEqual(element, testSentence.First()); //Check if last element is equel last element in array
            Assert.IsEmpty(queue.DequeueQueue(10)); // check if queue is empty
        }
    }
}
