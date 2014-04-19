using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStackProgram
{

    public class MyStack
    {

        int[] MyStackCollection;
        int Counter = 0;

        public MyStack(int count)
        {
            this.MyStackCollection = new int[count];
        }

        public void Push(int value)
        {
            this.MyStackCollection[Counter] = value;
            ++this.Counter;
        }

        public void Pop()
        {
            --this.Counter;

            int[] stackTemp = new int[this.MyStackCollection.Length];
            for (int i = 0; i < this.Counter; ++i)
            {
                stackTemp[i] = this.MyStackCollection[i];
            }
            this.MyStackCollection = stackTemp;
        }

        public void PrintMyStack()
        {
            Console.WriteLine("Print MyStack:");
            for (int i = 0; i < this.Counter; ++i)
            {
                Console.WriteLine("{0}", this.MyStackCollection[i]);
            }
        }

        public int StackLength()
        {
            return this.MyStackCollection.Length;
        }

        public int GetElementAtIndex(int index)
        {
            return this.MyStackCollection[index];
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            MyStack s = new MyStack(10);
            s.Push(1);
            s.Push(2);
            s.Push(3);
            s.PrintMyStack();
            s.Pop();
            s.PrintMyStack();
        }
    }
}
