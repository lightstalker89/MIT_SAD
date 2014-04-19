using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BspAVL;

namespace BspAVLTests
{
    [TestClass]
    public class AVLTests
    {
        [TestMethod]
        public void InsertDelete()
        {
            int[] array = new int[] { 19, 10, 46, 14, 4, 37, 7, 12, 18, 28, 40, 55, 51, 61, 49, 58, 32, 21 };

            AVLTree tree = new AVLTree();

            foreach (int item in array)
            {
                tree.Insert(item);
            }

            Assert.AreEqual<int>(19, tree.Root.Value);
            Assert.AreEqual<int>(10, tree.Root.Left.Value);
            Assert.AreEqual<int>(4, tree.Root.Left.Left.Value);
            Assert.AreEqual<int>(14, tree.Root.Left.Right.Value);
            Assert.IsNull(tree.Root.Left.Left.Left);
            Assert.AreEqual<int>(7, tree.Root.Left.Left.Right.Value);
            Assert.IsNull(tree.Root.Left.Left.Right.Left);
            Assert.IsNull(tree.Root.Left.Left.Right.Right);
            Assert.AreEqual<int>(12, tree.Root.Left.Right.Left.Value);
            Assert.AreEqual<int>(18, tree.Root.Left.Right.Right.Value);
            Assert.IsNull(tree.Root.Left.Right.Left.Left);
            Assert.IsNull(tree.Root.Left.Right.Left.Right);
            Assert.IsNull(tree.Root.Left.Right.Right.Left);
            Assert.IsNull(tree.Root.Left.Right.Right.Right);

            Assert.AreEqual<int>(46, tree.Root.Right.Value);
            Assert.AreEqual<int>(37, tree.Root.Right.Left.Value);
            Assert.AreEqual<int>(55, tree.Root.Right.Right.Value);
            Assert.AreEqual<int>(28, tree.Root.Right.Left.Left.Value);
            Assert.AreEqual<int>(40, tree.Root.Right.Left.Right.Value);
            Assert.AreEqual<int>(51, tree.Root.Right.Right.Left.Value);
            Assert.AreEqual<int>(61, tree.Root.Right.Right.Right.Value);
            Assert.AreEqual<int>(21, tree.Root.Right.Left.Left.Left.Value);
            Assert.AreEqual<int>(32, tree.Root.Right.Left.Left.Right.Value);
            Assert.IsNull(tree.Root.Right.Left.Right.Left);
            Assert.IsNull(tree.Root.Right.Left.Right.Right);
            Assert.AreEqual<int>(49, tree.Root.Right.Right.Left.Left.Value);
            Assert.IsNull(tree.Root.Right.Right.Left.Right);
            Assert.AreEqual<int>(58, tree.Root.Right.Right.Right.Left.Value);
            Assert.IsNull(tree.Root.Right.Right.Right.Right);
            Assert.IsNull(tree.Root.Right.Left.Left.Left.Left);
            Assert.IsNull(tree.Root.Right.Left.Left.Left.Right);
            Assert.IsNull(tree.Root.Right.Left.Left.Right.Left);
            Assert.IsNull(tree.Root.Right.Left.Left.Right.Right);
            Assert.IsNull(tree.Root.Right.Right.Left.Left.Left);
            Assert.IsNull(tree.Root.Right.Right.Left.Left.Right);
            Assert.IsNull(tree.Root.Right.Right.Right.Left.Left);
            Assert.IsNull(tree.Root.Right.Right.Right.Left.Right);

            tree.Insert(2);
            Assert.AreEqual<int>(2, tree.Root.Left.Left.Left.Value);

            tree.Delete(19);

            Assert.AreEqual<int>(21, tree.Root.Value);
            Assert.AreEqual<int>(10, tree.Root.Left.Value);
            Assert.AreEqual<int>(4, tree.Root.Left.Left.Value);
            Assert.AreEqual<int>(14, tree.Root.Left.Right.Value);
            Assert.AreEqual<int>(2, tree.Root.Left.Left.Left.Value);
            Assert.IsNull(tree.Root.Left.Left.Left.Left);
            Assert.IsNull(tree.Root.Left.Left.Left.Right);
            Assert.AreEqual<int>(7, tree.Root.Left.Left.Right.Value);
            Assert.IsNull(tree.Root.Left.Left.Right.Left);
            Assert.IsNull(tree.Root.Left.Left.Right.Right);
            Assert.AreEqual<int>(12, tree.Root.Left.Right.Left.Value);
            Assert.AreEqual<int>(18, tree.Root.Left.Right.Right.Value);
            Assert.IsNull(tree.Root.Left.Right.Left.Left);
            Assert.IsNull(tree.Root.Left.Right.Left.Right);
            Assert.IsNull(tree.Root.Left.Right.Right.Left);
            Assert.IsNull(tree.Root.Left.Right.Right.Right);

            Assert.AreEqual<int>(46, tree.Root.Right.Value);
            Assert.AreEqual<int>(37, tree.Root.Right.Left.Value);
            Assert.AreEqual<int>(55, tree.Root.Right.Right.Value);
            Assert.AreEqual<int>(28, tree.Root.Right.Left.Left.Value);
            Assert.AreEqual<int>(40, tree.Root.Right.Left.Right.Value);
            Assert.AreEqual<int>(51, tree.Root.Right.Right.Left.Value);
            Assert.AreEqual<int>(61, tree.Root.Right.Right.Right.Value);
            Assert.IsNull(tree.Root.Right.Left.Left.Left);
            Assert.AreEqual<int>(32, tree.Root.Right.Left.Left.Right.Value);
            Assert.IsNull(tree.Root.Right.Left.Right.Left);
            Assert.IsNull(tree.Root.Right.Left.Right.Right);
            Assert.AreEqual<int>(49, tree.Root.Right.Right.Left.Left.Value);
            Assert.IsNull(tree.Root.Right.Right.Left.Right);
            Assert.AreEqual<int>(58, tree.Root.Right.Right.Right.Left.Value);
            Assert.IsNull(tree.Root.Right.Right.Right.Right);
            Assert.IsNull(tree.Root.Right.Left.Left.Left);
            Assert.IsNull(tree.Root.Right.Left.Left.Right.Left);
            Assert.IsNull(tree.Root.Right.Left.Left.Right.Right);
            Assert.IsNull(tree.Root.Right.Right.Left.Left.Left);
            Assert.IsNull(tree.Root.Right.Right.Left.Left.Right);
            Assert.IsNull(tree.Root.Right.Right.Right.Left.Left);
            Assert.IsNull(tree.Root.Right.Right.Right.Left.Right);
        }

