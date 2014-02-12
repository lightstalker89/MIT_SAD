using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using Algodat_UE4;

namespace BinaryTreeTest
{
    [TestClass]
    public class UnitTest1
    {
        private int[] numbers;
        private BinaryTree binaryTree { get; set; }

        public UnitTest1()
        {
            Random r = new Random();
            this.numbers = new int[10000];

            for (int i = 0; i < this.numbers.Length; i++)
            {
                this.numbers[i] = r.Next(100000);
            }

            this.binaryTree = new BinaryTree();
        }

        [TestMethod]
        public void BinaryInsert()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            this.binaryTree.CreateTree(numbers);

            stopwatch.Stop();

            Console.WriteLine("Time needed for building and inserting: " + stopwatch.ElapsedMilliseconds + "ms - Ticks: " + stopwatch.ElapsedTicks);
            Console.WriteLine();
            //this.binaryTree.Print();
            stopwatch.Reset();

            foreach (int number in this.numbers)
            {
                bool result = this.binaryTree.FindNode(this.binaryTree.RootNode, number);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void BinarySearchUnsortedTest()
        {
            int compareCount = 0;
            int[] searchNumbers = new int[100];
            Random rnd = new Random();

            for (int i = 0; i < searchNumbers.Length; i++)
            {
                searchNumbers[i] = rnd.Next(100000);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (int number in searchNumbers)
            {
                if (this.binaryTree.Find(number))
                {
                    Console.WriteLine("Number found: {0}", number);
                }
                else
                {
                    compareCount++;
                }

            }

            stopwatch.Stop();

            Console.WriteLine("Time needed for searching numbers: " + stopwatch.ElapsedMilliseconds + "ms - Ticks: " + stopwatch.ElapsedTicks);
            Console.WriteLine();
            stopwatch.Reset();
            Console.WriteLine("Binary CompareCount: {0}", compareCount);
        }

        [TestMethod]
        public void BinarySearchSortedTest()
        {
            int compareCount = 0;
            int[] searchNumbers = new int[100];
            Random rnd = new Random();

            for (int i = 0; i < searchNumbers.Length; i++)
            {
                searchNumbers[i] = rnd.Next(1000);
            }

            Array.Sort(searchNumbers);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (int number in searchNumbers)
            {
                if (this.binaryTree.FindNode(this.binaryTree.RootNode, number))
                {
                    Console.WriteLine("Number found: {0}", number);
                }
                else
                {
                    compareCount++;
                }

            }

            stopwatch.Stop();

            Console.WriteLine("Time needed for searching numbers: " + stopwatch.ElapsedMilliseconds + "ms - Ticks: " + stopwatch.ElapsedTicks);
            Console.WriteLine();
            stopwatch.Reset();
            Console.WriteLine("Binary CompareCount: {0}", compareCount);
        }
    }
}
