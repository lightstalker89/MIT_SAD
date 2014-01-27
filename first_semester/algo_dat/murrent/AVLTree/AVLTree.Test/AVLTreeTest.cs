// *******************************************************
// * <copyright file="AVLTreeTest.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace AVLTree.Test
{
    #region Usings

    using System;

    using NUnit.Framework;

    #endregion

    /// <summary>
    /// </summary>
    public class AVLTreeTest
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly int[] numbers = new int[20];

        /// <summary>
        /// </summary>
        private Random random;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.random = new Random();

            for (int i = 0; i < this.numbers.Length; i++)
            {
                this.numbers[i] = this.random.Next(100000);
            }
        }

        /// <summary>
        /// </summary>
        public void InsertTest()
        {
            foreach (int number in this.numbers)
            {

            }
        }

        #endregion
    }
}