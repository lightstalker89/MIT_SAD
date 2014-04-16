using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class BinaryTree : Tree
    {
        private Comparer comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTree"/> class
        /// </summary>
        /// <param name="comp"></param>
        public BinaryTree(Comparer comp)
        {
            this.comparer = comp;
        }

        /// <summary>
        /// Insert new element to the binary tree
        /// </summary>
        /// <param name="key">Key of the element</param>
        /// <param name="value">Value of the element</param>
        public override void Insert(int key, int value)
        {
            if (base.Root == null)
            {
                base.Root = new TreeNode(key, value, null);
            }
            else
            {
                TreeNode node = base.Root;

                while (node != null)
                {
                    int compare = this.comparer.Compare(key, node.Index);

                    if (compare > 0)
                    {
                        if (node.RightChild == null)
                        {
                            node.RightChild = new TreeNode(key, value, node);
                            return;
                        }

                        node = node.RightChild;
                    }
                    else if (compare < 1)
                    {
                        if (node.LeftChild == null)
                        {
                            node.LeftChild = new TreeNode(key, value, node);
                            return;
                        }

                        node = node.LeftChild;
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
        /// Search for a node value in the tree
        /// </summary>
        /// <param name="key">Key of the node</param>
        /// <param name="value">Value of the node</param>
        /// <returns>If node is available</returns>
        public override bool Search(int key, out int value)
        {
            TreeNode node = base.Root;

            while (node != null)
            {
                int compare = this.comparer.Compare(key, node.Index);

                if (compare > 0)
                {
                    node = node.RightChild;
                }
                else if (compare < 0)
                {
                    node = node.LeftChild;
                }
                else
                {
                    value = node.Value;
                    return true;
                }
            }

            value = -1;
            return false;
        }

        /// <summary>
        /// Delete a node in the tree
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Delete(int key)
        {
            TreeNode node = base.Root;

            while (node != null)
            {
                if (this.comparer.Compare(key, node.Index) > 0)
                {
                    node = node.RightChild;
                }
                else if (this.comparer.Compare(key, node.Index) < 0)
                {
                    node = node.LeftChild;
                }
                else
                {
                    TreeNode left = node.LeftChild;
                    TreeNode right = node.RightChild;

                    if (left == null)
                    {
                        if (right == null)
                        {
                            if (node == base.Root)
                            {
                                base.Root = null;
                            }
                            else
                            {
                                TreeNode parent = node.Parent;
                                if (parent.LeftChild == node)
                                {
                                    parent.LeftChild = null;
                                }
                                else
                                {
                                    parent.RightChild = null;
                                }
                            }
                        }
                        else
                        {
                            Replace(node, right);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);
                    }
                    else
                    {
                        // Left and right child are available
                        
                        // Successor == the right child of the deleting node
                        TreeNode successor = right;

                        if (successor.LeftChild == null)
                        {
                            TreeNode parent = node.Parent;

                            successor.Parent = parent;
                            successor.LeftChild = left;
                            successor.BalanceFactor = node.BalanceFactor;

                            if (left != null)
                            {
                                left.Parent = successor;
                            }

                            if (node == base.Root)
                            {
                                base.Root = successor;
                            }
                            else
                            {
                                if (parent.LeftChild == node)
                                {
                                    parent.LeftChild = successor;
                                }
                                else
                                {
                                    parent.RightChild = successor;
                                }
                            }
                        }
                        else
                        {
                            while (successor.LeftChild != null)
                            {
                                successor = successor.LeftChild;
                            }

                            TreeNode parent = node.Parent;
                            TreeNode successorParent = successor.Parent;
                            TreeNode successorRight = successor.RightChild;

                            if (successorParent.LeftChild == successor)
                            {
                                successorParent.LeftChild = successorRight;
                            }
                            else
                            {
                                successorParent.RightChild = successorRight;
                            }

                            if (successorRight != null)
                            {
                                successorRight.Parent = successorParent;
                            }

                            successor.Parent = parent;
                            successor.LeftChild = left;
                            successor.BalanceFactor = node.BalanceFactor;
                            successor.RightChild = right;
                            right.Parent = successor;

                            if (left != null)
                            {
                                left.Parent = successor;
                            }

                            if (node == base.Root)
                            {
                                base.Root = successor;
                            }
                            else
                            {
                                if (parent.LeftChild == node)
                                {
                                    parent.LeftChild = successor;
                                }
                                else
                                {
                                    parent.RightChild = successor;
                                }
                            }
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Replace one node through another
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="source">Source node</param>
        private static void Replace(TreeNode target, TreeNode source)
        {
            TreeNode left = source.LeftChild;
            TreeNode right = source.RightChild;

            target.BalanceFactor = source.BalanceFactor;
            target.Index = source.Index;
            target.Value = source.Value;
            target.LeftChild = left;
            target.RightChild = right;

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
        /// Output the Binary-Tree
        /// </summary>
        /// <param name="type">Type to run through the tree</param>
        /// <param name="root">Root node of the tree</param>
        public override void Output(OutputType type, TreeNode root)
        {
            switch (type)
            {
                case OutputType.InOrder:
                    this.OutputInOrder(root);
                    Console.WriteLine();
                    break;
                case OutputType.PostOrder:
                    this.OutputPostOrder(root);
                    Console.WriteLine();
                    break;
                case OutputType.PreOrder:
                    this.OutputPreOrder(root);
                    Console.WriteLine();
                    break;
                default:
                    this.OutputInOrder(root);
                    Console.WriteLine();
                    break;
            }
        }

        /// <summary>
        /// Output the tree inOrder
        /// 1. Pass through the left branch - if leaf => finish
        /// 2. Output the value of the node
        /// 3. Pass through the right branch - leaf => finish
        /// </summary>
        /// <param name="node">Root node of the treee</param>
        private void OutputInOrder(TreeNode node)
        {
            if (node != null)
            {
                if (node.LeftChild != null)
                {
                    this.OutputInOrder(node.LeftChild);
                }

                Console.Write(node.Value.ToString() + " ");

                if (node.RightChild != null)
                {
                    this.OutputInOrder(node.RightChild);
                }
            }
        }

        /// <summary>
        /// Output the tree preOrder
        /// 1. Output the value of the node
        /// 2. Pass through the left branch
        /// 3. Pass throught the right branch
        /// </summary>
        /// <param name="node">Root node of the tree</param>
        private void OutputPreOrder(TreeNode node)
        {
            if (node != null)
            {
                Console.Write(node.Value.ToString() + " ");
                if (node.LeftChild != null)
                {
                    OutputPreOrder(node.LeftChild);
                }

                if (node.RightChild != null)
                {
                    OutputPreOrder(node.RightChild);
                }
            }
        }

        /// <summary>
        /// Output the tree postOrder
        /// 1. Pass through the left branch
        /// 2. Pass through the right branch
        /// 3. Output the value of the node
        /// </summary>
        /// <param name="node"></param>
        private void OutputPostOrder(TreeNode node)
        {
            if (node != null)
            {
                if (node.LeftChild != null)
                {
                    OutputPostOrder(node.LeftChild);
                }

                if (node.RightChild != null)
                {
                    OutputPostOrder(node.RightChild);
                }

                Console.Write(node.Value.ToString() + " ");
            }
        }
    }
}
