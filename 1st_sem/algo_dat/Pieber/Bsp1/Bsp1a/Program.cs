using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp1a
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> s = new Stack<int>();

            try
            {
                s.Push(3);
                int popValue = s.Pop();
                s.Push(8);
                s.Push(4);
                popValue = s.Pop();
                s.Push(4794);
                s.Push(1278395);
                popValue = s.Pop();
                int peekValue = s.Peek();
                popValue = s.Pop();
                s.Push(24373);
                popValue = s.Pop();
            }
            catch (StackEmptyException)
            {
                Console.WriteLine("Error: one Pop to much, end of stack reached");
            }
        }
    }
}
