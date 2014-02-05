using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Beispiel1A_1B;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestQueue
    {
        /// <summary>
        /// Enqueue items to the queue, and dequeue the first
        /// item. Should return the first item in the queue.
        /// </summary>
        [TestMethod]
        public void EnqueueDequeue()
        {
            string message = string.Empty;
            string input = string.Empty;
            string output = string.Empty;

            Beispiel1A_1B.Queue<string> myQueue = new Beispiel1A_1B.Queue<string>();

            try
            {
                for (int i = 0; i < 11; ++i)
                {
                    myQueue.Enqueue(String.Format("item{0}", i));
                }
                output = myQueue.Dequeue();
            }
            catch (NotSupportedException e)
            {
                message = e.Message;
            }
            catch (InvalidOperationException e)
            {
                message = e.Message;
            }
            catch (OutOfMemoryException e)
            {
                message = e.Message;
            }

            Assert.AreEqual("item0", output, message);
        }

        /// <summary>
        /// Dequeue an item from an empty queue and enqueue items to the queue.
        /// Should return an exception because no items are available for
        /// dequeue.
        /// </summary>
        [TestMethod]
        public void DequeueEnqueue()
        {
            string message = string.Empty;
            const string errorMessage1 = "Die Sequenz enthält keine Elemente.";

            string input = string.Empty;
            string output = string.Empty;

            Beispiel1A_1B.Queue<string> myQueue = new Beispiel1A_1B.Queue<string>();

            try
            {
                output = myQueue.Dequeue();
                for (int i = 0; i < 11; ++i)
                {
                    myQueue.Enqueue(String.Format("item{0}", i));
                }
            }
            catch (NotSupportedException e)
            {
                message = e.Message;
            }
            catch (InvalidOperationException e)
            {
                message = e.Message;
            }
            catch (OutOfMemoryException e)
            {
                message = e.Message;
            }

            Assert.AreEqual(errorMessage1, message);
        }

        /// <summary>
        /// Enqueue items to the queue, remove (dequeue) the first item
        /// and peek an item
        /// </summary>
        [TestMethod]
        public void EnqueuePeek()
        {
            string errorMessage1 = "Not supported Exception";
            string errorMessage2 = "Invalid operation Exception";
            string errorMessage3 = "Argument null Exception";
            string errorMessage4 = "Out of memory Exception";
            string message = string.Empty;

            string output = string.Empty;
            string peekOutput = string.Empty;

            Beispiel1A_1B.Queue<string> myQueue = new Beispiel1A_1B.Queue<string>();
            try
            {
                for (int i = 0; i < 11; ++i)
                {
                    myQueue.Enqueue(String.Format("item{0}", i));
                }

                output = myQueue.Dequeue();
                peekOutput = myQueue.Peek();

            }
            catch (NotSupportedException e)
            {
                message = "Not supported Exception";
            }
            catch (InvalidOperationException e)
            {
                message = "Invalid operation Exception";
            }
            catch (ArgumentNullException e)
            {
                message = "Argument null Exception";
            }
            catch (OutOfMemoryException e)
            {
                message = "Out of memory Exception";
            }

            Assert.AreEqual("item1", peekOutput, message);
        }

        /// <summary>
        /// Fill the queue to the limit.
        /// </summary>
        [TestMethod]
        public void EnqueueQueueFull()
        {
            string message = string.Empty;
            string errorMessage = "Not supported exeption";
            string errorMessage1 = "Out of memory exception";
            string errorMessage2 = "Invalid operation exception";

            string input = string.Empty;
            string output = string.Empty;

            Beispiel1A_1B.Queue<string> myQueue = new Beispiel1A_1B.Queue<string>();

            try
            {
                for (long i = 0; i < 501; ++i)
                {
                    myQueue.Enqueue(String.Format("item{0}", i));
                    if (i == 500)
                        input = String.Format("item{0}", i);
                }
                output = myQueue.Dequeue();
            }
            catch (OutOfMemoryException e)
            {
                message = "Out of memory exception";
            }
            catch (InvalidOperationException e)
            {
                message = "Invalid operation exception";
            }
            catch (NotSupportedException e)
            {
                message = "Not supported exception";
            }

            Assert.AreEqual(errorMessage1, message);
        }
    }
}
