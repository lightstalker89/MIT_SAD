using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Algodat_UE3.SearchAlgos;

namespace BinarySearchTests
{
    [TestClass]
    public class UnitTest1
    {
        private int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private BinarySearch BinSearch = new BinarySearch();

        [TestMethod]
        public void BinaryFound()
        {
            //if found item will be returned
            int num = this.BinSearch.Search(4, numbers);
            Console.WriteLine(num);
            Assert.AreEqual(4, numbers[this.BinSearch.Search(4, numbers)]);
        }

        [TestMethod]
        public void BinaryNotFound()
        {
            //if nit found -1 will be returned
            Assert.AreEqual(-1, this.BinSearch.Search(100, numbers));
        }

        [TestMethod]
        public void BinaryCheckMiddle()
        {
            Assert.AreEqual(6, BinSearch.FindMiddle(3,9));
        }
    }
}
