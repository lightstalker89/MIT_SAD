using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspAVL
{
    [System.Diagnostics.DebuggerDisplay("{Value} - {BalanceFactor}")]
    public class Node
    {
        Node left;
        Node right;
        Node parent;
        int leftDepth;
        int rightDepth;

        public Node(int value)
        {
            this.LeftDepth = 0;
            this.RightDepth = 0;

            this.left = null;
            this.right = null;

            this.Value = value;
        }

        public Node(Node n)
        {
            this.Value = n.Value;
            this.Left = n.Left;
            this.Right = n.Right;
            //this.Parent = n.Parent;
        }

        public int Value { get; set; }

        public Node Parent { get; private set; }

        public Node Left
        {
            get
            {
                return this.left;
            }
            set
            {
                //if (this.left == null && value != null)
                //{
                //    ++this.LeftDepth;
                //}
                /*else */if (value == null)
                {
                    this.LeftDepth = 0;
                }

                this.left = value;

                if (this.left != null &&
                    this.left.Parent != this)
                {
                    this.left.Parent = this;
                }
            }
        }

        public Node Right
        {
            get
            {
                return this.right;
            }
            set
            {
                //if (this.right == null && value != null)
                //{
                //    ++this.RightDepth;
                //}
                /*else */if (value == null)
                {
                    this.RightDepth = 0;
                }

                this.right = value;

                if (this.right != null &&
                    this.right.Parent != this)
                {
                    this.right.Parent = this;
                }
            }
        }

        public int BalanceFactor
        {
            get
            {
                // http://electrofriends.com/source-codes/software-programs/cpp-programs/cpp-data-structure/c-program-to-perform-insertion-and-deletion-operations-on-avl-trees/
                // http://oopweb.com/Algorithms/Documents/AvlTrees/Volume/AvlTrees.htm
                return this.LeftDepth - this.RightDepth;
            }
        }

        public int LeftDepth { get; set; }

        public int RightDepth { get; set; }

        //public int LeftDepth
        //{
        //    get
        //    {
        //        //return this.leftDepth + (this.Left == null ? 0 : Math.Max(this.Left.LeftDepth, this.Left.RightDepth));
        //        return this.leftDepth;
        //    }
        //    private set
        //    {
        //        //this.dirty = this.leftDepth != value;
        //        this.leftDepth = value;
        //    }
        //}

        //public int RightDepth
        //{
        //    get
        //    {
        //        //return this.rightDepth + (this.Right == null ? 0 : Math.Max(this.Right.LeftDepth, this.Right.RightDepth));
        //        return this.rightDepth;
        //    }
        //    private set
        //    {
        //        //this.dirty = this.rightDepth != value;
        //        this.rightDepth = value;
        //    }
        //}

        public bool IsLeaf
        {
            get
            {
                return this.Left == null &&
                       this.Right == null;
            }
        }
    }
}
