using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp1a
{
    public class Stack<T>
    {
        T[] stack;
        int lastIndex;

        public Stack()
        {
            stack = new T[0];
            lastIndex = -1;
        }

        public void Push(T value)
        {
            Array.Resize<T>(ref stack, (++lastIndex) + 1);
            stack[lastIndex] = value;
        }

        public T Pop()
        {
            return GetLastElement(true);
        }

        public T Peek()
        {
            return GetLastElement(false);
        }

        private T GetLastElement(bool remove)
        {
            if (lastIndex < 0)
            {
                throw new StackEmptyException();
            }

            if (remove)
            {
                T tmp = stack[lastIndex];

                stack[lastIndex--] = default(T);

                return tmp;
            }
            else
            {
                return stack[lastIndex];
            }
        }
    }
}
