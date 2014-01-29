using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp1b
{
    public class Queue<T>
    {
        T[] queue;
        int lastIndex;

        public Queue()
        {
            queue = new T[0];
            lastIndex = -1;
        }

        public void Enqueue(T value)
        {
            Array.Resize<T>(ref queue, (++lastIndex) + 1);
            queue[lastIndex] = value;
        }

        public T Dequeue()
        {
            return GetFirstElement(true);
        }

        public T Peek()
        {
            return GetFirstElement(false);
        }

        private T GetFirstElement(bool remove)
        {
            if (lastIndex < 0)
            {
                throw new QueueEmptyException();
            }

            if (remove)
            {
                T tmp = queue[0];

                Array.Reverse(queue);
                Array.Resize<T>(ref queue, lastIndex--);
                Array.Reverse(queue);

                return tmp;
            }
            else
            {
                return queue[0];
            }
        }
    }
}
