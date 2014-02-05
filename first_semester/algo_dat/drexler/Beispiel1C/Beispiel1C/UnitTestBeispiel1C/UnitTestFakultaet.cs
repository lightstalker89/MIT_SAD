using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Beispiel1C;

namespace UnitTestBeispiel1C
{
    [TestClass]
    public class UnitTestFakultaet
    {
        [TestMethod]
        public void BerechneStandardReksuion()
        {
            int result = Fakultaet.BerechneStandardRekusion(5);
            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void BerechneLimitStandardRekusion()
        {
            
        }

        [TestMethod]
        public void BerechneTailRekursion()
        {
            int result = Fakultaet.BerechneTailRekursion(5, 1);
            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void BerechneOhneRekursion()
        {
            int result = Fakultaet.BerechneOhneRekursion(5, 1);
            Assert.AreEqual(120, result);
        }
    }
}
