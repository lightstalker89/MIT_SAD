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
    using System;
    using System.Collections.Generic;
    using System.Collections;

    public class CustomAVLTree<TKey, TValue> : IEnumerable<TValue>
    {
        /// <summary>
        /// </summary>
        /// <param name="root">
        /// </param>
        public void PreOrder(CustomAVLNode root)
        {
            if (root != null)
            {
                Console.Write(root.Value + " ");
                PreOrder(root.Left);
                PreOrder(root.Right);
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
                InOrder(root.Left);
                Console.Write(root.Value + " ");
                InOrder(root.Right);
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
                PostOrder(root.Left);
                PostOrder(root.Right);
                Console.Write(root.Value + " ");
            }
        }

        private readonly IComparer<TKey> comparer;
        public CustomAVLNode Root { get; set; }

        public CustomAVLTree(IComparer<TKey> comparer)
        {
            this.comparer = comparer;
        }

        public CustomAVLTree()
            : this(Comparer<TKey>.Default)
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return new AvlNodeEnumerator(this.Root);
        }

        public void Clear()
        {
            this.Root = null;
        }

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

                            InsertBalance(node, 1);

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

                            InsertBalance(node, -1);

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

        private void InsertBalance(CustomAVLNode node, int balance)
        {
            while (node != null)
            {
                balance = (node.Balance += balance);

                if (balance == 0)
                {
                    return;
                }
                
                if (balance == 2)
                {
                    if (node.Left.Balance == 1)
                    {
                        RotateRight(node);
                    }
                    else
                    {
                        RotateLeftRight(node);
                    }

                    return;
                }
                
                if (balance == -2)
                {
                    if (node.Right.Balance == -1)
                    {
                        RotateLeft(node);
                    }
                    else
                    {
                        RotateRightLeft(node);
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

                                    DeleteBalance(parent, -1);
                                }
                                else
                                {
                                    parent.Right = null;

                                    DeleteBalance(parent, 1);
                                }
                            }
                        }
                        else
                        {
                            Replace(node, right);

                            DeleteBalance(node, 0);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);

                        DeleteBalance(node, 0);
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

                            DeleteBalance(successor, 1);
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

                            DeleteBalance(successorParent, -1);
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        private void DeleteBalance(CustomAVLNode node, int balance)
        {
            while (node != null)
            {
                balance = (node.Balance += balance);

                if (balance == 2)
                {
                    if (node.Left.Balance >= 0)
                    {
                        node = RotateRight(node);

                        if (node.Balance == -1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        node = RotateLeftRight(node);
                    }
                }
                else if (balance == -2)
                {
                    if (node.Right.Balance <= 0)
                    {
                        node = RotateLeft(node);

                        if (node.Balance == 1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        node = RotateRightLeft(node);
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

        public class CustomAVLNode
        {
            public CustomAVLNode Parent;
            public CustomAVLNode Left;
            public CustomAVLNode Right;
            public TKey Key;
            public TValue Value;
            public int Balance;
        }

        public class AvlNodeEnumerator : IEnumerator<TValue>
        {
            private readonly CustomAVLNode root;
            private Action action;
            private CustomAVLNode current;
            private CustomAVLNode right;

            public AvlNodeEnumerator(CustomAVLNode root)
            {
                this.right = this.root = root;

                this.action = root == null ? Action.End : Action.Right;
            }

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

            public void Reset()
            {
                this.right = this.root;

                this.action = this.root == null ? Action.End : Action.Right;
            }

            public TValue Current
            {
                get
                {
                    return this.current.Value;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public void Dispose()
            {

            }

            enum Action
            {
                Parent,
                Right,
                End
            }
        }
    }

}