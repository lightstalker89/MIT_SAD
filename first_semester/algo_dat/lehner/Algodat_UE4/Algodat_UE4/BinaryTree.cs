using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE4
{
    public class BinaryTree
    {
        List<Node> TreeItems { get; set; }
        public Node RootNode { get; set; }

        public BinaryTree()
        {
            this.TreeItems = new List<Node>();
        }

        public void CreateTree(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (i == 0)
                {
                    Node n = new Node(numbers[i]);
                    this.RootNode = n;
                    Console.WriteLine("Root: {0}", this.RootNode.Value);
                }
                else
                {
                    Node node = new Node(numbers[i]);
                    this.AddNode(this.RootNode, node);
                    Console.WriteLine("Node: {0}", node.Value);
                }
            }
        }

        public void AddNode(Node rootNode, Node node)
        {
            if (rootNode.Value > node.Value)
            {
                if (rootNode.LeftNode != null)
                {
                    this.AddNode(rootNode.LeftNode, node);
                }
                else
                {
                    rootNode.LeftNode = node;
                    node.ParentNode = rootNode;
                }
            }
            else if (rootNode.Value <= node.Value)
            {
                if (rootNode.RightNode != null)
                {
                    this.AddNode(rootNode.RightNode, node);
                }
                else
                {
                    rootNode.RightNode = node;
                    node.ParentNode = rootNode;
                }
            }
        }

        public void Print()
        {
            this.PrintTree(this.RootNode, "*");
        }

        private void PrintTree(Node root, string prefix)
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

        public void Remove(int number)
        {
            this.RemoveNode(this.RootNode, number);
        }

        public void RemoveNode(Node p, int number)
        {
            if (p == null)
            {
                return;
            }
            else
            {
                if (p.Value > number)
                {
                    this.RemoveNode(p.LeftNode, number);
                }
                else if (p.Value < number)
                {
                    this.RemoveNode(p.RightNode, number);
                }
                else if (p.Value == number)
                {
                    this.RemoveFoundNode(p);
                }
            }
        }

        public void RemoveFoundNode(Node q)
        {
            Node r;

            if (q.LeftNode == null || q.RightNode == null)
            {
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
                r = this.Successor(q);
                q.Value = r.Value;
            }

            Node p;
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
            }
            r = null;
        }

        public Node Successor(Node q)
        {
            if (q.RightNode != null)
            {
                Node r = q.RightNode;
                while (r.LeftNode != null)
                {
                    r = r.LeftNode;
                }
                return r;
            }
            else
            {
                Node p = q.ParentNode;
                while (p != null && q == p.RightNode)
                {
                    q = p;
                    p = q.ParentNode;
                }
                return p;
            }
        }

        public bool Find(int value)
        {
            return this.FindNode(this.RootNode, value);
        }

        public bool FindNode(Node n, int value)
        {

            while (n != null)
            {
                if (value > n.Value)
                {
                    //if (n.RightNode != null && value == n.RightNode.Value)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                        n = n.RightNode;
                    //}
                }
                else if (value < n.Value)
                {
                    //if (n.LeftNode != null && value == n.LeftNode.Value)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                        n = n.LeftNode;
                    //}
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
