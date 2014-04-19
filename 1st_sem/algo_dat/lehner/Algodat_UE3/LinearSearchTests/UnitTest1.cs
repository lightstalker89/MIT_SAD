using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Algodat_UE3.SearchAlgos;

namespace LinearSearchTests
{
    [TestClass]
    public class UnitTest1
    {

        private int[] numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        private LinearSearch LinSearch = new LinearSearch();

        [TestMethod]
        public void LinearFound()
        {
            //if found item will be returned
            Assert.AreEqual(7, this.LinSearch.Search(7, numbers));
        }

        [TestMethod]
        public void LinearNotFound()
        {
            //if nit found -1 will be returned
            Assert.AreEqual(-1, this.LinSearch.Search(100, numbers));
        }
    }
}
