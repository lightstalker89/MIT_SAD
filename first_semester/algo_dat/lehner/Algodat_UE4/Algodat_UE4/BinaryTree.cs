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
                }
            }
        }

        public void Print()
        {
            this.PrintTree(this.RootNode);
        }

        private void PrintTree(Node node)
        {

            Queue q = new Queue();
            q.Enqueue(node);//You don't need to write the root here, it will be written in the loop
            q.Enqueue(new Node(-1));//newline

            while (q.Count > 0)
            {
                Node n = q.Dequeue() as Node;

                if (n.Value == -1)
                {
                    Console.WriteLine();
                    if (q.Count > 0)
                    {
                        q.Enqueue(new Node(-1));
                    }
                }
                else
                {
                    Console.Write("{0}\t", n.Value); //Only write the value when you dequeue it
                    if (n.LeftNode != null)
                    {
                        //Console.Write("l{0}\t", n.LeftNode.Value);
                        q.Enqueue(n.LeftNode);//enqueue the left child
                    }
                    if (n.RightNode != null)
                    {
                        //Console.Write("r{0}\t", n.RightNode.Value);
                        q.Enqueue(n.RightNode);//enque the right child
                    }
                }
            }
        }

        public void Delete(int value)
        {
            this.DeleteNode(this.RootNode, value);
        }

        private void DeleteNode(Node n, int value)
        {
            if (n == null || value < 0)
                return;

            if (value > n.Value)
            {
                if (n.RightNode != null && value == n.RightNode.Value)
                    n.RightNode = null;
                else
                    this.DeleteNode(n.RightNode, value);
            }
            else
            {
                if (n.LeftNode != null && value == n.LeftNode.Value)
                    n.LeftNode = null;
                else
                    this.DeleteNode(n.LeftNode, value);
            }
        }
    }
}
