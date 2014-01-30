// *******************************************************
// * <copyright file="CustomBinaryTree.cs" company="MDMCoWorks">
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
    public class CustomBinaryTree
    {

        /// <summary>
        /// </summary>
        public Node Root { get; set; }

        /// <summary>
        /// </summary>
        public CustomBinaryTree()
        {
            this.Root = null;
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
        /// <param name="node">
        /// </param>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Search(Node node, int key)
        {
            if (node == null)
            {
                return false;
            }

            if (key < node.Item)
            {
                return this.Search(node.LeftChild, key);
            }

            return this.Search(node.RightChild, key);
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
                this.Preorder(root.LeftChild);
                this.Preorder(root.RightChild);
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
                this.Inorder(root.LeftChild);
                Console.Write(root.Item + " ");
                this.Inorder(root.RightChild);
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
                this.Postorder(root.LeftChild);
                this.Postorder(root.RightChild);
                Console.Write(root.Item + " ");
            }
        }
    }
}