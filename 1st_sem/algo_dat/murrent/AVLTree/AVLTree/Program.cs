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
    #region Usings

    using System;
    using System.Diagnostics;

    using SortHelper;

    #endregion

    /// <summary>
    /// </summary>
    public class Program
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static readonly CustomAVLTree<int, int> CustomAVLTree = new CustomAVLTree<int, int>();

        /// <summary>
        /// </summary>
        private static CArray numbers;

        /// <summary>
        /// </summary>
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            numbers = new CArray(50, 100000);

            Console.WriteLine("Building tree with unsorted array");

            Stopwatch.Start();

            foreach (int number in numbers.NumberArray)
            {
                CustomAVLTree.Insert(number, number);
            }

            Stopwatch.Stop();

            Console.WriteLine("Time needed for building and inserting: " + Stopwatch.ElapsedMilliseconds + "ms - Ticks: " + Stopwatch.ElapsedTicks);

            Stopwatch.Reset();

            // Console.WriteLine("Inorder traversal:");
            // CustomAVLTree.InOrder(CustomAVLTree.Root);
            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("Postorder traversal:");
            // CustomAVLTree.PostOrder(CustomAVLTree.Root);
            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("Preorder traversal:");
            // CustomAVLTree.PreOrder(CustomAVLTree.Root);
            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("Displaying Tree:");
            // CustomAVLTree.DisplayTree(CustomAVLTree.Root);
            Console.WriteLine("Building tree with sorted array");

            numbers = new CArray(50, 100000);

            Stopwatch.Start();

            foreach (int number in numbers.ArraySorted)
            {
                CustomAVLTree.Insert(number, number);
            }

            Stopwatch.Stop();

            Console.WriteLine("Time needed for building and inserting: " + Stopwatch.ElapsedMilliseconds + "ms - Ticks: " + Stopwatch.ElapsedTicks);
            Console.WriteLine();
            CustomAVLTree.DisplayTree(CustomAVLTree.Root, string.Empty);
            Stopwatch.Reset();

            // Console.WriteLine("Inorder traversal:");
            // CustomAVLTree.InOrder(CustomAVLTree.Root);
            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("Postorder traversal:");
            // CustomAVLTree.PostOrder(CustomAVLTree.Root);
            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("Preorder traversal:");
            // CustomAVLTree.PreOrder(CustomAVLTree.Root);
            // Console.WriteLine();
            // Console.WriteLine();
            // Console.WriteLine("Displaying Tree:");
            // CustomAVLTree.DisplayTree(CustomAVLTree.Root);

            Console.ReadKey();
        }

        #endregion
    }
}