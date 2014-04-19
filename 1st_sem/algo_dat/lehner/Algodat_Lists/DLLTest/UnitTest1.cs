using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Algodat_Lists;

namespace DLLTest
{
    [TestClass]
    public class UnitTest1
    {
        private int[] Numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private DoubleLinkedList DLL { get; set; }
        public UnitTest1()
        {
            this.DLL = new DoubleLinkedList();
            this.DLL.CreateDLList(this.Numbers);
        }

        [TestMethod]
        public void InsertTest()
        {
            this.DLL.StepForward();
            int i = 0;
            while (this.DLL.Current != null)
            {
                Assert.AreEqual(this.DLL.Current.Data, this.Numbers[i]);
                i++;
                this.DLL.StepForward();
            }
        }

        [TestMethod]
        public void InsertBeforeTest()
        {
            this.DLL.InsertBefore(this.DLL.Tail, 30);
            Console.WriteLine(this.DLL.Tail.Previous.Data);
            Assert.AreEqual(this.DLL.Tail.Previous.Data, 30);
        }

        [TestMethod]
        public void InsertAfterTest()
        {
            this.DLL.InsertAfter(this.DLL.Head, 12);
            Assert.AreEqual(this.DLL.Head.Next.Data, 12);
        }

        [TestMethod]
        public void RemoveTest()
        {
            this.DLL.Remove(this.DLL.Head);
            Assert.AreEqual(this.DLL.Head.Data, 2);
        }
    }
}
