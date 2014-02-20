using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using Algodat_UE4;

namespace BinaryTreeTest
{
    [TestClass]
    public class UnitTest1
    {
        private const int numberCount = 10000;

        private int[] numbers = new int[numberCount];
        private BinaryTree binaryTree { get; set; }

        public UnitTest1()
        {
            for (int i = 0; i < numberCount; i++)
            {
                this.numbers[i] = i + 1;
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
                searchNumbers[i] = rnd.Next(numberCount);
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
                searchNumbers[i] = rnd.Next(numberCount);
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
