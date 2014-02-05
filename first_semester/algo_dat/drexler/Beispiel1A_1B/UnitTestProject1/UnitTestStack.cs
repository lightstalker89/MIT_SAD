using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beispiel1A_1B;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestStack
    {
        /// <summary>
        /// Push items to the stack, and pop the last added
        /// item. Should return the same item.
        /// </summary>
        [TestMethod]
        public void StackPushPop()
        {
            string message = string.Empty;
            string input = string.Empty;
            string output = string.Empty;

            Stack<string> myStack = new Stack<string>();

            try
            {
                for (int i = 0; i < 11; ++i)
                {
                    myStack.Push(String.Format("item{0}",i));
                }
                output = myStack.Pop();
            }
            catch (NotSupportedException e)
            {
                message = e.Message;
            }
            catch (InvalidOperationException e)
            {
                message = e.Message;
            }
            catch (OutOfMemoryException e)
            {
                message = e.Message;
            }

            Assert.AreEqual("item10", output, message);
        }

        /// <summary>
        /// Pop an item from an empty stack and push items to the stack.
        /// Should return an exception because no items are available for
        /// pop.
        /// </summary>
        [TestMethod]
        public void StackPopPush()
        {
            string message = string.Empty;
            const string errorMessage1 = "Die Sequenz enthält keine Elemente.";

            string input = string.Empty;
            string output = string.Empty;         

            Stack<string> myStack = new Stack<string>();

            try
            {
                output = myStack.Pop();
                for (int i = 0; i < 11; ++i)
                {
                    myStack.Push(String.Format("item{0}", i));
                }
            }
            catch (InvalidOperationException e)
            {
                message = e.Message;
            }
            catch (NotSupportedException e)
            {
                message = e.Message;
            }
            catch (OutOfMemoryException e)
            {
                message = e.Message;
            }

            Assert.AreEqual(errorMessage1, message);
        }

        /// <summary>
        /// Push items to the stack, remove (pop) the last item
        /// and peek an item
        /// </summary>
        [TestMethod]
        public void StackPushPushPeek()
        {
            string errorMessage1 = "Not supported Exception";
            string errorMessage2 = "Invalid operation Exception";
            string errorMessage3 = "Argument null Exception";
            string errorMessage4 = "Out of memory Exception";
            string message = string.Empty;

            string output = string.Empty;
            string peekOutput = string.Empty;

            Stack<string> myStack = new Stack<string>();
            try
            {
                for (int i = 0; i < 11; ++i)
                {
                    myStack.Push(String.Format("item{0}", i));
                }

                output = myStack.Pop();
                peekOutput = myStack.Peek();

            }
            catch (NotSupportedException)
            {
                message = "Not supported Exception";
            }
            catch (InvalidOperationException)
            {
                message = "Invalid operation Exception";
            }
            catch (ArgumentNullException)
            {
                message = "Argument null Exception";
            }
            catch (OutOfMemoryException)
            {
                message = "Out of memory Exception";
            }

            Assert.AreEqual("item9", peekOutput, message);
        }

        /// <summary>
        /// Fill the stack to the limit.
        /// </summary>
        [TestMethod]
        public void PushStackFull()
        {
            string message = string.Empty;
            string errorMessage = "Not supported exeption";
            string errorMessage1 = "Out of memory exception";

            string input = string.Empty;
            string output = string.Empty;

            Stack<string> myStack = new Stack<string>();

            try
            {
                for (long i = 0; i < 501; ++i)
                {
                    myStack.Push(String.Format("item{0}", i));
                    if (i == 500)
                        input = String.Format("item{0}", i);
                }
                output = myStack.Pop();
            }
            catch (OutOfMemoryException)
            {
                message = "Out of memory exception";
            }
            catch (NotSupportedException)
            {
                message = "Not supported exception";
            }

            Assert.AreEqual(errorMessage1, message);  
        }
    }
}
