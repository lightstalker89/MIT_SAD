using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspAVL
{
    public class AVLTree
    {
        Node root;

        public AVLTree()
        {
            this.root = null;
        }

        public Node Root
        {
            get
            {
                return this.root;
            }
        }

        public void Insert(int value)
        {
            Node newNode = new Node(value);

            InsertNode(newNode, ref this.root);
        }

        private int InsertNode(Node n, ref Node current)
        {
            int depthValue = 0;

            if (current == null)
            {
                current = n;
                depthValue = 1;
            }
            else if (n.Value < current.Value)
            {
                Node left = current.Left;
                depthValue = InsertNode(n, ref left);

                //if (current.Left == null &&
                //    left != null &&
                //    current.Right != null)
                //{
                //    ++current.LeftDepth;
                //    depthValue = 0;
                //}
                /*else */
                if (depthValue == 1 &&
                    current.LeftDepth < current.RightDepth)
                {
                    ++current.LeftDepth;
                    depthValue = 0;
                }

                current.Left = left;
                current.LeftDepth += depthValue;

                if (LeftLeftRotation(ref current))
                {
                    depthValue = 0;
                }

                if (LeftRightRotation(ref current))
                {
                    depthValue = 0;
                }
            }
            else if (n.Value >= current.Value)
            {
                Node right = current.Right;
                depthValue = InsertNode(n, ref right);

                //if (current.Right == null &&
                //    right != null &&
                //    current.Left != null)
                //{
                //    ++current.RightDepth;
                //    depthValue = 0;
                //}
                /*else */
                if (depthValue == 1 &&
                    current.RightDepth < current.LeftDepth)
                {
                    ++current.RightDepth;
                    depthValue = 0;
                }

                current.Right = right;
                current.RightDepth += depthValue;

                if (RightRightRotation(ref current))
                {
                    depthValue = 0;
                }

                if (RightLeftRotation(ref current))
                {
                    depthValue = 0;
                }
            }

            return depthValue;
        }

        private bool LeftLeftRotation(ref Node n)
        {
            if (n.BalanceFactor > 1 &&
                n.Left != null &&
                n.Left.BalanceFactor > -1)
            {
                Node left, n2, tmp = null;

                left = new Node(n.Left);
                left.LeftDepth = n.LeftDepth - 1;
                left.RightDepth = n.RightDepth + 1;
                n2 = new Node(n);
                n2.LeftDepth = n.LeftDepth - 2;
                n2.RightDepth = n.RightDepth;

                if (left.Right != null)
                {
                    tmp = new Node(left.Right);
                }

                if (n.Parent == null)
                {
                    this.root = left;
                }
                else
                {
                    if (n.Parent.Left == n)
                    {
                        n.Parent.Left = left;
                    }
                    else if (n.Parent.Right == n)
                    {
                        n.Parent.Right = left;
                    }
                    n = left;
                }

                n2.Left = tmp;
                left.Right = n2;

                return true;
            }

            return false;
        }

        private bool LeftRightRotation(ref Node n)
        {
            if (n.BalanceFactor > 1 &&
                n.Left != null &&
                n.Left.BalanceFactor <= -1)
            {
                Node left, n2, tmp = null, newLeft = null, newRight = null;

                left = new Node(n.Left);
                left.LeftDepth = n.Left.LeftDepth;
                left.RightDepth = 0;// n.Left.RightDepth - 1;
                n2 = new Node(n);
                n2.LeftDepth = n.LeftDepth - 2;
                n2.RightDepth = n.RightDepth;
                tmp = new Node(left.Right);
                tmp.LeftDepth = Math.Max(left.LeftDepth, left.RightDepth) + 1;
                tmp.RightDepth = Math.Max(n2.LeftDepth, n2.RightDepth) + 1;

                if (n.Parent == null)
                {
                    this.root = tmp;
                }
                else
                {
                    if (n.Parent.Left == n)
                    {
                        n.Parent.Left = tmp;
                    }
                    else if (n.Parent.Right == n)
                    {
                        n.Parent.Right = tmp;
                    }
                    n = tmp;
                }

                if (tmp.Left != null)
                {
                    newLeft = new Node(tmp.Left);

                    if (newLeft.Left != null)
                    {
                        newLeft.LeftDepth = Math.Max(newLeft.Left.LeftDepth, newLeft.Left.RightDepth) + 1;
                    }

                    if (newLeft.Right != null)
                    {
                        newLeft.RightDepth = Math.Max(newLeft.Right.LeftDepth, newLeft.Right.RightDepth) + 1;
                    }

                    left.RightDepth = Math.Max(newLeft.LeftDepth, newLeft.RightDepth) + 1;
                }

                if (tmp.Right != null)
                {
                    newRight = new Node(tmp.Right);

                    if (newRight.Left != null)
                    {
                        newRight.LeftDepth = Math.Max(newRight.Left.LeftDepth, newRight.Left.RightDepth) + 1;
                    }

                    if (newRight.Right != null)
                    {
                        newRight.RightDepth = Math.Max(newRight.Right.LeftDepth, newRight.Right.RightDepth) + 1;
                    }

                    n2.LeftDepth = Math.Max(newRight.LeftDepth, newRight.RightDepth) + 1;
                }

                n2.Left = newRight;
                left.Right = newLeft;

                tmp.Right = n2;
                tmp.Left = left;

                return true;
            }

            return false;
        }

        private bool RightRightRotation(ref Node n)
        {
            if (n.BalanceFactor < -1 &&
                n.Right != null &&
                n.Right.BalanceFactor < 1)
            {
                Node right, n2, tmp = null;

                right = new Node(n.Right);
                right.LeftDepth = n.LeftDepth + 1;
                right.RightDepth = n.RightDepth - 1;
                n2 = new Node(n);
                n2.LeftDepth = n.LeftDepth;
                n2.RightDepth = n.RightDepth - 2;

                if (right.Left != null)
                {
                    tmp = new Node(right.Left);
                }

                if (n.Parent == null)
                {
                    this.root = right;
                }
                else
                {
                    if (n.Parent.Left == n)
                    {
                        n.Parent.Left = right;
                    }
                    else if (n.Parent.Right == n)
                    {
                        n.Parent.Right = right;
                    }
                    n = right;
                }

                n2.Right = tmp;
                right.Left = n2;

                return true;
            }

            return false;
        }

        private bool RightLeftRotation(ref Node n)
        {
            if (n.BalanceFactor < -1 &&
                n.Right != null &&
                n.Right.BalanceFactor >= 1)
            {
                Node right, n2, tmp = null, newLeft = null, newRight = null;

                right = new Node(n.Right);
                right.LeftDepth = n.Right.LeftDepth - 1;
                right.RightDepth = n.Right.RightDepth;
                n2 = new Node(n);
                n2.LeftDepth = n.LeftDepth;
                n2.RightDepth = 0;
                tmp = new Node(right.Left);
                tmp.LeftDepth = Math.Max(n2.LeftDepth, n2.RightDepth) + 1;
                tmp.RightDepth = Math.Max(right.LeftDepth, right.RightDepth) + 1;

                if (n.Parent == null)
                {
                    this.root = tmp;
                }
                else
                {
                    if (n.Parent.Left == n)
                    {
                        n.Parent.Left = tmp;
                    }
                    else if (n.Parent.Right == n)
                    {
                        n.Parent.Right = tmp;
                    }
                    n = tmp;
                }

                if (tmp.Right != null)
                {
                    newRight = new Node(tmp.Right);

                    if (newRight.Left != null)
                    {
                        newRight.LeftDepth = Math.Max(newRight.Left.LeftDepth, newRight.Left.RightDepth) + 1;
                    }

                    if (newRight.Right != null)
                    {
                        newRight.RightDepth = Math.Max(newRight.Right.LeftDepth, newRight.Right.RightDepth) + 1;
                    }

                    right.LeftDepth = Math.Max(newRight.LeftDepth, newRight.RightDepth) + 1;
                }

                if (tmp.Left != null)
                {
                    newLeft = new Node(tmp.Left);

                    if (newLeft.Left != null)
                    {
                        newLeft.LeftDepth = Math.Max(newLeft.Left.LeftDepth, newLeft.Left.RightDepth) + 1;
                    }

                    if (newLeft.Right != null)
                    {
                        newLeft.RightDepth = Math.Max(newLeft.Right.LeftDepth, newLeft.Right.RightDepth) + 1;
                    }

                    n2.RightDepth = Math.Max(newLeft.LeftDepth, newLeft.RightDepth) + 1;
                }

                n2.Right = newLeft;
                right.Left = newRight;

                tmp.Right = right;
                tmp.Left = n2;

                return true;
            }

            return false;
        }

        public Node Find(int val)
        {
            return Find(val, this.root);
        }

        private Node Find(int val, Node current)
        {
            if (current == null)
            {
                return null;
            }
            else if (current.Value > val)
            {
                return Find(val, current.Left);
            }
            else if (current.Value < val)
            {
                return Find(val, current.Right);
            }
            else
            {
                return current;
            }
        }

        public void Delete(int val)
        {
            Node n = Find(val);
            Node parent = null;
            bool succeeded;

            if (n == null)
            {
                return;
            }

            parent = n.Parent;

            if (parent != null)
            {
                if (parent.Left == n)
                {
                    Node left = parent.Left;
                    DeleteNodeFromParent(n, ref left);
                    parent.Left = left;
                    while (left != null && parent != null)
                    {
                        --parent.LeftDepth;
                        parent = parent.Parent;
                    }

                    if (left != null)
                    {
                        parent = left;
                    }
                }
                else
                {
                    Node right = parent.Right;
                    DeleteNodeFromParent(n, ref right);
                    parent.Right = right;

                    while (right != null && parent != null)
                    {
                        --parent.RightDepth;
                        parent = parent.Parent;
                    }

                    if (right != null)
                    {
                        parent = right;
                    }
                }
            }
            else
            {
                DeleteNodeFromParent(n, ref this.root);
                parent = this.root;
            }

            while (parent != null)
            {
                succeeded = LeftLeftRotation(ref parent);

                if (!succeeded)
                {
                    succeeded = RightRightRotation(ref parent);
                }

                if (!succeeded)
                {
                    succeeded = LeftRightRotation(ref parent);
                }

                if (!succeeded)
                {
                    succeeded = RightLeftRotation(ref parent);
                }

                parent = parent.Parent;
            }
        }

        private void DeleteNodeFromParent(Node n, ref Node current)
        {
            if (n.Left == null && n.Right == null)
            {
                current = null;
            }
            else if (n.Left != null && n.Right == null)
            {
                current = n.Left;
            }
            else if (n.Left == null && n.Right != null)
            {
                current = n.Right;
            }
            else
            {
                Node successor = FindSuccessor(current.Right);
                Node parent = successor.Parent;

                current.Value = successor.Value;

                DeleteNodeFromParent(successor, ref successor);
            }
        }

        private Node FindSuccessor(Node current)
        {
            if (current.Left == null)
            {
                if (current.Parent.Left == current)
                {
                    current.Parent.Left = null;
                }
                else if (current.Parent.Right == current)
                {
                    current.Parent.Right = null;
                }
                return current;
            }
            else
            {
                return FindSuccessor(current.Left);
            }
        }

        public void Clear()
        {
            this.root = null;
        }
    }
}
