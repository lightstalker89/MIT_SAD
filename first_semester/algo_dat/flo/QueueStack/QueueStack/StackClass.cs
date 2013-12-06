using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStack
{
    class StackClass
    {
        private Stack<string> stack;

        public StackClass()
        {
            this.stack = new Stack<string>();
        }

        /// <summary>
        /// Fill in specific number of elements in stack
        /// </summary>
        /// <param name="input">
        /// String elements which will be filled in in queue
        /// </param>
        public void FillInStack(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                stack.Push(input[i]);
            }
        }

        /// <summary>
        /// Get specific number of elements in stack
        /// </summary>
        /// <param name="count">
        /// Number of requested elements
        /// </param>
        public string[] TakeOutStack(int count)
        {
            string[] elements = new string[count];
            try
            {
                for (int i = 0; i < count; i++)
                {
                    elements[i] = (string)stack.Pop();
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
