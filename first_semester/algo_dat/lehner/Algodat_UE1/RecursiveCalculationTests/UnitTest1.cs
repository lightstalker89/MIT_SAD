using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursiveCalculation;
using System.Diagnostics;

namespace RecursiveCalculationTests
{
    [TestClass]
    public class FactorialTests
    {
        //factorials until 20
        private long[] CheckArray = {1,
            1,
            2,
            6,
            24,
            120,
            720,
            5040,
            40320,
            362880,
            3628800,
            39916800,
            479001600,
            6227020800,
            87178291200,
            1307674368000,
            20922789888000,
            355687428096000,    
            6402373705728000,
            121645100408832000,
            2432902008176640000
            //51090942171709440000,
            //1124000727777607680000,
            //25852016738884976640000
                                };

        [TestMethod]
        public void TestFactorialCalcNoRecursion()
        {
            FactorialCalc calc = new FactorialCalc();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < this.CheckArray.Length; ++i)
            {
                long value = calc.Calc(i);
                Assert.AreEqual(value, this.CheckArray[i]);
            }

            sw.Stop();
            Console.WriteLine("Time Stopwatch No Recursion: " + sw.Elapsed.ToString()
                + " msec " + sw.ElapsedMilliseconds.ToString()
                + " ticks " + sw.ElapsedTicks.ToString());

        }

        [TestMethod]
        public void TestFactorialCalcRecursion()
        {
            FactorialCalc calc = new FactorialCalc();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < this.CheckArray.Length; ++i)
            {
                long value = calc.CalcRecursive(i);
                Assert.AreEqual(value, this.CheckArray[i]);
            }

            sw.Stop();

            Console.WriteLine("Time Stopwatch Recursion: " + sw.Elapsed.ToString()
            + " msec " + sw.ElapsedMilliseconds.ToString()
            + " ticks " + sw.ElapsedTicks.ToString());

        }

        [TestMethod]
        public void TestFactorialCalcTailRecursion()
        {
            FactorialCalc calc = new FactorialCalc();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < this.CheckArray.Length; ++i)
            {
                long value = calc.CalcTailRecursive(i, 1);
                Assert.AreEqual(value, this.CheckArray[i]);
            }

            sw.Stop();

            Console.WriteLine("Time Stopwatch Tail Recursion: " + sw.Elapsed.ToString()
            + " msec " + sw.ElapsedMilliseconds.ToString()
            + " ticks " + sw.ElapsedTicks.ToString());

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NegativeNumbersCheck()
        {
            FactorialCalc calc = new FactorialCalc();
            calc.Calc(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NegativeNumbersCheckRecursive()
        {
            FactorialCalc calc = new FactorialCalc();
            calc.CalcRecursive(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NegativeNumbersCheckTailRecursive()
        {
            FactorialCalc calc = new FactorialCalc();
            calc.CalcTailRecursive(-1,1);
        }

        [TestMethod]
        public void MaximumValueCheck()
        {
            FactorialCalc calc = new FactorialCalc();
            long result = calc.Calc(21);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void MaximumValueCheckRecursive()
        {
            FactorialCalc calc = new FactorialCalc();
            long result = calc.CalcRecursive(21);
            Assert.IsFalse(result > 0);
        }

        [TestMethod]
        public void MaximumValueCheckTailRecursive()
        {
            FactorialCalc calc = new FactorialCalc();
            long result = calc.CalcTailRecursive(21,1);
            Assert.IsFalse(result > 0);
        }
    }
}
