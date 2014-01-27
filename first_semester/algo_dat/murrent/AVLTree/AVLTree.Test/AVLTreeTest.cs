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
        /// <summary>
        /// </summary>
        private CustomAVLTree<int> customAVLTree;

        /// <summary>
        /// </summary>
        private Random random;

        /// <summary>
        /// </summary>
        private readonly int[] numbers = new int[20];

        /// <summary>
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.random = new Random();
            this.customAVLTree = new CustomAVLTree<int>();

            for (int i = 0; i < numbers.Length; i++)
            {
                this.numbers[i] = random.Next(100000);
            }
        }

        /// <summary>
        /// </summary>
        public void InsertTest()
        {
            foreach (int number in numbers)
            {
                if (this.customAVLTree.RecursiveInsert(number))
                {
                    
                }
                else
                {
                    
                }
            }
        }
    }
}