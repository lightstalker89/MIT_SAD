using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Algodat_UE3.SearchAlgos;

namespace FibonacciSearchTests
{
    [TestClass]
    public class UnitTest1
    {
        private int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private FibonacciSearch FibSearch = new FibonacciSearch();

        [TestMethod]
        public void FibonacciFound()
        {
            //if found item will be returned
            Assert.AreEqual(4, numbers[this.FibSearch.Search(4, numbers)]);
        }

        [TestMethod]
        public void FibonacciNotFound()
        {
            //if nit found -1 will be returned
            Assert.AreEqual(-1, this.FibSearch.Search(100, numbers));
        }

        [TestMethod]
        public void FiboCheckFibonacciNumber()
        {
            Assert.AreEqual(5, FibSearch.FindFibonacciNumber(3,9));
        }
    }
}
