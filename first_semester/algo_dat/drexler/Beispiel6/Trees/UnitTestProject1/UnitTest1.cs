using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ListRandomNumbers;
using Trees;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private Comparer comp;
        private Tree avlTree;
        private Tree binaryTree;
        private CArray array;
        private readonly Stopwatch stopWatch = new Stopwatch();

        [TestInitialize]
        public void Init()
        {
            this.comp = new Comparer();
            this.avlTree = new AVLTree(this.comp);
            this.binaryTree = new BinaryTree(this.comp);
        }

        [TestMethod]
        public void InsertSortedArray()
        {
            this.array = new CArray(100, 300);
            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int item in this.array.SortedArray)
            {
                this.avlTree.Insert(item, item);
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for building the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);
            this.avlTree.Output(OutputType.InOrder, avlTree.Root);

            this.stopWatch.Reset();
            foreach (var item in this.array.SortedArray)
            {
                int foundValue = 0;
                Assert.IsTrue(this.avlTree.Search(item, out foundValue));
            }
        }

        [TestMethod]
        public void InsertUnsortedArray()
        {
            this.array = new CArray(100000, 130000);
            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int item in this.array.UnsortedArray)
            {
                this.avlTree.Insert(item, item);
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for building the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);
            this.avlTree.Output(OutputType.InOrder, avlTree.Root);

            this.stopWatch.Reset();

            foreach (var item in this.array.UnsortedArray)
            {
                int foundValue = 0;
                Assert.IsTrue(this.avlTree.Search(item, out foundValue));
            }
        }

        /// <summary>
        /// Search unsorted random numbers in the AVL tree
        /// </summary>
        [TestMethod]
        public void AVLSearchUnsortedArray()
        {
            this.array = new CArray(100000, 120000);
            this.array.CompareCount = 0;
            int[] randomSearchValue = new int[100];

            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int item in this.array.UnsortedArray)
            {
                this.avlTree.Insert(item, item);
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for building the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                randomSearchValue[i] = random.Next(120000);
            }

            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int item in randomSearchValue)
            {
                int foundValue = 0;
                if (this.avlTree.Search(item, out foundValue))
                {
                    Debug.WriteLine("Found value: {0}", foundValue);
                }
                else
                {
                    ++this.array.CompareCount;
                }
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for searching 100 unsorted items in the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);
            Debug.WriteLine("Comparing count: {0}", this.array.CompareCount);
        }

        /// <summary>
        /// Search sorted random numbers in the AVL tree
        /// </summary>
        [TestMethod]
        public void AVLSearchSortedArray()
        {
            this.array = new CArray(100000, 120000);
            this.array.CompareCount = 0;
            int[] randomSearchValue = new int[100];

            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int item in this.array.UnsortedArray)
            {
                this.avlTree.Insert(item, item);
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for building the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                randomSearchValue[i] = random.Next(120000);
            }

            Array.Sort(randomSearchValue);

            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int i in randomSearchValue)
            {
                int foundValue = 0;
                if (this.avlTree.Search(i, out foundValue))
                {
                    Debug.WriteLine("Found value: {0}", foundValue);
                }
                else
                {
                    ++this.array.CompareCount;
                }
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for searching 100 sorted items in the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);
            Debug.WriteLine("Comparing count: {0}", this.array.CompareCount);
        }

        /// <summary>
        /// Search sorted random numbers in the Binary tree
        /// </summary>
        [TestMethod]
        public void BinarySearchSortedArray()
        {
            this.array = new CArray(100000, 120000);
            this.array.CompareCount = 0;
            int[] randomSearchValue = new int[100];

            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int item in this.array.UnsortedArray)
            {
                this.binaryTree.Insert(item, item);
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for building the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                randomSearchValue[i] = random.Next(120000);
            }

            Array.Sort(randomSearchValue);

            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int i in randomSearchValue)
            {
                int foundValue = 0;
                if (this.binaryTree.Search(i, out foundValue))
                {
                    Debug.WriteLine("Found value: {0}", foundValue);
                }
                else
                {
                    ++this.array.CompareCount;
                }
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for searching 100 sorted items in the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);
            Debug.WriteLine("Comparing count: {0}", this.array.CompareCount);
        }

        /// <summary>
        /// Search unsorted random numbers in the Binary tree
        /// </summary>
        [TestMethod]
        public void BinarySearchUnsortedArray()
        {
            this.array = new CArray(100000, 120000);
            this.array.CompareCount = 0;
            int[] randomSearchValue = new int[100];

            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int item in this.array.UnsortedArray)
            {
                this.binaryTree.Insert(item, item);
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for building the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                randomSearchValue[i] = random.Next(120000);
            }

            this.stopWatch.Reset();
            this.stopWatch.Start();
            foreach (int item in randomSearchValue)
            {
                int foundValue = 0;
                if (this.binaryTree.Search(item, out foundValue))
                {
                    Debug.WriteLine("Found value: {0}", foundValue);
                }
                else
                {
                    ++this.array.CompareCount;
                }
            }

            this.stopWatch.Stop();
            Debug.WriteLine("Time for searching 100 unsorted items in the tree: {0}ms", this.stopWatch.ElapsedMilliseconds);
            Debug.WriteLine("Comparing count: {0}", this.array.CompareCount);
        }
    }
}
