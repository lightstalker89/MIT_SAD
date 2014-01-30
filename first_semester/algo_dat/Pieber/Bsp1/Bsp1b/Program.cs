using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp1b
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<int> s = new Queue<int>();

            try
            {
                s.Enqueue(3);
                s.Enqueue(8);
                s.Enqueue(4);
                s.Enqueue(4794);
                s.Enqueue(1278395);
                s.Enqueue(24373);
                int dequeueValue = s.Dequeue();
                dequeueValue = s.Dequeue();
                dequeueValue = s.Dequeue();
                int peekValue = s.Peek();
                dequeueValue = s.Dequeue();
                dequeueValue = s.Dequeue();
            }
            catch (QueueEmptyException)
            {
                Console.WriteLine("Error: one Dequeue to much, end of queue reached");
            }
        }
    }
}
