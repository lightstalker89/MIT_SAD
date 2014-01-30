using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BspAVL
{
    class BinaryTree
    {
        Node root;

        public BinaryTree()
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

            if (this.root == null)
            {
                this.root = newNode;
            }
            else
            {
                Node current = this.root;
                bool endLoop = false;

                while (!endLoop)
                {
                    if (newNode.Value < current.Value)
                    {
                        if (current.Left == null)
                        {
                            current.Left = newNode;
                            endLoop = true;
                        }

                        current = current.Left;
                    }
                    else if (newNode.Value >= current.Value)
                    {
                        if (current.Right == null)
                        {
                            current.Right = newNode;
                            endLoop = true;
                        }

                        current = current.Right;
                    }
                }
            }
        }

        private void InsertNode(Node n, Node current)
        {
            if (n.Value < current.Value)
            {
                if (current.Left == null)
                {
                    current.Left = n;
                }
                else
                {
                    InsertNode(n, current.Left);
                }
            }
            else if (n.Value >= current.Value)
            {
                if (current.Right == null)
                {
                    current.Right = n;
                }
                else
                {
                    InsertNode(n, current.Right);
                }
            }
        }

        public Node Find(int val)
        {
            Node current = this.root;
            bool endLoop = false;

            while (!endLoop && current != null)
            {
                if (val == current.Value)
                {
                    return current;
                }
                else if (val < current.Value)
                {
                    current = current.Left;
                }
                else if (val > current.Value)
                {
                    current = current.Right;
                }
            }

            return null;
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
    }
}
