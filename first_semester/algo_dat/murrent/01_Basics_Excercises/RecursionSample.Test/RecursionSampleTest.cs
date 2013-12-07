// *******************************************************
// * <copyright file="RecursionSampleTest.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace RecursionSample.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Numerics;

    using NSubstitute;

    using NUnit.Framework;

    /// <summary>
    /// The <see ref="RecursionSampleTest"/> class and its interaction logic 
    /// </summary>
    [TestFixture]
    public class RecursionSampleTest
    {
        /// <summary>
        /// </summary>
        private Dictionary<long, BigInteger> factorial;

        /// <summary>
        /// The recursion sample instance
        /// </summary>
        private RecursionSample recursionSample;

        /// <summary>
        /// Set up test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.factorial = new Dictionary<long, BigInteger>
                {
                    { 0, 1 }, 
                    { 1, 1 }, 
                    { 2, 2 }, 
                    { 3, 6 }, 
                    { 4, 24 }, 
                    { 5, 120 }, 
                    { 6, 720 }, 
                    { 7, 5040 }, 
                    { 8, 40320 }, 
                    { 9, 362880 }, 
                    { 10, 3628800 }, 
                    { 11, 39916800 }, 
                    { 12, 479001600 }, 
                    { 13, 6227020800 }, 
                    { 14, 87178291200 }, 
                    { 15, 1307674368000 }, 
                    { 16, 20922789888000 }, 
                    { 17, 355687428096000 }, 
                    { 18, 6402373705728000 }, 
                    { 19, 121645100408832000 }, 
                    { 20, 2432902008176640000 },
                    { 21, BigInteger.Parse("51090942171709440000")},
                    { 22, BigInteger.Parse("1124000727777607680000")},
                    { 23, BigInteger.Parse("25852016738884976640000")},
                    { 24, BigInteger.Parse("620448401733239439360000")},
                    { 25, BigInteger.Parse("15511210043330985984000000")},
                    { 26, BigInteger.Parse("403291461126605635584000000")},
                    { 27, BigInteger.Parse("10888869450418352160768000000")},
                    { 28, BigInteger.Parse("304888344611713860501504000000")},
                    { 29, BigInteger.Parse("8841761993739701954543616000000")},
                    { 30, BigInteger.Parse("265252859812191058636308480000000")},
                };

            this.recursionSample = Substitute.For<RecursionSample>();
        }

        /// <summary>
        /// Test the calculate factorial method without recursion
        /// </summary>
        [TestCase]
        public void CalculateFactorialNormalTest()
        {
            for (int i = 0; i < this.factorial.Count; ++i)
            {
                BigInteger result = this.recursionSample.CalculateFactorialNormal(i);

                Debug.WriteLine(result);

                Assert.That(result.Equals(this.factorial[i]));
            }
        }

        /// <summary>
        /// Test the calculate factorial method with invalid numbers
        /// </summary>
        [TestCase]
        public void CalculateFactorialWithNegativeNumber()
        {
            for (int i = -1; i > -100; --i)
            {
                Assert.Throws<ArgumentException>(() => this.recursionSample.CalculateFactorialNormal(i));
            }
        }

        ///// <summary>
        ///// Test the calculate factorial method with to big numbers
        ///// </summary>
        //[TestCase]
        //public void CalculateFactorialWithBigNumber()
        //{
        //    BigInteger result = this.recursionSample.CalculateFactorialNormal(1000);

        //    Assert.That(result.Equals(0) || result < 0);
        //}

        /// <summary>
        /// Test the calculate factorial method with recursion
        /// </summary>
        [TestCase]
        public void CalculateFactorialWithRecursionTest()
        {
            for (int i = 0; i < 29; ++i)
            {
                BigInteger result = this.recursionSample.CalculateFactorialWithRecursion(i);

                Debug.WriteLine(result);

                Assert.That(result.Equals(this.factorial[i]));
            }
        }

        /// <summary>
        /// Test the calculate factorial method with tail recursion
        /// </summary>
        [TestCase]
        public void CalculateFactorialWithTailRecursionTest()
        {
            for (int i = 0; i < 29; ++i)
            {
                BigInteger result = this.recursionSample.CalculateFactorialWithTailRecursion(i, 1);

                Debug.WriteLine(result);

                Assert.That(result.Equals(this.factorial[i]));
            }
        }
    }
}