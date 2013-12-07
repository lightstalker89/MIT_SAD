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
    using System.Collections.Generic;
    using System.Diagnostics;

    using NSubstitute;

    using NUnit.Framework;

    /// <summary>
    /// </summary>
    [TestFixture]
    public class RecursionSampleTest
    {
        private Dictionary<long, long> factorial;

        private RecursionSample recursionSample;

        /// <summary>
        /// Set up test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.factorial = new Dictionary<long, long>
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
                    { 15, 1307674368000 }
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
                long result = this.recursionSample.CalculateFactorialNormal(i);

                Debug.WriteLine(result);

                Assert.That(result.Equals(this.factorial[i]));
            }
        }
    }
}