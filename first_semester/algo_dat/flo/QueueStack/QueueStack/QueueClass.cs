using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace QueueStack
{
    class QueueClass
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
        public void FillInQueue(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                myQueue.Enqueue(input[i]);
            }
        }

        /// <summary>
        /// Get specific number of elements queue
        /// </summary>
        /// <param name="count">
        /// Number of requested elements
        /// </param>
        public string[] TakeOutQueue(int count)
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
                return elements;
            }
        }
    }
}
