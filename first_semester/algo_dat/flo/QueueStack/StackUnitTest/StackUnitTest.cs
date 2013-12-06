using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MyStackClass;

namespace StackUnitTest
{
    [TestFixture]
    public class StackUnitTest
    {
        private StackClass stack;

        [SetUp]
        public void Init()
        {
            stack = new StackClass();
        }

        [TestCase]
        public void TestStack()
        {
            string[] testSentence = new[]
            {"This", "is", "a", "random", "sentence", " filling", "in", "in", "the", "stack"};

            stack.FillInStack(testSentence); // Fill in elements
            string element = stack.PeekFirstElementInQueue(); //Get first element without removing it
            string[] returnedArray = stack.TakeOutStack(10);  // Get elements in stack
            string[] newArray = returnedArray.Reverse().ToArray(); // Revers array to get original order

            Assert.AreEqual(testSentence, newArray); // Check if output array is the same as input
            Assert.AreEqual(element, testSentence.Last()); //Check if last element is equel last element in array
            Assert.IsEmpty(stack.TakeOutStack(10)); // check if the stack is empty
        }
    }
}
