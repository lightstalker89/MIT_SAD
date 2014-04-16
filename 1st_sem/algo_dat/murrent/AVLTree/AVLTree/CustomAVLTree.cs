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
    using System.Collections;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// </summary>
    /// <typeparam name="TKey">
    /// </typeparam>
    /// <typeparam name="TValue">
    /// </typeparam>
    public class CustomAVLTree<TKey, TValue> : IEnumerable<TValue>
    {

        /// <summary>
        /// </summary>
        private readonly IComparer<TKey> comparer;

        /// <summary>
        /// </summary>
        public CustomAVLNode Root { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="comparer">
        /// </param>
        public CustomAVLTree(IComparer<TKey> comparer)
        {
            this.comparer = comparer;
        }

        /// <summary>
        /// </summary>
        public CustomAVLTree()
            : this(Comparer<TKey>.Default)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        public void PreOrder(CustomAVLNode root)
        {
            if (root != null)
            {
                Console.Write(root.Value + " ");
                this.PreOrder(root.Left);
                this.PreOrder(root.Right);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        public void InOrder(CustomAVLNode root)
        {
            if (root != null)
            {
                this.InOrder(root.Left);
                Console.Write(root.Value + " ");
                this.InOrder(root.Right);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        public void PostOrder(CustomAVLNode root)
        {
            if (root != null)
            {
                this.PostOrder(root.Left);
                this.PostOrder(root.Right);
                Console.Write(root.Value + " ");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        /// <param name="prefix">
        /// </param>
        public void DisplayTree(CustomAVLNode root, string prefix)
        {
            if (root == null)
            {
                Console.WriteLine(prefix + "+- <null>");
                return;
            }

            Console.WriteLine(prefix + "+- " + root.Key);
            this.DisplayTree(root.Left, prefix + "|  ");
            this.DisplayTree(root.Right, prefix + "|  ");
        }

        /// <summary>
        /// </summary>
        public void DisplayBreadthFirst()
        {
            Queue<CustomAVLNode> breadthFirstQueue = new Queue<CustomAVLNode>();
            breadthFirstQueue.Enqueue(this.Root);

            while (breadthFirstQueue.Count > 0)
            {
                CustomAVLNode node = breadthFirstQueue.Dequeue();
                Console.Write(node.Value + " ");

                if (node.Left != null)
                {
                    breadthFirstQueue.Enqueue(node.Left);
                }

                if (node.Right != null)
                {
                    breadthFirstQueue.Enqueue(node.Right);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            return new AvlNodeEnumerator(this.Root);
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            this.Root = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Search(TKey key, out TValue value)
        {
            CustomAVLNode node = this.Root;

            while (node != null)
            {
                if (this.comparer.Compare(key, node.Key) < 0)
                {
                    node = node.Left;
                }
                else if (this.comparer.Compare(key, node.Key) > 0)
                {
                    node = node.Right;
                }
                else
                {
                    value = node.Value;

                    return true;
                }
            }

            value = default(TValue);

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="value">
        /// </param>
        public void Insert(TKey key, TValue value)
        {
            if (this.Root == null)
            {
                this.Root = new CustomAVLNode { Key = key, Value = value };
            }
            else
            {
                CustomAVLNode node = this.Root;

                while (node != null)
                {
                    int compare = this.comparer.Compare(key, node.Key);

                    if (compare < 0)
                    {
                        CustomAVLNode left = node.Left;

                        if (left == null)
                        {
                            node.Left = new CustomAVLNode { Key = key, Value = value, Parent = node };

                            this.InsertBalance(node, 1);

                            return;
                        }

                        node = left;
                    }
                    else if (compare > 0)
                    {
                        CustomAVLNode right = node.Right;

                        if (right == null)
                        {
                            node.Right = new CustomAVLNode { Key = key, Value = value, Parent = node };

                            this.InsertBalance(node, -1);

                            return;
                        }

                        node = right;
                    }
                    else
                    {
                        node.Value = value;

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <param name="balance">
        /// </param>
        private void InsertBalance(CustomAVLNode node, int balance)
        {
            while (node != null)
            {
                balance = node.Balance += balance;

                if (balance == 0)
                {
                    return;
                }

                if (balance == 2)
                {
                    if (node.Left.Balance == 1)
                    {
                        this.RotateRight(node);
                    }
                    else
                    {
                        this.RotateLeftRight(node);
                    }

                    return;
                }

                if (balance == -2)
                {
                    if (node.Right.Balance == -1)
                    {
                        this.RotateLeft(node);
                    }
                    else
                    {
                        this.RotateRightLeft(node);
                    }

                    return;
                }

                CustomAVLNode parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? 1 : -1;
                }

                node = parent;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Delete(TKey key)
        {
            CustomAVLNode node = this.Root;

            while (node != null)
            {
                if (this.comparer.Compare(key, node.Key) < 0)
                {
                    node = node.Left;
                }
                else if (this.comparer.Compare(key, node.Key) > 0)
                {
                    node = node.Right;
                }
                else
                {
                    CustomAVLNode left = node.Left;
                    CustomAVLNode right = node.Right;

                    if (left == null)
                    {
                        if (right == null)
                        {
                            if (node == this.Root)
                            {
                                this.Root = null;
                            }
                            else
                            {
                                CustomAVLNode parent = node.Parent;

                                if (parent.Left == node)
                                {
                                    parent.Left = null;

                                    this.DeleteBalance(parent, -1);
                                }
                                else
                                {
                                    parent.Right = null;

                                    this.DeleteBalance(parent, 1);
                                }
                            }
                        }
                        else
                        {
                            Replace(node, right);

                            this.DeleteBalance(node, 0);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);

                        this.DeleteBalance(node, 0);
                    }
                    else
                    {
                        CustomAVLNode successor = right;

                        if (successor.Left == null)
                        {
                            CustomAVLNode parent = node.Parent;

                            successor.Parent = parent;
                            successor.Left = left;
                            successor.Balance = node.Balance;

                            left.Parent = successor;

                            if (node == this.Root)
                            {
                                this.Root = successor;
                            }
                            else
                            {
                                if (parent.Left == node)
                                {
                                    parent.Left = successor;
                                }
                                else
                                {
                                    parent.Right = successor;
                                }
                            }

                            this.DeleteBalance(successor, 1);
                        }
                        else
                        {
                            while (successor.Left != null)
                            {
                                successor = successor.Left;
                            }

                            CustomAVLNode parent = node.Parent;
                            CustomAVLNode successorParent = successor.Parent;
                            CustomAVLNode successorRight = successor.Right;

                            if (successorParent.Left == successor)
                            {
                                successorParent.Left = successorRight;
                            }
                            else
                            {
                                successorParent.Right = successorRight;
                            }

                            if (successorRight != null)
                            {
                                successorRight.Parent = successorParent;
                            }

                            successor.Parent = parent;
                            successor.Left = left;
                            successor.Balance = node.Balance;
                            successor.Right = right;
                            right.Parent = successor;

                            left.Parent = successor;

                            if (node == this.Root)
                            {
                                this.Root = successor;
                            }
                            else
                            {
                                if (parent.Left == node)
                                {
                                    parent.Left = successor;
                                }
                                else
                                {
                                    parent.Right = successor;
                                }
                            }

                            this.DeleteBalance(successorParent, -1);
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <param name="balance">
        /// </param>
        private void DeleteBalance(CustomAVLNode node, int balance)
        {
            while (node != null)
            {
                balance = node.Balance += balance;

                if (balance == 2)
                {
                    if (node.Left.Balance >= 0)
                    {
                        node = this.RotateRight(node);

                        if (node.Balance == -1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        node = this.RotateLeftRight(node);
                    }
                }
                else if (balance == -2)
                {
                    if (node.Right.Balance <= 0)
                    {
                        node = this.RotateLeft(node);

                        if (node.Balance == 1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        node = this.RotateRightLeft(node);
                    }
                }
                else if (balance != 0)
                {
                    return;
                }

                CustomAVLNode parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? -1 : 1;
                }

                node = parent;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <returns>
        /// </returns>
        private CustomAVLNode RotateLeft(CustomAVLNode node)
        {
            CustomAVLNode right = node.Right;
            CustomAVLNode rightLeft = right.Left;
            CustomAVLNode parent = node.Parent;

            right.Parent = parent;
            right.Left = node;
            node.Right = rightLeft;
            node.Parent = right;

            if (rightLeft != null)
            {
                rightLeft.Parent = node;
            }

            if (node == this.Root)
            {
                this.Root = right;
            }
            else if (parent.Right == node)
            {
                parent.Right = right;
            }
            else
            {
                parent.Left = right;
            }

            right.Balance++;
            node.Balance = -right.Balance;

            return right;
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <returns>
        /// </returns>
        private CustomAVLNode RotateRight(CustomAVLNode node)
        {
            CustomAVLNode left = node.Left;
            CustomAVLNode leftRight = left.Right;
            CustomAVLNode parent = node.Parent;

            left.Parent = parent;
            left.Right = node;
            node.Left = leftRight;
            node.Parent = left;

            if (leftRight != null)
            {
                leftRight.Parent = node;
            }

            if (node == this.Root)
            {
                this.Root = left;
            }
            else if (parent.Left == node)
            {
                parent.Left = left;
            }
            else
            {
                parent.Right = left;
            }

            left.Balance--;
            node.Balance = -left.Balance;

            return left;
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <returns>
        /// </returns>
        private CustomAVLNode RotateLeftRight(CustomAVLNode node)
        {
            CustomAVLNode left = node.Left;
            CustomAVLNode leftRight = left.Right;
            CustomAVLNode parent = node.Parent;
            CustomAVLNode leftRightRight = leftRight.Right;
            CustomAVLNode leftRightLeft = leftRight.Left;

            leftRight.Parent = parent;
            node.Left = leftRightRight;
            left.Right = leftRightLeft;
            leftRight.Left = left;
            leftRight.Right = node;
            left.Parent = leftRight;
            node.Parent = leftRight;

            if (leftRightRight != null)
            {
                leftRightRight.Parent = node;
            }

            if (leftRightLeft != null)
            {
                leftRightLeft.Parent = left;
            }

            if (node == this.Root)
            {
                this.Root = leftRight;
            }
            else if (parent.Left == node)
            {
                parent.Left = leftRight;
            }
            else
            {
                parent.Right = leftRight;
            }

            if (leftRight.Balance == -1)
            {
                node.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                node.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                node.Balance = -1;
                left.Balance = 0;
            }

            leftRight.Balance = 0;

            return leftRight;
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        /// <returns>
        /// </returns>
        private CustomAVLNode RotateRightLeft(CustomAVLNode node)
        {
            CustomAVLNode right = node.Right;
            CustomAVLNode rightLeft = right.Left;
            CustomAVLNode parent = node.Parent;
            CustomAVLNode rightLeftLeft = rightLeft.Left;
            CustomAVLNode rightLeftRight = rightLeft.Right;

            rightLeft.Parent = parent;
            node.Right = rightLeftLeft;
            right.Left = rightLeftRight;
            rightLeft.Right = right;
            rightLeft.Left = node;
            right.Parent = rightLeft;
            node.Parent = rightLeft;

            if (rightLeftLeft != null)
            {
                rightLeftLeft.Parent = node;
            }

            if (rightLeftRight != null)
            {
                rightLeftRight.Parent = right;
            }

            if (node == this.Root)
            {
                this.Root = rightLeft;
            }
            else if (parent.Right == node)
            {
                parent.Right = rightLeft;
            }
            else
            {
                parent.Left = rightLeft;
            }

            if (rightLeft.Balance == 1)
            {
                node.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                node.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                node.Balance = 1;
                right.Balance = 0;
            }

            rightLeft.Balance = 0;

            return rightLeft;
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <param name="source">
        /// </param>
        private static void Replace(CustomAVLNode target, CustomAVLNode source)
        {
            CustomAVLNode left = source.Left;
            CustomAVLNode right = source.Right;

            target.Balance = source.Balance;
            target.Key = source.Key;
            target.Value = source.Value;
            target.Left = left;
            target.Right = right;

            if (left != null)
            {
                left.Parent = target;
            }

            if (right != null)
            {
                right.Parent = target;
            }
        }

        /// <summary>
        /// </summary>
        public class CustomAVLNode
        {
            /// <summary>
            /// </summary>
            public CustomAVLNode Parent;

            /// <summary>
            /// </summary>
            public CustomAVLNode Left;

            /// <summary>
            /// </summary>
            public CustomAVLNode Right;

            /// <summary>
            /// </summary>
            public TKey Key;

            /// <summary>
            /// </summary>
            public TValue Value;

            /// <summary>
            /// </summary>
            public int Balance;
        }

        /// <summary>
        /// </summary>
        public class AvlNodeEnumerator : IEnumerator<TValue>
        {
            /// <summary>
            /// </summary>
            private readonly CustomAVLNode root;

            /// <summary>
            /// </summary>
            private Action action;

            /// <summary>
            /// </summary>
            private CustomAVLNode current;

            /// <summary>
            /// </summary>
            private CustomAVLNode right;

            /// <summary>
            /// </summary>
            /// <param name="root">
            /// </param>
            public AvlNodeEnumerator(CustomAVLNode root)
            {
                this.right = this.root = root;

                this.action = root == null ? Action.End : Action.Right;
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public bool MoveNext()
            {
                switch (this.action)
                {
                    case Action.Right:
                        this.current = this.right;

                        while (this.current.Left != null)
                        {
                            this.current = this.current.Left;
                        }

                        this.right = this.current.Right;

                        this.action = this.right != null ? Action.Right : Action.Parent;

                        return true;
                    case Action.Parent:
                        while (this.current.Parent != null)
                        {
                            CustomAVLNode previous = this.current;

                            this.current = this.current.Parent;

                            if (this.current.Left == previous)
                            {
                                this.right = this.current.Right;

                                this.action = this.right != null ? Action.Right : Action.Parent;

                                return true;
                            }
                        }

                        this.action = Action.End;

                        return false;
                    default:
                        return false;
                }
            }

            /// <summary>
            /// </summary>
            public void Reset()
            {
                this.right = this.root;

                this.action = this.root == null ? Action.End : Action.Right;
            }

            /// <summary>
            /// </summary>
            public TValue Current
            {
                get
                {
                    return this.current.Value;
                }
            }

            /// <summary>
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            /// <summary>
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// </summary>
            private enum Action
            {
                /// <summary>
                /// </summary>
                Parent,

                /// <summary>
                /// </summary>
                Right,

                /// <summary>
                /// </summary>
                End
            }
        }
    }
}