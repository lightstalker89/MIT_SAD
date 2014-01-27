// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace AVLTree
{
    using System;

    using SortHelper;

    /// <summary>
    /// </summary>
    public class Program
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static readonly int[] TreeValueCount = { 100, 10000 };

        /// <summary>
        /// </summary>
        private static readonly CustomAVLTree<int, int> CustomAVLTree = new CustomAVLTree<int, int>();

        /// <summary>
        /// </summary>
        private static CArray numbers;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            numbers = new CArray(1000, 100000);

            foreach (int number in numbers.NumberArray)
            {
                CustomAVLTree.Insert(number, number);
            }

            Console.WriteLine("Inorder traversal:");
            CustomAVLTree.InOrder(CustomAVLTree.Root);

            Console.WriteLine();
            Console.WriteLine("Postorder traversal:");
            CustomAVLTree.PostOrder(CustomAVLTree.Root);

            Console.WriteLine();
            Console.WriteLine("Preorder traversal:");
            CustomAVLTree.PreOrder(CustomAVLTree.Root);

            Console.ReadKey();
        }

        #endregion
    }
}