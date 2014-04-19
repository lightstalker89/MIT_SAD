using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStackProgram;

namespace MyStackTests
{
    [TestClass]
    public class MyStackTestsPop
    {
        [TestMethod]
        public void Pop_items()
        {
            MyStack s = new MyStack(10);
            s.Push(1);
            s.Push(2);
            s.Push(3);
            s.Pop();
            Assert.AreEqual(0, s.GetElementAtIndex(2));
        }

        [TestMethod]
        public void Push_item()
        {
            MyStack s = new MyStack(10);
            s.Push(1);
            Assert.AreEqual(1, s.GetElementAtIndex(0));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Stack_OutOfBound_Check()
        {
            MyStack s = new MyStack(10);
            s.Push(1);
            s.Push(2);
            s.Push(3);
            s.Push(4);
            s.Push(5);
            s.Push(6);
            s.Push(7);
            s.Push(8);
            s.Push(9);
            s.Push(10);
            s.Push(11);
        }
    }
}
