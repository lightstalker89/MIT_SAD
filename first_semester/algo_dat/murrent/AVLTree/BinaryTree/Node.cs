// *******************************************************
// * <copyright file="Node.cs" company="MDMCoWorks">
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
    public class Node
    {
        /// <summary>
        /// </summary>
        public int Item { get; set; }

        /// <summary>
        /// </summary>
        public Node LeftChild { get; set; }

        /// <summary>
        /// </summary>
        public Node RightChild { get; set; }

        /// <summary>
        /// </summary>
        public void DisplayNode()
        {
            Console.Write("[");
            Console.Write(this.Item);
            Console.Write("]");
        }
    }
}