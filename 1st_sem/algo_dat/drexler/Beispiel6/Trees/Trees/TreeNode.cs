//-----------------------------------------------------------------------
// <copyright file="TreeNode.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Trees
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Tree node calss
    /// </summary>
    public class TreeNode
    {
        private int balanceFactor;
        private TreeNode parent;
        private TreeNode leftChild;
        private TreeNode rightChild;
        private bool isRoot;
        private int index;
        private int value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class
        /// </summary>
        /// <param name="key">Index in the array</param>
        /// <param name="value">Value for the given index in the array</param>
        /// <param name="parent">Parent node of this tree node</param>
        public TreeNode(int key, int value, TreeNode parent)
        {
            this.index = key;
            this.value = value;
            this.parent = parent;

            if (this.parent == null)
            {
                this.isRoot = true;
            }
            else
            {
                this.isRoot = false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class
        /// </summary>
        /// <param name="parent">Parent node of this tree node</param>
        /// <param name="leftChild">Left child node of this tree node</param>
        public TreeNode(int key, int value, TreeNode parent, TreeNode leftChild) : this(key, value, parent)
        {
            this.leftChild = leftChild;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class
        /// </summary>
        /// <param name="parent">Parent node of this tree node</param>
        /// <param name="leftChild">Left child node of this tree node</param>
        /// <param name="rightChild">Right child node of this tree node</param>
        public TreeNode(int key, int value, TreeNode parent, TreeNode leftChild, TreeNode rightChild)
            : this(key, value, parent, leftChild)
        {
            this.rightChild = rightChild;
        }

        /// <summary>
        /// Gets the parent node of the current node
        /// </summary>
        public TreeNode Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        /// <summary>
        /// Gets or sets the value of the left child node
        /// </summary>
        public TreeNode LeftChild
        {
            get { return this.leftChild; }
            set { this.leftChild = value; }
        }

        /// <summary>
        /// Gets or sets the value of the right child node
        /// </summary>
        public TreeNode RightChild
        {
            get { return this.rightChild; }
            set { this.rightChild = value; }
        }

        /// <summary>
        /// Gets or sets the value of the balance factor of the tree
        /// </summary>
        public int BalanceFactor
        {
            get { return this.balanceFactor; }
            set { this.balanceFactor = value; }
        }

        /// <summary>
        /// Gets or sets the value of the boolean if it is root or not
        /// </summary>
        public bool IsRoot
        {
            get { return this.isRoot; }
            set { this.isRoot = value; }
        }

        /// <summary>
        /// Gets or sets the value for the index
        /// </summary>
        public int Index
        {
            get { return this.index; }
            set { this.index = value; }
        }

        /// <summary>
        /// Gets or sets the value of the node element
        /// </summary>
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }   
    }
}
