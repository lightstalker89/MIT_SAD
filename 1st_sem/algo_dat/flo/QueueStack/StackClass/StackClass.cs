using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStackClass
{
    public class StackClass
    {
        private Stack<string> myStack;

        public StackClass()
        {
            this.myStack = new Stack<string>();
        }

        /// <summary>
        /// Fill in specific number of elements in myStack
        /// </summary>
        /// <param name="input">
        /// String elements which will be filled in in queue
        /// </param>
        public void FillInStack(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                myStack.Push(input[i]);
            }
        }

        /// <summary>
        /// Get specific number of elements in myStack
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
                    elements[i] = (string)myStack.Pop();
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
        /// Peek first element in stack without removing it
        /// </summary>
        public string PeekFirstElementInQueue()
        {
            try
            {
                return (string)myStack.Peek();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return String.Empty;
            }
        }
    }
}
