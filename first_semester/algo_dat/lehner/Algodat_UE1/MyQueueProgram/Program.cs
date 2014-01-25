using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQueueProgram
{
    public class MyQueue
    {
        int[] MyQueueCollection;
        int Counter = 0;

        public MyQueue(int count)
        {
            this.MyQueueCollection = new int[count];
        }

        public void Enqueue(int value)
        {
            this.MyQueueCollection[this.Counter] = value;
            this.Counter++;
        }

        public void Dequeue()
        {
            int[] queueTemp = new int[this.MyQueueCollection.Length];
            for (int i = 1; i < this.Counter; ++i)
            {
                queueTemp[i-1] = this.MyQueueCollection[i];
            }
            this.MyQueueCollection = queueTemp;
            this.Counter--;
        }

        public void PrintMyQueue()
        {
            Console.WriteLine("Print MyQueue:");
            for (int i = 0; i < this.Counter; ++i)
            {
                Console.WriteLine("{0}", this.MyQueueCollection[i]);
            }
        }

        public int QueueLength()
        {
            return this.MyQueueCollection.Length;
        }

        public int GetElementAtIndex(int index)
        {
            return this.MyQueueCollection[index];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyQueue s = new MyQueue(10);
            s.Enqueue(1);
            s.Enqueue(2);
            s.Enqueue(3);
            s.PrintMyQueue();
            s.Dequeue();
            s.PrintMyQueue();
        }
    }
}
