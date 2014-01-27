// *******************************************************
// * <copyright file="Tree.cs" company="MDMCoWorks">
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
    public class Tree
    {
        /// <summary>
        /// </summary>
        public Node Root { get; set; }

        /// <summary>
        /// </summary>
        public Tree()
        {
            Root = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        public void Insert(int number)
        {
            Node newNode = new Node { Item = number };

            if (this.Root == null)
            {
                this.Root = newNode;
            }
            else
            {
                Node current = this.Root;

                while (true)
                {
                    Node parent = current;

                    if (number < current.Item)
                    {
                        current = current.LeftChild;

                        if (current == null)
                        {
                            parent.LeftChild = newNode;
                            return;
                        }
                    }
                    else
                    {
                        current = current.RightChild;
                        if (current == null)
                        {
                            parent.RightChild = newNode;
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        public void Preorder(Node root)
        {
            if (root != null)
            {
                Console.Write(root.Item + " ");
                Preorder(root.LeftChild);
                Preorder(root.RightChild);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        public void Inorder(Node root)
        {
            if (root != null)
            {
                Inorder(root.LeftChild);
                Console.Write(root.Item + " ");
                Inorder(root.RightChild);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        public void Postorder(Node root)
        {
            if (root != null)
            {
                Postorder(root.LeftChild);
                Postorder(root.RightChild);
                Console.Write(root.Item + " ");
            }
        }
    }
}