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
    using System;

    /// <summary>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// </summary>
        private static readonly Tree tree = new Tree();

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
                tree.Insert(random.Next(10000));
            }
            Console.WriteLine("Inorder traversal");
            tree.Inorder(tree.Root);
            Console.WriteLine(" ");

            Console.WriteLine();
            Console.WriteLine("Preorder traversal:");
            tree.Preorder(tree.Root);
            Console.WriteLine(" ");

            Console.WriteLine();
            Console.WriteLine("Postorder traversal:");
            tree.Postorder(tree.Root);
            Console.WriteLine(" ");

            Console.ReadLine();
        }
    }
}