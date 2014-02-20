using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class AVLTree : Tree
    {
        // private TreeNode root;
        private Comparer comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AVLTree"/> class
        /// </summary>
        /// <param name="comp"></param>
        public AVLTree(Comparer comp)
        {
            this.comparer = comp;
        }

        /// <summary>
        /// Insert a new node to the tree
        /// </summary>
        /// <param name="key">Index of the node in the array</param>
        /// <param name="value">Value of the node in the array</param>
        public override void Insert(int key, int value)
        {
            // Insert node in right place
            // Calculate balance factor
            // Do rotation if necessary
            // Choose right rotation 

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

                    if (compare < 0)
                    {
                        TreeNode left = node.LeftChild;

                        if (left == null)
                        {
                            node.LeftChild = new TreeNode(key, value, parent: node);
                            this.InsertBalance(node, 1);

                            return;
                        }
                        else
                        {
                            node = left;
                        }
                    }
                    else if (compare > 0)
                    {
                        TreeNode right = node.RightChild;

                        if (right == null)
                        {
                            node.RightChild = new TreeNode(key, value, parent: node);
                            this.InsertBalance(node, -1);

                            return;
                        }
                        else
                        {
                            node = right;
                        }
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
        /// Rebalancing the tree after insertion.
        /// If the balance of a node becomes 2 or -2 it must be rotated.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="p"></param>
        private void InsertBalance(TreeNode node, int balance)
        {
            while (node != null)
            {
                balance = (node.BalanceFactor += balance);

                if (balance == 0)
                {
                    return;
                }
                else if (balance == 2)
                {
                    if (node.LeftChild.BalanceFactor == 1)
                    {
                        this.RotateRight(node);
                    }
                    else
                    {
                        this.RotateLeftRight(node);
                    }

                    return;
                }
                else if (balance == -2)
	            {
                    if(node.RightChild.BalanceFactor == -1)
                    {
                        this.RotateLeft(node);
                    }
                    else
	                {
                        this.RotateRightLeft(node);
	                }

                    return;
	            }

                TreeNode parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.LeftChild == node ? 1 : -1;
                }

                node = parent;
            }
        }

        /// <summary>
        /// Delete a node from the tree
        /// </summary>
        /// <param name="key">Key of the node to delete from the array</param>
        public override bool Delete(int key)
        {
            TreeNode node = base.Root;

            while (node != null)
            {
                if (this.comparer.Compare(key, node.Index) < 0)
                {
                    node = node.LeftChild;
                }
                else if (this.comparer.Compare(key, node.Index) > 0)
                {
                    node = node.RightChild;
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
                                    this.DeleteBalance(parent, -1);
                                }
                                else
                                {
                                    parent.RightChild = null;
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

                            this.DeleteBalance(successor, 1);
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

                            this.DeleteBalance(successorParent, -1);
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Search for a node value in the tree
        /// </summary>
        /// <param name="key">Index of the node</param>
        /// <param name="value">Value of the node</param>
        /// <returns>If node was found or not</returns>
        public override bool Search(int key, out int value)
        {
            TreeNode node = base.Root;

            while (node != null)
            {
                if (this.comparer.Compare(key, node.Index) < 0)
                {
                    node = node.LeftChild;
                }
                else if (this.comparer.Compare(key, node.Index) > 0)
                {
                    node = node.RightChild;
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
        /// Rebalancing the tree after deletion. Do rotation if necessary
        /// </summary>
        /// <param name="node"></param>
        /// <param name="balance"></param>
        private void DeleteBalance(TreeNode node, int balance)
        {
            while (node != null)
            {
                balance = (node.BalanceFactor += balance);

                if (balance == 2)
                {
                    if (node.LeftChild.BalanceFactor >= 0)
                    {
                        node = this.RotateRight(node);

                        if (node.BalanceFactor == -1)
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
                    if (node.RightChild.BalanceFactor <= 0)
                    {
                        node = this.RotateLeft(node);

                        if (node.BalanceFactor == 1)
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

                TreeNode parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.LeftChild == node ? -1 : 1;
                }

                node = parent;
            }
        }

        /// <summary>
        /// x lies in the left branch, the left branch
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreeNode RotateLeft(TreeNode node)
        {
            TreeNode right = node.RightChild;
            TreeNode rightLeft = right.LeftChild;
            TreeNode parent = node.Parent;

            right.Parent = parent;
            right.LeftChild = node;
            node.RightChild = rightLeft;
            node.Parent = right;

            if (rightLeft != null)
            {
                rightLeft.Parent = node;
            }

            if (node == base.Root)
            {
                base.Root = right;
            }
            else if (parent.RightChild == node)
            {
                parent.RightChild = right;
            }
            else
            {
                parent.LeftChild = right;
            }

            right.BalanceFactor++;
            node.BalanceFactor = -right.BalanceFactor;

            return right;
        }

        /// <summary>
        /// x lies in the right branch, the left branch
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreeNode RotateLeftRight(TreeNode node)
        {
            TreeNode left = node.LeftChild;
            TreeNode leftRight = left.RightChild;
            TreeNode parent = node.Parent;
            TreeNode leftRightRight = leftRight.RightChild;
            TreeNode leftRightLeft = leftRight.LeftChild;

            leftRight.Parent = parent;
            node.LeftChild = leftRightRight;
            left.RightChild = leftRightLeft;
            leftRight.LeftChild = left;
            leftRight.RightChild = node;
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

            if (node == base.Root)
            {
                base.Root = leftRight;
            }
            else if (parent.LeftChild == node)
            {
                parent.LeftChild = leftRight;
            }
            else
            {
                parent.RightChild = leftRight;
            }

            if (leftRight.BalanceFactor == -1)
            {
                node.BalanceFactor = 0;
                left.BalanceFactor = 1;
            }
            else if (leftRight.BalanceFactor == 0)
            {
                node.BalanceFactor = 0;
                left.BalanceFactor = 0;
            }
            else
            {
                node.BalanceFactor = -1;
                left.BalanceFactor = 0;
            }

            leftRight.BalanceFactor = 0;

            return leftRight;
        }

        /// <summary>
        /// x lies in the right branch, the right branch
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreeNode RotateRight(TreeNode node)
        {
            TreeNode left = node.LeftChild;
            TreeNode leftRight = left.RightChild;
            TreeNode parent = node.Parent;

            left.Parent = parent;
            left.RightChild = node;
            node.LeftChild = leftRight;
            node.Parent = left;

            if (leftRight != null)
            {
                leftRight.Parent = node;
            }

            if (node == base.Root)
            {
                base.Root = left;
            }
            else if (parent.LeftChild == node)
            {
                parent.LeftChild = left;
            }
            else
            {
                parent.RightChild = left;
            }

            left.BalanceFactor--;
            node.BalanceFactor = -left.BalanceFactor;

            return left;
        }

        /// <summary>
        /// x lies in the left branch, the right branch
        /// </summary>
        /// <param name="node">Node where balancefactor is equal 2 or equal -2</param>
        /// <returns></returns>
        public TreeNode RotateRightLeft(TreeNode node)
        {
            TreeNode right = node.RightChild;
            TreeNode rightLeft = right.LeftChild;
            TreeNode parent = node.Parent;
            TreeNode rightLeftLeft = rightLeft.LeftChild;
            TreeNode rightLeftRight = rightLeft.RightChild;

            rightLeft.Parent = parent;
            node.RightChild = rightLeftLeft;
            right.LeftChild = rightLeftRight;
            rightLeft.RightChild = right;
            rightLeft.LeftChild = node;
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

            if (node == base.Root)
            {
                base.Root = rightLeft;
            }
            else if (parent.RightChild == node)
            {
                parent.RightChild = rightLeft;
            }
            else
            {
                parent.LeftChild = rightLeft;
            }

            if (rightLeft.BalanceFactor == 1)
            {
                node.BalanceFactor = 0;
                right.BalanceFactor = -1;
            }
            else if (rightLeft.BalanceFactor == 0)
            {
                node.BalanceFactor = 0;
                right.BalanceFactor = 0;
            }
            else
            {
                node.BalanceFactor = 1;
                right.BalanceFactor = 0;
            }

            rightLeft.BalanceFactor = 0;

            return rightLeft;
        }

        /// <summary>
        /// Clear the tree
        /// </summary>
        public void Clear()
        {
            base.Root = null;
        }

        /// <summary>
        /// Output the AVL-Tree
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

    /// <summary>
    /// The type to run through the AVL-Tree
    /// </summary>
    public enum OutputType
    {
        InOrder,
        PreOrder,
        PostOrder
    }
}
