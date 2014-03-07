using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Algodat_UE4;

namespace AVLTreeTest
{
    [TestClass]
    public class UnitTest1
    {
        private const int numberCount = 10000;

        private int[] numbers = new int[numberCount];
        private AVLTree avlTree { get; set; }

        public UnitTest1() 
        {
            for (int i = 0; i < numberCount; i++)
            {
                this.numbers[i] = i + 1;
            }
            this.avlTree = new AVLTree();
        }

        [TestMethod]
        public void AVLInsertTest()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            this.avlTree.CreateAVLTree(numbers);

            stopwatch.Stop();

            Console.WriteLine("Time needed for building and inserting: " + stopwatch.ElapsedMilliseconds + "ms - Ticks: " + stopwatch.ElapsedTicks);
            Console.WriteLine();
            //this.avlTree.Print();
            stopwatch.Reset();

            foreach (int number in this.numbers)
            {
               bool result = this.avlTree.FindNode(this.avlTree.RootNode, number);
                Assert.IsTrue(result);   
            }
        }

        [TestMethod]
        public void AVLSearchUnsortedTest()
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
                if (this.avlTree.FindNode(this.avlTree.RootNode, number))
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
            Console.WriteLine("AVL CompareCount: {0}", compareCount);
        }

        [TestMethod]
        public void AVLSearchSortedTest()
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
                if (this.avlTree.Find(number))
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
            Console.WriteLine("AVL CompareCount: {0}", compareCount);
        }
    }
}