        [TestMethod]
        public void Insert1()
        {
            AVLTree tree = new AVLTree();
            Node n;

            Assert.IsNull(tree.Root);

            tree.Insert(20);
            n = tree.Root;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            tree.Insert(45);
            n = tree.Root.Right;
            Assert.AreEqual<int>(45, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            tree.Insert(57);
            n = tree.Root;
            Assert.AreEqual<int>(45, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            n = tree.Root.Left;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            n = tree.Root.Right;
            Assert.AreEqual<int>(57, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            tree.Insert(5);
            n = tree.Root.Left.Left;
            Assert.AreEqual<int>(5, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            tree.Insert(12);
            n = tree.Root;
            Assert.AreEqual<int>(45, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            n = tree.Root.Left;
            Assert.AreEqual<int>(12, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            n = tree.Root.Right;
            Assert.AreEqual<int>(57, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            n = tree.Root.Left.Left;
            Assert.AreEqual<int>(5, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            n = tree.Root.Left.Right;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
        }

        [TestMethod]
        public void Insert2()
        {
            AVLTree tree = new AVLTree();
            Node n;

            Assert.IsNull(tree.Root);

            tree.Insert(1);
            n = tree.Root;
            Assert.AreEqual<int>(1, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);

            tree.Insert(2);
            Assert.AreEqual<int>(-1, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Right;
            Assert.AreEqual<int>(2, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);

            tree.Insert(3);
            n = tree.Root;
            Assert.AreEqual<int>(2, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Left;
            Assert.AreEqual<int>(1, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);
            n = tree.Root.Right;
            Assert.AreEqual<int>(3, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);

            tree.Insert(4);
            n = tree.Root;
            Assert.AreEqual<int>(2, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(-1, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Right;
            Assert.AreEqual<int>(3, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(-1, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Right.Right;
            Assert.AreEqual<int>(4, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);

            tree.Insert(5);
            n = tree.Root.Right;
            Assert.AreEqual<int>(4, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Right.Left;
            Assert.AreEqual<int>(3, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);
            n = tree.Root.Right.Right;
            Assert.AreEqual<int>(5, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);

            tree.Insert(6);
            n = tree.Root;
            Assert.AreEqual<int>(4, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Left;
            Assert.AreEqual<int>(2, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Right;
            Assert.AreEqual<int>(5, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(-1, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Left.Left;
            Assert.AreEqual<int>(1, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);
            n = tree.Root.Left.Right;
            Assert.AreEqual<int>(3, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);
            n = tree.Root.Right.Right;
            Assert.AreEqual<int>(6, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);

            tree.Insert(7);
            n = tree.Root;
            Assert.AreEqual<int>(4, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Left;
            Assert.AreEqual<int>(2, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Right;
            Assert.AreEqual<int>(6, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsFalse(n.IsLeaf);
            n = tree.Root.Left.Left;
            Assert.AreEqual<int>(1, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);
            n = tree.Root.Left.Right;
            Assert.AreEqual<int>(3, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);
            n = tree.Root.Right.Left;
            Assert.AreEqual<int>(5, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);
            n = tree.Root.Right.Right;
            Assert.AreEqual<int>(7, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            Assert.IsTrue(n.IsLeaf);
        }

        [TestMethod]
        public void Insert3()
        {
            AVLTree tree = new AVLTree();
            Node n;

            Assert.IsNull(tree.Root);

            tree.Insert(20);
            n = tree.Root;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);

            tree.Insert(17);
            n = tree.Root;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(1, n.BalanceFactor);
            n = tree.Root.Left;
            Assert.AreEqual<int>(17, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);

            tree.Insert(14);
            n = tree.Root;
            Assert.AreEqual<int>(17, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Left;
            Assert.AreEqual<int>(14, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Right;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);

            tree.Insert(11);
            n = tree.Root;
            Assert.AreEqual<int>(17, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(1, n.BalanceFactor);
            n = tree.Root.Left;
            Assert.AreEqual<int>(14, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(1, n.BalanceFactor);
            n = tree.Root.Right;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Left.Left;
            Assert.AreEqual<int>(11, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);

            tree.Insert(8);
            n = tree.Root;
            Assert.AreEqual<int>(17, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(1, n.BalanceFactor);
            n = tree.Root.Left;
            Assert.AreEqual<int>(11, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Right;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Left.Left;
            Assert.AreEqual<int>(8, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Left.Right;
            Assert.AreEqual<int>(14, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);

            tree.Insert(5);
            n = tree.Root;
            Assert.AreEqual<int>(11, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Left;
            Assert.AreEqual<int>(8, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(1, n.BalanceFactor);
            n = tree.Root.Right;
            Assert.AreEqual<int>(17, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Left.Left;
            Assert.AreEqual<int>(5, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Right.Left;
            Assert.AreEqual<int>(14, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Right.Right;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);

            tree.Insert(2);
            n = tree.Root;
            Assert.AreEqual<int>(11, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Left;
            Assert.AreEqual<int>(5, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Right;
            Assert.AreEqual<int>(17, n.Value);
            Assert.IsNotNull(n.Left);
            Assert.IsNotNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Left.Left;
            Assert.AreEqual<int>(2, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            n = tree.Root.Left.Right;
            Assert.AreEqual<int>(8, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Right.Left;
            Assert.AreEqual<int>(14, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
            n = tree.Root.Right.Right;
            Assert.AreEqual<int>(20, n.Value);
            Assert.IsNull(n.Left);
            Assert.IsNull(n.Right);
            Assert.AreEqual<int>(0, n.BalanceFactor);
        }

        [TestMethod]
        public void Find()
        {
            int[] array = new int[] { 7, 2, 19, 43, 24, 76, 4, 17 };

            AVLTree tree = new AVLTree();

            foreach (int item in array)
            {
                tree.Insert(item);
            }

            Node n = tree.Root.Right.Right;
            Assert.AreSame(n, tree.Find(76));

            n = tree.Root.Left.Left;
            Assert.AreSame(n, tree.Find(2));

            n = tree.Root.Left.Left.Right;
            Assert.AreSame(n, tree.Find(4));

            n = tree.Root.Left.Right.Left;
            Assert.AreSame(n, tree.Find(17));

            n = tree.Root.Left.Right;
            Assert.AreSame(n, tree.Find(19));

            n = tree.Root.Left;
            Assert.AreSame(n, tree.Find(7));

            n = tree.Root.Right;
            Assert.AreSame(n, tree.Find(43));

            n = tree.Root;
            Assert.AreSame(n, tree.Find(24));

            n = tree.Root.Right.Left;
            Assert.IsNull(n);
        }
    }
}
