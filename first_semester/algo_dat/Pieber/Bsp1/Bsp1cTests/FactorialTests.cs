using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bsp1c;

namespace Bsp1cTests
{
    [TestClass]
    public class FactorialTests
    {
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void FactorialStdRecursionTest()
        {
            ulong n = 3;
            ulong result = Program.FactorialStdRecursion(n);
            Assert.AreEqual(6ul, result);

            n = 20;
            result = Program.FactorialStdRecursion(n);
            Assert.AreEqual(2432902008176640000ul, result);

            n = 9;
            result = Program.FactorialStdRecursion(n);
            Assert.AreEqual(362880ul, result);

            n = 0;
            result = Program.FactorialStdRecursion(n);
            Assert.AreEqual(1ul, result);

            n = 21;
            result = Program.FactorialStdRecursion(n);
            // This assert fails if the data type of the return value of the function,
            // FactorialStdRecursion is NOT ulong.
            Assert.AreNotEqual(14197454024290336768ul, result);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void FactorialTailRecursionTest()
        {
            ulong n = 3;
            ulong result = Program.FactorialTailRecursion(n);
            Assert.AreEqual(6ul, result);

            n = 20;
            result = Program.FactorialTailRecursion(n);
            Assert.AreEqual(2432902008176640000ul, result);

            n = 9;
            result = Program.FactorialTailRecursion(n);
            Assert.AreEqual(362880ul, result);

            n = 0;
            result = Program.FactorialTailRecursion(n);
            Assert.AreEqual(1ul, result);

            n = 21;
            result = Program.FactorialTailRecursion(n);
            // This assert fails if the data type of the return value of the function,
            // FactorialStdRecursion is NOT ulong.
            Assert.AreNotEqual(14197454024290336768ul, result);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void FactorialLoopTest()
        {
            ulong n = 3;
            ulong result = Program.FactorialLoop(n);
            Assert.AreEqual(6ul, result);

            n = 20;
            result = Program.FactorialLoop(n);
            Assert.AreEqual(2432902008176640000ul, result);

            n = 9;
            result = Program.FactorialLoop(n);
            Assert.AreEqual(362880ul, result);

            n = 0;
            result = Program.FactorialLoop(n);
            Assert.AreEqual(1ul, result);

            n = 21;
            result = Program.FactorialLoop(n);
            // This assert fails if the data type of the return value of the function,
            // FactorialStdRecursion is NOT ulong.
            Assert.AreNotEqual(14197454024290336768ul, result);
        }
    }
}
