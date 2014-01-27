// *******************************************************
// * <copyright file="CustomAVLTree.cs" company="MDMCoWorks">
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

    #endregion

    /// <summary>
    /// </summary>
    /// <typeparam name="Type">
    /// </typeparam>
    public class CustomAVLTree<Type>
        where Type : IComparable<Type>
    {
        /// <summary>
        /// </summary>
        public CustomAVLTree()
        {
            Root = null;
        }

        /// <summary>
        /// </summary>
        private CustomAVLTreeNode<Type> Root { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetHeight(CustomAVLTreeNode<Type> node)
        {
            if (node == null)
            {
                return 0;
            }

            return Math.Max(GetHeight(node.LeftChild), GetHeight(node.RightChild)) + 1;
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsBanlance(CustomAVLTreeNode<Type> root)
        {
            if (root == null)
            {
                return true;
            }

            return Math.Abs(GetHeight(root.LeftChild) - GetHeight(root.RightChild)) < 2;
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        /// <returns>
        /// </returns>
        public bool ReBanlance(CustomAVLTreeNode<Type> root)
        {
            try
            {
                int heightDiff = GetHeight(root.LeftChild) - GetHeight(root.RightChild);
                if (heightDiff > 1)
                {
                    if (GetHeight(root.LeftChild.LeftChild) > GetHeight(root.LeftChild.RightChild))
                    {
                        RightRotation(root);
                    }
                    else
                    {
                        LeftRotation(root.LeftChild);
                        RightRotation(root);
                    }
                }
                else if (heightDiff < -1)
                {
                    if (GetHeight(root.RightChild.LeftChild) > GetHeight(root.RightChild.RightChild))
                    {
                        RightRotation(root.RightChild);
                        LeftRotation(root);
                    }
                    else
                    {
                        LeftRotation(root.RightChild);
                        RightRotation(root);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public bool RecursiveInsert(Type value)
        {
            try
            {
                RecursiveInsertNode(value, Root);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="curNode">
        /// </param>
        /// <param name="parentNode">
        /// </param>
        /// <returns>
        /// </returns>
        private bool RecursiveInsertNode(
            Type value,
            CustomAVLTreeNode<Type> curNode = null,
            CustomAVLTreeNode<Type> parentNode = null)
        {
            try
            {
                if (curNode == null)
                {
                    if (null == Root)
                    {
                        Root = new CustomAVLTreeNode<Type>(value);
                    }
                    else
                    {
                        curNode = new CustomAVLTreeNode<Type>(value);
                        curNode.Parent = parentNode;
                        if (parentNode != null)
                        {
                            if (parentNode.NodeValue.CompareTo(value) > 0)
                            {
                                parentNode.LeftChild = curNode;
                            }
                            else
                            {
                                parentNode.RightChild = curNode;
                            }
                        }

                        if (parentNode != null && this.IsBanlance(parentNode.Parent) == false)
                        {
                            ReBanlance(parentNode.Parent);
                        }
                    }
                }
                else if (curNode.NodeValue.CompareTo(value) > 0)
                {
                    RecursiveInsertNode(value, curNode.LeftChild, curNode);
                }
                else
                {
                    RecursiveInsertNode(value, curNode.RightChild, curNode);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        /// <returns>
        /// </returns>
        public bool LeftRotation(CustomAVLTreeNode<Type> root)
        {
            try
            {
                CustomAVLTreeNode<Type> rootRight = root.RightChild;
                root.RightChild = rootRight.LeftChild;
                if (rootRight.LeftChild != null)
                {
                    rootRight.LeftChild.Parent = root;
                }

                rootRight.Parent = root.Parent;
                if (root.Parent == null)
                {
                    Root = rootRight;
                }
                else if (root.Parent.LeftChild == root)
                {
                    root.Parent.LeftChild = rootRight;
                }
                else
                {
                    root.Parent.RightChild = rootRight;
                }

                rootRight.LeftChild = root;
                root.Parent = rootRight;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        /// <returns>
        /// </returns>
        public bool RightRotation(CustomAVLTreeNode<Type> root)
        {
            try
            {
                CustomAVLTreeNode<Type> rootLeft = root.LeftChild;
                root.LeftChild = rootLeft.RightChild;
                if (rootLeft.RightChild != null)
                {
                    rootLeft.RightChild.Parent = root;
                }

                rootLeft.Parent = Root.Parent;

                if (Root.Parent == null)
                {
                    Root = rootLeft;
                }
                else if (root.Parent.LeftChild == root)
                {
                    root.Parent.LeftChild = rootLeft;
                }
                else
                {
                    root.Parent.RightChild = rootLeft;
                }

                rootLeft.RightChild = root;
                root.Parent = rootLeft;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        private void InOrderTraversalForNode(CustomAVLTreeNode<Type> node)
        {
            if (node == null)
            {
                return;
            }

            InOrderTraversalForNode(node.LeftChild);
            Console.WriteLine(node.ToString());
            InOrderTraversalForNode(node.RightChild);
        }

        /// <summary>
        /// </summary>
        public void InOrderTraversal()
        {
            InOrderTraversalForNode(Root);
        }
    }
}