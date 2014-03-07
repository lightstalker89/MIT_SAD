using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListRandomNumbers;
using Timing;

namespace Trees
{
    class Program
    {

        private static readonly Stopwatch Stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            Console.SetBufferSize(555, 555);
            Console.SetWindowSize(150, 50);
            CArray array = new CArray(20, 24);

            Comparer comp = new Comparer();
            Tree avl = new AVLTree(comp);

            Console.WriteLine("--------------------------------AVL Tree -------------------------------");
            Console.WriteLine("Output unsorted array of numbers: ");
            Stopwatch.Start();
            foreach (int item in array.UnsortedArray)
            {
                avl.Insert(item, item);
                Console.Write(item.ToString() + " ");
            }

            Stopwatch.Stop();
            Console.WriteLine();
            Console.WriteLine("Result time for building avl-tree: {0}ms", Stopwatch.ElapsedMilliseconds);

            // **** Output avl-tree ****

            Console.WriteLine("Output AVL-Tree InOrder: ");
            avl.Output(OutputType.InOrder, avl.Root);

            Console.WriteLine("Output AVL-Tree PreOrder: ");
            avl.Output(OutputType.PreOrder, avl.Root);

            Console.WriteLine("Output AVL-Tree PostOrder: ");
            avl.Output(OutputType.PostOrder, avl.Root);

            Console.WriteLine("###---------AVL Search -----------###");
            Console.WriteLine("Search for number {0}:", array.UnsortedArray[6]);
            int value;
            bool isAvailable = avl.Search(array.UnsortedArray[6], out value);
            Console.WriteLine("{0} result from search {1}", isAvailable.ToString(), value.ToString());
            Console.WriteLine();

            Console.WriteLine("###---------AVL Delete -----------###");
            Console.WriteLine("Delete the number {0}", array.UnsortedArray[9]);
            bool isDeleted = avl.Delete(array.UnsortedArray[9]);
            if (isDeleted)
            {
                Console.WriteLine("Output AVL-Tree InOrder:");
                avl.Output(OutputType.InOrder, avl.Root);
            }
            

            Console.ReadKey();
            Console.WriteLine("--------------------------------Binary Tree -------------------------------");
            // ****Binary tree ****
            Tree binary = new BinaryTree(comp);

            Stopwatch.Reset();
            Stopwatch.Start();
            foreach (var item in array.UnsortedArray)
            {
                binary.Insert(item, item);
            }

            Stopwatch.Stop();
            Console.WriteLine();
            Console.WriteLine("Result time for building binary-tree: {0}ms", Stopwatch.ElapsedMilliseconds);
            Stopwatch.Reset();

            Console.WriteLine("Output Binary-Tree InOrder: ");
            binary.Output(OutputType.InOrder, binary.Root);

            Console.WriteLine("Output Binary-Tree PreOrder: ");
            binary.Output(OutputType.PreOrder, binary.Root);

            Console.WriteLine("Output Binary-Tree PostOrder: ");
            binary.Output(OutputType.PostOrder, binary.Root);

            Console.WriteLine("###---------Binary Search -----------###");
            Console.WriteLine("Search for number {0}:", array.UnsortedArray[6]);
            int bvalue;
            bool isAvail = avl.Search(array.UnsortedArray[6], out bvalue);
            Console.WriteLine("{0} result from search {1}", isAvail.ToString(), bvalue.ToString());
            Console.WriteLine();

            Console.WriteLine("###---------Binary Delete -----------###");
            Console.WriteLine("Delete the number {0}", array.UnsortedArray[9]);
            bool isDel = binary.Delete(array.UnsortedArray[9]);
            if (isDel)
            {
                Console.WriteLine("Output AVL-Tree InOrder: {0}", isDel.ToString());
                binary.Output(OutputType.InOrder, binary.Root);
            }

            Console.ReadKey();
        }
    }
}
