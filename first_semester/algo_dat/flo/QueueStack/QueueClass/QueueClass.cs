using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQueueClass
{
    public class QueueClass
    {
        /// <summary>
        /// The queue object of the class
        /// </summary>
        private Queue myQueue;

        public QueueClass()
        {
            this.myQueue = new Queue();
        }

        /// <summary>
        /// Fill in specific number of elements in queue
        /// </summary>
        /// <param name="input">
        /// String elements which will be filled in in queue
        /// </param>
        public void EnqueueQueue(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                myQueue.Enqueue(input[i]);
            }
        }

        /// <summary>
        /// Dequeue specific number of elements of the queue
        /// </summary>
        /// <param name="count">
        /// Number of requested elements
        /// </param>
        public string[] DequeueQueue(int count)
        {
            string[] elements = new string[count];
            try
            {
                for (int i = 0; i < count; i++)
                {
                    elements[i] = (string)myQueue.Dequeue();
                }
                return elements;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return new string[0];
            }
        }

        /// <summary>
        /// Peek first element in queue without removing it
        /// </summary>
        public string PeekFirstElementInQueue()
        {
            try
            {
                return (string)myQueue.Peek();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return String.Empty;
            }
        }
    }
}
