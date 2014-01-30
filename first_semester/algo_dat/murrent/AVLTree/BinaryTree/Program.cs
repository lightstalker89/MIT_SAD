// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BinaryTree
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// </summary>
        private static readonly CustomBinaryTree CustomBinaryTree = new CustomBinaryTree();

        /// <summary>
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            for (int i = 0; i < 50; i++)
            {
                CustomBinaryTree.Insert(random.Next(10000));
            }

            Console.WriteLine("Inorder traversal");
            CustomBinaryTree.Inorder(CustomBinaryTree.Root);
            Console.WriteLine(" ");

            Console.WriteLine();
            Console.WriteLine("Preorder traversal:");
            CustomBinaryTree.Preorder(CustomBinaryTree.Root);
            Console.WriteLine(" ");

            Console.WriteLine();
            Console.WriteLine("Postorder traversal:");
            CustomBinaryTree.Postorder(CustomBinaryTree.Root);
            Console.WriteLine(" ");

            Console.ReadLine();
        }
    }
}