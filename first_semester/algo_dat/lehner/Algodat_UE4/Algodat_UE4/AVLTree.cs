using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE4
{
    public class AVLTree
    {
        public AVLNode RootNode { get; set; }

        public void CreateAVLTree(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                this.Insert(numbers[i]);
            }
        }

        public void Insert(int number)
        {
            // create new node
            AVLNode n = new AVLNode(number);
            // start recursive procedure for inserting the node
            this.InsertAVL(this.RootNode, n);
        }

        public void InsertAVL(AVLNode p, AVLNode q)
        {
            // If  node to compare is null, the node is inserted. If the root is null, it is the root of the tree.
            if (p == null)
            {
                this.RootNode = q;
            }
            else
            {

                // If compare node is smaller, continue with the left node
                if (q.Value < p.Value)
                {
                    if (p.LeftNode == null)
                    {
                        p.LeftNode = q;
                        q.ParentNode = p;

                        // Node is inserted now, continue checking the balance
                        this.RecursiveBalance(p);
                    }
                    else
                    {
                        this.InsertAVL(p.LeftNode, q);
                    }

                }
                else if (q.Value > p.Value)
                {
                    if (p.RightNode == null)
                    {
                        p.RightNode = q;
                        q.ParentNode = p;

                        // Node is inserted now, continue checking the balance
                        this.RecursiveBalance(p);
                    }
                    else
                    {
                        this.InsertAVL(p.RightNode, q);
                    }
                }
                else
                {
                    // do nothing: This node already exists
                }
            }
        }

        public void RecursiveBalance(AVLNode cur)
        {

            // we do not use the balance in this class, but the store it anyway
            int balance = cur.GetBalance();

            // check the balance
            if (balance == -2)
            {
                if (this.Height(cur.LeftNode.LeftNode) >= this.Height(cur.LeftNode.RightNode))
                {
                    cur = this.RotateRight(cur);
                }
                else
                {
                    cur = this.RotateLeftRight(cur);
                }
            }
            else if (balance == 2)
            {
                if (this.Height(cur.RightNode.RightNode) >= this.Height(cur.RightNode.LeftNode))
                {
                    cur = this.RotateLeft(cur);
                }
                else
                {
                    cur = this.RotateRightLeft(cur);
                }
            }

            // we did not reach the root yet
            if (cur.ParentNode != null)
            {
                this.RecursiveBalance(cur.ParentNode);
            }
            else
            {
                this.RootNode = cur;
            }
        }

        public void Remove(int number)
        {
            // First we must find the node, after this we can delete it.
            this.RemoveAVL(this.RootNode, number);
        }

        public void RemoveAVL(AVLNode p, int number)
        {
            if (p == null)
            {
                // der Wert existiert nicht in diesem Baum, daher ist nichts zu tun
                return;
            }
            else
            {
                if (p.Value > number)
                {
                    this.RemoveAVL(p.LeftNode, number);
                }
                else if (p.Value < number)
                {
                    this.RemoveAVL(p.RightNode, number);
                }
                else if (p.Value == number)
                {
                    // we found the node in the tree.. now lets go on!
                    this.RemoveFoundNode(p);
                }
            }
        }

        public void RemoveFoundNode(AVLNode q)
        {
            AVLNode r;
            // at least one child of q, q will be removed directly
            if (q.LeftNode == null || q.RightNode == null)
            {
                // the root is deleted
                if (q.ParentNode == null)
                {
                    this.RootNode = null;
                    q = null;
                    return;
                }
                r = q;
            }
            else
            {
                // q has two children --> will be replaced by successor
                r = this.Successor(q);
                q.Value = r.Value;
            }

            AVLNode p;
            if (r.LeftNode != null)
            {
                p = r.LeftNode;
            }
            else
            {
                p = r.RightNode;
            }

            if (p != null)
            {
                p.ParentNode = r.ParentNode;
            }

            if (r.ParentNode == null)
            {
                this.RootNode = p;
            }
            else
            {
                if (r == r.ParentNode.LeftNode)
                {
                    r.ParentNode.LeftNode = p;
                }
                else
                {
                    r.ParentNode.RightNode = p;
                }
                // balancing must be done until the root is reached.
                this.RecursiveBalance(r.ParentNode);
            }
            r = null;
        }

        public AVLNode RotateLeft(AVLNode n)
        {

            AVLNode v = n.RightNode;
            v.ParentNode = n.ParentNode;

            n.RightNode = v.LeftNode;

            if (n.RightNode  != null)
            {
                n.RightNode.ParentNode = n;
            }

            v.LeftNode = n;
            n.ParentNode = v;

            if (v.ParentNode != null)
            {
                if (v.ParentNode.RightNode == n)
                {
                    v.ParentNode.RightNode = v;
                }
                else if (v.ParentNode.LeftNode == n)
                {
                    v.ParentNode.LeftNode = v;
                }
            }

            return v;
        }

        public AVLNode RotateRight(AVLNode n)
        {

            AVLNode v = n.LeftNode;
            v.ParentNode = n.ParentNode;

            n.LeftNode = v.RightNode;

            if (n.LeftNode != null)
            {
                n.LeftNode.ParentNode = n;
            }

            v.RightNode = n;
            n.ParentNode = v;


            if (v.ParentNode != null)
            {
                if (v.ParentNode.RightNode == n)
                {
                    v.ParentNode.RightNode = v;
                }
                else if (v.ParentNode.LeftNode == n)
                {
                    v.ParentNode.LeftNode = v;
                }
            }

            return v;
        }

        public AVLNode RotateLeftRight(AVLNode u)
        {
            u.LeftNode = this.RotateLeft(u.LeftNode);
            return this.RotateRight(u);
        }

        public AVLNode RotateRightLeft(AVLNode u)
        {
            u.RightNode = this.RotateRight(u.RightNode);
            return this.RotateLeft(u);
        }

        public AVLNode Successor(AVLNode q)
        {
            if (q.RightNode != null)
            {
                AVLNode r = q.RightNode;
                while (r.LeftNode != null)
                {
                    r = r.LeftNode;
                }
                return r;
            }
            else
            {
                AVLNode p = q.ParentNode;
                while (p != null && q == p.RightNode)
                {
                    q = p;
                    p = q.ParentNode;
                }
                return p;
            }
        }
 

        private int GetLeftDepth(AVLNode n, int count)
        {
            if (n == null)
            {
                return count;
            }
            if (n.LeftNode != null)
            {
                count = this.GetLeftDepth(n.LeftNode, count);
            }

            return count += 1;
        }

        private int GetRightDepth(AVLNode n, int count)
        {
            if (n == null)
            {
                return count;
            }
            if (n.RightNode != null)
            {
                count = this.GetRightDepth(n.RightNode, count);
            }

            return count += 1;
        }

        public int LeftHeight()
        {
            return this.GetLeftDepth(this.RootNode, 0);
        }

        public int RightHeight()
        {
            return this.GetRightDepth(this.RootNode, 0);
        }

        public int BalanceFactor()
        {
            return this.LeftHeight() - this.RightHeight();
        }

        public void Print()
        {
            this.PrintTree(this.RootNode, "*");
        }

        private void PrintTree(AVLNode root, string prefix)
        {
            if (root == null)
            {
                Console.WriteLine(prefix + "+- <null>");
                return;
            }

            Console.WriteLine(prefix + "+- " + root.Value);
            this.PrintTree(root.LeftNode, prefix + "|  ");
            this.PrintTree(root.RightNode, prefix + "|  ");
        }

        private int Height(AVLNode cur)
        {
            if (cur == null)
            {
                return -1;
            }
            else
            {
                return cur.Height();
            }
        }
    }
}
