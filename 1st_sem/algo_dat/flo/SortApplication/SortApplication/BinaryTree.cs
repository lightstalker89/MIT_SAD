using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortApplication
{
    public class BinaryTree
    {
        class BinaryTreeNode
        {
            private int id;
            public void setID(char _id) { id = _id; }
            public int getID() { return id; }
            public BinaryTreeNode left; 
            public BinaryTreeNode right;

            public BinaryTreeNode()
            {
                left = right = null;
            }

            public BinaryTreeNode(int _id)
            {
                id = _id;
                left = right = null;
            }
        }

        private BinaryTreeNode root;

        public BinaryTree()
        {
            root = null;
        }

        public void insert(int c)
        {
            addNode(c, ref root);
        }

        private void addNode(int c, ref BinaryTreeNode rptr)
        {
            if (rptr == null)
            {
                rptr = new BinaryTreeNode(c);
            }
            else if (rptr.getID() > c)
            {
                addNode(c, ref rptr.left);
            }
            else
            {
                addNode(c, ref rptr.right);
            }
        }

        public void inOrderTraversal()
        {
            inOrderTraversalHelper(root);
        }
        private void inOrderTraversalHelper(BinaryTreeNode r)
        {
            if (r != null)
            {
                inOrderTraversalHelper(r.left);
                Console.Write("{0}   ", r.getID());
                inOrderTraversalHelper(r.right);
            }
        }
        public void preOrderTraversal()
        {
            preOrderTraversalHelper(root);
        }
        private void preOrderTraversalHelper(BinaryTreeNode r)
        {
            if (r != null)
            {
                Console.Write("{0}   ", r.getID());
                preOrderTraversalHelper(r.left);
                preOrderTraversalHelper(r.right);
            }
        }
        public void postOrderTraversal()
        {
            postOrderTraversalHelper(root);
        }
        private void postOrderTraversalHelper(BinaryTreeNode r)
        {
            if (r != null)
            {
                postOrderTraversalHelper(r.left);
                postOrderTraversalHelper(r.right);
                Console.Write("{0}   ", r.getID());
            }
        }
    }
}
